
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
    public class TiposViewSchedule
    {

        public static Dictionary<string, ViewSchedule> ListaFamilias { get; set; }

        public static ViewSchedule elemetEncontrado;

        public static ViewSchedule ObtenerViewSchedule(string name, Document rvtDoc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            ViewSchedule elemento = M1_2_BuscarEnColecctor(name, BuiltInCategory.OST_Schedules, rvtDoc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, ViewSchedule>();
                return false;
            }
            else if (ListaFamilias.Count == 0)
            {
                return false;
            }


            if (ListaFamilias.ContainsKey(nombre))
                elemetEncontrado = ListaFamilias[nombre];

            if (elemetEncontrado != null && elemetEncontrado.IsValidObject == false)
            {
                ListaFamilias.Remove(nombre);
                elemetEncontrado = null;
            }

            return (elemetEncontrado == null ? false : true);
        }

        private static ViewSchedule M1_2_BuscarEnColecctor(string name, BuiltInCategory builtInCategory, Document rvtDoc)
        {
            ViewSchedule elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(ViewSchedule));
            filteredElementCollector.OfCategory(builtInCategory);
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                if (item.Name == name)
                {
                    elemento = (ViewSchedule)item;
                    return elemento;
                }
            }

            return elemento;
        }


        public static List<ViewSchedule> ObtenerTodos(Document rvtDoc)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);

            var ListLv = filteredElementCollector
                          .OfClass(typeof(ViewSchedule))
                          .OfCategory(BuiltInCategory.OST_Schedules)
                          .Cast<ViewSchedule>()
                          .ToList();

            return ListLv;
        }

        private static void AgregarDiccionario(string nombre, ViewSchedule element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, ViewSchedule>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
