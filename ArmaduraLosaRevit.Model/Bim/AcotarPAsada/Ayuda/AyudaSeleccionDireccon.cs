using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Ayuda
{
    internal class AyudaSeleccionDireccon
    {
        internal static EnumPasadas Ejecutar(XYZ ptoMASALto, EnvoltoriPasada envoltoriPasada)
        {
            try
            {
                var PLanarSUperior = envoltoriPasada._pasada.ObtenerCaraSuperior();
                XYZ ptoCentral = PLanarSUperior.ObtenerCenterDeCara();
                XYZ dire= (ptoMASALto.GetXY0()- ptoCentral.GetXY0()).Normalize();

                var caraSeleccon=envoltoriPasada.listaPLanos_Geometria.OrderByDescending(c => Util.GetProductoEscalar(c.FaceNormal, dire)).FirstOrDefault();
                if (caraSeleccon == null) return EnumPasadas.NONE;
                XYZ normal = caraSeleccon.FaceNormal;

                if (Math.Abs(normal.X) > Math.Abs(normal.Y))
                {
                    if (normal.X > 0)
                        return EnumPasadas.Derecha;
                    else
                        return EnumPasadas.Izquieda;
                }
                else 
                {
                    if (normal.Y > 0)
                        return EnumPasadas.Arriba;
                    else
                        return EnumPasadas.Bajo;
                }
            }
            catch (Exception)
            {
                return EnumPasadas.NONE;
            }
        }
    }
}
