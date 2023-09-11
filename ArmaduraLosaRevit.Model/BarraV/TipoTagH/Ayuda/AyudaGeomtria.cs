using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoTagH.Ayuda
{
    public class AyudaGeomtria
    {

        public static XYZ ObtenerDesfasePOrescalaArriba(double zz, View _view)
        {

            if (zz < 0)
            {
                XYZ dire = _view.RightDirection;

                if (_view.Scale == 50)
                    return new XYZ(0, 0, 0.5);
                else if (_view.Scale == 75)
                    return new XYZ(0, 0, 0.8);
                else if (_view.Scale == 100)
                    return new XYZ(0, 0, 2);
                else
                    return new XYZ(0, 0, 0);
            }
            else
            {
                if (_view.Scale == 100)
                    return new XYZ(0, 0, -0.5);
                else
                    return XYZ.Zero;
            }
        }

        public static XYZ MoverSOloRefuerzoManual_porEscala(XYZ DireccionEnFierrado, View _view)
        {
            XYZ dire = _view.RightDirection;

            if (DireccionEnFierrado.Z < 0) //directriz haciua arriba
            {

                if (_view.Scale == 50)
                    return dire * (1 + Util.CmToFoot(13.8));
                else if (_view.Scale == 75)
                    return dire * (1 + Util.CmToFoot(36.45)) - XYZ.BasisZ*0.5;
                else if (_view.Scale == 100)
                    return dire * (1 + Util.CmToFoot(58.8)) + -XYZ.BasisZ;
                else
                    return XYZ.Zero;
            }
            else //directriz haciua bajo
            {
                if (_view.Scale == 50)
                    return dire * (1 + Util.CmToFoot(13.8));
                else if (_view.Scale == 75)
                    return dire * (1 + Util.CmToFoot(36.45));
                else if (_view.Scale == 100)
                    return dire * (1 + Util.CmToFoot(58.8));
                else
                    return XYZ.Zero;
            }
        }


        //solo  para el caso reduerzo de borde  y en escala 100, el texto queda fuera pq la barra muy chica,entonces  se mueve texto contrario a rigthdirection
        internal static XYZ SoloRefuerZOBOrdeEscala100(View view, TipoBarraRefuerzoViga tipoBarraRefuerzoViga)
        {
            if (tipoBarraRefuerzoViga != TipoBarraRefuerzoViga.RefuerzoBorde) return XYZ.Zero;
            if (view.Scale != 100) return XYZ.Zero;



            return -view.RightDirection;
        }
    }
}
