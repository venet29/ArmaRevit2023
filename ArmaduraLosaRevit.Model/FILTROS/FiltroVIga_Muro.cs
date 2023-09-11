﻿using Autodesk.Revit.DB;
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
   public  class FiltroVIga_Muro : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element==null) return false;
            if (element.Category == null) return false;

            return (element.Category.Name == "Structural Framing" || element.Category.Name == "Walls" ? true : false);          
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }
}