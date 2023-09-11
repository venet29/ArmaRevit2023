using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Varios
{
    public class AyudaAgregarEspesorLosa
    {
        public static List<XYZ> Ejecutar(List<XYZ> listPtos, XYZ pto1SeleccionadoConMouse)
        {
            List<XYZ> listResult = new List<XYZ>();
            XYZ _deltaAux = new XYZ(0, 0, Util.CmToFoot(20));
            foreach (var item in listPtos)
            {
                listResult.Add(item + _deltaAux);
            }

            return listResult;
        }
    }
}
