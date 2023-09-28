using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Seleccion
{
    public class CantidadTrabas
    {
        internal static int Ejecutar(double _AnchoEstribo, int _cantidadLaterales=0)
        {
            if (_AnchoEstribo < Util.CmToFoot(38))
            {
                return 0;
            }

            else if (_AnchoEstribo < Util.CmToFoot(53) || _cantidadLaterales == 1)
                return 1;
            else if (_AnchoEstribo < Util.CmToFoot(78) || _cantidadLaterales == 2)
                return 2;
            else if (_AnchoEstribo < Util.CmToFoot(103) || _cantidadLaterales == 3)
                return 3;
            else if (_AnchoEstribo < Util.CmToFoot(128) || _cantidadLaterales == 4)
                return 4;
            else if (_AnchoEstribo < Util.CmToFoot(153) || _cantidadLaterales == 5)
                return 5;
            else if (_AnchoEstribo < Util.CmToFoot(178) || _cantidadLaterales == 6)
                return 6;

            else
                return 7;
        }
    }
}