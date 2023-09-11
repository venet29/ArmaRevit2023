using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.DTO
{
    public class Config_EspecialElev
    {

        public ParametroShareNhDTO tipoBarraElev { get; internal set; }
        public List<ParametroShareNhDTO> ListaPAraShare { get; internal set; }
        public bool IsAgregarId { get; set; }
        public CasoAnalisas TipoCasoAnalisis { get; internal set; }
        public CrearTrasformadaSobreVectorDesg Trasform_ { get; internal set; }
        public XYZ direccionMuevenBarrasFAlsa { get; internal set; }
    }
}
