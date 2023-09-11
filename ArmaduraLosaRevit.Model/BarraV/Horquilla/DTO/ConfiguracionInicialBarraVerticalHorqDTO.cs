using ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
   public class ConfiguracionInicialBarraVerticalHorqDTO
    {
        public bool IsOK { get; set; }
        public int inicial_diametroHorqMM { get; set; } //horq 
        public int inicial_diametroMM { get; set; } //barra

        public string inicial_NumeroHorq { get; set; } //horqu
        public string inicial_Numero { get; set; } //barra 

        public IntervaloBarras_HorqDTO IntervaloBarras_HorqDTO_ { get; set; }
        public Document Document_ { get; internal set; }

        public static ConfiguracionInicialBarraVerticalHorqDTO Obtner()
        {

            return new ConfiguracionInicialBarraVerticalHorqDTO();
        }


        public ConfiguracionInicialBarraVerticalHorqDTO()
        {            
        }

    }
}
