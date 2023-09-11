using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzoCreador;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS
{
    public class ManejadorRefuerzoTipoBorde
    {
        private readonly UIApplication _uiapp;
        private readonly DatosRefuerzoTipoBorde _datosRefuerzoTipoBorde;
        private Document _doc;
        private View _view;

        public ManejadorRefuerzoTipoBorde(UIApplication uiapp, DatosRefuerzoTipoBorde datosRefuerzoTipoBorde)
        {
            this._uiapp = uiapp;
            this._datosRefuerzoTipoBorde = datosRefuerzoTipoBorde;
            if (datosRefuerzoTipoBorde.CantidadBarras == 1)
                _datosRefuerzoTipoBorde.IsEstribo = false;
        }
        public Result Ejecutar()
        {

            _doc = _uiapp.ActiveUIDocument.Document;
            _view = _uiapp.ActiveUIDocument.Document.ActiveView;

            if (!Directory.Exists(ConstNH.CONST_COT)) return Result.Failed;
            View3D elem3d = _view as View3D;
            if (null != elem3d)
            {
                TaskDialog.Show("Error", "Comando no se puede ejectur en 3D");
                return Result.Failed;
            }

            View3D elem3d_parabusacr = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d_parabusacr == null)
            {
                Util.ErrorMsg("Error 3D, Favor cargar configuracion inicial del 3D");
                return Result.Failed;
            }

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);


            try
            {
                SeleccionarBordeLosa seleccionarBordeLosa = new SeleccionarBordeLosa(_uiapp);
                if (!seleccionarBordeLosa.SeleccionarEjecutar())
                {
                    Util.ErrorMsg("Erro al seleccionar borde de losa");
                    return Result.Failed;
                }
                if (seleccionarBordeLosa == null) return Result.Failed;
                if (seleccionarBordeLosa._FloorSeleccion == null) return Result.Failed;


                SeleccionarLosaConMouse_Intervalos seleccionarLosaConMouse = new SeleccionarLosaConMouse_Intervalos(_uiapp);
                if (seleccionarLosaConMouse.M1_SelecconarFloor() == null) return Result.Failed;
                if (!seleccionarLosaConMouse.M3_ObtenerBordeLosa(seleccionarBordeLosa.PtoSeleccionBordeLosa)) return Result.Failed;

                if (_datosRefuerzoTipoBorde.TipoSeleccionPtos == TipoSeleccionPtosBordeLosa.Selec2Puntos &&
                   !seleccionarLosaConMouse.M4_SeleccionarDosPtos()) return Result.Failed;

                List<DatosBordeLosaDTO> ListaDatosBordeLosaDTO = seleccionarLosaConMouse.M5_ObtenerListaDatosBordeLosaDTO(_datosRefuerzoTipoBorde);

                for (int i = 0; i < ListaDatosBordeLosaDTO.Count ; i++)
                {

                    var item = ListaDatosBordeLosaDTO[i];

                    ReCalcularfactorLargoDeProlongacion_ini_fin(i, ListaDatosBordeLosaDTO.Count);

                    CalculoGeometriaRectanguloBordeLosa CalculoGeometriaRectanguloBordeLosa = new CalculoGeometriaRectanguloBordeLosa(_uiapp, seleccionarBordeLosa, item);
                    CalculoGeometriaRectanguloBordeLosa.Ejecutar();


                    //obtienen los bordes de muro falso
                    SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo1 = CalculoGeometriaRectanguloBordeLosa.ObtenerMuroMirror1();
                    SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo2 = CalculoGeometriaRectanguloBordeLosa.ObtenerMuroMirror2();



                    CalculosRefuerzoTipoBorde calculosRefuerzoTipoBorde;
                    if (UtilRefuerzo.ISOrdena2PtosBordeMuro(helperSeleccinarMuroRefuerzo1.pto1SeleccionadoConMouse, helperSeleccinarMuroRefuerzo2.pto1SeleccionadoConMouse))
                        calculosRefuerzoTipoBorde = new CalculosRefuerzoTipoBorde(_uiapp, elem3d_parabusacr, seleccionarLosaConMouse, helperSeleccinarMuroRefuerzo2, helperSeleccinarMuroRefuerzo1, _datosRefuerzoTipoBorde);
                    else
                        calculosRefuerzoTipoBorde = new CalculosRefuerzoTipoBorde(_uiapp, elem3d_parabusacr, seleccionarLosaConMouse, helperSeleccinarMuroRefuerzo1, helperSeleccinarMuroRefuerzo2, _datosRefuerzoTipoBorde);
                    calculosRefuerzoTipoBorde.Ejecutar();



                    IGeometriaTag _newGeometriaTagEstribo = calculosRefuerzoTipoBorde.GenerarTagEstribo(_datosRefuerzoTipoBorde.IsEstribo);
                    if (_newGeometriaTagEstribo == null) return Result.Failed;
                    _newGeometriaTagEstribo.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(0) });


                    IGeometriaTag _newGeometriaTagRef = calculosRefuerzoTipoBorde.GenerarTagRefuerzo(ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL);
                    if (_newGeometriaTagRef == null) return Result.Failed;
                    _newGeometriaTagRef.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(0) });


                    using (TransactionGroup transGroup = new TransactionGroup(_doc))
                    {
                        transGroup.Start("RefuerzoTipoBorde-NH");

                        //dibujar 
                        CreadorRefuerzoTipoViga creadorRefuerzoTipoViga =
                            new CreadorRefuerzoTipoViga(_uiapp, calculosRefuerzoTipoBorde, _newGeometriaTagRef, _newGeometriaTagEstribo, seleccionarLosaConMouse.LosaSelecionado);
                        if (!creadorRefuerzoTipoViga.Ejecutar(TipoRebar.REFUERZO_BA_BORDE))
                        {
                            transGroup.RollBack();
                            return Result.Failed;
                        }
                        transGroup.Assimilate();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Failed;
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            return Result.Succeeded;
        }

        private void ReCalcularfactorLargoDeProlongacion_ini_fin(int i, int count)
        {
            if (count == 1)
            {
                return;
            }
            else
            {
                if (i == 0)
                {
                    _datosRefuerzoTipoBorde._empotramientoPatasDTO.factorLargoFin = 0.5;
                }
                else if (i == count - 1)
                {
                    _datosRefuerzoTipoBorde._empotramientoPatasDTO.factorLargoIni = 0.5;
                }
                else
                {
                    _datosRefuerzoTipoBorde._empotramientoPatasDTO.factorLargoIni = 0.5;
                    _datosRefuerzoTipoBorde._empotramientoPatasDTO.factorLargoFin = 0.5;
                }

            }

        }
    }
}
