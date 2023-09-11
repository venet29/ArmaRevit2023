using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.NewFolder1;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
{

    public class TiposRebarShapeDTO
    {
        public double EspesorLosa_1 { get; set; }
        public double LargoPathreiforment { get; set; }
        public double LargoMin_1 { get; set; }

        public string TipoBarra { get; set; }
        public UbicacionLosa UbicacionEnlosa { get; set; }
        public UIApplication uiapp { get; set; }
        public bool IsLuzSecuandiria { get; internal set; }
        public double DiametroMM { get; internal set; }
        public double LargoAhoraDefinidoUsuario { get; internal set; }
    }
    public class TiposRebarShape : ITiposRebarShape
    {
        private double _LargoMin_1;
        private Document _doc;
        private double _EspesorLosa_1;
        private double _EspesorMuro_Izq_abajo;
        private double _EspesorMuro_Dere_Sup;
        private string versionPAth = "v2";
        private UbicacionLosa _ubicacionEnlosa;
        private string _TipoBarra;
        private double LargoPathreiforment;
        private bool IsLuzSecuandiria;
        private double LargoAhoraDefinidoUsuario;

        //private double _diametroBarraDibujada;
        private DatosNuevaBarraDTO datosNuevaBarra;
        private string nombreFamiliaRebarShape;
        private string nombreFamiliaRebarShapeAlternativo;
        private double LargoAhorroIzq;
        private double LargoAhorroDere;
        private double LargoPaTa;
        private double largoPataInf;
        private double distanciaBordeLosaHastaEscalera;
        private double desplazamientoPorLuzSecundaria;
        private double _EspesorLosa_EnFoot;

        public DimensionesBarras dimBarras { get; private set; }
        public DimensionesBarras dimBarrasAlternativa { get; private set; }
        public DimensionesBarras dimBarras_parameterSharedLetras { get; private set; }
        public bool IsBarrAlternative { get; private set; }
        public RebarShape tipoRebarShapeAlternativa { get; private set; }
        public RebarShape tipoRebarShapePrincipal { get; private set; }
        public double espesorEscalera { get; private set; }

        public TiposRebarShape(SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO datosNuevaBarraDTO, double EspesorLosa_1)
        {
            this.datosNuevaBarra = datosNuevaBarraDTO;

            this._doc = solicitudDTO.UIdoc.Document;
            this._ubicacionEnlosa = solicitudDTO.UbicacionEnlosa;
            this._TipoBarra = solicitudDTO.TipoBarra;

            this._LargoMin_1 = datosNuevaBarraDTO.LargoMininoLosa;
            this.LargoPathreiforment = datosNuevaBarraDTO.LargoPathreiforment;
            this.IsLuzSecuandiria = datosNuevaBarra.IsLuzSecuandiria;
            this._EspesorLosa_1 = EspesorLosa_1;
            //  this._diametroBarraDibujada = datosNuevaBarraDTO.DiametroMM;
            nombreFamiliaRebarShape = "";
            nombreFamiliaRebarShapeAlternativo = "";
            //locales
            _EspesorMuro_Izq_abajo = ConstNH.ESPESORMURO_Izq_abajoFOOT;
            _EspesorMuro_Dere_Sup = ConstNH.ESPESORMURO_Dere_SupFOOT;
            LargoAhoraDefinidoUsuario = 0;


        }



        public TiposRebarShape(TiposRebarShapeDTO _TiposRebarShapeDTO)
        {
            this.datosNuevaBarra = new DatosNuevaBarraDTO();

            this._doc = _TiposRebarShapeDTO.uiapp.ActiveUIDocument.Document;
            this._ubicacionEnlosa = _TiposRebarShapeDTO.UbicacionEnlosa;
            this._TipoBarra = _TiposRebarShapeDTO.TipoBarra;

            this._LargoMin_1 = _TiposRebarShapeDTO.LargoMin_1;
            this.LargoPathreiforment = _TiposRebarShapeDTO.LargoPathreiforment;
            this.datosNuevaBarra.DiametroMM = _TiposRebarShapeDTO.DiametroMM;
            this._EspesorLosa_1 = _TiposRebarShapeDTO.EspesorLosa_1;
            this.IsLuzSecuandiria = _TiposRebarShapeDTO.IsLuzSecuandiria;
            this.LargoAhoraDefinidoUsuario = _TiposRebarShapeDTO.LargoAhoraDefinidoUsuario;
            //  this._diametroBarraDibujada = datosNuevaBarraDTO.DiametroMM;
            nombreFamiliaRebarShape = "";
            nombreFamiliaRebarShapeAlternativo = "";
            //locales
            _EspesorMuro_Izq_abajo = ConstNH.ESPESORMURO_Izq_abajoFOOT;
            _EspesorMuro_Dere_Sup = ConstNH.ESPESORMURO_Dere_SupFOOT;

        }
        /// <summary>
        /// define la forma del rebarshape para crear el pathreinformet
        /// modificar los valores de los parametros internos de los rebar shape para poder dibujar las barra a medida
        /// modifica :  A,B,C,D,E
        /// </summary>
        /// <param name="ubicacionEnlosa"></param>
        public DatosNuevaBarraDTO DefinirRebarShape()
        {
            //solo para 


            CalcularLargosYDesplazamientos();


            //valor no definido, conversacion conversacion 04/08/2021
            largoPataInf = Util.CmToFoot(ConstNH.CONST_PATA_SX); // Util.MmToFoot(20 * datosNuevaBarra.DiametroMM + 100);

            string aux_Letracambiar = TipoLetraCambiar.ObtenerLetra(_TipoBarra);

            switch (_TipoBarra)
            {

                case "f1_a":
                case "f1_b":
                    // barra Principal
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1B";
                    //para obtener familia y modificar   //para obtener familia
                    dimBarras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: Util.CmToFoot(30), e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    //para guardar datos internos

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: Util.CmToFoot(30), e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);

                    IsBarrAlternative = false;
                    break;


                case "f1_esc135_sinpata"://sinpata  ---> seria f1A
                    // barra Principal
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1B";
                    //para obtener familia y modificar   //para obtener familia
                    dimBarras = new DimensionesBarras(a: Util.CmToFoot(10), b: Util.CmToFoot(12), c: LargoPathreiforment, d: Util.CmToFoot(60), e: Util.CmToFoot(12), g: Util.CmToFoot(10), caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos internos

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: Util.CmToFoot(10), b: Util.CmToFoot(12), c: LargoPathreiforment, d: Util.CmToFoot(60), e: Util.CmToFoot(12), g: Util.CmToFoot(10), caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);

                    IsBarrAlternative = false;
                    break;
                case "f1_esc45_conpata"://con pata s---> seria f1B
                    // barra Principal
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1B";
                    //para obtener familia y modificar   //para obtener familia
                    dimBarras = new DimensionesBarras(a: Util.CmToFoot(10), b: Util.CmToFoot(12), c: LargoPathreiforment, d: Util.CmToFoot(60), e: Util.CmToFoot(12), g: Util.CmToFoot(10), caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos internos

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: Util.CmToFoot(10), b: Util.CmToFoot(12), c: LargoPathreiforment, d: Util.CmToFoot(60), e: Util.CmToFoot(12), g: Util.CmToFoot(10), caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);

                    IsBarrAlternative = false;
                    break;

                case "f1_SUP":
                    // barra Principal
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1B";
                    //para obtener familia y modificar   //para obtener familia
                    dimBarras = new DimensionesBarras(a: largoPataInf, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos internos

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: largoPataInf, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);

                    IsBarrAlternative = false;
                    break;
                case "f9a":
                case "f1":
                    // barra Principal
                    //LargoPaTa = LargoPaTa + Util.MmToFoot(datosNuevaBarra.DiametroMM);
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1B";
                    //para obtener familia y modificar   //para obtener familia                    
                    dimBarras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos internos

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);

                    IsBarrAlternative = false;
                    break;
                case "s2":
                    nombreFamiliaRebarShape = "M_00";
                    dimBarras = new DimensionesBarras(a: Util.CmToFoot(ConstNH.CONST_PATA_SX), b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: 0, b: 0, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    IsBarrAlternative = false;
                    break;

                case "f3_ab":
                case "f3_ba":
                    nombreFamiliaRebarShape = "M_00";
                    dimBarras = new DimensionesBarras(a: Util.CmToFoot(ConstNH.CONST_PATA_SX), b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: 0, b: Util.CmToFoot(30), c: LargoPathreiforment, d: Util.CmToFoot(30), e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    IsBarrAlternative = false;
                    break;
                case "f3_a0":
                case "f3_b0":

                    nombreFamiliaRebarShape = "M_00";
                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: 0, b: Util.CmToFoot(30), c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    IsBarrAlternative = false;
                    break;
                case "f3_esc135":// PATA LISA ESCALERA
                case "f3_esc45"://PATA LISA ESCALERE

                    distanciaBordeLosaHastaEscalera = Util.CmToFoot(20);
                    nombreFamiliaRebarShape = "M_00";
                    dimBarras = new DimensionesBarras(a: Util.CmToFoot(ConstNH.CONST_PATA_SX), b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPaTa, b: Util.CmToFoot(60), c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    IsBarrAlternative = false;
                    break;

                case "f3b_esc135":// PATA C HACIA ARRIBA ESCALERA
                case "f3b_esc45"://PATA C  HACIA ARRIBA ESCALERE
                    espesorEscalera = 18;
                    distanciaBordeLosaHastaEscalera = Util.CmToFoot(20);
                    dimBarras = new DimensionesBarras(a: 0, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    nombreFamiliaRebarShape = "M_00";
                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: Util.CmToFoot(10), b: Util.CmToFoot(espesorEscalera), c: Util.CmToFoot(60), d: LargoPathreiforment + distanciaBordeLosaHastaEscalera, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    IsBarrAlternative = false;
                    break;
                case "f3_0a":
                case "f3_0b":
                    nombreFamiliaRebarShape = "M_00";
                    dimBarras = new DimensionesBarras(a: 0, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: 0, b: 0, c: LargoPathreiforment, d: Util.CmToFoot(30), e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    IsBarrAlternative = false;
                    break;
                case "f3":
                    nombreFamiliaRebarShape = "M_00";
                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    IsBarrAlternative = false;
                    break;
                case "f9":
                case "f4":
                    nombreFamiliaRebarShape = "NH4_bajo";
                    //para obtener familia y modificar
                    //dimBarras = new DimensionesBarras(a: Util.CmToFoot(200), b: 0, c: 0, d: 0, e: Util.CmToFoot(100), caso: "RebarShapeNh");
                    //dimBarras = new DimensionesBarras(a: LargoPaTa+Util.MmToFoot(datosNuevaBarra.DiametroMM), b: 0, c: LargoPathreiforment, d: 0, e: LargoPaTa+ Util.MmToFoot(datosNuevaBarra.DiametroMM), caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    dimBarras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: LargoPaTa, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //para guardar datos interno
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: LargoPaTa, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);
                    IsBarrAlternative = false;
                    break;

                case "s3":
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F7B" : "NH_F7A";
                    dimBarras = new DimensionesBarras(a: largoPataInf, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: largoPataInf, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);
                    IsBarrAlternative = false;
                    break;
                case "f7":
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F7A" : "NH_F7B";
                    dimBarras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);
                    IsBarrAlternative = false;
                    break;
                case "s1":
                    nombreFamiliaRebarShape = "NH4_bajo";
                    dimBarras = new DimensionesBarras(a: largoPataInf, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: largoPataInf, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: largoPataInf, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: _EspesorLosa_EnFoot, e: largoPataInf, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);

                    IsBarrAlternative = false;
                    break;

                case "f10":
                case "f11":
                    nombreFamiliaRebarShape = "NH_F10";

                    dimBarras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: _EspesorLosa_EnFoot, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    dimBarras_parameterSharedLetras = new DimensionesBarras(_EspesorLosa_EnFoot, b: LargoPathreiforment, c: _EspesorLosa_EnFoot, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);
                    IsBarrAlternative = false;
                    break;


                case "f10a":
                    // nombreFamiliaRebarShape = "NH_F11";
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F11_v2" : "NH_F11";
                    //obtiene la el rebarshape F9A
                    dimBarras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);
                    IsBarrAlternative = false;
                    break;

                case "f11a":
                    // nombreFamiliaRebarShape = "NH_F11";
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F11_v2" : "NH_F11";
                    dimBarras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //obtiene la el rebarshape F9A
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);
                    IsBarrAlternative = false;
                    break;
                case "f16a":
                case "f16":
                    nombreFamiliaRebarShape = "M_00";
                    nombreFamiliaRebarShapeAlternativo = "M_00";
                    //para guardar datos internos
                    TiposRebarShape_largoAhorroRedondeo5.ObtenerLargoAhorro5(LargoPathreiforment, LargoAhorroIzq, LargoAhorroDere);

                    dimBarras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    dimBarrasAlternativa = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Dere, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, b: 0, c: 0, c2: TiposRebarShape_largoAhorroRedondeo5.largo1Dere, d: 0, e: 0, g: 0, largo2: TiposRebarShape_largoAhorroRedondeo5.largo1Dere, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    IsBarrAlternative = true;
                    break;
                case "f16_Izq":

                    f16_Izq_individual();
                    break;
                case "f16_Dere":
                    f16_Dere_individual();
                    break;
                case "f17A_Tras":
                case "f17B_Tras":
                case "f17b":
                case "f17a":
                case "f17":

                    if (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior)
                    {
                        nombreFamiliaRebarShape = "M_00";
                        nombreFamiliaRebarShapeAlternativo = "NH_F11";

                        dimBarras = new DimensionesBarras(a: LargoPathreiforment - LargoAhorroDere, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                        dimBarrasAlternativa = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment - LargoAhorroIzq, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    }
                    else
                    {
                        nombreFamiliaRebarShape = "NH_F11_v2";
                        nombreFamiliaRebarShapeAlternativo = "M_00";
                        dimBarras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment - LargoAhorroIzq, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                        dimBarrasAlternativa = new DimensionesBarras(a: LargoPathreiforment - LargoAhorroDere, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    }

                    //nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "M_00" : "NH_F11";
                    //nombreFamiliaRebarShapeAlternativo = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F11_v2" : "M_00";
                    ////para guardar datos internos
                    //dimBarras = new DimensionesBarras(a: _EspesorLosa_EnFoot,b:LargoPathreiforment - LargoAhorroIzq, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    //dimBarrasAlternativa = new DimensionesBarras(a: LargoPathreiforment - LargoAhorroDere, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment - LargoAhorroIzq, c: 0, c2: LargoPathreiforment - LargoAhorroDere, d: 0, e: 0, g: 0, largo2: LargoPathreiforment - LargoAhorroDere, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);
                    IsBarrAlternative = true;
                    break;
                case "f18":
                    nombreFamiliaRebarShape = "NH_F11";
                    nombreFamiliaRebarShapeAlternativo = "NH_F11_v2";

                    //para guardar datos internos
                    dimBarras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment - LargoAhorroIzq, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    dimBarrasAlternativa = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment - LargoAhorroDere, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: _EspesorLosa_EnFoot, b: LargoPathreiforment - LargoAhorroIzq, c: 0, c2: LargoPathreiforment - LargoAhorroDere, d: 0, e: 0, g: 0, largo2: LargoPathreiforment - LargoAhorroDere + _EspesorLosa_EnFoot, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);
                    IsBarrAlternative = true;
                    break;
                case "f19":
                    nombreFamiliaRebarShapeAlternativo = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1A";
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1B" : "NH_F1B";

                    TiposRebarShape_largoAhorroRedondeo1.ObtenerLargoAhorro1(LargoPathreiforment, LargoAhorroIzq, LargoAhorroDere);
                    //para guardar datos internos
                    dimBarras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo1.largo1Izq, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    dimBarrasAlternativa = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo1.largo1Dere, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    double aux_largo2 = LargoPaTa + _EspesorLosa_EnFoot + TiposRebarShape_largoAhorroRedondeo1.largo1Dere;
                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo1.largo1Izq, c2: TiposRebarShape_largoAhorroRedondeo1.largo1Dere, d: 0, e: 0, g: 0, largo2: aux_largo2, caso: "RebarShapeNh", LetrasCambiosEspesor: aux_Letracambiar);

                    IsBarrAlternative = true;
                    break;

                case "f20A_Dere_Tras":
                case "f20B_Izq_Tras":
                    nombreFamiliaRebarShape = "M_00";
                    nombreFamiliaRebarShapeAlternativo = "M_00";
                    //para guardar datos internos

                    dimBarras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    dimBarrasAlternativa = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    IsBarrAlternative = true;
                    break;
                case "f20A_Izq_Tras":
                case "f20B_Dere_Tras":
                case "f20a":
                case "f20b":
                case "f20":
                    nombreFamiliaRebarShape = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1B";
                    nombreFamiliaRebarShapeAlternativo = (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior) ? "NH_F1A" : "NH_F1B";
                    //para guardar datos internos
                    dimBarras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


                    TiposRebarShape_largoAhorroRedondeo1.ObtenerLargoAhorroDer1(LargoPathreiforment, LargoAhorroDere);
                    double largoAltenativa = LargoPathreiforment - LargoAhorroDere;// + LargoPaTa + _EspesorLosa_EnFoot;
                                                                                   //double LargoPataAlaternativa = ((LargoPaTa) / largoAltenativa) * LargoPathreiforment; //tiene q ser un  valor mas grande pq al tener un largo de base  menor y por porpocionalidad se dibuja mas pequeño

                    double auxLargo2 = TiposRebarShape_largoAhorroRedondeo1.largo1Dere + LargoPaTa + _EspesorLosa_EnFoot;
                    dimBarrasAlternativa = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo1.largo1Dere, c2: 0, d: 0, e: 0, g: 0, largo2: auxLargo2, caso: "RebarShapeNh", LetrasCambiosEspesor: "");

                    //para guardar datos internos            
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: LargoPathreiforment, c2: LargoPathreiforment - LargoAhorroDere, d: 0, e: 0, g: 0, largo2: auxLargo2, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");

                    IsBarrAlternative = true;
                    break;
                case "f19_Izq":
                case "f19_Dere":
                case "f21A_Izq_Tras":
                case "f21B_Dere_Tras":
                case "f21":
                    TiposRebarShape_largoAhorroRedondeo1.ObtenerLargoAhorroDer1(LargoPathreiforment, LargoAhorroDere);

                    if ((_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior))
                    {
                        nombreFamiliaRebarShape = "M_00";
                        nombreFamiliaRebarShapeAlternativo = "NH_F1A";
                    }
                    else
                    {
                        nombreFamiliaRebarShape = "M_00";
                        nombreFamiliaRebarShapeAlternativo = "NH_F1B";
                    }


                    dimBarras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo1.largo1Dere, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    dimBarrasAlternativa = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo1.largo1Dere, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");



                    //para guardar datos internos
                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPaTa, b: _EspesorLosa_EnFoot, c: TiposRebarShape_largoAhorroRedondeo1.largo1Dere, c2: TiposRebarShape_largoAhorroRedondeo1.largo1Dere, d: 0, e: 0, g: 0, largo2: TiposRebarShape_largoAhorroRedondeo1.largo1Dere, caso: "RebarShapeNh", LetrasCambiosEspesor: "B_");

                    IsBarrAlternative = true;
                    break;

                case "f21A_Dere_Tras":
                case "f21B_Izq_Tras":

                    f16_Izq_individual();
                    break;
                case "f22_Dere":
                case "f22_Izq": //obsoleto 03-05-2022
                case "f22A": //nuevo 03-05-2022
                case "f22B":// nuevo 03-05-2022
                case "f22":

                    TiposRebarShape_largoAhorroRedondeo1.ObtenerLargoAhorroIZq1(LargoPathreiforment, LargoAhorroIzq);

                    if (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior)
                    {
                        nombreFamiliaRebarShape = "M_00";
                        nombreFamiliaRebarShapeAlternativo = "M_00";
                        //para guardar datos internosnn

                        dimBarras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                        dimBarrasAlternativa = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo1.largo1Izq, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


                        dimBarras_parameterSharedLetras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo1.largo1Izq, b: 0, c: 0, c2: LargoPathreiforment, d: 0, e: 0, g: 0, largo2: LargoPathreiforment, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                        IsBarrAlternative = true;
                    }
                    else if (_ubicacionEnlosa == UbicacionLosa.Derecha || _ubicacionEnlosa == UbicacionLosa.Superior)
                    {
                        nombreFamiliaRebarShape = "M_00";
                        nombreFamiliaRebarShapeAlternativo = "M_00";
                        //para guardar datos internos

                        dimBarras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                        dimBarrasAlternativa = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo1.largo1Izq, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


                        dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, c2: TiposRebarShape_largoAhorroRedondeo1.largo1Izq, d: 0, e: 0, g: 0, largo2: TiposRebarShape_largoAhorroRedondeo1.largo1Izq, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                        IsBarrAlternative = true;
                    }
                    break;

                case "f22Tras":

                    nombreFamiliaRebarShape = "M_00";
                    nombreFamiliaRebarShapeAlternativo = "M_00";
                    //para guardar datos internos

                    dimBarras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    dimBarrasAlternativa = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


                    dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, c2: LargoPathreiforment, d: 0, e: 0, g: 0, largo2: LargoPathreiforment, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
                    IsBarrAlternative = true;
                    break;
                default:
                    break;
            }


            if (dimBarras != null)
            {
                dimBarras._LargoAhorro_Izq_ = LargoAhorroIzq;
                dimBarras._LargoAhorro_Dere_ = LargoAhorroDere;

            }

            if (dimBarrasAlternativa != null)
            {
                dimBarrasAlternativa._LargoAhorro_Izq_ = LargoAhorroIzq;
                dimBarrasAlternativa._LargoAhorro_Dere_ = LargoAhorroDere;
            }

            try
            {
                //busca la familia de rebarshape de barra principal
                tipoRebarShapePrincipal = ObtenerRebarSHape(nombreFamiliaRebarShape, dimBarras);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ObtenerRebarSHape {nombreFamiliaRebarShape}  ,  {_TipoBarra}:" + ex.Message);
            }
            //busca la familia de rebarshape de barra secundaria
            if (nombreFamiliaRebarShapeAlternativo != "") tipoRebarShapeAlternativa = ObtenerRebarSHape(nombreFamiliaRebarShapeAlternativo, dimBarrasAlternativa);
            return ObtenerdatosNuevaBarra();

        }

        private void CalcularLargosYDesplazamientos()
        {
            distanciaBordeLosaHastaEscalera = 0;
            desplazamientoPorLuzSecundaria = (IsLuzSecuandiria == true ? ConstNH.CONST_DESPLAZA_PORLUZSECUNDARIO_CM : 0);
            _EspesorLosa_EnFoot = Util.CmToFoot(_EspesorLosa_1 - ConstNH.RECUBRIMIENTO_LOSA_SUP_cm - ConstNH.RECUBRIMIENTO_LOSA_INF_cm - desplazamientoPorLuzSecundaria);
            //  double _descuentoEspewsoractual_foot = Util.CmToFoot(_diametroBarraDibujada / 10.0f);


            if (LargoAhoraDefinidoUsuario != 0)
            {
                LargoAhorroIzq = LargoAhoraDefinidoUsuario;
                LargoAhorroDere = LargoAhoraDefinidoUsuario;
            }
            else
            {
                LargoAhorroIzq = _LargoMin_1 * ConstNH.CONST_PORCENTAJE_LARGOPATA;//  + _EspesorMuro_Izq_abajo;
                LargoAhorroDere = _LargoMin_1 * ConstNH.CONST_PORCENTAJE_LARGOPATA;// + _EspesorMuro_Dere_Sup;

                if (RedonderLargoBarras.RedondearFoot1_mascercano(LargoAhorroIzq))
                {
                    LargoAhorroIzq = RedonderLargoBarras.NuevoLargobarraFoot;
                    LargoAhorroDere = RedonderLargoBarras.NuevoLargobarraFoot;
                }
            }
            // pata barras
            LargoPaTa = _LargoMin_1 * ConstNH.CONST_PORCENTAJE_LARGOPATA;
            if (RedonderLargoBarras.Redondear5_LargoPAta_L1(LargoPaTa, _EspesorLosa_EnFoot))
            {
                LargoPaTa = RedonderLargoBarras.largoPata_L1;
            }

            if (LargoPaTa < ConstNH.CONST_LARGOMIN_PATA)
                LargoPaTa = ConstNH.CONST_LARGOMIN_PATA;

        }

        private void f16_Dere_individual()
        {
            nombreFamiliaRebarShape = "M_00";
            nombreFamiliaRebarShapeAlternativo = "M_00";
            //para guardar datos internos
            TiposRebarShape_largoAhorroRedondeo5.ObtenerLargoAhorroIZq5(LargoPathreiforment, LargoAhorroIzq);

            dimBarras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
            dimBarrasAlternativa = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


            dimBarras_parameterSharedLetras = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, c2: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, d: 0, e: 0, g: 0, largo2: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
            IsBarrAlternative = true;
        }

        private void f16_Izq_individual()
        {
            nombreFamiliaRebarShape = "M_00";
            nombreFamiliaRebarShapeAlternativo = "M_00";
            //para guardar datos internos
            TiposRebarShape_largoAhorroRedondeo5.ObtenerLargoAhorroIZq5(LargoPathreiforment, LargoAhorroIzq);


            dimBarras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
            dimBarrasAlternativa = new DimensionesBarras(a: LargoPathreiforment, b: 0, c: 0, d: 0, e: 0, caso: "RebarShapeNh", LetrasCambiosEspesor: "");


            dimBarras_parameterSharedLetras = new DimensionesBarras(a: TiposRebarShape_largoAhorroRedondeo5.largo1Izq, b: 0, c: 0, c2: LargoPathreiforment, d: 0, e: 0, g: 0, largo2: LargoPathreiforment, caso: "RebarShapeNh", LetrasCambiosEspesor: "");
            IsBarrAlternative = true;
        }




        /// <summary>
        /// </summary>
        /// <param name="nombreFamiliaRebarShape"></param>
        /// <param name="largobaa"></param>
        /// <returns></returns>
        private RebarShape ObtenerRebarSHape(string nombreFamiliaRebarShape, DimensionesBarras dimBarras_)
        {
            RebarShape rebarshape = null;
            if (nombreFamiliaRebarShape == "M_00") // si NO nesista modificar rebarshape
            {
                rebarshape = TiposFormasRebarShape.getRebarShape(nombreFamiliaRebarShape, _doc);
            }
            else // Si nesista modificar rebarshape
            {
                Family fam = null;

                //Los buscar segum el nombre
                string nombreFamiliaRebarShape_modif = nombreFamiliaRebarShape + "_A" + dimBarras_.a.valorCM + "_B" + dimBarras_.b.valorCM + "_C" + dimBarras_.c.valorCM + "_D" + dimBarras_.d.valorCM + "_E" + dimBarras_.e.valorCM + versionPAth;

                rebarshape = TiposFormasRebarShape.getRebarShape(nombreFamiliaRebarShape_modif, _doc);

                if (rebarshape != null) return rebarshape;

                // si no enceuntra la barra especifica, busca la rebarshape de plantilla
                fam = TiposRebarShapeFamilia.M1_GetRebarShapeFamilia(nombreFamiliaRebarShape, _doc);
                // si lo encuentra el modifca los parametros
                if (fam != null && fam.IsValidObject)
                {

                    //double factor = (nombreFamiliaRebarShape == "NH4_bajo") ? 8.0f : 4.0f;
                    // int factor2 = (nombreFamiliaRebarShape == "NH4_bajo") ? 8 : 4;
                    //double factorRebarShape2= factor2 / largobaa;
                    //crea una copia de la la familianhs
                    double factorRebarShape = 1;// factor / largobaa;
                    string newNombreFamiliaRebarShape = ChangeFamilyRebar.SetDimensionRebarShape(fam, _doc, dimBarras_, nombreFamiliaRebarShape, factorRebarShape, true, versionPAth);
                    // rebarshape = ChangeFamilyRebar.SetDimensionRebarShape1(fam, doc, dimBarras_, nombreFamiliaRebarShape, factor / largobaa, true);
                    rebarshape = TiposFormasRebarShape.getRebarShape(newNombreFamiliaRebarShape, _doc);

                }

            }

            return rebarshape;
        }

        /// <summary>
        /// devuelve nuevo objeto con datos calculados en esta clase necesarios para crear las barras
        /// </summary>
        /// <returns></returns>
        private DatosNuevaBarraDTO ObtenerdatosNuevaBarra()
        {
            if (tipoRebarShapePrincipal == null)
            { }
            datosNuevaBarra.tipoRebarShapePrincipal = tipoRebarShapePrincipal;
            datosNuevaBarra.tipoRebarShapeAlternativa = tipoRebarShapeAlternativa;

            datosNuevaBarra.nombreFamiliaRebarShape = nombreFamiliaRebarShape;
            datosNuevaBarra.nombreFamiliaRebarShapeAlternativo = nombreFamiliaRebarShapeAlternativo;

            datosNuevaBarra.dimBarras_parameterSharedLetras = dimBarras_parameterSharedLetras;
            datosNuevaBarra.dimBarrasAlternativa = dimBarrasAlternativa;
            datosNuevaBarra.dimBarras = dimBarras;

            datosNuevaBarra.LargoPaTa_foot = LargoPaTa;
            datosNuevaBarra.LargoAhorroDere = LargoAhorroDere;
            datosNuevaBarra.LargoAhorroIzq = LargoAhorroIzq;
            datosNuevaBarra.LargoPataInf = largoPataInf;

            datosNuevaBarra.IsBarrAlternative = IsBarrAlternative;

            return datosNuevaBarra;
        }
    }
}
