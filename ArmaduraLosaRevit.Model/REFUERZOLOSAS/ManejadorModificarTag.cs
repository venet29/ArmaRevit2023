using ArmaduraLosaRevit.Model.BarraV.DTO;
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
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS
{

    public class ManejadorModificarTag
    {
        private readonly UIApplication _uiapp;
        private readonly int _cantidadBarras;
        private Document _doc;
        private View _view;

        public ManejadorModificarTag(UIApplication uiapp, int cantidadBarras)
        {
            this._uiapp = uiapp;
            this._cantidadBarras = cantidadBarras;
        }
        public Result EjecutarCambioContag()
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


         //   //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
        //    _updateGeneral.DesCargarBarras();
         

            try
            {

                SeleccionarRebarElemento _seleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
                if (!_seleccionarRebarElemento.GetSelecionarRebar()) return Result.Failed;



                CalculoBarraRefuerzoSOLOTag calculoBarraRefuerzoSOLOTag = new CalculoBarraRefuerzoSOLOTag(_uiapp, _seleccionarRebarElemento);
                calculoBarraRefuerzoSOLOTag.ObtenerTipo();

                IGeometriaTag _newGeometriaTagRef = calculoBarraRefuerzoSOLOTag.GenerarTagRefuerzo();
                _newGeometriaTagRef.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(90) });

                BarraRefuerzoBordeLibreSOLOTag barraRefuerzoBordeLibreSOLOTag = new BarraRefuerzoBordeLibreSOLOTag(_uiapp, _seleccionarRebarElemento, _newGeometriaTagRef, _cantidadBarras);

                var _configuracionTAgEstriboDTo = new ConfiguracionTAgBarraDTo()
                {
                    desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                    IsDIrectriz = false,
                    tagOrientation = TagOrientation.Horizontal,
                };


                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("ModificarTag-NH");

                    barraRefuerzoBordeLibreSOLOTag.Ejecutar(_configuracionTAgEstriboDTo);
                    transGroup.Assimilate();
                }

            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                string message = ex.Message;
          //      _updateGeneral.CargarBArras();
                return Autodesk.Revit.UI.Result.Failed;
            }
           // _updateGeneral.CargarBArras();
            return Result.Succeeded;

        }

        public Result EjecutarCambioSintag()
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


          //  //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
        //    _updateGeneral.DesCargarBarras();
           

            try
            {

                SeleccionarRebarElemento _seleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
                if (!_seleccionarRebarElemento.GetSelecionarRebar()) return Result.Failed;

                IGeometriaTag _newGeometriaTagRef = new GeomeTagNull();
             
                BarraRefuerzoBordeLibreSOLOTag barraRefuerzoBordeLibreSOLOTag = new BarraRefuerzoBordeLibreSOLOTag(_uiapp, _seleccionarRebarElemento, _newGeometriaTagRef, _cantidadBarras);
                barraRefuerzoBordeLibreSOLOTag.cambiarParametrosCantidadBarra();



            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                string message = ex.Message;
          //      _updateGeneral.CargarBArras();
                return Autodesk.Revit.UI.Result.Failed;
            }
          //  _updateGeneral.CargarBArras();
            return Result.Succeeded;

        }



    }


}
