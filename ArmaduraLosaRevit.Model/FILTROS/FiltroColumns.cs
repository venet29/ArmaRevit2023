using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FILTROS
{
   public  class FiltroColumns : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element==null) return false;
            if (element.Category == null) return false;
            
            Debug.Print(element.Category.Name);
            if (element.Category.Name == "Structural Columns")
            {
                var materialidad = element.Document.GetElement(((FamilyInstance)element).StructuralMaterialId) as Material;
                if (materialidad.Name.Contains("Concrete"))
                    return true;
                else
                    return false;
            }
            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
}
