using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda
{
    public class AyudaTraslapoIguales
    {

        public static bool SonIguales(BarraFlexionTramosDTO barraFlexionTramosDTO_IZq, BarraFlexionTramosDTO barraFlexionTramosDTO_Dere)
        {
            //barraFlexionTramosDTO_IZq.TraslapoFinTramo == null  --->  este caso no se puede dar porque se previene antes
            return true;
            if (barraFlexionTramosDTO_Dere.CasosTraslapoDTO_InicioTramo == null) return false;

            if (barraFlexionTramosDTO_IZq.CasosTraslapoDTO_FinTramo.BarraTramosPosterior.IdentiFIcadorParaTraslapo == barraFlexionTramosDTO_Dere.CasosTraslapoDTO_InicioTramo.BarraTramosAnterior.IdentiFIcadorParaTraslapo &&
                barraFlexionTramosDTO_IZq.CasosTraslapoDTO_FinTramo.BarraTramosPosterior.p2_mm.GetXYZ_cmTofoot().DistanceTo(barraFlexionTramosDTO_Dere.CasosTraslapoDTO_InicioTramo.BarraTramosAnterior.p1_mm.GetXYZ_cmTofoot())<1)
                return true;
            else
                return false;

        }

        internal static bool EstanTraslapadas(BarraFlexionTramosDTO barraFlexionTramosDTO_actual, BarraFlexionTramosDTO barraFlexionTramosDTO_iteracion,double largoTraslapo_mm)
        {
            if (barraFlexionTramosDTO_actual.p1_mm.X == 0)
                return barraFlexionTramosDTO_actual.p1_mm.Y - largoTraslapo_mm < barraFlexionTramosDTO_iteracion.p1_mm.Y - largoTraslapo_mm && barraFlexionTramosDTO_iteracion.p1_mm.Y - largoTraslapo_mm < barraFlexionTramosDTO_actual.p2_mm.Y + largoTraslapo_mm;
            else
                return barraFlexionTramosDTO_actual.p1_mm.X - largoTraslapo_mm < barraFlexionTramosDTO_iteracion.p1_mm.X - largoTraslapo_mm && barraFlexionTramosDTO_iteracion.p1_mm.X - largoTraslapo_mm < barraFlexionTramosDTO_actual.p2_mm.X + largoTraslapo_mm;
        }
    }
}
