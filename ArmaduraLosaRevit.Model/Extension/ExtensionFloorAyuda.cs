using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionFloorAyuda
    {

        public static List<IntersectionResultNH> ObtenerListaIntersectionResult(List<PlanarFace> ListaPlanarFace, Curve lineVertcal, bool ISMensajes = false)
        {
            List<IntersectionResultNH> listaptos = new List<IntersectionResultNH>();

            foreach (PlanarFace PlanarFaceSuperior in ListaPlanarFace)
            {

                IntersectionResultNH _resultadoInterseccion = new IntersectionResultNH();

                IntersectionResultArray resultsSuperior;
                SetComparisonResult resultSuperior = PlanarFaceSuperior.Intersect(lineVertcal, out resultsSuperior);
                if (resultSuperior == SetComparisonResult.Overlap)
                {
                    IntersectionResult iResult = resultsSuperior.get_Item(0);

                    _resultadoInterseccion.Isok = true;
                    _resultadoInterseccion.ptoInterseccion = iResult.XYZPoint;
                    _resultadoInterseccion.planarInterseccion = PlanarFaceSuperior;       

                    listaptos.Add(_resultadoInterseccion);
                }


            }

            return listaptos;
        }
        public static XYZ ObtenerPto_ConListaPlanarFace(List<PlanarFace> ListaPlanarFace, Curve lineVertcal, bool ISMensajes)
        {
            XYZ ptoInterseccion = XYZ.Zero;
            foreach (PlanarFace PlanarFaceSuperior in ListaPlanarFace)
            {


                IntersectionResultArray resultsSuperior;
                SetComparisonResult resultSuperior = PlanarFaceSuperior.Intersect(lineVertcal, out resultsSuperior);
                if (resultSuperior == SetComparisonResult.Overlap)
                {
                    IntersectionResult iResult = resultsSuperior.get_Item(0);
                    ptoInterseccion = iResult.XYZPoint;
                    break;
                }

            }

            if (ptoInterseccion.IsAlmostEqualTo(XYZ.Zero) && ISMensajes)
            {
                Util.ErrorMsg($"Error al obtener espesor Losa variable");
            }
            return ptoInterseccion;
        }

        public static PlanarFace ObtenerPlanarFace(List<PlanarFace> ListaPlanarFace, Curve lineVertcal, bool ISMensajes = false)
        {
            PlanarFace planarfaceInterseccion = null;
            foreach (PlanarFace _lanarFace in ListaPlanarFace)
            {


                IntersectionResultArray resultsSuperior;
                SetComparisonResult resultSuperior = _lanarFace.Intersect(lineVertcal, out resultsSuperior);
                if (resultSuperior == SetComparisonResult.Overlap)
                {
                    IntersectionResult iResult = resultsSuperior.get_Item(0);
                    planarfaceInterseccion = _lanarFace;
                    break ;
                }

            }

            if (planarfaceInterseccion==null && ISMensajes)
            {
                Util.ErrorMsg($"Error al obtener planar face");

            }
            return planarfaceInterseccion;
        }
    }

    public class IntersectionResultNH
    {
        public bool Isok { get; set; }
        public XYZ ptoInterseccion { get; set; }
        public PlanarFace planarInterseccion { get; set; }

        public IntersectionResultNH()
        {
            Isok = false;
        }
    }

}
