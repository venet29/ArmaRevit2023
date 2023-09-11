using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas.Tipos
{
    internal class TrabaMuroNull : ITipoTraba
    {

        public TrabaMuroNull()
        {

        }


        public List<BarraTrabaDTO> ObtenerListaEspacimientoTrabas()
        {
            return new List<BarraTrabaDTO>();
        }
    }
}