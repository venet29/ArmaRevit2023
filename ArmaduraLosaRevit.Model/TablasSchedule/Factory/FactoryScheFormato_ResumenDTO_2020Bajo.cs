using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Factory
{
    public class FactoryScheduleFormato_ResumenDTO_2020BAjo
    {

     

        internal static ScheduleFormatoDTO_2020Bajo ObtenerDtoPesoBarra() => new ScheduleFormatoDTO_2020Bajo()
        {
            posicion = 1,
            _IsUseDefault=false,
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
            _DisplayUnitType = DisplayUnitType.DUT_CURRENCY,
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'UnitSymbolType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SymbolTypeId` class to replace uses of specific values of this enumeration.'
            _UnitSymbolType = UnitSymbolType.UST_NONE ,
#pragma warning restore CS0618 // 'UnitSymbolType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SymbolTypeId` class to replace uses of specific values of this enumeration.'
            _IsBackgroundColor = false,
            _BackgroundColor = new Color(0x00, 0x00, 0xFF),
            _IsTextColor = false,
            _TextColor = new Color(0x00, 0x00, 0xFF),
            _ancho = 0,
            _IsAccuracy=true,
            _Accuracy=0.01,
            _IsDisplayType=true,
            _DisplayType= ScheduleFieldDisplayType.Totals

        };
    }
}
