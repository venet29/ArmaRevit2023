using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
#pragma warning disable CS0105 // The using directive for 'Autodesk.Revit.DB' appeared previously in this namespace
using Autodesk.Revit.DB;
#pragma warning restore CS0105 // The using directive for 'Autodesk.Revit.DB' appeared previously in this namespace
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FILTROS
{
    public class FiltroVIga_Muro_Column : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element == null) return false;
            if (element.Category == null) return false;
            if (element.Category.Name == "Structural Framing" || element.Category.Name == "Walls" || element.Category.Name == "Structural Columns")
            {
                if (element.IsEstructural())
                    return true;
                else
                    return false;
            }
            else
                return false; ;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
}
