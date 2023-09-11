﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FILTROS
{
    public class RebarDimenionesSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element == null) return false;
            if (element.Category == null) return false;

            Debug.Print(element.Category.Name);
            if (element.Category.Name == "Structural Rebar" || element.Category.Name.Contains("Rebar"))
            {
                return true;
            }
            else if (element.Category.Name == "Lines")
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
