using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ViewFilter.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter
{
    public class CreadorViewFilter_Base
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        //protected View3D _view3D;
        protected View _view3D;
        protected List<ElementId> ListaBorrarFiltroExitente; // LOS BORRAR DE LA LISTA DE FILTROS DESPONIBLE. PUEDE ESTAR ASIGNADO O NO
        protected List<ElementId> ListaRemoverFiltroExitente;  // REMUEVE EL FILTRO DE LAVISTA
        protected List<FilterRule> filterRules;
        protected List<FilterRule> filterRulesv2;
        protected List<ElementId> categories;
        protected List<ParametrosFiltro> _listapara;
        protected string nombreFiltro;

        protected OverrideGraphicSettings _ogp = null;
        public CreadorViewFilter_Base(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            ListaBorrarFiltroExitente = new List<ElementId>();
            ListaRemoverFiltroExitente = new List<ElementId>();
            filterRules = new List<FilterRule>();
            filterRulesv2 = new List<FilterRule>(); 
            categories = new List<ElementId>();
        }

        protected void RestearParametros()
        {
            categories.Clear();
            categories.Add(new ElementId(BuiltInCategory.OST_Rebar));
            categories.Add(new ElementId(BuiltInCategory.OST_PathRein));

            ListaBorrarFiltroExitente.Clear();
            ListaRemoverFiltroExitente.Clear();
        }
        protected void RestearParametrosSoloRebar()
        {
            categories.Clear();
            categories.Add(new ElementId(BuiltInCategory.OST_Rebar));
            ListaBorrarFiltroExitente.Clear();
            ListaRemoverFiltroExitente.Clear();
        }
        protected ElementFilter B_CreateElementFilterFromFilterRules_AndFilter(IList<FilterRule> filterRules)
        {  
            LogicalAndFilter elemFilter = null;
            try
            {
                // We use a LogicalAndFilter containing one ElementParameterFilter
                // for each FilterRule. We could alternatively create a single
                // ElementParameterFilter containing the entire list of FilterRules.
                IList<ElementFilter> elemFilters = new List<ElementFilter>();
                foreach (FilterRule filterRule in filterRules)
                {
                    ElementParameterFilter elemParamFilter = new ElementParameterFilter(filterRule);
                    elemFilters.Add(elemParamFilter);
                }
                elemFilter = new LogicalAndFilter(elemFilters);
            }
            catch (Exception)
            {
                return null;
            }

            return elemFilter;
        }
        protected ElementFilter B_CreateElementFilterFromFilterRules_ORFilter(IList<FilterRule> filterRules)
        {

            LogicalOrFilter elemFilter = null;
            try
            {
                IList<ElementFilter> elemFilters = new List<ElementFilter>();
                foreach (FilterRule filterRule in filterRules)
                {
                    ElementParameterFilter elemParamFilter = new ElementParameterFilter(filterRule);
                    elemFilters.Add(elemParamFilter);
                }
                elemFilter = new LogicalOrFilter(elemFilters);
            }
            catch (Exception)
            {
                return null;
            }
            return elemFilter;
        }
        public string ObtenerNombreAntiguoVista(View _ViewMantenerBArras) => _ViewMantenerBArras.ObtenerNombre_ViewNombre();

        protected void D_EjecutarREmoverBorrar_SinTrans(View _view)
        {
            try
            {
                foreach (ElementId item in ListaRemoverFiltroExitente)
                {
                    if (_view.IsFilterApplied(item))
                        _view.RemoveFilter(item);
                }

                if (ListaBorrarFiltroExitente.Count > 0)
                    _doc.Delete(ListaBorrarFiltroExitente);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al remover y borrar filtros  \n  ex:{ex.Message}");                
            }
        }

        public void D_EjecutarREmoverBorrar_ConTras(View _view,  List<ElementId> ListaBorrarFiltroExitente=null)
        {
            try
            {
                if (ListaBorrarFiltroExitente != null)
                    this.ListaBorrarFiltroExitente = ListaBorrarFiltroExitente;

                using (Transaction t = new Transaction(_doc, "EjecutarREmoverBorrar-NH"))
                {
                    t.Start();
                    D_EjecutarREmoverBorrar_SinTrans(_view);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al remover y borrar filtros (CON TRANS) \n  ex:{ex.Message}");               
            }
        }

        protected bool C_FilterCreador_conTrans(ElementFilter elemFilter,View _view, bool visibilidad)
        {
            try
            {
                using (Transaction t = new Transaction(_doc, "AgregarFiltro-NH"))
                {
                    t.Start();
                    C_FilterCreador_SinTrans(elemFilter, _view, visibilidad);
                    //2)add parameter
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        protected bool C_FilterCreador_SinTrans(ElementFilter elemFilter, View _view, bool visibilidad)
        {
            try
            {
                    ParameterFilterElement parameterFilterElement = TiposFiltros.M1_GetFiltros_nh(nombreFiltro, _doc);

                    if (parameterFilterElement == null)
                        parameterFilterElement = ParameterFilterElement.Create(_doc, nombreFiltro, categories);

                    parameterFilterElement.SetElementFilter(elemFilter);

                    // Apply filter to view
                    if (!_view.IsFilterApplied(parameterFilterElement.Id))
                    {
                        _view.AddFilter(parameterFilterElement.Id);
                        _view.SetFilterVisibility(parameterFilterElement.Id, visibilidad);
                        if (_ogp != null)
                            _view.SetFilterOverrides(parameterFilterElement.Id, _ogp);
                    }
                    //2)add parameter

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
