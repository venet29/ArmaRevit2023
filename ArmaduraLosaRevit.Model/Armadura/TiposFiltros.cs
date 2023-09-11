
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
    public class TiposFiltros
    {

        public static Dictionary<string, ParameterFilterElement> ListaFamilias { get; set; }

        public static ParameterFilterElement elemetEncontrado;

        public static ParameterFilterElement M1_GetFiltros_nh(string name, Document _doc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            ParameterFilterElement elemento = M1_2_BuscarEnColecctor(name,   _doc);

            //AgregarDiccionario(name, elemento);

            return elemento;
        }

        public static Dictionary<string, ParameterFilterElement> M2_GetAllFiltros_nh( Document _doc)
        {

            
          //  if(ListaFamilias.Count!=0) return ListaFamilias;
            //Debug.WriteLine($" ---->   name:{name}");
            M1_2_BuscarEnColecctor("", _doc);

            //AgregarDiccionario(name, elemento);

            return ListaFamilias;
        }

        private static ParameterFilterElement M1_2_BuscarEnColecctor(string name,  Document _doc)
        {
            ListaFamilias= new Dictionary<string, ParameterFilterElement>();
            ParameterFilterElement elemento = null;
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            ICollection<Element> oldFilters = collector.OfClass(typeof(ParameterFilterElement)).ToElements();

            foreach (var item in oldFilters)
            {
                AgregarDiccionario(item.Name, (ParameterFilterElement)item);
                if (item.Name == name)
                {
                    elemento = (ParameterFilterElement)item;
                 
                }
            }

            return elemento;
        }

        private static void AgregarDiccionario(string nombre, ParameterFilterElement element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, ParameterFilterElement>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, ParameterFilterElement>();
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
    }
}
