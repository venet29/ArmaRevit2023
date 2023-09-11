using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using System;
using ArmaduraLosaRevit.Model.EditarPath.Ayuda;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.EditarPath.Seleccion;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

namespace ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion
{
    public class EditarPathReinMouse_ExtederPathApunto
    {
        //  private ExternalCommandData commandData;
        private UIApplication _uiApplication;
        private UIDocument uidoc;
        private SeleccionPath _seleccionPath;
        private double distanciaAlargarPAth;

        public EditarPathReinMouse_ExtederPathApunto(UIApplication uiapp)
        {
            //  this.commandData = commandData;
            this._uiApplication = uiapp;
            this.uidoc = uiapp.ActiveUIDocument;
        }

        public Result M1_ExtederPathApunto(double distFoot = 0)
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiApplication);
                if (!_seleccionarPathReinfomentConPto.SeleccionarPathReinforment()) return Result.Cancelled;

                if (_seleccionarPathReinfomentConPto.PathReinforcement == null)
                {
                    Util.ErrorMsg("PathReinforment No seleccionado");
                    return Result.Failed;
                }

                _seleccionPath = new SeleccionPath(_uiApplication, _seleccionarPathReinfomentConPto);
                _seleccionPath.ObtenerBordeDepath();
                DireccionEdicionPathRein direccionEdicionPathRein = _seleccionPath.ObtenerPtoSeleccionConMouseEnPathrein();

                // Util.InfoMsg("Lado Seleccionado :" + direccionEdicionPathRein.ToString());
                //ObjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Centers | ObjectSnapTypes.Points;
                ObjectSnapTypes snapTypes = ObjectSnapTypes.Points | ObjectSnapTypes.Nearest;
                //selecciona un objeto floor
                XYZ _ptoSelecMouse;
                try
                {
                    _ptoSelecMouse = uidoc.Selection.PickPoint(snapTypes, "Element:Seleccionar punto ");
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    return Result.Cancelled;
                }

                ObtenerDistanciaAlargar(distFoot, _ptoSelecMouse);

       
                using (TransactionGroup transGroup = new TransactionGroup(uidoc.Document))
                {
                    transGroup.Start("ExtederPathApunto-NH");
                    EditarPathRein editarPathRein = new EditarPathRein(_uiApplication, _seleccionarPathReinfomentConPto);
                    editarPathRein.EditarPath(distanciaAlargarPAth, direccionEdicionPathRein);

                    transGroup.Assimilate();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error M1_ExtederPathApunto ex:{ex.Message} ");
                return Result.Failed;
            }

        }

        //resum= si valor es negativo se acorta el lado del path
        //       si es positivo el path se alarga
        private void ObtenerDistanciaAlargar(double distFoot, XYZ _ptoSelecMouse)
        {
            distanciaAlargarPAth = _seleccionPath.ObtenerDistanciaPerpendicularDesdePtoABorde(_ptoSelecMouse);
            DireccionEdicionPathRein _posicionPtoMouse = _seleccionPath.ObtenerPtoSeleccionConMouseEnPathrein(_ptoSelecMouse);

            XYZ DireccionDesdeCentroAlBorde = _seleccionPath.ObtenerDireccionDesdeCentroAlBorde(_seleccionPath._coordenadaPath.centro);
            XYZ DireccionBordePtoMouse = _seleccionPath.ObtenerDireccionBordeAlPtoMouse(_ptoSelecMouse);

            if (Util.GetProductoEscalar(DireccionDesdeCentroAlBorde.Normalize(), DireccionBordePtoMouse.Normalize()) > 0)
                distanciaAlargarPAth = distFoot + distanciaAlargarPAth * 1;
            else
                distanciaAlargarPAth = distFoot + distanciaAlargarPAth * -1;
        }

 
    }



}
