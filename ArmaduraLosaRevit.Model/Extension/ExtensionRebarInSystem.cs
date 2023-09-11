using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
     public static class ExtensionRebarInSystem
    {

        public static double ObtenerDiametroFoot(this RebarInSystem rebar)
        {
            
            double diamFoot = rebar.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsDouble();
            //if (Util.IsNumeric(diamString))
            //{
            //    diamInt = Util.MmToFoot(Util.ConvertirStringInDouble(diamString));
            //}
            return diamFoot;
        }

        public static int ObtenerDiametroMM(this RebarInSystem rebar)
        {
            double diamFoot = rebar.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsDouble();
            
            return (int)(Math.Round( Util.FootToMm(diamFoot)));
        }


        public static double ObtenerEspaciento_cm(this RebarInSystem rebar)
        {
            double espa = rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_BAR_SPACING).AsDouble();
         
            return Util.FootToCm(espa);
        }
        public static double ObtenerLargoCm(this RebarInSystem rebar) => Util.FootToCm(ObtenerLargoFoot(rebar));
        public static double ObtenerLargoFoot(this RebarInSystem rebar) => rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble();
        public static double ObtenerPeso(this RebarInSystem rebarInSys)
        {
            double peso = 0;
            try
            {
                int diam = rebarInSys.ObtenerDiametroMM();
                double cal1 = (rebarInSys.ObtenerDiametroMM() / 12.73) * (rebarInSys.ObtenerDiametroMM() / 12.73);
                double cal2 = (rebarInSys.ObtenerLargoCm() / 100.0f);
                peso = (rebarInSys.ObtenerDiametroMM() / 12.73) * (rebarInSys.ObtenerDiametroMM() / 12.73) * (rebarInSys.ObtenerLargoCm() / 100.0f) * rebarInSys.Quantity;
            }
            catch (Exception)
            {
                return 0;
            }

            return peso;
        }

    }
}
