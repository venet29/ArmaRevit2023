using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Stairsnh.Servicio;

using System.Linq;
using System.Collections.Generic;
using System.Text;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Prueba;
using System;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;

using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.GEOM;

namespace ArmaduraLosaRevit.Model.Stairsnh.Entidades
{
    public class GeometrisStairAreaMax : GeometriaBase
    {
        private PlanarFace planarFaceMaxArea;

        public ServicioModificarCoordenadasEscalera _servicioModificarCoordenadas { get; set; }
        public GeometrisStairAreaMax(UIApplication _uiapp) : base(_uiapp)
        { }

        public void M1_GetGEomPlanarFaceMAxiamaArea(Stairs stairs)
        {
            M1_AsignarGeometriaObjecto(stairs);
        }

        public override void M3_AnalizarGeometrySolid(GeometryObject obj2)
        {
            Solid solid2 = obj2 as Solid;
            if (solid2.Faces.Size == 0) return;

            PlanarFace planarFaceMaxArea_aux = solid2.Faces.Cast<PlanarFace>().MinBy(c => -c.Area);

            if (planarFaceMaxArea == null)
            { planarFaceMaxArea = planarFaceMaxArea_aux; }
            else
            {
                if (planarFaceMaxArea_aux.Area > planarFaceMaxArea.Area)
                { planarFaceMaxArea = planarFaceMaxArea_aux; }
                else
                { return; }
            }
            #region Cara analizadas -- no tuitlizado

            //_planarFaceDeldaño = solid2.Faces.Cast<PlanarFace>().First(c => Util.IsVertical(c.FaceNormal));
            //PlanarFace planarFaceHorizontalMasAlto = solid2.Faces.Cast<PlanarFace>().Where(c => Util.GetProductoEscalar(c.FaceNormal.GetXY0().Normalize(), planarFaceMaxArea_aux.FaceNormal.GetXY0().Normalize()) == 1).MinBy(fc => -fc.Origin.Z);

            //List<PlanarFace> planarFacetrasversal = solid2.Faces.
            //                             Cast<PlanarFace>().
            //                             Where(c => Math.Abs(Util.GetProductoEscalar(c.FaceNormal.GetXY0().Normalize(), planarFaceMaxArea_aux.YVector.GetXY0().Normalize())) == 1).
            //                             ToList();

            #endregion

            M3_1_AnalisandoCAraInferior();
        }

        public bool M3_1_AnalisandoCAraInferior(bool IsGraficar = false)
        {
            if (planarFaceMaxArea == null) return false;

            _servicioModificarCoordenadas = new ServicioModificarCoordenadasEscalera(planarFaceMaxArea,0);
            _servicioModificarCoordenadas.M1_Obtener4ptosPrincipales();

            _servicioModificarCoordenadas.M2_ObtenerPtoReferenciaLines(planarFaceMaxArea.FaceNormal.GetXY0().Normalize(),planarFaceMaxArea.Origin);

            if (!IsGraficar) return true;

            CrearLIneaAux _crearLIneaAux = new CrearLIneaAux(_doc);
            _crearLIneaAux.CrearLinea(_servicioModificarCoordenadas.pto1, _servicioModificarCoordenadas.pto2);
            _crearLIneaAux.CrearLinea(_servicioModificarCoordenadas.pto2, _servicioModificarCoordenadas.pto3);
            _crearLIneaAux.CrearLinea(_servicioModificarCoordenadas.pto3, _servicioModificarCoordenadas.pto4);


            return true;
        }

        public double MaxLeng(EdgeArrayArray loops)
        {
            double maxLeng = 0;
            foreach (EdgeArray a in loops)
            {
                foreach (Edge e in a)
                {
                    maxLeng = (e.ApproximateLength > maxLeng ? e.ApproximateLength : maxLeng);
                }
            }
            return maxLeng;
        }


     

        internal PlanarFace GetPlanarFaceMaxArea()
        {
            return planarFaceMaxArea;
        }

        public RebarInferiorDTO ObtenerGEometriaLong(TipoBarra tipobarra, UbicacionLosa ubicacion)
        {

            return ObtenerGEometria(tipobarra, ubicacion, _servicioModificarCoordenadas.lista4ptos);
        }

