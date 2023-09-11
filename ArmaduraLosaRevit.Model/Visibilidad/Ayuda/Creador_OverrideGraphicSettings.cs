using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Ayuda
{
    class Creador_OverrideGraphicSettings
    {
        internal static OverrideGraphicSettings ObtenerOverrideGraphicSettings(GraphicSettingDTO encontrado)
        {

            if (encontrado == null) return null;
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();


            if (encontrado.halftone != null)
                ogs.SetHalftone((bool)encontrado.halftone);

            if (encontrado.detailLevel != ViewDetailLevel.Undefined)
                ogs.SetDetailLevel(encontrado.detailLevel);
            //1)project lines
            if (encontrado.IsEdit_ProjectionLine)
            {
                if (encontrado.ProjectionLinePatternId != null)
                    ogs.SetProjectionLinePatternId(encontrado.ProjectionLinePatternId);

                if (encontrado.ProjectionLineColor != null)
                    ogs.SetProjectionLineColor(encontrado.ProjectionLineColor);

                if (encontrado.ProjectionLineWeight > 0)
                    ogs.SetProjectionLineWeight(encontrado.ProjectionLineWeight);
            }
            //2)SurfacePattern
            if (encontrado.IsEdit_SurfacePattern)
            {
                //a) Foreground
                if (encontrado.SurfaceForegroundPatternVisible != null)
                    ogs.SetSurfaceForegroundPatternVisible((bool)encontrado.SurfaceForegroundPatternVisible);
                if (encontrado.SurfaceForegroundPatternColor != null)
                    ogs.SetSurfaceForegroundPatternColor(encontrado.SurfaceForegroundPatternColor);

                if (encontrado.SurfaceForegroundPatternId != null)
                    ogs.SetSurfaceForegroundPatternId(encontrado.SurfaceForegroundPatternId);

                //b)Background
                if (encontrado.SurfaceBackgroundPatternVisible != null)
                    ogs.SetSurfaceBackgroundPatternVisible((bool)encontrado.SurfaceBackgroundPatternVisible);

                if (encontrado.SurfaceBackgroundPatternColor != null)
                    ogs.SetSurfaceBackgroundPatternColor(encontrado.SurfaceBackgroundPatternColor);

                if (encontrado.SurfaceBackgroundPatternId != null)
                    ogs.SetSurfaceBackgroundPatternId(encontrado.SurfaceBackgroundPatternId);
            }
            // 3) trasparence 
            if (encontrado.SurfaceTransparency > 0)
                ogs.SetSurfaceTransparency(encontrado.SurfaceTransparency);

            //4)cutline
            if (encontrado.IsEdit_CutLine)
            {
                if (encontrado.CutLineColor != null)
                    ogs.SetCutLineColor(encontrado.CutLineColor);

                if (encontrado.CutLinePattern != null)
                    ogs.SetCutLinePatternId(encontrado.CutLinePattern.Id);

                if (encontrado.CutLineWeight > 0)
                    ogs.SetCutLineWeight(encontrado.CutLineWeight);
            }
            //5) cut Patterns

            if (encontrado.IsEdit_CutPatterns)
            {
                //a) Foreground
                if (encontrado.CutForegroundPatternVisible!=null)
                    ogs.SetCutForegroundPatternVisible((bool)encontrado.CutForegroundPatternVisible);

                if (encontrado.CutForegroundPatternColor != null)
                    ogs.SetCutForegroundPatternColor(encontrado.CutForegroundPatternColor);

                if (encontrado.CutForegroundPatternId!= null)
                    ogs.SetCutForegroundPatternId(encontrado.CutForegroundPatternId);

                //b)Background
                if (encontrado.CutBackgroundPatternVisible!=null)
                    ogs.SetCutBackgroundPatternVisible((bool)encontrado.CutBackgroundPatternVisible);

                if (encontrado.CutBackgroundPatternColor != null)
                    ogs.SetCutBackgroundPatternColor(encontrado.CutBackgroundPatternColor);

                if (encontrado.CutBackgroundPatternId != null)
                    ogs.SetCutBackgroundPatternId(encontrado.CutBackgroundPatternId);
            }
            //*************************************

            return ogs;

        }
    }
}
