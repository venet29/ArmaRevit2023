using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS
{
    class UtilRefuerzo
    {
        public static bool ISOrdena2Ptos(XYZ p1, XYZ p2)
        {
            XYZ[] resul = new XYZ[2];
            double deltaY = Math.Abs(p2.Y - p1.Y);
            double deltaX = Math.Abs(p2.X - p1.X);


            if (deltaY > deltaX)          
                return VerificarDireccion_Y(p1, p2);            
            else 
                return VerificarDireccion_X(p1, p2);


            //if (Math.Abs(p1.X - p2.X) < 0.001)  //vertical
            //    return VerificarDireccion_Y(p1, p2);
            //else
            //    return VerificarDireccion_X(p1, p2);

        }

        private static bool VerificarDireccion_X(XYZ p1, XYZ p2)
        {

            // el pto con x menor es el incial       ini  -  fin
            if (p1.X < p2.X)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool VerificarDireccion_Y(XYZ p1, XYZ p2)
        {
            // el pto con y menor es el incial            fini
            if (p1.Y < p2.Y)//                             |
            {                                         //  ini
                return true;
            }
            else
            {
                return false;
            }
        }

        //busca p1> p2    vertical: Y mayor ,  inclinado :  X mayor
        public static bool ISOrdena2PtosBordeMuro(XYZ p1, XYZ p2)
        {
          


            if (Math.Abs(p1.X - p2.X) < 0.001)  //vertical
            {

                // el pto con y menor es el incial            fini
                if (p1.Y < p2.Y)//                             |
                {                                         //  ini
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {

                // el pto con x menor es el incial       ini  -  fin
                if (p1.X < p2.X)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

    }
}
