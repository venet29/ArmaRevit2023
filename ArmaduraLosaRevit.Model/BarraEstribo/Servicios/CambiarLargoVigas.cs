using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Servicios
{

    // clase para redefinir el largo del estribo segun su largo de estribo  y el agro de porcentaje
    public class CambiarLargoVigas
    {
        public static GenerarDatosIniciales_Service Recal_CambiarLargoVigas { get; internal set; }
        private static GenerarDatosIniciales_Service Inicial_CambiarLargoVigas;
        public static bool Ejecutar(GenerarDatosIniciales_Service generarDatosIniciales_Service, BarraCorteTramos barraCorteDTO)
        {
            Inicial_CambiarLargoVigas = (GenerarDatosIniciales_Service)generarDatosIniciales_Service.Clone();

            Recal_CambiarLargoVigas = generarDatosIniciales_Service;

            double posiInicial = barraCorteDTO.Posi_Inicial_Porcentaje;
            double posiFinal = barraCorteDTO.Posi_Final_Porcentaje;

            try
            {
                if (barraCorteDTO.LargoEstriboEnbarra == LargoEstriboEnbarra.Completa)
                {

                    return true;
                }
                else if (barraCorteDTO.LargoEstriboEnbarra == LargoEstriboEnbarra.Inicial)
                {
                   
                }
                else if (barraCorteDTO.LargoEstriboEnbarra == LargoEstriboEnbarra.Centrol)
                {

                }
                else if (barraCorteDTO.LargoEstriboEnbarra == LargoEstriboEnbarra.Final)
                {

                }
            }
            catch (Exception ex)
            {
                Recal_CambiarLargoVigas = Inicial_CambiarLargoVigas;
                Util.ErrorMsg($"Error al recalcular las dimensiones de viga id:{generarDatosIniciales_Service._SeleccionPtosEstriboViga_sinSeleccionBarras._ElemetSelect.Id}.\n ex:{ex.Message} ");
                return false;
            }
            return true;
        }
    }
}
