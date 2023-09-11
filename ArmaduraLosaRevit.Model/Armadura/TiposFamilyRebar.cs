
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
using System.Runtime.InteropServices.WindowsRuntime;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposFamilyRebar
    {




        /// <summary>
        /// obtienen la familia del documento con el nombre  'name' 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rvtDoc"></param>
        /// <returns></returns>
        public static Family getFamilyRebarShape(string name, Document rvtDoc)
        {

            //ElementClassFilter sad  = new ElementClassFilter()


                     

            List<Family> m_family = new List<Family>();
      
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(Family));
            m_family = filteredElementCollector.Cast<Family>().ToList();


            Family familiaResult = null;

            foreach (var item in m_family)
            {
                if (item.Name == name)
                {
                    familiaResult = item;
                    return familiaResult;
                }
            }

            return familiaResult;

        } 

    }
}
