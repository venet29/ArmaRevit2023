using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    class Tipo_ProjectInformation
    {

        internal static Element Obtener( Document _doc)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc);
            filteredElementCollector.OfCategory(BuiltInCategory.OST_ProjectInformation);
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                Debug.WriteLine(item.Name);
            }

            return (m_roomTagTypes.Count > 0 ? m_roomTagTypes[0] : null);
        }
    }
}
