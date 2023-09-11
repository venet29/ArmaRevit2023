using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.DTO
{
    public class Config_EspecialCorte
    {
        public double tolerancia { get; set; }
        public TipoCOnfLargo TipoCOnfigLargo { get; internal set; }
        public TipoCOnfCuantia TipoCOnfigCuantia { get; internal set; }
        public List<ParametroShareNhDTO> ListaPAraShare { get; internal set; }
        public CasoAnalisas TipoCasoAnalisis  { get; internal set; }
        public CrearTrasformadaSobreVectorDesg Trasform_ { get; internal set; }
    }
}
