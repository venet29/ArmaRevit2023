using ArmaduraLosaRevit.Model.ViewFilter.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ViewFilter.Rules;
using ArmaduraLosaRevit.Model.ViewFilter.Analisis;

namespace ArmaduraLosaRevit.Model.ViewFilter
{
    public class CreadorViewFilterAllView : CreadorViewFilter_Base
    {

        public CreadorViewFilterAllView(UIApplication _uiapp) : base(_uiapp)
        {

        }
        public bool M1_CreateViewFilterEnTodasVistas()
        {
            try
            {
                using (Transaction tt = new Transaction(_doc, "AgregarFiltroV2-NH"))
                {
                    tt.Start();
                    IList<Element> elems = new FilteredElementCollector(_doc).OfClass(typeof(View)).ToElements();

                    categories.Clear();
                    categories.Add(new ElementId(BuiltInCategory.OST_Rebar));
                    categories.Add(new ElementId(BuiltInCategory.OST_PathRein));

                    foreach (Element e in elems)
                    {
                        View _view = e as View;

                        if (_view == null) continue;
                        if (!(_view.ViewType == ViewType.FloorPlan || _view.ViewType == ViewType.Section || _view.ViewType == ViewType.CeilingPlan)) continue;

                        filterRules.Clear();
                        ListaBorrarFiltroExitente.Clear();
                        ListaRemoverFiltroExitente.Clear();
                    

                        M2_CreateViewFilterBase_ocultarBarrasNoDeVista_sinTrans(_view);
                    }
                    tt.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        private void M2_CreateViewFilterBase_ocultarBarrasNoDeVista_sinTrans(View _view)
        {
  

            try
            {
                BuscarBorrarRemoverFiltro _BuscarBorrarRemoverFiltro = new BuscarBorrarRemoverFiltro(_doc);
                if (!_BuscarBorrarRemoverFiltro.A_BuscarSiExisteFiltro_OcultarBarrasNoDeVista(_view)) return;
                ListaBorrarFiltroExitente = _BuscarBorrarRemoverFiltro.ListaBorrarFiltroExitente;
                ListaRemoverFiltroExitente = _BuscarBorrarRemoverFiltro.ListaRemoverFiltroExitente;

                List<FilterRule> _newListFilterRule = CreadorFilter.A_ObtenerRegala_ocultarBarrasNoDeVista(_doc, _view);
                if (_newListFilterRule == null) return;
                if (_newListFilterRule.Count == 0) return;

                filterRules.AddRange(_newListFilterRule);

                ElementFilter elemFilter = B_CreateElementFilterFromFilterRules_AndFilter(filterRules);
                if (elemFilter == null) return;

                D_EjecutarREmoverBorrar_SinTrans(_view);

                C_FilterCreador_sinTrans(_view, categories, elemFilter);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }

        private bool C_FilterCreador_sinTrans(View _view, List<ElementId> categories, ElementFilter elemFilter)
        {
            try
            {
              

                // string nombre = ParameterUtil.FindParaByName(_view.Parameters, "View Name")?.AsString();
                string nombre = _view.ObtenerNombreIsDependencia();
                //1)add filter

                string nombreFiltro = $"Not{(nombre != null ? nombre : _view.Name)}";

                ParameterFilterElement parameterFilterElement = TiposFiltros.M1_GetFiltros_nh(nombreFiltro, _doc);

                if (parameterFilterElement == null)
                    parameterFilterElement = ParameterFilterElement.Create(_doc, nombreFiltro, categories);


                parameterFilterElement.SetElementFilter(elemFilter);

                // Apply filter to view
                if (!_view.IsFilterApplied(parameterFilterElement.Id))
                {
                    _view.AddFilter(parameterFilterElement.Id);
                    _view.SetFilterVisibility(parameterFilterElement.Id, false);
                }
                //2)add parameter
                ParameterUtil.SetParaStringNH(_view, "ViewNombre", nombre);

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

    }
}
