using System;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Ayuda
{
    internal class AyudaObtenerLargoAhorroF16
    {
        internal static double ObtenerAhorroDerecho_Foot(ManejadorEditarREbarShapYPAthSymbol_DTO pathReinforcementSymbol)
        {
            try
            { 
                return pathReinforcementSymbol.PathReinforcement.ObtenerLargoOffSet(); 
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return -1;
            }
        }


        internal static double ObtenerAhorroIzq(ManejadorEditarREbarShapYPAthSymbol_DTO pathReinforcementSymbol)
        {
            double largoAhoro = 0;
            try
            {
                var largoPrimario = pathReinforcementSymbol.PathReinforcement.ObtenerLargoFoot_SinPAtas();
                var largoAlter = pathReinforcementSymbol.PathReinforcement.ObtenerLargoAlternativo();
                var largoOfset = pathReinforcementSymbol.PathReinforcement.ObtenerLargoOffSet();
                largoAhoro = (largoAlter + largoOfset) - largoPrimario;

                return largoAhoro;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return -1;
            }
        }
    }
}