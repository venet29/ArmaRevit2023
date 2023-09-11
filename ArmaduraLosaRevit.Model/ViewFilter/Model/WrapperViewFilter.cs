using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ViewFilter.Model
{
    public class WrapperViewFilter
    {

        public WrapperViewFilter(View _view, List<ElementId> categories, ElementFilter new_ViewFilter_, List<ElementId> ListaBorrarFiltroExitente_pre)
        {
            this.view = _view;
            this.categories = categories;
            this.elemFilter = new_ViewFilter_;
            this.ListaBorrarFiltroExitente_pre = ListaBorrarFiltroExitente_pre;

            if (_view.ViewType == ViewType.FloorPlan || _view.ViewType == ViewType.CeilingPlan)
            {
                var _viewAux = (ViewPlan)view;
                
                var paraView=  ParameterUtil.FindParaByName(_viewAux.Parameters, "View Name");

                nombre = (paraView!=null ? paraView.AsString() :_viewAux.Name);
                
            }
            else if ( _view.ViewType == ViewType.Section )
            {
                var _viewAux = (ViewSection)view;


                var paraView = ParameterUtil.FindParaByName(_viewAux.Parameters, "View Name");

                nombre = (paraView != null ? paraView.AsString() : _viewAux.Name);
         
            }

        }

        public View view { get; set; }
        public List<ElementId> categories { get; set; }

        public ElementFilter elemFilter { get; set; }
        public List<ElementId> ListaBorrarFiltroExitente_pre { get;  set; }
        public string nombre { get;  set; }
    }
}
