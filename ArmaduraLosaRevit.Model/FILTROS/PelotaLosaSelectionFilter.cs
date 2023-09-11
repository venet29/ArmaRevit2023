using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FILTROS
{
   public  class PelotaLosaSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {

            if (element.Category.Name == "PELOTA DE LOSAS(NH)")
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
