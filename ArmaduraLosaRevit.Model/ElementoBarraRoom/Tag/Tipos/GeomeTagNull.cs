using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    internal class GeomeTagNull : IGeometriaTag
    {
        public List<TagBarra> listaTag { get; set; }
        public GeomeTagNull()
        {

        }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagNull  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M2_CAlcularPtosDeTAg(bool IsGarficarEnForm = false)
        {
            listaTag = new List<TagBarra>();
        }

        public bool M1_ObtnerPtosInicialYFinalDeBarra(double anguloRoomRad)
        {
            return true;
        }

        public void M3_DefinirRebarShape()
        {

        }

        public bool M4_IsFAmiliaValida() => false;
    }
}