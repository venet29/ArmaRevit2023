
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
   public class TiposGenericAnnotation
    {
        public static Dictionary<string, FamilySymbol> ListaFamilias { get; set; }
        protected static FamilySymbol elemetEncontrado;

        public static FamilySymbol M1_GetFamilySymbol_nh(string name, Document rvtDoc)
        {

            if (BuscarDiccionario_(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            FamilySymbol elemento = M1_2_BuscarEnColecctor(name, rvtDoc, BuiltInCategory.OST_GenericAnnotation);


            AgregarDiccionario(name, elemento);

            return elemento;
        }




        public static FamilySymbol M1_2_BuscarEnColecctor(string name,  Document rvtDoc,BuiltInCategory builtInCategory)
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
        protected static bool BuscarDiccionario_(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, FamilySymbol>();
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

        protected static void AgregarDiccionario(string nombre, FamilySymbol _familySymbol)
        {
            if (_familySymbol == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, FamilySymbol>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, _familySymbol);


        }
    


                              



     







    }
}
