
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
   public class TiposPathReinSpanSymbol_
    {
        public static Dictionary<string, Element> ListaFamilias { get; set; }
        protected static Element elemetEncontrado;

        public static Element M1_GetFamilySymbol_nh(string name, Document rvtDoc)
        {

            if (BuscarDiccionario_(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            Element elemento = M1_2_BuscarEnColecctor(name, rvtDoc, BuiltInCategory.OST_PathReinSpanSymbol);

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
                    return elemento;
                }
            }

            return elemento;
        }
        protected static bool BuscarDiccionario_(string nombre)
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

            return (elemetEncontrado == null ? false : true);
        }

        protected static void AgregarDiccionario(string nombre, Element element)
        {
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }
    


                              



        public static Element getPathReinSpanSymbol(string name, string scale, Document rvtDoc)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_PathReinSpanSymbol);
            var m_roomTagTypes = filteredElementCollector.ToList();

            return m_roomTagTypes.Where(fa => fa.Name == name || fa.Name == name + "_"+ scale).FirstOrDefault();

            //        Family family = new FilteredElementCollector(doc)
            //.OfClass(typeof(Family))
            //.OfCategory(BuiltInCategory.OST_PathReinSpanSymbol)
            //.Cast<Family>()
            //.FirstOrDefault(fa => fa.Name == name || fa.Name == name + "_"+ scale");
        }



        public static Element GetFamilySymbol_nh(string name, BuiltInCategory builtInCategory, Document rvtDoc)
        {
            //Debug.WriteLine($" ---->   name:{name}");
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
                    return elemento;
                }
            }

            return elemento;
        }



        public static FamilySymbol getFamilySymbol_nh(string name, BuiltInCategory builtInCategory, Document rvtDoc)
        {
            FamilySymbol elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(builtInCategory);
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                if (item.Name == name)
                {
                    elemento = (FamilySymbol)item;
                    return elemento;
                }
            }

            return elemento;
        }



    }
}
