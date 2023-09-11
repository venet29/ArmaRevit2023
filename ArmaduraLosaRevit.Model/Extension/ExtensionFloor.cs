using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionFloor
    {

        public static XYZ ObtenerNormal(this Floor selecFloor)
        {
            XYZ face_losa_Nomal = XYZ.Zero;
            PlanarFace face_losa_seleccionado = selecFloor.ObtenerPLanarFAce_superior();

            if (face_losa_seleccionado != null)
                face_losa_Nomal = face_losa_seleccionado.FaceNormal;
            else
            {
                var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(selecFloor);
                face_losa_Nomal = losa_Encontrada_RuledFace.normal;
            }

            return face_losa_Nomal;
        }


        public static double ObtenerEspesorLosaCm(this Floor selecFloor, Document _doc = null)
        {
            if (_doc == null)
                _doc = selecFloor.Document;
            string espesor_strin = ParameterUtil.FindParaByBuiltInParameter(selecFloor, BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM, _doc);

            double espesorDBL = selecFloor.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM).AsDouble();

            double aux_espesor = 0;
            if (double.TryParse(espesor_strin, out aux_espesor))
            {
                aux_espesor = Util.FootToCm(aux_espesor);
                aux_espesor = Math.Round(aux_espesor, 1);

            }

            return aux_espesor;
        }
        public static XYZ ObtenerOrigin(this Floor floor)
        {
            XYZ vector = ((Line)((LocationCurve)((floor).Location)).Curve).Origin;

            return vector;
        }





        //   private static XYZ ptoSUp;
        // private static XYZ ptoinf;
        //  private static bool ISMensajes;

        public static double ObtenerEspesorConPtosFloor(this Floor floor, XYZ ptoselec, bool _ISMensajes = false)
        {
            XYZ ptoSUp;
            XYZ ptoinf;
            double espesor = 0;

            try
            {
                var lsitaPara = floor.ListaFace();

                if (floor.Id.IntegerValue == 2106735)
                { 
                
                }
                var ListaPlanarFaceSuperior = floor.ListaFace()[0].Where(c => c.FaceNormal.Z > 0).ToList();
                var ListaPlanarFaceInferior = floor.ListaFace()[0].Where(c => c.FaceNormal.Z < 0).ToList();

                Curve lineVertcal = Line.CreateBound(ptoselec + new XYZ(0, 0, +5), ptoselec + new XYZ(0, 0, -5));

                ptoSUp = XYZ.Zero;
                ptoinf = XYZ.Zero;

                //****************************
                ptoSUp = ExtensionFloorAyuda.ObtenerPto_ConListaPlanarFace(ListaPlanarFaceSuperior, lineVertcal, _ISMensajes);
                //**
                ptoinf = ExtensionFloorAyuda.ObtenerPto_ConListaPlanarFace(ListaPlanarFaceInferior, lineVertcal, _ISMensajes);

                //busca si el cara es tipo 'RuledFace'
                if (ptoSUp.IsAlmostEqualTo(XYZ.Zero))
                {
                    var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(floor);
                    if (losa_Encontrada_RuledFace == null) return 0;
                    ptoSUp = losa_Encontrada_RuledFace.ReludfaceNH.ProjectNH(ptoselec);
                }
                //busca si el cara es tipo 'RuledFace'
                if (ptoinf.IsAlmostEqualTo(XYZ.Zero))
                {
                    var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceInferiorLosa_RuledFace(floor);
                    if (losa_Encontrada_RuledFace == null) return 0;
                    ptoinf = losa_Encontrada_RuledFace.ReludfaceNH.ProjectNH(ptoselec);
                }
                //***
                espesor = Math.Abs(ptoSUp.Z - ptoinf.Z);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener espesor losa en 'ObtenerEspesorConPtosFloor'. Ex:{ex.Message}");
            }
            return espesor;
        }



    }
}
