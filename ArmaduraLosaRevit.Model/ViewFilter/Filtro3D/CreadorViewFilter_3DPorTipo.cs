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
    public class CreadorViewFilter_3DPorTipo : CreadorViewFilter_Base
    {
        public CreadorViewFilter_3DPorTipo(UIApplication _uiapp, View view3D) :base(_uiapp)
        {
            _view3D = view3D;
            nombreFiltro = "";
        }

        public void M2_CreateViewFilterPorTipo(List<ParametrosFiltro> listapara, string nombreFiltro_)
        {
            if (_view3D == null) return;
           // if (!(_view3D is View3D )) return;
            if (listapara == null) return;
            if (listapara.Count==0) return;

            categories.Clear();
            categories.Add(new ElementId(BuiltInCategory.OST_Rebar));
            categories.Add(new ElementId(BuiltInCategory.OST_PathRein));

            _listapara = listapara;
            nombreFiltro = nombreFiltro_;
            M2_CreateViewFilterBase(_view3D);
        }

       

        protected void M2_CreateViewFilterBase(View _view3D)
        {
            try
            {            
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
      
    }
}
