using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstriboP.DTO
{
    public class DatosBarraAutoDTO
    {

    
        public XYZnh PtoBordeMuro { get; set; }
        public XYZnh PtoCentralSobreMuro { get; set; }

        public int Inicial_diametroMM { get; set; }
        public int Inicial_Cantidadbarra { get; set; }

        public double EspaciamientoREspectoBordeFoot { get; set; }
    }
}
