using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.FactoryGraphicSettings;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.FactoryGraphicSettings
{

    //Edita el VV
    public class FactoryGraphicSetting_ViewLOSAEntrega_VV
    {
        static List<GraphicSettingDTO> ListaFactoRY_ENTREGA;
        public static ViewDetailLevel detailLevel { get; private set; }

        public static OverrideGraphicSettings ObtenerOverrideGraphicSettings_Entrega(string _catname, Document _doc)
        {
            if (ListaFactoRY_ENTREGA == null)
                if (!CArgar_ENTREGA(_doc)) return null;
            if (ListaFactoRY_ENTREGA.Count == 0)
                if (!CArgar_ENTREGA(_doc)) return null;


            OverrideGraphicSettings ogs = new OverrideGraphicSettings();

            var encontrado = ListaFactoRY_ENTREGA.Where(c => c.Nombre == _catname).FirstOrDefault();
            if (encontrado == null) return null;

            return Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(encontrado);

        }
        public static bool CArgar_ENTREGA(Document _doc)
        {
            ListaFactoRY_ENTREGA = new List<GraphicSettingDTO>();


       

            Element _tipopatter_Structural_Rebar = TiposPatternType.ObtenerTipoPattern("<Solid fill>", _doc);

            if (_tipopatter_Structural_Rebar == null)
            {
                Util.ErrorMsg($"Error no se pudo encontrar '<Solid fill>' ");
                return false;
            }

            byte valorwalls = 75;
            byte valorWall2 = 225;
            ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Walls")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLinePatternId = LinePatternElement.GetSolidPatternId(),
                ProjectionLineColor = new Color(valorwalls, valorwalls, valorwalls),
                ProjectionLineWeight = 1,

                IsEdit_SurfacePattern = true,
                SurfaceBackgroundPatternVisible = true,
                SurfaceBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
                SurfaceBackgroundPatternColor = new Color(valorWall2, valorWall2, valorWall2),
                SurfaceForegroundPatternVisible = true,
                SurfaceForegroundPatternId = _tipopatter_Structural_Rebar.Id,
                SurfaceForegroundPatternColor = new Color(valorWall2, valorWall2, valorWall2),

                IsEdit_CutLine = true,
                CutLinePatternId= null,
                CutLineColor = new Color(valorwalls, valorwalls, valorwalls),
                CutLineWeight = 1,

                IsEdit_CutPatterns = true,
                CutBackgroundPatternVisible = true,
                CutBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
                CutBackgroundPatternColor = new Color(valorWall2, valorWall2, valorWall2),
                CutForegroundPatternVisible = true,
                CutForegroundPatternId = _tipopatter_Structural_Rebar.Id,
                CutForegroundPatternColor = new Color(valorWall2, valorWall2, valorWall2),

            });

            byte valorStrFra = 75;
            byte valorStrFra2 = 225;
            ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Structural Framing")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLinePatternId = null,
                ProjectionLineColor = new Color(valorStrFra, valorStrFra, valorStrFra),
                ProjectionLineWeight = 1,

                IsEdit_SurfacePattern = true,
                SurfaceBackgroundPatternVisible = true,
                SurfaceBackgroundPatternId = _tipopatter_Structural_Rebar?.Id,
                SurfaceBackgroundPatternColor = new Color(valorStrFra2, valorStrFra2, valorStrFra2),
                SurfaceForegroundPatternVisible = true,
                SurfaceForegroundPatternId = _tipopatter_Structural_Rebar?.Id,
                SurfaceForegroundPatternColor = new Color(valorStrFra2, valorStrFra2, valorStrFra2),

                IsEdit_CutLine = true,
                CutLineColor = new Color(valorStrFra, valorStrFra, valorStrFra),
                CutLineWeight = 1,

                IsEdit_CutPatterns = true,
                CutBackgroundPatternVisible = true,
                CutBackgroundPatternId = _tipopatter_Structural_Rebar?.Id,
                CutBackgroundPatternColor = new Color(valorStrFra2, valorStrFra2, valorStrFra2),
                CutForegroundPatternVisible = true,
                CutForegroundPatternId = _tipopatter_Structural_Rebar?.Id,
                CutForegroundPatternColor = new Color(valorStrFra2, valorStrFra2, valorStrFra2),

            });

            Element _tipopatter_Structural_Columns = TiposPatternType.ObtenerTipoPattern("HATCH LOSAS", _doc);

            if (_tipopatter_Structural_Rebar == null)
            {
                Util.ErrorMsg($"Error no se pudo encontrar 'HATCH LOSA' ");
                return false;
            }
            byte valorStrColumns = 75;
            byte valorStrColumns2 = 225;
            ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Structural Columns")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLinePatternId = null,
                ProjectionLineColor = new Color(valorStrColumns, valorStrColumns, valorStrColumns),
                ProjectionLineWeight = 1,

                IsEdit_SurfacePattern = true,
                SurfaceBackgroundPatternVisible = true,
                SurfaceBackgroundPatternId = _tipopatter_Structural_Columns?.Id,
                SurfaceBackgroundPatternColor = new Color(valorStrColumns2, valorStrColumns2, valorStrColumns2),
                SurfaceForegroundPatternVisible = true,
                SurfaceForegroundPatternId = _tipopatter_Structural_Columns?.Id,
                SurfaceForegroundPatternColor = new Color(valorStrColumns2, valorStrColumns2, valorStrColumns2),

                IsEdit_CutLine = true,
                CutLineColor = new Color(valorStrColumns, valorStrColumns, valorStrColumns),
                CutLineWeight = 1,

                IsEdit_CutPatterns = true,
                CutBackgroundPatternVisible = true,
                CutBackgroundPatternId = _tipopatter_Structural_Columns?.Id,
                CutBackgroundPatternColor = new Color(valorStrColumns2, valorStrColumns2, valorStrColumns2),
                CutForegroundPatternVisible = true,
                CutForegroundPatternId = _tipopatter_Structural_Columns?.Id,
                CutForegroundPatternColor = new Color(valorStrColumns2, valorStrColumns2, valorStrColumns2),

            });

            //Element _tipopatter_Structural_Rebar = TiposPatternType.ObtenerTipoPattern("<Solid fill>", _doc);

            //if (_tipopatter_Structural_Rebar == null)
            //{
            //    Util.ErrorMsg($"Error no se pudo encontrar '<Solid fill>' ");
            //    return false;
            //}



            //ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Floors")
            //{  

            //    IsEdit_SurfacePattern = true,
            //    SurfaceBackgroundPatternVisible = true,
            //    SurfaceBackgroundPatternId= _tipopatter_Structural_Rebar.Id,
            //    SurfaceBackgroundPatternColor = new Color(240, 240, 240),
            //    SurfaceForegroundPatternVisible = true,
            //    SurfaceForegroundPatternId = _tipopatter_Structural_Rebar.Id,
            //    SurfaceForegroundPatternColor = new Color(240, 240, 240),

            //    IsEdit_CutPatterns = true,
            //    CutBackgroundPatternVisible = true,
            //    CutBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
            //    CutBackgroundPatternColor = new Color(240, 240, 240),
            //    CutForegroundPatternVisible = true,
            //    CutForegroundPatternId = _tipopatter_Structural_Rebar.Id,
            //    CutForegroundPatternColor = new Color(240, 240, 240),

            //});

            //ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Structural Foundations")
            //{
            //    IsEdit_SurfacePattern = true,
            //    SurfaceBackgroundPatternVisible = true,
            //    SurfaceBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
            //    SurfaceBackgroundPatternColor = new Color(240, 240, 240),
            //    SurfaceForegroundPatternVisible = true,
            //    SurfaceForegroundPatternId = _tipopatter_Structural_Rebar.Id,
            //    SurfaceForegroundPatternColor = new Color(240, 240, 240),

            //    IsEdit_CutPatterns = true,
            //    CutBackgroundPatternVisible = true,
            //    CutBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
            //    CutBackgroundPatternColor = new Color(240, 240, 240),
            //    CutForegroundPatternVisible = true,
            //    CutForegroundPatternId = _tipopatter_Structural_Rebar.Id,
            //    CutForegroundPatternColor = new Color(240, 240, 240),

            //});


            ListaFactoRY_ENTREGA.Add(FActoryGraphicSettingsBarrasElevLosa.ObtenerPAthSymbolLosa_Barra());
            ListaFactoRY_ENTREGA.Add(FActoryGraphicSettingsBarrasElevLosa.ObtenerPAthSymbolLosa_Rebar());

            return true;
          

        }

    };

}








