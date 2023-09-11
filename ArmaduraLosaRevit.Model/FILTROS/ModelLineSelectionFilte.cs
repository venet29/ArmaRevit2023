using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FILTROS
{
   public  class LineSelectionFilte : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element.Category == null) return false;
            if (element.Category.Name == "OST_RoomSeparationLines" || element.Category.Name == "OST_Lines")
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
