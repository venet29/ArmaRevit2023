
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
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class Tipos_FiilRegion
    {


        static Dictionary<string, FilledRegionType> ListaFamilias { get; set; }

        private static FilledRegionType elemetEncontrado;

        public static void Limpiar() => ListaFamilias = new Dictionary<string, FilledRegionType>();

        public static FilledRegionType ObtenerFilledRegionTypePorNombre(Document doc, string name)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            FilledRegionType elemento = BuscarElementType(name, doc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }



        private static FilledRegionType BuscarElementType(string name, Document _doc)
        {
            FilteredElementCollector fillRegionTypes = new FilteredElementCollector(_doc).OfClass(typeof(FilledRegionType));
            List<FilledRegionType> temp = fillRegionTypes.Cast<FilledRegionType>().ToList();
            FilledRegionType primer = fillRegionTypes.Cast<FilledRegionType>().Where(c => c.Name == name).FirstOrDefault();
            return primer;
        }


        public static List<FilledRegionType> ObtenerTodosLosElemetType(Document _doc)
        {
            FilteredElementCollector fillRegionTypes = new FilteredElementCollector(_doc).OfClass(typeof(FilledRegionType));
            List<FilledRegionType> temp = fillRegionTypes.Cast<FilledRegionType>().ToList();

            return temp;
        }



        private static void AgregarDiccionario(string nombre, FilledRegionType element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, FilledRegionType>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, FilledRegionType>();
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


        public static bool CrearFillIniciales(Document _doc)
        {
            // falta desarrollar


            try
            {
                ElementType _tipodeHook = ObtenerTodosLosElemetType(_doc).FirstOrDefault();

                if (_tipodeHook == null)
                {
                    Util.ErrorMsg("No se encontro ningun FillRegion");
                    return false;
                }


                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CreateFIllRegion-NH");

                    ElementType _tipodeHook50 = BuscarElementType("SOLIDO PASADAS", _doc);
                    if (_tipodeHook50 == null)
                    {
                        var fillSOlid = TiposPatternType.ObtenerTipoPattern("Solid", _doc);

                        FilledRegionType newArrow50 = (FilledRegionType)_tipodeHook.Duplicate("SOLIDO PASADAS");
                        newArrow50.ForegroundPatternId = fillSOlid.Id;
                        newArrow50.BackgroundPatternColor = FactoryColores.ObtenerColoresPorNombre(Enumeraciones.TipoCOlores.rojopuro);
                        newArrow50.ForegroundPatternColor = FactoryColores.ObtenerColoresPorNombre(Enumeraciones.TipoCOlores.rojopuro);
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
