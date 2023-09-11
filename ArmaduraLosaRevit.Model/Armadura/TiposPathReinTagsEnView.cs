using System.Collections.Generic;
using System.Data;
using System.Linq;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposPathReinTagsEnView
    {

        public static Dictionary<string, Element> ListaFamilias { get; set; }

        public static Element elemetEncontrado;

        public static List<IndependentTag> M1_GetFamilySymbol_ConPathReinforment(ElementId elementid, Document _doc, ElementId viewId)
        {

           //Debug.WriteLine($" ---->   name:{name}");
          return  M1_2_BuscarEnColecctorPorPathReinforment(elementid, _doc, viewId);

        }
 
        private static  List<IndependentTag>  M1_2_BuscarEnColecctorPorPathReinforment(ElementId PathReinfElemId, Document rvtDoc, ElementId viewId)
        {
          //  Document rvtDoc = _uiapp.ActiveUIDocument.Document;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc, viewId);
          //  filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_PathReinTags).WhereElementIsNotElementType(); 
            var m_roomTagTypes = filteredElementCollector.Cast<IndependentTag>().Where(c=> c.Obtener_GetTaggedLocalElementID() == PathReinfElemId).ToList();


            return m_roomTagTypes;
        }

        public static List<IndependentTag> M1_2_BuscarPathReinInView(Document rvtDoc, ElementId viewId)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc, viewId);
            //  filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_PathReinTags).WhereElementIsNotElementType();
            var m_roomTagTypes = filteredElementCollector.Cast<IndependentTag>().ToList();


            return m_roomTagTypes;
        }


        private static void AgregarDiccionario(string nombre, Element element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
