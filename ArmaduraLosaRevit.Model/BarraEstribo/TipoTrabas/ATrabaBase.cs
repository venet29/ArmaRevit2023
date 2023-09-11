using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas
{
    public abstract class ATrabaBase
    {

        protected DireccionTraba _direccionTraba;
        protected readonly int _cantidadLineas;
        protected ConfiguracionBarraTrabaDTO _configuracionBarraTrabaDTO;
        protected double _espesor;
        public int cantidadTrabasTrasversales { get; set; }

        public int DiametroTrabasTrasversales_mm { get; set; }
        public int cantidadTrabasLongitudinales { get; set; }
        public int DiametroTrabasLongitudinales_mm { get; set; }
        public TipoTraba_posicion TipoTraba_Posicion { get; }
        public List<BarraTrabaDTO> ListaTrabaDTO { get; set; }

        public ATrabaBase(ConfiguracionBarraTrabaDTO _configuracionBarraTrabaDTO)
        {
            this._espesor = _configuracionBarraTrabaDTO.EspesroMuroOVigaFoot;
            this._direccionTraba = _configuracionBarraTrabaDTO.UbicacionTraba;
            this._cantidadLineas = _configuracionBarraTrabaDTO.CantidadTrabasTranversal;
            this.cantidadTrabasTrasversales=_configuracionBarraTrabaDTO.CantidadTrabasTranversal;
            this.cantidadTrabasLongitudinales = _configuracionBarraTrabaDTO.CantidadTrabasLongitudinal;
            this.ListaTrabaDTO = new List<BarraTrabaDTO>();
            this.DiametroTrabasLongitudinales_mm = _configuracionBarraTrabaDTO.DiamtroTrabaEstriboMM;
            this.TipoTraba_Posicion = _configuracionBarraTrabaDTO.tipoTraba_posicion;
            this.DiametroTrabasTrasversales_mm = _configuracionBarraTrabaDTO.DiamtroTrabaEstriboMM;
            this._configuracionBarraTrabaDTO = _configuracionBarraTrabaDTO;
        }
        

    }
}
