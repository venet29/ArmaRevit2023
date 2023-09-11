using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter.Analisis
{



    internal class BuscarBorrarRemoverFiltro
    {
        private readonly Document _doc;

        public  List<ElementId> ListaBorrarFiltroExitente { get; private set; }
        public  List<ElementId> ListaRemoverFiltroExitente { get; private set; }

        public BuscarBorrarRemoverFiltro(Document _doc)
        {
            this._doc = _doc;
        }

        private void ResetLista()
        {
            if (ListaBorrarFiltroExitente == null)
            {
                ListaBorrarFiltroExitente = new List<ElementId>();
                ListaRemoverFiltroExitente = new List<ElementId>();
            }
            else
            {
                ListaRemoverFiltroExitente.Clear();
                ListaRemoverFiltroExitente.Clear();
            }
        }
        //borra los filtros creados cuando las vista tenian los template de elvacio y de losas asignado
        private void BorrarFiltrosTemplate_deElevaciones_NOT()
        {
            List<ParameterFilterElement> oldFilters = TiposFiltros.M2_GetAllFiltros_nh(_doc).Select(c => (ParameterFilterElement)c.Value).ToList();

            foreach (ParameterFilterElement filter in oldFilters)
            {
                if ((filter.Name == $"Not{ConstNH.NOMBRE_VIEW_TEMPLATE_LOSA}" ||
                    filter.Name == $"Not{ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV}"))
                    ListaBorrarFiltroExitente.Add(filter.Id);
            }
        }


        internal  bool A_BuscarSiExisteFiltro_OcultarBarrasNoDeVista(View view)
        {
            try
            {
                ResetLista();

                BorrarFiltrosTemplate_deElevaciones_NOT();

                var listaFiltroView = view.GetFilters().ToList();

                foreach (ElementId item in listaFiltroView)
                {
                    if (ListaBorrarFiltroExitente.Contains(item)) continue;

                    Element filtroVIew = _doc.GetElement(item);

                    if (filtroVIew.Name == $"Not{view.Name}") continue;

                    if (!(filtroVIew is ParameterFilterElement)) continue;

                    if (((ParameterFilterElement)filtroVIew).Name.Contains("Not"))
                        ListaRemoverFiltroExitente.Add(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
      



        internal bool B_BuscarSiExisteFiltro_DejarBarrasNoDeVista(View view)
        {
            try
            {
                ResetLista();

                BorrarFiltrosTemplate_deElevaciones_NOT();

                // borrar filtro defind
                var listaFiltroView = view.GetFilters().ToList();

                foreach (ElementId item in listaFiltroView)
                {
                    if (ListaBorrarFiltroExitente.Contains(item)) continue;

                    Element filtroVIew = _doc.GetElement(item);

                    if (filtroVIew.Name == $"Not{view.Name}_Inv") continue;

                    if (!(filtroVIew is ParameterFilterElement)) continue;

                    if (((ParameterFilterElement)filtroVIew).Name.Contains("Not") && ((ParameterFilterElement)filtroVIew).Name.Contains("_Inv"))
                        ListaRemoverFiltroExitente.Add(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        internal bool C_BuscarSiExisteFiltro_NombreVistaYTipoBarras(View view, List<string> ListaNombreIgualExcluir, List<string> ListaNombreContein)
        {
            try
            {
                ResetLista();

                BorrarFiltrosTemplate_deElevaciones_NOT();

                // borrar filtro defind
                var listaFiltroView = view.GetFilters().ToList();

                foreach (ElementId item in listaFiltroView)
                {
                    if (ListaBorrarFiltroExitente.Contains(item)) continue;

                    Element filtroVIew = _doc.GetElement(item);

                    if  (ListaNombreIgualExcluir.Exists(c=>c== filtroVIew.Name)) continue;

                    if (!(filtroVIew is ParameterFilterElement)) continue;

                    if (ListaNombreContein.Exists(c=> ((ParameterFilterElement)filtroVIew).Name.Contains(c)))
                        ListaRemoverFiltroExitente.Add(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
