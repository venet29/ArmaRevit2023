using ArmaduraLosaRevit.Model.BarraEstribo.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas
{
    public interface ITipoTraba
    {
    
        List<BarraTrabaDTO> ObtenerListaEspacimientoTrabas();
    }
}
