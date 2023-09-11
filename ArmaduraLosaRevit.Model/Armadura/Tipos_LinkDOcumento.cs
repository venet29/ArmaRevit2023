
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

    public class LinkDOcumentosDTO
    {
        public string Pathname { get; set; }
        public string Nombre { get; set; }
        public Document documento { get; set; }
        public RevitLinkInstance RevitLinkInstanc { get; set; }
        public int Inicio { get; internal set; }//borra
        public int Cantidad { get; internal set; }//borra
    }

    public class Tipos_LinkDOcumento
    {
        private static List<LinkDOcumentosDTO> ListaFamilias { get; set; }

        private static LinkDOcumentosDTO elemetEncontrado;

        public static void Limpiar() => ListaFamilias = new List<LinkDOcumentosDTO>();


        public static List<LinkDOcumentosDTO> ObtenerLinkTODOSDocumentos(UIApplication _uiapp, Document _doc)
        {
            try
            {
                //https://adndevblog.typepad.com/aec/2012/10/accessing-data-from-linked-file-using-revit-api.html
                FilteredElementCollector collector = new FilteredElementCollector(_doc);

                IList<Element> elems = collector.OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkType)).ToElements();

                foreach (Element e in elems)
                {
                    RevitLinkType linkType = e as RevitLinkType;

                    String s = String.Empty;
                    foreach (Document linkedDoc in _uiapp.Application.Documents)
                    {
                        if (linkedDoc.Title.Contains(linkType.Name.Replace(".rvt", "")))
                        {
                            var encontr = new LinkDOcumentosDTO()
                            {
                                Pathname = linkedDoc.PathName.ToString(),
                                documento = linkedDoc,
                                Nombre = Path.GetFileName(linkedDoc.PathName)
                            };
                            AgregarDiccionario(encontr);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener link. ex:{ex.Message}");
                return ListaFamilias;
            }
            return ListaFamilias;
        }


        public static List<LinkDOcumentosDTO> ObtenerLinkDocumentoActual(Document _doc)
        {

            List<LinkDOcumentosDTO> ListaLink = new List<LinkDOcumentosDTO>();
            try
            {
                // instace
                FilteredElementCollector collector2 = new FilteredElementCollector(_doc);
                collector2.OfClass(typeof(RevitLinkInstance));
                StringBuilder linkedDocs = new StringBuilder();
                foreach (Element elem in collector2)
                {
                    var _RevitLinkInstanc = elem as RevitLinkInstance;
                    if (_RevitLinkInstanc == null) continue;


                    Document linkedDoc = _RevitLinkInstanc.GetLinkDocument();
                    if (linkedDoc == null) continue;

                    var encontr = new LinkDOcumentosDTO()
                    {
                        RevitLinkInstanc = _RevitLinkInstanc,
                        Pathname = linkedDoc.PathName.ToString(),
                        documento = linkedDoc,
                        Nombre = Path.GetFileName(linkedDoc.PathName)
                    };
                    ListaLink.Add(encontr);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'RevitLinkInstances'. ex:{ex.Message} ");
                return new List<LinkDOcumentosDTO>();
            }

          //  Util.ErrorMsg($"No se encontro 'RevitLinkInstances' en documento alctual ");
            return ListaLink;
        }

        private static void AgregarDiccionario(LinkDOcumentosDTO _link)
        {
            if (_link == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new List<LinkDOcumentosDTO>();
            }
            if (!ListaFamilias.Exists(c => c.Pathname == _link.Pathname))
                ListaFamilias.Add(_link);
        }
        private static bool BuscarDiccionario(LinkDOcumentosDTO _link)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new List<LinkDOcumentosDTO>();
                return false;
            }
            else if (ListaFamilias.Count == 0)
            {
                return false;
            }

            if (ListaFamilias.Exists(c => c.Pathname == _link.Pathname))
                ListaFamilias.Add(_link);

            if (elemetEncontrado != null && elemetEncontrado.documento.IsValidObject == false)
            {
                ListaFamilias.Remove(_link);
                elemetEncontrado = null;
            }

            return (elemetEncontrado == null ? false : true);
        }


    }
}
