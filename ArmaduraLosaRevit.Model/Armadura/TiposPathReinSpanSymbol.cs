
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
   public class TiposPathReinSpanSymbol
    {
        public static Dictionary<string, Element> ListaFamilias { get; set; }
        protected static Element elemetEncontrado;

        public static Element M1_GetFamilySymbol_nh(string name, Document rvtDoc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            Element elemento = M1_2_BuscarEnColecctorv2(name, rvtDoc, BuiltInCategory.OST_PathReinSpanSymbol);

            AgregarDiccionario(name, elemento);

            return elemento;
        }


        public static Element M1_2_BuscarEnColecctor(string name, Document rvtDoc, BuiltInCategory builtInCategory)
        {
            Element elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(builtInCategory).WhereElementIsElementType();
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                if (item.Name == name)
                {
                    elemento = item;
                   // return elemento;
                }

                AgregarDiccionario(item.Name, item);
            }

            //opcion busqueda
            // var m_roomTagTypes = filteredElementCollector.ToList();
            // return m_roomTagTypes.Where(fa => fa.Name == name || fa.Name == name + "_" + scale).FirstOrDefault();
            return elemento;
        }

        //verificando si es mas rapido
        public static Element M1_2_BuscarEnColecctorv2(string name, Document rvtDoc, BuiltInCategory builtInCategory)
        {
            Element elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            elemento = filteredElementCollector.OfCategory(builtInCategory)
                                              .WhereElementIsElementType()
                                              .Where(c=> AgregarDiccionario(c.Name, c) &&c.Name==name).FirstOrDefault();
      
            return elemento;
        }

        public static List<Element> M1_2_BuscarListaEnColecctor(Document rvtDoc, BuiltInCategory builtInCategory)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(builtInCategory).WhereElementIsElementType();
            var m_roomTagTypes = filteredElementCollector.ToList();

            //opcion busqueda
            // var m_roomTagTypes = filteredElementCollector.ToList();
            // return m_roomTagTypes.Where(fa => fa.Name == name || fa.Name == name + "_" + scale).FirstOrDefault();
            return m_roomTagTypes;
        }

        protected static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
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

        protected static bool AgregarDiccionario(string nombre, Element element)
        {
            if (element == null) return false;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);
            return true;
        }
    }
}
