using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionPlanarFace
    {
        public static XYZ ObtenerPtosInterseccionFaceHorizontal(this PlanarFace _planarFace, XYZ ptoselec, bool _ISMensajes = false)
        {
            XYZ ptoInters = XYZ.Zero;
            try
            {
                if (_planarFace == null) return ptoInters;
                ptoInters = ObtenerPtosInterseccionFace(_planarFace, ptoselec, new XYZ(0, 0, 1), _ISMensajes);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerPtosInterseccionFaceHorizontal'. Ex:{ex.Message}");
            }
            return ptoInters;
        }
        public static XYZ ObtenerPtosInterseccionFace(this PlanarFace _planarFace, XYZ ptoselec, XYZ direccon , bool ISMensajes)
        {
           
            XYZ ptoInters = XYZ.Zero;
            try
            {
                // si esta sobre el mismo plano y plano es horizontal
                if (_planarFace.IsNormalEnZ() && Util.IsSimilarValor(_planarFace.Origin.Z, ptoselec.Z, 0.1))
                    return ptoselec;


                var ListaPlanarFaceSuperior = new List<PlanarFace>() { _planarFace };
                Curve lineVertcal = Line.CreateBound(ptoselec + direccon * 50, ptoselec - direccon * 50);
                ptoInters = XYZ.Zero;
                //****************************
                ptoInters = ExtensionFloorAyuda.ObtenerPto_ConListaPlanarFace(ListaPlanarFaceSuperior, lineVertcal, false);
                //**
                if (ptoInters.IsAlmostEqualTo(XYZ.Zero))
                {
                    ptoInters = ObtenerPtosInterseccionFace_utilizarPlano_YCondireccion(_planarFace, ptoselec, direccon,false);
                    if (ptoInters.IsAlmostEqualTo(XYZ.Zero))
                    {
                        if (ISMensajes)
                            Util.ErrorMsg($"Error al obtener punto intersecion con face superior. Se utliza punto 'origen' de face superior (caso A).\n\nPto:{ptoselec.REdondearString_foot(4)} ");

                        ptoInters = GetPtosIntersFaceUtilizarPlanoNh(_planarFace, ptoselec);
                        return ptoInters;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerPtosInterseccionFace'. Ex:{ex.Message}");
            }
            return ptoInters;
        }

        // se utiliza especialmente cuando las barras esta fuera del plano, ejemplo vigas sobre losa y barra esta en losa
        public static XYZ GetPtosIntersFaceUtilizarPlanoNh(this PlanarFace _planarFace, XYZ ptoselec)
        {

            try
            {
                XYZ ptoInters = XYZ.Zero;

                ptoInters = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(_planarFace.FaceNormal, _planarFace.Origin, ptoselec);
                //Plane geomPlane = Plane.CreateByNormalAndOrigin(_planarFace.FaceNormal, _planarFace.Origin); // 2017

                //ptoInters = geomPlane.ProjectOnto(ptoselec);

                if (ptoInters.IsAlmostEqualTo(XYZ.Zero))
                {
                    Util.ErrorMsg($"Error al obtener punto intersecion con face superior. Se utliza punto 'origen' de face superior (caso B).\n\nPto:{ptoselec.REdondearString_foot(4)}");
                    return ptoselec;
                }
                return ptoInters;
            }
            catch (Exception ex)
            {
                //Util.ErrorMsg($"Error en 'ObtenerPtosInterseccionFace'. Ex:{ex.Message}");
                return XYZ.Zero;
            }
           
        }

        // punto interssecion planarface infinito y un punto con una direccion
        public static XYZ ObtenerPtosInterseccionFace_utilizarPlano_YCondireccion(this PlanarFace _planarFace, XYZ ptoselec, XYZ direccion, bool ISMensajes)
        {
            XYZ ptoInters = XYZ.Zero;
            try
            {

                XYZ PA = _planarFace.Origin; // Punto en el plano
                XYZ Pnorm = _planarFace.FaceNormal; // Vector de dirección del plano

                XYZ L = ptoselec; // Punto en la línea
                XYZ V = direccion; // Vector de dirección de la línea

                double t = (Util.GetProductoEscalar(Pnorm, PA) - Util.GetProductoEscalar(Pnorm, L)) / Util.GetProductoEscalar(Pnorm, V);

                ptoInters = L + t * V;

                if (ptoInters.IsAlmostEqualTo(XYZ.Zero))
                {
                    Util.ErrorMsg($"Error al obtener punto intersecion con face superior. Se utliza punto 'origen' de face superior (caso B).\n\nPto:{ptoselec.REdondearString_foot(4)} ");
                    return ptoselec;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerPtosInterseccionFace'. Ex:{ex.Message}");
            }
            return ptoInters;
        }


        public static double ObtenerDistaciaAFace_ConPtoYDireccion_utilzaPlano(this PlanarFace _planarFace, XYZ ptoselec, bool _ISMensajes = false)
        {
            double distancia = 0; ;
            try
            {
                var puntoSobreFace = GetPtosIntersFaceUtilizarPlanoNh(_planarFace, ptoselec);

                distancia = puntoSobreFace.DistanceTo(ptoselec);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerDistaciaAFace_ConPtoYDireccion_utilzPlano'. Ex:{ex.Message}");
            }
            return distancia;
        }

        public static bool IsPtosProyectadoVerticalmenteFace(this PlanarFace _planarFace, XYZ ptoselec, bool _ISMensajes = false)
        {
            XYZ ptoInters = XYZ.Zero; ;
            try
            {
                var ListaPlanarFaceSuperior = new List<PlanarFace>() { _planarFace };

                Curve lineVertcal = Line.CreateBound(ptoselec + new XYZ(0, 0, +50), ptoselec + new XYZ(0, 0, -50));

                ptoInters = XYZ.Zero;

                //****************************
                ptoInters = ExtensionFloorAyuda.ObtenerPto_ConListaPlanarFace(ListaPlanarFaceSuperior, lineVertcal, _ISMensajes);
                //**

                if (ptoInters.IsAlmostEqualTo(XYZ.Zero))
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'IsPtosProyectadoVerticalmenteFace'. Ex:{ex.Message}");
                return false;
            }

        }


        public static bool IsNormalEnXY(this PlanarFace _planarFace)
        {
            try
            {
                var norm = _planarFace.FaceNormal;

                if (!Util.IsSimilarValor(Math.Abs(norm.Z), 0, 0.00001)) return false;

                if (Util.IsSimilarValor(Math.Abs(norm.X), 0, 0.00001)) return false;
                if (Util.IsSimilarValor(Math.Abs(norm.Y), 0, 0.00001)) return false;

                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'IsPtosProyectadoVerticalmenteFace'. Ex:{ex.Message}");
                return false;
            }
        }




        public static bool IsNormalEnX(this PlanarFace _planarFace)
        {
            try
            {
                var norm = _planarFace.FaceNormal;

                if (!Util.IsSimilarValor(Math.Abs(norm.Y), 0, 0.00001)) return false;
                if (!Util.IsSimilarValor(Math.Abs(norm.Z), 0, 0.00001)) return false;

                if (Util.IsSimilarValor(Math.Abs(norm.X), 0, 0.00001)) return false;

                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'IsPtosProyectadoVerticalmenteFace'. Ex:{ex.Message}");
                return false;
            }
        }
        public static bool IsNormalEnY(this PlanarFace _planarFace)
        {
            try
            {
                var norm = _planarFace.FaceNormal;
                if (!Util.IsSimilarValor(Math.Abs(norm.X), 0, 0.00001)) return false;
                if (!Util.IsSimilarValor(Math.Abs(norm.Z), 0, 0.00001)) return false;

                if (Util.IsSimilarValor(Math.Abs(norm.Y), 0, 0.00001)) return false;

                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'IsPtosProyectadoVerticalmenteFace'. Ex:{ex.Message}");
                return false;
            }
        }
        public static bool IsNormalEnZ(this PlanarFace _planarFace)
        {
            try
            {
                var norm = _planarFace.FaceNormal;
                if (!Util.IsSimilarValor(Math.Abs(norm.X), 0, 0.00001)) return false;
                if (!Util.IsSimilarValor(Math.Abs(norm.Y), 0, 0.00001)) return false;

                if (Util.IsSimilarValor(Math.Abs(norm.Z), 0, 0.00001)) return false;

                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'IsPtosProyectadoVerticalmenteFace'. Ex:{ex.Message}");
                return false;
            }
        }



        public static (XYZ, bool) ObtenerCentroCara(this PlanarFace plan)
        {
            if (plan == null) return (XYZ.Zero, false);
            BoundingBoxUV bb = plan.GetBoundingBox();
            UV ptocentro = (bb.Max + bb.Min) / 2;
            XYZ result = plan.Evaluate(ptocentro);
            return (result, true);
        }
    }
}
