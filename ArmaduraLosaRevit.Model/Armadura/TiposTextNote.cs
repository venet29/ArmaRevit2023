
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
    public class TiposTextNote
    {

        public static Dictionary<string, TextNoteType> ListaFamilias { get; set; }

        public static TextNoteType elemetEncontrado;

        public static TextNoteType ObtenerTextNote(string name, Document _Doc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            TextNoteType elemento = M2_BuscarEnColecctor(name,   _Doc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }

        public static void Limpiar() => ListaFamilias = new Dictionary<string, TextNoteType>();

        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, TextNoteType>();
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

        public static TextNoteType ObtenerPrimeroEncontrado(Document _doc)
        {
            // Get access to all the TextNote Elements

            FilteredElementCollector collectorUsed   = new FilteredElementCollector(_doc);
            return (TextNoteType)collectorUsed.OfClass(typeof(TextNoteType)) .FirstOrDefault();
        }

        private static TextNoteType M2_BuscarEnColecctor(string name, Document _doc)
        {
            FilteredElementCollector colectorFilter = new FilteredElementCollector(_doc).OfClass(typeof(TextNoteType));

            var elemetEncontradoAux = colectorFilter.Where(e => e.Name.ToString() == name).FirstOrDefault();
            if (elemetEncontradoAux != null)
                elemetEncontrado = (TextNoteType)elemetEncontradoAux;
            else
                elemetEncontrado = null;

            return elemetEncontrado;
        }

        private static void AgregarDiccionario(string nombre, TextNoteType element)
        {
            if (element == null)
            {
            //    Util.ErrorMsg($"Tipos TextNote '{nombre}' no encontrada. Revisar familia");
                return;
            }
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, TextNoteType>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
