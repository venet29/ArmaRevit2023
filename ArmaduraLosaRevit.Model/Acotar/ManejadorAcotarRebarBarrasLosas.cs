using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using System;
using ArmaduraLosaRevit.Model.EditarPath.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Acotar.calculos;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.TextoNoteNH;

namespace ArmaduraLosaRevit.Model.Acotar
{
    public  class ManejadorAcotarRebarBarrasLosas
    {
        private readonly UIApplication _uiapp;
        private UIDocument _uidoc;
        private View _actiview;
        private XYZ p2_acotar;
        private XYZ p3_texto;
#pragma warning disable CS0649 // Field 'ManejadorAcotarRebarBarrasLosas._seleccionPath' is never assigned to, and will always have its default value null
        private SeleccionPath _seleccionPath;
#pragma warning restore CS0649 // Field 'ManejadorAcotarRebarBarrasLosas._seleccionPath' is never assigned to, and will always have its default value null
        private double distanciaAlargarPAth;
        private SeleccionarRebarElemento _seleccionarRebarElemento;

        public ManejadorAcotarRebarBarrasLosas(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            // sirefere3ncia es null salir
            this._uidoc = uiapp.ActiveUIDocument;
            this._actiview = uiapp.ActiveUIDocument.ActiveView; 
        }

        public Result P1_AcotarBordePathMouse()
        {
            try
            {

                //seleccionar borde path
                _seleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _actiview);
                if (!_seleccionarRebarElemento.GetSelecionarRebar()) return Result.Cancelled;

                if (_seleccionarRebarElemento._RebarSeleccion == null)
                {
                    Util.ErrorMsg("Rebar No seleccionado");
                    return Result.Failed;
                }

             
                if (!P1_1_SelecionarPtosDirectriz()) return Result.Cancelled;


                P1_2_ObtenerDistanciaAlargar();

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

        private void P1_2_ObtenerDistanciaAlargar( )
        {

            CalcularDistanciaRebar _CalcularDistanciaRebar = new CalcularDistanciaRebar(p2_acotar, _seleccionarRebarElemento);
            _CalcularDistanciaRebar.Calculardistancia();
            //para dejar positivo

            distanciaAlargarPAth = Math.Abs(_CalcularDistanciaRebar.distanciaAlargarPAth);
        }

    }
}
