using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
     public class ConfiguracionTAgEstriboDTo
    {
        public bool IsDIrectriz { get; set; }
        public TagOrientation tagOrientation { get; set; }

        public XYZ LeaderElbow { get; set; }
        public XYZ desplazamientoPathReinSpanSymbol { get; set; }
    }
}

