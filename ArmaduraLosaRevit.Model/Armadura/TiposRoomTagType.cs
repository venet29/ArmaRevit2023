
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
   public class TiposRoomTagType
    {

        public static RoomTagType getRoomTagType(string name,Document rvtDoc)
        {
            RoomTagType m_RoomTagType = null;
            List<RoomTagType> m_RoomTagTypes= new List<RoomTagType>();
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_RoomTags);
            m_RoomTagTypes = filteredElementCollector.Cast<RoomTagType>().ToList<RoomTagType>();


            foreach (var item in m_RoomTagTypes)
            {
                if (item.Name == name)
                {
                    m_RoomTagType = item;
                    return m_RoomTagType;
                }
            }
            return m_RoomTagType;

        }


    }
}
