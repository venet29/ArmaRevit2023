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
   public  class FiltroElement : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element==null) return false;
 

            return true;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
}
