using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;

namespace ArmaduraLosaRevit.Model.Traslapo.Calculos
{
    public interface iCalculoDatosParaReinforment
    {
        CoordenadaPath _coordCalculos { get; set; }
        ContenedorDatosPathReinformeDTO datosNuevoPathIzqAbajoDTO { get; set; }
         ContenedorDatosPathReinformeDTO datosNuevoPathDereArribaDTO { get; set; }
        bool IsOK { get; set; }

        bool M2_IsPuedoDibujarPath();
        bool M1_Obtener2PathReinformeTraslapoDatos();
    }
}
