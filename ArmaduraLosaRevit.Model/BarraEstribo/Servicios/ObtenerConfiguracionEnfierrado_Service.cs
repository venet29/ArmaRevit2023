using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Servicios
{
    public class ObtenerConfiguracionEnfierrado_Service
    {


        public static TipoEstriboConfig obtener(DatosConfinamientoAutoDTO configuracionInicialEstriboDTO)
        {

            bool isEstribo = ((configuracionInicialEstriboDTO.IsEstribo == null || configuracionInicialEstriboDTO.IsEstribo == false) ? false : true);
            bool isLateral = ((configuracionInicialEstriboDTO.IsLateral == null || configuracionInicialEstriboDTO.IsLateral == false) ? false : true);
            bool isTraba = ((configuracionInicialEstriboDTO.IsTraba == null || configuracionInicialEstriboDTO.IsTraba == false) ? false : true);
            bool isTrabaFalsa = ((configuracionInicialEstriboDTO.IsTrabaFalsa == null || configuracionInicialEstriboDTO.IsTrabaFalsa == false) ? false : true);

            // public enum TipoEstriboConfig { E, *EL, *ET, *ELT, L, *LT, T }

            if (isEstribo == true && isLateral == true && isTraba == true)
            { return TipoEstriboConfig.ELT; }
            else if (isEstribo == true && isLateral == true && isTrabaFalsa == true)
            { return TipoEstriboConfig.EL_TF; }
            else if (isEstribo == true && isLateral == true && isTraba == false)
            { return TipoEstriboConfig.EL; }
          
            else if (isEstribo == true && isLateral == false && isTraba == true)
            { return TipoEstriboConfig.ET; }
            else if (isEstribo == false && isLateral == true && isTraba == true)
            { return TipoEstriboConfig.LT; }
            else if (isEstribo == true && isLateral == false && isTrabaFalsa == true)
            { return TipoEstriboConfig.E_TF; }
            else if (isEstribo == true && isLateral == false && isTraba == false)
            { return TipoEstriboConfig.E; }           
            else if (isEstribo == false && isLateral == true && isTraba == false)
            { return TipoEstriboConfig.L; }
            else //if (isEstribo == false && isLateral == false && isTraba == true)
            { return TipoEstriboConfig.T; }

        }
    }
}
