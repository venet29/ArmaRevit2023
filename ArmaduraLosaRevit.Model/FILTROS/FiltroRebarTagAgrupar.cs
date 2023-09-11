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
   public  class FiltroRebarTagAgrupar : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element==null) return false;
            if (element.Category == null) return false;
            
            Debug.Print(element.Category.Name);
            if (element.Category.Name == "Structural Rebar Tags")
            {

                IndependentTag tg = element as IndependentTag;
                //  if (tg.Name == "MRA Rebar_F_SIN_50" || tg.Name == "MRA Rebar_F_50" || tg.Name == "MRA Rebar_SIN_50")
                if (tg.Name.Contains("MRA Rebar_F_SIN_") || tg.Name.Contains("MRA Rebar_F_") || tg.Name.Contains("MRA Rebar_SIN_"))
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
