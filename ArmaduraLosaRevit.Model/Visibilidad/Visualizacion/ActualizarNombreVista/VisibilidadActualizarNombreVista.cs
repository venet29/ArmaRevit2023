using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista
{
    public class VisibilidadActualizarNombreVista
    {
        private Document _doc;
        private UIApplication _uiapp;
        private string nombreActualView;

        public VisibilidadActualizarNombreVista(UIApplication _uiapp, string nombreActualView)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
            this.nombreActualView = nombreActualView;
        }



        public void ActualizarNombreVista(List<Element> ElementosIDs, View _view, string nombrePredefinido = "")
        {
            if (ElementosIDs == null) return;
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarNombreVista-NH");
                    DesactivarViewTample(_view);
                    _view.Name = nombreActualView;
                    //view.IsolateElementsTemporary(ElementosIDs);
                    for (int i = 0; i < ElementosIDs.Count; i++)
                    {
                        Element paraElem = ElementosIDs[i];
                        if (paraElem == null) continue;
                        if (_view != null && ParameterUtil.FindParaByName(paraElem, "NombreVista") != null)
                            ParameterUtil.SetParaInt(paraElem, "NombreVista", (nombrePredefinido == "" ? nombreActualView : nombrePredefinido));  //"nombre de vista"
                    }

                    trans2.Commit();
                    // uidoc.RefreshActiveView();
                } // fin trans 

                ActualizaParametroEnView(_view);
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }
        }

        private void DesactivarViewTample(View _view)
        {
            Parameter par = _view.GetParameter2("View Template");
            ElementId _vireetmeplateIUd = par.AsElementId();
            par.Set(new ElementId(-1));
        }




        public void ActualizaParametroEnViewForzado(View _view)
        {
            Parameter _para = _view.GetParameter2("View Template");
            if (_para == null) return;
            string _paraViewNombre = _para.AsString();

            if (_paraViewNombre == null)
            {
                ActualizaParametroEnView(_view);
                return;
            }

            //caso view tempplane en losa
            if (_view.ViewType == ViewType.FloorPlan && _paraViewNombre == ConstNH.NOMBRE_VIEW_TEMPLATE_LOSA)
            {
                ActualizaParametroEnView(_view);
                return;
            }
        }

        private void ActualizaParametroEnView(View _view)
        {
            using (Transaction trans2 = new Transaction(_doc))
            {
                trans2.Start("Actualizar_ViewNombre-NH");

                ParameterUtil.SetParaStringNH(_view, "ViewNombre", _view.ObtenerNombreIsDependencia());
                //    par.Set(_vireetmeplateIUd);
                trans2.Commit();
                // uidoc.RefreshActiveView();
            } // fin trans 

        }
    }
}
