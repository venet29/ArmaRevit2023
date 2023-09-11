using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos.Ayuda
{
    public class AyudaCalculo
    {

        //vertical completamnte  -->  resul[0] = menor Y ,  resul[1] = mayor y;
        //cualquier otro caso -->  resul[0] = menor x ,  resul[1] = mayor x;
        public static (XYZ resIni , XYZ resFin) Ordena2Ptos(XYZ p1, XYZ p2)
        {
            //resul[0] = ini;
            //resul[1] = fin;
            XYZ[] resul = new XYZ[2];
            if (Math.Abs(p1.X - p2.X) < 0.001)  //vertical
            {
                // el pto con y menor es el incial            ini
                if (p1.Y < p2.Y)//                             |
                {                                         //  fin
                    resul[0] = p2;
                    resul[1] = p1;
                }
                else
                {
                    resul[0] = p1;
                    resul[1] = p2;
                }
            }
            else
            {
                // el pto con x menor es el incial       ini  -  fin
                if (p1.X < p2.X)
                {
                    resul[0] = p1;
                    resul[1] = p2;
                }
                else
                {
                    resul[0] = p2;
                    resul[1] = p1;
                }
            }
            return (resul[0], resul[1]);
        }
    }
}
