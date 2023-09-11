using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.TipoTag;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos
{
    public class CalculosRefuerzoCabezaMuroV2 : ACalculosRefuerzo, ICalculosRefuerzo
    {


        //private double aumentarAnchoEstribo = Util.CmToFoot(3);
        //private double subirEStribo = Util.CmToFoot(4);//pq las bbarras estan mas abajo sobre la losa 2cm
        //                                               //private readonly double desplazamientoBArraAbajo = Util.CmToFoot(2);
        private double LargoRetroceso = 0.1;
        private int _diamEnMM;
        private XYZ _direcDereSup;
        private XYZ _direccionDistribucionBarra;
        private XYZ _direcIzqInf;
        private readonly UIApplication _uiapp;
        private readonly View3D _elem3D;
        private readonly DatosRefuerzoCabezaMuroDTO _datosRefuerzoCabezaMuroDTO;
        public DatosRefuerzoTipoVigaDTO _datosRefuerzoTipoViga { get; set; }
        public int NumeroBArras { get; set; }
        /*
p1 y p2 tiene que ser ptos a la izqAbajo
p3 yp4 tiene que ser tos al dereArriba

*/
        public CalculosRefuerzoCabezaMuroV2(UIApplication uiapp, View3D elem3d, SeleccinarMuroRefuerzo lsm1, SeleccinarMuroRefuerzo lsm2, DatosRefuerzoCabezaMuroDTO datosRefuerzoCabezaMuroDTO) : base(uiapp, lsm1, lsm2)
        {

            this.NumeroBArras = datosRefuerzoCabezaMuroDTO.CantidadBarras;
            this._diamEnMM = datosRefuerzoCabezaMuroDTO.diamtroBarraRefuerzo_MM;
            ListaBArrasSuperior = new List<CalculoBarraRefuerzo>();
            ListaBArrasInferior = new List<CalculoBarraRefuerzo>();
            ListaEstriboRefuerzoDTO = new List<EstriboRefuerzoDTO>();
            this._uiapp = uiapp;
            this._elem3D = elem3d;
            this._datosRefuerzoCabezaMuroDTO = datosRefuerzoCabezaMuroDTO;
            _tipoBarra = TipoBarraRefuerzo.BarraRefSinPatas;
            if (_datosRefuerzoCabezaMuroDTO.IsUsar2Pto) LargoRetroceso = 0.0f;
        }


        public bool Ejecutar()
        {
            try
            {
                CalculosIniciales();
                ObtenerAngulodeSeleccion();
                OBtenerTrasformadas();
                Ordenar4PtosInicales();
               
                p1 = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(_p1Tras));
                p2 = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(_p2Tras));
                p3 = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(_p3Tras));
                p4 = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(_p4Tras));
                _direccionDistribucionBarra = (p1 - p2).Normalize();
                //   pa_Orig = _Invertrans1.OfPoint(_InverTrans2_rotacion.OfPoint(pa)) - new XYZ(0, 0, desplazamientoBArraAbajo);

                ObtenerIntervalos();
                GenerarPtosBarraRefuerzo();
                //GenerarPtosBarraEStribo();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }

        private void ObtenerIntervalos()
        {
            if (NumeroBArras == 1)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { };
            }
            if (NumeroBArras == 2)
            {
                numbersSup = new int[] { 5 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 3)
            {
                numbersSup = new int[] { 5 };
                numbersInf = new int[] { 5 };
            }
            else if (NumeroBArras == 4)
            {
                numbersSup = new int[] { 5, 15 };
                numbersInf = new int[] { 5 };
            }
            else if (NumeroBArras == 5)
            {
                numbersSup = new int[] { 5, 15 };
                numbersInf = new int[] { 5, 15 };
            }
            else if (NumeroBArras == 6)
            {
                numbersSup = new int[] { 5, 15, 25 };
                numbersInf = new int[] { 5, 15 };
            }
            else if (NumeroBArras == 7)
            {
                numbersSup = new int[] { 5, 15, 25 };
                numbersInf = new int[] { 5, 15, 25 };
            }
            else
            {
                numbersSup = new int[] { 5, 15, 25, 35, };
                numbersInf = new int[] { 5, 15, 25 };
            }


        }

        public void CalculosIniciales()
        {
            _direcDereSup = ObtenerDireccionMoverPto(_lsm1);
            p1 = _lsm1.ListaPtosBordeMuroIntersectado[0] + _direcDereSup * LargoRetroceso;
            p2 = _lsm1.ListaPtosBordeMuroIntersectado[1] + _direcDereSup * LargoRetroceso;


            _direcIzqInf = ObtenerDireccionMoverPto(_lsm2);
            p3 = _lsm2.ListaPtosBordeMuroIntersectado[0] + _direcIzqInf * LargoRetroceso;
            p4 = _lsm2.ListaPtosBordeMuroIntersectado[1] + _direcIzqInf * LargoRetroceso;

      
            if (Util.IsIntersection2(p1, p4, p2, p3))
            {

                XYZ auxP = p4;
                p4 = p3;
                p3 = auxP;

            }

            listaPtos.Clear();
            listaPtos.Add(p1);
            listaPtos.Add(p2);
            listaPtos.Add(p3);
            listaPtos.Add(p4);

            pInicialMouse = (p1 + p2) / 2;
            pFinalMouse = (p3 + p4) / 2;
        }

        public XYZ ObtenerDireccionMoverPto(SeleccinarMuroRefuerzo ums)
        {
            XYZ PtoSObreLInea = XYZ.Zero;

            if (ums.ListaPtosBordeMuroIntersectado.Count != 2) return PtoSObreLInea;

            Line line = Line.CreateBound(ums.ListaPtosBordeMuroIntersectado[0], ums.ListaPtosBordeMuroIntersectado[1]);
            PtoSObreLInea = line.ProjectExtendida3D(ums.pto1SeleccionadoConMouse);
            XYZ direcion = (ums.pto1SeleccionadoConMouse - PtoSObreLInea).Normalize();

            return direcion;
        }




        private void GenerarPtosBarraRefuerzo()
        {

            //barra central
            XYZ direccionMoviminetoBarras = _direccionDistribucionBarra;
#pragma warning disable CS0219 // The variable 'deltaEnYCentral' is assigned but its value is never used
            double deltaEnYCentral = 0;
#pragma warning restore CS0219 // The variable 'deltaEnYCentral' is assigned but its value is never used
            CalculoBarraRefuerzoV2 barraRefuerzoDTOCentral = new CalculoBarraRefuerzoV2(_uiapp, "b0Sup", _diamEnMM,
                              (p1 + p2) / 2, (p4 + p3) / 2,
                              LargoRetroceso, direccionMoviminetoBarras, _lsm1.NormalCaraSuperiorLosa, _datosRefuerzoCabezaMuroDTO.IsUsar2Pto);
            barraRefuerzoDTOCentral.generaPtosrExtremos_cabezamuro();
             barraRefuerzoDTOCentral.CalculosIniciales();
            barraRefuerzoDTOCentral.BuscarPatasAmbosLadosHorizontal(_datosRefuerzoCabezaMuroDTO._empotramientoPatasDTO, _elem3D);
            ListaBArrasSuperior.Add(barraRefuerzoDTOCentral.ObtenerbarraRefuerzoDTOCentralOriginal());

            //  int[] numbersSup = {5, 15 };
            foreach (int i in numbersSup)
            {
                double deltaEnY = Util.CmToFoot(i);
                CalculoBarraRefuerzoV2 barraRefuerzoDTO = new CalculoBarraRefuerzoV2(_uiapp, "b" + i + "Sup", _diamEnMM,
                                   DesplazarArriba(p1, deltaEnY), DesplazarArriba(p4, deltaEnY),
                                   LargoRetroceso, direccionMoviminetoBarras, _lsm1.NormalCaraSuperiorLosa, _datosRefuerzoCabezaMuroDTO.IsUsar2Pto);
                barraRefuerzoDTO.generaPtosrExtremos_cabezamuro();
                  barraRefuerzoDTO.CalculosIniciales();
                barraRefuerzoDTO.BuscarPatasAmbosLadosHorizontal(_datosRefuerzoCabezaMuroDTO._empotramientoPatasDTO, _elem3D);
                ListaBArrasSuperior.Add(barraRefuerzoDTO.ObtenerbarraRefuerzoDTOCentralOriginal());

                //solo para desarroollo
                //  ListaGraficar.Add(barraRefuerzoDTO.pa); ListaGraficar.Add(barraRefuerzoDTO.pb); ListaGraficar.Add(barraRefuerzoDTO.pc); ListaGraficar.Add(barraRefuerzoDTO.pd);
            }


            //  int[] numbersInf = { 5, 15 };
            foreach (int i in numbersInf)
            {
                double deltaEnY = -Util.CmToFoot(i);
                CalculoBarraRefuerzoV2 barraRefuerzoDTO = new CalculoBarraRefuerzoV2(_uiapp, "b" + i + "Inf", _diamEnMM,
                                                    Desplazarbajo(p2, deltaEnY), Desplazarbajo(p3, deltaEnY),
                                                     LargoRetroceso, -direccionMoviminetoBarras, _lsm1.NormalCaraSuperiorLosa, _datosRefuerzoCabezaMuroDTO.IsUsar2Pto);
                barraRefuerzoDTO.generaPtosrExtremos_cabezamuro();
               barraRefuerzoDTO.CalculosIniciales();
                barraRefuerzoDTO.BuscarPatasAmbosLadosHorizontal(_datosRefuerzoCabezaMuroDTO._empotramientoPatasDTO, _elem3D);
                ListaBArrasInferior.Add(barraRefuerzoDTO.ObtenerbarraRefuerzoDTOCentralOriginal());
            }


        }


        private XYZ DesplazarBajoYModicaY(XYZ pb_Orig, double desplaEnY) => new XYZ(pb_Orig.X, pb_Orig.Y + desplaEnY, pb_Orig.Z);

        private XYZ DesplazarArriba(XYZ pX_Orig, double desplaEnY) => pX_Orig + _direccionDistribucionBarra * desplaEnY;

        private XYZ Desplazarbajo(XYZ pX_Orig, double desplaEnY) => pX_Orig + _direccionDistribucionBarra * desplaEnY;


    }
}
