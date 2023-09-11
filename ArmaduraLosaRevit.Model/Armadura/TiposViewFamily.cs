
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
    public class TiposViewFamily
    {

        public static Dictionary<string, ViewFamilyType> ListaFamilias { get; set; }

        public static ViewFamilyType elemetEncontrado;

        public static ViewFamilyType ObtenerTiposViewFamily(ViewFamily ViewFamilyname, Document rvtDoc)
        {

            if (BuscarDiccionario(ViewFamilyname.ToString())) return elemetEncontrado;

            ViewFamilyType elemento =null;

            if(ViewFamilyname== ViewFamily.Detail)
                elemento = M1_2_BuscarEnColecctor(rvtDoc);

            if (elemento == null) return null;

            AgregarDiccionario(ViewFamilyname.ToString(), elemento);

            return elemento;
        }

        public static void Limpiar() => ListaFamilias = new Dictionary<string, ViewFamilyType>();

        private static ViewFamilyType M1_2_BuscarEnColecctor( Document _doc)
        {


              ViewFamilyType DetailViewId = null;
            IList<Element> elems = new FilteredElementCollector(_doc).OfClass(typeof(ViewFamilyType)).ToElements();
            foreach (Element e in elems)
            {
                ViewFamilyType v = e as ViewFamilyType;

                //if (v != null && v.ViewFamily == ViewFamily.Detail)
                if (v != null && v.ViewFamily == ViewFamily.Detail)
                {
                    DetailViewId = (ViewFamilyType)e;
                    return DetailViewId;
                }
            }

            return DetailViewId;

        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, ViewFamilyType>();
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

        private static void AgregarDiccionario(string nombre, ViewFamilyType element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, ViewFamilyType>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
