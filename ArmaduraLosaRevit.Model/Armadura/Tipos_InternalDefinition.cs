
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
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class Tipos_InternalDefinition
    {


        private static Dictionary<string, InternalDefinition> ListaFamilias { get; set; }

        private static InternalDefinition elemetEncontrado;

        public static void Limpiar() => ListaFamilias = new Dictionary<string, InternalDefinition>();
        public static InternalDefinition ObtenerPrimerInternalDefinition(Document doc, string nombreElemento)
        {
            if (elemetEncontrado == null)
                elemetEncontrado = FindAllInternalDefinition(doc, nombreElemento).FirstOrDefault();

            return elemetEncontrado;
        }
        public static InternalDefinition ObtenerInternalDefinitionPorNombre(Document doc, string name)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            InternalDefinition elemento = FindAllInternalDefinitionv2(doc, name);

            AgregarDiccionario(name, elemento);

            return elemento;
        }


        public static List<InternalDefinition> FindAllInternalDefinition(Document doc, string nombreElemento)
        {
            List<InternalDefinition> ret = new List<InternalDefinition>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(InternalDefinition));
            List<InternalDefinition> temp = collector.Cast<InternalDefinition>().ToList();

            foreach (InternalDefinition e in temp)
            {

                // ParameterSet paramList = e.GetOrderedParameters();
                if (e.Name != nombreElemento) continue;
                // IList<Parameter> paramList = e.GetOrderedParameters();
                ret.Add(e);
                return ret;

            }


            return ret;
        }
        private static InternalDefinition FindAllInternalDefinitionv2(Document doc, string paramName)
        {
            BindingMap bindingMap = doc.ParameterBindings;
            DefinitionBindingMapIterator iter = bindingMap.ForwardIterator();
            iter.Reset();

            while (iter.MoveNext())
            {
                Definition tempDefinition = iter.Key;
                // find the definition of which the name is the appointed one
                if (String.Compare(tempDefinition.Name, paramName) != 0)
                {
                    AgregarDiccionario(tempDefinition.Name, (InternalDefinition)iter.Key);
                    continue;
                }
                InternalDefinition intDef = (InternalDefinition)iter.Key;
                return intDef;
                
            }
            return null;
        }


        public static List<InternalDefinition> ObtenerTodosLosInternalDefinitione(Document doc)
        {
            List<InternalDefinition> ret = new List<InternalDefinition>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector = collector.WhereElementIsElementType();
            List<InternalDefinition> temp =collector.Cast<InternalDefinition>().ToList();

            return temp;
        }



        private static void AgregarDiccionario(string nombre, InternalDefinition element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, InternalDefinition>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, InternalDefinition>();
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
