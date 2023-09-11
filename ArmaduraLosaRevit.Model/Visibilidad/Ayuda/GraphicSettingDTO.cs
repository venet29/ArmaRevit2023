using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.Visibilidad.Ayuda
{
    public class GraphicSettingDTO
    {

        public bool? halftone { get; internal set; }
        public string Nombre { get; }

        public bool IsEdit_ProjectionLine { get; set; }
        public bool IsEdit_SurfacePattern { get; set; }

        public bool IsEdit_CutLine { get; set; }
        public bool IsEdit_CutPatterns { get; set; }
        //****************************
        public bool? CutForegroundPatternVisible;
        public Color ProjectionLineColor { get; set; }

        public Color SurfaceForegroundPatternColor { get; set; }

        public Color SurfaceBackgroundPatternColor { get; set; }

        public Color CutLineColor { get; set; }
        public Element CutLinePattern { get; set; }
        public Color CutForegroundPatternColor { get; set; }

        public Color CutBackgroundPatternColor { get; set; }
        public ElementId CutBackgroundPatternId { get; set; }
        public bool? CutBackgroundPatternVisible { get; internal set; }
        public ViewDetailLevel detailLevel { get; internal set; }
        public int CutLineWeight { get; internal set; }
        public ElementId CutLinePatternId { get; internal set; }

        public ElementId CutForegroundPatternId { get; set; }
        public ElementId ProjectionLinePatternId { get; set; }
        public int ProjectionLineWeight { get; set; }
        public ElementId SurfaceBackgroundPatternId { get; set; }
        public bool? SurfaceBackgroundPatternVisible { get; set; }
        public bool? SurfaceForegroundPatternVisible { get; set; }
        public ElementId SurfaceForegroundPatternId { get; set; }
        public int SurfaceTransparency { get; set; }

        public GraphicSettingDTO(string nombre)
        {
            this.Nombre = nombre;

            halftone = null;
            SurfaceBackgroundPatternVisible = null;
                SurfaceForegroundPatternVisible = null;
            IsEdit_ProjectionLine = false;
            IsEdit_SurfacePattern = false;

            IsEdit_CutLine = false;
            IsEdit_CutPatterns = false;
            SurfaceTransparency = -1;

        }
        public GraphicSettingDTO(string nombre,
            Color ProjectionLineColor_,
            Color SurfaceForegroundPatternColor_,
            Color SurfaceBackgroundPatternColor_,
            Color CutLineColor_,
            Color CutForegroundPatternColor_,
            Color CutBackgroundPatternColor_)
        {
            this.Nombre = nombre;
            this.ProjectionLineColor = ProjectionLineColor_;
            this.SurfaceForegroundPatternColor = SurfaceForegroundPatternColor_;
            this.SurfaceBackgroundPatternColor = SurfaceBackgroundPatternColor_;
            this.CutLineColor = CutLineColor_;

            this.CutForegroundPatternColor = CutForegroundPatternColor_;
            this.CutBackgroundPatternColor = CutBackgroundPatternColor_;
        }


    }


}
