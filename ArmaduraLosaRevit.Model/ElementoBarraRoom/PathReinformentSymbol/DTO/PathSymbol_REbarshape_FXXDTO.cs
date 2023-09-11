namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO
{
    public partial class PathSymbol_REbarshape_FxxDTO
    {
        public bool IsOK { get; set; }
        public double pataIzq_foot { get; set; }
        public double pataDere_foot { get; set; }

        public double DesDereSup_foot { get; set; }
        public double DesDereInf_foot { get; set; }

        public double DesIzqSup_foot { get; set; }
        public double DesIzqInf_foot { get; set; }
        public bool CopiarFamiliasDiferentesPatas { get; set; }

  

        public PathSymbol_REbarshape_FxxDTO()
        {
            IsOK = false;
            CopiarFamiliasDiferentesPatas = false;
        }

    }

    //solo para caso f11 de pata de fundaciones
    public partial class PathSymbol_REbarshape_FxxDTO
    {

        //espesor
        public double EspIzq_foot { get; set; }
        public double EspDere_foot { get; set; }
    }

}
