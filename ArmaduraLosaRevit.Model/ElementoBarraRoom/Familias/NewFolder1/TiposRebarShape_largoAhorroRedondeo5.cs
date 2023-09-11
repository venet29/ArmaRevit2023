using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.NewFolder1
{
    class TiposRebarShape_largoAhorroRedondeo5
    {
        public static double largo1Izq { get; private set; }
        public static double largo1Dere { get; private set; }

        internal static void ObtenerLargoAhorro5(double LargoPathreiforment , double LargoAhorroIzq, double LargoAhorroDere)
        {
            try
            {
                largo1Izq = 0;
                largo1Dere = 0;

                largo1Izq = LargoPathreiforment - LargoAhorroIzq;
                RedonderLargoBarras.RedondearFoot5_mascercano(largo1Izq);
                largo1Izq = RedonderLargoBarras.NuevoLargobarraFoot;

                largo1Dere = LargoPathreiforment - LargoAhorroDere;
                RedonderLargoBarras.RedondearFoot5_mascercano(largo1Dere);
                largo1Dere = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerLargoAhorro' ex:{ex.Message}");
                return;
            }
        }

        internal static void ObtenerLargoAhorroIZq5(double LargoPathreiforment, double LargoAhorroIzq)
        {
            try
            {
                largo1Izq = 0;
                largo1Dere = 0;

                largo1Izq = LargoPathreiforment - LargoAhorroIzq;
                RedonderLargoBarras.RedondearFoot5_mascercano(largo1Izq);
                largo1Izq = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerLargoAhorro' ex:{ex.Message}");
                return;
            }
        }

        internal static void ObtenerLargoAhorroDer5(double LargoPathreiforment,double LargoAhorroDere)
        {
            try
            {
                largo1Izq = 0;
                largo1Dere = 0;

                largo1Dere = LargoPathreiforment - LargoAhorroDere;
                RedonderLargoBarras.RedondearFoot5_mascercano(largo1Dere);
                largo1Dere = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerLargoAhorro' ex:{ex.Message}");
                return;
            }
        }
    }
}
