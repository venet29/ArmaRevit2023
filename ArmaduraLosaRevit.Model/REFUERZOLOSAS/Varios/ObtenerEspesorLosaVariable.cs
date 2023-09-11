using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Varios
{


    // fuuncion implementadad como extension de losa
    public class ObtenerEspesorLosaVariable
    {
        private static XYZ ptoSUp;
        private static XYZ ptoinf;
        private static bool ISMensajes;

        public static double ObtenerEspesorConPtos(Floor floor, XYZ ptoselec, bool _ISMensajes=false)
        {
            double espesor = 0;
            ISMensajes = _ISMensajes;
            try
            {
                //  PlanarFace PlanarFaceSuperior = floor.ObtenerCaraSuperior();
               // PlanarFace PlanarFaceInferior = floor.ObtenerCaraInferior();

                var ListaPlanarFaceSuperior = floor.ListaFace()[0].Where(c => c.FaceNormal.Z > 0).ToList();
                var ListaPlanarFaceInferior = floor.ListaFace()[0].Where(c => c.FaceNormal.Z < 0).ToList();

                Curve lineVertcal = Line.CreateBound(ptoselec + new XYZ(0, 0, +5), ptoselec + new XYZ(0, 0, -5));

                 ptoSUp = XYZ.Zero;
                 ptoinf = XYZ.Zero;


                //****************************
                ptoSUp = ObtenerPtoSuperior(ListaPlanarFaceSuperior, lineVertcal);
                //**
                ptoinf = ObtenerPtoInferior(ListaPlanarFaceInferior, lineVertcal);


                if (ptoSUp == XYZ.Zero || ptoinf == XYZ.Zero)
                {
                   // Util.ErrorMsg($"Error al obtener espesor Losa variable");
                    return 0;
                }

                //***
                espesor = Math.Abs(ptoSUp.Z - ptoinf.Z);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener espesor  de losa en 'ObtenerEspesorConPtos Losa Var'. Ex:{ex.Message}");
            }
            return espesor;

        }

        private static XYZ ObtenerPtoInferior( List<PlanarFace> ListaPlanarFaceInferior, Curve lineVertcal)
        {
            foreach (PlanarFace PlanarFaceInferior in ListaPlanarFaceInferior)
            {


                IntersectionResultArray resultsInferiore;
                SetComparisonResult resultInferior = PlanarFaceInferior.Intersect(lineVertcal, out resultsInferiore);
                if (resultInferior == SetComparisonResult.Overlap)
                {
                    IntersectionResult iResultINF = resultsInferiore.get_Item(0);
                    ptoinf = iResultINF.XYZPoint;
                }

            }

            if (ptoinf == XYZ.Zero && ISMensajes)
            {
                Util.ErrorMsg($"Error al obtener espesor Losa variable");
  
            }


            return ptoinf;
        }

        private static XYZ ObtenerPtoSuperior(List<PlanarFace> ListaPlanarFaceSuperior, Curve lineVertcal)
        {
            ptoSUp = XYZ.Zero;
            foreach (PlanarFace PlanarFaceSuperior in ListaPlanarFaceSuperior)
            {


                IntersectionResultArray resultsSuperior;
                SetComparisonResult resultSuperior = PlanarFaceSuperior.Intersect(lineVertcal, out resultsSuperior);
                if (resultSuperior == SetComparisonResult.Overlap)
                {
                    IntersectionResult iResult = resultsSuperior.get_Item(0);
                    ptoSUp = iResult.XYZPoint;
                    break;
                }

            }

            if (ptoSUp == XYZ.Zero && ISMensajes)
            {
                Util.ErrorMsg($"Error al obtener espesor Losa variable");

            }
            return ptoSUp;
        }
    }
}
