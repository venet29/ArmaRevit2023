using ArmaduraLosaRevit.Model.BarraV.Horquilla.Calculos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.DTO
{
     public class ConfiguracionTagDTO
    {
        public bool IsDIrectriz { get; set; }
        public TagOrientation tagOrientation { get; set; }

        public XYZ LeaderElbow { get; set; }
        public XYZ desplazamientoPathReinSpanSymbol { get; set; }
        public XYZ PtoFinal { get; internal set; }
        public XYZ TagHeadPosition { get; internal set; }
        //barras elevacione Horizon y Vertical


    }
}

