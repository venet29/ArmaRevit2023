
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
    public class TiposWorkSet
    {

        public static Dictionary<string, Workset> ListaFamilias { get; set; }

        public static Workset elemetEncontrado;

        public static Workset ObtenerTiposView(string  nombreVista, Document rvtDoc)
        {

            if (BuscarDiccionario(nombreVista)) return elemetEncontrado;

            elemetEncontrado = M1_2_BuscarEnColecctor(rvtDoc, nombreVista);

            //AgregarDiccionario(nombreVista, elemetEncontrado);

            return elemetEncontrado;
        }



        public static void Limpiar() => ListaFamilias = new Dictionary<string, Workset>();

        private static Workset M1_2_BuscarEnColecctor(Document _doc,string nombreVista)
        {

            FilteredElementCollector collector= new FilteredElementCollector(_doc);

            collector.OfClass(typeof(Workset));

            /// var asdListaVIew = collector.Cast<View>().ToDictionary(c=> c.Name,c=>c);

            var asdListaVIew2 = collector.Cast<Workset>()
                                        .Where(c => c != null)
                                        .ToList();

            foreach (Workset item in asdListaVIew2)
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
                ListaFamilias = new Dictionary<string, Workset>();
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

        private static void AgregarDiccionario(string nombre, Workset element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Workset>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
