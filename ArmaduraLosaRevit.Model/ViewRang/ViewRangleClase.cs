using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewRang
{
    public interface IViewRangleClase
    {
        void EditarParametroViewRange(double ValorTopClipPlane, double ValorCutPlane, double ValorBottomClipPlane, double ValorViewDepthPlane);
    }

    public class ViewRangleClase : IViewRangleClase
    {
        private Document _doc;
        private View _view;
        private double TopClipPlaneActual;
        private double CutPlaneActual;
        private double BottomClipPlaneActual;
        private double ViewDepthPlaneActual;

        double ValorTopClipPlaneNuevo;
        double ValorCutPlaneNuevo;
        double ValorBottomClipPlaneNuevo;
        double ValorViewDepthPlaneNuevo;
        private PlanViewRange _viewRange;

        public static IViewRangleClase CreatorNuevoViewRange(Document doc, View view)
        {
            if (!(view is ViewPlan))
                return new ViewRangleClaseNull();

            return new ViewRangleClase(doc, view);
        }
        private ViewRangleClase(Document doc, View view)
        {
            this._doc = doc;
            this._view = view;
            this._viewRange = (_view as ViewPlan).GetViewRange();
        }

        public void EditarParametroViewRange(double ValorTopClipPlane, double ValorCutPlane, double ValorBottomClipPlane, double ValorViewDepthPlane)
        {
            this.ValorTopClipPlaneNuevo = ValorTopClipPlane;
            this.ValorCutPlaneNuevo = ValorCutPlane;
            this.ValorBottomClipPlaneNuevo = ValorBottomClipPlane;
            this.ValorViewDepthPlaneNuevo = ValorViewDepthPlane;

            CambiarParametros();
        }

        private void CambiarParametros()
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("text-NH");

                    ObtenerParametrosActualesViewRange();

                    if (SivaloresIguealesSalir()) return;

                    AsignarNuevosValoresViewRange();

                   // var bottomXYZ = (_doc.GetElement(_viewRange.GetLevelId(PlanViewPlane.BottomClipPlane)) as Level).ProjectElevation + _viewRange.GetOffset(PlanViewPlane.BottomClipPlane);
                  //  var topXYZ = (_doc.GetElement(_viewRange.GetLevelId(PlanViewPlane.CutPlane)) as Level).ProjectElevation + _viewRange.GetOffset(PlanViewPlane.CutPlane);

                    AsignarNuevoViewRangeAvista();

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }


        private void ObtenerParametrosActualesViewRange( )
        {
            TopClipPlaneActual = _viewRange.GetOffset(PlanViewPlane.TopClipPlane);
            CutPlaneActual = _viewRange.GetOffset(PlanViewPlane.CutPlane);
            BottomClipPlaneActual = _viewRange.GetOffset(PlanViewPlane.BottomClipPlane);
            ViewDepthPlaneActual = _viewRange.GetOffset(PlanViewPlane.ViewDepthPlane);
        }
        private bool SivaloresIguealesSalir()
        {
            return ((Math.Abs(TopClipPlaneActual - ValorTopClipPlaneNuevo) < 0.1 &&
                    CutPlaneActual == ValorCutPlaneNuevo &&
                    BottomClipPlaneActual == ValorBottomClipPlaneNuevo &&
                    ViewDepthPlaneActual == ValorViewDepthPlaneNuevo)
                    ? true : false);
        }
        private void AsignarNuevosValoresViewRange( )
        {
            _viewRange.SetOffset(PlanViewPlane.TopClipPlane, ValorTopClipPlaneNuevo);
            _viewRange.SetOffset(PlanViewPlane.CutPlane, ValorCutPlaneNuevo);
            _viewRange.SetOffset(PlanViewPlane.BottomClipPlane, ValorBottomClipPlaneNuevo);
            _viewRange.SetOffset(PlanViewPlane.ViewDepthPlane, ValorViewDepthPlaneNuevo);
        }

        private void AsignarNuevoViewRangeAvista() =>  (_view as ViewPlan).SetViewRange(_viewRange);
      
    }
}
