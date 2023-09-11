using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Traslapos
{
    public class TraslapoDTO
    {
        public Rebar RebarInicial { get; set; }
        public Rebar RebarFinal { get; set; }

        public XYZ ptoIni { get; set; }
        public XYZ ptoFin { get; set; }
        public bool IsOK { get; set; } = false;
    }
}
