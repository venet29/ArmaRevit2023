
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;

namespace ArmaduraLosaRevit.Model.Armadura
{
   public class TiposAreaReinSpanSymbol
    {

        public static Element getAreaReinSpanSymbol(string name,Document rvtDoc)
        {
            Element elemento =null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_AreaReinSpanSymbol);
           var m_roomTagTypes = filteredElementCollector.ToList();        
            foreach (var item in m_roomTagTypes)
            {
                if (item.Name == name)
                {
                    elemento = item;
                    return elemento;
                }
            }

            return elemento;
        }

        //***********************************************

        public static Element getAreaReinTag(string name, Document rvtDoc)
        {
            Element elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_AreaReinTags);
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                if (item.Name == name)
                {
                    elemento = item;
                    return elemento;
                }
            }

            return elemento;
        }

    }
}
