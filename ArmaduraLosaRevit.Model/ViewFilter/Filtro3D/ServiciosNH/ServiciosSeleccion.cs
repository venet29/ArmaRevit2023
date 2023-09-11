using ArmaduraLosaRevit.Model.modeloNH;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.ServiciosNH
{
    public static class ServiciosSeleccion
    {
        public static List<PorTiposNh> ObtenerViewLosa(UIApplication uiApp)
        {

            var listaView = SeleccionarView.ObtenerView_losa_elev_fund(uiApp.ActiveUIDocument);// _uiapp.ActiveUIDocument);
            var lst1 = listaView.Where(c => c.ViewType == ViewType.FloorPlan)
                                    .Select(c => new PorTiposNh(c.Name)).ToList();
            return lst1;
        }

        public static List<PorTiposNh> ObtenerViewElev(UIApplication uiApp)
        {

            SeleccionarView _SeleccionarView = new SeleccionarView();
            var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(uiApp.ActiveUIDocument.Document);// _uiapp.ActiveUIDocument.Document);
            var lst2 = ListaViewSection.Select(c => new PorTiposNh(c.Name)).ToList();
            return lst2;
        }
    }
}
