using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas
{
    public  class FactoryTipoTrabas
    {
        public static ITipoTraba ObtenerTipoTrabas(ConfiguracionBarraTrabaDTO _configuracionBarraTrabaDTO)
        {
            double espesorMuroOviga = Math.Round( Util.FootToCm(_configuracionBarraTrabaDTO.EspesroMuroOVigaFoot));

            switch (espesorMuroOviga)
            {
                case 14:
                case 15:
                    return new TrabaMuroEspesor15_20_25(_configuracionBarraTrabaDTO);
                case 18:
                    return new TrabaMuroEspesor15_20_25(_configuracionBarraTrabaDTO);
                case 20:
                    return new TrabaMuroEspesor15_20_25(_configuracionBarraTrabaDTO);
                case 25:
                    return new TrabaMuroEspesor15_20_25(_configuracionBarraTrabaDTO);
                case 30:
                    return new TrabaMuroEspesor30(_configuracionBarraTrabaDTO);
                case 35:
                    return new TrabaMuroEspesor30(_configuracionBarraTrabaDTO);
                case 40:
                    return new TrabaMuroEspesor30(_configuracionBarraTrabaDTO);
                case 45:
                    return new TrabaMuroEspesor30(_configuracionBarraTrabaDTO);
                case 50:
                    return new TrabaMuroEspesor30(_configuracionBarraTrabaDTO);
                case 55:
                    return new TrabaMuroEspesor30(_configuracionBarraTrabaDTO);
                default:
                    //NOTA :se debe implemetar para otros espesores
                    Util.ErrorMsg($"NO es implementado las trabas para muro con espesor :{espesorMuroOviga} ");
                    return new TrabaMuroNull();

            }
        }
    }
}
