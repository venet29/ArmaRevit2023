using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.FILTROS
{
    public  class FiltroConduit : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element==null) return false;
            if (element.Category == null) return false;
            
            Debug.Print(element.Category.Name);
            if (element.Category.Name == ConstBim.BuitInCategoryDUct)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
}
