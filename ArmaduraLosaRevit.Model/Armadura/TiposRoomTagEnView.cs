
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
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposRoomTagEnView
    {

        public static Dictionary<string, Element> ListaFamilias { get; set; }

        public static Element elemetEncontrado;

        public static List<RoomTag> M1_GetFamilySymbol_nh(ElementId elementid, Document rvtDoc, ElementId viewId)
        {

           //Debug.WriteLine($" ---->   name:{name}");
          return  M1_2_BuscarEnColecctor(elementid, rvtDoc, viewId);

        }
 
        private static  List<RoomTag>  M1_2_BuscarEnColecctor(ElementId RoomId, Document rvtDoc,ElementId viewId)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc, viewId);
          //  filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_RoomTags).WhereElementIsNotElementType(); 
            var m_roomTagTypes = filteredElementCollector.Cast<RoomTag>().Where(c=> c.Room.Id== RoomId).ToList();
           // var m_roomTagTypes = filteredElementCollector.Cast<IndependentTag>().ToList();
           // var m_roomTagTypes = filteredElementCollector.ToList();nn

            //M_Path Reinforcement Tag(ID_cuantia_largo)_A_50_0

            return m_roomTagTypes;
        }

        internal static List<RoomTag> M1_AllGetFamilySymbol(Document _doc, Autodesk.Revit.DB.View elemview)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc, elemview.Id);
            //  filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_RoomTags).WhereElementIsNotElementType();
            var m_roomTagTypes = filteredElementCollector.Cast<RoomTag>().ToList();

            return m_roomTagTypes;
        }

        private static void AgregarDiccionario(string nombre, Element element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
