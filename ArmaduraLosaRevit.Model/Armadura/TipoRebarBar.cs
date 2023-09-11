using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TipoRebarBar
    {

        //public static bool clonarRebarBar(UIDocument Uidocument)
        //{
        //    bool result = false;
        //    Family linesId = new FilteredElementCollector(Uidocument.Document).OfClass(typeof(RebarBarType)).FirstOrDefault();




        //    List<Family> m_family = new List<Family>();

        //    FilteredElementCollector filteredElementCollector = new FilteredElementCollector(Uidocument.Document);
        //    filteredElementCollector.OfClass(typeof(Family));
        //    m_family = filteredElementCollector.Cast<Family>().ToList<Family>();
        //    Family familiaResult = null;

        //    //foreach (var item in m_family)
        //    //{
        //    //    if (item.Name == name)
        //    //    {
        //    //        familiaResult = item;
        //    //        return familiaResult;
        //    //    }
        //    //}

        //    //return familiaResult;










        //    Family sourceFamily = null;

        //    using (var trans = new Transaction(Uidocument.Document))
        //    {
        //        trans.Start("Select family");

        //        // Prompt user to select some family instance. SelectSingleElementOfType is my own method.
        //        //var sourceFamilyInstance = uidoc.SelectSingleElementOfType<FamilyInstance>("Select FamilyInstance");

        //        //// Get Family object from family instance
        //        //sourceFamily = sourceFamilyInstance?.Symbol.Family;

        //        //if (sourceFamily == null)
        //        //return Result.Failed;

        //        /*
        //         * here I need to call something like:
        //         * 
        //         * var destFamily = sourceFamily.Clone();
        //         * destFamily.Name = destFamily.Name + " dest";
        //         * destFamily.SomeParam = "New value";
        //         * destFamily.SaveToCurrentDrawing()
        //         */

        //        trans.Commit();
        //    }



        //    return result;
        //}



        public static Family GetFamiliesOfCategory(Document doc, BuiltInCategory bic)
        {
            // This does not work:

            //IEnumerable<Family> families = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where<Family>(f => FamilyFirstSymbolCategoryEquals(f, bic)).FirstOrDefault;
            Family families = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where<Family>(f => FamilyFirstSymbolCategoryEquals(f, bic)).FirstOrDefault();

            return families;
        }
        static bool FamilyFirstSymbolCategoryEquals(Family f, BuiltInCategory bic)
        {
            Document doc = f.Document;
            ISet<ElementId> ids = f.GetFamilySymbolIds();
            Category cat = (0 == ids.Count)
              ? null
              : doc.GetElement(ids.First<ElementId>()).Category;
            return null != cat && cat.Id.IntegerValue.Equals((int)bic);
        }

    }
}
