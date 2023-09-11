using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    public class DatosRefuerzoCabezaMuroDTO
    {
        internal string tipobarra;

        public int diamtroBarraRefuerzo_MM { get; set; }
        public int CantidadBarras { get; set; }
        public int diamtroBarraS1_MM { get; set; }
        public int espacimientoS1_Cm { get; set; }

        public EmpotramientoPatasLosaDTO _empotramientoPatasDTO { get; set; }

        public bool  IsSuple { get; set; }
        public bool IsUsar2Pto { get;  set; } // se dibuja con los ptos del mouse, evita que se agrgue largodedesarollo
        public bool IsBArras { get;  set; }
        public TipoRefuerzoLOSA _tipoRefuerzoLOSA { get; set; } = TipoRefuerzoLOSA.losa;

    }
}
