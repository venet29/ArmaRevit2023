using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    public class DatosBordeLosaDTO
    {
        public XYZ ptoInicial { get; set; }
        public XYZ ptoFinal { get; set; }
        public XYZ DireccionHaciaLosa { get; set; }
    }
}
