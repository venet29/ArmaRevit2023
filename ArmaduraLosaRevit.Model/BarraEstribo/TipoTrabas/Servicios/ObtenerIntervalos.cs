using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas.Servicios
{
    public class ObtenerIntervalos
    {
        private ConfiguracionBarraTrabaDTO configuracionBarraTrabaDTO;

        public ObtenerIntervalos(ConfiguracionBarraTrabaDTO configuracionBarraTrabaDTO)
        {
            this.configuracionBarraTrabaDTO = configuracionBarraTrabaDTO;
        }

        public double[] ObtenerSeparacion(int _cantidadLineas)
        {
            double intervalo = 15;
            double[] lista = new double[_cantidadLineas];
            //{0, intervalo, intervalo*2, intervalo*3, intervalo*4, intervalo*5, intervalo*6, intervalo*7, intervalo*8, intervalo*9, intervalo*10, intervalo*11, intervalo*12
            //,intervalo*13,intervalo*14,intervalo*15,intervalo*16,intervalo*17,intervalo*18,intervalo*19,intervalo*20};

            for (int i = 0; i < _cantidadLineas; i++)
            {
                lista[i] = intervalo * (i+1);
            }
            return lista;
        }
    }
}
