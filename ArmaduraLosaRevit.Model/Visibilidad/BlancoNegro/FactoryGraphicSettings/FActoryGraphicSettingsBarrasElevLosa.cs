using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.FactoryGraphicSettings
{
    class FActoryGraphicSettingsBarrasElevLosa
    {
        internal static GraphicSettingDTO ObtenerbarrasRefuerzoLosa(Document _doc)
        {
            var ogsMAlla = new GraphicSettingDTO("BarrasRefuerzoLosa")
            {

                IsEdit_ProjectionLine = true,
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta),
                ProjectionLineWeight = 5,

                IsEdit_SurfacePattern = true,
                SurfaceBackgroundPatternVisible = true,
                SurfaceForegroundPatternVisible = true,
            };

            var idpat = TiposPatternType.ObtenerTipoPattern("0.5_EST_ESPESORES", _doc);
            if (idpat == null)
            {
                Util.ErrorMsg("Error al obtener '0.5_EST_ESPESORES'. No se aplica en configuracion");
            }
            else
                ogsMAlla.SurfaceBackgroundPatternId = idpat.Id;

            return ogsMAlla;
        }


        internal static GraphicSettingDTO ObtenerPAthSymbolLosa_Barra()
        {
            var ogsMAlla = new GraphicSettingDTO("Barra")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta),
                ProjectionLineWeight = 5,
            };
            return ogsMAlla;
        }

        internal static GraphicSettingDTO ObtenerPAthSymbolLosa_Rebar()
        {
            var ogsMAlla = new GraphicSettingDTO("Rebar")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta),
                ProjectionLineWeight = 5,
            };
            return ogsMAlla;
        }

        public static GraphicSettingDTO ObtenerColor_Rebar_3D(int diamtro) => new GraphicSettingDTO($"Rebar{diamtro}")
        {
            IsEdit_ProjectionLine = true,
            ProjectionLineColor = FactoryColores.ObtenerColoresPorDiametro(diamtro),
            ProjectionLineWeight = 5,
        };



        internal static GraphicSettingDTO ObtenerbarrasEstriboLosa()
        {
            var ogsMAlla = new GraphicSettingDTO("EstriboLosa")
            {
                IsEdit_ProjectionLine = true,
                ProjectionLinePatternId = LinePatternElement.GetSolidPatternId(),
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.negro),
                ProjectionLineWeight = 1,

                IsEdit_CutLine = true,
                CutLineColor = new Color(254, 254, 254)
            };
            return ogsMAlla;
        }
    }
}
