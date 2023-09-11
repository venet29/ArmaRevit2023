namespace ArmaduraLosaRevit.Model.Enumeraciones
{
    public class VariablesSistemasDTO
    {
        public VariablesSistemasDTO()
        {
        }

        public bool IsConAhorro { get; set; }
        public bool IsDibujarS4 { get; set; }
        public bool IsVerificarEspesor { get; set; }
        public double LargoBarras_cm { get; set; }
        public double LargoRecorrido_cm { get; set; }
        public string tipoPorF1 { get; set; }
        public string tipoPorF3 { get; set; }
        public string tipoPorF4 { get; set; }
        public bool IsAjusteBarra_Recorrido { get; set; }
        public bool IsAjusteBarra_Largo { get; set; }
        public bool IsReSeleccionarPuntoRango { get;  set; }
    }
}