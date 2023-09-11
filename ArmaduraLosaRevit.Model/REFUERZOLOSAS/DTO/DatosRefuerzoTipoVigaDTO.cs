using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    public class DatosRefuerzoTipoVigaDTO
    {

        public string tipobarra { get; set; }
        public TipoBarraRefuerzo TipoBarra { get;  set; }
        public int diamtroBarraRefuerzo_MM { get; set; }
        public int CantidadBarras { get; set; }
        public int diamtroEstribo_MM { get; set; }
        public int espacimientoEstribo_Cm { get; set; }
        public EmpotramientoPatasLosaDTO _empotramientoPatasDTO { get; set; }
        public bool IsEstribo { get; internal set; }
        public bool IsUsarLinea { get; internal set; }
        public bool IsBuscarPatas { get; internal set; }
        public string tipoPosicionRef { get; internal set; }
        public bool IsBArras { get; internal set; }
        public TipoRefuerzoLOSA _tipoRefuerzoLOSA { get; set; } = TipoRefuerzoLOSA.losa;
    }
}
