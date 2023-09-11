using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using ArmaduraLosaRevit.Model.ViewFilter.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter.Rules
{
    class CreadorFilter3DRevison
    {
        // a)
        internal static List<FilterRule> A_ObtenerRegla_Sintipo(Document _doc, List<ParametrosFiltro> ListaPArametros)
        {
            List<FilterRule> _newListFilterRule = new List<FilterRule>();
            try
            {
                foreach (ParametrosFiltro item in ListaPArametros)
                {
                    ElementId result = AyudaBuscaParametrerShared.ObtenerParameterShare(_doc, item.Nombrefilfro);
                    if (result != null)
                    {
                        var _newFilterRule = ParameterFilterRuleFactory.CreateNotEqualsRule(result, item.Valorfilfro, false);
                        _newListFilterRule.Add(_newFilterRule);
                    }
                    else
                    {
                        Util.ErrorMsg($"No se encontro parametro compartido '{item.Nombrefilfro}'");
                        _newListFilterRule.Clear();
                    }
                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return null;
            }
            return _newListFilterRule;
        }
        internal static List<FilterRule> A_ObtenerRegla_parameterShare(Document _doc, List<ParametrosFiltro> ListaPArametros)
        {
            List<FilterRule> _newListFilterRule = new List<FilterRule>();
            try
            {
                foreach (ParametrosFiltro item in ListaPArametros)
                {
                    ElementId result = AyudaBuscaParametrerShared.ObtenerParameterShare(_doc, item.Nombrefilfro);
                    if (result != null)
                    {
                        FilterRule _newFilterRule = default;
                        if (item.TipoPArametroso == TipoParametros.texto)
                            _newFilterRule = item.reglafiltroSTR(result, item.Valorfilfro, false);
                        else if (item.TipoPArametroso == TipoParametros.numero)
                            _newFilterRule = item.reglafiltroDOUBLE(result, Util.ConvertirStringInDouble(item.Valorfilfro), 0.000001D);

                        _newListFilterRule.Add(_newFilterRule);
                    }
                    else
                    {
                        Util.ErrorMsg($"No se encontro parametro compartido '{item.Nombrefilfro}'");
                        _newListFilterRule.Clear();
                    }
                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return null;
            }
            return _newListFilterRule;
        }
        internal static List<FilterRule> A_ObtenerRegla_parameterDiametro(Document _doc, List<ParametrosFiltro> ListaPArametros)
        {
            List<FilterRule> _newListFilterRule = new List<FilterRule>();
            try
            {
                foreach (ParametrosFiltro item in ListaPArametros)
                {
                    int dimm = Util.ConvertirStringInInteger(item.Valorfilfro.Replace(".0 mm", "").Trim());
                    RebarBarType _rebarBarTypeH = TiposRebarBarType.getRebarBarType("Ø" + dimm, _doc, true);
                    var resultPara = ParameterUtil.FindParaByName(_rebarBarTypeH.Parameters, "Bar Diameter");
                    //ElementId result = _rebarBarTypeH.get_Parameter(BuiltInParameter.re).Id;

                    if (resultPara != null)
                    {
                        ElementId result = resultPara.Id;
                         FilterRule _newFilterRule = default;
                        if (item.TipoPArametroso == TipoParametros.texto)
                            _newFilterRule = item.reglafiltroSTR(result, item.Valorfilfro, false);
                        else if (item.TipoPArametroso == TipoParametros.numero)
                            _newFilterRule = item.reglafiltroDOUBLE(result, Util.MmToFoot(dimm), 0.000000001);
                        //_newFilterRule = item.reglafiltroSTR(result, item.Valorfilfro.Replace(" mm", ""), false);
                        _newListFilterRule.Add(_newFilterRule);
                    }
                    else
                    {
                        Util.ErrorMsg($"No se encontro parametro compartido '{item.Nombrefilfro}'");
                        _newListFilterRule.Clear();
                    }
                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return null;
            }
            return _newListFilterRule;
        }

        internal static List<FilterRule> A_ObtenerRegla_parameterDiametroColor(Document _doc, List<ParametrosFiltro> ListaPArametros)
        {
            List<FilterRule> _newListFilterRule = new List<FilterRule>();
            try
            {
                foreach (ParametrosFiltro item in ListaPArametros)
                {
                    int dimm = Util.ConvertirStringInInteger(item.Valorfilfro.Replace(".0 mm", "").Trim());
                    RebarBarType _rebarBarTypeH = TiposRebarBarType.getRebarBarType("Ø" + dimm, _doc, true);
                    var resultPara = ParameterUtil.FindParaByName(_rebarBarTypeH.Parameters, "Bar Diameter");
                    //ElementId result = _rebarBarTypeH.get_Parameter(BuiltInParameter.re).Id;

                    if (resultPara != null)
                    {
                        ElementId result = resultPara.Id;
                        FilterRule _newFilterRule = default;
                        if (item.TipoPArametroso == TipoParametros.texto)
                            _newFilterRule = item.reglafiltroSTR(result, item.Valorfilfro, false);
                        else if (item.TipoPArametroso == TipoParametros.numero)
                            _newFilterRule = item.reglafiltroDOUBLE(result, Util.MmToFoot(dimm), 0.000000001);
                        //_newFilterRule = item.reglafiltroSTR(result, item.Valorfilfro.Replace(" mm", ""), false);
                        _newListFilterRule.Add(_newFilterRule);
                    }
                    else
                    {
                        Util.ErrorMsg($"No se encontro parametro compartido '{item.Nombrefilfro}'");
                        _newListFilterRule.Clear();
                    }
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
