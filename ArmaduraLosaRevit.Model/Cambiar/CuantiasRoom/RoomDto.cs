using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cambiar.CuantiasRoom
{
     public  class RoomCuantiaDatosDto
    {
        public int dire1 { get; set; }
        public int dire2 { get; set; }

        public int diamH { get; set; }
        public int diamV { get; set; }
        public double espaH { get; set; }
        public double espaV { get; set; }
        public RoomCuantiaDatosDto(int diamH, int diamV, double espaH, double espaV)
        {
            this.diamH = diamH;
            this.diamV = diamV;
            this.espaH = espaH;
            this.espaV = espaV;
        }
    }
}

