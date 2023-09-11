
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
    public class TiposView
    {

        public static Dictionary<string, View> ListaFamilias { get; set; }

        public static View elemetEncontrado;

        public static View ObtenerTiposView(string  nombreVista, Document rvtDoc)
        {

            if (BuscarDiccionario(nombreVista)) return elemetEncontrado;

            elemetEncontrado = M1_2_BuscarEnColecctor(rvtDoc, nombreVista);

            //AgregarDiccionario(nombreVista, elemetEncontrado);

            return elemetEncontrado;
        }

        public static View ObtenerTiposView_queCOntenga( Document _doc)
        {

            if (BuscarDiccionario("SoloPAraCrearLista")) return elemetEncontrado;

            elemetEncontrado = M1_2_BuscarEnColecctor(_doc, "SoloPAraCrearLista");

            if (elemetEncontrado == null)
                elemetEncontrado = ListaFamilias.Where(c => c.Key.Contains("ARMADURA") && c.Key.Contains("LOSA") && c.Value.IsTemplate==true).Select(rr => rr.Value).FirstOrDefault();
            //AgregarDiccionario(nombreVista, elemetEncontrado);

            return elemetEncontrado;
        }

        public static void Limpiar() => ListaFamilias = new Dictionary<string, View>();

        private static View M1_2_BuscarEnColecctor(Document _doc,string nombreVista)
        {

            FilteredElementCollector collector= new FilteredElementCollector(_doc);

            collector.OfClass(typeof(View));

            /// var asdListaVIew = collector.Cast<View>().ToDictionary(c=> c.Name,c=>c);

            var asdListaVIew2 = collector.Cast<View>()
                                        .Where(c => c != null)
                                        .ToList();

            foreach (View item in asdListaVIew2)
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
                ListaFamilias = new Dictionary<string, View>();
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

        private static void AgregarDiccionario(string nombre, View element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, View>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

        public static List<Element> ObtenerTodosSegunTipo(Document _doc, BuiltInCategory _categoty)
        {
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            var lista = collector.OfCategory(_categoty)
                                .ToList();
            return lista;
        }

    }
}
