using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
     public class AyudaSeleccionRebar_PathRein
    {
        public static bool EsdireccionIzqDere(Element pt, string parametro, string caso1, string caso2)
        {
            if (pt is PathReinforcement )
            {
                var resul = ParameterUtil.FindParaByName(pt, parametro);
                if (resul == null) return false;
                return resul.AsString() == caso1 || resul.AsString() == caso2;
            }
            else if (pt is Rebar)
            {
                var TipoRebar = ParameterUtil.FindParaByName(pt, "BarraTipo");
                if (TipoRebar == null) return false;
                if (!TipoRebar.AsString().Contains("LOSA_")) return false;

                var resul = ParameterUtil.FindParaByName(pt, parametro);
                if (resul == null) return false;
                return resul.AsString() == caso1 || resul.AsString() == caso2;
            }
            return false;
        }

        public static bool PerteneceAView_Rebar(Element pt,View _view)
        {
            if (pt is Rebar)
            {
                var TipoRebar = ParameterUtil.FindParaByName(pt, "BarraTipo");
                if (TipoRebar == null) return false;
                string aux_tipoBarra = TipoRebar.AsString();
                if (aux_tipoBarra == null)
                {
                    ConstNH.sbLog.AppendLine($"------ rebar :{pt.Id.IntegerValue}  barras sin tipo:'BarraTipo' ");
                    return false;
                }

                if (!aux_tipoBarra.Contains("LOSA_")) return false;

                var result = ParameterUtil.FindParaByName(pt, "NombreVista");
                if (result == null) return false;
                return result.AsString() == _view.ObtenerNombreIsDependencia();
            }
            return false;
        }


        public static bool PerteneceAView_TodasRebar_MenosSystem(Element pt, View _view)
        {
            if (pt is Rebar)
            {
                var result = ParameterUtil.FindParaByName(pt, "NombreVista");
                if (result == null) return false;
                return result.AsString() == _view.ObtenerNombreIsDependencia();
            }
            return false;
        }

        public static bool PerteneceAView_PathAndRebarInSystem(Element pt,View _view, bool _isConsiderarTodasLasElevaciones)
        {
            if (_isConsiderarTodasLasElevaciones) return true;
            if (pt is RebarInSystem || pt is PathReinforcement)
            {
                var result = ParameterUtil.FindParaByName(pt, "NombreVista");
                if (result == null) return false;
                return result.AsString() == _view.ObtenerNombreIsDependencia();
            }
    
            return false;
        }

    }
}
