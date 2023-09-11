using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Dto
{
   public class ScheduleFormatoDTO_2021Arriba
    {
        public int posicion { get; set; }

        public double _ancho { get; set; }

        public bool _IsBackgroundColor { get; set; } //para indicar si se cambia _BackgroundColor
        public Color _BackgroundColor { get; set; }

        public bool _IsTextColor { get; set; }//para indicar si se cambia _TextColor
        public Color _TextColor { get; set; }
        public bool _IsDisplayUnitType { get; set; } = false;
        public bool _IsUnitSymbolType { get; set; } = false;
        public string nombre { get; internal set; }

        public bool _IsAccuracy { get; set; } = false; //para indicar si se cambia _BackgroundColor
        public double _Accuracy { get; set; }

        public bool _IsUseDefault { get; set; } = false; //para indicar si se cambia _BackgroundColor
        public bool _IsDisplayType { get; internal set; } = false;
        public ScheduleFieldDisplayType _DisplayType { get;  set; }
    }
}
