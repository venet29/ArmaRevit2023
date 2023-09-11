using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Factory
{
    public class FactoryScheFormato_ResumenDTO_2021Arriba
    {

     

        internal static ScheduleFormatoDTO_2021Arriba ObtenerDtoPesoBarra() => new ScheduleFormatoDTO_2021Arriba()
        {
            posicion = 1,
            _IsUseDefault=false,
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
