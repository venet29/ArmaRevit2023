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
    public class CreadorViewFilter_3DRevisarCub : CreadorViewFilter_Base
    {
        public CreadorViewFilter_3DRevisarCub(UIApplication _uiapp, View view3D) :base(_uiapp)
        {
            _view3D = view3D;
            nombreFiltro = "";
        }

        public void M2_CreateViewFilterRevisar(List<ParametrosFiltro> listapara)
        {
            if (_view3D == null) return;
           // if (!(_view3D is View3D )) return;
            if (listapara == null) return;
            if (listapara.Count==0) return;

            _listapara = listapara;
            nombreFiltro = $"MostrarBarras_{_listapara[0].Nombrefilfro}-NH";
            M2_CreateViewFilterBase(_view3D);
        }

       

        protected void M2_CreateViewFilterBase(View _view3D)
        {

            try
            {
             
                //a)
                List<string> ListaNombreIgualExcluir = new List<string>() { };

                List<string> ListaNombreContein = new List<string>();
                if (_listapara[0].Nombrefilfro == "LargoRevision")
                {
                    RestearParametrosSoloRebar();
                    ListaNombreContein = ListaFiltro.ListaFiltroBorrarLargoRevision();
                }
                else
                {
                    RestearParametros();
                    ListaNombreContein = ListaFiltro.ListaFiltroBorrarRevision_sinLArgo();
                }
             

                BuscarBorrarRemoverFiltro _BuscarBorrarRemoverFiltro = new BuscarBorrarRemoverFiltro(_doc);                
                if (!_BuscarBorrarRemoverFiltro.C_BuscarSiExisteFiltro_NombreVistaYTipoBarras(_view3D, ListaNombreIgualExcluir, ListaNombreContein)) return;

                ListaBorrarFiltroExitente = _BuscarBorrarRemoverFiltro.ListaBorrarFiltroExitente;
                ListaRemoverFiltroExitente = _BuscarBorrarRemoverFiltro.ListaRemoverFiltroExitente;
                D_EjecutarREmoverBorrar_ConTras(_view3D);

                //b)
                List<FilterRule> _newListFilterRule = CreadorFilter3DRevison.A_ObtenerRegla_parameterShare(_doc, _listapara);                
                if (_newListFilterRule == null) return;
                if (_newListFilterRule.Count == 0) return;
               
                filterRules.AddRange(_newListFilterRule);

                ElementFilter elemFilter = B_CreateElementFilterFromFilterRules_AndFilter(filterRules);
                if (elemFilter == null) return;
               
                C_FilterCreador_conTrans(  elemFilter, _view3D,false);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }
        /*
        public void M3_BorrarFiltros(List<string> ListaNombreIgualExcluir,List<string> ListaNombreContein)
        {
            if (_view3D == null) return;
            if (!(_view3D is View3D)) return;

            M3_BorrarBase(_view3D,ListaNombreIgualExcluir, ListaNombreContein);
        }
        private void M3_BorrarBase(View3D _view3D, List<string> ListaNombreIgualExcluir, List<string> ListaNombreContein)
        {
            RestearParametros();
            try
            {

                //a)
                //List<string> ListaNombreIgualExcluir = new List<string>() { };
               // List<string> ListaNombreContein = ListaFiltro.ListaFiltroBorrarRevision();

                BuscarBorrarRemoverFiltro _BuscarBorrarRemoverFiltro = new BuscarBorrarRemoverFiltro(_doc);
                if (!_BuscarBorrarRemoverFiltro.C_BuscarSiExisteFiltro_NombreVistaYTipoBarras(_view3D, ListaNombreIgualExcluir, ListaNombreContein)) return;

                ListaBorrarFiltroExitente = _BuscarBorrarRemoverFiltro.ListaBorrarFiltroExitente;
                ListaRemoverFiltroExitente = _BuscarBorrarRemoverFiltro.ListaRemoverFiltroExitente;

                //b)


                D_EjecutarREmoverBorrar_ConTras(_view3D);

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                Util.DebugDescripcion(ex);
            }
        }
        */
    }
}
