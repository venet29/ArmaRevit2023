using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras.Ayuda
{
    public class AyudaPuntosDeCurva
    {
        public static string ObtnertextoListapto(List<Curve> listcurve)
        {
            string resul=$"Cantidad curvas :{listcurve.Count}\n\n";

            foreach (var item in listcurve)
            {
                resul += $" P1: {item.GetEndPoint(0).REdondearString_foot(3)}  P2: {item.GetEndPoint(1).REdondearString_foot(3)}\n";
            }
            return resul;
        }
    }
}
