
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using ArmaduraLosaRevit.Model.FAMILIA;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposWallTagsEnView
    {
      


        public static IndependentTag cargarListaDetagWall(Document _doc, View _view, string nametagWall)
        {
            List <IndependentTag> ListaRebarTagInView = new List<IndependentTag>();
            IndependentTag _resultadoTagMuro = null;
            try
            {
                FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc, _view.Id);
                //  filteredElementCollector.OfClass(typeof(FamilySymbol));
                filteredElementCollector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_WallTags).WhereElementIsElementType();
                ListaRebarTagInView = filteredElementCollector.Cast<IndependentTag>().ToList();

                _resultadoTagMuro = ListaRebarTagInView
                    .Where(c => c.Name == "12mm")
                    .Where(r => r.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString()== "TAG MUROS (CORTO)")
                    .FirstOrDefault();

                var ListaRebarTagInView_ = filteredElementCollector.ToList();

            }
            catch (Exception)
            {

            }

            return _resultadoTagMuro;
        }


        public static FamilySymbol cargarListaDetagWall_model(Document _doc )
        {
            List<FamilySymbol> ListaRebarTagInView = new List<FamilySymbol>();
            FamilySymbol _resultadoTagMuro = null;
            try
            {
                FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc);
                //  filteredElementCollector.OfClass(typeof(FamilySymbol));
                filteredElementCollector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_WallTags).WhereElementIsElementType();
                ListaRebarTagInView = filteredElementCollector.Cast<FamilySymbol>().ToList();

                _resultadoTagMuro = ListaRebarTagInView
                    
                    .Where(r => r.FamilyName.Contains(FactoryCargarFamilias.TAGMUROS))
                    .FirstOrDefault();

                var ListaRebarTagInView_ = filteredElementCollector.ToList();

            }
            catch (Exception)
            {

            }

            return _resultadoTagMuro;
        }


        public static FamilySymbol cargarListaDetagWall2(Document _doc,string nombrefamilia, string nametagWall)
        {
            List<FamilySymbol> ListaRebarTagInView = new List<FamilySymbol>();
            FamilySymbol _resultadoTagMuro = null;
            try
            {
                FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc);
                //  filteredElementCollector.OfClass(typeof(FamilySymbol));
                filteredElementCollector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_WallTags);
                ListaRebarTagInView = filteredElementCollector.Cast<FamilySymbol>().ToList();
                var ListaRebarTagInView_ = filteredElementCollector.ToList();

                _resultadoTagMuro = ListaRebarTagInView
                    .Where(c => c.Name == nombrefamilia && c.FamilyName == nametagWall)                    
                    .FirstOrDefault();

        

            }
            catch (Exception)
            {

            }

            return _resultadoTagMuro;
        }
    }
}
