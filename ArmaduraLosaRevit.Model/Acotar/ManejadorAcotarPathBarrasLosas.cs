using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using System;
using ArmaduraLosaRevit.Model.EditarPath.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.TextoNoteNH;

namespace ArmaduraLosaRevit.Model.Acotar
{
    public  class ManejadorAcotarPathBarrasLosas
    {
        private readonly UIApplication _uiapp;
        private UIDocument _uidoc;
        private XYZ p2_acotar;
        private XYZ p3_texto;
        private SeleccionPath _seleccionPath;
        private double distanciaAlargarPAth;

        public ManejadorAcotarPathBarrasLosas(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            // sirefere3ncia es null salir
            this._uidoc = uiapp.ActiveUIDocument;
        }

        public Result P1_AcotarBordePathMouse()
        {
            try
            {

                //seleccionar borde path
                SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiapp);
                if (!_seleccionarPathReinfomentConPto.SeleccionarPathReinforment()) return Result.Cancelled;

                if (_seleccionarPathReinfomentConPto.PathReinforcement == null)
                {
                    Util.ErrorMsg("PathReinforment No seleccionado");
                    return Result.Failed;
                }

                _seleccionPath = new SeleccionPath(_uiapp, _seleccionarPathReinfomentConPto);
                _seleccionPath.ObtenerBordeDepath();
                DireccionEdicionPathRein direccionEdicionPathRein = _seleccionPath.ObtenerPtoSeleccionConMouseEnPathrein();


                if (!P1_1_SelecionarPtosDirectriz()) return Result.Cancelled;


                P1_2_ObtenerDistanciaAlargar(0, p2_acotar);

              //  double anguloBarra_ = Math.Round( Util.AnguloEntre2PtosGrado90(XYZ.Zero, _seleccionPath.direccionBarras, EnGrados: false),0);

                CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.AcotarBarra, TipoCOloresTexto.rojo);
                double distaCm = Math.Round(Util.FootToCm(distanciaAlargarPAth), 0);
                _CrearTexNote.M1_CrearConTrans(p3_texto + new XYZ(Util.CmToFoot(-10), Util.CmToFoot(+10),0) , $"[{distaCm}]", _seleccionPath._angulo23_rad);
                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {

                Util.ErrorMsg("Error al ejecutar EjecutarExtenderPath() : " + ex);
                return Result.Failed;
            }

        }

        private bool P1_1_SelecionarPtosDirectriz()
        {
            try
            {
                ObjectSnapTypes snapTypes = ObjectSnapTypes.Intersections | ObjectSnapTypes.Nearest;
                p2_acotar = _uidoc.Selection.PickPoint(snapTypes, "2) seleccionar distancia acotar");
                // sirefere3ncia es null salir
                if (p2_acotar == XYZ.Zero) return false;

            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                ObjectSnapTypes snapTypes = ObjectSnapTypes.Points; 
                p3_texto = _uidoc.Selection.PickPoint(snapTypes, "3) seleccionar pto ubucacion texto");
                // sirefere3ncia es null salir
                if (p3_texto == XYZ.Zero) return false;

            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        private void P1_2_ObtenerDistanciaAlargar(double distFoot, XYZ _ptoSelecMouse)
        {
            distanciaAlargarPAth = _seleccionPath.ObtenerDistanciaPerpendicularDesdePtoABorde(_ptoSelecMouse);
            DireccionEdicionPathRein _posicionPtoMouse = _seleccionPath.ObtenerPtoSeleccionConMouseEnPathrein(_ptoSelecMouse);

            XYZ DireccionDesdeCentroAlBorde = _seleccionPath.ObtenerDireccionDesdeCentroAlBorde(_seleccionPath._coordenadaPath.centro);
            XYZ DireccionBordePtoMouse = _seleccionPath.ObtenerDireccionBordeAlPtoMouse(_ptoSelecMouse);

            if (Util.GetProductoEscalar(DireccionDesdeCentroAlBorde.Normalize(), DireccionBordePtoMouse.Normalize()) > 0)
                distanciaAlargarPAth = distFoot + distanciaAlargarPAth * 1;
            else
                distanciaAlargarPAth = distFoot + distanciaAlargarPAth * -1;

            //para dejar positivo

            distanciaAlargarPAth = Math.Abs(distanciaAlargarPAth);
        }

    }
}
