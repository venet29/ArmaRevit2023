using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class FamiliasGeneral
    {

        public static Family GetFamily(string name, Document rvtDoc)
        {

            //ElementClassFilter sad  = new ElementClassFilter()




            List<Family> m_family = new List<Family>();

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(Family));
            m_family = filteredElementCollector.Cast<Family>().ToList();


            Family familiaResult = null;

            foreach (var item in m_family)
            {
                Debug.WriteLine(item.Name);
                if (item.Name == name)
                {
                    familiaResult = item;
                    return familiaResult;
                }
            }

            return familiaResult;

        }



        public static Element GetFamilySymbol(string name, Document rvtDoc, BuiltInCategory builtInCategory)
        {
            Element elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(builtInCategory);
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                Debug.WriteLine(item.Name);
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
