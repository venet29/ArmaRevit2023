using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura.DTO
{
     public class DiametrosBarrasDTO
    {
        public int diametro_mm { get; set; }
        public int Standard_Bend_Diameter_mm { get; set; }
        public int Standard_Hook_Bend_Diameter_mm { get; set; }
        public int StirrupTie_Bend_Diameter_mm { get; set; }

        public DiametrosBarrasDTO(int diamtro,int factor)
        {
            this.diametro_mm = diamtro;

            this.Standard_Bend_Diameter_mm = diamtro* factor;
            this.Standard_Hook_Bend_Diameter_mm = diamtro* factor;
            this.StirrupTie_Bend_Diameter_mm = diamtro* factor;


        }
    }
}
