
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
    public class TiposLineaPattern
    {

        protected static Dictionary<string, Element> ListaFamilias { get; set; }

        protected static Element elemetEncontrado;

        public static Element ObtenerTipoLinea(string name, Document _Doc)
        {
            //NOTA SI SE DESEA USAR EL TIPO ' SOLID'   USAR DIRECTAMENETE   LinePatternElement.GetSolidPatternId()
            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            Element elemento = M2_BuscarEnColecctor(name, _Doc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }

        public static void Limpiar() => ListaFamilias = new Dictionary<string, Element>();

        protected static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
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

        protected static Element M2_BuscarEnColecctor(string name, Document _doc)
        {
            FilteredElementCollector graphic_styles = new FilteredElementCollector(_doc).OfClass(typeof(GraphicsStyle));
            elemetEncontrado = graphic_styles.Where<Element>(e => e.Name.ToString() == name).FirstOrDefault();

            return elemetEncontrado;
        }

        protected static void AgregarDiccionario(string nombre, Element element)
        {
            if (element == null)
            {
                // Util.ErrorMsg($"Linea tipo '{nombre}' no encontrada.");
                return;
            }
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
