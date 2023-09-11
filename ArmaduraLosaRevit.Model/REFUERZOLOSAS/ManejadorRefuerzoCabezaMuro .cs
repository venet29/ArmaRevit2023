using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzoCreador;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS
{
    public class ManejadorRefuerzoCabezaMuro
    {
        private readonly UIApplication _uiapp;
        private readonly DatosRefuerzoCabezaMuroDTO _datosRefuerzoCabezaMuroDTO;
        private Document _doc;
        private View _view;
        private TipoRefuerzoLOSA _tipoRefuerzoLOSA;

        public ManejadorRefuerzoCabezaMuro(UIApplication uiapp, DatosRefuerzoCabezaMuroDTO datosRefuerzoCabezaMuroDTO)
        {
            this._uiapp = uiapp;
            this._datosRefuerzoCabezaMuroDTO = datosRefuerzoCabezaMuroDTO;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            this._tipoRefuerzoLOSA = datosRefuerzoCabezaMuroDTO._tipoRefuerzoLOSA;
        }
        public Result Ejecutar()
        {

            View3D elem3d = _view as View3D;
            if (null != elem3d)
            {
                TaskDialog.Show("Error", "Comando no se puede ejectur en 3D");
                return Result.Failed;
            }

            View3D elem3d_parabuscar = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d_parabuscar == null)
            {
                Util.ErrorMsg("Error 3D, Favor cargar configuracion inicial del 3D");
                return Result.Failed;
            }

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            try
            {


                SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo1 = new SeleccinarMuroRefuerzo(_uiapp);
                if (!helperSeleccinarMuroRefuerzo1.EjecutarSeleccion()) return Result.Failed;
                if (helperSeleccinarMuroRefuerzo1 == null) return Result.Failed;

                SeleccionarLosaConMouse seleccionarLosaConMouse = new SeleccionarLosaConMouse(_uiapp);
                if (seleccionarLosaConMouse.M1_SelecconarFloor() == null) return Result.Failed;
                if (seleccionarLosaConMouse == null) return Result.Failed;

                //OBTENER BORDE INTERSECTADO
                if (!helperSeleccinarMuroRefuerzo1.GetBordeIntersectaConPto(seleccionarLosaConMouse._ptoSeleccionEnLosa)) return Result.Failed;


                SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo2 = new SeleccinarMuroRefuerzo(_uiapp);
                helperSeleccinarMuroRefuerzo2 = helperSeleccinarMuroRefuerzo1.ObtenerMuroMirror();

                //hacer los calculos para 
                CalculosRefuerzoCabezaMuro calculosRefuerzoCabezaMuro = new CalculosRefuerzoCabezaMuro(_uiapp, elem3d_parabuscar, helperSeleccinarMuroRefuerzo1, helperSeleccinarMuroRefuerzo2, _datosRefuerzoCabezaMuroDTO);
                if (!calculosRefuerzoCabezaMuro.Ejecutar()) return Result.Failed;

                IGeometriaTag _newGeometriaTagRef = calculosRefuerzoCabezaMuro.GenerarTagRefuerzo(0);
                _newGeometriaTagRef.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(0) });


                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("RefuerzoCabezaMuro-NH");

                    if (_datosRefuerzoCabezaMuroDTO.IsBArras)
                    {
                        //dibujar barras refuerzo
                        CreadorRefuerzoCabezaMuro _creadorRefuerzoCabezaMuro = new CreadorRefuerzoCabezaMuro(_uiapp, calculosRefuerzoCabezaMuro, _newGeometriaTagRef, seleccionarLosaConMouse.LosaSelecionado);

                        if (!_creadorRefuerzoCabezaMuro.Ejecutar())
                        {
                            transGroup.RollBack();
                            return Result.Failed;
                        }
                    }

                    if (_datosRefuerzoCabezaMuroDTO.IsSuple && _datosRefuerzoCabezaMuroDTO._tipoRefuerzoLOSA == TipoRefuerzoLOSA.losa)
                    {
                        //dibujar s1 refuerzo
                        S1Refuerzo s1Refuerzo = new S1Refuerzo(_uiapp, calculosRefuerzoCabezaMuro.listaPtos, _datosRefuerzoCabezaMuroDTO, seleccionarLosaConMouse);

                        if (!s1Refuerzo.Ejecutar())
                        {
                            transGroup.RollBack();
                            return Result.Failed;
                        }

                    }
                    transGroup.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Autodesk.Revit.UI.Result.Failed;
            }

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return Result.Succeeded;

        }


        //ejecuta segun sea la seleccion del mouse
        public Result EjecutarLibreConDosMOuse()
        {

            _doc = _uiapp.ActiveUIDocument.Document;
            _view = _uiapp.ActiveUIDocument.Document.ActiveView;



            View3D elem3d = _view as View3D;
            if (null != elem3d)
            {
                TaskDialog.Show("Error", "Comando no se puede ejectur en 3D");
                return Result.Failed;
            }

            View3D elem3d_parabuscar = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d_parabuscar == null)
            {
                Util.ErrorMsg("Error #D, Favor cargar configuracion inicial del 3D");
                return Result.Failed;
            }

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            try
            {
                if (!Directory.Exists(ConstNH.CONST_COT)) return Result.Failed;
                SeleccionarLosaConMouse seleccionarLosaConMouse1 = new SeleccionarLosaConMouse(_uiapp);
                if (seleccionarLosaConMouse1.M1_SelecconarFloor() == null) return Result.Failed;

                CalculoGeometriaRectangulo2ptos _CalculoGeometriaRectangulo2ptos = new CalculoGeometriaRectangulo2ptos(_uiapp,
                    seleccionarLosaConMouse1, _datosRefuerzoCabezaMuroDTO.diamtroBarraRefuerzo_MM, elem3d_parabuscar, _tipoRefuerzoLOSA);


                if (!_CalculoGeometriaRectangulo2ptos.Ejecutar()) return Result.Failed;


                //obtienen los bordes de muro falso
                SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo1 = _CalculoGeometriaRectangulo2ptos.ObtenerMuroMirror1();
                SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo2 = _CalculoGeometriaRectangulo2ptos.ObtenerMuroMirror2();



                //hacer los calculos para 
                CalculosRefuerzoCabezaMuroV2 calculosRefuerzoCabezaMuro = new CalculosRefuerzoCabezaMuroV2(_uiapp, elem3d_parabuscar, helperSeleccinarMuroRefuerzo1, helperSeleccinarMuroRefuerzo2, _datosRefuerzoCabezaMuroDTO);
                if (!calculosRefuerzoCabezaMuro.Ejecutar()) return Result.Failed;

                IGeometriaTag _newGeometriaTagRef = calculosRefuerzoCabezaMuro.GenerarTagRefuerzo(0);
                _newGeometriaTagRef.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(0) });


                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("RefuerzoCabezaMuro-NH");

                    if (_datosRefuerzoCabezaMuroDTO.IsBArras)
                    {
                        //dibujar barras refuerzo
                        CreadorRefuerzoCabezaMuro _creadorRefuerzoCabezaMuro = new CreadorRefuerzoCabezaMuro(_uiapp, calculosRefuerzoCabezaMuro, _newGeometriaTagRef, seleccionarLosaConMouse1.LosaSelecionado);
                        _creadorRefuerzoCabezaMuro.Ejecutar();
                    }

                    if (_datosRefuerzoCabezaMuroDTO.IsSuple)
                    {
                        //dibujar s1 refuerzo
                        S1Refuerzo s1Refuerzo = new S1Refuerzo(_uiapp, calculosRefuerzoCabezaMuro.listaPtos, _datosRefuerzoCabezaMuroDTO, seleccionarLosaConMouse1);
                        s1Refuerzo.Ejecutar2ptos();
                    }
                    transGroup.Assimilate();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                // If there are something wrong, give error information and return failed
                //message = ex.Message;
                //     _updateGeneral.CargarBArras();
                return Autodesk.Revit.UI.Result.Failed;
            }

            //_updateGeneral.CargarBArras();

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return Result.Succeeded;

        }

    }
}
