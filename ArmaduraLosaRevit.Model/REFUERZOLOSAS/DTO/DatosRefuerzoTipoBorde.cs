using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    public class DatosRefuerzoTipoBorde
    {
#pragma warning disable CS0649 // Field 'DatosRefuerzoTipoBorde.tipobarra' is never assigned to, and will always have its default value null
        internal string tipobarra;
#pragma warning restore CS0649 // Field 'DatosRefuerzoTipoBorde.tipobarra' is never assigned to, and will always have its default value null

        public int diamtroBarraRefuerzo_MM { get; set; }
        public int CantidadBarras { get; set; }
        public int diamtroEstribo_MM { get; set; }
        public int espacimientoEstribo_Cm { get; set; }

        public EmpotramientoPatasLosaDTO _empotramientoPatasDTO { get; set; }
        public bool IsEstribo { get;  set; }

        public TipoRefuerzoLOSA _tipoRefuerzoLOSA { get; set; } = TipoRefuerzoLOSA.losa;
        public TipoSeleccionPtosBordeLosa TipoSeleccionPtos { get; internal set; }
        public bool IsIntervalos { get; internal set; }
    }
}
