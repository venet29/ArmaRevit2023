using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos
{
    public class CalculosRefuerzoTipoViga : ACalculosRefuerzoV2, ICalculosRefuerzo
    {
        private readonly double aumentarAnchoEstribo = Util.CmToFoot(3);
        private readonly double aumentarDerechaEstribo = Util.CmToFoot(15);
        private readonly double subirEStribo = Util.CmToFoot(4);//pq las bbarras estan mas abajo sobre la losa 2cm
        private readonly UIApplication _uiapp;
        private readonly View3D _elem3D;
        //private readonly DatosRefuerzoTipoVigaDTO _datosRefuerzoTipoViga;
        public DatosRefuerzoTipoVigaDTO _datosRefuerzoTipoViga { get; set; }
        private string _tipoPosicionRef;
        private bool _IsBuscarTipo;
        private int esp_cm = 10;
        private int esp_borde_cm = 5;

        // private int _numeroBArras;

        public int NumeroBArras { get; set; } = 3;

        /*
p1 y p2 tiene que ser ptos a la izqAbajo
p3 yp4 tiene que ser tos al dereArriba

*/
        public CalculosRefuerzoTipoViga(UIApplication _uiapp, View3D elem3d, SeleccinarMuroRefuerzo lsm1, SeleccinarMuroRefuerzo lsm2, DatosRefuerzoTipoVigaDTO _datosRefuerzoTipoViga) : base(_uiapp, lsm1, lsm2)
        {
            this.esp_cm = ConstNH.REFLOSA_ESPACIEMIENTO_ENTREBARRA_CM;
            this.esp_borde_cm = ConstNH.REFLOSA_ESPACIEMIENTO_BORDELOSA_CM;
            this.NumeroBArras = _datosRefuerzoTipoViga.CantidadBarras;
            this.ListaBArrasSuperior = new List<CalculoBarraRefuerzo>();
            this.ListaBArrasInferior = new List<CalculoBarraRefuerzo>();
            this.ListaEstriboRefuerzoDTO = new List<EstriboRefuerzoDTO>();
            this._uiapp = _uiapp;
            this._elem3D = elem3d;
            this._datosRefuerzoTipoViga = _datosRefuerzoTipoViga;
            this._tipoPosicionRef = _datosRefuerzoTipoViga.tipoPosicionRef;
            this._tipoBarra = _datosRefuerzoTipoViga.TipoBarra;//   TipoBarraRefuerzo.BarraRefSinPatas;
            this._IsBuscarTipo = _datosRefuerzoTipoViga.IsBuscarPatas;
        }


        public bool Ejecutar()
        {
            try
            {
                ObtenerAngulodeSeleccion();
                OBtenerTrasformadas();
                Ordenar4PtosInicales();

                CalculoIntervalos _calculoIntervalos = new CalculoIntervalos(esp_borde_cm, esp_cm);

                if (_tipoPosicionRef == "Central")
                    _calculoIntervalos.ObtenerIntervalos(NumeroBArras);
                else if (_tipoPosicionRef == "Superior")
                    _calculoIntervalos.ObtenerIntervalosSup(NumeroBArras);
                else if (_tipoPosicionRef == "Inferior")
                    _calculoIntervalos.ObtenerIntervalosInf(NumeroBArras);

                numbersSup = _calculoIntervalos.numbersSup;
                numbersInf = _calculoIntervalos.numbersInf;

                GenerarPtosBarraRefuerzo();
         


                if (_datosRefuerzoTipoViga.IsEstribo && NumeroBArras>3)
                    GenerarPtosBarraEStribo(_tipoPosicionRef);
                else if (_datosRefuerzoTipoViga.IsEstribo && ( NumeroBArras == 2 || NumeroBArras == 3))
                    GenerarUNPtosBarraEStribo(_tipoPosicionRef);



            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void GenerarUNPtosBarraEStribo(string tipoPosicionRef)
        {
            //primer estribo
            CalculoBarraRefuerzo Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
            CalculoBarraRefuerzo Estribo1b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersSup[0]}Inf").FirstOrDefault();
            double anchoEstribo1 = Estribo1a.pb_Orig.DistanceTo(Estribo1b.pb_Orig) + aumentarAnchoEstribo * 2;
            EstriboRefuerzoDTO estriboRefuerzoDTO1 = new EstriboRefuerzoDTO(DesplazarArriba(Estribo1a.pb_Orig, Estribo1a.AnguloBarraRad),
                                                                            DesplazarArriba(Estribo1a.pb_Orig, Estribo1a.AnguloBarraRad),
                                                                            DesplazarArriba(Estribo1a.pc_Orig, Estribo1a.AnguloBarraRad),
                                                                            anchoEstribo1, Util.CmToFoot(_datosRefuerzoTipoViga.espacimientoEstribo_Cm),
                                                                            _datosRefuerzoTipoViga.diamtroEstribo_MM,
                                                                            (NumeroBArras < 4 ? TipoEstriboRefuerzoLosa.E : TipoEstriboRefuerzoLosa.ED),
                                                                            TipoRebar.REFUERZO_ES);

            ListaEstriboRefuerzoDTO.Add(estriboRefuerzoDTO1);
        }

        private void GenerarPtosBarraRefuerzo()
        {
            //int[] numbersSup = { 5, 10 };
            XYZ direccionMoviminetoBarras = (p2-p1 ).Normalize();
            CalculoBarraRefuerzoDTo _CalculoBarraRefuerzoDTo = new CalculoBarraRefuerzoDTo()
            {
                _empotramientoPatasDTO = _datosRefuerzoTipoViga._empotramientoPatasDTO,
                diamtroBarraRefuerzo_MM = _datosRefuerzoTipoViga.diamtroBarraRefuerzo_MM
            };

            foreach (int i in numbersSup)
            {
                double deltaEnY = Util.CmToFoot(i);
                CalculoBarraRefuerzo barraRefuerzoDTO = new CalculoBarraRefuerzo(_uiapp, "b" + i + "Sup", _CalculoBarraRefuerzoDTo,
                                   DesplazarBajoYModicaY(_p1Tras, deltaEnY), DesplazarBajoYModicaY(_p4Tras, deltaEnY),
                               Invertrans1, InverTrans2_rotacion, direccionMoviminetoBarras);
                barraRefuerzoDTO.generaPtosrExtremos();
                barraRefuerzoDTO.CalculosIniciales();
                if (_IsBuscarTipo)
                    barraRefuerzoDTO.BuscarPatasAmbosLadosHorizontal(_datosRefuerzoTipoViga._empotramientoPatasDTO, _elem3D);
                else
                    barraRefuerzoDTO.IngresarPatasLadosHorizontal(_datosRefuerzoTipoViga._empotramientoPatasDTO, _elem3D, _tipoBarra);
                ListaBArrasSuperior.Add(barraRefuerzoDTO);
            }

            /// int[] numbersInf = { 5, 10 };
            foreach (int i in numbersInf)
            {
     
                double deltaEnY = -Util.CmToFoot(i);
                CalculoBarraRefuerzo barraRefuerzoDTO = new CalculoBarraRefuerzo(_uiapp, "b" + i + "Inf", _CalculoBarraRefuerzoDTo,
                                                    DesplazarBajoYModicaY(_p2Tras, deltaEnY), DesplazarBajoYModicaY(_p3Tras, deltaEnY),
                                                    Invertrans1, InverTrans2_rotacion, -direccionMoviminetoBarras);
                barraRefuerzoDTO.generaPtosrExtremos();
                barraRefuerzoDTO.CalculosIniciales();
                if (_IsBuscarTipo)
                    barraRefuerzoDTO.BuscarPatasAmbosLadosHorizontal(_datosRefuerzoTipoViga._empotramientoPatasDTO, _elem3D);
                else
                    barraRefuerzoDTO.IngresarPatasLadosHorizontal(_datosRefuerzoTipoViga._empotramientoPatasDTO, _elem3D, _tipoBarra);
                ListaBArrasInferior.Add(barraRefuerzoDTO);

            }
            //_tipoBarra = ListaBArrasSuperior[ListaBArrasSuperior.Count - 1].tipoBarraRefuerzo;
        }
        private void GenerarPtosBarraEStribo(string _tipoPosicionRef)
        {

            //primer estribo
            CalculoBarraRefuerzo Estribo1a = null;//ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
            CalculoBarraRefuerzo Estribo1b = null;// ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersSup[0]}Inf").FirstOrDefault();


            if (_tipoPosicionRef == "Central")
            {
                Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
                Estribo1b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersSup[0]}Inf").FirstOrDefault();
            }
            else if (_tipoPosicionRef == "Superior")
            {
                Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
                Estribo1b = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[1]}Sup").FirstOrDefault();
            }
            else if (_tipoPosicionRef == "Inferior")
            {
                Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[0]}Inf").FirstOrDefault();
                Estribo1b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 2]}Inf").FirstOrDefault();
            }



            double anchoEstribo1 = Estribo1a.pb_Orig.DistanceTo(Estribo1b.pb_Orig) + aumentarAnchoEstribo * 2;
            XYZ despla2 = (Estribo1a.pb_Orig - Estribo1b.pb_Orig).Normalize() * Util.CmToFoot(3);
            EstriboRefuerzoDTO estriboRefuerzoDTO1 = new EstriboRefuerzoDTO(DesplazarArriba(Estribo1a.pb_Orig, Estribo1a.AnguloBarraRad) + despla2,
                                                                            DesplazarArriba(Estribo1a.pb_Orig, Estribo1a.AnguloBarraRad) + despla2,
                                                                            DesplazarArriba(Estribo1a.pc_Orig, Estribo1a.AnguloBarraRad) + despla2,
                                                                            anchoEstribo1,
                                                                            Util.CmToFoot(_datosRefuerzoTipoViga.espacimientoEstribo_Cm),
                                                                            _datosRefuerzoTipoViga.diamtroEstribo_MM,
                                                                            (NumeroBArras < 4 ? TipoEstriboRefuerzoLosa.E : TipoEstriboRefuerzoLosa.ED),
                                                                            TipoRebar.REFUERZO_ES);
            ListaEstriboRefuerzoDTO.Add(estriboRefuerzoDTO1);


            //segundo estribo
            if (NumeroBArras < 3) return;
            CalculoBarraRefuerzo Estribo2a = null;// ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersInf[0]}Sup").FirstOrDefault();
            CalculoBarraRefuerzo Estribo2b = null;// ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault();

            if (_tipoPosicionRef == "Central")
            {
                Estribo2a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersInf[0]}Sup").FirstOrDefault();
                Estribo2b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault();
            }
            else if (_tipoPosicionRef == "Superior")
            {
                Estribo2a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 2]}Sup").FirstOrDefault();
                Estribo2b = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[0]}Sup").FirstOrDefault();
            }
            else if (_tipoPosicionRef == "Inferior")
            {
                Estribo2a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[1]}Inf").FirstOrDefault();
                Estribo2b = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault();
            }

            double anchoEstribo2 = Estribo2a.pb_Orig.DistanceTo(Estribo2b.pb_Orig) + aumentarAnchoEstribo * 2;
            XYZ desplaParaleloBArra = (Estribo2a.pc_Orig - Estribo2a.pb_Orig).Normalize() * Util.CmToFoot(3);
            despla2 = (Estribo2a.pb_Orig - Estribo2b.pb_Orig).Normalize() * Util.CmToFoot(3);
            EstriboRefuerzoDTO estriboRefuerzoDTO2 = new EstriboRefuerzoDTO(DesplazarArriba(Estribo2a.pb_Orig, Estribo2a.AnguloBarraRad) + desplaParaleloBArra + despla2,
                                                                            DesplazarArriba(Estribo2a.pb_Orig, Estribo2a.AnguloBarraRad) + desplaParaleloBArra + despla2,
                                                                            DesplazarArriba(Estribo2a.pc_Orig, Estribo2a.AnguloBarraRad) + desplaParaleloBArra + despla2,
                                                                            anchoEstribo2, Util.CmToFoot(_datosRefuerzoTipoViga.espacimientoEstribo_Cm),
                                                                            _datosRefuerzoTipoViga.diamtroEstribo_MM,
                                                                            TipoEstriboRefuerzoLosa.ED,
                                                                            TipoRebar.REFUERZO_ES);

            ListaEstriboRefuerzoDTO.Add(estriboRefuerzoDTO2);

        }
        private XYZ DesplazarArriba(XYZ pb_Orig, double ang) => new XYZ(pb_Orig.X, pb_Orig.Y + aumentarAnchoEstribo * Math.Sin(ang), pb_Orig.Z + subirEStribo);
        private XYZ DesplazarBajoYModicaY(XYZ pb_Orig, double desplaEnY) => new XYZ(pb_Orig.X, pb_Orig.Y + desplaEnY, pb_Orig.Z);
    }
}
