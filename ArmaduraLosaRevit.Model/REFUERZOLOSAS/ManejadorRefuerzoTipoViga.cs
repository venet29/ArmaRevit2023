using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzoCreador;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Events;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar;
using System.IO;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS
{
    public class ManejadorRefuerzoTipoViga
    {
        private readonly UIApplication _uiapp;
        private readonly DatosRefuerzoTipoVigaDTO _datosRefuerzoTipoViga;
        private Document _doc;
        private View _view;
        private TipoRefuerzoLOSA _tipoRefuerzoLOSA;

        public ManejadorRefuerzoTipoViga(UIApplication uiapp, DatosRefuerzoTipoVigaDTO datosRefuerzoCabezaVigaDTO)
        {
            this._uiapp = uiapp;
            this._datosRefuerzoTipoViga = datosRefuerzoCabezaVigaDTO;
            this._tipoRefuerzoLOSA = datosRefuerzoCabezaVigaDTO._tipoRefuerzoLOSA;
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
                Util.ErrorMsg("Error #D, Favor cargar configuracion inicial del 3D");
                return Result.Failed;
            }

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);


            try
            {

                SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo1 = new SeleccinarMuroRefuerzo(_uiapp);
                if (!helperSeleccinarMuroRefuerzo1.EjecutarSeleccion()) return Result.Cancelled;

                SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo2 = new SeleccinarMuroRefuerzo(_uiapp);
                if (!helperSeleccinarMuroRefuerzo2.EjecutarSeleccion()) return Result.Cancelled;

                //obtaner
                if (!helperSeleccinarMuroRefuerzo1.GetBordeIntersectaConPto(helperSeleccinarMuroRefuerzo2.pto1SeleccionadoConMouse)) return Result.Failed;
                if (!helperSeleccinarMuroRefuerzo1.ObtenerPtoInterseccionSobreBorde())
                {
                    Util.ErrorMsg("Error al calcular geometria de enfierrado");
                    return Result.Cancelled;
                }

                if (!helperSeleccinarMuroRefuerzo2.GetBordeIntersectaConPto(helperSeleccinarMuroRefuerzo1.pto1SeleccionadoConMouse)) return Result.Failed;

                //calculos  intervalos
                CalculosRefuerzoTipoViga calculosRefuerzoTipoViga = new CalculosRefuerzoTipoViga(_uiapp, elem3d_parabusacr, helperSeleccinarMuroRefuerzo1, helperSeleccinarMuroRefuerzo2, _datosRefuerzoTipoViga);

                if (!calculosRefuerzoTipoViga.Ejecutar()) return Result.Failed;

                //tag
                IGeometriaTag _newGeometriaTagEstribo = null;
                if (_datosRefuerzoTipoViga.IsEstribo)
                {
                    _newGeometriaTagEstribo = calculosRefuerzoTipoViga.GenerarTagEstribo(_datosRefuerzoTipoViga.tipoPosicionRef);
                    _newGeometriaTagEstribo.Ejecutar(new GeomeTagArgs()
                    {
                        angulorad = Util.GradosToRadianes(90),
                        tipoPosicionEstribo = _datosRefuerzoTipoViga.tipoPosicionRef
                    });
                }
                IGeometriaTag _newGeometriaTagRef = calculosRefuerzoTipoViga.GenerarTagRefuerzo(_datosRefuerzoTipoViga.tipoPosicionRef);
                if (!_newGeometriaTagRef.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(90), tipoPosicionEstribo = _datosRefuerzoTipoViga.tipoPosicionRef }))
                    return Result.Failed;

                XYZ PtoBUscarLosa = (helperSeleccinarMuroRefuerzo1.pto1SeleccionadoConMouse + helperSeleccinarMuroRefuerzo2.pto1SeleccionadoConMouse) / 2;

                SeleccionarLosaOFunda _SeleccionarLosaOFunda = new SeleccionarLosaOFunda(_uiapp, elem3d_parabusacr, _tipoRefuerzoLOSA);
                if (!_SeleccionarLosaOFunda.SeleccionarElementoFLoor_Fundaction(PtoBUscarLosa)) return Result.Failed;
                Floor LosaSelecionado = _SeleccionarLosaOFunda.LosaSelecionado; // seleccionarLosaConPto.EjecturaSeleccionarLosaConPto(PtoBUscarLosa, lv);                

                //dibujar 
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("RefuerzoTipoViga-NH");
                    CreadorRefuerzoTipoViga creadorRefuerzoTipoViga = new CreadorRefuerzoTipoViga(_uiapp, calculosRefuerzoTipoViga, _newGeometriaTagRef, _newGeometriaTagEstribo, LosaSelecionado);
                    if (!creadorRefuerzoTipoViga.Ejecutar(TipoRebar.REFUERZO_BA_REF_LO))
                    {
                        transGroup.RollBack();
                        return Result.Failed;
                    }
                    transGroup.Assimilate();
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

    }

    public class ManejadorRefuerzoTipoVigaModelLIne
    {
        private readonly UIApplication _uiapp;
        private readonly DatosRefuerzoTipoVigaDTO _datosRefuerzoTipoViga;
        private Document _doc;
        private View _view;
        private TipoRefuerzoLOSA _tipoRefuerzoLOSA;

        public ManejadorRefuerzoTipoVigaModelLIne(UIApplication uiapp, DatosRefuerzoTipoVigaDTO datosRefuerzoCabezaVigaDTO)
        {
            this._uiapp = uiapp;
            this._datosRefuerzoTipoViga = datosRefuerzoCabezaVigaDTO;
            this._tipoRefuerzoLOSA = datosRefuerzoCabezaVigaDTO._tipoRefuerzoLOSA;
        }
        public Result Ejecutar()
        {

            _doc = _uiapp.ActiveUIDocument.Document;
            _view = _uiapp.ActiveUIDocument.Document.ActiveView;

            View3D elem3d = _view as View3D;
            if (null != elem3d)
            {
                TaskDialog.Show("Error", "Comando no se puede ejectur en 3D");
                return Result.Failed;
            }

            View3D elem3d_parabusacr = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d_parabusacr == null)
            {
                Util.ErrorMsg("Error #D, Favor cargar configuracion inicial del 3D");
                return Result.Failed;
            }

            try
            {
                CalculoGeometriaRectangulo2LineasViga calculoGeometriaRectangulo2LineasViga = new CalculoGeometriaRectangulo2LineasViga(_uiapp, elem3d_parabusacr, _tipoRefuerzoLOSA);
                if (!calculoGeometriaRectangulo2LineasViga.Ejecutar()) return Result.Cancelled;

                SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo1 = calculoGeometriaRectangulo2LineasViga.ObtenerMuroMirror1();
                SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo2 = calculoGeometriaRectangulo2LineasViga.ObtenerMuroMirror2();

                CalculosRefuerzoTipoViga calculosRefuerzoTipoViga;
                //hacer los calculos para 
                //if (UtilRefuerzo.ISOrdena2Ptos(helperSeleccinarMuroRefuerzo1.pto1SeleccionadoConMouse, helperSeleccinarMuroRefuerzo2.pto1SeleccionadoConMouse))
                //    calculosRefuerzoTipoViga = new CalculosRefuerzoTipoViga(_uiapp, elem3d_parabusacr, helperSeleccinarMuroRefuerzo1, helperSeleccinarMuroRefuerzo2, _datosRefuerzoTipoViga);
                //else
                //    calculosRefuerzoTipoViga = new CalculosRefuerzoTipoViga(_uiapp, elem3d_parabusacr, helperSeleccinarMuroRefuerzo2, helperSeleccinarMuroRefuerzo1, _datosRefuerzoTipoViga);
                calculosRefuerzoTipoViga = new CalculosRefuerzoTipoViga(_uiapp, elem3d_parabusacr, helperSeleccinarMuroRefuerzo1, helperSeleccinarMuroRefuerzo2, _datosRefuerzoTipoViga);

                if (!calculosRefuerzoTipoViga.Ejecutar()) return Result.Failed;

                //tag
                IGeometriaTag _newGeometriaTagEstribo = null;
                if (_datosRefuerzoTipoViga.IsEstribo)
                {
                    _newGeometriaTagEstribo = calculosRefuerzoTipoViga.GenerarTagEstribo(_datosRefuerzoTipoViga.tipoPosicionRef);
                    _newGeometriaTagEstribo.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(90) });
                }
                IGeometriaTag _newGeometriaTagRef = calculosRefuerzoTipoViga.GenerarTagRefuerzo(_datosRefuerzoTipoViga.tipoPosicionRef);
                _newGeometriaTagRef.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(90) });

                XYZ PtoBUscarLosa = (helperSeleccinarMuroRefuerzo1.pto1SeleccionadoConMouse + helperSeleccinarMuroRefuerzo2.pto1SeleccionadoConMouse) / 2;

                //OTRA ALTERNATIVA SELECIONAR LOSA CON PUNTO
                Floor LosaSelecionado = calculoGeometriaRectangulo2LineasViga._losaSelecionado;
                //Level lv = _view.GenLevel;

                //if (_tipoRefuerzoLOSA == TipoRefuerzoLOSA.fundacion)
                //{
                //    BuscarFundacionLosa _buscarElementosBajo = new BuscarFundacionLosa(_uiapp, Util.CmToFoot(300));
                //    if (!_buscarElementosBajo.OBtenerRefrenciaFundacion(elem3d_parabusacr, PtoBUscarLosa, new XYZ(0, 0, -1))) return Result.Failed;
                //    LosaSelecionado = (Floor)_buscarElementosBajo.FundLosaElementHost;
                //}
                //else
                //{
                //    SeleccionarLosaConPto seleccionarLosaConPto = new SeleccionarLosaConPto(_doc);
                //    //OTRA ALTERNATIVA SELECIONAR LOSA CON PUNTO                  
                //    LosaSelecionado = seleccionarLosaConPto.EjecturaSeleccionarLosaConPto(PtoBUscarLosa, lv);
                //}

                if (LosaSelecionado == null) return Result.Failed;

                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("RefuerzoTipoViga-NH");
                    //dibujar 
                    CreadorRefuerzoTipoViga creadorRefuerzoTipoViga = new CreadorRefuerzoTipoViga(_uiapp, calculosRefuerzoTipoViga, _newGeometriaTagRef, _newGeometriaTagEstribo, LosaSelecionado);
                    creadorRefuerzoTipoViga.Ejecutar(TipoRebar.REFUERZO_BA_REF_LO);
                    transGroup.Assimilate();
                }

            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                string message = ex.Message;
                //    _updateGeneral.CargarBArras();
                return Autodesk.Revit.UI.Result.Failed;
            }
            //    _updateGeneral.CargarBArras();
            return Result.Succeeded;

        }

    }


}
