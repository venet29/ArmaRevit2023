using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos.Ayuda;
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
    public class CalculosRefuerzoTipoBorde : ACalculosRefuerzo, ICalculosRefuerzo
    {

        private readonly double aumentarAnchoEstribo = ConstNH.aumentarAnchoEstribo_foot;
        private readonly double subirEStribo = Util.CmToFoot(4);//pq las bbarras estan mas abajo sobre la losa 2cm
        private readonly UIApplication _uiapp;
        private readonly View3D _elem3D;
        private readonly SeleccionarLosaConMouse _seleccionarLosaConMouse;
        public DatosRefuerzoTipoBorde _datosRefuerzoTipoBorde  { get; set; }
        public DatosRefuerzoTipoVigaDTO _datosRefuerzoTipoViga { get ; set; }


        // private int _numeroBArras;

        public int NumeroBArras { get; set; } = 3;
     
        private int esp_cm = ConstNH.REFLOSA_ESPACIEMIENTO_ENTREBARRA_CM;
        private int esp_borde_cm = 5;

        // private int[] numbersSup;
        // private int[] numbersInf;

        public CalculosRefuerzoTipoBorde(UIApplication uiapp, View3D elem3d, SeleccionarLosaConMouse seleccionarLosaConMouse, SeleccinarMuroRefuerzo lsm1, SeleccinarMuroRefuerzo lsm2, DatosRefuerzoTipoBorde _datosRefuerzoTipoBorde)
            : base(uiapp, lsm1, lsm2)
        {

            this.esp_cm = ConstNH.REFLOSA_ESPACIEMIENTO_ENTREBARRA_CM;
            this.esp_borde_cm = ConstNH.REFLOSA_ESPACIEMIENTO_BORDELOSA_CM;


          //  this._numeroBArras = _datosRefuerzoTipoBorde.CantidadBarras;


            ListaBArrasSuperior = new List<CalculoBarraRefuerzo>();
            ListaBArrasInferior = new List<CalculoBarraRefuerzo>();
            ListaEstriboRefuerzoDTO = new List<EstriboRefuerzoDTO>();
            this._uiapp = uiapp;
            this._elem3D = elem3d;
            this._seleccionarLosaConMouse = seleccionarLosaConMouse;
            this._datosRefuerzoTipoBorde = _datosRefuerzoTipoBorde;
            this.NumeroBArras = _datosRefuerzoTipoBorde.CantidadBarras;
            _tipoBarra = TipoBarraRefuerzo.BarraRefSinPatas;
        }


        public void Ejecutar()
        {
            ObtenerAngulodeSeleccion();
            OBtenerTrasformadas();
            Ordenar4PtosInicales();
            // ObtenerIntervalos();
            ObtenerOrientacionParaEnfierrar();
            GenerarPtosBarraRefuerzo();

            //esto es pq para  refuerzo se da la opcion de dibujar barras desde wpf
          _datosRefuerzoTipoViga = new DatosRefuerzoTipoVigaDTO() { IsBArras = true };

            if (_datosRefuerzoTipoBorde.IsEstribo)
            {
                if (NumeroBArras <= 3)
                    GenerarPtosBarraEStribo_UnEstribo();
                else
                    GenerarPtosBarraEStribo_dos();
            }
        }

        private void ObtenerOrientacionParaEnfierrar()
        {
            XYZ _ptoouse = trans2_rotacion.OfPoint(trans1.OfPoint(_seleccionarLosaConMouse._ptoSeleccionEnLosa));

            CalculoIntervalos _calculoIntervalos = new CalculoIntervalos(esp_borde_cm, esp_cm);
            _calculoIntervalos.ObtenerIntervalos_BordeLosa(NumeroBArras);
            if (_ptoouse.Y > 0) //la barra se dibujan haci arriba
            {
                numbersInf = new int[] { 0 };
                numbersSup = _calculoIntervalos.numbersSup;
            }
            else //la barra se dibujan haci abajo
            {
                numbersInf = _calculoIntervalos.numbersInf;
                numbersSup = new int[] { 0 };

            }

        }


        private void GenerarPtosBarraRefuerzo()
        {

            //agrega una linia arrba por ser cantidad de lines impar
            //  if ((numeroBArras % 2) == 0) masUnoPOrImpar = 1;


            //int[] numbersSup = { 5, 10 };
            XYZ direccionMoviminetoBarras = _lsm1.DireccionEnfierrado;
            CalculoBarraRefuerzoDTo _CalculoBarraRefuerzoDTo = new CalculoBarraRefuerzoDTo()
            {
                _empotramientoPatasDTO = _datosRefuerzoTipoBorde._empotramientoPatasDTO,
                diamtroBarraRefuerzo_MM = _datosRefuerzoTipoBorde.diamtroBarraRefuerzo_MM
            };
            foreach (int i in numbersSup)
            {
                double deltaEnY = Util.CmToFoot(i);
                CalculoBarraRefuerzo barraRefuerzoDTO = new CalculoBarraRefuerzo(_uiapp, "b" + i + "Sup", _CalculoBarraRefuerzoDTo,
                                   DesplazarBajoYModicaY(_p1Tras, deltaEnY), DesplazarBajoYModicaY(_p4Tras, deltaEnY),
                               Invertrans1, InverTrans2_rotacion, direccionMoviminetoBarras);
                barraRefuerzoDTO.generaPtosrExtremos();
                barraRefuerzoDTO.CalculosIniciales();
                barraRefuerzoDTO.BuscarPatasAmbosLadosHorizontal(_datosRefuerzoTipoBorde._empotramientoPatasDTO, _elem3D);
                ListaBArrasSuperior.Add(barraRefuerzoDTO);

                //solo para desarroollo
                //   ListaGraficar.Add(barraRefuerzoDTO.pa); ListaGraficar.Add(barraRefuerzoDTO.pb); ListaGraficar.Add(barraRefuerzoDTO.pc); ListaGraficar.Add(barraRefuerzoDTO.pd);
            }

            /// int[] numbersInf = { 5, 10 };
            foreach (int i in numbersInf)
            {
                double deltaEnY = -Util.CmToFoot(i);
                CalculoBarraRefuerzo barraRefuerzoDTO = new CalculoBarraRefuerzo(_uiapp, "b" + i + "Inf", _CalculoBarraRefuerzoDTo,
                                                    DesplazarBajoYModicaY(_p2Tras, deltaEnY), DesplazarBajoYModicaY(_p3Tras, deltaEnY),
                                                    Invertrans1, InverTrans2_rotacion, direccionMoviminetoBarras);
                barraRefuerzoDTO.generaPtosrExtremos();
                barraRefuerzoDTO.CalculosIniciales();
                barraRefuerzoDTO.BuscarPatasAmbosLadosHorizontal(_datosRefuerzoTipoBorde._empotramientoPatasDTO, _elem3D);
                ListaBArrasInferior.Add(barraRefuerzoDTO);

            }

            if (ListaBArrasSuperior.Count > 0)
                _tipoBarra = ListaBArrasSuperior[ListaBArrasSuperior.Count - 1].tipoBarraRefuerzo;
            else if (ListaBArrasInferior.Count > 0)
                _tipoBarra = ListaBArrasInferior[ListaBArrasInferior.Count - 1].tipoBarraRefuerzo;
            else
                _tipoBarra = TipoBarraRefuerzo.BarraRefSinPatas;



        }
        private void GenerarPtosBarraEStribo_UnEstribo()
        {

            //primer estribo
            CalculoBarraRefuerzo Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
            CalculoBarraRefuerzo Estribo1b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersSup[0]}Inf").FirstOrDefault();
            double anchoEstribo1 = Estribo1a.pb_Orig.DistanceTo(Estribo1b.pb_Orig) + aumentarAnchoEstribo * 2;
            EstriboRefuerzoDTO estriboRefuerzoDTO1 = new EstriboRefuerzoDTO(DesplazarArriba(Estribo1a.pb_Orig, Estribo1a.AnguloBarraRad),
                                                                            DesplazarArriba(Estribo1a.pb_Orig, Estribo1a.AnguloBarraRad),
                                                                            DesplazarArriba(Estribo1a.pc_Orig, Estribo1a.AnguloBarraRad),
                                                                            anchoEstribo1, Util.CmToFoot(_datosRefuerzoTipoBorde.espacimientoEstribo_Cm),
                                                                            _datosRefuerzoTipoBorde.diamtroEstribo_MM,
                                                                            (NumeroBArras < 4 ? TipoEstriboRefuerzoLosa.E : TipoEstriboRefuerzoLosa.ED),
                                                                            TipoRebar.REFUERZO_ES);

            ListaEstriboRefuerzoDTO.Add(estriboRefuerzoDTO1);
        }


        private void GenerarPtosBarraEStribo_dos()
        {
            CalculoBarraRefuerzo Estribo1a;
            CalculoBarraRefuerzo Estribo1b;
            //primer estribo
            if (ListaBArrasSuperior.Count > ListaBArrasInferior.Count)
            {
                Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
                Estribo1b = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[0]}Sup").FirstOrDefault();
            }
            else
            {
                Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[0]}Sup").FirstOrDefault();
                Estribo1b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 2]}Inf").FirstOrDefault();
            }
            double anchoEstribo1 = Estribo1a.pb_Orig.DistanceTo(Estribo1b.pb_Orig) + aumentarAnchoEstribo * 2;
            EstriboRefuerzoDTO estriboRefuerzoDTO1 = new EstriboRefuerzoDTO(DesplazarArriba(Estribo1a.pb_Orig, Estribo1a.AnguloBarraRad),
                                                                            DesplazarArriba(Estribo1a.pb_Orig, Estribo1a.AnguloBarraRad), 
                                                                            DesplazarArriba(Estribo1a.pc_Orig, Estribo1a.AnguloBarraRad),
                                                                            anchoEstribo1, Util.CmToFoot(_datosRefuerzoTipoBorde.espacimientoEstribo_Cm),
                                                                            _datosRefuerzoTipoBorde.diamtroEstribo_MM,
                                                                            (NumeroBArras < 4 ? TipoEstriboRefuerzoLosa.E : TipoEstriboRefuerzoLosa.ED),
                                                                             TipoRebar.REFUERZO_ES);

            ListaEstriboRefuerzoDTO.Add(estriboRefuerzoDTO1);

            //segundo estribo
            CalculoBarraRefuerzo Estribo2a;
            CalculoBarraRefuerzo Estribo2b;
            if (ListaBArrasSuperior.Count > ListaBArrasInferior.Count)
            {
                Estribo2a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 2]}Sup").FirstOrDefault();
                Estribo2b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[0]}Inf").FirstOrDefault();
            }
            else
            {
                Estribo2a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[0]}Inf").FirstOrDefault();
                Estribo2b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault();
            }
            XYZ desplaParaleloBArra = (Estribo2a.pc_Orig - Estribo2a.pb_Orig).Normalize() * Util.CmToFoot(3);
            double anchoEstribo2 = Estribo2a.pb_Orig.DistanceTo(Estribo2b.pb_Orig) + aumentarAnchoEstribo * 2;
            EstriboRefuerzoDTO estriboRefuerzoDTO2 = new EstriboRefuerzoDTO(DesplazarArriba(Estribo2a.pb_Orig, Estribo2a.AnguloBarraRad) + desplaParaleloBArra,
                                                                            DesplazarArriba(Estribo2a.pb_Orig, Estribo2a.AnguloBarraRad) + desplaParaleloBArra,
                                                                            DesplazarArriba(Estribo2a.pc_Orig, Estribo2a.AnguloBarraRad) + desplaParaleloBArra,
                                                                            anchoEstribo2, Util.CmToFoot(_datosRefuerzoTipoBorde.espacimientoEstribo_Cm),
                                                                            _datosRefuerzoTipoBorde.diamtroEstribo_MM,
                                                                            TipoEstriboRefuerzoLosa.ED,
                                                                            TipoRebar.REFUERZO_ES);
            ListaEstriboRefuerzoDTO.Add(estriboRefuerzoDTO2);
        }

        private XYZ DesplazarArriba(XYZ pb_Orig, double ang) => new XYZ(pb_Orig.X - aumentarAnchoEstribo * Math.Sin(ang), pb_Orig.Y + aumentarAnchoEstribo *Math.Cos(ang), pb_Orig.Z + subirEStribo);
        private XYZ DesplazarBajoYModicaY(XYZ pb_Orig, double desplaEnY) => new XYZ(pb_Orig.X, pb_Orig.Y + desplaEnY, pb_Orig.Z);
    }
}
