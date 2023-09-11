using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Fauto
{
    

    public class FactoryFautoDTO
    {

        public static FautoDTO ObtenerConfig_F1_Sup() => new FautoDTO("f3", "f1_SUP", "f1_SUP","f3");
        public static FautoDTO ObtenerConfig_F1() => new FautoDTO("f1", "", "","f4");

        public static FautoDTO ObtenerConfig_F1_conAhorro() => new FautoDTO("f21", "", "", "f19");

        public static FautoDTO ObtenerConfig_F1_Sup_conAhorro() => new FautoDTO("f16", "f1_SUP", "f1_SUP", "f16");
    }
     public  class FautoDTO
    {
        public string tipoBarra { get; set; }

        public string tipoBarra_izq_infer { get; set; }
        public string tipoBarra_dere_sup { get; set; }
        public string tipoBarraPataAmbosLados { get;  set; }

        public FautoDTO(string tipoBarra,string tipoBarra_izq_infer, string tipoBarra_dere_sup, string _tipoBarraPataAmbosLados)
        {
            this.tipoBarra = tipoBarra;
            this.tipoBarra_izq_infer = tipoBarra_izq_infer;
            this.tipoBarra_dere_sup = tipoBarra_dere_sup;
            this.tipoBarraPataAmbosLados = _tipoBarraPataAmbosLados;
        }

    }
}
