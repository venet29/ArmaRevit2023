
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
    public class AyudaObtenerParametros
    {
        public static string ObtenerNombreView(Element c)
        {
            try
            {
                if (ParameterUtil.FindParaByName(c, "NombreVista") == null) return "sin";
                var name = c.LookupParameter("NombreVista").AsString();
                if (name == null) return "sin";
                if (name == "") return "sin";
                return name;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener Parametro 'NombreVista' ex:{ex.Message}");
                return "sin";
            }
        }
        public static string ObtenerBarraTipo(Element c)
        {
            try
            {
                if (ParameterUtil.FindParaByName(c, "BarraTipo") == null) return "sin";
                var name = c.LookupParameter("BarraTipo").AsString();
                if (name == null) return "sin";
                if (name == "") return "sin";
                return name;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener Parametro 'ObtenerBarraTipo' ex:{ex.Message}");
                return "sin";
            }

        }

        public static string ObtenerIdBarraCopiar(Element c)
        {
            try
            {
                if (ParameterUtil.FindParaByName(c, "IdBarraCopiar") == null) return "sin";
                Parameter parameter = ParameterUtil.FindParaByName(c, "IdBarraCopiar");
                if (null == parameter) return "0";
                string name = parameter.AsValueString();
                if (name == "") return "0";
                return name;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener Parametro 'MetodoIdBarraCopiar' ex:{ex.Message}");
                return "sin";
            }

        }


        public static ObtenerTipoBarra ObtenerTipoBarraRebra(Element _rebar)
        {
            ObtenerTipoBarra _newObtenerTipoBarra = null;
            try
            {
                if (!_rebar.IsValidObject) return null;

                if (_rebar is Rebar || _rebar is RebarInSystem)
                {
                    _newObtenerTipoBarra = new ObtenerTipoBarra(_rebar);
                    if (!_newObtenerTipoBarra.Ejecutar()) return null;
                }
                else if (_rebar is PathReinforcement)
                {
                    _newObtenerTipoBarra = new ObtenerTipoBarra(_rebar);
                    if (!_newObtenerTipoBarra.Ejecutar()) return null;
                    _newObtenerTipoBarra.TipoBarraGeneral = TipoBarraGeneral.Losa;
                }
             //   Debug.WriteLine(_newObtenerTipoBarra.TipoBarraEspecifico.ToString());

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);

            }
            return _newObtenerTipoBarra;
        }

    }
}
