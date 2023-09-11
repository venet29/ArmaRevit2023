using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga
{
    public class CasosTraslapoDTO
    {
        public bool IsOK { get;  set; }
        private static int InstanceNH=0;
        public int InstanceNH_B { get; set; }
        public TraslapoEnSeccionNh TraslapoEnSeccionNh_ { get; set; }
        public TipoTraslapoEnSeccionNh TipoTraslapoEnSeccionNh_ { get; set; }
        public BarraFlexionTramosDTO BarraTramosAnterior { get; set; }
        public BarraFlexionTramosDTO BarraTramosPosterior { get; set; }
        public Rebar BarraAnterior { get; set; }
        public Rebar BarrasPosterior { get; set; }
        public CasosTraslapoDTO(TraslapoEnSeccionNh traslapoInicio,
                                TipoTraslapoEnSeccionNh resulta)
        {
            this.TraslapoEnSeccionNh_ = traslapoInicio;
            this.TipoTraslapoEnSeccionNh_ = resulta;
            InstanceNH = InstanceNH + 1;
            InstanceNH_B = InstanceNH;
            IsOK = true;
        }

    }
}
