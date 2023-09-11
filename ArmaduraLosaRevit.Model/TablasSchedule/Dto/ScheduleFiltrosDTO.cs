using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Dto
{
   
    public class ScheduleFiltrosDTO
    {
        public string nombrePArameter { get; set; }

        public object valor { get; set; }

        public ScheduleFilterType _ScheduleFilterType { get; set; }
        public ElementId ElementIdPAra { get; set; } 
    }
}
