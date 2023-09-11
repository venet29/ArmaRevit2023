using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.PathReinf.Ayuda
{
    class ObtenerCasoBarraSimple
    {
        internal static bool IsCaso_f1_f3Suplemuro_f4(string tipoBarra)
        {
            if (tipoBarra.ToLower() == "f1" || tipoBarra.ToLower() == "f3_refuezosuple" || tipoBarra.ToLower() == "4")
                return true;
            else
                return false;
        }

        internal static bool IsCaso_f12(string tipoBarra)
        {
            if (tipoBarra.ToLower() == "f11" || tipoBarra.ToLower() == "f11a" || tipoBarra.ToLower() == "f11b")
                return true;
            else
                return false;
        }
    }
}
