using ArmaduraLosaRevit.Model.Elementos_viga;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
  public static  class ExtensionViga
    {

        public static PlanarFace ObtenerCaraInferior(this FamilyInstance familyInstance)
        {
          //  return obtenerFaceLosa(familyInstance, Util.Pointsdownward_soloViga);


            GeometriaViga _geometriaBase = new GeometriaViga(familyInstance.Document);
            _geometriaBase.M1_AsignarGeometriaObjecto(familyInstance);
            PlanarFace FaceInferior = _geometriaBase.listaPlanarFace.Where(c => Util.Pointsdownward_soloViga(c.FaceNormal)).FirstOrDefault();
            return FaceInferior;
        }

        public static PlanarFace ObtenerCaraSuperior(this FamilyInstance familyInstance)
        {
            GeometriaViga _geometriaBase = new GeometriaViga(familyInstance.Document);
            _geometriaBase.M1_AsignarGeometriaObjecto(familyInstance);
            PlanarFace FaceSuperior = _geometriaBase.listaPlanarFace.Where(c => Util.PointsUpwards_soloViga(c.FaceNormal)).FirstOrDefault();

            return FaceSuperior;
        }

        public static double ObtenerEspesorConCaraVerical_foot(this FamilyInstance familyInstance)
        {
            GeometriaViga _geometriaBase = new GeometriaViga(familyInstance.Document);
            _geometriaBase.M1_AsignarGeometriaObjecto(familyInstance);
            PlanarFace FaceSuperior = _geometriaBase.listaPlanarFace.Where(c => !(Util.IsSimilarValor(c.FaceNormal.X,0,0.0001) && 
                                                                                 Util.IsSimilarValor(c.FaceNormal.Y, 0, 0.0001))).OrderByDescending(c=> c.Area).FirstOrDefault();
            double espesor_foot=familyInstance.ObtenerEspesorConPtos_foot(FaceSuperior.ObtenerCenterDeCara(), -FaceSuperior.FaceNormal);// 

            return espesor_foot;
        }

        public static double ObtenerLargo(this FamilyInstance familyInstance)
        {
            double largo = ((Line)((LocationCurve)((familyInstance).Location)).Curve).Length;

            return largo;
        }
      



    }
}
