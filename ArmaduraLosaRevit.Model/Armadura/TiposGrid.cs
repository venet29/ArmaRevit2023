
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
using ArmaduraLosaRevit.Model.GRIDS.model;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposGrid
    {

        public static Dictionary<string, Grid> ListaFamilias { get; set; }

        public static Grid elemetEncontrado;

        public static Grid ObtenerTextNote(string name, Document _Doc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            Grid elemento = M2_BuscarEnColecctor(name,   _Doc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }

        public static void Limpiar() => ListaFamilias = new Dictionary<string, Grid>();

        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Grid>();
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

        public static Element ObtenerPrimeroEncontrado(Document _doc)
        {
            // Get access to all the TextNote Elements

            FilteredElementCollector collectorUsed   = new FilteredElementCollector(_doc);
            return collectorUsed.OfClass(typeof(Grid)) .FirstOrDefault();
        }

        private static Grid M2_BuscarEnColecctor(string name, Document _doc)
        {
            FilteredElementCollector colectorFilter = new FilteredElementCollector(_doc).OfClass(typeof(Grid));

            var elemetEncontradoAux = colectorFilter.Where(e => e.Name.ToString() == name).FirstOrDefault();
            if (elemetEncontradoAux != null)
                elemetEncontrado = (Grid)elemetEncontradoAux;
            else
                elemetEncontrado = null;

            return elemetEncontrado;
        }

        private static void AgregarDiccionario(string nombre, Grid element)
        {
            if (element == null)
            {
            //    Util.ErrorMsg($"Tipos TextNote '{nombre}' no encontrada. Revisar familia");
                return;
            }
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Grid>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }



        public static List<EnvoltorioGrid> ObtenerTodosGrid(Document document, ElementId viewId=null)
        {

            List<EnvoltorioGrid> ListaResul = new List<EnvoltorioGrid>();
            FilteredElementCollector filtroGrid = new FilteredElementCollector(document).OfClass(typeof(Grid));// 2014

            if(viewId!=null)
                filtroGrid = new FilteredElementCollector(document,viewId).OfClass(typeof(Grid));// 2014

            foreach (Grid item in filtroGrid)
            {
                var nuewgrid = new EnvoltorioGrid(item);
                ListaResul.Add(nuewgrid);
            }
            return ListaResul;

        }
    }




}
