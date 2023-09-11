using ArmaduraLosaRevit.Model.BarraV.Automatico.model;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda
{
    public class ObtenerListaElevacionVAriosDto
    {
        public static List<ElevacionVAriosDto> Ejecutar(Document _doc, string prefijoView,string prefijoJson, List<FileInfo> ListaRutasArchivos)
        {
            try
            {
                if (ListaRutasArchivos.Count == 0)
                {
                    return new List<ElevacionVAriosDto>();
                }

                SeleccionarView _SeleccionarView = new SeleccionarView();
                var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(_doc);

                var ListaTodasLosaList = ListaViewSection.Where(c => c.ViewType == ViewType.Section)
                                        .Select(c => new ElevacionVarioSelected(c.Name)).ToList();
                var ListaElevacionVAriosDto = ListaViewSection.Where(c => c.ViewType == ViewType.Section)
                                        .Select(c => new ElevacionVAriosDto(c.Name, c, ListaRutasArchivos, ObtenerFileInfo(c, prefijoView, prefijoJson, ListaRutasArchivos))).OrderBy(c=>c.NombreElev).ToList();

                ListaElevacionVAriosDto.ForEach(c => c.ObtenerTipoDeView());

                return ListaElevacionVAriosDto;
            }
            catch (Exception)
            {

                Util.ErrorMsg("Error al Obtener Lista 'ElevacionVAriosDto'");
            }
            return new List<ElevacionVAriosDto>();

        }

        private static FileInfo ObtenerFileInfo(ViewSection _viewSection, string prefijoView, string prefijoJson, List<FileInfo> listaRutasArchivos)
        {
            try
            {
                FileInfo result = new FileInfo("NoEncontrado");
                if (_viewSection == null) return result;
                if (listaRutasArchivos == null) return result;

                var splitPrefijoView = prefijoView.Split(',');
                var splitPrefijoJson = prefijoJson.Split(',');

                //1
                var nombreView = _viewSection.Name;
                foreach (string item in splitPrefijoView)
                {
                    nombreView= nombreView.Replace(item, "");
                }

                //busca Igual
                foreach (var item in listaRutasArchivos)
                {
                    var arcJson = item.Name.Replace(".json", "");
         
                    foreach (string itemJson in splitPrefijoJson)
                    {
                        arcJson = arcJson.Replace(itemJson, "");
                    }

                    if (arcJson.Trim() == nombreView.Trim())
                        return item;
                }
                //busca que contenga
                foreach (var item in listaRutasArchivos)
                {
                    var arcJson = item.Name.Replace("_", "").Replace(".json", "");
                    if (arcJson.Contains(nombreView))
                        return item;
                }
                //var result = listaRutasArchivos.Where(ifo => ifo.Name.Replace(".json", "").Replace("_", "").Contains(_viewSection.Name.Replace(prefijo, ""))).First();

                return result;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return new FileInfo("NoEncontrado");
            }
        }
    }
}
