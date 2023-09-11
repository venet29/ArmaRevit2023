using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ParametrosShare.Actualizar.Model
{
    public class CambirSOLOTipoBarraDTO
    {
        public string valor_inicial { get; set; } = "";
        public List<string> SaltarSiesIgual { get; set; } = new List<string>();   
        public List<string> ContinuarSiesIgual { get; set; } = new List<string>();

    }
}
