using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Traslapos.ayuda
{
    public class AyudaManejadorTraslapo
    {
        public static bool IsInicialIntervalo { get; set; } = false;


        public static Rebar UltimaBarra { get; set; }
        internal static void Reset()
        {
            UltimaBarra = default;
            IsInicialIntervalo = false;
        }

        public static bool IsContinudadDebarra( Rebar nuevaBarras)
        {

            XYZ ptUltimoInicial = default;
            XYZ ptUltimoFinal = default;
            double diamtroUltimo = default;
            XYZ ptInicial = default;
            double diamtroInicio = default;
            try
            {
                if (!IsInicialIntervalo) return false;
                if (!UltimaBarra.IsValidObject) return false;
                if (UltimaBarra == null) return false;
    
 
                ptUltimoInicial = UltimaBarra.ObtenerInicioCurvaMasLarga();
                ptUltimoFinal = UltimaBarra.ObtenerFinCurvaMasLarga();
                diamtroUltimo = UltimaBarra.ObtenerDiametroFoot();

                ptInicial = nuevaBarras.ObtenerInicioCurvaMasLarga();
                diamtroInicio = nuevaBarras.ObtenerDiametroFoot();

                if (ptUltimoFinal.GetXY0().DistanceTo(ptInicial.GetXY0()) > Math.Max(diamtroInicio, diamtroUltimo) && (ptInicial.Z>= ptUltimoFinal.Z)) return false;

                if (!(ptUltimoFinal.Z > ptInicial.Z && ptInicial.Z > ptUltimoInicial.Z)) return false;
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
