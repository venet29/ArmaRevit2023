using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Contenedores;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ArmaduraLosaRevit.Model.Traslapo.Calculos
{
    public class CalculoDatosParaReinformentV2 : iCalculoDatosParaReinforment
    {


        public ContenedorDatosPathReinformeDTO datosNuevoPathIzqAbajoDTO { get; set; }
        public ContenedorDatosPathReinformeDTO datosNuevoPathDereArribaDTO { get; set; }


        private Line lineSentidoBarraInical;
        private Line lineSentidoBarraFinal;
        public CoordenadaPath _coordCalculos { get; set; }
        private XYZ _puntoSeleccionMouse;
        private bool _isDibujarPath;
        public bool IsOK { get; set; }
        private ContenedorDatosLosaDTO _datosLosaPath1DTO;
        private ContenedorDatosLosaDTO _datosLosaPath2DTO;

        private XYZ ptoInterIni_pA;
        private XYZ ptoInterFinal_pB;

        public CalculoDatosParaReinformentV2() { }


        public CalculoDatosParaReinformentV2(CoordenadaPath coordCalculos, XYZ puntoSeleccionMouse,
                                          ContenedorDatosLosaDTO _datosLosaPath1DTO, ContenedorDatosLosaDTO _datosLosaPath2DTO, bool Isok = true)
        {
            this._coordCalculos = coordCalculos;
            this._puntoSeleccionMouse = puntoSeleccionMouse;

            this._isDibujarPath = true;
            this._datosLosaPath1DTO = _datosLosaPath1DTO;
            this._datosLosaPath2DTO = _datosLosaPath2DTO;
            this.IsOK = Isok;

        }

        public bool M1_Obtener2PathReinformeTraslapoDatos()
        {
            try
            {
                if (_coordCalculos == null) return false;
                if (!_isDibujarPath) return false;

                if (!M1_1_ObtenerLineasParalelasBarra()) return false;

               
                M1_2_ObtenerPtoInterIni_PA();
                M1_3_ObtenerptoInterIni_PB();
                if (!M1_4_ObtenerPtoInterseccionLineasParalelasBarras_paraObtenerContenedorDatosPathReinforme_IzqAbajo()) return false;
                if (!M1_5_ObtenerPtoInterseccionLineasParalelasBarras_paraObtenerContenedorDatosPathReinforme_DereArriba()) return false;

#pragma warning disable CS0219 // The variable 'i' is assigned but its value is never used
                int i = 0;
#pragma warning restore CS0219 // The variable 'i' is assigned but its value is never used
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($" Error en 'M1_Obtener2PathReinformeTraslapoDatos' ex:{ex.Message}");
                return false;
            }
            return true;

        }

   

        private bool M1_1_ObtenerLineasParalelasBarra()
        {
            try
            {


                //crar linea en sentido de barra
                if (Util.IsIntersection(_coordCalculos.p1, _coordCalculos.p3,
                                        _coordCalculos.p2, _coordCalculos.p4))
                {
                    lineSentidoBarraInical = Line.CreateBound(_coordCalculos.p1, _coordCalculos.p4);
                    lineSentidoBarraFinal = Line.CreateBound(_coordCalculos.p2, _coordCalculos.p3);

                }
                else
                {
                    lineSentidoBarraInical = Line.CreateBound(_coordCalculos.p1, _coordCalculos.p3);
                    lineSentidoBarraFinal = Line.CreateBound(_coordCalculos.p2, _coordCalculos.p4);

                    XYZ pto3_auxiliar = _coordCalculos.p3;
                    _coordCalculos.p3 = _coordCalculos.p4;
                    _coordCalculos.p4 = pto3_auxiliar;
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'M1_1_ObtenerLineasParalelasBarra' ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private void M1_2_ObtenerPtoInterIni_PA()
        {
            IntersectionResult ptoInterseccionLineSentidoBarraInical = lineSentidoBarraInical.Project(_puntoSeleccionMouse);
            if (ptoInterseccionLineSentidoBarraInical == null) Util.ErrorMsg("Error al seleccionar ptoInterIni(p4)");
            ptoInterIni_pA = ptoInterseccionLineSentidoBarraInical.XYZPoint;

        }

        private void M1_3_ObtenerptoInterIni_PB()
        {
            IntersectionResult ptoInterseccionLineSentidoBarraFinal = lineSentidoBarraFinal.Project(_puntoSeleccionMouse);
            if (ptoInterseccionLineSentidoBarraFinal == null) return;
            ptoInterFinal_pB = ptoInterseccionLineSentidoBarraFinal.XYZPoint;

        }
        //NOTA:ob1
        private bool M1_4_ObtenerPtoInterseccionLineasParalelasBarras_paraObtenerContenedorDatosPathReinforme_IzqAbajo()
        {
            try
            {


                //  ContenedorDatosLosa _datosLosaPath1 = new ContenedorDatosLosa(_calculoTiposTraslapos.TipoPathReinf_IzqBajo, _datosLosaYpathIniciales);
                //primera path  izq, abajo
                //   pt1(p1)          ptoInterIni(pA) -- PA1
                //             |pto mouse
                //   pt2(p2)        ptoIniterFinal(pB)--PB2

                XYZ ptoInterIni_pA1 = UtilBarras.extenderLineaDistancia(_coordCalculos.p1, ptoInterIni_pA, _datosLosaPath1DTO._largoTraslapoFoot / 2)[0];

                XYZ ptoIniterFinal_pB1 = UtilBarras.extenderLineaDistancia(_coordCalculos.p2, ptoInterFinal_pB, _datosLosaPath1DTO._largoTraslapoFoot / 2)[0];

                IList<Curve> curvesPathreiforment_IzqInfe = new List<Curve>();
                curvesPathreiforment_IzqInfe.Add(Line.CreateBound(ptoInterIni_pA1, ptoIniterFinal_pB1)); //opcion original y correcta

                double largoPathReinfIzqAbajo = _coordCalculos.p2.DistanceTo(ptoIniterFinal_pB1); //p2 a p3

                datosNuevoPathIzqAbajoDTO = new ContenedorDatosPathReinformeDTO(_datosLosaPath1DTO, curvesPathreiforment_IzqInfe, largoPathReinfIzqAbajo);


                datosNuevoPathIzqAbajoDTO.ptoTagSoloTraslapo = M1_4_1_ObtenerPosicionPtoMouseParaGenerarTag_IZqInferior(ptoInterIni_pA1);

                datosNuevoPathIzqAbajoDTO.AnguloP2toP3Rad = Util.angulo_entre_pt_Rad_XY0(_coordCalculos.p2, ptoIniterFinal_pB1);

                datosNuevoPathIzqAbajoDTO.Lista4ptosPAth = M1_4_2_ObterListaCoordPathIzqInferior(ptoInterIni_pA1, ptoIniterFinal_pB1);

                _isDibujarPath = true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtenerPtoInterseccionLineasParalelasBarras4' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private XYZ M1_4_1_ObtenerPosicionPtoMouseParaGenerarTag_IZqInferior(XYZ ptoInterIni_pA1)
        {
            double anguloHaciaAtrasPtoMEdio = Util.angulo_entre_pt_Rad_XY0(ptoInterIni_pA1, _coordCalculos.p1);
            XYZ ptoPAraTAg = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_puntoSeleccionMouse, anguloHaciaAtrasPtoMEdio, _coordCalculos.p1.DistanceTo(ptoInterIni_pA1) * 0.4);
            return ptoPAraTAg;
        }

        private List<XYZ> M1_4_2_ObterListaCoordPathIzqInferior(XYZ ptoInterIni_pA1, XYZ ptoIniterFinal_pB1)
        {
            //agregar a lista pto path --> se utliza para buscar coordenadas de tag
            List<XYZ> Lista4ptosPAth = new List<XYZ>();
            Lista4ptosPAth.Add(_coordCalculos.p1); Lista4ptosPAth.Add(_coordCalculos.p2);
            Lista4ptosPAth.Add(ptoIniterFinal_pB1); Lista4ptosPAth.Add(ptoInterIni_pA1);
            return Lista4ptosPAth;
        }


        //NOTA:ob1
        private bool M1_5_ObtenerPtoInterseccionLineasParalelasBarras_paraObtenerContenedorDatosPathReinforme_DereArriba()
        {
            try
            {
                // ContenedorDatosLosa _datosLosaPath2 = new ContenedorDatosLosa(_calculoTiposTraslapos.TipoPathReinf_DerArriba, _datosLosaYpathIniciales);
                //segundo path  dere,arrib
                //   ptoInterIni(pA)         pto4(p4)
                //       |
                //  ptoIniterFinal(pB)       pt3(p3)



                XYZ p3_des = Util.ExtenderPuntoCOnRespeco2ptos(_coordCalculos.p4, _coordCalculos.p3, Util.CmToFoot(ConstNH.CONST_DESPLA_ENTRE_BARRAS_TRASLAPO_CM));
                XYZ p4_des = Util.ExtenderPuntoCOnRespeco2ptos(_coordCalculos.p3, _coordCalculos.p4, -Util.CmToFoot(ConstNH.CONST_DESPLA_ENTRE_BARRAS_TRASLAPO_CM));

                XYZ ptoInterIni_pB_des = Util.ExtenderPuntoCOnRespeco2ptos(ptoInterIni_pA, ptoInterFinal_pB, Util.CmToFoot(ConstNH.CONST_DESPLA_ENTRE_BARRAS_TRASLAPO_CM));
                XYZ ptoInterIni_pA_des = Util.ExtenderPuntoCOnRespeco2ptos(ptoInterFinal_pB, ptoInterIni_pA, -Util.CmToFoot(ConstNH.CONST_DESPLA_ENTRE_BARRAS_TRASLAPO_CM));


                XYZ ptoInterIni_pA2_des = UtilBarras.extenderLineaDistancia(ptoInterIni_pA_des, p4_des, _datosLosaPath1DTO._largoTraslapoFoot / 2)[1];
                XYZ ptoIniterFinal_pB2_des = UtilBarras.extenderLineaDistancia(ptoInterIni_pB_des, p3_des, _datosLosaPath1DTO._largoTraslapoFoot / 2)[1];


                IList<Curve> curvesPathreiforment_DereSup = new List<Curve>();
                curvesPathreiforment_DereSup.Add(Line.CreateBound(p4_des, p3_des));

                double largoPathReinfDereArriba = ptoInterIni_pA_des.DistanceTo(p4_des) + _datosLosaPath2DTO._largoTraslapoFoot / 2;

                datosNuevoPathDereArribaDTO = new ContenedorDatosPathReinformeDTO(_datosLosaPath2DTO, curvesPathreiforment_DereSup, largoPathReinfDereArriba);

                datosNuevoPathDereArribaDTO.ptoTagSoloTraslapo = M1_5_1_ObtenerPosicionPtoMouseParaGenerarTag_DereSup(p4_des, ptoInterIni_pA2_des, largoPathReinfDereArriba);

                datosNuevoPathDereArribaDTO.AnguloP2toP3Rad = Util.angulo_entre_pt_Rad_XY0(ptoIniterFinal_pB2_des, p3_des);
                //agregar a lista pto path --> se utliza para buscar coordenadas de tag
                datosNuevoPathDereArribaDTO.Lista4ptosPAth = M1_5_2_ObterListaCoordPathDereSup(p3_des, p4_des, ptoInterIni_pA2_des, ptoIniterFinal_pB2_des);

                _isDibujarPath = true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtenerPtoInterseccionLineasParalelasBarras5' ex:{ex.Message}");
                return false;
            }
            return true;
        }


        private XYZ M1_5_1_ObtenerPosicionPtoMouseParaGenerarTag_DereSup(XYZ ptoInterIni_p4_des, XYZ ptoInterIni_pA2, double largoPathReinfDereArriba)
        {
            double anguloHaciaAtrasPtoMEdio = Util.angulo_entre_pt_Rad_XY0(ptoInterIni_pA2, ptoInterIni_p4_des);
            XYZ ptoPAraTAg = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_puntoSeleccionMouse, anguloHaciaAtrasPtoMEdio, largoPathReinfDereArriba * 0.4, Util.CmToFoot(-5));
            return ptoPAraTAg;
        }
        private static List<XYZ> M1_5_2_ObterListaCoordPathDereSup(XYZ ptoIniterFinal_p3_des, XYZ ptoInterIni_p4_des, XYZ ptoInterIni_pA2, XYZ ptoIniterFinal_pB2)
        {
            List<XYZ> Lista4ptosPAth = new List<XYZ>();
            Lista4ptosPAth.Add(ptoInterIni_pA2); Lista4ptosPAth.Add(ptoIniterFinal_pB2);
            Lista4ptosPAth.Add(ptoIniterFinal_p3_des); Lista4ptosPAth.Add(ptoInterIni_p4_des);
            return Lista4ptosPAth;
        }

        public bool M2_IsPuedoDibujarPath()
        {
            return _isDibujarPath;
        }


    }
}
