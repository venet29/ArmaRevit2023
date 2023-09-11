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
    class FActoryGraphicSettingsDTO_BarrasElevEntrega
    {
        internal static GraphicSettingDTO ObtenerCOnfMAllas()
        {
            var ogsMAlla = new GraphicSettingDTO("MAlla elev") {
                halftone = true,
                IsEdit_ProjectionLine = true,
                ProjectionLineColor =  FactoryColores.ObtenerColoresPorNombre(TipoCOlores.negro),
                ProjectionLineWeight=1
                
            };

            return ogsMAlla;

        }
        internal static GraphicSettingDTO ObtenerCOnfBArrasElev()
        {
            var ogsMAlla = new GraphicSettingDTO("BArras elev") { };

            return ogsMAlla;

        }

        internal static GraphicSettingDTO ObtenerCOnfEstrivo_CO()
        {
            var ogsMAlla = new GraphicSettingDTO("estriboCO elev")
            {
        
                IsEdit_ProjectionLine = true,
                ProjectionLineColor = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.negro),
                ProjectionLineWeight = 1

            };

            return ogsMAlla;
        }
    }
}
