using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter.Rules
{
    class CreadorFilter
    {
        // a)
        internal static List<FilterRule> A_ObtenerRegala_ocultarBarrasNoDeVista(Document _doc, View view)
        {
            List<FilterRule> _newListFilterRule = new List<FilterRule>();
            try
            {
                ElementId result = AyudaBuscaParametrerShared.ObtenerParameterShare(_doc, "NombreVista");
                if (result != null)
                {
                    var _newFilterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(result, view.ObtenerNombreIsDependencia(), false);
                    _newListFilterRule.Add(_newFilterRule);
                }
                else
                {
                    Util.ErrorMsg("No se encontro parametro compartido 'NombreVista'");
                    _newListFilterRule.Clear();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return null;
            }
            return _newListFilterRule;
        }

        //2) fittro obtener 
        internal static List<FilterRule> B_ObtenerRegala_DejarBarrasNoDeVista_Inferior(Document _doc, View _view, string nombreTipo)
        {
            List<FilterRule> _newListFilterRule = new List<FilterRule>();
            try
            {
                ElementId result_NombreVista = AyudaBuscaParametrerShared.ObtenerParameterShare(_doc, "NombreVista");

                int found = _view.ObtenerNombreIsDependencia().IndexOf("(");
                string nombreviewSINPArentesis = _view.Name.Substring(0, found).Trim();

                if (result_NombreVista != null)
                {
                    var _AuxnewFilterRule = ParameterFilterRuleFactory.CreateContainsRule(result_NombreVista, nombreviewSINPArentesis, false);
                    _newListFilterRule.Add(_AuxnewFilterRule);
                }
                else
                {
                    Util.ErrorMsg("No se encontro parametro compartido 'NombreVista'");
                    _newListFilterRule.Clear();
                }
                //2) nombre tipo

                ElementId result_barraTipoa = AyudaBuscaParametrerShared.ObtenerParameterShare(_doc, "BarraTipo");

                if (result_barraTipoa != null)
                {
                    var _AuxnewFilterRule = ParameterFilterRuleFactory.CreateContainsRule(result_barraTipoa, nombreTipo, false);
                    _newListFilterRule.Add(_AuxnewFilterRule);
                }
                else
                {
                    Util.ErrorMsg("No se encontro parametro compartido 'BarraTipo'");
                    _newListFilterRule.Clear();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return null;
            }
            return _newListFilterRule;
        }


    }
}
