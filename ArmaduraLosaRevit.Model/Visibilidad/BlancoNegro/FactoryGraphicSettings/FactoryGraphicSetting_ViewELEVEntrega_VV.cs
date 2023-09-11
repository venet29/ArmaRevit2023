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
    public class FactoryGraphicSetting_ViewELEVEntrega_VV
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
            //Element _tipopatter_Structural_Rebar = TiposPatternType.ObtenerTipoPattern("Solid",_doc);
            // if (_tipopatter_Structural_Rebar == null)
            // {
            //     Util.ErrorMsg($"Error no se pudo encontrar 'pattern'  tipo:Solid de caso 'Structural Rebar' no encontrada ");
            //     return;
            // }
            ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Structural Rebar")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLinePatternId = null,
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta),
                ProjectionLineWeight = 5,

                IsEdit_CutLine = true,
                CutLinePattern = null,
                CutLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta),
                CutLineWeight = 5,

            });


            Element _tipopatter_Structural_Rebar = TiposPatternType.ObtenerTipoPattern("<Solid fill>", _doc);

            if (_tipopatter_Structural_Rebar == null)
            {
                Util.ErrorMsg($"Error no se pudo encontrar '<Solid fill>' ");
                return false;
            }



            ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Floors")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.negro),
                ProjectionLineWeight = 1,

                IsEdit_SurfacePattern = true,
                SurfaceBackgroundPatternVisible = true,
                SurfaceBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
                SurfaceBackgroundPatternColor = new Color(225, 225, 225),
                SurfaceForegroundPatternVisible = true,
                SurfaceForegroundPatternId = _tipopatter_Structural_Rebar.Id,
                SurfaceForegroundPatternColor = new Color(225, 225, 225),

                IsEdit_CutLine = true,
                CutLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.negro),
                CutLineWeight = 1,

                IsEdit_CutPatterns = true,
                CutBackgroundPatternVisible = true,
                CutBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
                CutBackgroundPatternColor = new Color(225, 225, 225),
                CutForegroundPatternVisible = true,
                CutForegroundPatternId = _tipopatter_Structural_Rebar.Id,
                CutForegroundPatternColor = new Color(225, 225, 225),

            });

            ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Structural Foundations")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.negro),
                ProjectionLineWeight = 1,

                IsEdit_SurfacePattern = true,
                SurfaceBackgroundPatternVisible = true,
                SurfaceBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
                SurfaceBackgroundPatternColor = new Color(225, 225, 225),
                SurfaceForegroundPatternVisible = true,
                SurfaceForegroundPatternId = _tipopatter_Structural_Rebar.Id,
                SurfaceForegroundPatternColor = new Color(225, 225, 225),

                IsEdit_CutLine = true,
                CutLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.negro),
                CutLineWeight = 1,

                IsEdit_CutPatterns = true,
                CutBackgroundPatternVisible = true,
                CutBackgroundPatternId = _tipopatter_Structural_Rebar.Id,
                CutBackgroundPatternColor = new Color(225, 225, 225),
                CutForegroundPatternVisible = true,
                CutForegroundPatternId = _tipopatter_Structural_Rebar.Id,
                CutForegroundPatternColor = new Color(225, 225, 225),

            });


            //  anotation

            ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Structural Rebar Tags")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLinePatternId = null,
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.negro),
                ProjectionLineWeight = 1,
            });


            byte valorwalls = 75;
#pragma warning disable CS0219 // The variable 'valorWall2' is assigned but its value is never used
            byte valorWall2 = 225;
#pragma warning restore CS0219 // The variable 'valorWall2' is assigned but its value is never used
            ListaFactoRY_ENTREGA.Add(new GraphicSettingDTO("Walls")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLinePatternId = LinePatternElement.GetSolidPatternId(),
                ProjectionLineColor = new Color(valorwalls, valorwalls, valorwalls),
                ProjectionLineWeight = 1,

   

                IsEdit_CutLine = true,
                CutLinePatternId = null,
                CutLineColor = new Color(valorwalls, valorwalls, valorwalls),
                CutLineWeight = 1,

         

            });


            return true;


        }

    };

}








