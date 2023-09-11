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
    public class CreadorViewFilter_OcultarBarrasNoDeVista: CreadorViewFilter_Base
    {
        protected  UIApplication _Uiapp;
       // protected  string nombreAntiguoVista;
 
        protected string NombreAntiguoVista;

        public CreadorViewFilter_OcultarBarrasNoDeVista(UIApplication _uiapp):base(_uiapp)
        {
            _Uiapp = _uiapp;
          //  this.nombreAntiguoVista = nombreAntiguoVista;

        
        }


        public void M2_CreateViewFilterTodos(View _view)
        {

            if (_view == null) return;
            if (!(_view.ViewType == ViewType.FloorPlan || _view.ViewType == ViewType.Section || _view.ViewType == ViewType.CeilingPlan)) return;

            M2_CreateViewFilterBase(_view);
        }


        public void M2_CreateViewFilterLosa(View _view)
        {

            if (_view == null) return;
            if ((_view.ViewType != ViewType.FloorPlan)) return;

            M2_CreateViewFilterBase(_view);
        }

        public void M2_CreateViewFilterElev(View _view)
        {

            if (_view == null) return;
            if (_view.ViewType != ViewType.Section) return;

            M2_CreateViewFilterBase(_view);
        }

        private void M2_CreateViewFilterBase(View _view)
        {
            RestearParametros();
            //categories.Add(new ElementId(BuiltInCategory.OST_Rebar));
            //categories.Add(new ElementId(BuiltInCategory.OST_PathRein));

            //ListaBorrarFiltroExitente.Clear();
            //ListaRemoverFiltroExitente.Clear();
            try
            {

               // if (!A_BuscarSiExisteFiltro(_view, nombreAntiguoVista)) return;

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

                D_EjecutarREmoverBorrar_ConTras(_view);

                C_FilterCreador_conTrans(_view, categories, elemFilter); 
        
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }

        protected bool C_FilterCreador_conTrans(View _view, List<ElementId> categories, ElementFilter elemFilter)
        {
            try
            {
              

                using (Transaction t = new Transaction(_doc, "AgregarFiltro-NH"))
                {
                    t.Start();
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
    }
}
