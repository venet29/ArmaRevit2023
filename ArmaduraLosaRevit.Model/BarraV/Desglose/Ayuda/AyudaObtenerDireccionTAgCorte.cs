using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda
{
    public class AyudaObtenerDireccionTAgCorte
    {
        internal static XYZ Obtener(List<Curve> listaCurvasZcero, XYZ _ptobarra)
        {
            XYZ Direccion = XYZ.Zero;
            try
            {


                double DistanciaMinima = 1000000;
           
                foreach (var item in listaCurvasZcero)
                {
                    IntersectionResult result = item.Project(_ptobarra);
                    if (result.Distance < DistanciaMinima)
                    {
                        DistanciaMinima = result.Distance;
                        Direccion = (result.XYZPoint - _ptobarra).Normalize();
                    }
                }
            }
            catch (Exception)
            {
                return XYZ.Zero;
            }
            return Direccion;
        }
    }
}
