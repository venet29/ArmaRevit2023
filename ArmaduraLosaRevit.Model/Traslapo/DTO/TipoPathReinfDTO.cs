using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Traslapo.DTO
{
   public class TipoPathReinfDTO
    {
        public TipoBarra Tipobarra { get; set; }
        public UbicacionLosa Direccion { get; set; }
        public TipoCaraObjeto UbicacionEnLosa { get;  set; }
        public string TipoDireccionBarraVocal { get;  set; }
        public TipoPathReinfDTO(UbicacionLosa Direccion, TipoBarra Tipobarra, TipoCaraObjeto tipoCaraObjeto= TipoCaraObjeto.Inferior)
        {
            this.Tipobarra = Tipobarra;
            this.Direccion = Direccion;
            this.UbicacionEnLosa = tipoCaraObjeto;
        }

        public TipoPathReinfDTO(UbicacionLosa Direccion, TipoBarra Tipobarra, string _TipoDireccionBarra, TipoCaraObjeto tipoCaraObjeto = TipoCaraObjeto.Inferior)
        {
            this.Tipobarra = Tipobarra;
            this.Direccion = Direccion;
            this.UbicacionEnLosa = tipoCaraObjeto;
            this.TipoDireccionBarraVocal = _TipoDireccionBarra;
        }
    }
}
