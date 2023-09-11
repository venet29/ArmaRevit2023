using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule
{
    public class ScheduleFormato_2020BAjo
    {


        public static void ApplyFormattingToField(ViewSchedule schedule, ScheduleFormatoDTO_2020Bajo _scheduleFormatoDTO)
        {
            // Get the field.
            ScheduleDefinition definition = schedule.Definition;
            ScheduleField field = definition.GetField(_scheduleFormatoDTO.posicion);
            field.ColumnHeading = _scheduleFormatoDTO.nombre;
            // Build unit formatting for the field.
            FormatOptions options = field.GetFormatOptions();

            options.UseDefault = false;
#pragma warning disable CS0618 // 'FormatOptions.DisplayUnits' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetUnitTypeId()` and `SetUnitTypeId(ForgeTypeId)` methods instead.'
            options.DisplayUnits = _scheduleFormatoDTO._DisplayUnitType;// DisplayUnitType.DUT_SQUARE_INCHES;
#pragma warning restore CS0618 // 'FormatOptions.DisplayUnits' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetUnitTypeId()` and `SetUnitTypeId(ForgeTypeId)` methods instead.'
#pragma warning disable CS0618 // 'UnitSymbolType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SymbolTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'FormatOptions.UnitSymbol' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetSymbolTypeId()` and `SetSymbolTypeId(ForgeTypeId)` methods instead.'
            options.UnitSymbol = UnitSymbolType.UST_IN_SUP_2;
#pragma warning restore CS0618 // 'FormatOptions.UnitSymbol' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetSymbolTypeId()` and `SetSymbolTypeId(ForgeTypeId)` methods instead.'
#pragma warning restore CS0618 // 'UnitSymbolType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SymbolTypeId` class to replace uses of specific values of this enumeration.'

            // Build style overrides for the field
            // Use override options to indicate fields that are overridden and apply changes
            TableCellStyle style = field.GetStyle();
            TableCellStyleOverrideOptions overrideOptions = style.GetCellStyleOverrideOptions();
            overrideOptions.BackgroundColor = true;
            style.BackgroundColor = new Color(0x00, 0x00, 0xFF);
            overrideOptions.FontColor = true;
            style.TextColor = new Color(0xFF, 0xFF, 0xFF);
            overrideOptions.Italics = true;
            style.IsFontItalic = true;

            style.SetCellStyleOverrideOptions(overrideOptions);

            double width = field.GridColumnWidth;

            using (Transaction t = new Transaction(schedule.Document, "Set style etc"))
            {
                t.Start();
                field.SetStyle(style);
                field.SetFormatOptions(options);
                // Change column width (affects width in grid and on sheet) - units are in Revit length units - ft.
                field.GridColumnWidth = width + 0.5;
                t.Commit();
            }
        }

        public static void ApplyFormattingToFieldSinTras(ViewSchedule schedule, ScheduleFormatoDTO_2020Bajo _scheduleFormatoDTO)
        {
            try
            {
                

                // Get the field.
                ScheduleDefinition definition = schedule.Definition;
                ScheduleField field = definition.GetField(_scheduleFormatoDTO.posicion);

                // Build unit formatting for the field.
                FormatOptions options = field.GetFormatOptions();
                options.UseDefault = false;

                if (_scheduleFormatoDTO._IsUseDefault) options.UseDefault = _scheduleFormatoDTO._IsUseDefault;// DisplayUnitType.DUT_SQUARE_INCHES;
#pragma warning disable CS0618 // 'FormatOptions.DisplayUnits' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetUnitTypeId()` and `SetUnitTypeId(ForgeTypeId)` methods instead.'
                if (_scheduleFormatoDTO._IsDisplayUnitType) options.DisplayUnits = _scheduleFormatoDTO._DisplayUnitType;// DisplayUnitType.DUT_SQUARE_INCHES;
#pragma warning restore CS0618 // 'FormatOptions.DisplayUnits' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetUnitTypeId()` and `SetUnitTypeId(ForgeTypeId)` methods instead.'
#pragma warning disable CS0618 // 'FormatOptions.UnitSymbol' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetSymbolTypeId()` and `SetSymbolTypeId(ForgeTypeId)` methods instead.'
                if (_scheduleFormatoDTO._IsUnitSymbolType) options.UnitSymbol = _scheduleFormatoDTO._UnitSymbolType;
#pragma warning restore CS0618 // 'FormatOptions.UnitSymbol' is obsolete: 'This property is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `GetSymbolTypeId()` and `SetSymbolTypeId(ForgeTypeId)` methods instead.'

                if (_scheduleFormatoDTO._IsAccuracy) options.Accuracy = _scheduleFormatoDTO._Accuracy;

                // Build style overrides for the field
                // Use override options to indicate fields that are overridden and apply changes
                TableCellStyle style = field.GetStyle();
                TableCellStyleOverrideOptions overrideOptions = style.GetCellStyleOverrideOptions();
                overrideOptions.BackgroundColor = true;
                if (_scheduleFormatoDTO._IsBackgroundColor) style.BackgroundColor = _scheduleFormatoDTO._BackgroundColor;
                overrideOptions.FontColor = true;
                if (_scheduleFormatoDTO._IsTextColor) style.TextColor = _scheduleFormatoDTO._TextColor;
                overrideOptions.Italics = true;
                style.IsFontItalic = true;

                overrideOptions.HorizontalAlignment = true;
                style.FontHorizontalAlignment = HorizontalAlignmentStyle.Center;

                style.SetCellStyleOverrideOptions(overrideOptions);

                double width = field.GridColumnWidth;

                if (_scheduleFormatoDTO._IsDisplayType) field.DisplayType = _scheduleFormatoDTO._DisplayType ;

                field.SetStyle(style);
                if (_scheduleFormatoDTO._IsDisplayUnitType) field.SetFormatOptions(options);
                // Change column width (affects width in grid and on sheet) - units are in Revit length units - ft.
                if (_scheduleFormatoDTO._ancho > 0) field.GridColumnWidth = width + _scheduleFormatoDTO._ancho;
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al crear  columna :{_scheduleFormatoDTO.nombre}  \n  ex:{ex.Message}");
            }
        }
    }
}
