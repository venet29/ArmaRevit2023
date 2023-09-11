using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;

using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using System.Diagnostics;
using MediaNH = System.Windows.Media;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Fauto;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.RebarLosa.Geom
{
    public class BarraRoomGeometriaFX
    {
        // propeidades
        public  int espesor_izq_bajo { get; set; }
        public  int espesor_dere_sup { get; set; }

        public  List<XYZ> ListaInterDireLosa { get; set; }
        public  List<XYZ> ListaInterDirePerpLosa { get; set; }

        public  double LargoDireLosa { get; set; }
        public  double LargoDirePerpLosa { get; set; }

        public  string DireLosaHor { get; set; }
        public  string DireLosaVert { get; set; }

        public  XYZ ptoSelect { get; set; }
        public  XYZ ptoSelect_2 { get; set; }
        public  XYZ ptoSelect_tras { get; set; }

        public  double LargoRecorridoX { get; set; }
        public  double LargoRecorridoY { get; set; }

        public  List<WrapperBoundarySegment> listBorde { get; set; }
        public  AnalisisFauto _analisisFauto { get; private set; }
        public  string tipoBarra { get; set; }
        public  string tipoBarra_izq_infer { get; set; }
        public  string tipoBarra_dere_sup { get; set; }
        public  UbicacionLosa ubicacionBArraEnlosa { get; set; }
        public  bool IsBuscarTipoBarra { get; internal set; }

        //*****************************
        #region 1)COntructor


        public BarraRoomGeometriaFX()
        {



        }
        #endregion

        #region 2.3) metodos para obtener geome de pologono de losas y barra

        private void clearValores()
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
        public  IList<Curve> CrearCurva(List<XYZ> lista)
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
        public  Tuple<List<XYZ>, IList<Curve>> ListaFinal_ptov2(List<XYZ> listaPtos, Application app,
                                                                      SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom, SolicitudBarraDTO _solicitudBarraDTO,
                                                                      ReferenciaRoomDatos roomAnalizado, bool IsTest)
        {
            List<XYZ> _listaPtoFinal = new List<XYZ>();
            IList<Curve> _curvesPathreiforment = new List<Curve>();

            Element floor_1 = _seleccionarLosaBarraRoom.LosaSeleccionada1 as Floor;
            string TipoBarra = _solicitudBarraDTO.TipoBarra;
            TipoConfiguracionBarra tipobarra = _solicitudBarraDTO.TipoConfiguracionBarra;
            double anguloBarra_ = roomAnalizado.anguloBarraLosaGrado_1;
            double LargoMin_maximo1_soloParaSuple = roomAnalizado.largomin_1;
            double LargoMin_maximo2_soloParaSuple = roomAnalizado.largomin_2;


            Options gOptions = new Options();
            gOptions.ComputeReferences = true;
            gOptions.DetailLevel = ViewDetailLevel.Undefined;
            gOptions.IncludeNonVisibleObjects = false;
            GeometryElement geo = floor_1.get_Geometry(gOptions);

            foreach (GeometryObject obj in geo) // 2013
            {


                Transform trans1 = null;
                Transform Invertrans1 = null;
                Transform trans2_rotacion = null;
                Transform InverTrans2_rotacion = null;
                //var tarsas=getInstGeo.GetTransformed();

                Solid floor_ = obj as Solid;
                if (floor_ != null)
                {
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
                         if (Util.PointsUpwards(normal))
                            {
                            XYZ ptXAxis = XYZ.BasisX;
                            XYZ ptYAxis = XYZ.BasisY;

                            _curvesPathreiforment.Clear();


                            //obtener ptos proyectados en cara superior de la losa
                            for (int i = 0; i < listaPtos.Count; i++)
                            {
                                //IntersectionResult resul = f.Project(listaPtos[i]+ ConstNH.CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT );
                                //if (resul != null) listaPtos[i] = resul.XYZPoint;
                                XYZ resul = f.ProjectNH(listaPtos[i] + ConstNH.CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT);
                                if (resul.IsDistintoLargoCero()) listaPtos[i] = resul;
                            }
                            // Plane plane1 = Plane.(XYZ.Zero, ptYAxis ,ptXAxis);
                            //trans1 = Transform.CreateTranslation(-minxyz);
                            double valorx = listaPtos.Min(val => val.X);
                            double valory = listaPtos.Min(val => val.Y);
                            //trans1 = Transform.CreateTranslation(new XYZ(-valorx, -valory, -minxyz.Z));
                            trans1 = Transform.CreateTranslation(-listaPtos[1]);

                            if (TipoBarra.ToLower() == "s1" || TipoBarra.ToLower() == "s3" || TipoBarra.ToLower() == "s2" || (TipoBarra.ToLower() == "s2" && tipobarra == TipoConfiguracionBarra.suple))  //                                                                 Angle_1/ Angle_1 => solo para amntener el signo
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

                            // ConstantesGenerales.sbLog = new StringBuilder();

                          //  if (!IsTest) GraficarPtos(listaPtos[0], listaPtos[1], listaPtos[2], listaPtos[3]);

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
                            if (TipoBarra.ToLower() == "s1" || TipoBarra.ToLower() == "s3" || TipoBarra.ToLower() == "s2" || (TipoBarra.ToLower() == "s2" && tipobarra == TipoConfiguracionBarra.suple))
                            { }
                            else if (TipoBarra.ToLower() == "f1_SUP" && !IsBuscarTipoBarra)
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

                            _listaPtoFinal = PtosParaBarraInferiorv2(Invertrans1, InverTrans2_rotacion, p1, p2, p3, p4, _solicitudBarraDTO.TipoOrientacion);
                            _curvesPathreiforment = PathBarraInferior(_listaPtoFinal, TipoBarra, anguloBarra_, roomAnalizado.largomin_1);

                            break;
                        }
                    }
                }
            }

            return Tuple.Create(_listaPtoFinal, _curvesPathreiforment);
        }


        //private  void GraficarPtos(XYZ p1, XYZ p2, XYZ p3, XYZ p4)
        //{
        //    if (ConstNH.MODO_DISEÑO == false) return;

        //    MediaNH.PointCollection ptoTrasladados = new MediaNH.PointCollection();
        //    ptoTrasladados.Add(new System.Windows.Point(p1.X, p1.Y));
        //    ptoTrasladados.Add(new System.Windows.Point(p2.X, p2.Y));
        //    ptoTrasladados.Add(new System.Windows.Point(p3.X, p3.Y));
        //    ptoTrasladados.Add(new System.Windows.Point(p4.X, p4.Y));
        //    GraficarPtos DibujoPtosTrasladados = new GraficarPtos(ptoTrasladados);
        //    DibujoPtosTrasladados.ShowDialog();
        //}

    
       
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
        private Result OrdenarPtosTransform(ref XYZ p1, ref XYZ p2, ref XYZ p3, ref XYZ p4)
        {
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
     
        
    



        public  List<XYZ> PtosParaBarraInferiorv2(Transform Invertrans1, Transform InverTrans2_rotacion, XYZ p1, XYZ p2, XYZ p3, XYZ p4,
                                            TipoOrientacionBarra TipoOrientacion)
        {

            List<XYZ> ListaPtoFinal = new List<XYZ>();

            /// ordena ptos de que estan trasnformadosde modo que tengas esta configuracion
            /// p1 -- p4
            ///  |     |
            /// p2 -- p3
            /// ordenan todos de modo que p1 y p2 queden ala izq y p3,p4 a la derecha


            XYZ pf1 = new XYZ();
            XYZ pf2 = new XYZ();
            XYZ pf3 = new XYZ();
            XYZ pf4 = new XYZ();

            double LargoRecorrido_axu = 0;

            //NOta LargoRecorridoY , LargoRecorridoX viene del diseño automatico,
            //para diseño manual con mouse LargoRecorridoY=0 , LargoRecorridoX=0

            if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                LargoRecorrido_axu = LargoRecorridoY;
            else
                LargoRecorrido_axu = LargoRecorridoX;


            if (p4.Y > p1.Y)
            {
                if (LargoRecorridoY != 0)// && ptoSelect_tras.Y + LargoRecorrido_axu / 2 < p1.Y)
                    //pf1 = new XYZ(p1.X, ptoSelect_tras.Y + LargoRecorrido_axu / 2, 0);
                    pf1 = new XYZ(p1.X, ptoSelect_tras.Y + LargoRecorrido_axu / 2, 0);
                else
                    pf1 = p1;

                pf4 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, pf1.Y, 0), new XYZ(1000, pf1.Y, 0)));
            }
            else if (p4.Y <= p1.Y)
            {
                if (LargoRecorridoY != 0)// && ptoSelect_tras.Y + LargoRecorrido_axu / 2 < p4.Y)
                    pf4 = new XYZ(p4.X, ptoSelect_tras.Y + LargoRecorrido_axu / 2, 0);
                else
                    pf4 = p4;
                pf1 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, pf4.Y, 0), new XYZ(1000, pf4.Y, 0)));
            }

            //analizar inferiro
            if (p2.Y > p3.Y)
            {

                if (LargoRecorridoY != 0)// && ptoSelect_tras.Y - LargoRecorrido_axu / 2 > p2.Y)
                    pf2 = new XYZ(p2.X, ptoSelect_tras.Y - LargoRecorrido_axu / 2, 0);
                else
                    pf2 = p2;


                pf3 = Util.Intersection(Line.CreateBound(p3, p4), Line.CreateBound(new XYZ(-1000, pf2.Y, 0), new XYZ(1000, pf2.Y, 0)));
            }
            else if (p2.Y <= p3.Y)
            {

                if (LargoRecorridoY != 0)// && ptoSelect_tras.Y - LargoRecorrido_axu / 2 > p3.Y)
                    pf3 = new XYZ(p3.X, ptoSelect_tras.Y - LargoRecorrido_axu / 2, 0);
                else
                    pf3 = p3;
                pf2 = Util.Intersection(Line.CreateBound(p1, p2), Line.CreateBound(new XYZ(-1000, pf3.Y, 0), new XYZ(1000, pf3.Y, 0)));
            }
            //pf0_ = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(new XYZ(pf1.X + largoMin * 0.15, pf1.Y, pf1.Z)));
            //pf1_ = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(new XYZ(pf2.X + largoMin * .15, pf2.Y, pf2.Z)));
            //pf2_ = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(new XYZ(pf3.X - largoMin * 0.15, pf3.Y, pf3.Z)));
            //pf3_ = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(new XYZ(pf4.X - largoMin * 0.15, pf4.Y, pf4.Z)));
            ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf1)));
            ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf2)));
            ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf3)));
            ListaPtoFinal.Add(Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf4)));

            return ListaPtoFinal;

        }

       
        public  IList<Curve> PathBarraInferior(List<XYZ> ListaPtoFinal, string TipoBarra, double anguloBarraGrado, double largomin)
        {

            /// ordena ptos de que estan trasnformadosde modo que tengas esta configuracion
            /// p1 -- p4
            ///  |     |
            /// p2 -- p3
            /// ordenan todos de modo que p1 y p2 queden ala izq y p3,p4 a la derecha


            IList<Curve> curvesPathreiforment = new List<Curve>();


            if ((TipoBarra == "f9_") || (TipoBarra == "f9b_") || (TipoBarra == "f9a_"))// para dibujar las patas hacia abjao
            {
                curvesPathreiforment.Add(Line.CreateBound(ListaPtoFinal[1], ListaPtoFinal[0]));
            }
            else// para dibujar las patas hacia arriba
            {
                curvesPathreiforment.Add(Line.CreateBound(ListaPtoFinal[3], ListaPtoFinal[2]));

                Curve pathDereSupDesplazado_cmHAciaArriba = DesplazarCurvaDesface(Line.CreateBound(ListaPtoFinal[3], ListaPtoFinal[2]));
                curvesPathreiforment.Add(pathDereSupDesplazado_cmHAciaArriba);// para dibujar F1SUP dere-super  ---se usa en metodo RedefinirCurvesPathreiforment()    

                Curve PathIZqAbajODespalzadoHAciaIzquierda = BarraRoomGeometria.DesplazarCurvaIzqInf(ListaPtoFinal[0], ListaPtoFinal[1], anguloBarraGrado, largomin);

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
        private  Curve DesplazarCurvaIzqInf(XYZ p0_, XYZ p1_, double anguloBarraGrado, double largomin)
        {

            XYZ p0 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p0_, Util.GradosToRadianes(anguloBarraGrado), largomin * ConstNH.PORCENTAJE_LARGO_PATA);
            XYZ p1 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p1_, Util.GradosToRadianes(anguloBarraGrado), largomin * ConstNH.PORCENTAJE_LARGO_PATA);

            return Line.CreateBound(p0, p1);
        }

        //desplaza pathreinforment para caso F1_sup para diferiencia f1 de f1SUp - OBS1
        private  Curve DesplazarCurvaDesface(Curve curve)
        {
            XYZ p0_ = curve.GetEndPoint(0); //inicio
            XYZ p1_ = curve.GetEndPoint(1); // fin
            double anglePath = Util.angulo_entre_pt_Rad_XY0(p1_, p0_);
            XYZ p0_desfaseSup = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p0_, anglePath, Util.CmToFoot(ConstNH.CONST_DESPLA_ENTRE_BARRAS_F1_SUP_CM));
            XYZ p1_desfaseSup = Util.ExtenderPuntoRespectoOtroPtosConAngulo(p1_, anglePath, Util.CmToFoot(ConstNH.CONST_DESPLA_ENTRE_BARRAS_F1_SUP_CM));

            return Line.CreateBound(p0_desfaseSup, p1_desfaseSup);
        }
        // s1 , s2 s s3

      

        //el segundo pto del mouse lo proyecta en el borde del room intersectado y obtiene el angulo entre ese pto intersectado y el segundo pto del mouse seleccionado
        public  double ObtenerAnguloSeleccionSegundoPtoMouse(List<XYZ> ListaInterseccionRoom, XYZ PtoSeleccionMouse)
        {
            Line ln = Line.CreateBound(ListaInterseccionRoom[0], ListaInterseccionRoom[1]);
            IntersectionResult InterPtoSobreLinea = ln.Project(PtoSeleccionMouse);
            XYZ ptoSobreLinea = InterPtoSobreLinea.XYZPoint;
            return Util.RadianeToGrados(Util.angulo_entre_pt_Rad_XY0(ptoSobreLinea, PtoSeleccionMouse));
            //   ;
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
        public  List<XYZ> ListaPoligonosRooms_MASROOM_fx(UIApplication _uiapp,IEnumerable<Element> Listarooms, Options opt, XYZ pto1, TipoOrientacionBarra orientacion,
                                                   double Angle_pelotaLosa1_, ref Room m_roomSelecionado_1,
                                                   ref double LargoMin_1, ref double LargoMin_2)
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
            foreach (Room room in Listarooms)
            {
                Document doc = room.Document;
                // comprobra si punto esta al interior del volumen de un room
                if (room.IsPointInRoom(new XYZ(pto1.X, pto1.Y, pto1.Z + 1)))
                {
                    m_roomSelecionado_1 = room;
                    LargoMin_1 = 100;
                }

                else
                { continue; }

              //  if (m_roomSelecionado_1 != null) ConstantesGenerales.sbLog.AppendLine("int idRoom =" + m_roomSelecionado_1.Id.ToString() + ";");
               // if (orientacion != null) ConstantesGenerales.sbLog.AppendLine("Orientacion  :" + orientacion.ToString());

                // obtiene puntos del poligono de room
                List<XYZ> boundary_pts = RoomFuncionesPuntos.ListRoomVertice(room);
                ParameterSet pars = m_roomSelecionado_1.Parameters;
                //2
                double aux_angle_ = 0.0;
                double.TryParse(ParameterUtil.FindValueParaByName(pars, "Angulo", room.Document), out aux_angle_);
                Angle_pelotaLosa1_ = Util.RadianeToGrados(aux_angle_);



                IList<IList<BoundarySegment>> boundaries = room.GetBoundarySegments(new SpatialElementBoundaryOptions()); // 2012


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

                double aux_angle = Angle_pelotaLosa1_;
                if (orientacion == TipoOrientacionBarra.Vertical)
                { aux_angle = aux_angle + 90; }

                if (0 < n)
                {
                    double largoBarra = ConstNH.LARGO_LINEA_AUXILIAR_FOOT;


                    //genera linea auxiliar para buscar las intersecciones con el room
                    Line LineAuxBuscarInters_direccionBarra = null;
                    Line LineAuxBuscarInters_direccionPerpenBarra = null;

                    //linea en la direccion de losa
                    LineAuxBuscarInters_direccionBarra = Line.CreateBound(new XYZ(pto1.X - largoBarra * Math.Cos(Util.GradosToRadianes(aux_angle)), pto1.Y - largoBarra * Math.Sin(Util.GradosToRadianes(aux_angle)), pto1.Z),
                                                            new XYZ(pto1.X + largoBarra * Math.Cos(Util.GradosToRadianes(aux_angle)), pto1.Y + largoBarra * Math.Sin(Util.GradosToRadianes(aux_angle)), pto1.Z));
                    //linea perpendicular a la direccion de losa
                    LineAuxBuscarInters_direccionPerpenBarra = Line.CreateBound(new XYZ(pto1.X - largoBarra * Math.Cos(Util.GradosToRadianes(aux_angle) + Math.PI / 2), pto1.Y - largoBarra * Math.Sin(Util.GradosToRadianes(aux_angle) + Math.PI / 2), pto1.Z),
                                                            new XYZ(pto1.X + largoBarra * Math.Cos(Util.GradosToRadianes(aux_angle) + Math.PI / 2), pto1.Y + largoBarra * Math.Sin(Util.GradosToRadianes(aux_angle) + Math.PI / 2), pto1.Z));




                    // int iBoundary = 0, iSegment;

                    //foreach( BoundarySegmentArray b in boundaries ) // 2011
                    BoundarySegmentRoomsGeom newlistaBS = new BoundarySegmentRoomsGeom(doc);
                    // contiene una lista que tiene la lista de los boundery de los room, puede tener mas de una lista, por presencia de shaft, openin o muro dentro del room


                    //selcciona la losa que contiene el room
                    Floor floorSeleccionada = RoomFuncionesSeleccionarLosa.ObtenerFloor_ConUNRoom(room);
                    if (floorSeleccionada == null) continue;

                    foreach (IList<BoundarySegment> b in boundaries) // 2012
                    {
                        // analiza una lista que contienen los bordes de un boundary
                        BoundarySegmentRoomsGeom newlistaBS_aux = new BoundarySegmentRoomsGeom(b, _uiapp, room.Name, floorSeleccionada);

                        //reorden los lados del poligonos para juntos los similes
                        newlistaBS_aux.M1_Reordendar();

                        newlistaBS.ListaWrapperBoundarySegment.AddRange(newlistaBS_aux.ListaWrapperBoundarySegment);
                    }
                   

                    //++iBoundary;
                    //iSegment = 0;
                    foreach (WrapperBoundarySegment s in newlistaBS.ListaWrapperBoundarySegment)
                    {
                        //++iSegment;

                        //Element neighbour = s.Element; // 2015
                        //Element neighbour = doc.GetElement(s.ElementId); // 2016

                        //Curve curve = s.Curve; // 2015
                        //double length = s.le
                        XYZ p_DESFACE = s.coordenadasBorde.desface;

                        //generando una  linea auxiliar que representa una segmento del poligono de losa
                        XYZ startPont = s.coordenadasBorde.StartPoint;
                        XYZ EndPoint = s.coordenadasBorde.EndPoint;
                        double anguloSegmentoRoom = s.anguloGrado;

                        Line LineAuxRoomBordeRoom = Line.CreateBound(startPont, EndPoint);

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
#if false
                        if (false) // nose tuliza
                        {
                            //b)linea en la direccion perpendicular de la losa                             
                            IntersectionResultArray results_direccionPerpenBarra_2;
                            SetComparisonResult result_direccionPerpenBarra = LineAuxBuscarInters_direccionPerpenBarra.Intersect(LineAuxRoomBordeRoom, out results_direccionPerpenBarra_2);
                            if (result_direccionPerpenBarra == SetComparisonResult.Overlap)
                            {
                                IntersectionResult iResult = results_direccionPerpenBarra_2.get_Item(0);  // iResult.XYZPoint

                                // agrega pt de intesccion entre lines
                                ListaInterDirePerpLosa.Add(iResult.XYZPoint);
                            }
                        } 
#endif
                    }
                }


                Get2BordesOrdenados(pto1, aux_angle);
                //por si se encuentras mas de 2 intersecciones con borde de room
                //  if (ListaPtos.Count > 2)
                ListaInterseccion = GetListaInterseccion(ListaPtos, pto1, aux_angle);
                //si se encontraron 4 puntos de interseccion salenhsnhs
                if (ListaInterseccion.Count == 4) return ListaInterseccion;
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
        public  List<XYZ> ListaPoligonosRooms_fx(UIApplication _uiapp,ReferenciaRoomDatos refereciaRoomDatos, Options opt, TipoOrientacionBarra orientacion)
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
                double largoBarra =  ConstNH.LARGO_LINEA_AUXILIAR_FOOT;


                //genera linea auxiliar para buscar las intersecciones con el room
                Line LineAuxBuscarInters_direccionBarra = null;
                Line LineAuxBuscarInters_direccionPerpenBarra = null;

                //linea en la direccion de losa
                LineAuxBuscarInters_direccionBarra = Line.CreateBound(new XYZ(PtoSeleccionMouse.X - largoBarra * Math.Cos(Util.GradosToRadianes(AnguloDireccionBarra)), PtoSeleccionMouse.Y - largoBarra * Math.Sin(Util.GradosToRadianes(AnguloDireccionBarra)), PtoSeleccionMouse.Z),
                                                        new XYZ(PtoSeleccionMouse.X + largoBarra * Math.Cos(Util.GradosToRadianes(AnguloDireccionBarra)), PtoSeleccionMouse.Y + largoBarra * Math.Sin(Util.GradosToRadianes(AnguloDireccionBarra)), PtoSeleccionMouse.Z));
                //linea perpendicular a la direccion de losa
                LineAuxBuscarInters_direccionPerpenBarra = Line.CreateBound(new XYZ(PtoSeleccionMouse.X - largoBarra * Math.Cos(Util.GradosToRadianes(AnguloDireccionBarra) + Math.PI / 2), PtoSeleccionMouse.Y - largoBarra * Math.Sin(Util.GradosToRadianes(AnguloDireccionBarra) + Math.PI / 2), PtoSeleccionMouse.Z),
                                                        new XYZ(PtoSeleccionMouse.X + largoBarra * Math.Cos(Util.GradosToRadianes(AnguloDireccionBarra) + Math.PI / 2), PtoSeleccionMouse.Y + largoBarra * Math.Sin(Util.GradosToRadianes(AnguloDireccionBarra) + Math.PI / 2), PtoSeleccionMouse.Z));




                // int iBoundary = 0, iSegment;

                //foreach( BoundarySegmentArray b in boundaries ) // 2011
                BoundarySegmentRoomsGeom newlistaBS = new BoundarySegmentRoomsGeom(room.Document);
                // contiene una lista que tiene la lista de los boundery de los room, puede tener mas de una lista, por presencia de shaft, openin o muro dentro del room


                //selcciona la losa que contiene el room

                ConstNH.sbLog.Clear();
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
                foreach (WrapperBoundarySegment s in newlistaBS.ListaWrapperBoundarySegment)
                {
                    //++iSegment;

                    //Element neighbour = s.Element; // 2015
                    //Element neighbour = doc.GetElement(s.ElementId); // 2016

                    //Curve curve = s.Curve; // 2015
                    //double length = s.le
                    XYZ p_DESFACE = s.coordenadasBorde.desface;

                    //generando una  linea auxiliar que representa una segmento del poligono de losa
                    XYZ startPont = s.coordenadasBorde.StartPoint;
                    XYZ EndPoint = s.coordenadasBorde.EndPoint;
                    double anguloSegmentoRoom = s.anguloGrado;

                    Line LineAuxRoomBordeRoom = Line.CreateBound(startPont, EndPoint);

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

            Get2BordesOrdenados(PtoSeleccionMouse, AnguloDireccionBarra);
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
        private List<XYZ> GetListaInterseccion(Dictionary<XYZ, List<XYZ>> listaInterseccion, XYZ ptoMouse, double Angle_1)
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

            double dista_aux_negativo = 100000;
            double dista_aux_positivo = 100000;


            foreach (KeyValuePair<XYZ, List<XYZ>> item in listaInterseccion)
            {


                Transform trans1 = Transform.CreateTranslation(-ptoMouse);
                Transform trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(Angle_1), XYZ.Zero);

                //pto interseccion con borde de room
                XYZ p1 = trans2_rotacion.OfPoint(trans1.OfPoint(item.Key));

                double aux_distacia = XYZ.Zero.DistanceTo(p1);

                //si pto es anterior alpto mouse y distancia entre ellos es menor al de referencia (dista_aux_negativo)
                if (p1.X < 0 && aux_distacia < dista_aux_negativo)
                {                    //remplaza el valor auxiliar de referecnia
                    dista_aux_negativo = aux_distacia;
                    //limpia las lista aux 
                    ListaInterseccion_negativo_aux.Clear();
                    // copia los ptos   startPont  ,   EndPoint
                    ListaInterseccion_negativo_aux = item.Value;

                }//si pto es posterior al pto mouse y distancia entre ellos es menor al de referencia (dista_aux_positivo)
                else if (p1.X > 0 && aux_distacia < dista_aux_positivo)  ///  p1.X > 0
                {
                    //remplaza el valor auxiliar de referecnia
                    dista_aux_positivo = aux_distacia;
                    //limpia las lista aux 
                    ListaInterseccion_positivo_aux.Clear();
                    // copia los ptos   startPont  ,   EndPoint
                    ListaInterseccion_positivo_aux = item.Value;
                }


            }

            ListaInterseccion_aux.Add(ListaInterseccion_negativo_aux[0]);
            ListaInterseccion_aux.Add(ListaInterseccion_negativo_aux[1]);
            ListaInterseccion_aux.Add(ListaInterseccion_positivo_aux[0]);
            ListaInterseccion_aux.Add(ListaInterseccion_positivo_aux[1]);

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
        private  void Get2BordesOrdenados(XYZ ptoMouse, double Angle_1)
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


            WrapperBoundarySegment bordeNeg = null;
            WrapperBoundarySegment bordePos = null;

            List<XYZ> ListaInterseccion_negativo_aux = new List<XYZ>();
            List<XYZ> ListaInterseccion_positivo_aux = new List<XYZ>();
            List<XYZ> ListaInterseccion_aux = new List<XYZ>();

            double dista_aux_negativo = 100000;
            double dista_aux_positivo = 100000;


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
            if (bordeNeg == null || bordePos == null) return;
            listBorde.Add(bordeNeg);// izq- infe
            listBorde.Add(bordePos); // dere - sup

        }


        #endregion
    }
}
