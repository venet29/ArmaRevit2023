using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.CreaLine
{
   public class ptosLinea
    {
   
        public ptosLinea(double v1, double v2, double v3)
        {
            this.x = v1;
            this.y = v2;
            this.z = v3;
        }

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }
}
