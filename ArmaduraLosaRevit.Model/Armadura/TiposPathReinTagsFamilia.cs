
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
    public class TiposPathReinTagsFamilia
    {

        public static Dictionary<string, Family> ListaFamilias { get; set; }

        public static Family elemetEncontrado;

        public static Family M1_GetFamilySymbol_nh(string name, Document rvtDoc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            Family elemento = M1_2_BuscarEnColecctor(name, rvtDoc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Family>();
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
        private static Family M1_2_BuscarEnColecctor(string name, Document rvtDoc)
        {

            Family m_family_ = null;

            //start = new TimeSpan(DateTime.Now.Ticks);
            FilteredElementCollector filteredElementCollector1 = new FilteredElementCollector(rvtDoc);
            m_family_ = filteredElementCollector1
                .OfClass(typeof(Family))
                .Cast<Family>()
                .Where(c => c.Name == name).FirstOrDefault();
            //opcion para obtener lista de colector
            //var asdf = filteredElementCollector1.OfType<Family>().ToList();

            return m_family_;

        }



        public static List<FamilySymbol> M2_TodasLAsFamilias(Document rvtDoc)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_PathReinTags).WhereElementIsElementType();
            var m_roomTagTypes = filteredElementCollector.Cast<FamilySymbol>().ToList();


            return m_roomTagTypes;
        }

        private static void AgregarDiccionario(string nombre, Family element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Family>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
