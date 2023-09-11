namespace ArmaduraLosaRevit.Model.Dimensiones.DTO

{
    public class DimensionesDatosTextoDTO
    {
        public string Above { get; set; } = "textoAbovoe";
        public string Below { get; set; } = "textobelow";
        public string ValueOverride { get; set; } = "replaceWithText";
        public bool IsSobreEscribir { get; set; } = false;
    }


}
