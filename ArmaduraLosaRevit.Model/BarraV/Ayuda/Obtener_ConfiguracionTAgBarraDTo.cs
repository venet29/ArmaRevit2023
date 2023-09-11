using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Ayuda
{
    public class Obtener_ConfiguracionTAgBarraDTo
    {
        public static ConfiguracionTAgBarraDTo Ejecutar(XYZ DireccionEnFierrado ,bool IsDIrectriz, XYZ LeaderElbow)
        {

            ConfiguracionTAgBarraDTo confBarraTagSindirectriz = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, -0),
                IsDIrectriz = IsDIrectriz,
                LeaderElbow = LeaderElbow,
                tagOrientation = TagOrientation.Horizontal,
                BarraTipo = TipoRebar.ELEV_BA_H,
                TipoCaraObjeto_ = (DireccionEnFierrado.Z > 0 ? TipoCaraObjeto.Inferior : TipoCaraObjeto.Superior)
            };

            return confBarraTagSindirectriz;
        }
    }
}
