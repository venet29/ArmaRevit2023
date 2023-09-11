
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
    public class Tipos_Arrow
    {


        private static Dictionary<string, Element> ListaFamilias { get; set; }

        private static Element elemetEncontrado;

        public static void Limpiar() => ListaFamilias = new Dictionary<string, Element>();
        public static Element ObtenerPrimerArrowheads(Document doc, string nombreElemento)
        {
            if (elemetEncontrado == null)
                elemetEncontrado = FindAllArrowheads(doc, nombreElemento).FirstOrDefault();

            return elemetEncontrado;
        }
        public static Element ObtenerArrowheadPorNombre(Document doc, string name)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            Element elemento = FindAllArrowheads(doc, name).FirstOrDefault();

            AgregarDiccionario(name, elemento);

            return elemento;
        }


        public static List<Element> FindAllArrowheads(Document doc, string nombreElemento)
        {
            List<Element> ret = new List<Element>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector = collector.WhereElementIsElementType();
            List<Element> temp = collector.ToList();
            foreach (Element e in temp)
            {

                // ParameterSet paramList = e.GetOrderedParameters();
                if (e.Name != nombreElemento) continue;
                // IList<Parameter> paramList = e.GetOrderedParameters();
                ret.Add(e);
                return ret;

            }


            return ret;
        }

        private static ElementType BuscarElementType(string name, Document doc)
        {

            var provider = new ParameterValueProvider(new ElementId(BuiltInParameter.ALL_MODEL_FAMILY_NAME));
            var collector = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WherePasses(new ElementParameterFilter(new FilterStringRule(provider, new FilterStringEquals(), "Arrowhead")));
            var elements = collector.ToElements().Cast<ElementType>().ToList();
            var newArrow = elements.FirstOrDefault(x => x.Name.StartsWith(name));
            return newArrow;

        }


        public static List<Element> ObtenerTodosLosElemetType(Document doc)
        {
            List<Element> ret = new List<Element>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector = collector.WhereElementIsElementType();
            List<Element> temp = collector.ToList();

            return temp;
        }



        private static void AgregarDiccionario(string nombre, Element element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }
        private static bool BuscarDiccionario(string nombre)
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


        public static bool CrearArropwIniciales(Document _doc)
        {


            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CreateIncialesArrow-NH");

                    ElementType _tipodeHook = BuscarElementType("Filled Dot 2mm", _doc);

                    if (_tipodeHook != null)

                    {
                        ElementType _tipodeHook50 = BuscarElementType("Filled Dot 2mm_50", _doc);
                        if (_tipodeHook50 == null)
                        {
                            var newArrow50 = _tipodeHook.Duplicate("Filled Dot 2mm_50");
                            if (ParameterUtil.FindParaByName(newArrow50, "Tick Size") != null) ParameterUtil.SetParaInt(newArrow50, "Tick Size", Util.CmToFoot(1/10f));
                        }

                        ElementType _tipodeHook75 = BuscarElementType("Filled Dot 2mm_75", _doc);
                        if (_tipodeHook75 == null)
                        {
                            var newArrow75 = _tipodeHook.Duplicate("Filled Dot 2mm_75");
                            if (ParameterUtil.FindParaByName(newArrow75, "Tick Size") != null) ParameterUtil.SetParaInt(newArrow75, "Tick Size", Util.CmToFoot(0.66665 / 10f));
                        }

                        ElementType _tipodeHook100 = BuscarElementType("Filled Dot 2mm_100", _doc);
                        if (_tipodeHook100 == null)
                        {
                            var newArrow100 = _tipodeHook.Duplicate("Filled Dot 2mm_100");
                            if (ParameterUtil.FindParaByName(newArrow100, "Tick Size") != null) ParameterUtil.SetParaInt(newArrow100, "Tick Size", Util.CmToFoot(0.5 /10f));
                        }
                    }

               


                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                return false;
            }
            return true;
        }
    }
}
