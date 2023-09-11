using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarDimensiones
    {

        public static DimensionType DimensionTipoTraslapo { get; set; } = default;
        public static List<Dimension> ListaDimensiones { get; private set; }

        public static Dimension ObtenerDimensionePorNombre(Document doc, string nombre)
        {



            List<Dimension> linearDimensions = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Dimensions)
                        .Cast<Dimension>().Where(q => q.DimensionShape == DimensionShape.Linear).ToList();

            List<Dimension> linearDimensions2 = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Dimensions).Cast<Dimension>().ToList();

            //buscar primer nivel
            FilteredElementCollector Colectornivel = new FilteredElementCollector(doc);
            Dimension Lv = Colectornivel
                          .OfCategory(BuiltInCategory.OST_Dimensions)
                          .Cast<Dimension>()
                         .Where(X => X.Name == nombre).FirstOrDefault();

            return Lv;
        }


        public static DimensionType ObtenerDimensionTYpo_Traslapo(Document doc)
        {
            DimensionType _DimensionType = null;
            if (DimensionTipoTraslapo != null)
                return DimensionTipoTraslapo;
            else
                _DimensionType= ObtenerDimensionTypePorNombre(doc, "Traslapo");

            if (_DimensionType == null)
            {
                Util.ErrorMsg("No se encontro familia para crear traslapo, cargar configuracion inicial para crear");
                return null;
            }
            else
                return _DimensionType;
        }

        public static DimensionType ObtenerDimensionTypePorNombre(Document doc, string nombre)
        {

            List<DimensionType> m_family = new List<DimensionType>();
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(DimensionType));
            m_family = filteredElementCollector.Cast<DimensionType>().ToList();


            DimensionType familiaResult = null;

            foreach (var item in m_family)
            {
                if (item.Name == nombre)
                {
                    return familiaResult = item;
                    // return familiaResult;
                }
            }

            Util.ErrorMsg($"No se encontro DimensionType:{nombre}");
            return familiaResult;
        }


        public static List<Dimension> ObtenerListaInViewPorNombre(Document _doc, View _view, string nombre)
        {
            List<Dimension> listaDimension = new List<Dimension>();
            try
            {
                ElementCategoryFilter annoter = new ElementCategoryFilter(BuiltInCategory.OST_Dimensions);
                ElementClassFilter filterClass = new ElementClassFilter(typeof(Dimension));
                LogicalAndFilter hostFilter = new LogicalAndFilter(annoter, annoter);

                FilteredElementCollector collector3 = new FilteredElementCollector(_doc, _view.Id);
                collector3.WherePasses(hostFilter).WhereElementIsNotElementType(); // Filters;
                listaDimension = collector3.OfType<Dimension>().Where(c => c.Name == nombre).ToList();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener traslapos de vistas. \n ex:{ex.Message}");
                return new List<Dimension>();
            }
            return listaDimension;
        }


        public static bool SeleccionarMouseDimension(UIDocument _uidoc,string nombre)
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                ListaDimensiones = new List<Dimension>();

                ISelectionFilter f = new FiltroDimension();
                //selecciona un objeto floor
               var  _listaElementsTagRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar Dimensiones");
                if (_listaElementsTagRebarSeleccionado.Count == 0) return false;

                foreach (var item in _listaElementsTagRebarSeleccionado)
                {
                    if (item is Dimension &&  item.Name==nombre)
                        ListaDimensiones.Add(item as Dimension);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
                return false;
            }
            return true;
            // if (_listaElementsRebarSeleccionado.Count > 0) ObtenerListaDeREbarNivelActualS();
        }

    }
}
