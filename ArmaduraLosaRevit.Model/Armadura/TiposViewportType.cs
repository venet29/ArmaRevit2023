
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
    public class TiposViewportType
    {

        public static Dictionary<string, Viewport> ListaFamilias { get; set; }

        public static Viewport elemetEncontrado;
        public static void Limpiar() => ListaFamilias = new Dictionary<string, Viewport>();
        public static Viewport ObtenerTiposView(string  nombreVista, Document rvtDoc)
        {

            if (BuscarDiccionario(nombreVista)) return elemetEncontrado;

            elemetEncontrado = M1_2_BuscarEnColecctor(rvtDoc, nombreVista);

            //AgregarDiccionario(nombreVista, elemetEncontrado);

            return elemetEncontrado;
        }

        public static Viewport ObtenerTiposView_queCOntenga( Document _doc)
        {

            if (BuscarDiccionario("SoloPAraCrearLista")) return elemetEncontrado;

            elemetEncontrado = M1_2_BuscarEnColecctor(_doc, "SoloPAraCrearLista");

            if (elemetEncontrado == null)
                elemetEncontrado = ListaFamilias.Where(c => c.Key.Contains("ARMADURA") && c.Key.Contains("LOSA")).Select(rr => rr.Value).FirstOrDefault();
            //AgregarDiccionario(nombreVista, elemetEncontrado);

            return elemetEncontrado;
        }



        private static Viewport M1_2_BuscarEnColecctor(Document _doc,string nombreVista)
        {

            FilteredElementCollector collector= new FilteredElementCollector(_doc);

            collector.OfClass(typeof(Viewport));

            /// var asdListaVIew = collector.Cast<View>().ToDictionary(c=> c.Name,c=>c);

            var asdListaVIew2 = collector.Cast<Viewport>()
                                        .Where(c => c != null)
                                        .ToList();

            foreach (Viewport item in asdListaVIew2)
            {
                AgregarDiccionario(item.Name, item);                               
            }

            if (BuscarDiccionario(nombreVista))
                return elemetEncontrado;
            else
                return null;

        }

        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Viewport>();
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

        private static void AgregarDiccionario(string nombre, Viewport element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Viewport>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }



        public static List<Element> ObtenerTodosLabel(Document _doc, View _view)
        {
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            var lista = collector.OfCategory(BuiltInCategory.OST_ViewportLabel)
                                .ToList();
            return lista;
        }


    }
}
