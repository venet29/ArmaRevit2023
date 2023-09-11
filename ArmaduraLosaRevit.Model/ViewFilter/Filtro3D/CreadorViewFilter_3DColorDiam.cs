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
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.FactoryGraphicSettings;
using ArmaduraLosaRevit.Model.Visibilidad.Ayuda;

namespace ArmaduraLosaRevit.Model.ViewFilter
{
    public class FiltroDiamDTO
    {
        public ElementFilter ElementFilter { get; set; }
        public OverrideGraphicSettings ORGS { get; set; }
        public bool Visibilidad { get; set; }
        public int dimm { get; internal set; }
    }

    public class CreadorViewFilter_3DColorDiam : CreadorViewFilter_Base
    {
        public CreadorViewFilter_3DColorDiam(UIApplication _uiapp, View view3D) : base(_uiapp)
        {
            _view3D = view3D;
        }

        public void M2_CreateViewFilterRevisar_Diam(List<ParametrosFiltro> listapara)
        {
            if (_view3D == null) return;
            // if (!(_view3D is View3D)) return;
            if (listapara == null) return;
            if (listapara.Count == 0) return;

            _listapara = listapara;
            M2_CreateViewFilter_conTodosDiamVisibleBase(_view3D);

        }
        public void M2_CreateViewFilterRevisar_DiamColor(List<ParametrosFiltro> listapara)
        {
            if (_view3D == null) return;
            // if (!(_view3D is View3D)) return;
            if (listapara == null) return;
            if (listapara.Count == 0) return;

            _listapara = listapara;
            M2_CreateViewFilter_conTodosDiamVisibleBase_Color(_view3D);

        }
        private void M2_CreateViewFilter_conTodosDiamVisibleBase(View _view3D)
        {
            if (_listapara[0].Nombrefilfro.Contains("Diam"))
                RestearParametrosSoloRebar();
            else
                RestearParametros();
            try
            {
                //a)
                List<string> ListaNombreIgualExcluir = new List<string>() { };
                List<string> ListaNombreContein = ListaFiltro.ListaFiltroBorrarDiam();

                BuscarBorrarRemoverFiltro _BuscarBorrarRemoverFiltro = new BuscarBorrarRemoverFiltro(_doc);
                if (!_BuscarBorrarRemoverFiltro.C_BuscarSiExisteFiltro_NombreVistaYTipoBarras(_view3D, ListaNombreIgualExcluir, ListaNombreContein)) return;

                ListaBorrarFiltroExitente = _BuscarBorrarRemoverFiltro.ListaBorrarFiltroExitente;
                ListaRemoverFiltroExitente = _BuscarBorrarRemoverFiltro.ListaRemoverFiltroExitente;
                D_EjecutarREmoverBorrar_ConTras(_view3D);

                List<FiltroDiamDTO> FiltroDiamDTO = new List<FiltroDiamDTO>();
                List<ParametrosFiltro> ListaPArametros = _listapara.Where(c => c.IsVisibilidad == true).ToList();

                filterRules.Clear();
                nombreFiltro = $"MostrarBarrasDiam-NH";

                //b)
                List<FilterRule> _newListFilterRule = CreadorFilter3DRevison.A_ObtenerRegla_parameterDiametro(_doc, ListaPArametros);
                if (_newListFilterRule == null) return;
                if (_newListFilterRule.Count == 0) return;

                filterRules.AddRange(_newListFilterRule);

                ElementFilter elemFilter = B_CreateElementFilterFromFilterRules_AndFilter(filterRules);
                if (elemFilter == null) return;

                FiltroDiamDTO.Add(new FiltroDiamDTO() { ElementFilter = elemFilter, ORGS = null, Visibilidad = false });

                //c)cambiar color por diamtro

                try
                {
                    using (Transaction t = new Transaction(_doc, "AgregarFiltroDiamtro-NH"))
                    {
                        t.Start();
                        for (int i = 0; i < FiltroDiamDTO.Count; i++)
                        {
                            var item = FiltroDiamDTO[i];
                            _ogp = null;
                            //  nombreFiltro = $"MostrarBarrasDiam-NH";
                            C_FilterCreador_SinTrans(item.ElementFilter, _view3D, false);
                        }
                        //2)add parameter
                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Util.DebugDescripcion(ex);

                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }

        private void M2_CreateViewFilter_conTodosDiamVisibleBase_Color(View _view3D)
        {
            if (_listapara[0].Nombrefilfro.Contains("Diam"))
                RestearParametrosSoloRebar();
            else
                RestearParametros();

            try
            {
                //a)
                List<string> ListaNombreIgualExcluir = new List<string>() { };
                List<string> ListaNombreContein = ListaFiltro.ListaFiltroBorrarDiamColor();

                BuscarBorrarRemoverFiltro _BuscarBorrarRemoverFiltro = new BuscarBorrarRemoverFiltro(_doc);
                if (!_BuscarBorrarRemoverFiltro.C_BuscarSiExisteFiltro_NombreVistaYTipoBarras(_view3D, ListaNombreIgualExcluir, ListaNombreContein)) return;

                ListaBorrarFiltroExitente = _BuscarBorrarRemoverFiltro.ListaBorrarFiltroExitente;
                ListaRemoverFiltroExitente = _BuscarBorrarRemoverFiltro.ListaRemoverFiltroExitente;
                D_EjecutarREmoverBorrar_ConTras(_view3D);

                List<FiltroDiamDTO> FiltroDiamDTO = new List<FiltroDiamDTO>();
                List<ParametrosFiltro> ListaPArametros = _listapara.Where(c => c.IsVisibilidad == true).ToList();

                filterRules.Clear();

                for (int j = 0; j < ListaPArametros.Count; j++)
                {
                    List<ParametrosFiltro> ListaIndicidual = new List<ParametrosFiltro>() { ListaPArametros[j] };
                    filterRules.Clear();
                    //b)
                    List<FilterRule> _newListFilterRule = CreadorFilter3DRevison.A_ObtenerRegla_parameterDiametroColor(_doc, ListaIndicidual);
                    if (_newListFilterRule == null) return;
                    if (_newListFilterRule.Count == 0) return;

                    filterRules.AddRange(_newListFilterRule);

                    ElementFilter elemFilter = B_CreateElementFilterFromFilterRules_AndFilter(filterRules);
                    if (elemFilter == null) return;

                    FiltroDiamDTO.Add(new FiltroDiamDTO() { ElementFilter = elemFilter, ORGS =null, Visibilidad = true });

                    int dimm = Util.ConvertirStringInInteger(ListaPArametros[j].Valorfilfro.Replace(".0 mm", "").Trim());
                    nombreFiltro = $"MostrarBarrasDiam{dimm}-NH";
                    _ogp = Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(FActoryGraphicSettingsBarrasElevLosa.ObtenerColor_Rebar_3D(dimm));
                    //c)cambiar color por diamtro

                    try
                    {
                        using (Transaction t = new Transaction(_doc, "AgregarFiltroDiamtro-NH"))
                        {
                            t.Start();
                            for (int i = 0; i < FiltroDiamDTO.Count; i++)
                            {
                                var item = FiltroDiamDTO[i];
                                C_FilterCreador_SinTrans(item.ElementFilter, _view3D, item.Visibilidad);
                            }
                            //2)add parameter
                            t.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.DebugDescripcion(ex);

                    }
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }
    }
}
