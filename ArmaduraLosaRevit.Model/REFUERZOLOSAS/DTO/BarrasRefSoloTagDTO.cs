using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    public class BarrasRefSoloTagDTO
    {
        public double largo { get; set; }
        public XYZ P1 { get; set; }
        public XYZ P2 { get; set; }

        public TipoBarraSoloTag posicion { get; set; }
    }
}
