using ArmaduraLosaRevit.Model.BarraV.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra
{
    public interface IbarraBasev
    {
        void M0_CalcularCurva();
        bool M1_DibujarBarra();
        bool M1_1_DibujarBarraCOnfiguracion();
        bool M2_DibujarTags(ConfiguracionTAgBarraDTo confBarraTag);

        ElementId M3_ObtenerIdRebar();
    }
}
