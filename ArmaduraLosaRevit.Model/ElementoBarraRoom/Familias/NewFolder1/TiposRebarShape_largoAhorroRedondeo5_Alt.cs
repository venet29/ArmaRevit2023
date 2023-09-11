using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.NewFolder1
{
    class TiposRebarShape_largoAhorroRedondeo5_Alt
    {
        public static double largo5_AltIzq { get; private set; }
        public static double largo5_AltDere { get; private set; }

        internal static void ObtenerLargoAhorro5_Alt(double LargoPathreiforment , double LargoAhorroIzq, double LargoAhorroDere)
        {
            try
            {
                largo5_AltIzq = 0;
                largo5_AltDere = 0;

                largo5_AltIzq = LargoPathreiforment - LargoAhorroIzq;
                RedonderLargoBarras.RedondearFoot5_AltMascercano(largo5_AltIzq);
                largo5_AltIzq = RedonderLargoBarras.NuevoLargobarraFoot;

                largo5_AltDere = LargoPathreiforment - LargoAhorroDere;
                RedonderLargoBarras.RedondearFoot5_AltMascercano(largo5_AltDere);
                largo5_AltDere = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerLargoAhorro' ex:{ex.Message}");
                return;
            }
        }

        internal static void ObtenerLargoAhorroIZq5_Alt(double LargoPathreiforment, double LargoAhorroIzq)
        {
            try
            {
                largo5_AltIzq = 0;
                largo5_AltDere = 0;

                largo5_AltIzq = LargoPathreiforment - LargoAhorroIzq;
                RedonderLargoBarras.RedondearFoot5_mascercano(largo5_AltIzq);
                largo5_AltIzq = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerLargoAhorro' ex:{ex.Message}");
                return;
            }
        }

        internal static void ObtenerLargoAhorroDer5_Alt(double LargoPathreiforment,double LargoAhorroDere)
        {
            try
            {
                largo5_AltIzq = 0;
                largo5_AltDere = 0;

                largo5_AltDere = LargoPathreiforment - LargoAhorroDere;
                RedonderLargoBarras.RedondearFoot5_mascercano(largo5_AltDere);
                largo5_AltDere = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerLargoAhorro' ex:{ex.Message}");
                return;
            }
        }
    }
}
