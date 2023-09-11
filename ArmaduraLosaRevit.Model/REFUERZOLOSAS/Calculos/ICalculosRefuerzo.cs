using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos
{
   public interface ICalculosRefuerzo
    {
        DatosRefuerzoTipoVigaDTO _datosRefuerzoTipoViga { get; set; }
        int NumeroBArras { get; set; }
        List<CalculoBarraRefuerzo> ListaBArrasSuperior { get; set; }
         List<CalculoBarraRefuerzo> ListaBArrasInferior { get; set; }
         List<EstriboRefuerzoDTO> ListaEstriboRefuerzoDTO { get; set; }
    }
}
