using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;

using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using MediaNH = System.Windows.Media;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Fauto;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Geometria;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom
{
    public class BarraRoomGeometria
    {
        private static string TipoBarraInicial;

        // propeidades
        public static int espesor_izq_bajo { get; set; }
        public static int espesor_dere_sup { get; set; }

        public static List<XYZ> ListaInterDireLosa { get; set; }
        public static List<XYZ> ListaInterDirePerpLosa { get; set; }

        public static double LargoDireLosa { get; set; }
        public static double LargoDirePerpLosa { get; set; }

        public static string DireLosaHor { get; set; }
        public static string DireLosaVert { get; set; }

        public static XYZ ptoSelect { get; set; }
        public static XYZ ptoSelect_2 { get; set; }
        public static XYZ ptoSelect_tras { get; set; }

        public static double LargoRecorridoX { get; set; }
        public static double LargoRecorridoY { get; set; }

        public static List<WrapperBoundarySegment> listBorde { get; set; }
        public static AnalisisFauto _analisisFauto { get; private set; }
        public static string tipoBarra { get; set; }
        public static string tipoBarra_izq_infer { get; set; }
        public static string tipoBarra_dere_sup { get; set; }
        public static UbicacionLosa ubicacionBArraEnlosa { get; set; }
        public static bool IsBuscarTipoBarra { get; internal set; }
        public static bool IsLosaINclinada { get; set; }
        public static bool Isok { get; set; }


        //*****************************
        #region 1)COntructor


        public BarraRoomGeometria()
        {



        }
        #endregion

        #region 2.3) metodos para obtener geome de pologono de losas y barra

        private static void clearValores()
        {

            LargoDireLosa = 0;
            LargoDirePerpLosa = 0;

            if (ListaInterDireLosa == null) ListaInterDireLosa = new List<XYZ>();
            ListaInterDireLosa.Clear();
            if (ListaInterDirePerpLosa == null) ListaInterDirePerpLosa = new List<XYZ>();
            ListaInterDirePerpLosa.Clear();
        }

        /// <summary>
        /// crea curva con puntos del poligono del area a reforzar
        /// </summary>
        /// <param name="lista">lista pto poligono area a reforzar </param>
        /// <returns>curva que se usa en 'AreaReinforcement'</returns>
        public static IList<Curve> CrearCurva(List<XYZ> lista)
        {


            IList<Curve> curveList = new List<Curve>();

            XYZ point1 = new XYZ();
            XYZ point2 = new XYZ();
            XYZ point3 = new XYZ();
            XYZ point4 = new XYZ();

            point1 = lista[0];

            point2 = lista[1];

            point3 = lista[2];

            point4 = lista[3];

            Line line1;
            Line line2;
            Line line3;
            Line line4;

            line1 = Autodesk.Revit.DB.Line.CreateBound(point1, point2);
            line2 = Autodesk.Revit.DB.Line.CreateBound(point2, point3);
            line3 = Autodesk.Revit.DB.Line.CreateBound(point3, point4);
            line4 = Autodesk.Revit.DB.Line.CreateBound(point4, point1);

            curveList.Add(line1);
            curveList.Add(line2);
            curveList.Add(line3);
            curveList.Add(line4);

            return curveList;
        }



        //obtiene los perimetros del lista con los putos del perimetro del path
        /*
           a)
           list<XYZZ
            1------4
            2------3

            b) lista con las curvas  de los path             
        */
        public static Tuple<List<XYZ>, IList<Curve>> ListaFinal_ptov2(List<XYZ> listaPtos, UIApplication uiapp,
                                                                      SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom, SolicitudBarraDTO _solicitudBarraDTO,
                                                                      ReferenciaRoomDatos roomAnalizado, bool IsTest)
        {
            List<XYZ> _listaPtoFinal = new List<XYZ>();
            IList<Curve> _curvesPathreiforment = new List<Curve>();

            Element floor_1 = _seleccionarLosaBarraRoom.LosaSeleccionada1 as Floor;

            var face = floor_1.ObtenerCaraSuperior(_seleccionarLosaBarraRoom.PtoConMouseEnlosa1, new XYZ(0,0,1));

            TipoBarraInicial = _solicitudBarraDTO.TipoBarra;
            TipoConfiguracionBarra tipobarra = _solicitudBarraDTO.TipoConfiguracionBarra;
            double anguloBarra_ = roomAnalizado.anguloBarraLosaGrado_1;
            double LargoMin_maximo1_soloParaSuple = roomAnalizado.largomin_1;
            double LargoMin_maximo2_soloParaSuple = roomAnalizado.largomin_2;


            Options gOptions = new Options();
            gOptions.ComputeReferences = false;
            gOptions.DetailLevel = ViewDetailLevel.Undefined;
            gOptions.IncludeNonVisibleObjects = false;
            GeometryElement geo = floor_1.get_Geometry(gOptions);

          //  var listasss = AyudaLosa.ObtenerListaRuledFace((Floor)floor_1);

            foreach (GeometryObject obj in geo) // 2013
            {
                Transform trans1 = null;
                Transform Invertrans1 = null;
                Transform trans2_rotacion = null;
                Transform InverTrans2_rotacion = null;
                //var tarsas=getInstGeo.GetTransformed();
                Solid floor_ = obj as Solid;
                if (floor_ == null) continue;


                foreach (Face f in floor_.Faces)
                {
                    BoundingBoxUV b = f.GetBoundingBox();
                    UV p = b.Min;
                    UV q = b.Max;
                    UV midparam = p + 0.5 * (q - p);
                    XYZ midpoint = f.Evaluate(midparam);
                    XYZ normal = f.ComputeNormal(midparam);
                    XYZ minxyz = f.Evaluate(b.Min);


                    //if (Util.IsVertical(normal) && Util.PointsUpwards(normal))
                    if (!_solicitudBarraDTO.tipofaceBusqueda(normal)) continue;

                    var PtoProyeccion = f.ProjectNH(_seleccionarLosaBarraRoom.PtoConMouseEnlosa1);
                    if (PtoProyeccion.IsLargoCero()) continue;
                   
                    XYZ ptXAxis = XYZ.BasisX;
                    XYZ ptYAxis = XYZ.BasisY;

                    _curvesPathreiforment.Clear();

                    // Plane plane1 = Plane.(XYZ.Zero, ptYAxis ,ptXAxis);
                    //trans1 = Transform.CreateTranslation(-minxyz);
                    double valorx = listaPtos.Min(val => val.X);
                    double valory = listaPtos.Min(val => val.Y);
                    //trans1 = Transform.CreateTranslation(new XYZ(-valorx, -valory, -minxyz.Z));
                    trans1 = Transform.CreateTranslation(-listaPtos[1]);

                    if (TipoBarraInicial.ToLower() == "s1" || TipoBarraInicial.ToLower() == "s3" || TipoBarraInicial.ToLower() == "s2" || (TipoBarraInicial.ToLower() == "s2" && tipobarra == TipoConfiguracionBarra.suple))  //                                                                 Angle_1/ Angle_1 => solo para amntener el signo
                    {
                        anguloBarra_ = Util.AnguloEntre2PtosGrado90(listaPtos[0], listaPtos[1], true);
                        trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, Util.GradosToRadianes(-90 - anguloBarra_), XYZ.Zero);// obser: suple1
                    }
                    else
                    {
                        trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(anguloBarra_), XYZ.Zero);
                    }
                    Invertrans1 = trans1.Inverse;
                    InverTrans2_rotacion = trans2_rotacion.Inverse;
                    //trans1.Origin = listaPtos[3];



                    //p1,p2,p3,p4 pto  del segemneto que 
                    XYZ p1 = new XYZ();

                    XYZ p2 = new XYZ();
                    XYZ p3 = new XYZ();
                    XYZ p4 = new XYZ();
                    p1 = trans2_rotacion.OfPoint(trans1.OfPoint(listaPtos[0]));
                    p2 = trans2_rotacion.OfPoint(trans1.OfPoint(listaPtos[1]));

                    p3 = trans2_rotacion.OfPoint(trans1.OfPoint(listaPtos[2]));
                    p4 = trans2_rotacion.OfPoint(trans1.OfPoint(listaPtos[3]));


                    //LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);

                    // if (!IsTest) GraficarPtos(p1, p2, p3, p4);


                    //obtiene tipo de barra
                    if (TipoBarraInicial.ToLower() == "s1" || TipoBarraInicial.ToLower() == "s3" || TipoBarraInicial.ToLower() == "s2" || (TipoBarraInicial.ToLower() == "s2" && tipobarra == TipoConfiguracionBarra.suple))
                    { }
                    else if (TipoBarraInicial.ToLower() == "f1_SUP" && !IsBuscarTipoBarra)
                    {

                    }
                    else
                    {
                        if (ptoSelect != null) ptoSelect_tras = trans2_rotacion.OfPoint(trans1.OfPoint(ptoSelect));
                        //buscartipo de barra
                        if (IsBuscarTipoBarra)
                        {
                            _analisisFauto = new AnalisisFauto(listBorde, p1, p2, p3, p4, _solicitudBarraDTO.TipoOrientacion);
                            tipoBarra = _analisisFauto.BuscarConfiParaCasoFautoV2();
                            //  tipoBarra = BuscarConfiParaCasoFauto(p1, p2, p3, p4, _solicitudBarraDTO.TipoOrientacion);
                        }

                    }

                    // deja los puntos segun obs: suple2
                    OrdenarPtosTransform(ref p1, ref p2, ref p3, ref p4);
                    if (TipoBarraInicial.ToLower() == "s1" || TipoBarraInicial.ToLower() == "s3" || TipoBarraInicial.ToLower() == "s2" || (TipoBarraInicial.ToLower() == "s2" && tipobarra == TipoConfiguracionBarra.suple))
                    {
                        // PtosParaBarraSuple(_listaPtoFinal, Invertrans1, InverTrans2_rotacion, p1, p2, p3, p4, Math.Max(LargoMin_maximo1_soloParaSuple, LargoMin_maximo2_soloParaSuple), Math.Max(LargoMin_maximo1_soloParaSuple, LargoMin_maximo2_soloParaSuple), ref _curvesPathreiforment);

                        var result = PtosParaBarraSuplev2(Invertrans1, InverTrans2_rotacion, p1, p2, p3, p4, Math.Max(LargoMin_maximo1_soloParaSuple, LargoMin_maximo2_soloParaSuple), Math.Max(LargoMin_maximo1_soloParaSuple, LargoMin_maximo2_soloParaSuple));
                        _listaPtoFinal = result.Item1;

                        IObtenerZPara4ptosPath obtenerZPara4ptosPath = FactoryObtenerZPara4ptosPath.Crear_ObtenerZPara4ptosPath(uiapp, _listaPtoFinal, f);
                        _listaPtoFinal = obtenerZPara4ptosPath.M1_Obtener4PtoConZCorrespondiente();

                        _curvesPathreiforment = result.Item2;
                        //  if (!IsTest) GraficarPtos(_listaPtoFinal[0], _listaPtoFinal[1], _listaPtoFinal[2], _listaPtoFinal[3]);
                    }
                    else if (_solicitudBarraDTO.IsCasoS4)
                    {
                        GeometriaS4 newGeometriaS4 = new GeometriaS4(Invertrans1, InverTrans2_rotacion, p1, p2, p3, p4, _solicitudBarraDTO.UbicacionEnlosa);
                        newGeometriaS4.Generar4Ptos();
                        _listaPtoFinal = newGeometriaS4.Lista;
                        _curvesPathreiforment = PathBarraInferior(_listaPtoFinal, TipoBarraInicial, anguloBarra_, roomAnalizado.largomin_1);
                    }
                    else
                    {
                        _listaPtoFinal = PtosParaBarraInferiorv2(Invertrans1, InverTrans2_rotacion, p1, p2, p3, p4, _solicitudBarraDTO.TipoOrientacion, _solicitudBarraDTO);
                        if (_listaPtoFinal.Count == 0) return null;

                        IObtenerZPara4ptosPath obtenerZPara4ptosPath = FactoryObtenerZPara4ptosPath.Crear_ObtenerZPara4ptosPath(uiapp, _listaPtoFinal, f);
                        _listaPtoFinal = obtenerZPara4ptosPath.M1_Obtener4PtoConZCorrespondiente();

                        _curvesPathreiforment = PathBarraInferior(_listaPtoFinal, TipoBarraInicial, anguloBarra_, roomAnalizado.largomin_1);
                    }

                    break;

                }

            }

            return Tuple.Create(_listaPtoFinal, _curvesPathreiforment);
        }


       


        /// <summary>
        /// ordena ptos de que estan trasnformadosde modo que tengas esta configuracion
        /// p1 -- p4
        ///  |     |
        /// p2 -- p3
        /// ordenan todos de modo que p1 y p2 queden ala izq y p3,p4 a la derecha
        /// </summary>
        /// <param name="p1"> pto de linea1</param>
        /// <param name="p2"> pto de linea1 </param>
        /// <param name="p3"> pto de linea2 </param>
        /// <param name="p4"> pto de linea2 </param>
        public static Result OrdenarPtosTransform(ref XYZ p1, ref XYZ p2, ref XYZ p3, ref XYZ p4)
        {



            if (Util.IsSimilarValor(p1.X, 0, 0.0003) && p1.Z == 0 &&
               Util.IsSimilarValor(p1.X, 0, 0.0003) && p1.Z == 0 &&
                Util.IsSimilarValor(p1.X, 0, 0.0003) && p1.Z == 0 &&
                 Util.IsSimilarValor(p1.X, 0, 0.0003) && p1.Z == 0)
            {
                // nose pq se agres un cm acada lado al tener p1.x==0
                //p1 = p1 - new XYZ(Util.CmToFoot(1), 0, 0);
                //p2 = p2 - new XYZ(Util.CmToFoot(1), 0, 0);
                //p3= p3 + new XYZ(Util.CmToFoot(1), 0, 0);
                //p4 = p4 + new XYZ(Util.CmToFoot(1), 0, 0);


            }

            Result resulta = Result.Cancelled;
            // si los pt1 y pt2 esta en lalinea de la derecha
            if (p4.X < p1.X)
            {
                XYZ aux_p4 = p4;
                XYZ aux_p3 = p3;
                p4 = p1;
                p3 = p2;
                p2 = aux_p3;
                p1 = aux_p4;
            }



            //priemra linea
            if (p2.Y > p1.Y)
            {
                XYZ aux_p1 = p1;
                p1 = p2;
                p2 = aux_p1;
                resulta = Result.Succeeded;
            }
            else if (p2.Y > p1.Y)
            {
                TaskDialog.Show("Error ", " P1 y p2, son Igueles ");
                return resulta = Result.Failed;
            }
            else
            { resulta = Result.Succeeded; }  //ok las configuracion de 1er linea es correcta


            //segunda linea
            if (p3.Y > p4.Y)
            {
                XYZ aux_p4 = p4;
                p4 = p3;
                p3 = aux_p4;
                resulta = Result.Succeeded;
            }
            else if (p3.Y > p4.Y)
            {
                TaskDialog.Show("Error ", " P1 y p2, son Igueles ");
                return resulta = Result.Failed;
            }
            else
            { resulta = Result.Succeeded; }  //ok las configuracion de 1er linea es correcta

            return resulta;
        }



        //MUEVE LAS PATH SYMBOL DEL CASO F1_SUP_   :Obs2
        internal static Tuple<XYZ, XYZ> ObtenerPtoMouseF1_SUP(ReferenciaRoomDatos refereciaRoomDatos, List<XYZ> listaPtosPerimetroBarras)
        {
            //lado izq - bajo

            Curve curvePathIzq = Line.CreateBound(listaPtosPerimetroBarras[0], listaPtosPerimetroBarras[1]);
            XYZ ptoProyectEnCurvePathIzq = curvePathIzq.Project(refereciaRoomDatos.PtoSeleccionMouse1).XYZPoint;
            //derecha superior
            Curve curvePathDERE = Line.CreateBound(listaPtosPerimetroBarras[3], listaPtosPerimetroBarras[2]);
            XYZ ptoProyectEnCurvePathDERE = curvePathDERE.Project(refereciaRoomDatos.PtoSeleccionMouse1).XYZPoint;

            //lado izq - bajo
            XYZ p0_x = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoProyectEnCurvePathIzq,
                                                                    Util.angulo_entre_pt_Rad_XY0(ptoProyectEnCurvePathIzq, ptoProyectEnCurvePathDERE),
                                                               Math.Max((refereciaRoomDatos.largomin_1 * ConstNH.PORCENTAJE_LARGO_PATA) / 2, ConstNH.LARGO_MIN_PATH_S4_FOOT / 2));

            XYZ p0_xY = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p0_x,
                                                       Util.angulo_entre_pt_Rad_XY0(listaPtosPerimetroBarras[1], listaPtosPerimetroBarras[0]),
                                                        Util.CmToFoot(ConstNH.DESPLAMIENTOS1_SUP));

            //derecha superior
            XYZ p1_x = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoProyectEnCurvePathDERE,
                                                 Util.angulo_entre_pt_Rad_XY0(ptoProyectEnCurvePathDERE, ptoProyectEnCurvePathIzq),
                                                   Math.Max((refereciaRoomDatos.largomin_1 * ConstNH.PORCENTAJE_LARGO_PATA) / 2, ConstNH.LARGO_MIN_PATH_S4_FOOT / 2));
            XYZ p1_xY = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p1_x,
                                                        Util.angulo_entre_pt_Rad_XY0(listaPtosPerimetroBarras[2], listaPtosPerimetroBarras[3]),
                                                        Util.CmToFoot(ConstNH.DESPLAMIENTOS1_SUP));

            //solo para diselo - solo resultado grafico
            if (ConstNH.MODO_DISEÑO == true)
            {
                MediaNH.PointCollection nh = new MediaNH.PointCollection();

                nh.Add(new System.Windows.Point(listaPtosPerimetroBarras[0].X, listaPtosPerimetroBarras[0].Y));
                nh.Add(new System.Windows.Point(listaPtosPerimetroBarras[1].X, listaPtosPerimetroBarras[1].Y));
                nh.Add(new System.Windows.Point(listaPtosPerimetroBarras[2].X, listaPtosPerimetroBarras[2].Y));
                nh.Add(new System.Windows.Point(listaPtosPerimetroBarras[3].X, listaPtosPerimetroBarras[3].Y));
                nh.Add(new System.Windows.Point(p0_x.X, p0_x.Y));
                nh.Add(new System.Windows.Point(p0_xY.X, p0_xY.Y));
                nh.Add(new System.Windows.Point(p1_x.X, p1_x.Y));
                nh.Add(new System.Windows.Point(p1_xY.X, p1_xY.Y));
                //  GraficarPtos DibujoPtosTrasladados = new GraficarPtos(nh);
                // DibujoPtosTrasladados.ShowDialog();
            }

            return Tuple.Create(p0_xY, p1_xY);
        }


        //MUEVE LAS PATH SYMBOL DEL CASO F1_SUP_   :Obs2
        internal static XYZ ObtenerPtoMouseParaDirectrizCasoAutomatico(ReferenciaRoomDatos refereciaRoomDatos, List<XYZ> listaPtosPerimetroBarras)
        {
            //lado izq - bajo

            Curve curvePathIzq = Line.CreateBound(listaPtosPerimetroBarras[0], listaPtosPerimetroBarras[1]);
            XYZ ptoProyectEnCurvePathIzq = curvePathIzq.Project(refereciaRoomDatos.PtoSeleccionMouse1).XYZPoint;
            //derecha superior
            Curve curvePathDERE = Line.CreateBound(listaPtosPerimetroBarras[3], listaPtosPerimetroBarras[2]);
            XYZ ptoProyectEnCurvePathDERE = curvePathDERE.Project(refereciaRoomDatos.PtoSeleccionMouse1).XYZPoint;

            double largoPath = listaPtosPerimetroBarras[1].DistanceTo(listaPtosPerimetroBarras[2]);
            //lado izq - bajo
            XYZ p0_RespectoIzq_inf = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoProyectEnCurvePathIzq,
                                                                    Util.angulo_entre_pt_Rad_XY0(ptoProyectEnCurvePathIzq, ptoProyectEnCurvePathDERE),
                                                                     largoPath * ConstNH.DistanciaDirectrizEnporcentaje);

            return p0_RespectoIzq_inf;
        }

        private static Tuple<List<XYZ>, IList<Curve>> PtosParaBarraSuplev2(Transform Invertrans1, Transform InverTrans2_rotacion,
                                               XYZ p1, XYZ p2, XYZ p3, XYZ p4, double largoMinIzq, double largoMinDere)
        {
            double CONST_FACTOR_LARGOSUPLE = ConstNH.CONST_FACTOR_LARGOSUPLE;
            XYZ pf1 = new XYZ();
            XYZ pf2 = new XYZ();
            XYZ pf3 = new XYZ();
            XYZ pf4 = new XYZ();

            IList<Curve> curvesPathreiforment = new List<Curve>();

            List<XYZ> ListaPtoFinal = new List<XYZ>();
            // largoMinIzq = Util.CmToFoot(largoMinIzq);
            //largoMinDere = Util.CmToFoot(largoMinDere);
            // se supone que los ptos p1,p2,p3,p4 ya tiene la sigueinte configuracion
            /// p1 -- p4
            ///  |     |
            /// p2 -- p3

            //hacia arriba
            if (p1.Y < p4.Y)  //p1 es menor hacia rriba
            {
                if (p1.X < p4.X)
                {
                    pf1 = new XYZ(p1.X - largoMinIzq * CONST_FACTOR_LARGOSUPLE, p1.Y, p1.Z);
                    pf4 = new XYZ(p4.X + largoMinDere * CONST_FACTOR_LARGOSUPLE, p1.Y, p4.Z);
                }
                else
                {
                    pf1 = new XYZ(p1.X + largoMinIzq * CONST_FACTOR_LARGOSUPLE, p1.Y, p1.Z);
                    pf4 = new XYZ(p4.X - largoMinDere * CONST_FACTOR_LARGOSUPLE, p1.Y, p4.Z);
                }
            }
            else
            {

                if (p1.X < p4.X)
                {
                    // p1f   pf4
                    pf1 = new XYZ(p1.X - largoMinIzq * CONST_FACTOR_LARGOSUPLE, p4.Y, p1.Z);
                    pf4 = new XYZ(p4.X + largoMinDere * CONST_FACTOR_LARGOSUPLE, p4.Y, p4.Z);
                }
                else
                {
                    // p4f   p1f
                    pf1 = new XYZ(p1.X + largoMinIzq * CONST_FACTOR_LARGOSUPLE, p4.Y, p1.Z);
                    pf4 = new XYZ(p4.X - largoMinDere * CONST_FACTOR_LARGOSUPLE, p4.Y, p4.Z);
                }

            }


            //hacia abajo
            if (p2.Y > p3.Y)  //p1 es menor hacia rriba
            {
                if (p2.X < p3.X)
                {
                    pf2 = new XYZ(p2.X - largoMinIzq * CONST_FACTOR_LARGOSUPLE, p2.Y, p2.Z);
                    pf3 = new XYZ(p3.X + largoMinDere * CONST_FACTOR_LARGOSUPLE, p2.Y, p3.Z);
                }
                else
                {
                    pf2 = new XYZ(p2.X + largoMinIzq * CONST_FACTOR_LARGOSUPLE, p2.Y, p2.Z);
                    pf3 = new XYZ(p3.X - largoMinDere * CONST_FACTOR_LARGOSUPLE, p2.Y, p3.Z);

                }
            }
            else
            {
                if (p2.X < p3.X)
                {
                    pf2 = new XYZ(p2.X - largoMinIzq * CONST_FACTOR_LARGOSUPLE, p3.Y, p2.Z);
                    pf3 = new XYZ(p3.X + largoMinDere * CONST_FACTOR_LARGOSUPLE, p3.Y, p3.Z);
                }
                else
                {
                    pf2 = new XYZ(p2.X + largoMinIzq * CONST_FACTOR_LARGOSUPLE, p3.Y, p2.Z);
                    pf3 = new XYZ(p3.X - largoMinDere * CONST_FACTOR_LARGOSUPLE, p3.Y, p3.Z);

                }

            }


            if (DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO && RedonderLargoBarras.RedondearPtos_5MasCercano(pf1, pf2, pf3, pf4))
            {
                pf1 = RedonderLargoBarras.CoordenadaPath4.p1;
                pf2 = RedonderLargoBarras.CoordenadaPath4.p2;
                pf3 = RedonderLargoBarras.CoordenadaPath4.p3;
                pf4 = RedonderLargoBarras.CoordenadaPath4.p4;
            }


            ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf1)));
            ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf2)));
            ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf3)));
            ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf4)));

            if (pf4.Y < pf3.Y)
            //curvesPathreiforment.Add(Line.CreateBound(ListaPtoFinal[2], ListaPtoFinal[3]));
            //curvesPathreiforment.Add(Line.CreateBound(ListaPtoFinal[3], ListaPtoFinal[2]));
            { curvesPathreiforment.Add(Line.CreateBound(ListaPtoFinal[0], ListaPtoFinal[1])); }
            else
            {
                // origne path =ListaPtoFinal[3]                
                curvesPathreiforment.Add(Line.CreateBound(ListaPtoFinal[3], ListaPtoFinal[2]));

            }

            var result = Tuple.Create(ListaPtoFinal, curvesPathreiforment);
            return result;

        }

        public static List<XYZ> PtosParaBarraInferiorv2(Transform Invertrans1, Transform InverTrans2_rotacion, XYZ p1, XYZ p2, XYZ p3, XYZ p4,
                                            TipoOrientacionBarra TipoOrientacion, SolicitudBarraDTO _solicitudBarraDTO)
        {

            List<XYZ> ListaPtoFinal = new List<XYZ>();

            /// ordena ptos de que estan trasnformadosde modo que tengas esta configuracion
            /// p1 -- p4
            ///  |     |
            /// p2 -- p3
            /// ordenan todos de modo que p1 y p2 queden ala izq y p3,p4 a la derecha






            Geometriafx _Geometriafx = new Geometriafx(Invertrans1, InverTrans2_rotacion, p1, p2, p3, p4, ptoSelect_tras, TipoOrientacion);
            if (!_Geometriafx.Generar4Ptos()) return ListaPtoFinal;
          
            XYZ pt1Inv = _Geometriafx.pt1Inv;
            XYZ pt2Inv = _Geometriafx.pt2Inv;
            XYZ pt3Inv = _Geometriafx.pt3Inv;
            XYZ pt4Inv = _Geometriafx.pt4Inv;


            FormaDibujarPAth _FormaDibujarPAth = (_solicitudBarraDTO.Ui_pathSymbolDTO_ != null ? _solicitudBarraDTO.Ui_pathSymbolDTO_.FormaDibujar_ : FormaDibujarPAth.Normal);

            if (_FormaDibujarPAth == FormaDibujarPAth.Normal)
            {

                ListaPtoFinal.Add(pt1Inv);
                ListaPtoFinal.Add(pt2Inv);
                ListaPtoFinal.Add(pt3Inv);
                ListaPtoFinal.Add(pt4Inv);
            }
            else if (_FormaDibujarPAth == FormaDibujarPAth.Inicial)
            {
                double Largo_foot = _solicitudBarraDTO.Ui_pathSymbolDTO_.Largo_Foot;
                XYZ direccionBArras_IniciaFinal = (pt3Inv - pt2Inv).Normalize();

                ListaPtoFinal.Add(pt1Inv);
                ListaPtoFinal.Add(pt2Inv);
                ListaPtoFinal.Add(pt2Inv + direccionBArras_IniciaFinal * Largo_foot);
                ListaPtoFinal.Add(pt1Inv + direccionBArras_IniciaFinal * Largo_foot);
            }
            else if (_FormaDibujarPAth == FormaDibujarPAth.Final)
            {
                double Largo_foot = _solicitudBarraDTO.Ui_pathSymbolDTO_.Largo_Foot;
                XYZ direccionBArras_IniciaFinal = (pt3Inv - pt2Inv).Normalize();
                ListaPtoFinal.Add(pt4Inv - direccionBArras_IniciaFinal * Largo_foot);
                ListaPtoFinal.Add(pt3Inv - direccionBArras_IniciaFinal * Largo_foot);
                ListaPtoFinal.Add(pt3Inv);
                ListaPtoFinal.Add(pt4Inv);

            }
            else if (_FormaDibujarPAth == FormaDibujarPAth.mouse)
            {
                double Largo_foot_Izq = _solicitudBarraDTO.Ui_pathSymbolDTO_.LargoIzq_foot;
                double Largo_foot_dere = _solicitudBarraDTO.Ui_pathSymbolDTO_.LargoDere_foot;

                XYZ direccionBArras_IniciaFinal = (pt3Inv - pt2Inv).Normalize();

                Line linea_14 = Line.CreateBound(pt1Inv, pt4Inv);
                XYZ inter_1_4 = linea_14.ProjectExtendida3D(_solicitudBarraDTO.Ui_pathSymbolDTO_.ptoMouse);

                Line linea_23 = Line.CreateBound(pt2Inv, pt3Inv);
                XYZ inter_2_3 = linea_23.ProjectExtendida3D(_solicitudBarraDTO.Ui_pathSymbolDTO_.ptoMouse);

                ListaPtoFinal.Add(inter_1_4 - direccionBArras_IniciaFinal * Largo_foot_Izq);
                ListaPtoFinal.Add(inter_2_3 - direccionBArras_IniciaFinal * Largo_foot_Izq);
                ListaPtoFinal.Add(inter_2_3 + direccionBArras_IniciaFinal * Largo_foot_dere);
                ListaPtoFinal.Add(inter_1_4 + direccionBArras_IniciaFinal * Largo_foot_dere);

            }





            return ListaPtoFinal;

        }


        public static IList<Curve> PathBarraInferior(List<XYZ> ListaPtoFinal, string TipoBarra, double anguloBarraGrado, double largomin)
        {

            /// ordena ptos de que estan trasnformadosde modo que tengas esta configuracion
            /// p1 -- p4
            ///  |     |
            /// p2 -- p3
            /// ordenan todos de modo que p1 y p2 queden ala izq y p3,p4 a la derecha


            IList<Curve> curvesPathreiforment = new List<Curve>();

            // XYZ daltaZ = new XYZ(0, 0, -Util.CmToFoot(8));

            if ((TipoBarra == "f9_") || (TipoBarra == "f9b_") || (TipoBarra == "f9a_"))// para dibujar las patas hacia abjao
            {
                curvesPathreiforment.Add(Line.CreateBound(ListaPtoFinal[1], ListaPtoFinal[0]));
            }
            else// para dibujar las patas hacia arriba
            {
                curvesPathreiforment.Add(Line.CreateBound(ListaPtoFinal[3], ListaPtoFinal[2]));

                Curve pathDereSupDesplazado_cmHAciaArriba = DesplazarCurvaDesface(Line.CreateBound(ListaPtoFinal[3], ListaPtoFinal[2]));
                curvesPathreiforment.Add(pathDereSupDesplazado_cmHAciaArriba);// para dibujar F1SUP dere-super  ---se usa en metodo RedefinirCurvesPathreiforment()    

                Curve PathIZqAbajODespalzadoHAciaIzquierda = DesplazarCurvaIzqInf(ListaPtoFinal[0], ListaPtoFinal[1], anguloBarraGrado, largomin);

                Curve pathIzqInfDesplazado_cmHAciaArriba = DesplazarCurvaDesface(PathIZqAbajODespalzadoHAciaIzquierda);
                curvesPathreiforment.Add(pathIzqInfDesplazado_cmHAciaArriba); // para dibujar F1SUP izq-Inf  ---se usa en metodo RedefinirCurvesPathreiforment()
            }

            return curvesPathreiforment;
        }

        //desplza los ptos 1 y 0 haci el interior del path , por caso S1_SUP
        /*
         *    0 -->0'---- 3
         *    1 -->1'---- 2
        */
        public static Curve DesplazarCurvaIzqInf(XYZ p0_, XYZ p1_, double anguloBarraGrado, double largomin)
        {

            XYZ p0 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p0_, Util.GradosToRadianes(anguloBarraGrado), Math.Max(largomin * ConstNH.PORCENTAJE_LARGO_PATA, ConstNH.LARGO_MIN_PATH_S4_FOOT));
            XYZ p1 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p1_, Util.GradosToRadianes(anguloBarraGrado), Math.Max(largomin * ConstNH.PORCENTAJE_LARGO_PATA, ConstNH.LARGO_MIN_PATH_S4_FOOT));

            return Line.CreateBound(p0, p1);
        }

        //desplaza pathreinforment para caso F1_sup para diferiencia f1 de f1SUp - OBS1
        private static Curve DesplazarCurvaDesface(Curve curve)
        {
            XYZ p0_ = curve.GetEndPoint(0); //inicio
            XYZ p1_ = curve.GetEndPoint(1); // fin
            double anglePath = Util.angulo_entre_pt_Rad_XY0(p1_, p0_);
            XYZ p0_desfaseSup = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p0_, anglePath, Util.CmToFoot(ConstNH.CONST_DESPLA_ENTRE_BARRAS_F1_SUP_CM));
            XYZ p1_desfaseSup = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p1_, anglePath, Util.CmToFoot(ConstNH.CONST_DESPLA_ENTRE_BARRAS_F1_SUP_CM));

            return Line.CreateBound(p0_desfaseSup, p1_desfaseSup);
        }
        // s1 , s2 s s3

        /// <summary> usa 'Load_s1_s3'  -->
        /// iterra sobre la lista 'ListaRooms' que contiene los room del view analizadoy busca 
        /// la que contiene al punto pto con la funcion'room.IsPointInRoom'
        /// funcion  para obtener 4 puntos que definen dos lados opuestos del poligono para
        /// posteriormente obtener el area de refuerzo
        /// 
        ///   p1                  p4
        ///    |                  |        
        ///    |   -----barra---- |
        ///    |                  |
        ///    p2                 p3
        /// </summary>
        /// <param name="rooms">lisata de room que estan en cierto nivel</param>
        /// <param name="opt">objeto Options a seleccionar</param>
        /// <param name="pto">pto de seleccion con el mouse sobre la losa </param>
        /// <returns>lista de 4 pto que definen 2 lados del poligono de losa</returns>
        /// 
        //public static List<XYZ> ListaPoligonosRooms_S1_S3V2(XYZ pto1, XYZ pto2, TipoOrientacionBarra orientacion, double Angle_pelotaLosa1_, ref Room m_roomSelecionado_1, ref Room m_roomSelecionado_2, Floor losaSeleccionada)
        public static Tuple<List<XYZ>, double> ListaPoligonosRooms_S1_S3V2(UIApplication _uiapp, ReferenciaRoomDatos refRoomDatos, TipoOrientacionBarra orientacion)

        {
            List<XYZ> ListaInterseccion = new List<XYZ>();

            List<XYZ> ListaInterseccionRoom1 =
                AnalizarRoom_S1_S3_1pto(_uiapp, refRoomDatos.PtoSeleccionMouse1, refRoomDatos.PtoSeleccionMouse2, orientacion, refRoomDatos.anguloBarraLosaGrado_1, refRoomDatos.Room1, refRoomDatos.Losa);
            List<XYZ> ListaInterseccionRoom2 =
                AnalizarRoom_S1_S3_1pto(_uiapp, refRoomDatos.PtoSeleccionMouse2, refRoomDatos.PtoSeleccionMouse1, orientacion, refRoomDatos.anguloBarraLosaGrado_1, refRoomDatos.Room2, refRoomDatos.Losa);

            ListaInterseccion.AddRange(ListaInterseccionRoom1);
            ListaInterseccion.AddRange(ListaInterseccionRoom2);


            double anguloBordeRoomYsegundoPto = ObtenerAnguloSeleccionSegundoPtoMouse(ListaInterseccionRoom2, refRoomDatos.PtoSeleccionMouse2);


            return Tuple.Create(ListaInterseccion, anguloBordeRoomYsegundoPto);
        }

        //el segundo pto del mouse lo proyecta en el borde del room intersectado y obtiene el angulo entre ese pto intersectado y el segundo pto del mouse seleccionado
        public static double ObtenerAnguloSeleccionSegundoPtoMouse(List<XYZ> ListaInterseccionRoom, XYZ PtoSeleccionMouse)
        {

            return Math.Round(Util.AnguloEntre2PtosGrado90(ListaInterseccionRoom[0], ListaInterseccionRoom[1], true), 0);


            //Line ln = Line.CreateBound(ListaInterseccionRoom[0], ListaInterseccionRoom[1]);
            //IntersectionResult InterPtoSobreLinea = ln.Project(PtoSeleccionMouse);
            //XYZ ptoSobreLinea = InterPtoSobreLinea.XYZPoint;
            //return Util.RadianeToGrados(Util.angulo_entre_ptRadXY0(ptoSobreLinea, PtoSeleccionMouse));
            ////   ;
        }

        private static List<XYZ> AnalizarRoom_S1_S3_1pto(UIApplication _uiapp, XYZ pto1, XYZ pto2, TipoOrientacionBarra orientacion, double Angle_pelotaLosa1_, Room m_roomSelecionado_1, Floor losaSeleccionada)
        {
            List<XYZ> ListaInterseccion = new List<XYZ>();

            XYZ direccionSeleccon = Util.CrossProduct(pto1, pto2);

            Floor floorSeleccionada = losaSeleccionada;
            Room room = m_roomSelecionado_1;
            // obtiene puntos del poligono de room
            List<XYZ> boundary_pts = RoomFuncionesPuntos.ListRoomVertice(room);

            Document doc = _uiapp.ActiveUIDocument.Document;
            IList<IList<BoundarySegment>> boundaries = room.GetBoundarySegments(new SpatialElementBoundaryOptions()); // 2012

            //si tienes pts analiza  --------- nota: puede que alla room sin asignar
            if (boundaries.Count > 0)
            {
                double largoBarra = ConstNH.LARGO_LINEA_AUXILIAR_FOOT;// 100f;
                double aux_angle = Angle_pelotaLosa1_;
                if (orientacion == TipoOrientacionBarra.Vertical)
                { aux_angle = aux_angle + 90; }
                //genera linea auxiliar para buscar las intersecciones con el room
                Line LineAuxBuscarInters = null;
                if (pto2 == null)
                {
                    LineAuxBuscarInters = Line.CreateBound(new XYZ(pto1.X - largoBarra * Math.Cos(Util.GradosToRadianes(aux_angle)), pto1.Y - largoBarra * Math.Sin(Util.GradosToRadianes(aux_angle)), pto1.Z),
                                                            new XYZ(pto1.X + largoBarra * Math.Cos(Util.GradosToRadianes(aux_angle)), pto1.Y + largoBarra * Math.Sin(Util.GradosToRadianes(aux_angle)), pto1.Z));
                }
                else
                {
                    LineAuxBuscarInters = Line.CreateBound(pto1, pto2.AsignarZ(pto1.Z));
                }

                BoundarySegmentRoomsGeom newlistaBS = new BoundarySegmentRoomsGeom(doc);
                foreach (IList<BoundarySegment> b in boundaries) // 2012
                {

                    BoundarySegmentRoomsGeom newlistaBS_aux = new BoundarySegmentRoomsGeom(b, _uiapp, room.Name, floorSeleccionada);
                    //reorden los lados del poligonos para juntos los similes
                    newlistaBS_aux.M1_Reordendar();
                    //agrega ptos del room de mayor area (considero que el primer elemto es el mas grande)
                    boundary_pts = newlistaBS_aux.ListaVerticesRoomActualizados;

                    newlistaBS.ListaWrapperBoundarySegment.AddRange(newlistaBS_aux.ListaWrapperBoundarySegment);

                }

                double distMax = 1000;
                foreach (WrapperBoundarySegment s in newlistaBS.ListaWrapperBoundarySegment)
                {
                    XYZ startPont = s.coordenadasBorde.StartPoint.AsignarZ(pto1.Z);
                    XYZ EndPoint = s.coordenadasBorde.EndPoint.AsignarZ(pto1.Z);
                    if (startPont.DistanceTo(EndPoint) < Util.MinLineLength * 2) continue;
                    Line LineAuxRoom = Line.CreateBound(startPont, EndPoint);
                    IntersectionResultArray resultArray_aux;
                    SetComparisonResult result = LineAuxBuscarInters.Intersect(LineAuxRoom, out resultArray_aux);


                    if (result != SetComparisonResult.Overlap) continue;

                    IntersectionResult iResult = resultArray_aux.get_Item(0);
                    double distInterseccion = iResult.XYZPoint.DistanceTo(pto1);

                    if (distInterseccion > distMax) continue;
                    distMax = distInterseccion;
                    ListaInterseccion.Clear();
                    ListaInterseccion.Add(startPont);
                    ListaInterseccion.Add(EndPoint);

                }
                //for (int i = 0; i <= boundary_pts.Count - 2; i++)
                //{
                //    XYZ startPont = boundary_pts[i].AsignarZ(pto1.Z);
                //    XYZ EndPoint = boundary_pts[i + 1].AsignarZ(pto1.Z);
                //    Line LineAuxRoom = Line.CreateBound(startPont, EndPoint);
                //    IntersectionResultArray resultArray_aux;
                //    SetComparisonResult result = LineAuxBuscarInters.Intersect(LineAuxRoom, out resultArray_aux);

                //    if (result != SetComparisonResult.Overlap) continue;

                //    ListaInterseccion.Add(startPont);
                //    ListaInterseccion.Add(EndPoint);
                //}
            }



            return ListaInterseccion;
        }




        /// <summary>
        ///  usa ' Load_fx()'  -->
        ///  
        /// iterra sobre la lista 'ListaRooms' que contiene los room del view analizadoy busca 
        /// la que contiene al punto pto con la funcion'room.IsPointInRoom'
        /// funcion  para obtener 4 puntos que definen dos lados opuestos del poligono para
        /// posteriormente obtener el area de refuerzo
        /// 
        /// </summary>
        /// <param name="Listarooms"></param>
        /// <param name="opt"></param>
        /// <param name="pto1"> punto interno de room para buscar a los 0 grados y despues a los 90 grados las intersacciones con el ´poligono de losa</param>
        /// <param name="pto2">  actualmeante es el valor null</param>
        /// <param name="orientacion"> que corresponde a giro  de la pelota de losa</param>
        /// <param name="Angle_1"></param>
        /// <param name="m_roomSelecionado_1"></param>
        /// <param name="m_roomSelecionado_2"></param>
        /// <param name="LargoMin_1"></param>
        /// <param name="LargoMin_2"></param>
        /// <returns></returns>
        public static List<XYZ> ListaPoligonosRooms_fx(UIApplication _uiapp, ReferenciaRoomDatos refereciaRoomDatos, Options opt, TipoOrientacionBarra orientacion)
        {
            //Limpia los valores de las listas
            clearValores();

            List<XYZ> ListaInterseccion = new List<XYZ>();
            listBorde = new List<WrapperBoundarySegment>();
            /*
             
            p1                      p3
             | _______barra________  |
             |                       |   
             p2                      p4   

             ListaInterseccion = { p1,p2,p3,p4}
             */


            espesor_izq_bajo = 0;
            espesor_dere_sup = 0;
            Room room = refereciaRoomDatos.Room1;
            Floor floorSeleccionada = refereciaRoomDatos.Losa;
            //double Angle_pelotaLosa1_ =Util.GradosToRadianes( refereciaRoomDatos.anguloPelotaLosaGrado);
            double Angle_pelotaLosa1_ = refereciaRoomDatos.anguloPelotaLosaGrado_1;
            XYZ PtoSeleccionMouse = refereciaRoomDatos.PtoSeleccionMouse1;
            // obtiene puntos del poligono de room
            List<XYZ> boundary_pts = RoomFuncionesPuntos.ListRoomVertice(room);

            IList<IList<BoundarySegment>> boundaries = room.GetBoundarySegments(new SpatialElementBoundaryOptions()); // 2012

            Document _doc = _uiapp.ActiveUIDocument.Document;

            // diccionario para obtener los ptos interseccion
            //       startPont
            //          |
            //          |
            //          |    
            //     iResult.XYZPoint   ---------------  linea aux   ptomouse --------------------------------
            //          |
            //          |
            //          |
            //       EndPoint
            //    ListaPtos.Add(  ptomouse  ,  List(startPont, EndPoint ))
            Dictionary<XYZ, List<XYZ>> ListaPtos = new Dictionary<XYZ, List<XYZ>>();


            int n = 0;

            if (null != boundaries)
            {
                //n = boundaries.Size; // 2011
                n = boundaries.Count; // 2012
            }

            double AnguloDireccionBarra = Angle_pelotaLosa1_;
            if (orientacion == TipoOrientacionBarra.Vertical)
            { AnguloDireccionBarra = AnguloDireccionBarra + 90; }

            if (0 < n)
            {
                double largoBarra = ConstNH.LARGO_LINEA_AUXILIAR_FOOT;


                //genera linea auxiliar para buscar las intersecciones con el room
                Line LineAuxBuscarInters_direccionBarra = null;
                // Line LineAuxBuscarInters_direccionPerpenBarra = null;

                double ValorZ = PtoSeleccionMouse.Z;
                //linea en la direccion de losa
                LineAuxBuscarInters_direccionBarra = Line.CreateBound(new XYZ(PtoSeleccionMouse.X - largoBarra * Math.Cos(Util.GradosToRadianes(AnguloDireccionBarra)), PtoSeleccionMouse.Y - largoBarra * Math.Sin(Util.GradosToRadianes(AnguloDireccionBarra)), ValorZ),
                                                        new XYZ(PtoSeleccionMouse.X + largoBarra * Math.Cos(Util.GradosToRadianes(AnguloDireccionBarra)), PtoSeleccionMouse.Y + largoBarra * Math.Sin(Util.GradosToRadianes(AnguloDireccionBarra)), ValorZ));
                //linea perpendicular a la direccion de losa
                // LineAuxBuscarInters_direccionPerpenBarra = Line.CreateBound(new XYZ(PtoSeleccionMouse.X - largoBarra * Math.Cos(Util.GradosToRadianes(AnguloDireccionBarra) + Math.PI / 2), PtoSeleccionMouse.Y - largoBarra * Math.Sin(Util.GradosToRadianes(AnguloDireccionBarra) + Math.PI / 2), ValorZ),
                //                                         new XYZ(PtoSeleccionMouse.X + largoBarra * Math.Cos(Util.GradosToRadianes(AnguloDireccionBarra) + Math.PI / 2), PtoSeleccionMouse.Y + largoBarra * Math.Sin(Util.GradosToRadianes(AnguloDireccionBarra) + Math.PI / 2), ValorZ));

                // int iBoundary = 0, iSegment;
                //foreach( BoundarySegmentArray b in boundaries ) // 2011
                BoundarySegmentRoomsGeom newlistaBS = new BoundarySegmentRoomsGeom(_doc);
                // contiene una lista que tiene la lista de los boundery de los room, puede tener mas de una lista, por presencia de shaft, openin o muro dentro del room


                //selcciona la losa que contiene el room

                //         ConstantesGenerales.sbLog.Clear();
                foreach (IList<BoundarySegment> b in boundaries) // 2012
                {

                    // analiza una lista que contienen los bordes de un boundary
                    BoundarySegmentRoomsGeom newlistaBS_aux = new BoundarySegmentRoomsGeom(b, _uiapp, room.Name, floorSeleccionada);

                    //reorden los lados del poligonos para juntos los similes
                    newlistaBS_aux.M1_Reordendar();

                    newlistaBS.ListaWrapperBoundarySegment.AddRange(newlistaBS_aux.ListaWrapperBoundarySegment);
                }
                //ConstantesGenerales.sbLog.AppendLine("int IdLosa = " + LosaSeleccionada1.Id.ToString() + ";");
                //LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);

                //++iBoundary;
                //iSegment = 0;
                int iSegment = 0;
                foreach (WrapperBoundarySegment s in newlistaBS.ListaWrapperBoundarySegment)
                {
                    //++iSegment;
                    iSegment += 1;
                    //Element neighbour = s.Element; // 2015
                    //Element neighbour = doc.GetElement(s.ElementId); // 2016

                    //Curve curve = s.Curve; // 2015
                    //double length = s.le
                    XYZ p_DESFACE = s.coordenadasBorde.desface;

                    if (IsLosaINclinada && s.obtenerRefereciasCercanas.elementoContiguo == ElementoContiguo.RoomContacto) p_DESFACE = XYZ.Zero;


                    //generando una  linea auxiliar que representa una segmento del poligono de losa
                    XYZ startPont = s.coordenadasBorde.StartPoint.AsignarZ(PtoSeleccionMouse.Z);
                    XYZ EndPoint = s.coordenadasBorde.EndPoint.AsignarZ(PtoSeleccionMouse.Z);
                    Debug.WriteLine($" segmento_{iSegment}  startPont:{startPont.REdondearString_foot(2)}    EndPoint:{EndPoint.REdondearString_foot(2)}");
                    double anguloSegmentoRoom = s.anguloGrado;
                    if (startPont.DistanceTo(EndPoint) < Util.MinLineLength * 2) continue;
                    Line LineAuxRoomBordeRoom = Line.CreateBound(startPont, EndPoint);
                    //Line LineAuxRoomBordeRoom = Line.CreateBound(startPont.AsignarZ(0), EndPoint.AsignarZ(0));

                    //a)linea en la direccion de la losa                             
                    IntersectionResultArray results_direccionBarra_2;
                    SetComparisonResult result_direccionBarra = LineAuxBuscarInters_direccionBarra.Intersect(LineAuxRoomBordeRoom, out results_direccionBarra_2);
                    if (result_direccionBarra == SetComparisonResult.Overlap)
                    {



                        IntersectionResult iResult = results_direccionBarra_2.get_Item(0);
                        s.coordenadasBorde.pointIntersccion = iResult.XYZPoint;
                        listBorde.Add(s);
                        // iResult.XYZPoint                                                      
                        if (ListaInterseccion.Count == 0)// si tiene 0 ptos
                        {
                            ListaInterseccion.Add(startPont + p_DESFACE);
                            ListaInterseccion.Add(EndPoint + p_DESFACE);

                            ListaPtos.Add(iResult.XYZPoint, new List<XYZ>() { startPont + p_DESFACE, EndPoint + p_DESFACE });
                        }
                        else  //si tiene 2 ptos
                        {
                            // if para ingresar los datos de forma que queden   -->  ListaInterseccion={p1,p2,p3,p4}
                            //  p1  --- p4
                            //  p2  --- p3   
                            if (Util.IsIntersection(ListaInterseccion[0], startPont, ListaInterseccion[1], EndPoint))
                            {
                                ListaInterseccion.Add(startPont + p_DESFACE);
                                ListaInterseccion.Add(EndPoint + p_DESFACE);
                                ListaPtos.Add(iResult.XYZPoint, new List<XYZ>() { startPont + p_DESFACE, EndPoint + p_DESFACE });
                            }
                            else
                            {
                                ListaInterseccion.Add(EndPoint + p_DESFACE);
                                ListaInterseccion.Add(startPont + p_DESFACE);
                                ListaPtos.Add(iResult.XYZPoint, new List<XYZ>() { EndPoint + p_DESFACE, startPont + p_DESFACE });
                            }

                        }
                        // agrega pt de intesccion entre lines
                        ListaInterDireLosa.Add(iResult.XYZPoint);

                    }

                }
            }

            if (ListaPtos.Count == 0) return ListaInterseccion;

            if (!Get2BordesOrdenados(PtoSeleccionMouse, AnguloDireccionBarra))
            {
                ListaInterseccion.Clear();
                return ListaInterseccion;
            }
            //por si se encuentras mas de 2 intersecciones con borde de room
            //  if (ListaPtos.Count > 2)
            ListaInterseccion = GetListaInterseccion(ListaPtos, PtoSeleccionMouse, AnguloDireccionBarra);
            //si se encontraron 4 puntos de interseccion salenhsnhs
            if (ListaInterseccion.Count == 4) return ListaInterseccion;



            return ListaInterseccion;
        }



        /// <summary>
        /// metodo para obtener los puntos del largo de la barra cuando hay mas de 2 intersercciones 
        /// </summary>
        /// <param name="listaInterseccion"></param>
        /// <returns></returns>
        private static List<XYZ> GetListaInterseccion(Dictionary<XYZ, List<XYZ>> listaInterseccion, XYZ ptoMouse, double Angle_1)
        {


            //       startPont
            //          |
            //          |
            //          |    
            //     iResult.XYZPoint   ---------------  linea_aux_ptomouse --------------------------------
            //          |
            //          |
            //          |
            //       EndPoint

            List<XYZ> ListaInterseccion_negativo_aux = new List<XYZ>();
            List<XYZ> ListaInterseccion_positivo_aux = new List<XYZ>();
            List<XYZ> ListaInterseccion_aux = new List<XYZ>();

            XYZ p1 = null;
            XYZ p2 = null;
            XYZ p3 = null;
            XYZ p4 = null;

            double dista_aux_negativo = 100000;
            double dista_aux_positivo = 100000;

            try
            {


                foreach (KeyValuePair<XYZ, List<XYZ>> item in listaInterseccion)
                {


                    Transform trans1 = Transform.CreateTranslation(-ptoMouse);
                    Transform trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(Angle_1), XYZ.Zero);

                    //pto interseccion con borde de room
                    XYZ pp_Ayuda = trans2_rotacion.OfPoint(trans1.OfPoint(item.Key));

                    double aux_distacia = XYZ.Zero.DistanceTo(pp_Ayuda);

                    //si pto es anterior alpto mouse y distancia entre ellos es menor al de referencia (dista_aux_negativo)
                    if (pp_Ayuda.X < 0 && aux_distacia < dista_aux_negativo)
                    {                    //remplaza el valor auxiliar de referecnia
                        dista_aux_negativo = aux_distacia;
                        //limpia las lista aux 
                        ListaInterseccion_negativo_aux.Clear();
                        // copia los ptos   startPont  ,   EndPoint
                        ListaInterseccion_negativo_aux = item.Value;

                    }//si pto es posterior al pto mouse y distancia entre ellos es menor al de referencia (dista_aux_positivo)
                    else if (pp_Ayuda.X > 0 && aux_distacia < dista_aux_positivo)  ///  p1.X > 0
                    {
                        //remplaza el valor auxiliar de referecnia
                        dista_aux_positivo = aux_distacia;
                        //limpia las lista aux 
                        ListaInterseccion_positivo_aux.Clear();
                        // copia los ptos   startPont  ,   EndPoint
                        ListaInterseccion_positivo_aux = item.Value;
                    }


                }

                if (ListaInterseccion_negativo_aux.Count() != 2 || ListaInterseccion_positivo_aux.Count() != 2)
                {
                    ListaInterseccion_aux.Clear();
                    Debug.WriteLine($"errrr al obtener ptos");
                    return ListaInterseccion_aux;
                }

                p1 = ListaInterseccion_negativo_aux[0];
                p2 = ListaInterseccion_negativo_aux[1];
                p3 = ListaInterseccion_positivo_aux[0];
                p4 = ListaInterseccion_positivo_aux[1];

                ListaInterseccion_aux.Add(p1);
                ListaInterseccion_aux.Add(p2);
                ListaInterseccion_aux.Add(p3);
                ListaInterseccion_aux.Add(p4);



            }
            catch (Exception ex)
            {
                ListaInterseccion_aux.Clear();
                Debug.WriteLine($"ex:{ex.Message}");
            }



            return ListaInterseccion_aux;
        }


        /// <summary>
        /// o resposabilidad: ordenar la lista 'listBorde' de modo que solo queden dos ordenados
        ///
        /// </summary>
        /// <remarks> deben quedar ordenas inicial nenor 'x' o si es vertical 'y' mas bajo</remarks>
        /// <remarks> puede que existan mas datos en liista por representan intersecciones 
        /// del pto con la room</remarks>
        /// <param name="listaInterseccion"></param>
        /// <param name="ptoMouse"></param>
        /// <param name="Angle_1"></param>
        /// <returns></returns>
        private static bool Get2BordesOrdenados(XYZ ptoMouse, double Angle_1)
        {
            //       startPont
            //          |
            //          |
            //          |    
            //     iResult.XYZPoint   ---------------  linea_aux_ptomouse --------------------------------
            //          |
            //          |
            //          |
            //       EndPoint

            Isok = true;
            WrapperBoundarySegment bordeNeg = null;
            WrapperBoundarySegment bordePos = null;

            List<XYZ> ListaInterseccion_negativo_aux = new List<XYZ>();
            List<XYZ> ListaInterseccion_positivo_aux = new List<XYZ>();
            List<XYZ> ListaInterseccion_aux = new List<XYZ>();

            double dista_aux_negativo = 100000;
            double dista_aux_positivo = 100000;

            try
            {


                foreach (WrapperBoundarySegment item in listBorde)
                {


                    Transform trans1 = Transform.CreateTranslation(-ptoMouse);
                    Transform trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(Angle_1), XYZ.Zero);

                    //pto interseccion con borde de room
                    XYZ p1 = trans2_rotacion.OfPoint(trans1.OfPoint(item.coordenadasBorde.pointIntersccion));

                    double aux_distacia = XYZ.Zero.DistanceTo(p1);

                    //si pto es anterior alpto mouse y distancia entre ellos es menor al de referencia (dista_aux_negativo)
                    if (p1.X < 0 && aux_distacia < dista_aux_negativo)
                    {                    //remplaza el valor auxiliar de referecnia
                        dista_aux_negativo = aux_distacia;
                        bordeNeg = item;

                    }//si pto es posterior al pto mouse y distancia entre ellos es menor al de referencia (dista_aux_positivo)
                    else if (p1.X > 0 && aux_distacia < dista_aux_positivo)  ///  p1.X > 0
                    {
                        //remplaza el valor auxiliar de referecnia
                        dista_aux_positivo = aux_distacia;
                        bordePos = item;
                    }


                }


                listBorde.Clear();
                if (bordeNeg == null || bordePos == null)
                {
                    Debug.WriteLine($"no se encuentran ptos finales");
                    return false;
                }
                listBorde.Add(bordeNeg);// izq- infe
                listBorde.Add(bordePos); // dere - sup
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                Isok = false;
                return false;
            }
            return true;
        }


        #endregion
    }
}
