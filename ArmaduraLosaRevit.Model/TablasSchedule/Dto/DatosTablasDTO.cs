using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Dto
{
    public class DatosTablasDTO
    {

        public double OrdeElevacion { get; set; }
        public string nivel { get; set; }
        public double area { get; set; }
        public double vol { get; set; }



        public object[] ObtenerDato_array()
        {
          
            if(Util.IsNumeric(nivel))
                return new object[] {
                    OrdeElevacion,
               nivel+".0",
               area,
               vol
                    };
            else
                return new object[] {
                    OrdeElevacion,
               nivel,
               area,
               vol
                    };


        }
    }
}

