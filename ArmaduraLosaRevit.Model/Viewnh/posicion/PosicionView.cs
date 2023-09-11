using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Visualizacion;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Viewnh.posicion
{
    class PosicionView
    {
        private UIApplication _uiapp;
        private Document _doc;
        public View View { get; set; }
        public PosicionView(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            View = _doc.ActiveView;
        }

      

        public void obtener()
        {

            View activeView = _doc.ActiveView;
            List<UIView> uiViewsWithActiveView = new List<UIView>();
            foreach (UIView uiv in _uiapp.ActiveUIDocument.GetOpenUIViews())
            {
                if (uiv.ViewId.IntegerValue == activeView.Id.IntegerValue)
                    uiViewsWithActiveView.Add(uiv);
            }

            UIView ActiveUIView = uiViewsWithActiveView.FirstOrDefault();
            if (uiViewsWithActiveView.Count > 1)
            {
      
            }


            if (ActiveUIView == null) return;


            Rectangle rect = ActiveUIView.GetWindowRectangle();
            IList<XYZ> corners = ActiveUIView.GetZoomCorners();
            XYZ p = corners[0];
            XYZ q = corners[1];

            string msg = string.Format("UIView Windows rectangle: ; " + "zoom corners: {1}-{2}; ", p, q);
            TaskDialog.Show("coordinates", msg);
        }

        public bool M1_Obtener3D(string NombreView)
        {
            try
            {
                if (NombreView == "") return false;
                View = TiposView.ObtenerTiposView(NombreView, _doc);
                if (View == null)
                {
                    Util.ErrorMsg($"Nombre de vista asignado ({NombreView}), no tiene view asociado(con igual nombre).");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al redefinir Ventana revit.  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public void M2_ActivarBounderyPAthReinf()
        {
            if (View.ViewType == ViewType.FloorPlan)
            {
                //2)mostara barras
                IVisibilidadView visibilidad = VisibilidadView.Creador_Visibilidad(View, BuiltInCategory.OST_PathReinBoundary, "Boundary");

                bool result = visibilidad.EstadoActualHide();
                if (visibilidad.EstadoActualHide())
                    visibilidad.CambiarVisibilityBuiltInCategory();
            }
        }
        public void M3_ObtenerViewBarras( int Idbarra)
        {
            try
            {

                View3D elem3d = View as View3D;
                if (null == elem3d)
                {
                    Util.ErrorMsg("Debe ejecutar comando en un View3D");
                    return ;
                }

                _uiapp.ActiveUIDocument.ActiveView = (View3D)View;

                var elemt = _doc.GetElement(new ElementId(Idbarra));
                var bdBox = elemt.get_BoundingBox(View);

                //  View activeView = _doc.ActiveView;
                List<UIView> uiViewsWithActiveView = new List<UIView>();
                foreach (UIView uiv in _uiapp.ActiveUIDocument.GetOpenUIViews())
                {
                    if (uiv.ViewId.IntegerValue == View.Id.IntegerValue)
                        uiViewsWithActiveView.Add(uiv);
                }

                if (uiViewsWithActiveView.Count >= 1)
                {
                    UIView ActiveUIView = uiViewsWithActiveView.FirstOrDefault();
                    ActiveUIView.ZoomAndCenterRectangle(bdBox.Min, bdBox.Max);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al redefinir Ventana revit.  ex:{ex.Message}");
            }
        }


        public void M3_ObtenerZOOMViewBarras(BoundingBoxXYZ bdBox)
        {
            try
            {

                View3D elem3d = View as View3D;
                if (null == elem3d)
                {
                    Util.ErrorMsg("Debe ejecutar comando en un View3D");
                    return;
                }

                _uiapp.ActiveUIDocument.ActiveView = (View3D)View;


                //  View activeView = _doc.ActiveView;
                List<UIView> uiViewsWithActiveView = new List<UIView>();
                foreach (UIView uiv in _uiapp.ActiveUIDocument.GetOpenUIViews())
                {
                    if (uiv.ViewId.IntegerValue == View.Id.IntegerValue)
                        uiViewsWithActiveView.Add(uiv);
                }

                if (uiViewsWithActiveView.Count >= 1)
                {
                    UIView ActiveUIView = uiViewsWithActiveView.FirstOrDefault();
                    ActiveUIView.ZoomAndCenterRectangle(bdBox.Min, bdBox.Max);

                    
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al redefinir Ventana revit.  ex:{ex.Message}");
            }
        }


    }
}
