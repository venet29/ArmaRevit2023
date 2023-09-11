using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Traslapos
{
    public class AyudaManejadorTraslapo
    {
        public static bool IsInicialIntervalo { get; set; } = false;


        public static Rebar UtilmaBarra { get; set; }


        public static bool IsContinudadDebarra(Rebar nuevaBarras)
        {
            if (!IsInicialIntervalo) return false;
            if (UtilmaBarra == null) return false;

            XYZ ptUltimoInicial = UtilmaBarra.ObtenerInicioCurvaMasLarga();
            XYZ ptUltimoFinal = UtilmaBarra.ObtenerFinCurvaMasLarga();
            double diamtroUltimo = UtilmaBarra.ObtenerDiametroFoot();

            XYZ ptInicial = nuevaBarras.ObtenerInicioCurvaMasLarga();
            double diamtroInicio = nuevaBarras.ObtenerDiametroFoot();

            if (ptUltimoFinal.GetXY0().DistanceTo(ptInicial.GetXY0()) > Math.Max(diamtroInicio, diamtroUltimo)) return false;

            if(!(ptUltimoFinal.Z> ptInicial.Z && ptInicial.Z> ptUltimoInicial.Z)) return false;

            return true;
        
        }

    }
}
