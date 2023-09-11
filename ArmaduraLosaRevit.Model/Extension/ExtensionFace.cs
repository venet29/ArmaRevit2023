using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionFace
    {
        private static double CurvePoints_Umin;
        private static double CurvePoints_Umax;
        private static double CurvePoints_Vmin;
        private static double CurvePoints_Vmax;

        public static bool IsTopFace(this Face f)
        {
            BoundingBoxUV b = f.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midpoint = p + 0.5 * (q - p);
            XYZ normal = f.ComputeNormal(midpoint);
            return Util.PointsUpwards(normal);
        }
        public static XYZ ObtenerNormal(this Face f)
        {
            BoundingBoxUV b = f.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midpoint = p + 0.5 * (q - p);
            XYZ normal = f.ComputeNormal(midpoint);
            return normal;
        }

        public static bool IsDownFace(this Face f)
        {
            BoundingBoxUV b = f.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midpoint = p + 0.5 * (q - p);
            XYZ normal = f.ComputeNormal(midpoint);
            return Util.Pointsdownwards(normal);
        }

        public static List<Curve> ObtenerListaCurvas(this Face f)
        {
            List<Curve> listaCurve = new List<Curve>();
            try
            {
                if (f == null) return listaCurve;
                //  f.GetEdgesAsCurveLoops().SelectMany(c=>c.SelectMany()).
                List<CurveLoop> list = f.GetEdgesAsCurveLoops().ToList().ToList();

                foreach (CurveLoop cl in list)
                {
                    foreach (Curve _curve in cl)
                    {
                        if (_curve.Length < 0.0001) continue;
                        listaCurve.Add(_curve);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener el planarface ex:{ex.Message}");
                listaCurve.Clear();
                return listaCurve;
            }
            return listaCurve;
        }

        public static List<XYZ> ObtenerListaPuntos(this Face f)
        {
            List<XYZ> listaCurve = new List<XYZ>();
            try
            {
                listaCurve = (List<XYZ>)f.Triangulate().Vertices;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener el ptos planarface ex:{ex.Message}");
                listaCurve.Clear();
            }
            return listaCurve;
        }


        public static XYZ ObtenerCenterDeCara(this Face MyFace)
        {
            CalcularALargosVU_maximoYmoin(MyFace);

            UV MyCenter = new UV((CurvePoints_Umax + CurvePoints_Umin) / 2, (CurvePoints_Vmax + CurvePoints_Vmin) / 2);
            XYZ ptcentro = MyFace.Evaluate(MyCenter);
            return ptcentro;
        }


        public static double MaximoladoLArgo(this Face MyFace)
        {
            CalcularALargosVU_maximoYmoin(MyFace);

            return Math.Max(CurvePoints_Umax - CurvePoints_Umin, CurvePoints_Vmax - CurvePoints_Vmin);
        }

        private static void CalcularALargosVU_maximoYmoin(Face MyFace)
        {
            CurvePoints_Umin = double.MaxValue;
            CurvePoints_Umax = double.MinValue;
            CurvePoints_Vmin = double.MaxValue;
            CurvePoints_Vmax = double.MinValue;
            List<List<UV>> EdgePointsUV = new List<List<UV>>();
            foreach (EdgeArray MyEdgeArray in MyFace.EdgeLoops)
            {
                foreach (Edge MyEdge in MyEdgeArray)
                {
                    foreach (UV MyUV in MyEdge.TessellateOnFace(MyFace))
                    {
                        CurvePoints_Umin = Math.Min(CurvePoints_Umin, MyUV.U);
                        CurvePoints_Umax = Math.Max(CurvePoints_Umax, MyUV.U);
                        CurvePoints_Vmin = Math.Min(CurvePoints_Vmin, MyUV.V);
                        CurvePoints_Vmax = Math.Max(CurvePoints_Vmax, MyUV.V);
                    }
                }
            }
        }

        //obtiene la interseccion con face de un pto,
        // la form  1 los hace proyectnado un linea he intersectando con el plano ( acotado)
        // la forma 2   hace la proyeccion con una linea y un plano q simula el plano(plano infinito)
        // la forma 3 tercera forma es la que entraga  revit para intersectar pto con plano, pero genera error en algunso casos, cuando pto esta sobre plano
        public static XYZ ProjectNH(this Face pl, XYZ ptoInter, bool IsMensaje=true)
        {
            if(pl==null) return XYZ.Zero;
            XYZ ptoInterseccion = XYZ.Zero;
            var planar = (PlanarFace)pl;
            var nomarlPLAno = planar.FaceNormal;
            
            //fomar 1)
            ptoInterseccion = planar.ObtenerPtosInterseccionFace(ptoInter, nomarlPLAno, IsMensaje);

            //forma 2)
            if (ptoInterseccion.IsLargoCero())            
                ptoInterseccion = planar.GetPtosIntersFaceUtilizarPlanoNh(ptoInter);
            
            //forma 3
            if (ptoInterseccion.IsLargoCero())
            {
                // la tres liena inferieores son el codigo original
                var resul = pl.Project(ptoInter);
                if (resul == null) return XYZ.Zero;
                ptoInterseccion = resul.XYZPoint;
            }
            return ptoInterseccion;// resul.XYZPoint;
        }

    }
}
