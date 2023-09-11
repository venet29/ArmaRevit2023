using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Factory
{
    public class Factory_ScheduleFiltrosDTO
    {
        public static ScheduleFiltrosDTO ObtenerDtoNombreVista() => new ScheduleFiltrosDTO()
        {

            _ScheduleFilterType= ScheduleFilterType.Equal
        };

    }
}
