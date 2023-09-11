
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using View = Autodesk.Revit.DB.View;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposView3D
    {

        public static Dictionary<string, View3D> ListaFamilias { get; set; }

        public static View3D elemetEncontrado;

        public static View3D ObtenerTiposView(string  nombreVista, Document rvtDoc)
        {

            if (BuscarDiccionario(nombreVista)) return elemetEncontrado;

            elemetEncontrado = M1_2_BuscarEnColecctor(rvtDoc, nombreVista);

            //AgregarDiccionario(nombreVista, elemetEncontrado);

            return elemetEncontrado;
        }

        public static void Limpiar() => ListaFamilias = new Dictionary<string, View3D>();

        private static View3D M1_2_BuscarEnColecctor(Document _doc,string nombreVista)
        {

            var asdListaVIew2 = ObtenerTodos(_doc);

            foreach (View3D item in asdListaVIew2)
            {
                AgregarDiccionario(item.Name, item);                               
            }

            if (BuscarDiccionario(nombreVista))
                return elemetEncontrado;
            else
                return null;

        }

        public static List<View3D> ObtenerTodos(Document _doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            collector.OfClass(typeof(View3D));
            return collector.Cast<View3D>()
                                        .Where(c => c != null)
                                        .ToList();
        }

        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, View3D>();
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

        private static void AgregarDiccionario(string nombre, View3D element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, View3D>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
