using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda
{
    class ObtenerPuntoIntermedio
    {
        internal static XYZnh ObtenerPtoFinal_XYZNH(BarraFlexion barraFlexionTramos_, Tabla03_Info_Traslapos_Vigas traslapo_)
        {
            double L = barraFlexionTramos_.BarraFlexionTramosDTO_.diametro_Barras__mm;
            XYZ resultado = default;
            XYZ p1 = barraFlexionTramos_.BarraFlexionTramosDTO_.p1_mm.GetXYZ();
            XYZ p2 = barraFlexionTramos_.BarraFlexionTramosDTO_.p2_mm.GetXYZ();
            XYZ DireccionP2_p1 = (p2 - p1).Normalize();
            double distanci = p2.DistanceTo(p1);
            if (traslapo_.Traslapo_1 != "0")
            {
                resultado = p1 + DireccionP2_p1 * distanci / 3.0d;
            }
            else if (traslapo_.Traslapo_2 != "0")
            {
                resultado = p1 + DireccionP2_p1 * distanci / 2.0d;
            }
            else if (traslapo_.Traslapo_3 != "0")
            {
                resultado = p1 + DireccionP2_p1 * distanci * 3.0d / 4.0d;
            }

            return resultado.GEtXYZnh();
        }

        internal static XYZ ObtenerPtoFinal_REvit(BarraFlexion barraFlexionTramos_, Tabla03_Info_Traslapos_Vigas traslapo_)
        {
            throw new NotImplementedException();
        }

        internal static XYZnh ObtenerPtoInicial_XYZNH(BarraFlexion barraFlexionTramos_, Tabla03_Info_Traslapos_Vigas traslapo_)
        {
            throw new NotImplementedException();
        }

        internal static XYZ ObtenerPtoIncial_REvit(BarraFlexion barraFlexionTramos_, Tabla03_Info_Traslapos_Vigas traslapo_)
        {
            throw new NotImplementedException();
        }
    }
}
