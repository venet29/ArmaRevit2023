using System;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Calculo
{
    class ObtenerDesplazaminetoEnSentidoBarras
    {
        internal static XYZ Ejecutar(XYZ posicionAnteriorGuardada, IndependentTag indtag, CoordenadaPath coordenadaPath)
        {
   
            try
            {
                XYZ tagHeadPosition= indtag.TagHeadPosition;
                XYZ direc = coordenadaPath.p3 - coordenadaPath.p2;

                var lineaux = Line.CreateBound(tagHeadPosition + direc * 2, tagHeadPosition - direc * 2);

                XYZ ptoInter = lineaux.ProjectExtendidaXY0(posicionAnteriorGuardada);

               return ptoInter - tagHeadPosition;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg($"Error al obtener posicon de tag {indtag.Name}");
                return indtag.TagHeadPosition;
            }
        }
    }
}