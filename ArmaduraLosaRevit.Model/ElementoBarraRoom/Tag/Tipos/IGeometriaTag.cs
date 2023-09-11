using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag
{
    public interface IGeometriaTag
    {
        List<TagBarra> listaTag { get; set; }
    
        bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad);
        void M2_CAlcularPtosDeTAg(bool IsGarficarEnForm = false);

        void M3_DefinirRebarShape();
        bool M4_IsFAmiliaValida();
        ///bool Intercambiar_F_L();

       // void Ejecutar(double AnguloRadian);
        bool Ejecutar(GeomeTagArgs args);

    }

}