        public RebarInferiorDTO ObtenerGEometriaTrasn(TipoBarra tipobarra, UbicacionLosa ubicacion)
        {

            return ObtenerGEometria(tipobarra, ubicacion, _servicioModificarCoordenadas.lista4ptosTrasversal);
        }
        public RebarInferiorDTO ObtenerGEometria( TipoBarra tipobarra, UbicacionLosa ubicacion,List<XYZ> _Lista4ptos)
        {
            
            RebarInferiorDTO rebarInferiorDTO = new RebarInferiorDTO(_uiapp);

            rebarInferiorDTO.ServicioModificarCoordenadasEscalera = _servicioModificarCoordenadas;
            //rebarInferebarInferiorDTOriorDTO.listaPtosPerimetroBarras.Add(new XYZ(-82.053810000, 9.317590000, 26.541990000) + new XYZ(0, 0, -Util.CmToFoot(2)));
            //rebarInferiorDTO.listaPtosPerimetroBarras.Add(new XYZ(-82.053810000, -12.335960000, 26.541990000) + new XYZ(0, 0, -Util.CmToFoot(2)));
            //rebarInferiorDTO.listaPtosPerimetroBarras.Add(new XYZ(-62.746060000, -12.335960000, 26.541990000) + new XYZ(0, 0, -Util.CmToFoot(2)));
            //rebarInferiorDTO.listaPtosPerimetroBarras.Add(new XYZ(-62.746060000, 9.317590000, 26.541990000) + new XYZ(0, 0, -Util.CmToFoot(2)));
            rebarInferiorDTO.listaPtosPerimetroBarras.AddRange(_Lista4ptos);
            List<XYZ> ListaPtosPerimetroBarras = _Lista4ptos;
            XYZ PtoConMouseEnlosa1 = new XYZ( ListaPtosPerimetroBarras.Average(c=>c.X), ListaPtosPerimetroBarras.Average(c => c.Y), ListaPtosPerimetroBarras.Average(c => c.Z));
            double Elevation = ListaPtosPerimetroBarras.Max(c => c.Z);

            rebarInferiorDTO.barraIni = Line.CreateBound(ListaPtosPerimetroBarras[0].GetXY0(), ListaPtosPerimetroBarras[1].GetXY0())
                                            .Project(PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(Elevation);

            rebarInferiorDTO.barraFin = Line.CreateBound(ListaPtosPerimetroBarras[3].GetXY0(), ListaPtosPerimetroBarras[2].GetXY0())
                                            .Project(PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(Elevation);

            rebarInferiorDTO.PtoDirectriz1 = Line.CreateBound(ListaPtosPerimetroBarras[1].GetXY0(), ListaPtosPerimetroBarras[2].GetXY0())
                                                 .Project(PtoConMouseEnlosa1).XYZPoint.AsignarZ(Elevation);
            rebarInferiorDTO.PtoDirectriz2 = Line.CreateBound(ListaPtosPerimetroBarras[0].GetXY0(), ListaPtosPerimetroBarras[3].GetXY0())
                                                 .Project(PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(Elevation);

            double espesor = Util.CmToFoot(15);
            double espaciamiento = Util.CmToFoot(20);
            int diamm = 8;
            bool IsLuzSecuandiria = false;

            rebarInferiorDTO.espesorLosaFoot = espesor;// Util.CmToFoot(15)

            if (rebarInferiorDTO.espesorLosaFoot <= 0)
            {
                Util.ErrorMsg($"Error en el espesor de Room  e:{rebarInferiorDTO.espesorLosaFoot }");
                rebarInferiorDTO.IsOK = false;
                return rebarInferiorDTO;

            }

          
            //rebarInferiorDTO.numeroBarra = 20;
            rebarInferiorDTO.diametroMM = diamm;
            rebarInferiorDTO.tipoBarra = tipobarra;
            rebarInferiorDTO.ubicacionLosa = ubicacion;
            rebarInferiorDTO.espaciamientoFoot = espaciamiento;// Util.CmToFoot(15);
            rebarInferiorDTO.largo_recorridoFoot = ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]);// Util.CmToFoot(660);
            rebarInferiorDTO.floor = _elemento;//  _uiapp.ActiveUIDocument.Document.GetElement(new ElementId(1111503));

            rebarInferiorDTO.largomin_1 =100;

            //rebarInferiorDTO.espesorBarraFooT = rebarInferiorDTO.espesorLosaFoot 
            //                                    - Util.CmToFoot(ConstantesGenerales.RECUBRIMIENTO_LOSA_INF + ConstantesGenerales.RECUBRIMIENTO_LOSA_SUP);
            rebarInferiorDTO.ptoSeleccionMouse = PtoConMouseEnlosa1;

            int AcortamientoEspesorSecundario = (IsLuzSecuandiria == true ? 1 : 0);
            rebarInferiorDTO.AcortamientoEspesorSecundario = AcortamientoEspesorSecundario;
            rebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT = rebarInferiorDTO.espesorLosaFoot + Util.CmToFoot(-ConstNH.RECUBRIMIENTO_LOSA_SUP_cm - ConstNH.RECUBRIMIENTO_LOSA_INF_cm - AcortamientoEspesorSecundario);

            rebarInferiorDTO.anguloBarraGrados = Util.AnguloEntre2PtosGrados_enPlanoXY(rebarInferiorDTO.barraIni, rebarInferiorDTO.barraFin);
            rebarInferiorDTO.anguloBarraRad = Util.GradosToRadianes(rebarInferiorDTO.anguloBarraGrados);

            rebarInferiorDTO.anguloTramoRad = 0;
            rebarInferiorDTO.LargoPata = Util.CmToFoot(100);
            rebarInferiorDTO.IsOK = true;
            return rebarInferiorDTO;
        }



    }
}