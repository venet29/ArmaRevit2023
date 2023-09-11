using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Dto
{
   public class ScheduleFormatoDTO_2020Bajo
    {
        public int posicion { get; set; }

        public double _ancho { get; set; }
//#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
//        public DisplayUnitType _DisplayUnitType { get; set; }
//#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
//#pragma warning disable CS0618 // 'UnitSymbolType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SymbolTypeId` class to replace uses of specific values of this enumeration.'
//        public UnitSymbolType _UnitSymbolType { get; set; }
//#pragma warning restore CS0618 // 'UnitSymbolType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SymbolTypeId` class to replace uses of specific values of this enumeration.'

        public bool _IsBackgroundColor { get; set; } //para indicar si se cambia _BackgroundColor
        public Color _BackgroundColor { get; set; }

        public bool _IsTextColor { get; set; }//para indicar si se cambia _TextColor
        public Color _TextColor { get; set; }
        public bool _IsDisplayUnitType { get; set; } = true;
        public bool _IsUnitSymbolType { get; set; } = true;
        public string nombre { get; internal set; }

        public bool _IsAccuracy { get; set; } = false; //para indicar si se cambia _BackgroundColor
        public double _Accuracy { get; set; }

        public bool _IsUseDefault { get; set; } = false; //para indicar si se cambia _BackgroundColor
        public bool _IsDisplayType { get; internal set; } = false;
        public ScheduleFieldDisplayType _DisplayType { get;  set; }
    }
}
