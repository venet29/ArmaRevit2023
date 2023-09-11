using ArmaduraLosaRevit.Model.BarraV.Horquilla.Calculos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
     public class ConfiguracionTAgBarraDTo
    {
        public bool IsDIrectriz { get; set; }
        public TagOrientation tagOrientation { get; set; }

        public XYZ LeaderElbow { get; set; }
        public XYZ desplazamientoPathReinSpanSymbol { get; set; }
        //barras elevacione Horizon y Vertical
        public TipoRebar BarraTipo { get;  set; }

        //barras SOLO elevacione Horizon 
        public TipoCaraObjeto TipoCaraObjeto_ { get; set; }
        public RecalcularPtosYEspaciamieto_Horquilla RecalcularPtosYEspaciamieto_Horqu { get; internal set; }

        public static ConfiguracionTAgBarraDTo OBtenercasoVertical() => new ConfiguracionTAgBarraDTo()
        {
            desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
            IsDIrectriz = true,
            LeaderElbow = new XYZ(0, 0, ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT),
            tagOrientation = TagOrientation.Vertical,
            BarraTipo = TipoRebar.ELEV_BA_V
        };

        public static ConfiguracionTAgBarraDTo OBtenercasoHorizontal(XYZ ptocodo) => new ConfiguracionTAgBarraDTo()
        {
            desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
            IsDIrectriz = true,
            LeaderElbow = ptocodo,
            tagOrientation = TagOrientation.Horizontal,
            BarraTipo = TipoRebar.ELEV_BA_V
        };
    }
}

