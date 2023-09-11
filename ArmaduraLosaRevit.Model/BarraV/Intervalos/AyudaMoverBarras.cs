using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    class AyudaMoverBarras
    {
        public static bool MOverBarras(bool moverPorTraslapo)
        {

            if (VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras)
                return false;
            else
                return !moverPorTraslapo;
        }
    }
}
