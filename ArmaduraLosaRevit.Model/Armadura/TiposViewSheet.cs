
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
    public class TiposViewSheetDTO
    {
        public ViewSheet ViewSheet_ { get; set; }
        public string Nombre { get; set; }
        public string Numero { get; set; }
        public TiposViewSheetDTO(ViewSheet element)
        {
            this.ViewSheet_ = element;
            this.Nombre = element.Name;
            this.Numero = element.SheetNumber;
        }

    }
    public class TiposViewSheet
    {

        public static List<TiposViewSheetDTO> ListaSheetNombre { get; set; }

        public static TiposViewSheetDTO elemetEncontrado;

        public static TiposViewSheetDTO ObtenerTiposView_PorNombre(string nombreSheet, Document rvtDoc)
        {

            if (BuscarDiccionario("PorNombre",nombreSheet)) return elemetEncontrado;

            M1_2_BuscarEnColecctor_NombreVista(rvtDoc);

            if (BuscarDiccionario("PorNombre",nombreSheet))
                return elemetEncontrado;
            else
                return null;

        }

        public static TiposViewSheetDTO ObtenerTiposView_PorNumero(string NumeroSheet, Document rvtDoc)
        {
            if (BuscarDiccionario("PorNumero", NumeroSheet)) return elemetEncontrado;

            M1_2_BuscarEnColecctor_NombreVista(rvtDoc);

            if (BuscarDiccionario("PorNumero", NumeroSheet))
                return elemetEncontrado;
            else
                return null;
        }

        public static void Limpiar() => ListaSheetNombre = new List<TiposViewSheetDTO>();

        private static bool M1_2_BuscarEnColecctor_NombreVista(Document _doc)
        {
            try
            {
                FilteredElementCollector collector = new FilteredElementCollector(_doc);

                collector.OfClass(typeof(ViewSheet));

                /// var asdListaVIew = collector.Cast<View>().ToDictionary(c=> c.Name,c=>c);

                var asdListaVIew2 = collector.Cast<ViewSheet>()
                                            .Where(c => c != null)
                                            .ToList();

                foreach (ViewSheet item in asdListaVIew2)
                {
                    AgregarDiccionario(item);
                }

            }
            catch (Exception)
            {

              return false;
            }
            return true;
        }

        private static bool BuscarDiccionario(string tipo,string nombre)
        {
            elemetEncontrado = null;
            if (ListaSheetNombre == null)
            {
                ListaSheetNombre = new List<TiposViewSheetDTO>();
                return false;
            }
            else if (ListaSheetNombre.Count == 0)
            {
                return false;
            }

            if(tipo=="PorNombre")
                elemetEncontrado = ListaSheetNombre.Find(c => c.Nombre == nombre);
            else
                elemetEncontrado = ListaSheetNombre.Find(c => c.Numero == nombre);

            if (elemetEncontrado != null && elemetEncontrado.ViewSheet_.IsValidObject == false)
            {
                ListaSheetNombre.Remove(elemetEncontrado);
                elemetEncontrado = null;
            }

            return (elemetEncontrado == null ? false : true);
        }
  
        private static void AgregarDiccionario(ViewSheet element)
        {
            if (element == null) return;
            if (ListaSheetNombre == null)
            {
                ListaSheetNombre = new List<TiposViewSheetDTO>();
            }

            if (!ListaSheetNombre.Exists(c => c.Nombre == element.Name))
                ListaSheetNombre.Add(new TiposViewSheetDTO(element));
        }

        public static FamilySymbol ObtenerFamiliaPAraSheet(Document _doc, string nombre = "FORMATO GENERAL DELPORTE (A0)")
        {
            try
            {
                // Get an available title block from document
                FilteredElementCollector collector = new FilteredElementCollector(_doc);
                collector.OfClass(typeof(FamilySymbol));
                collector.OfCategory(BuiltInCategory.OST_TitleBlocks);
                var listaTipoListaOlo = collector.ToList();

                var fsElement = listaTipoListaOlo.Where(c => ((FamilySymbol)c).FamilyName == nombre).FirstOrDefault();


                if (fsElement == null)
                {
                    Util.ErrorMsg("No se encontro template: 'FORMATO DELPORTE'");
                    return null;
                }
                var fs = fsElement as FamilySymbol;

                return fs;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"No se encontro template: 'FORMATO DELPORTE'.\n\nex:{ex.Message}");
                return null;
            }
        }

    }
}
