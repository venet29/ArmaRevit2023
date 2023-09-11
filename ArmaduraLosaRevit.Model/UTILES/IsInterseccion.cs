using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    internal class IsInterseccion
    {


        public static bool IsInterseccionPoligonos_XY0(List<XYZ> poli1, List<XYZ> poli2 )
        {
            try
            {
         
                // poligono termine con punto inicial
                if (!Util.IsSimilarValor(poli1[0].DistanceTo(poli1.Last()), 0, 0.01))
                    poli1.Add(poli1[0]);


                // poligono termine con punto inicial
                if (!Util.IsSimilarValor(poli2[0].DistanceTo(poli2.Last()), 0, 0.01))
                    poli2.Add(poli2[0]);

                bool _result = false;

                for (int i = 0; i < poli1.Count-1; i++)
                {
                    for (int j = 0; j < poli2.Count-1; j++)
                    {
                        _result = Util.IsIntersection2(poli1[i].GetXY0(), poli1[i + 1].GetXY0(), poli2[j].GetXY0(), poli2[j + 1].GetXY0());

                        if(_result) return true;
                    }
                }

                return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'IsInterseccionPoligonos_XY0'. ex:{ex.Message}");
                return false;
            }
     
        }

    }
}
