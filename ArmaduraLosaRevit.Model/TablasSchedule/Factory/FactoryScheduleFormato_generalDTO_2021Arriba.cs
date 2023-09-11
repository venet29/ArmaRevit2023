using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule
{
    public class FactoryScheduleFormato_generalDTO_2021Arriba
    {

        public static ScheduleFormatoDTO_2021Arriba ObtenerDtoNombreVista() => new ScheduleFormatoDTO_2021Arriba()
        {
            posicion = 1,
            _IsDisplayUnitType = false,
            _IsUnitSymbolType = false,
            _IsBackgroundColor = false,
            _BackgroundColor = new Color(0x00, 0x00, 0xFF),
            _IsTextColor = false,
            _TextColor = new Color(0x00, 0x00, 0xFF),
            _ancho = 0.3


        };

        public static ScheduleFormatoDTO_2021Arriba ObtenerDtoCantiadad() => new ScheduleFormatoDTO_2021Arriba()
        {
            posicion = 2,
            _IsDisplayUnitType = false,
            _IsUnitSymbolType = false,
            _IsBackgroundColor = false,
            _BackgroundColor = new Color(0x00, 0x00, 0xFF),
            _IsTextColor = false,
            _TextColor = new Color(0x00, 0x00, 0xFF),
            _ancho = 0


        };
        public static ScheduleFormatoDTO_2021Arriba ObtenerDtoLArgo() => new ScheduleFormatoDTO_2021Arriba()
        {
            posicion = 3,
            _IsBackgroundColor = false,
            _BackgroundColor = new Color(0x00, 0x00, 0xFF),
            _IsTextColor = false,
            _TextColor = new Color(0x00, 0x00, 0xFF),
            _ancho = 0


        };
        public static ScheduleFormatoDTO_2021Arriba ObtenerDtoDiamtro() => new ScheduleFormatoDTO_2021Arriba()
        {
            posicion = 4,
            _IsBackgroundColor = false,
            _BackgroundColor = new Color(0x00, 0x00, 0xFF),
            _IsTextColor = false,
            _TextColor = new Color(0x00, 0x00, 0xFF),
            _ancho = 0


        };

        internal static ScheduleFormatoDTO_2021Arriba ObtenerDtoPesoBarra() => new ScheduleFormatoDTO_2021Arriba()
        {
            posicion = 5,
            _IsUseDefault = false,
            _IsBackgroundColor = false,
            _BackgroundColor = new Color(0x00, 0x00, 0xFF),
            _IsTextColor = false,
            _TextColor = new Color(0x00, 0x00, 0xFF),
            _ancho = 0,
            _IsAccuracy = true,
            _Accuracy = 0.01,
            _IsDisplayType = true,
            _DisplayType = ScheduleFieldDisplayType.Totals

        };
    }
}
