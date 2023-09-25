using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.Elemento_Losa;
using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ahorro;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Fauto;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ocultar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.TagF;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Prueba.User;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom
{



    //[System.Runtime.InteropServices.Guid("FFF94424-FCC2-4A3E-BC2A-12FA727DA794")]
    public sealed class BarraRoom
    {
        #region 0)propiedades generales

        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian
        //Propiedades***********************************************************************************************************************************
        // porcentaje de largo minimo
        XYZ ptocero = XYZ.Zero;
        bool Ismensaje12Mt = true;
        public static int contador { get; set; }


        List<XYZ> ListaPtos = new List<XYZ>();
        // private ExternalCommandData commandData;
        private Document _doc;
        private Options opt;
        private UIDocument uidoc;
        private UIApplication _uiapp;
        private Application app;
        private View3D view3D;
        private View3D view3D_Visualizar;
        //   private string rutaRaiz = InfoSystema.ObtenerRutaDeFamilias();



        //private string rutaRaiz = @"F:\_revit\FAMILY\";
        // Room m_roomSelecionado = null;

        public BarraRoomGeometria barraLosaGeometria { get; set; }

        //level de losa
        public Level LevelLosa { get; set; }

        //direccion mayor de la losa
        public XYZ DireccionMayor { get; set; }

        // view en que se analiza la losa
        public View _view { get; set; }

        ///public string TipoBarra { get; set; }

        public IList<Curve> curvesPathreiforment;
        public IList<Curve> CurvesPathreiforment
        {
            get { return curvesPathreiforment; }
            set { curvesPathreiforment = value; }
        }

        public IList<Curve> CurvesPathreiforment_IzqInf { get; set; }
        public IList<Curve> CurvesPathreiforment_DereSup { get; set; }
        //  public double LargoPathreiforment { get; set; }

        private double largoPathreiforment;

        public double LargoPathreiforment
        {
            get { return largoPathreiforment; }
            set
            {
                largoPathreiforment = value;

                if (largoPathreiforment > Util.CmToFoot(1200) && Ismensaje12Mt)
                { Util.InfoMsg($"Barra Mayor a 12mt   LArgo: {Math.Round(Util.FootToCm(largoPathreiforment) / 100, 1)} mt"); }

            }
        }

        public double LargoRecorrido { get; set; }

        //se utiliza para modificar la familas de rebarshape previo a agergarlos a los pathrteinforment
        public DimensionesBarras dimBarras { get; set; }
        public DimensionesBarras dimBarrasAlternativa { get; set; }

        // se utiliza para guardar los valores en los parametrso compartido del rebar shape ----  A,B,C,D,E
        public DimensionesBarras dimBarras_parameterSharedLetras { get; set; }
        // tipo de barra -- PathRefuerza o AreaRefuerza
        public TipoRefuerzo Tiporefuerzo { get; set; }

        public TipoDireccionBarra TipoDireccionBarra_ { get; set; } = TipoDireccionBarra.NONE;
        //orientacion de barra  vertical u orizontal
        // public TipoOrientacionBarra TipoOrientacion { get; set; }
        // cuantia de la barra a dibuja
        public string CuantiaB { get; set; }


        #endregion

        #region Propiedades  BarraLosaGeometria

        public UbicacionLosa ubicacionEnlosa { get; set; } = UbicacionLosa.Derecha;


        RebarBarType rebarBarType = null;


        public PathReinforcement m_createdPathReinforcement { get; set; }
        public PathReinforcement m_createdPathReinforcement_dere { get; set; }
        public PathReinforcement m_createdPathReinforcement_izq { get; set; }

        public PathReinSpanSymbol _PathReinSpanSymbol { get; set; }


        public string nombreSimboloPathReinforcement = "";
        public AreaReinforcement m_createdAreaReinforcement = null;
        public string nombreSimboloAreaReinforcement = "";
        //public IList<Curve> curvesPathreiforment { get; set; }
        public string TipoBarraStr { get; set; }
        public TipoOrientacionBarra TipoOrientacion { get; set; }


        public Room _roomSelecionado1;


        public Room RoomSelecionado_1
        {
            get { return _roomSelecionado1; }
            set { _roomSelecionado1 = value; }
        }
        public Room _roomSelecionado2;
        public Room RoomSelecionado_2
        {
            get { return _roomSelecionado2; }
            set { _roomSelecionado2 = value; }
        }



        //angulo de pelota de losa
        public double Angle_pelotaLosa1Grado { get; set; }
        //angulo de pelota de losa
        public double Angle_pelotaLosa2 { get; set; }

        //Espesor de losa
        public double EspesorLosaCm_1 { get; set; }
        public double EspesorLosa_2 { get; set; }

        //Espesor de losa
        public double EspesorMuro_Izq_abajo { get; set; }
        public double EspesorMuro_Dere_Sup { get; set; }



        public double largoMin_1;
        public double LargoMin_1
        {
            get { return largoMin_1; }
            set { largoMin_1 = value; }
        }
        public double largoMin_2;
        public double LargoMin_2
        {
            get { return largoMin_2; }
            set { largoMin_2 = value; }
        }
        #region Hoook

        //HOOK barra Principal
        // star es el que tiene x menor   
        // si es vertical start y menor
        public RebarHookType tipodeHookStartPrincipal { get; set; }
        public RebarHookType tipodeHookEndPrincipal { get; set; }

        //Rebar shape Principal y secundaria
        public RebarShape tipoRebarShapePrincipal { get; set; }
        public RebarShape tipoRebarShapeAlternativa { get; set; }


        // HOOK barra alternativa
        // star es el que tiene x menor   
        // si es vertical start y menor
        public RebarHookType tipodeHookStarAlternativa { get; set; }
        public RebarHookType tipodeHookEndAlternativa { get; set; }
        public bool IsBarrAlternative { get; set; }
        #endregion


        //lista con los puntos poligono de los segmentos intersecatados , 4 ptos, inical y final de los dos segmentos
        // [ ptoini1, ptofin1 , ptoini2 , ptofin2]
        //  1 __ 1    sentido horizontal          1 __ 2    sentido vertical
        //  2    2                                1 __ 2 
        public List<XYZ> ListaPtosPoligonoLosa { get; set; }

        //lista con los puntos que circunscribe el area que ocupara la losa
        //0  - 3
        //1  - 2
        public List<XYZ> ListaPtosPerimetroBarras { get; set; }
        public SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom { get; set; }
        public int DiametroOrientacionPrincipal_mm { get; set; } = 0;

        //floor que pertenece las barras
        public Floor LosaSeleccionada1 { get; set; }
        //floor que pertenece las barras
        public Floor SelecFloor_2 { get; set; }

        public string rutaImagenPricipal { get; set; }
        public string rutaImagenAlternativa { get; set; }
        public Dictionary<string, string> listaImagenes = new Dictionary<string, string>(20);


        public Dictionary<string, string> listaRutasFamilias = new Dictionary<string, string>(20);


        public Result statusbarra { get; set; }
        public string message { get; set; }

        #endregion

        // XYZ ptoSinDesface_ = XYZ.Zero;
        public bool IsLuzSecuandiria { get; set; }
        public int diametroEnMM { get; set; }
        public double Espaciamiento { get; set; }
        public string tipoPrefijo;

        /// <summary>
        /// largo del recorrido de la barra
        /// esto es opcional, pq se utilza solo cuando se genera enfierra barra inferior automatio
        /// F:\_revit\REVITArmaduraLosaRevit\ArmaduraLosaRevit\ArmaduraLosaRevit.Model\observaciones.docx
        /// Observacion 1)
        /// </summary>
        public double LargoRecorridoX { get; set; }
        public double LargoRecorridoY { get; set; }


        //  public BarraRoomDatos barraRoomDatos { get; set; }
        public string TipoBarra_izq_Inf { get; set; }
        public string TipoBarra_dere_sup { get; set; }
        public XYZ ptoConMouseEnlosaF1_SUPIzqINf { get; private set; }
        public XYZ ptoConMouseEnlosaF1_SUPDereSup { get; private set; }
        public XYZ ptoIZqInf_Dimension { get; internal set; }
        public XYZ ptoDereSUp_Dimension { get; internal set; }
        public XYZ ptoDereSup_Dimension { get; private set; }
        public object Enumeracion { get; private set; }
        public double AnguloBordeRoomYSegundoPtoMouseGrado { get; private set; }

        // BarraRoomPersis barraRoomPersis;

        private XYZ ptoConMouseEnlosa1;
        private XYZ ptoConMouseEnlosa2;
        //private CargarFAmilias_carga cargarFAmilias_carga;

        private DatosNuevaBarraDTO _datosNuevaBarraDTO;
        private bool IsTest;
        private SolicitudBarraDTO _solicitudBarraDTO;



        public ReferenciaRoomDatos _refereciaRoomDatos { get; set; }
        //private PathReinformeTraslapoDatos datosNuevoPath1;
#pragma warning disable CS0169 // The field 'BarraRoom._utilfallas' is never used
        private UtilitarioFallasAdvertencias _utilfallas;
#pragma warning restore CS0169 // The field 'BarraRoom._utilfallas' is never used
        private IGeometriaTag _listaTAgBArra;
        private bool IsCasoS4;
        private TipoRebar _BarraTipo = TipoRebar.NONE;
        private string _NombreVista;
        private Element elemtoSymboloPath;

        public BarraRoom newBarralosa_izq { get; set; }
        public BarraRoom newBarralosa_dere { get; set; }

        // verifca si todo esta ok
        bool IsOK;
        private TiposPathReinformentSymbol ptSymbol;

        //cosntructor************************************************************************************************************************
        #region 1)constructor



        //1-CONTRUCTOR ORIGINAL -- diseñar con mouse
        public BarraRoom(UIApplication uiapp, string tipoBarra, UbicacionLosa ubicacionEnlosa_, DatosDiseñoDto _datosDiseñoDto,
                            bool IsBuscarTipoBarra = false, XYZ ptoCOnMOuse1 = null, XYZ ptoCOnMOuse2 = null, Floor floor = null, bool IsTest = false)
        {

            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView) || (!Directory.Exists(ConstNH.CONST_COT)))
            {
                statusbarra = Result.Failed;
                return;
            }
            this._uiapp = uiapp;
            this.ubicacionEnlosa = ubicacionEnlosa_;
            if (tipoBarra == "s4")
            {
                IsCasoS4 = true;
                tipoBarra = "f1_SUP";
            }
            else IsCasoS4 = false;

            bool IsConAhorro = false;
            bool IsConF1_SUP = false;
            if (tipoBarra == "fautoahorro")
            {
                tipoBarra = "fauto";
                IsConAhorro = true;
                IsConF1_SUP = false;
                DatosDiseño.DISENO_TIPO_F1 = TipoDiseño_F1.f1_conAhorro;
            }
            else if (tipoBarra == "fautoahorrof1_sup")
            {
                tipoBarra = "fauto";
                IsConAhorro = true;
                IsConF1_SUP = VariablesSistemas.IsDibujarS4; //deberia ser true
                DatosDiseño.DISENO_TIPO_F1 = TipoDiseño_F1.f1_sup_conAhorro;
            }
            else if (tipoBarra == "fautof1_sup")
            {
                tipoBarra = "fauto";
                IsConAhorro = false;
                IsConF1_SUP = VariablesSistemas.IsDibujarS4;//deberia ser true
                DatosDiseño.DISENO_TIPO_F1 = TipoDiseño_F1.f1_sup;
            }
            else
            {
                IsConAhorro = false;
                if (tipoBarra == "f1_SUP")
                {
                    IsConF1_SUP = true;
                    DatosDiseño.DISENO_TIPO_F1 = TipoDiseño_F1.f1_sup;
                }
                else
                {
                    IsConF1_SUP = false;
                    DatosDiseño.DISENO_TIPO_F1 = TipoDiseño_F1.f1;
                }
            }



            this.IsTest = IsTest;
            _solicitudBarraDTO = new SolicitudBarraDTO(_uiapp, tipoBarra, ubicacionEnlosa, TipoConfiguracionBarra.refuerzoInferior, IsBuscarTipoBarra);
            _solicitudBarraDTO.IsCasoS4 = IsCasoS4;
            _solicitudBarraDTO.Ui_pathSymbolDTO_ = _datosDiseñoDto.Ui_pathSymbolDTO_;
            contador += 1;

            IsOK = true;
            this.TipoBarraStr = (tipoBarra == "f1_SUP" || tipoBarra == "fauto" ? "f3" : tipoBarra);

            // solo se utiliza en la funcion 'pruebar_fx_sx_'
            IsBuscarTipoBarra = (tipoBarra == "fauto" ? true : IsBuscarTipoBarra);
            BarraRoomGeometria.IsBuscarTipoBarra = IsBuscarTipoBarra;
            BarraRoomGeometria.IsLosaINclinada = false;


            ConfiguracionAhorro configuracionAhorro = FactoryAhorro.ObtenerAhorro(IsConAhorro, IsConF1_SUP);

            if (!CargarDatosIniciales(_uiapp, ubicacionEnlosa)) return;

            if (IsTest)
                _view = SeleccionarView.ObtenerViewPOrNombre(_uiapp.ActiveUIDocument, "PLANTA ESTRUCTURA CIELO 3° PISO");


            AyudaAsignarTipoBarra _newAyudaAsignarTipoBarra = new AyudaAsignarTipoBarra();
            //carganhs
            if (this.TipoBarraStr == "s1" || this.TipoBarraStr == "s2" || this.TipoBarraStr == "s3")
            {
                tipoPrefijo = "F'=";
                statusbarra = Load_s1_s3V2(_uiapp, ptoCOnMOuse1, ptoCOnMOuse2, floor, IsTest);
                if (!(statusbarra == Result.Succeeded)) return;


                // _solicitudBarraDTO.TipoBarra = ubicacionEnlosa.ToString();
                _solicitudBarraDTO.UbicacionEnlosa = ubicacionEnlosa;
                _solicitudBarraDTO.ObtenerOrientacion();
                ptoConMouseEnlosa1 = AyudaSuples.ObtenerPtoPathSymbolSx(ListaPtosPerimetroBarras);
                _seleccionarLosaBarraRoom.PtoConMouseEnlosa1 = ptoConMouseEnlosa1;

                if (_datosDiseñoDto.IsConsiderarDatosIniciales)
                {
                    diametroEnMM = _datosDiseñoDto.DiamnetroMM;
                    Espaciamiento = Util.CmToFoot(_datosDiseñoDto.espaciamientoCm);
                }
                if (_newAyudaAsignarTipoBarra.AsignarBarraSuperior(TipoBarraStr))
                    _BarraTipo = _newAyudaAsignarTipoBarra._tipoRebar;
                else
                    _BarraTipo = TipoRebar.LOSA_SUP;
            }
            else
            {
                statusbarra = Load_fx(ptoCOnMOuse1, floor, IsTest);
                _BarraTipo = TipoRebar.LOSA_INF;
                tipoPrefijo = "F=";
                if (_datosDiseñoDto.IsConsiderarDatosIniciales && tipoBarra == "f1_SUP")
                {
                    diametroEnMM = _datosDiseñoDto.DiamnetroMM;
                    Espaciamiento = Util.CmToFoot(_datosDiseñoDto.espaciamientoCm);
                    _BarraTipo = TipoRebar.LOSA_SUP_S4;
                    tipoPrefijo = "F'=";
                }

            }

            if (!(statusbarra == Result.Succeeded)) return;


            _solicitudBarraDTO.TipoBarra = TipoBarraStr;

            if (IsConAhorro)
                _solicitudBarraDTO = configuracionAhorro.VerificarDiseñoConAHorro(_solicitudBarraDTO, ListaPtosPerimetroBarras);

            //
            if (Result.Failed == statusbarra) return;
            try
            {
                // arrregar
                TipoBarraStr = _solicitudBarraDTO.TipoBarra;// this.TipoBarraStr;
                //obtener largo path
                DefinirLargoPathreiforment();
            }
            catch (Exception ex0)
            {
                TaskDialog.Show("Error", "Error al crear barra:1" + ex0.Message);
            }

            try
            {
                //asigan los  CurvesPathreiforment , en el caso que sean mas de 3
                RedefinirCurvesPathreiforment();
            }
            catch (Exception ex0)
            {
                TaskDialog.Show("Error", "Error al crear barra:2" + ex0.Message);
            }

            if (Espaciamiento > Util.CmToFoot(40))
            {
                Util.ErrorMsg($"Espaciamiento barras mayor de 40 cm--> esp:{Util.FootToCm(Espaciamiento).ToString("N0")}");
                IsOK = false;
                statusbarra = Result.Failed;
                return;
            }

            try
            {
                //objetos con los datos para dibujar barra
                _datosNuevaBarraDTO = new DatosNuevaBarraDTO()
                {
                    LargoPathreiforment = LargoPathreiforment,
                    LargoRecorridoFoot = LargoRecorrido,
                    DiametroMM = diametroEnMM,
                    LargoMininoLosa = LargoMin_1,
                    IsLuzSecuandiria = IsLuzSecuandiria,
                    EspaciamientoFoot = Espaciamiento,
                    EspesorLosaCm_1 = EspesorLosaCm_1,
                    Prefijo_cuantia = tipoPrefijo,
                    DiametroOrientacionPrincipal_mm = DiametroOrientacionPrincipal_mm,
                    PathSymbol_REbarshape_FXXDTO_ = _datosDiseñoDto.PathSymbol_REbarshape_FXXDTO_
                };
            }
            catch (Exception ex0)
            {
                TaskDialog.Show("Error", "Error al crear barra:4" + ex0.Message);
                statusbarra = Result.Failed;
                return;
            }

            try
            {
                _listaTAgBArra = FactoryGeomTag.CrearGeometriaTag(_doc, _seleccionarLosaBarraRoom, _solicitudBarraDTO, ListaPtosPerimetroBarras);
                _listaTAgBArra.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(Angle_pelotaLosa1Grado) });
            }
            catch (Exception ex0)
            {

                TaskDialog.Show("Error", "Error al crear barra:3" + ex0.Message);
                statusbarra = Result.Failed;
                return;
            }


            try
            {
                //no se utiliza los ganchosRE
                //obtener hook
                ITipoHook ObtenerTipoHook = new TipoHook(_solicitudBarraDTO);
                ObtenerTipoHook.DefinirHook();
            }
            catch (Exception ex0)
            {
                TaskDialog.Show("Error", "Error al crear barra:5" + ex0.Message);
                statusbarra = Result.Failed;
                return;
            }

            if (!ObtenerRebarshape())
            {
                Util.ErrorMsg($"No se encuentra RebarShape tipo {TipoBarraStr} ");
                statusbarra = Result.Failed;
                return;
            }

            if (!ObtenerPathSymbol())
            {
                Util.ErrorMsg($"No se encuentra PathSymbol tipo {TipoBarraStr} ");
                statusbarra = Result.Failed;
                return;
            }

            if (IsOK == false)
            {
                statusbarra = Result.Failed;
                return;
            }
#if DEBUG
            // LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif
        }

        private bool ObtenerPathSymbol(bool IsInvertir = true)
        {
            //********************************
            try
            {
               //generar psthsymol nomral e invertido
                if (_solicitudBarraDTO.TipoBarra == "f22a" && IsInvertir)
                {
                    _solicitudBarraDTO.TipoBarra = "f22aInv";
                    ObtenerPathSymbol(false);
                    _solicitudBarraDTO.TipoBarra = "f22a";
                }
                else if (_solicitudBarraDTO.TipoBarra == "f22aInv" && IsInvertir)
                {
                    _solicitudBarraDTO.TipoBarra = "f22a";
                    ObtenerPathSymbol(false);
                    _solicitudBarraDTO.TipoBarra = "f22aInv";
                }
                else if (_solicitudBarraDTO.TipoBarra == "f22b" && IsInvertir)
                {
                    _solicitudBarraDTO.TipoBarra = "f22bInv";
                    ObtenerPathSymbol(false);
                    _solicitudBarraDTO.TipoBarra = "f22b";
                }
                else if (_solicitudBarraDTO.TipoBarra == "f22bInv" && IsInvertir)
                {
                    _solicitudBarraDTO.TipoBarra = "f22b";
                    ObtenerPathSymbol(false);
                    _solicitudBarraDTO.TipoBarra = "f22bInv";
                }

                ptSymbol = new TiposPathReinformentSymbol(_solicitudBarraDTO, _seleccionarLosaBarraRoom, Angle_pelotaLosa1Grado);
                _datosNuevaBarraDTO.nombreSimboloPathReinforcement = ptSymbol.M1_DefinirPathReinformentSymbol();

                if (ObtenerCasoAoBDeAhorro.IScaso_16_17_18_19_20_20Inv_21_22_AoB(_solicitudBarraDTO.TipoBarra))
                {
                    IPathSymbol _IPathSymbolcs = FactoryPathSymbol.ObtenerPathSymbol(_uiapp, _solicitudBarraDTO, _datosNuevaBarraDTO);
                    if (_IPathSymbolcs.M1_ObtenerPArametros())
                    {
                        if (_IPathSymbolcs.M2_ejecutar())
                            elemtoSymboloPath = _IPathSymbolcs.elemtoSymboloPath;
                        else
                        {
                            IsOK = false;
                            return false;
                        }
                    }
                }

            }
            catch (Exception ex0)
            {
                TaskDialog.Show("Error", "Error al crear barra:7" + ex0.Message);
                statusbarra = Result.Failed;
                return true;
            }

            return true;
        }

        private bool ObtenerRebarshape()
        {
            //********************************
            try
            {
                if (ObtenerCasoAoBDeAhorro.IScaso_16_17_18_19_20_20Inv_21_22_AoB(_solicitudBarraDTO.TipoBarra))
                {
                    IRebarShapeNh PathRebarShape = FactoryRebarShapeNh.ObtenerRebarShapeNh(_uiapp, _solicitudBarraDTO, _datosNuevaBarraDTO);
                    if (!PathRebarShape.M0_Ejecutar())
                    {
                        IsOK = false;
                        return false;
                    }

                    _datosNuevaBarraDTO = PathRebarShape.DatosNuevaBarraDTO_;
                }
                else
                {
                    ITiposRebarShape PathRebarShape = new TiposRebarShape(_solicitudBarraDTO, _datosNuevaBarraDTO, EspesorLosaCm_1);
                    _datosNuevaBarraDTO = PathRebarShape.DefinirRebarShape();

                }
            }
            catch (Exception ex0)
            {
                TaskDialog.Show("Error", $"Error al crear barra:6   -->  {_solicitudBarraDTO.TipoBarra}" + ex0.Message);
                statusbarra = Result.Failed;
                return false;
            }
            return true;
        }

        //1.1-CONTRUCTOR ORIGINAL -- diseñar con mouse --> para rebar
        public BarraRoom(UIApplication uiapp, string tipoBarra, UbicacionLosa ubicacionEnlosa_)
        {

            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView) || (!Directory.Exists(ConstNH.CONST_COT)))
            {
                statusbarra = Result.Failed;
                return;
            }


            _solicitudBarraDTO = new SolicitudBarraDTO(uiapp, tipoBarra, ubicacionEnlosa_, TipoConfiguracionBarra.refuerzoInferior, false);

            contador += 1;
            this._uiapp = uiapp;
            this.ubicacionEnlosa = ubicacionEnlosa_;
            this.TipoBarraStr = (tipoBarra == "f1_SUP" || tipoBarra == "fauto" ? "f3" : tipoBarra);

            // solo se utiliza en la funcion 'pruebar_fx_sx_'

            BarraRoomGeometria.IsBuscarTipoBarra = false;

            if (!CargarDatosIniciales(uiapp, ubicacionEnlosa_)) return;


            BarraRoomGeometria.IsLosaINclinada = true;
            //  statusbarra = Load_fx(pointMouse: null, floor: null, IsTest = false);


        }

        public void AsignarTipoFaceBusqueda(Func<XYZ, bool> tipofaceBusqueda)
        {
            _solicitudBarraDTO.tipofaceBusqueda = tipofaceBusqueda;
        }


        public RebarInferiorDTO ObtenerGEometria()
        {
            RebarInferiorDTO rebarInferiorDTO = new RebarInferiorDTO(_uiapp);
            try
            {


                rebarInferiorDTO.listaPtosPerimetroBarras.AddRange(ListaPtosPerimetroBarras);

                var resultZ = _view.Obtener_Z_SoloPLantas();
                if (!resultZ.Isok)
                {
                    Util.ErrorMsg($"Error en el espesor de Room  e:{rebarInferiorDTO.espesorLosaFoot}");
                    rebarInferiorDTO.IsOK = false;
                    return rebarInferiorDTO;
                }
                rebarInferiorDTO.barraIni = Line.CreateBound(ListaPtosPerimetroBarras[0].GetXY0(), ListaPtosPerimetroBarras[1].GetXY0())
                                                .Project(_seleccionarLosaBarraRoom.PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);

                rebarInferiorDTO.barraFin = Line.CreateBound(ListaPtosPerimetroBarras[3].GetXY0(), ListaPtosPerimetroBarras[2].GetXY0())
                                                .Project(_seleccionarLosaBarraRoom.PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);

                rebarInferiorDTO.PtoDirectriz1 = Line.CreateBound(ListaPtosPerimetroBarras[1].GetXY0(), ListaPtosPerimetroBarras[2].GetXY0())
                                                     .Project(_seleccionarLosaBarraRoom.PtoConMouseEnlosa1).XYZPoint.AsignarZ(resultZ.valorz);
                rebarInferiorDTO.PtoDirectriz2 = Line.CreateBound(ListaPtosPerimetroBarras[0].GetXY0(), ListaPtosPerimetroBarras[3].GetXY0())
                                                     .Project(_seleccionarLosaBarraRoom.PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);


                // ConstantesGenerales.sbLog.AppendLine($"DatosnNIciales   BarraRoom--------------------------------------------------------------------");
                //ConstantesGenerales.sbLog.AppendLine($" barraIni={rebarInferiorDTO.barraIni}       barraFin ={rebarInferiorDTO.barraIni}    PtoDirectriz1={rebarInferiorDTO.PtoDirectriz1}  PtoDirectriz2={rebarInferiorDTO.PtoDirectriz2}");
                rebarInferiorDTO.espesorLosaFoot = Util.CmToFoot(_refereciaRoomDatos.espesorCM_1);// Util.CmToFoot(15)

                if (rebarInferiorDTO.espesorLosaFoot <= 0)
                {
                    Util.ErrorMsg($"Error en el espesor de Room  e:{rebarInferiorDTO.espesorLosaFoot}");
                    rebarInferiorDTO.IsOK = false;
                    return rebarInferiorDTO;

                }

                //rebarInferiorDTO.numeroBarra = 20;
                rebarInferiorDTO.diametroMM = _refereciaRoomDatos.diametro;
                rebarInferiorDTO.tipoBarra = EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, TipoBarraStr);
                rebarInferiorDTO.ubicacionLosa = ubicacionEnlosa;
                rebarInferiorDTO.espaciamientoFoot = _refereciaRoomDatos.Espaciamiento;// Util.CmToFoot(15);
                rebarInferiorDTO.largo_recorridoFoot = ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]);// Util.CmToFoot(660);
                rebarInferiorDTO.floor = _seleccionarLosaBarraRoom.LosaSeleccionada1;//  _uiapp.ActiveUIDocument.Document.GetElement(new ElementId(1111503));

                rebarInferiorDTO.largomin_1 = _refereciaRoomDatos.largomin_1;

                //rebarInferiorDTO.espesorBarraFooT = rebarInferiorDTO.espesorLosaFoot 
                //                                    - Util.CmToFoot(ConstantesGenerales.RECUBRIMIENTO_LOSA_INF + ConstantesGenerales.RECUBRIMIENTO_LOSA_SUP);
                rebarInferiorDTO.ptoSeleccionMouse = _seleccionarLosaBarraRoom.PtoConMouseEnlosa1.AsignarZ(_view.GenLevel.ProjectElevation);

                int AcortamientoEspesorSecundario = (IsLuzSecuandiria == true ? 1 : 0);
                rebarInferiorDTO.AcortamientoEspesorSecundario = AcortamientoEspesorSecundario;
                rebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT = rebarInferiorDTO.espesorLosaFoot + Util.CmToFoot(-ConstNH.RECUBRIMIENTO_LOSA_SUP_cm - ConstNH.RECUBRIMIENTO_LOSA_INF_cm - AcortamientoEspesorSecundario);

                rebarInferiorDTO.anguloBarraGrados = Util.AnguloEntre2PtosGrados_enPlanoXY(rebarInferiorDTO.barraIni, rebarInferiorDTO.barraFin);
                rebarInferiorDTO.anguloBarraRad = Util.GradosToRadianes(rebarInferiorDTO.anguloBarraGrados);
                rebarInferiorDTO.TipoDireccionBarra_ = TipoDireccionBarra_;
                rebarInferiorDTO.anguloTramoRad = Util.GradosToRadianes(31.5);
                rebarInferiorDTO.LargoPata = Util.CmToFoot(100);
                rebarInferiorDTO.IsOK = true;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerGEometria' ex.{ex.Message}");
                rebarInferiorDTO.IsOK = false;
                return rebarInferiorDTO;
            }
            return rebarInferiorDTO;
        }


        //2.1) CONSTRUCTO para barra alternativa 2 y 3 superior --- > CASO f1_SUP  
        public BarraRoom(UIApplication uiapp, BarraRoomDTO barraRoomDto,
                                List<XYZ> ListaPtosPerimetroBarrasPathBorde, IList<Curve> CurvesPathreiforment,
                                XYZ ptomouse)
        {
            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView) || (!Directory.Exists(ConstNH.CONST_COT)))
            {
                statusbarra = Result.Failed;
                return;
            }


            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR BarraRoom CASO f1_SUP ");

            if (!resultadoConexion)
            {
                statusbarra = Result.Failed;
                Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                return;
            }
            else if (!_ManejadorUsuarios.ObteneResultado())
            {
                statusbarra = Result.Failed;
                Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                return;
            }


            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null)
            {
                statusbarra = Result.Failed;
                return;
            }

            contador += 1;
            this.ubicacionEnlosa = barraRoomDto.ubicacionEnlosa;
            this._uiapp = uiapp;
            BarraRoomGeometria.IsBuscarTipoBarra = false;
            this.TipoBarraStr = barraRoomDto.TipoBarraStr;
            _solicitudBarraDTO = new SolicitudBarraDTO(uiapp, TipoBarraStr, this.ubicacionEnlosa, TipoConfiguracionBarra.refuerzoInferior, false);

            ptoConMouseEnlosa1 = ptomouse;
            _BarraTipo = barraRoomDto._TipoRebar;
            if (!CargarDatosIniciales(uiapp, this.ubicacionEnlosa)) return;


            //IViewRangleClase viewRangleClase = ViewRangleClase.CreatorNuevoViewRange(doc, view);
            //viewRangleClase.EditarParametroViewRange(ConstantesGenerales.VIEWRANGE_TOP, ConstantesGenerales.VIEWRANGE_CORTE, ConstantesGenerales.VIEWRANGE_BOTTON, ConstantesGenerales.VIEWRANGE_DEPTH);
            _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(uidoc, _solicitudBarraDTO);
            _seleccionarLosaBarraRoom.LosaSeleccionada1 = barraRoomDto.LosaSeleccionada1;
            //carga
            ListaPtosPerimetroBarras = ListaPtosPerimetroBarrasPathBorde;

            this.LargoPathreiforment = ListaPtosPerimetroBarras[1].DistanceTo(ListaPtosPerimetroBarras[2]);
            this.LargoRecorrido = ListaPtosPerimetroBarras[3].DistanceTo(ListaPtosPerimetroBarras[2]);
            this.LargoMin_1 = this.LargoPathreiforment;
            // floor asignado 
            this.diametroEnMM = barraRoomDto.diametroEnMM;
            this.LosaSeleccionada1 = barraRoomDto.LosaSeleccionada1;
            this.EspesorLosaCm_1 = barraRoomDto.EspesorLosaCm_1;
            this.CurvesPathreiforment = CurvesPathreiforment;
            this.Espaciamiento = barraRoomDto.Espaciamiento;
            this.AnguloBordeRoomYSegundoPtoMouseGrado = barraRoomDto.AnguloBordeRoomYSegundoPtoMouseGrado;
            // if (SALIRAS) return;
            //no se utiliza los ganchosRE

            Angle_pelotaLosa1Grado = barraRoomDto.Angle_pelotaLosa1Grado;

            //objetos con los datos para dibujar barra
            _datosNuevaBarraDTO = new DatosNuevaBarraDTO()
            {
                LargoPathreiforment = LargoPathreiforment,
                LargoRecorridoFoot = LargoRecorrido,
                LargoMininoLosa = LargoMin_1,
                IsLuzSecuandiria = IsLuzSecuandiria,
                EspaciamientoFoot = Espaciamiento,
                DiametroOrientacionPrincipal_mm = barraRoomDto.diametroEnMM,
                Prefijo_cuantia = barraRoomDto.Prefijo_cuantia,
            };

            //definir tag
            _listaTAgBArra = FactoryGeomTag.CrearGeometriaTag(_doc, ptomouse, _solicitudBarraDTO, ListaPtosPerimetroBarras);
            _listaTAgBArra.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(Angle_pelotaLosa1Grado) });

            //obtener hook
            ITipoHook ObtenerTipoHook = new TipoHook(_solicitudBarraDTO);
            ObtenerTipoHook.DefinirHook();

            //ITiposRebarShape PathRebarShape = new TiposRebarShape(_solicitudBarraDTO, _datosNuevaBarraDTO, EspesorLosaCm_1);
            //_datosNuevaBarraDTO = PathRebarShape.DefinirRebarShape();
            if (!ObtenerRebarshape()) return;

            //ITiposPathReinformentSymbol ptSymbol = new TiposPathReinformentSymbol(_solicitudBarraDTO, barraRoomDto.AnguloBordeRoomYSegundoPtoMouseGrado, Angle_pelotaLosa1Grado);
            //_datosNuevaBarraDTO.nombreSimboloPathReinforcement = ptSymbol.M1_DefinirPathReinformentSymbol();
            if (!ObtenerPathSymbol()) return;

            statusbarra = Result.Succeeded;
            // DefinirLargoPathreiforment();

        }


        private List<XYZ> Obtener4PtosPathRein_F1_Sup(List<XYZ> listaPath)
        {
            //curve.of
            // throw new NotImplementedException();
            return listaPath;
        }


        // 3)cosntructor para traslapo
        public BarraRoom(UIApplication uiapp, ContenedorDatosPathReinformeDTO DatosNuevoPathDTO)
        {
            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView) || (!Directory.Exists(ConstNH.CONST_COT)))
            {
                statusbarra = Result.Failed;
                return;
            }

            //  this.commandData = commandData;

            ContenedorDatosLosaDTO _datosLosaDTO = DatosNuevoPathDTO.datosLosaDTO;
            contador += 1;
            this.ubicacionEnlosa = _datosLosaDTO.ubicacionLosa;
            this._uiapp = uiapp;
            BarraRoomGeometria.IsBuscarTipoBarra = false;
            this.TipoBarraStr = _datosLosaDTO.tipoBarra.ToString();
            if (TipoBarraStr == "s4")
            {
                IsCasoS4 = true;
                TipoBarraStr = "f1_SUP";
            }
            if (_datosLosaDTO.TipoDireccionBarraVocal != "")
                this.TipoDireccionBarra_ = (_datosLosaDTO.TipoDireccionBarraVocal.ToLower() == ConstNH.NOMBRE_BARRA_PRINCIPAL ? TipoDireccionBarra.Primaria : TipoDireccionBarra.Secundario);

            IsLuzSecuandiria = (TipoDireccionBarra_ == TipoDireccionBarra.Secundario);
            _solicitudBarraDTO = new SolicitudBarraDTO(uiapp, this.TipoBarraStr, this.ubicacionEnlosa, DatosNuevoPathDTO.TipoConfiguracionBarra, false);


            _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(_uiapp.ActiveUIDocument, _solicitudBarraDTO);
            // _seleccionarLosaBarraRoom.LosaSeleccionada1 = barraRoomDto.LosaSeleccionada1;

            //cargar datos iniciales
            if (!CargarDatosIniciales(uiapp, _datosLosaDTO.ubicacionLosa)) return;
            _BarraTipo = TipoRebar.LOSA_INF;
            //carga
            ListaPtosPerimetroBarras.Add(DatosNuevoPathDTO.Lista4ptosPAth[0]);
            ListaPtosPerimetroBarras.Add(DatosNuevoPathDTO.Lista4ptosPAth[1]);
            ListaPtosPerimetroBarras.Add(DatosNuevoPathDTO.Lista4ptosPAth[2]);
            ListaPtosPerimetroBarras.Add(DatosNuevoPathDTO.Lista4ptosPAth[3]);


            this.LargoPathreiforment = DatosNuevoPathDTO.LargoPathreiforment;
            this.LargoMin_1 = _datosLosaDTO.largoMinimo;
            // floor asignado 
            this.diametroEnMM = Util.FootToMmInt(_datosLosaDTO.diametroEnFoot);
            this.LosaSeleccionada1 = _datosLosaDTO._losa;
            this.EspesorLosaCm_1 = _datosLosaDTO.espesorLosaCM;
            this.CurvesPathreiforment = DatosNuevoPathDTO.CurvesPathreiforment;
            this.Espaciamiento = _datosLosaDTO.Espaciamiento;
            // if (SALIRAS) return;
            //no se utiliza los ganchosRE
            this.ptoConMouseEnlosa1 = DatosNuevoPathDTO.ptoTagSoloTraslapo;

            //objetos con los datos para dibujar barra
            _datosNuevaBarraDTO = new DatosNuevaBarraDTO()
            {
                LargoPathreiforment = LargoPathreiforment,
                LargoRecorridoFoot = ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]),
                DiametroMM = diametroEnMM,
                LargoMininoLosa = LargoMin_1,
                IsLuzSecuandiria = IsLuzSecuandiria,
                EspaciamientoFoot = Espaciamiento,
                EspesorLosaCm_1 = EspesorLosaCm_1,
                Prefijo_cuantia = DatosNuevoPathDTO.Prefijo_F,// (_datosNuevaBarraDTO.TipoCaraObjeto_ == TipoCaraObjeto.Inferior ? "F=" : "F'="),
                DiametroOrientacionPrincipal_mm = 10,//valor referncia pq genneral mente barras entre 8 o 12. Buscar o cualcular
                PathSymbol_REbarshape_FXXDTO_ = new PathSymbol_REbarshape_FxxDTO()
            };

            //definir tag
            _listaTAgBArra = FactoryGeomTag.CrearGeometriaTag(_doc, DatosNuevoPathDTO.ptoTagSoloTraslapo, _solicitudBarraDTO, DatosNuevoPathDTO.Lista4ptosPAth);
            _listaTAgBArra.Ejecutar(new GeomeTagArgs() { angulorad = DatosNuevoPathDTO.AnguloRoomRad });

            //obtener hook
            ITipoHook ObtenerTipoHook = new TipoHook(_solicitudBarraDTO);
            ObtenerTipoHook.DefinirHook();

            if (!ObtenerRebarshape()) return;

            //dimBarras_internos= PathRebarShape.
            // ITiposPathReinformentSymbol ptSymbol = null;
            if (TipoBarraStr == "s1" || TipoBarraStr == "s2" || TipoBarraStr == "s3" || TipoBarraStr == "s4")
            {
                //es if es solo para el caso de editar tipo de path, para losa casos superior
                //no esta claro si   AnguloBordeRoomYSegundoPtoMouseGrado=0 wa siempre 0 para el caso de tralapo
                AnguloBordeRoomYSegundoPtoMouseGrado = Math.Round(Util.AnguloEntre2PtosGrado90(ListaPtosPerimetroBarras[0], ListaPtosPerimetroBarras[1], true), 0);
                ptSymbol = new TiposPathReinformentSymbol(_solicitudBarraDTO, AnguloBordeRoomYSegundoPtoMouseGrado);

                _datosNuevaBarraDTO.nombreSimboloPathReinforcement = ptSymbol.M1_DefinirPathReinformentSymbol();
            }
            else
            {
                if (!ObtenerPathSymbol()) return;
            }
            statusbarra = Result.Succeeded;
        }
        //4-CONTRUCTOR PARA BARRAS INFERIORES   ----> CASOS DISEÑO AUTOMATICO
        public BarraRoom(UIApplication uiapp, NH_RefereciaCrearBarra nH_RefereciaLosaParaBarra, ConfiguracionAhorro configuracionAhorro, bool IsBuscarTipoBarra = false)
        {
            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView) || (!Directory.Exists(ConstNH.CONST_COT)))
            {
                statusbarra = Result.Failed;
                return;
            }
            this._uiapp = uiapp;
            contador += 1;
            this.ubicacionEnlosa = nH_RefereciaLosaParaBarra.UbicacionEnLosa;
            BarraRoomGeometria.IsBuscarTipoBarra = IsBuscarTipoBarra;
            BarraRoomGeometria.IsLosaINclinada = false;
            this.Ismensaje12Mt = false;
            this.TipoBarraStr = nH_RefereciaLosaParaBarra.tipoBarra.ToString();
            this.LargoRecorridoX = nH_RefereciaLosaParaBarra.LargoRecorridoX;
            this.LargoRecorridoY = nH_RefereciaLosaParaBarra.LargoRecorridoY;

            _solicitudBarraDTO = new SolicitudBarraDTO(uiapp, this.TipoBarraStr, nH_RefereciaLosaParaBarra.UbicacionEnLosa, TipoConfiguracionBarra.refuerzoInferior, false);

            //carga parametros iniciales
            if (!CargarDatosIniciales(uiapp, nH_RefereciaLosaParaBarra.UbicacionEnLosa)) return;

            //viewrange
            // IViewRangleClase viewRangleClase = ViewRangleClase.CreatorNuevoViewRange(doc, view);
            // viewRangleClase.EditarParametroViewRange(ConstantesGenerales.VIEWRANGE_TOP, ConstantesGenerales.VIEWRANGE_CORTE, ConstantesGenerales.VIEWRANGE_BOTTON, ConstantesGenerales.VIEWRANGE_DEPTH);

            //carga losa datos niciales
            if (_solicitudBarraDTO.TipoBarra.ToLower() == "s1" || _solicitudBarraDTO.TipoBarra.ToLower() == "s3")
            { return; }// statusbarra = Load_s1_s3();
            else
            {
                statusbarra = Load_fx(nH_RefereciaLosaParaBarra.PosicionPto_Barra, nH_RefereciaLosaParaBarra.PelotaLosa as Floor);
                ptoConMouseEnlosa1 = BarraRoomGeometria.ObtenerPtoMouseParaDirectrizCasoAutomatico(_refereciaRoomDatos, ListaPtosPerimetroBarras);
                _seleccionarLosaBarraRoom.PtoConMouseEnlosa1 = ptoConMouseEnlosa1;
            }
            _BarraTipo = TipoRebar.LOSA_INF;
            if (statusbarra != Result.Succeeded) return;

            _solicitudBarraDTO = configuracionAhorro.VerificarDiseñoConAHorro(_solicitudBarraDTO, ListaPtosPerimetroBarras);

            this.TipoBarraStr = _solicitudBarraDTO.TipoBarra;
            this.ubicacionEnlosa = _solicitudBarraDTO.UbicacionEnlosa;
            //obtener largo path
            DefinirLargoPathreiforment();

            //asigan los  CurvesPathreiforment , en el caso que sean mas de 3
            RedefinirCurvesPathreiforment();

            //definir tag
            _listaTAgBArra = FactoryGeomTag.CrearGeometriaTag(_doc, _seleccionarLosaBarraRoom.PtoConMouseEnlosa1, _solicitudBarraDTO, ListaPtosPerimetroBarras);
            _listaTAgBArra.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(Angle_pelotaLosa1Grado) });

            //objetos con los datos para dibujar barra
            _datosNuevaBarraDTO = new DatosNuevaBarraDTO()
            {
                LargoPathreiforment = LargoPathreiforment,
                LargoRecorridoFoot = ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]),
                DiametroMM = diametroEnMM,
                LargoMininoLosa = LargoMin_1,
                IsLuzSecuandiria = IsLuzSecuandiria,
                EspaciamientoFoot = Espaciamiento,
                DiametroOrientacionPrincipal_mm = DiametroOrientacionPrincipal_mm
            };

            ITiposRebarShape PathRebarShape = new TiposRebarShape(_solicitudBarraDTO, _datosNuevaBarraDTO, EspesorLosaCm_1);
            _datosNuevaBarraDTO = PathRebarShape.DefinirRebarShape();

            ITiposPathReinformentSymbol ptSymbol = new TiposPathReinformentSymbol(_solicitudBarraDTO, _seleccionarLosaBarraRoom, Angle_pelotaLosa1Grado);
            _datosNuevaBarraDTO.nombreSimboloPathReinforcement = ptSymbol.M1_DefinirPathReinformentSymbol();


        }
        //5-CONTRUCTOR PARA BARRAS SUPLES  ----> CASOS DISEÑO ATUTOMATICO
        public BarraRoom(UIApplication uiapp, NH_RefereciaCrearSuple nH_RefereciaLosaParaSuple, bool IsBuscarTipoBarra = false)
        {
            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView) || (!Directory.Exists(ConstNH.CONST_COT)))
            {
                statusbarra = Result.Failed;
                return;
            }
            this.Ismensaje12Mt = false;
            this._uiapp = uiapp;
            contador += 1;
            this.ubicacionEnlosa = nH_RefereciaLosaParaSuple.UbicacionEnLosa;
            //para que busqye el tipo automaticamente
            BarraRoomGeometria.IsBuscarTipoBarra = IsBuscarTipoBarra;
            this.TipoBarraStr = nH_RefereciaLosaParaSuple.tipoBarra.ToString();


            _solicitudBarraDTO = new SolicitudBarraDTO(uiapp, this.TipoBarraStr, nH_RefereciaLosaParaSuple.UbicacionEnLosa, TipoConfiguracionBarra.suple, false);

            if (!CargarDatosIniciales(uiapp, nH_RefereciaLosaParaSuple.UbicacionEnLosa)) return;

            //  IViewRangleClase viewRangleClase = ViewRangleClase.CreatorNuevoViewRange(doc, view);
            //  viewRangleClase.EditarParametroViewRange(ConstantesGenerales.VIEWRANGE_TOP, ConstantesGenerales.VIEWRANGE_CORTE, ConstantesGenerales.VIEWRANGE_BOTTON, ConstantesGenerales.VIEWRANGE_DEPTH);


            //carga losa datos niciales
            if (TipoBarraStr.ToLower() == "s1" || TipoBarraStr.ToLower() == "s3" || TipoBarraStr.ToLower() == "s2")

            {
                statusbarra = Load_s1_s3V2(uiapp, nH_RefereciaLosaParaSuple.PosicionPtoSupleInicial,
                                           nH_RefereciaLosaParaSuple.PosicionPtoSupleFinal,
                                           (Floor)nH_RefereciaLosaParaSuple.PelotaLosa, IsTest);

                // _solicitudBarraDTO.UbicacionEnlosa = ubicacionEnlosa.ToString();
                _solicitudBarraDTO.UbicacionEnlosa = ubicacionEnlosa;
                nH_RefereciaLosaParaSuple.UbicacionEnLosa = ubicacionEnlosa;
                _solicitudBarraDTO.ObtenerOrientacion();
                //ptoConMouseEnlosa1 = new XYZ(ListaPtosPerimetroBarras.Average(p => p.X),
                //                            ListaPtosPerimetroBarras.Average(p => p.Y),
                //                            ListaPtosPerimetroBarras.Average(p => p.Z));
                ptoConMouseEnlosa1 = AyudaSuples.ObtenerPtoPathSymbolSx(ListaPtosPerimetroBarras);
                _seleccionarLosaBarraRoom.PtoConMouseEnlosa1 = ptoConMouseEnlosa1;
                _BarraTipo = TipoRebar.LOSA_SUP;
            }
            else
            { return; }   //statusbarra = Load_fx(nH_RefereciaLosaParaBarra.PosicionPto_Barra, nH_RefereciaLosaParaBarra.PelotaLosa as Floor);



            //definir tag
            _listaTAgBArra = FactoryGeomTag.CrearGeometriaTag(_doc, _seleccionarLosaBarraRoom, _solicitudBarraDTO, ListaPtosPerimetroBarras);
            _listaTAgBArra.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(Angle_pelotaLosa1Grado) });


            //NOTA. solo caso SUPLE AUTOMATICO
            ReconfigurarAnguloBordeRoomYSegundoPtoMouseGrado();


            //obtener largo path
            DefinirLargoPathreiforment();
            nH_RefereciaLosaParaSuple.diametro = diametroEnMM;
            //objetos con los datos para dibujar barra
            _datosNuevaBarraDTO = new DatosNuevaBarraDTO()
            {
                LargoPathreiforment = LargoPathreiforment,
                LargoRecorridoFoot = ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]),
                LargoMininoLosa = LargoMin_1,
                DiametroMM = nH_RefereciaLosaParaSuple.diametro,
                IsLuzSecuandiria = IsLuzSecuandiria,
                EspaciamientoFoot = Espaciamiento
            };

            ITiposRebarShape PathRebarShape = new TiposRebarShape(_solicitudBarraDTO, _datosNuevaBarraDTO, EspesorLosaCm_1);
            _datosNuevaBarraDTO = PathRebarShape.DefinirRebarShape();

            ITiposPathReinformentSymbol ptSymbol = new TiposPathReinformentSymbol(_solicitudBarraDTO, _seleccionarLosaBarraRoom, Angle_pelotaLosa1Grado);
            _datosNuevaBarraDTO.nombreSimboloPathReinforcement = ptSymbol.M1_DefinirPathReinformentSymbol();

        }
        //se reconfigura AnguloBordeRoomYSegundoPtoMouseGrado para tener siempre las misma configuracion
        private void ReconfigurarAnguloBordeRoomYSegundoPtoMouseGrado()
        {
            //si es orizontal mantener la misma direccion de losa
            //si es vertical  AnguloBordeRoomYSegundoPtoMouseGrado = angulo de losa +90
            _seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado = (TipoOrientacion == TipoOrientacionBarra.Horizontal ?
                                                                             _refereciaRoomDatos.anguloPelotaLosaGrado_1 :
                                                                             _refereciaRoomDatos.anguloPelotaLosaGrado_1 + 90);
            AnguloBordeRoomYSegundoPtoMouseGrado = _seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado;

        }


        /// <summary>
        /// obtiene largo del  Pathreiforment
        /// </summary>
        private void DefinirLargoPathreiforment()
        {
            LargoPathreiforment = ListaPtosPerimetroBarras[1].DistanceTo(ListaPtosPerimetroBarras[2]);
            LargoRecorrido = ListaPtosPerimetroBarras[3].DistanceTo(ListaPtosPerimetroBarras[2]);
        }

        /// <summary>
        /// ReAsigna las variables a las 3 pathreimforment correspondiente, se usa para los caso F1SUP
        /// para dibujar los path de para  superiore cortos 
        /// 
        /// </summary>
        private void RedefinirCurvesPathreiforment()
        {
            //solo si tiene mas de una curva
            if (CurvesPathreiforment.Count > 1)
            {
                int cont = 1;
                IList<Curve> aux_CurvesPathreiforment = CurvesPathreiforment;
                Curve aux_Curves = null;
                foreach (Curve item in aux_CurvesPathreiforment)
                {
                    if (cont == 1)
                    {
                        //caso normal
                        aux_Curves = item;
                    }
                    else if (cont == 2)
                    {
                        //para path corte lado dere F1sup
                        CurvesPathreiforment_DereSup.Add(item);
                    }
                    else
                    {
                        //para path corte lado IZq F1sup
                        CurvesPathreiforment_IzqInf.Add(item);
                    }
                    cont += 1;
                }
                CurvesPathreiforment.Clear();
                CurvesPathreiforment.Add(aux_Curves);
            }
        }

        //para caso f1_sup para dibujar en view diferente - superior
        public void BUscarViewSUPERIOR()
        {//deshabilitado
            return;
#pragma warning disable CS0162 // Unreachable code detected
            try
#pragma warning restore CS0162 // Unreachable code detected
            {
                var Viewsup = Util.GetFirstViewporNombre(_doc, _view.Name
                                                                    .Replace(ConstNH.NOMBRE_PLANOLOSA_INF, "")
                                                                    .Replace("(", "")
                                                                    .Replace(")", "")
                                                                    .Trim(), ConstNH.NOMBRE_PLANOLOSA_SUP);
                if (Viewsup != null) _view = (View)Viewsup;
            }
            catch (Exception)
            {
            }
        }

        ////desplza los ptos 1 y 0 haci el inerior del path
        ///*
        // *    0 -->0'---- 3
        // *    1 -->1'---- 2
        //*/
        //private Curve DesplazarCurvaIzqInf(Curve item)
        //{

        //    XYZ p0 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(item.GetEndPoint(0),
        //                                                        Util.GradosToRadianes(_refereciaRoomDatos.anguloBarraLosaGrado_1)
        //                                                        , largoMin_1 * 0.15);
        //    XYZ p1 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(item.GetEndPoint(1),
        //                                                  Util.GradosToRadianes(_refereciaRoomDatos.anguloBarraLosaGrado_1)
        //                                                  , largoMin_1 * 0.15);

        //    return Line.CreateBound(p0, p1);
        //}

        /// <summary>
        /// valores de carga inicial
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="nH_RefereciaLosaParaBarra"></param>
        /// <returns></returns>
        private bool CargarDatosIniciales(UIApplication uiapp, UbicacionLosa direccion)
        {

            this._uiapp = uiapp;
            this.uidoc = _uiapp.ActiveUIDocument;
            this.app = _uiapp.Application;
            this._doc = uidoc.Document;
            this.opt = app.Create.NewGeometryOptions();

            view3D = TiposFamilia3D.Get3DBuscar(_doc);

            view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            //  barraRoomDatos = new BarraRoomDatos() { doc = uidoc.Document };

            // barraRoomPersis = new BarraRoomPersis(barraRoomDatos);
            // cargarFAmilias_carga = new CargarFAmilias_carga(doc);
            //rutaRaiz = InfoSystema.ObtenerRutaDeFamilias();

            //obtener view aanalizado comprueba que nosea 3d
            //falta agregar que no sea elevacion
            this._view = uiapp.ActiveUIDocument.Document.ActiveView;
            var scale = _view.Scale;
            if (IsEscalaInCorrecta()) return false;


            View3D elem3d = _view as View3D;
            if (null != elem3d)
            {
                Util.ErrorMsg("Debe ejecutar comando en un ViewPlan");
                statusbarra = Result.Failed;
                return false;
            }


            TipoOrientacion = TipoOrientacionBarra.Horizontal;
            switch (direccion)
            {
                case UbicacionLosa.Derecha:
                case UbicacionLosa.Izquierda:
                    TipoOrientacion = TipoOrientacionBarra.Horizontal;
                    break;
                case UbicacionLosa.Inferior:
                case UbicacionLosa.Superior:
                    TipoOrientacion = TipoOrientacionBarra.Vertical;
                    break;

                default:
                    break;
            }

            this.ubicacionEnlosa = ubicacionEnlosa;

            //crear lista imagenes
            //  if (!IsTest) cargarFAmilias_carga.CrearDiccionarioImagenes();

            // cargar familias necesarias
            //   if (!IsTest) cargarFAmilias_carga.cargarFamilias_run();


            // lsita para crear reinforme
            curvesPathreiforment = new List<Curve>();
            CurvesPathreiforment_IzqInf = new List<Curve>();
            CurvesPathreiforment_DereSup = new List<Curve>();


            // deja los hook gancho como null
            tipodeHookStartPrincipal = null;
            tipodeHookEndPrincipal = null;
            tipodeHookStarAlternativa = null;
            tipodeHookEndAlternativa = null;

            this.TipoBarra_izq_Inf = "";
            this.TipoBarra_dere_sup = "";

            dimBarras = null;
            dimBarras_parameterSharedLetras = null;

            //para obtiene los putnos de dos segmentos roomm, para generar barras
            ListaPtosPoligonoLosa = new List<XYZ>();
            //para guardar 4 ptos que definen el area de la barra a dibujar
            ListaPtosPerimetroBarras = new List<XYZ>();



            int valor3 = 1;
            if (valor3 == 1)
                Tiporefuerzo = TipoRefuerzo.PathRefuerza;
            else
                Tiporefuerzo = TipoRefuerzo.AreaRefuerzo;

            message = "";



            EspesorMuro_Izq_abajo = 0;
            EspesorMuro_Dere_Sup = 0;
            EspesorLosaCm_1 = 0;
            Angle_pelotaLosa1Grado = 0;

            DireccionMayor = new XYZ(1 * Math.Sin(Util.GradosToRadianes(Angle_pelotaLosa1Grado)), 1 * Math.Cos(Util.GradosToRadianes(Angle_pelotaLosa1Grado)), 0);

            statusbarra = Result.Failed;

            return true;
        }

        private bool IsEscalaInCorrecta()
        {
            if (_view.Scale == 50 || _view.Scale == 75 || _view.Scale == 100 || IsTest)
                return false;

            TaskDialog.Show("Error escala", "La escala para crear barra debe ser 50 -75-100");
            statusbarra = Result.Failed;
            return true;

        }


        /// <summary>
        /// se tuliza para  barra inferioress
        /// </summary>
        /// <returns></returns>


        /// <summary>
        /// se tuliza para  barra inferioress
        /// </summary>
        /// <returns></returns>
        public Result Load_fx(XYZ pointMouse, Floor floor, bool IsTest = false)
        {


            // seleccionado vista con nombre  '{3D}'
            view3D = TiposFamilia3D.Get3DBuscar(uidoc.Document);

            //selecciona un objeto floor
            ptoConMouseEnlosa1 = null;
            //seleccionar losa y room
            _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(uidoc, _solicitudBarraDTO);


            Result result_seleccion = Result.Cancelled;
            if (pointMouse == null && floor == null)
            {
                result_seleccion = _seleccionarLosaBarraRoom.selecconarUNRoom();
            }
            else // para programa automatico
            {
                if (pointMouse == null)
                {
                    Debug.WriteLine("Error 'Load_fx'  pointMouse == null ");
                    return result_seleccion;
                }
                if (floor == null)
                {
                    Debug.WriteLine("Error 'Load_fx'  floor == null ");
                    return result_seleccion;
                }
                _seleccionarLosaBarraRoom.AsignarUnRoom(pointMouse, floor);
                result_seleccion = Result.Succeeded;
            }

            if (result_seleccion != Result.Succeeded)
                return result_seleccion;



            if (!_seleccionarLosaBarraRoom.ObtenerUNRoom())
            {
                Util.ErrorMsg("No se puedo encontrar room. Tanto el room como la losa deben estar asignadas al mismo nivel y la base del room debe estar a nivel de losa");
                return Result.Failed;
            }

            if (_seleccionarLosaBarraRoom.RoomSelecionado1 == null)
            {
                TaskDialog.Show("Error", $"Room seleccionado igual a NULL. Nivel analizado {_seleccionarLosaBarraRoom.PtoConMouseEnlosa1.REdondearString_foot(2)}. Verificar que losa y room esten en el mismo nivel");
                return Result.Failed;
            }


            if (!_seleccionarLosaBarraRoom.LosaSeleccionada1.IsEstructural())
            {
                Util.ErrorMsg($"Elemento seleccionado con ID:{_seleccionarLosaBarraRoom.LosaSeleccionada1.Id} no esta definido como elemento estructural");
                statusbarra = Result.Failed;
                return Result.Failed;
            }

            _roomSelecionado1 = _seleccionarLosaBarraRoom.RoomSelecionado1;
            _refereciaRoomDatos = new ReferenciaRoomDatos(_seleccionarLosaBarraRoom, _solicitudBarraDTO);
            _refereciaRoomDatos.GetParametrosUnRoom();

            if (!_refereciaRoomDatos.IsLuzSecuandiria)
                TipoDireccionBarra_ = TipoDireccionBarra.Primaria;
            else
                TipoDireccionBarra_ = TipoDireccionBarra.Secundario;
            DiametroOrientacionPrincipal_mm = _refereciaRoomDatos.DiametroOrientacionPrincipal;
            LosaSeleccionada1 = _seleccionarLosaBarraRoom.LosaSeleccionada1;// doc.GetElement(r_fx.ElementId) as Floor;            //obtiene el nivel del la losa
            var faceInf = LosaSeleccionada1.ObtenerCaraInferior(false);
            LevelLosa = _seleccionarLosaBarraRoom.LevelLosa1;// doc.GetElement(SelecFloor_1.LevelId) as Level;
            ptoConMouseEnlosa1 = _seleccionarLosaBarraRoom.PtoConMouseEnlosa1;// = r_fx.GlobalPoint;

            if (_solicitudBarraDTO.Ui_pathSymbolDTO_ != null)
                _solicitudBarraDTO.Ui_pathSymbolDTO_.ptoMouse = ptoConMouseEnlosa1;

            List<XYZ> _listaPtosLineasAtrasDelante = BarraRoomGeometria.ListaPoligonosRooms_fx(_uiapp, _refereciaRoomDatos, opt, _solicitudBarraDTO.TipoOrientacion);
            //   ConstantesGenerales.sbLog.AgregarListaPtos(_listaPtosLineasAtrasDelante, "Lista De bordes de room");

            if (_listaPtosLineasAtrasDelante.Count != 4)

            {
                TaskDialog.Show("Error", "Error al buscar los ptos que intersectan con el room. Ptos encontrados : " + ListaPtosPerimetroBarras.Count);

                return Result.Failed;
            }


            //*********************************nn
            // double anguloBarra = 0;
            EspesorLosaCm_1 = _refereciaRoomDatos.espesorCM_1;
            Angle_pelotaLosa1Grado = _refereciaRoomDatos.anguloPelotaLosaGrado_1;
            LargoMin_1 = _refereciaRoomDatos.largomin_1;
            CuantiaB = _refereciaRoomDatos.CuantiaBarra;
            IsLuzSecuandiria = _refereciaRoomDatos.IsLuzSecuandiria;
            diametroEnMM = _refereciaRoomDatos.diametro;

            Espaciamiento = _refereciaRoomDatos.Espaciamiento;
            CuantiaB = _refereciaRoomDatos.CuantiaBarra;

            //*********************************

            // LargoRecorridoY= LargoRecorridoX = 4;
            // 4 ptos que definen el area de la barra a dibujarnhs
            BarraRoomGeometria.LargoRecorridoX =  LargoRecorridoX;
            BarraRoomGeometria.LargoRecorridoY =  LargoRecorridoY;
            BarraRoomGeometria.ptoSelect = ptoConMouseEnlosa1;

            //obtiene   < pto perimetros,curva path>
            Tuple<List<XYZ>, IList<Curve>> result = BarraRoomGeometria.ListaFinal_ptov2(_listaPtosLineasAtrasDelante, _uiapp, _seleccionarLosaBarraRoom, _solicitudBarraDTO, _refereciaRoomDatos, IsTest);

            if (result == null) return Result.Failed;
            ListaPtosPerimetroBarras = result.Item1;
            curvesPathreiforment = result.Item2;

            //      ConstantesGenerales.sbLog.AgregarListaPtos(ListaPtosPerimetroBarras, "ListaPtos Perimetro Barras");
            //      ConstantesGenerales.sbLog.AppendLine("Largo:" + ListaPtosPerimetroBarras[3].DistanceTo(ListaPtosPerimetroBarras[2]));
#if DEBUG

#endif
            //**************************************************************************************************************
            //usar la obtener la posicon de los fierro f1_SUP
            if (_solicitudBarraDTO.TipoBarra.ToLower() == "f1_sup" || _solicitudBarraDTO.TipoBarra.ToLower() == "fauto")
            {
                Tuple<XYZ, XYZ> resultPtoMOuseF1_sup = BarraRoomGeometria.ObtenerPtoMouseF1_SUP(_refereciaRoomDatos, ListaPtosPerimetroBarras);
                ptoConMouseEnlosaF1_SUPIzqINf = resultPtoMOuseF1_sup.Item1;
                ptoConMouseEnlosaF1_SUPDereSup = resultPtoMOuseF1_sup.Item2;
            }

            //int iptoPerimetro = 1;
            //Debug.Print("Pto Mouse:" + _seleccionarLosaBarraRoom.PtoConMouseEnlosa1);
            //foreach (var item in ListaPtosPerimetroBarras) Debug.Print("Pto Perimetro" + iptoPerimetro++ + ":" + item);



            if (BarraRoomGeometria.IsBuscarTipoBarra)
            {
                AnalisisFauto _fauto = BarraRoomGeometria._analisisFauto;
                _solicitudBarraDTO.TipoBarra = _fauto.tipoBarraPrincipal;
                _solicitudBarraDTO.UbicacionEnlosa = _fauto.ubicacionENlosa;
                this.TipoBarraStr = _fauto.tipoBarraPrincipal;
                this.ubicacionEnlosa = _fauto.ubicacionENlosa;
                this.TipoBarra_izq_Inf = _fauto.tipoBarra_izq_infer;
                this.TipoBarra_dere_sup = _fauto.tipoBarra_dere_sup;
            }

            if (ListaPtosPerimetroBarras.Count != 4)
            {
                //TaskDialog.Show("Error", "Ptos que definen area de refuerzo deber ser 4. Ptos encontrados: " + ListaPtosPerimetroBarras.Count);
                Debug.WriteLine("Ptos que definen area de refuerzo deber ser 4. Ptos encontrados: " + ListaPtosPerimetroBarras.Count);
                return Result.Failed;
            }
            //*********************************


            return Result.Succeeded;
        }



        /// <summary>
        /// se utiliza para barras superior
        /// </summary>
        /// <returns></returns>
        private Result Load_s1_s3V2(UIApplication _uiapp, XYZ ptoSeleccionRoom1, XYZ ptoSeleccionRoom2, Floor floor1, bool IsTest = false)
        {
            ConstNH.sbLog = new StringBuilder();
            _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(uidoc, _solicitudBarraDTO);

            Result result_seleccion = Result.Cancelled;
            if (ptoSeleccionRoom1 == null && ptoSeleccionRoom2 == null && floor1 == null)
            {
                result_seleccion = _seleccionarLosaBarraRoom.selecconarDOSRoom();
            }
            else
            {
                _seleccionarLosaBarraRoom.AsignarDOSRoom(ptoSeleccionRoom1, ptoSeleccionRoom2, floor1);
                result_seleccion = Result.Succeeded;
            }


            if (result_seleccion != Result.Succeeded)
                return result_seleccion;

            #region refactorizar
            _seleccionarLosaBarraRoom.ObtenerDOSRoom();

            _roomSelecionado1 = _seleccionarLosaBarraRoom.RoomSelecionado1;
            _roomSelecionado2 = _seleccionarLosaBarraRoom.RoomSelecionado2;

            _refereciaRoomDatos = new ReferenciaRoomDatos(_seleccionarLosaBarraRoom, _solicitudBarraDTO);
            if (!_refereciaRoomDatos.GetParametrosUnRoom()) return Result.Failed;
            if (!_refereciaRoomDatos.GetParametrosDOsRoom()) return Result.Failed;

            LosaSeleccionada1 = _seleccionarLosaBarraRoom.LosaSeleccionada1;// doc.GetElement(r_fx.ElementId) as Floor;            //obtiene el nivel del la losa
            LevelLosa = _seleccionarLosaBarraRoom.LevelLosa1;// doc.GetElement(SelecFloor_1.LevelId) as Level;

            ptoConMouseEnlosa1 = _seleccionarLosaBarraRoom.PtoConMouseEnlosa1;// = r_fx.GlobalPoint;
            BarraRoomGeometria.ptoSelect = ptoConMouseEnlosa1;

            ptoConMouseEnlosa2 = _seleccionarLosaBarraRoom.PtoConMouseEnlosa2;// = r_fx.GlobalPoint;
            BarraRoomGeometria.ptoSelect_2 = ptoConMouseEnlosa2;

            #endregion

            // if (_refereciaRoomDatos != null) ConstantesGenerales.sbLog.AppendLine("N° Room = " + rm1..ToString() + ";");
            if (_refereciaRoomDatos != null) ConstNH.sbLog.AppendLine("N° Angulo = " + _refereciaRoomDatos.anguloBarraLosaGrado_1.ToString() + ";");
            if (LosaSeleccionada1 != null) ConstNH.sbLog.AppendLine("int IdLosa = " + LosaSeleccionada1.Id.ToString() + ";");
            if (ptoConMouseEnlosa1 != null) ConstNH.sbLog.AppendLine("XYZ ptoMOuse1 = new XYZ" + ptoConMouseEnlosa1.ToString() + ";");
            if (ptoConMouseEnlosa2 != null) ConstNH.sbLog.AppendLine("XYZ ptoMOuse2 = new XYZ" + ptoConMouseEnlosa2.ToString() + ";");



            Tuple<List<XYZ>, double> result1 = BarraRoomGeometria.ListaPoligonosRooms_S1_S3V2(_uiapp.ActiveUIDocument.Application, _refereciaRoomDatos, TipoOrientacion);

            List<XYZ> ListaPtosPoligonoLosa = result1.Item1;
            _seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado = result1.Item2;
            AnguloBordeRoomYSegundoPtoMouseGrado = _seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado;

            if (ListaPtosPoligonoLosa.Count > 4)
            {
                Debug.Print("Error al buscar los ptos que intersectan con el room. Ptos encontrados : " + ListaPtosPoligonoLosa.Count);
                return Result.Failed;
            }
            if (ListaPtosPoligonoLosa.Count < 4)
            {
                TaskDialog.Show("Error", "Error al buscar los ptos que intersectan con el room. Ptos encontrados : " + ListaPtosPoligonoLosa.Count);
                return Result.Failed;
            }


            if (!(LosaSeleccionada1.IsEstructural()))
            {
                Util.ErrorMsg($"Elemento seleccionado con ID:{LosaSeleccionada1.Id} no esta definido como elemento estructural");
                statusbarra = Result.Failed;
                return Result.Failed;
            }

            //      ConstantesGenerales.sbLog.AgregarListaPtos(ListaPtosPoligonoLosa, "Lista De bordes de room");


            //*********************************
            // segunda room1
            EspesorLosaCm_1 = _refereciaRoomDatos.espesorCM_1;
            Angle_pelotaLosa1Grado = _refereciaRoomDatos.anguloPelotaLosaGrado_1;
            LargoMin_1 = _refereciaRoomDatos.largomin_1;

            CuantiaB = _refereciaRoomDatos.CuantiaBarra;
            IsLuzSecuandiria = _refereciaRoomDatos.IsLuzSecuandiria;
            diametroEnMM = _refereciaRoomDatos.diametro;

            Espaciamiento = _refereciaRoomDatos.Espaciamiento;
            CuantiaB = _refereciaRoomDatos.CuantiaBarra;


            // segunda room2

            EspesorLosa_2 = _refereciaRoomDatos.espesor_2;
            Angle_pelotaLosa2 = _refereciaRoomDatos.anguloPelotaLosaGrado_2;
            LargoMin_2 = _refereciaRoomDatos.largomin_2;

            //*********************************
            //// 4 ptos que definen el area de la barra a dibujar

            var result = BarraRoomGeometria.ListaFinal_ptov2(ListaPtosPoligonoLosa, _uiapp, _seleccionarLosaBarraRoom, _solicitudBarraDTO, _refereciaRoomDatos, IsTest);
            if (result == null) return Result.Failed;

            ListaPtosPerimetroBarras = result.Item1;
            curvesPathreiforment = result.Item2;
            HorientacionSuple(ListaPtosPerimetroBarras, _refereciaRoomDatos.anguloPelotaLosaGrado_1);
            ConstNH.sbLog.AgregarListaPtos(ListaPtosPerimetroBarras, "ListaPtosPerimetroBarras");
            ConstNH.sbLog.AppendLine("Largo:" + ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]));
#if DEBUG
            // LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif
            if (ListaPtosPoligonoLosa.Count != 4)
            {
                TaskDialog.Show("Error", "Ptos que definen area de refuerzo deber ser 4. Ptos encontrados: " + ListaPtosPoligonoLosa.Count);
                return Result.Failed;
            }
            //*********************************


            return Result.Succeeded;
        }


        private void HorientacionSuple(List<XYZ> ListaPtosPerimetroBarras, double AnguloRoomGrado)
        {
            double anguloGrad = Util.AnguloEntre2PtosGrado90(ListaPtosPerimetroBarras[0], ListaPtosPerimetroBarras[1], true);

            if (Math.Abs(anguloGrad - AnguloRoomGrado) > 45)
                ubicacionEnlosa = UbicacionLosa.Izquierda;
            else
                ubicacionEnlosa = UbicacionLosa.Superior;

        }


        #endregion

        //meotods************************************************************************************************************************
        #region 2)metodo

        public Result CrearBarraPorTraslapo(XYZ desplazamiento)
        {
            //CrearBarra_1Trans(CurvesPathreiforment, LargoPathreiforment, nombreSimboloPathReinforcement, diametroEnMM, Espaciamiento, desplazamiento);
            return CrearBarra(CurvesPathreiforment, LargoPathreiforment, nombreSimboloPathReinforcement, diametroEnMM, Espaciamiento, desplazamiento);

        }


        public Result CrearBarra(IList<Curve> curvesPathreiforment_, double LargoPathreiforment_, string nombreSimboloPathReinforcement_, int diametro_, double espaciamiento_, XYZ desplazamientoPathReinSpanSymbol)
        {


            if (XYZ.Zero.DistanceTo(desplazamientoPathReinSpanSymbol) > 0.1)
            {
                ptoConMouseEnlosa1 = desplazamientoPathReinSpanSymbol;
                desplazamientoPathReinSpanSymbol = XYZ.Zero;
            }

            if (!VerificarDatos(_datosNuevaBarraDTO)) return Result.Failed;



            if (Tiporefuerzo == TipoRefuerzo.PathRefuerza && ListaPtosPerimetroBarras.Count > 0)
            {

                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Transaction Group-NH");
                    try
                    {
                        using (Transaction trans = new Transaction(_doc))
                        {
                            trans.Start("CreatePathReinforcement-NH");
                            //crea  refuerzo path
                            //flip true se dibuja hacia abajo de la curva de trayectoria

                            //flip false se dibuja hacia arriba de la curva de trayectoria
                            bool aux_flop = false;
                            if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                            {
                                //nada
                            }
                            else if (TipoOrientacion == TipoOrientacionBarra.Vertical)
                            {
                                if (Angle_pelotaLosa1Grado > 90)
                                    aux_flop = true;
                            }
                            //**************************************************************************************************************************************
                            //1)crea el pathreinformet 
                            curvesPathreiforment_ = RedondearRecorrido(curvesPathreiforment_.ToList(), _datosNuevaBarraDTO);
                            m_createdPathReinforcement = CreatePathReinforcement(aux_flop, LosaSeleccionada1, curvesPathreiforment_, diametro_);
                            if (m_createdPathReinforcement == null)
                            {
                                TiposPathReinformentSymbolElement.pathReinforcementTypeId = null;
                                trans.RollBack();
                                return statusbarra = Result.Failed;
                            }

                            //Element paraElem = m_createdPathReinforcement as Element;// _doc.GetElement(m_createdPathReinforcement.Id);
   
                            AgregarParameterShared(m_createdPathReinforcement);
                            if (m_createdPathReinforcement == null)
                            {
                                //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                                message = "NO se pudo crear m_createdPathReinforcement";
                                trans.RollBack();
                                return statusbarra = Result.Failed; ;
                            }

                            //CONFIGURACION DE PATHREINFORMENT
                            if (view3D_Visualizar != null)
                            {
                                //permite que la barra se vea en el 3d como solido
                                m_createdPathReinforcement.SetSolidInView(view3D_Visualizar, true);
                                //permite que la barra se vea en el 3d como sin interferecnias 
                                m_createdPathReinforcement.SetUnobscuredInView(view3D_Visualizar, true);
                            }

                            // Largo de princial Alternativa. Si es 0 no la activa
                            double largoPrincial = LargoPathreiforment_;
                            // Largo de barra Alternativa. Si es 0 no la activa 
                            double largoAlternative = LargoPathreiforment_;

                            // a) seleciona que barra se botton - inferior
                            //b) activa barra alternativa de ser necesario
                            //c) asigna largo de barra princiapales y alterniva si corresonde
                            //b) asigna largos              

                            if (TipoBarraStr.Contains("f16"))
                            {
                                ICasoBarra iTipoBarra = new Barra16(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f17"))
                            {
                                ICasoBarra iTipoBarra = new BarraF17(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f18"))
                            {
                                ICasoBarra iTipoBarra = new BarraF18(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f19"))
                            {
                                ICasoBarra iTipoBarra = new BarraF19(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f20a"))
                            {
                                ICasoBarra iTipoBarra = new BarraF20a(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f20b"))
                            {
                                ICasoBarra iTipoBarra = new BarraF20b(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f20"))
                            {
                                ICasoBarra iTipoBarra = new BarraF20(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f21a"))
                            {
                                ICasoBarra iTipoBarra = new BarraF21a(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f21b"))
                            {
                                ICasoBarra iTipoBarra = new BarraF21b(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else if (TipoBarraStr.Contains("f22")) // considera  f22a,f22aInv,f22b,f22bInv,
                            {
                                ICasoBarra iTipoBarra = new BarraF22(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                                iTipoBarra.LayoutRebar_PathReinforcement();
                            }
                            else
                            {
                                LayoutRebar_PathReinforcement(_solicitudBarraDTO, _datosNuevaBarraDTO);
                            }


                            //**************************************************************************************************************************************************************************
                            //2)Family fam = TiposFamilyRebar.getFamilyRebarShape("Structural Path Reinforcement 3", doc);
                         
                            if (elemtoSymboloPath == null)
                            {
                                //obtiene elemento dentos de la biblioteca de familia del PathReinforcementSymbol
                                string scala = _view.ObtenerEscalaPAraPAthSimbol().ToString();// ObtenerNombre_EscalaConfiguracion().ToString(); //ConstNH.CONST_ESCALA_BASE.ToString();// view.Scale.ToString();
                                if (IsTest) scala = "100";
                                //Element elemtoSymboloPath = TiposPathReinSpanSymbol.getPathReinSpanSymbol(_datosNuevaBarraDTO.nombreSimboloPathReinforcement, scala, _doc);
                                elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(_datosNuevaBarraDTO.ObtenerNombrePAthSymbol(scala), _doc);
                                if (elemtoSymboloPath == null) elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(_datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + scala, _doc);
                                // ParameterUtil.SetParaInt(elemtoSymboloPath, "A_", Util.CmToFoot(100 / 100));
                                if (elemtoSymboloPath == null)
                                {
                                    //   _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                                    Util.ErrorMsg($"NO se pudo  encontar PathSymbol :{_datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + scala}   ");
                                    trans.RollBack();
                                    return statusbarra = Result.Failed;
                                }
                            }

                            // busca el elemnto 'Arrow Filled 20 Degree' que es la flecha del recorrido una un metodo muy generico tratar de mejorar
                            // List<Element> listaArrow = Tipos_Arrow.FindAllArrowheads(_doc, "Arrow Filled 20 Degree");
                            Element _Arrow = Tipos_Arrow.ObtenerPrimerArrowheads(_doc, "Arrow Filled 20 Degree");
                            if (_Arrow != null)
                            { //asigna el tipo de flecha de recorrido al symbolo del pathrebar
                                ParameterUtil.SetParaElementId(elemtoSymboloPath, BuiltInParameter.LEADER_ARROWHEAD, _Arrow.Id);
                            }
                            else
                            { Util.ErrorMsg("'Arrow Filled 20 Degree' NO encontrado "); }

                            // 2.1)crea el symbolo con la forma de la barra
                            //double x = 0;// Util.CmToFoot(10);
                            //double y = 0; //Util.CmToFoot(10);
                            _PathReinSpanSymbol = PathReinSpanSymbol.Create(_doc, _view.Id, new LinkElementId(m_createdPathReinforcement.Id), desplazamientoPathReinSpanSymbol, elemtoSymboloPath.Id);
                            if (ptoConMouseEnlosa1 != null) _PathReinSpanSymbol.TagHeadPosition = ptoConMouseEnlosa1;
                            if (TipoBarraStr.EsRefuerzoSUperiorSx())
                                _PathReinSpanSymbol.TagHeadPosition = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoConMouseEnlosa1,
                                                                                                         Util.GradosToRadianes(AnguloBordeRoomYSegundoPtoMouseGrado)
                                                                                                         , largoPrincial * 0.15);

                            ParameterUtil.SetParaInt(_PathReinSpanSymbol, "A_r2", Util.CmToFoot(100 / 100));
                            // ParameterUtil.SetParaInt(symbolPath, "B_sy", Util.CmToFoot(100 / 100));
                            if (_PathReinSpanSymbol == null)
                            {
                                //    _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                                message = "NO se pudo crear Path Symbol";
                                trans.RollBack();
                                return statusbarra = Result.Failed;
                            }


                            ConstNH.sbLog.AppendLine($"g");
                            //else 
                            if (_listaTAgBArra.M4_IsFAmiliaValida())
                            {
                                foreach (TagBarra item in _listaTAgBArra.listaTag)
                                {
                                    if (item.IsOk)
                                    {
                                        ;
                                        if (!item.DibujarTagPathReinforment(m_createdPathReinforcement, _uiapp, _view, desplazamientoPathReinSpanSymbol))
                                        {
                                            message = "NO se pudo obtener el TAg de PathReinforcement";
                                            Util.ErrorMsg($"CrearBarra  -->   ex {message}");
                                            trans.RollBack();
                                            return statusbarra = Result.Failed;
                                        }
                                    }
                                }
                            }
                            else if (_datosNuevaBarraDTO.IsBarrAlternative)
                            {
                                string numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)B_InfIzq_" + _view.ObtenerEscalaPAraPAthSimbol();
                                DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);
                                numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)B_DereSup_" + _view.ObtenerEscalaPAraPAthSimbol();
                                DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);
                            }
                            else
                            {
                                //string numbreIndependentTagPath = "M_Path Reinforcement Tag(" + (dimBarras_internos != null ? dimBarras_internos.nDatos : 5) + ")";
                                string numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)_" + _view.ObtenerEscalaPAraPAthSimbol();
                                DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);

                            }
                        
                            // doc.Regenerate();
                            trans.Commit();
                            //   uidoc.RefreshActiveView();
                        } // fin trans
                    }
                    catch (Exception ex)
                    {

                        Util.ErrorMsg($"CrearBarra  -->   ex {ex.Message}    Log:{LogNH._sbuilder.ToString()}");
                        statusbarra = Result.Failed;
                        return Result.Failed;
                    }

                    OcultarYcopiarParametrosPAth();

                    transGroup.Assimilate();

                    return Result.Succeeded;

                }// fin trasn group 


            }


            return Result.Succeeded;
        }

        private void OcultarYcopiarParametrosPAth()
        {
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Ocultando Y parametrospath Path");

                    //verifica si pathsymbol esta corrido
                    if (_PathReinSpanSymbol.TagHeadPosition.AsignarZ(0).DistanceTo(ptoConMouseEnlosa1.AsignarZ(0)) > 0.1)
                    {
                        if (ptoConMouseEnlosa1 != null) _PathReinSpanSymbol.TagHeadPosition = ptoConMouseEnlosa1;
                    }

                    if (!m_createdPathReinforcement.IsValidObject) return;
                    OcultarBarras _OcultarBarras = new OcultarBarras(_doc);
                    _OcultarBarras.Ocultar1BarraCreada_AgregarParametroARebarSystem(m_createdPathReinforcement, Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo), IsConTransaccion: false);

                    // AyudaRebarSystem.AgregarParametroRebarSystem_sinTrans(m_createdPathReinforcement, _NombreVista, Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo));
                    // AgergarParametroRebarSystem(m_createdPathReinforcement, Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo));

                    ITagF newITagF = FactoryITagF_.ObtenerICasoyBarraX(_doc, m_createdPathReinforcement, _solicitudBarraDTO.TipoBarra);
                    newITagF.Ejecutar();

                    trans2.Commit();
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
            }
        }


        //tb modificar
        /*
           ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion 
       EditarPathReinMouse_ExtederPathA2punto.RedimensionarVectores
             */
        private IList<Curve> RedondearRecorrido(List<Curve> curvesPathreiforment_, DatosNuevaBarraDTO datosNuevaBarraDTO)
        {
            if (DatosDiseño.IS_PATHREIN_AJUSTADO == false) return curvesPathreiforment_;

            XYZ p1 = curvesPathreiforment_[0].GetPoint2(0);
            XYZ p2 = curvesPathreiforment_[0].GetPoint2(1);
            XYZ Vector = (p2 - p1).Normalize();


            double cantidadBArras = Math.Round((datosNuevaBarraDTO.LargoRecorridoFoot) / datosNuevaBarraDTO.EspaciamientoFoot, 5);

            if (cantidadBArras < 1) return curvesPathreiforment_;

            double parteDecimal = Util.ParteDecimal(cantidadBArras);

            long CantidadBarra = (long)cantidadBArras;

            if (parteDecimal < ConstNH.CONST_FACTOR_DISMINUIR_1BARRA) CantidadBarra -= 1;

            if (CantidadBarra < 1)
                CantidadBarra = 1;

            double largoREcorridoborrar = datosNuevaBarraDTO.LargoRecorridoFoot - (CantidadBarra * datosNuevaBarraDTO.EspaciamientoFoot + Util.MmToFoot(datosNuevaBarraDTO.DiametroMM));

            p1 = p1 + Vector * largoREcorridoborrar / 2;
            p2 = p2 - Vector * largoREcorridoborrar / 2;

            MoverPtoTagSicorresponde(Vector, largoREcorridoborrar);

            Line nuevaline1 = Line.CreateBound(p1, p2);

            return new List<Curve>() { nuevaline1 };
        }

        private void MoverPtoTagSicorresponde(XYZ Vector, double largoREcorridoborrar)
        {
            List<XYZ> ListaPtosPerimetroBarrasCorregida = new List<XYZ>();
            ListaPtosPerimetroBarrasCorregida.Add(ListaPtosPerimetroBarras[0] + Vector * largoREcorridoborrar / 2);
            ListaPtosPerimetroBarrasCorregida.Add(ListaPtosPerimetroBarras[1] - Vector * largoREcorridoborrar / 2);
            ListaPtosPerimetroBarrasCorregida.Add(ListaPtosPerimetroBarras[2] - Vector * largoREcorridoborrar / 2);
            ListaPtosPerimetroBarrasCorregida.Add(ListaPtosPerimetroBarras[3] + Vector * largoREcorridoborrar / 2);
            List<List<XYZ>> listInLiits = new List<List<XYZ>>();
            listInLiits.Add(ListaPtosPerimetroBarrasCorregida);
            if (IsDentroPoligono.Probar_Si_punto_alInterior_Polilinea(ptoConMouseEnlosa1, listInLiits) == false)
            {
                XYZ direccionHAciaCenrtol = (ListaPtosPerimetroBarrasCorregida[1] - ListaPtosPerimetroBarrasCorregida[0]).Normalize();
                double distancia = ListaPtosPerimetroBarrasCorregida[0].DistanceTo(ListaPtosPerimetroBarrasCorregida[1]) / 2;
                XYZ PtoEnlinea1_4 = Line.CreateBound(ListaPtosPerimetroBarrasCorregida[0].AsignarZ(0), ListaPtosPerimetroBarrasCorregida[3].AsignarZ(0)).ProjectExtendidaXY0(ptoConMouseEnlosa1.AsignarZ(0)).
                                        AsignarZ(ptoConMouseEnlosa1.Z);
                ptoConMouseEnlosa1 = PtoEnlinea1_4 + direccionHAciaCenrtol * distancia;
                //ptoConMouseEnlosa1 =    new XYZ(ListaPtosPerimetroBarrasCorregida.Average(r => r.X),
                //                             ListaPtosPerimetroBarrasCorregida.Average(r => r.Y),
                //                             ListaPtosPerimetroBarrasCorregida.Average(r => r.Z));
                _seleccionarLosaBarraRoom.PtoConMouseEnlosa1 = ptoConMouseEnlosa1;
            }
        }

        public Result CrearBarra_1Trans(IList<Curve> curvesPathreiforment_, double LargoPathreiforment_, string nombreSimboloPathReinforcement_, int diametro_, double espaciamiento_, XYZ desplazamientoPathReinSpanSymbol)
        {



            if (XYZ.Zero.DistanceTo(desplazamientoPathReinSpanSymbol) > 0.1)
            {
                ptoConMouseEnlosa1 = desplazamientoPathReinSpanSymbol;
                desplazamientoPathReinSpanSymbol = XYZ.Zero;
            }

            if (!VerificarDatos(_datosNuevaBarraDTO)) return Result.Failed;

            try
            {

                try
                {
                    using (Transaction trans = new Transaction(_doc))
                    {
                        trans.Start("CreatePathReinforcement-NH");
                        //crea  refuerzo path
                        //flip true se dibuja hacia abajo de la curva de trayectoria

                        //flip false se dibuja hacia arriba de la curva de trayectoria
                        bool aux_flop = false;
                        if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                        {
                            //nada
                        }
                        else if (TipoOrientacion == TipoOrientacionBarra.Vertical)
                        {
                            if (Angle_pelotaLosa1Grado > 90)
                                aux_flop = true;
                        }



                        //**************************************************************************************************************************************
                        //1)crea el pathreinformet 
                        curvesPathreiforment_ = RedondearRecorrido(curvesPathreiforment_.ToList(), _datosNuevaBarraDTO);
                        m_createdPathReinforcement = CreatePathReinforcement(aux_flop, LosaSeleccionada1, curvesPathreiforment_, diametro_);

                        if (m_createdPathReinforcement == null)
                        {
                            TiposPathReinformentSymbolElement.pathReinforcementTypeId = null;
                            trans.RollBack();
                            return statusbarra = Result.Failed;
                        }

                        //Element paraElem = m_createdPathReinforcement as Element;// _doc.GetElement(m_createdPathReinforcement.Id);

                        AgregarParameterShared(m_createdPathReinforcement);

                        if (m_createdPathReinforcement == null)
                        {
                            //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                            message = "NO se pudo crear m_createdPathReinforcement";
                            trans.RollBack();
                            return statusbarra = Result.Failed; ;
                        }

                        //CONFIGURACION DE PATHREINFORMENT
                        if (view3D_Visualizar != null)
                        {
                            //permite que la barra se vea en el 3d como solido
                            m_createdPathReinforcement.SetSolidInView(view3D_Visualizar, true);
                            //permite que la barra se vea en el 3d como sin interferecnias 
                            m_createdPathReinforcement.SetUnobscuredInView(view3D_Visualizar, true);
                        }

                        // Largo de princial Alternativa. Si es 0 no la activa
                        double largoPrincial = LargoPathreiforment_;
                        // Largo de barra Alternativa. Si es 0 no la activa 
                        double largoAlternative = LargoPathreiforment_;

                        // a) seleciona que barra se botton - inferior
                        //b) activa barra alternativa de ser necesario
                        //c) asigna largo de barra princiapales y alterniva si corresonde
                        //b) asigna largos              

                        if (TipoBarraStr.Contains("f16"))
                        {
                            ICasoBarra iTipoBarra = new Barra16(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                            iTipoBarra.LayoutRebar_PathReinforcement();
                        }
                        else if (TipoBarraStr.Contains("f17"))
                        {
                            ICasoBarra iTipoBarra = new BarraF17(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                            iTipoBarra.LayoutRebar_PathReinforcement();
                        }
                        else if (TipoBarraStr.Contains("f18"))
                        {
                            ICasoBarra iTipoBarra = new BarraF18(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                            iTipoBarra.LayoutRebar_PathReinforcement();
                        }
                        else if (TipoBarraStr.Contains("f19"))
                        {
                            ICasoBarra iTipoBarra = new BarraF19(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                            iTipoBarra.LayoutRebar_PathReinforcement();
                        }
                        else if (TipoBarraStr.Contains("f20"))
                        {
                            ICasoBarra iTipoBarra = new BarraF20(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                            iTipoBarra.LayoutRebar_PathReinforcement();
                        }
                        else if (TipoBarraStr.Contains("f21"))
                        {
                            ICasoBarra iTipoBarra = new Barra11(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                            iTipoBarra.LayoutRebar_PathReinforcement();
                        }
                        else if (TipoBarraStr.Contains("f22"))
                        {
                            ICasoBarra iTipoBarra = new BarraF22(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                            iTipoBarra.LayoutRebar_PathReinforcement();
                        }
                        else
                        {
                            // LayoutRebar_PathReinforcement( largoPrincial, largoAlternative, this.ubicacionEnlosa, IsLuzSecuandiria, espaciamiento_);
                            LayoutRebar_PathReinforcement(_solicitudBarraDTO, _datosNuevaBarraDTO);
                        }

                        //  CODIGO NO IMPLEMNETADO Metodod que obtienen las dimensiones de  Pathreinformnen para porder dibujar el symbolo de la barra
                        //if (1 == 2)
                        //{
                        //    DimensionesBarras DImensionesParaSymbolo = ObtenerDImensionesSimbolos(m_createdPathReinforcement, dimBarras, TipoBarra);
                        //}

                        //**************************************************************************************************************************************************************************
                        //2)Family fam = TiposFamilyRebar.getFamilyRebarShape("Structural Path Reinforcement 3", doc);
                        //obtiene elemento dentos de la biblioteca de familia del PathReinforcementSymbol
                        string scala = _view.ObtenerEscalaPAraPAthSimbol().ToString(); //ConstNH.CONST_ESCALA_BASE.ToString();// view.Scale.ToString();
                        if (IsTest) scala = "100";
                        //Element elemtoSymboloPath = TiposPathReinSpanSymbol.getPathReinSpanSymbol(_datosNuevaBarraDTO.nombreSimboloPathReinforcement, scala, _doc);
                        Element elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(_datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + scala, _doc);

                        // ParameterUtil.SetParaInt(elemtoSymboloPath, "A_", Util.CmToFoot(100 / 100));
                        if (elemtoSymboloPath == null)
                        {
                            //   _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                            Util.ErrorMsg($"NO se pudo  encontar PathSymbol :{_datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + scala}   ");
                            trans.RollBack();
                            return statusbarra = Result.Failed;
                        }

                        // busca el elemnto 'Arrow Filled 20 Degree' que es la flecha del recorrido una un metodo muy generico tratar de mejorar
                        // List<Element> listaArrow = Tipos_Arrow.FindAllArrowheads(_doc, "Arrow Filled 20 Degree");
                        Element _Arrow = Tipos_Arrow.ObtenerPrimerArrowheads(_doc, "Arrow Filled 20 Degree");
                        if (_Arrow != null)
                        { //asigna el tipo de flecha de recorrido al symbolo del pathrebar
                            ParameterUtil.SetParaElementId(elemtoSymboloPath, BuiltInParameter.LEADER_ARROWHEAD, _Arrow.Id);
                        }
                        else
                        { Util.ErrorMsg("'Arrow Filled 20 Degree' NO encontrado "); }


                        // 2.1)crea el symbolo con la forma de la barra
                        //double x = 0;// Util.CmToFoot(10);
                        //double y = 0; //Util.CmToFoot(10);
                        _PathReinSpanSymbol = PathReinSpanSymbol.Create(_doc, _view.Id, new LinkElementId(m_createdPathReinforcement.Id), desplazamientoPathReinSpanSymbol, elemtoSymboloPath.Id);
                        if (ptoConMouseEnlosa1 != null) _PathReinSpanSymbol.TagHeadPosition = ptoConMouseEnlosa1;
                        if (TipoBarraStr.EsRefuerzoSUperiorSx())
                            _PathReinSpanSymbol.TagHeadPosition = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoConMouseEnlosa1,
                                                                                                     Util.GradosToRadianes(AnguloBordeRoomYSegundoPtoMouseGrado)
                                                                                                     , largoPrincial * 0.15);

                        ParameterUtil.SetParaInt(_PathReinSpanSymbol, "A_r2", Util.CmToFoot(100 / 100));
                        // ParameterUtil.SetParaInt(symbolPath, "B_sy", Util.CmToFoot(100 / 100));
                        if (_PathReinSpanSymbol == null)
                        {
                            //    _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                            message = "NO se pudo crear Path Symbol";
                            trans.RollBack();
                            return statusbarra = Result.Failed;
                        }

                        //**************************************************************************************************************************************************************************
                        //3) crea el tag con la cuentia de las barra nnv
                        //if (TipoBarra.EsRefuerzoSUperiorSx())
                        //{
                        //    //string numbreIndependentTagPath = "M_Path Reinforcement Tag(" + (dimBarras_internos != null ? dimBarras_internos.nDatos : 5) + ")";
                        //    string numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_" + view.Scale;
                        //    DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, trans, numbreIndependentTagPath);
                        //    numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_L_" + view.Scale;
                        //    DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, trans, numbreIndependentTagPath);
                        //}


                        //else 
                        if (_listaTAgBArra.M4_IsFAmiliaValida())
                        {
                            foreach (TagBarra item in _listaTAgBArra.listaTag)
                            {
                                if (item.IsOk)
                                {
                                    if (!DibujarTagPathReinforment(item, desplazamientoPathReinSpanSymbol))
                                    {
                                        message = "NO se pudo obtener el TAg de PathReinforcement";
                                        Debug.WriteLine($"ex {message}");
                                        Util.ErrorMsg($"CrearBarra  -->   ex {message}");
                                        trans.RollBack();
                                        return statusbarra = Result.Failed;
                                    }
                                }
                            }
                        }
                        else if (_datosNuevaBarraDTO.IsBarrAlternative)
                        {
                            string numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)B_InfIzq_" + _view.ObtenerNombre_EscalaConfiguracion();
                            DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);
                            numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)B_DereSup_" + _view.ObtenerNombre_EscalaConfiguracion();
                            DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);
                        }
                        else
                        {
                            //string numbreIndependentTagPath = "M_Path Reinforcement Tag(" + (dimBarras_internos != null ? dimBarras_internos.nDatos : 5) + ")";
                            string numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)_" + _view.ObtenerNombre_EscalaConfiguracion();
                            DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);
                            //  string numbreIndependentTagPath2 = "M_Path Reinforcement Tag(ID_cuantia_largo)_45_" + view.Scale;
                            // DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, trans, numbreIndependentTagPath2);
                        }
                        // doc.Regenerate();
                        trans.Commit();
                        //   uidoc.RefreshActiveView();
                    } // fin trans
                }
                catch (Exception ex)
                {
                    // _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                    Debug.WriteLine($"ex {ex.Message}");
                    Util.ErrorMsg($"CrearBarra  -->   ex {ex.Message}");
                    string message = ex.Message;
                    statusbarra = Result.Failed;
                    message = "Error al crear Path Symbol";
                    return Result.Failed;
                }


                //OcultarBarras _OcultarBarras = new OcultarBarras(_doc);
                //_OcultarBarras.Ocultar1BarraCreada_AgregarParametroARebarSystem(m_createdPathReinforcement, IsConTransaccion: true);
                OcultarYcopiarParametrosPAth();

            }
            catch (Exception ex)
            {

                Debug.WriteLine($"ex {ex.Message}");
                Util.ErrorMsg($"CrearBarra  -->   ex {ex.Message}");
            }

            return Result.Succeeded;
        }



        public Result CrearBarraAuto(IList<Curve> curvesPathreiforment_, double LargoPathreiforment_, string nombreSimboloPathReinforcement_, int diametro_, double espaciamiento_, XYZ desplazamientoPathReinSpanSymbol)
        {
            Stopwatch timeMeasure = Stopwatch.StartNew();


            if (XYZ.Zero.DistanceTo(desplazamientoPathReinSpanSymbol) > 0.1)
            {
                ptoConMouseEnlosa1 = desplazamientoPathReinSpanSymbol;
                desplazamientoPathReinSpanSymbol = XYZ.Zero;
            }

            if (!VerificarDatos(_datosNuevaBarraDTO)) return Result.Failed;
            //   _uiapp.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);

            if (Tiporefuerzo == TipoRefuerzo.PathRefuerza && ListaPtosPerimetroBarras.Count > 0)
            {
                //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);

                try
                {
                    //crea  refuerzo path
                    //flip true se dibuja hacia abajo de la curva de trayectoria

                    //flip false se dibuja hacia arriba de la curva de trayectoria
                    bool aux_flop = false;
                    if (TipoOrientacion == TipoOrientacionBarra.Horizontal)
                    {
                        //nada
                    }
                    else if (TipoOrientacion == TipoOrientacionBarra.Vertical)
                    {
                        if (Angle_pelotaLosa1Grado > 90)
                            aux_flop = true;
                    }
                    //**************************************************************************************************************************************
                    //1)crea el pathreinformet 
                    curvesPathreiforment_ = RedondearRecorrido(curvesPathreiforment_.ToList(), _datosNuevaBarraDTO);
                    m_createdPathReinforcement = CreatePathReinforcement(aux_flop, LosaSeleccionada1, curvesPathreiforment_, diametro_);
                    if (m_createdPathReinforcement == null)
                    {
                        TiposRebarBarType.ListaFamilias = new Dictionary<string, RebarBarType>();
                        TiposPathReinformentSymbolElement.pathReinforcementTypeId = null;
                        return statusbarra = Result.Failed;
                    }

                    //Element paraElem = m_createdPathReinforcement as Element;// _doc.GetElement(m_createdPathReinforcement.Id);

                    AgregarParameterShared(m_createdPathReinforcement);
                    if (m_createdPathReinforcement == null)
                    {
                        //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                        message = "NO se pudo crear m_createdPathReinforcement";
                        Util.ErrorMsg(message);
                        return statusbarra = Result.Failed; ;
                    }

                    //CONFIGURACION DE PATHREINFORMENT
                    if (view3D_Visualizar != null)
                    {
                        //permite que la barra se vea en el 3d como solido
                        m_createdPathReinforcement.SetSolidInView(view3D_Visualizar, true);
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        m_createdPathReinforcement.SetUnobscuredInView(view3D_Visualizar, true);
                    }

                    // Largo de princial Alternativa. Si es 0 no la activa
                    double largoPrincial = LargoPathreiforment_;
                    // Largo de barra Alternativa. Si es 0 no la activa 
                    double largoAlternative = LargoPathreiforment_;

                    // a) seleciona que barra se botton - inferior
                    //b) activa barra alternativa de ser necesario
                    //c) asigna largo de barra princiapales y alterniva si corresonde
                    //b) asigna largos              

                    if (TipoBarraStr.Contains("f16"))
                    {
                        ICasoBarra iTipoBarra = new Barra16(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                        iTipoBarra.LayoutRebar_PathReinforcement();
                    }
                    else if (TipoBarraStr.Contains("f17"))
                    {
                        ICasoBarra iTipoBarra = new BarraF17(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                        iTipoBarra.LayoutRebar_PathReinforcement();
                    }
                    else if (TipoBarraStr.Contains("f18"))
                    {
                        ICasoBarra iTipoBarra = new BarraF18(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                        iTipoBarra.LayoutRebar_PathReinforcement();
                    }
                    else if (TipoBarraStr.Contains("f19"))
                    {
                        ICasoBarra iTipoBarra = new BarraF19(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                        iTipoBarra.LayoutRebar_PathReinforcement();
                    }
                    else if (TipoBarraStr.Contains("f20"))
                    {
                        ICasoBarra iTipoBarra = new BarraF20(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                        iTipoBarra.LayoutRebar_PathReinforcement();
                    }
                    else if (TipoBarraStr.Contains("f21"))
                    {
                        ICasoBarra iTipoBarra = new Barra11(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                        iTipoBarra.LayoutRebar_PathReinforcement();
                    }
                    else if (TipoBarraStr.Contains("f22"))
                    {
                        ICasoBarra iTipoBarra = new BarraF22(this, _solicitudBarraDTO, _datosNuevaBarraDTO);
                        iTipoBarra.LayoutRebar_PathReinforcement();
                    }
                    else
                    {
                        // LayoutRebar_PathReinforcement( largoPrincial, largoAlternative, this.ubicacionEnlosa, IsLuzSecuandiria, espaciamiento_);
                        LayoutRebar_PathReinforcement(_solicitudBarraDTO, _datosNuevaBarraDTO);
                    }

                    //**************************************************************************************************************************************************************************
                    //2)Family fam = TiposFamilyRebar.getFamilyRebarShape("Structural Path Reinforcement 3", doc);
                    //obtiene elemento dentos de la biblioteca de familia del PathReinforcementSymbol
                    string scala = _view.ObtenerEscalaPAraPAthSimbol().ToString();// ConstNH.CONST_ESCALA_BASE.ToString();// view.Scale.ToString();
                    if (IsTest) scala = "100";
                    //Element elemtoSymboloPath = TiposPathReinSpanSymbol.getPathReinSpanSymbol(_datosNuevaBarraDTO.nombreSimboloPathReinforcement, scala, _doc);
                    Element elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(_datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + scala, _doc);

                    // ParameterUtil.SetParaInt(elemtoSymboloPath, "A_", Util.CmToFoot(100 / 100));
                    if (elemtoSymboloPath == null)
                    {
                        //   _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                        message = "NO se pudo  encontar Path Symbol en libreria";

                        return statusbarra = Result.Failed;
                    }
                    // busca el elemnto 'Arrow Filled 20 Degree' que es la flecha del recorrido una un metodo muy generico tratar de mejorar
                    // List<Element> listaArrow = Tipos_Arrow.FindAllArrowheads(_doc, "Arrow Filled 20 Degree");
                    Element _Arrow = Tipos_Arrow.ObtenerPrimerArrowheads(_doc, "Arrow Filled 20 Degree");
                    if (_Arrow != null)
                    { //asigna el tipo de flecha de recorrido al symbolo del pathrebar
                        ParameterUtil.SetParaElementId(elemtoSymboloPath, BuiltInParameter.LEADER_ARROWHEAD, _Arrow.Id);
                    }
                    else
                    { Util.ErrorMsg("'Arrow Filled 20 Degree' NO encontrado "); }

                    // 2.1)crea el symbolo con la forma de la barra
                    //double x = 0;// Util.CmToFoot(10);
                    //double y = 0; //Util.CmToFoot(10);
                    _PathReinSpanSymbol = PathReinSpanSymbol.Create(_doc, _view.Id, new LinkElementId(m_createdPathReinforcement.Id), desplazamientoPathReinSpanSymbol, elemtoSymboloPath.Id);
                    if (ptoConMouseEnlosa1 != null) _PathReinSpanSymbol.TagHeadPosition = ptoConMouseEnlosa1;
                    if (TipoBarraStr.EsRefuerzoSUperiorSx())
                        _PathReinSpanSymbol.TagHeadPosition = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoConMouseEnlosa1,
                                                                                                 Util.GradosToRadianes(AnguloBordeRoomYSegundoPtoMouseGrado)
                                                                                                 , largoPrincial * 0.15);

                    ParameterUtil.SetParaInt(_PathReinSpanSymbol, "A_r2", Util.CmToFoot(100 / 100));
                    // ParameterUtil.SetParaInt(symbolPath, "B_sy", Util.CmToFoot(100 / 100));
                    if (_PathReinSpanSymbol == null)
                    {
                        //    _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                        message = "NO se pudo crear Path Symbol";

                        return statusbarra = Result.Failed;
                    }

                    //**************************************************************************************************************************************************************************

                    //else 
                    if (_listaTAgBArra.M4_IsFAmiliaValida())
                    {
                        foreach (TagBarra item in _listaTAgBArra.listaTag)
                        {
                            if (item.IsOk)
                            {
                                if (!DibujarTagPathReinforment(item, desplazamientoPathReinSpanSymbol))
                                {
                                    message = "NO se pudo obtener el TAg de PathReinforcement";
                                    Debug.WriteLine($"ex {message}");
                                    Util.ErrorMsg($"CrearBarra  -->   ex {message}");

                                    return statusbarra = Result.Failed;
                                }
                            }
                        }
                    }
                    else if (_datosNuevaBarraDTO.IsBarrAlternative)
                    {
                        string numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)B_InfIzq_" + _view.ObtenerEscalaPAraPAthSimbol();
                        DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);
                        numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)B_DereSup_" + _view.ObtenerEscalaPAraPAthSimbol();
                        DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);
                    }
                    else
                    {
                        //string numbreIndependentTagPath = "M_Path Reinforcement Tag(" + (dimBarras_internos != null ? dimBarras_internos.nDatos : 5) + ")";
                        string numbreIndependentTagPath = "M_Path Reinforcement Tag(ID_cuantia_largo)_" + _view.ObtenerEscalaPAraPAthSimbol();
                        DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, numbreIndependentTagPath);
                        //  string numbreIndependentTagPath2 = "M_Path Reinforcement Tag(ID_cuantia_largo)_45_" + view.Scale;
                        // DibujarTagPathReinforment(desplazamientoPathReinSpanSymbol, trans, numbreIndependentTagPath2);
                    }

                    // doc.Regenerate();

                }
                catch (Exception ex)
                {
                    // _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                    Debug.WriteLine($"ex {ex.Message}");
                    Util.ErrorMsg($"CrearBarra  -->   ex {ex.Message}");
                    string message = ex.Message;
                    statusbarra = Result.Failed;
                    message = "Error al crear Path Symbol";
                    return Result.Failed;
                }


                //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);

                return Result.Succeeded;


            }
            else
            {
                Debug.WriteLine("Error en CrearBarraAuto  ->  ListaPtosPerimetroBarras.Count==0");
                Util.ErrorMsg("Error en CrearBarraAuto  ->  ListaPtosPerimetroBarras.Count==0");
                return Result.Succeeded;
            }
        }


        private void AgregarParameterShared(Element paraElem)
        {
            //agrega parametro para poder ver imagens en el resumen de barrasutiliza el
            //listaImagenes : diccionario
            //TipoBarra :tipo de barra q obtiene la ruta de la imagene en el diccionario

            //"Image" : nombre del parametro
            // if (barraRoomPersis.listaImagenes.ContainsKey(TipoBarra)) ParameterUtil.SetParaInt(paraElem, "Image", ImageType.Create(doc, barraRoomPersis.listaImagenes[TipoBarra]));
            dimBarras_parameterSharedLetras = _datosNuevaBarraDTO.dimBarras_parameterSharedLetras;
            if (dimBarras_parameterSharedLetras != null)
            {
                if (dimBarras_parameterSharedLetras.a.IsOk == true && ParameterUtil.FindParaByName(paraElem, "A_") != null) ParameterUtil.SetParaInt(paraElem, "A_", dimBarras_parameterSharedLetras.a.valor);
                if (dimBarras_parameterSharedLetras.b.IsOk == true && ParameterUtil.FindParaByName(paraElem, "B_") != null) ParameterUtil.SetParaInt(paraElem, "B_", dimBarras_parameterSharedLetras.b.valor);
                if (dimBarras_parameterSharedLetras.c.IsOk == true && ParameterUtil.FindParaByName(paraElem, "C_") != null) ParameterUtil.SetParaInt(paraElem, "C_", dimBarras_parameterSharedLetras.c.valor);
                if (dimBarras_parameterSharedLetras.c2.IsOk == true && ParameterUtil.FindParaByName(paraElem, "C2_") != null) ParameterUtil.SetParaInt(paraElem, "C2_", dimBarras_parameterSharedLetras.c2.valor);
                if (dimBarras_parameterSharedLetras.d.IsOk == true && ParameterUtil.FindParaByName(paraElem, "D_") != null) ParameterUtil.SetParaInt(paraElem, "D_", dimBarras_parameterSharedLetras.d.valor);
                if (dimBarras_parameterSharedLetras.e.IsOk == true && ParameterUtil.FindParaByName(paraElem, "E_") != null) ParameterUtil.SetParaInt(paraElem, "E_", dimBarras_parameterSharedLetras.e.valor);
                if (dimBarras_parameterSharedLetras.g.IsOk == true && ParameterUtil.FindParaByName(paraElem, "G_") != null) ParameterUtil.SetParaInt(paraElem, "G_", dimBarras_parameterSharedLetras.g.valor);
                if (dimBarras_parameterSharedLetras.Largo2.IsOk == true && ParameterUtil.FindParaByName(paraElem, "L2barra") != null) ParameterUtil.SetParaInt(paraElem, "L2barra", dimBarras_parameterSharedLetras.Largo2.valor);
                if (dimBarras_parameterSharedLetras.Largo2.IsOk == true && ParameterUtil.FindParaByName(paraElem, "EspesorCambio") != null) ParameterUtil.SetParaInt(paraElem, "EspesorCambio", dimBarras_parameterSharedLetras.LetrasCambiosEspesor);
                // if (dimBarras_internos != null) ParameterUtil.SetParaInt(paraElem, "LARGO_", dimBarras_internos.Largo.valor);
            }


            _NombreVista = _view.ObtenerNombreIsDependencia();
            if (_view != null && ParameterUtil.FindParaByName(paraElem, "NombreVista") != null)
            {
                ParameterUtil.SetParaInt(paraElem, "NombreVista", _NombreVista);
            }//"nombre de vista"}

            if (_datosNuevaBarraDTO.Prefijo_cuantia != "")
            {
                ParameterUtil.SetParaStringNH(paraElem, "Prefijo_F", _datosNuevaBarraDTO.Prefijo_cuantia);
            }

            if (TipoDireccionBarra_ != TipoDireccionBarra.NONE)
            {
                string text = ((TipoDireccionBarra_ == TipoDireccionBarra.Primaria) ? ConstNH.NOMBRE_BARRA_PRINCIPAL : ConstNH.NOMBRE_BARRA_SECUNADARIA);
                if (ParameterUtil.FindParaByName(paraElem, "TipoDireccionBarra") != null)
                    ParameterUtil.SetParaInt(paraElem, "TipoDireccionBarra", text);  //"nombre de vista"
            }
            if (ParameterUtil.FindParaByName(paraElem, "IDNumero") != null) ParameterUtil.SetParaInt(paraElem, "IDNumero", contador);
            if (ParameterUtil.FindParaByName(paraElem, "IDTipo") != null) ParameterUtil.SetParaInt(paraElem, "IDTipo", TipoBarraStr);
            if (ParameterUtil.FindParaByName(paraElem, "IDTipoDireccion") != null) ParameterUtil.SetParaInt(paraElem, "IDTipoDireccion", ubicacionEnlosa.ToString());
            if (ParameterUtil.FindParaByName(paraElem, "EsPrincipal") != null) ParameterUtil.SetParaInt(paraElem, "EsPrincipal", (_datosNuevaBarraDTO.IsLuzSecuandiria == false ? 1 : 0));

            if (ParameterUtil.FindParaByName(paraElem, "BarraTipo") != null && _BarraTipo != TipoRebar.NONE)
            {
                string BarraTTipo = Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo);
                ParameterUtil.SetParaInt(paraElem, "BarraTipo", BarraTTipo);  //"nombre de vista"
            }


        }

        private bool VerificarDatos(DatosNuevaBarraDTO datosNuevaBarraDTO)
        {
            if (datosNuevaBarraDTO.nombreFamiliaRebarShape == "")
            {
                Util.ErrorMsg($"sin nombreFamiliaRebarShape");
                return false;
            }

            if (datosNuevaBarraDTO.tipoRebarShapePrincipal == null)
            {
                Util.ErrorMsg($"sin tipoRebarShapePrincipal : \n NombreRebarShape {datosNuevaBarraDTO.nombreFamiliaRebarShape} " +
                                                             $"\n TipoRebarShapePrincipal  {datosNuevaBarraDTO.tipoRebarShapePrincipal}");
                return false;
            }
            if (datosNuevaBarraDTO.LargoPathreiforment < Util.CmToFoot(10))
            {
                Util.ErrorMsg($"LargoPathreiforment valor min (10cm) --> {datosNuevaBarraDTO.LargoPathreiforment}");
                return false;
            }
            if (datosNuevaBarraDTO.EspaciamientoFoot < Util.CmToFoot(5))
            {
                Util.ErrorMsg($"Espaciamiento de barra menor a 5cb --> {datosNuevaBarraDTO.EspaciamientoFoot}");
                return false;
            }

            return true;
        }

        private bool DibujarTagPathReinforment(XYZ desplazamientoPathReinSpanSymbol, string nombreIndependentTagPath)
        {

            Element IndependentTagPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreIndependentTagPath, _doc);
            if (IndependentTagPath == null) { return false; }
            IndependentTag independentTag = IndependentTag.Create(_doc, IndependentTagPath.Id, _view.Id, new Reference(m_createdPathReinforcement), false,
                                                       (TipoOrientacion == TipoOrientacionBarra.Horizontal ? TagOrientation.Horizontal : TagOrientation.Vertical), desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            if (ptoConMouseEnlosa1 != null) independentTag.TagHeadPosition = ptoConMouseEnlosa1;
            return true;
            //independentTag.
        }
        private bool DibujarTagPathReinforment(TagBarra tagBarra, XYZ desplazamientoPathReinSpanSymbol)
        {
           if (!tagBarra.IsOk) return false;
            IndependentTag independentTag = IndependentTag.Create(_doc, tagBarra.ElementIndependentTagPath.Id, _view.Id, new Reference(m_createdPathReinforcement), false,
                                                      TagOrientation.Horizontal, desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)

            if (independentTag == null) return false;
            if (ptoConMouseEnlosa1 != null) independentTag.TagHeadPosition = tagBarra.posicion;
            return true;
        }

        public Result CrearBarraExtremos()
        {

            try
            {
                m_createdPathReinforcement_dere = null;
                m_createdPathReinforcement_izq = null;

                BarraRoomF1_Sup BarraRoomF1_Sup = new BarraRoomF1_Sup(this.ListaPtosPerimetroBarras, this.largoMin_1, this.diametroEnMM);
                BarraRoomF1_Sup.CalcularPAth_F1_SUP();
                BarraRoom newBarralosa_dere = null;
                BarraRoom newBarralosa_izq = null;

                try
                {


                    //lado 2) izq - inferior
                    if (this.TipoBarra_izq_Inf != "")
                    {

                        BarraRoomDTO barraRoomDTO_izq = new BarraRoomDTO()
                        {
                            diametroEnMM = this.diametroEnMM,
                            LosaSeleccionada1 = this.LosaSeleccionada1,
                            EspesorLosaCm_1 = this.EspesorLosaCm_1,
                            Espaciamiento = this.Espaciamiento,
                            TipoBarraStr = this.TipoBarra_izq_Inf,
                            ubicacionEnlosa = (ubicacionEnlosa == UbicacionLosa.Izquierda ? UbicacionLosa.Izquierda : UbicacionLosa.Inferior),
                            _TipoRebar = TipoRebar.LOSA_SUP_S4,
                            Angle_pelotaLosa1Grado = Angle_pelotaLosa1Grado,
                            Prefijo_cuantia = _datosNuevaBarraDTO.Prefijo_cuantia
                        };

                        newBarralosa_izq = new BarraRoom(_uiapp, barraRoomDTO_izq,
                                                                  BarraRoomF1_Sup.ListaPtosPerimetroBarrasIzqINf, this.CurvesPathreiforment_IzqInf, this.ptoConMouseEnlosaF1_SUPIzqINf);

                        if (newBarralosa_izq.statusbarra == Result.Succeeded)
                        {
                            newBarralosa_izq.BUscarViewSUPERIOR();
                            newBarralosa_izq.CrearBarra(newBarralosa_izq.CurvesPathreiforment,
                                                        newBarralosa_izq.LargoPathreiforment,
                                                        newBarralosa_izq.nombreSimboloPathReinforcement,
                                                        newBarralosa_izq.diametroEnMM,
                                                        newBarralosa_izq.Espaciamiento,
                                                        ptoConMouseEnlosaF1_SUPIzqINf);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"barra izq  ex:{ex.Message}");
                    Util.ErrorMsg($"barra dere  ex:{ex.Message}");
                }


                try
                {


                    //lado 3) Dere -sup
                    if (this.TipoBarra_dere_sup != "")
                    {
                        BarraRoomDTO barraRoomDTO_dere = new BarraRoomDTO()
                        {
                            diametroEnMM = this.diametroEnMM,
                            LosaSeleccionada1 = this.LosaSeleccionada1,
                            EspesorLosaCm_1 = this.EspesorLosaCm_1,
                            Espaciamiento = this.Espaciamiento,
                            TipoBarraStr = this.TipoBarra_dere_sup,
                            ubicacionEnlosa = (ubicacionEnlosa == UbicacionLosa.Derecha ? UbicacionLosa.Derecha : UbicacionLosa.Superior),
                            _TipoRebar = TipoRebar.LOSA_SUP_S4,
                            Angle_pelotaLosa1Grado = Angle_pelotaLosa1Grado,
                            Prefijo_cuantia = _datosNuevaBarraDTO.Prefijo_cuantia

                        };

                        newBarralosa_dere = new BarraRoom(_uiapp,
                                                                   barraRoomDTO_dere,
                                                                   BarraRoomF1_Sup.ListaPtosPerimetroBarrasDereSUp, this.CurvesPathreiforment_DereSup, this.ptoConMouseEnlosaF1_SUPDereSup);

                        if (newBarralosa_dere.statusbarra == Result.Succeeded)
                        {
                            newBarralosa_dere.BUscarViewSUPERIOR();
                            newBarralosa_dere.CrearBarra(newBarralosa_dere.CurvesPathreiforment,
                                          newBarralosa_dere.LargoPathreiforment,
                                          newBarralosa_dere.nombreSimboloPathReinforcement,
                                          newBarralosa_dere.diametroEnMM,
                                          newBarralosa_dere.Espaciamiento,
                                         ptoConMouseEnlosaF1_SUPDereSup);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"barra dere  ex:{ex.Message}");
                    Util.ErrorMsg($"barra dere  ex:{ex.Message}");
                }


                if (IsCasoS4)
                {
                    _PathReinSpanSymbol = (newBarralosa_dere != null ?
                                             newBarralosa_dere._PathReinSpanSymbol :
                                             (newBarralosa_izq != null ? newBarralosa_izq._PathReinSpanSymbol : null));

                }

            }
            catch (Exception ex)
            {

                statusbarra = Result.Failed;
                message = ex.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        public Result CrearBarraExtremos_Auto()
        {
            try
            {
                m_createdPathReinforcement_dere = null;
                m_createdPathReinforcement_izq = null;

                BarraRoomF1_Sup BarraRoomF1_Sup = new BarraRoomF1_Sup(this.ListaPtosPerimetroBarras, this.largoMin_1, this.diametroEnMM);
                BarraRoomF1_Sup.CalcularPAth_F1_SUP();
                try
                {
                    //lado 2) izq - inferior
                    if (this.TipoBarra_izq_Inf != "")
                    {
                        BarraRoomDTO barraRoomDTO_izq = new BarraRoomDTO()
                        {
                            diametroEnMM = this.diametroEnMM,
                            LosaSeleccionada1 = this.LosaSeleccionada1,
                            EspesorLosaCm_1 = this.EspesorLosaCm_1,
                            Espaciamiento = this.Espaciamiento,
                            TipoBarraStr = this.TipoBarra_izq_Inf,
                            ubicacionEnlosa = (ubicacionEnlosa == UbicacionLosa.Izquierda ? UbicacionLosa.Izquierda : UbicacionLosa.Inferior),
                            _TipoRebar = TipoRebar.LOSA_SUP_S4,
                            Prefijo_cuantia = _datosNuevaBarraDTO.Prefijo_cuantia
                        };

                        newBarralosa_izq = new BarraRoom(_uiapp, barraRoomDTO_izq,
                                                                  BarraRoomF1_Sup.ListaPtosPerimetroBarrasIzqINf, BarraRoomF1_Sup.CurvesPathreiforment_IzqInf, this.ptoConMouseEnlosaF1_SUPIzqINf);

                    }
                }
                catch (Exception ex)
                {
                    newBarralosa_izq = null;
                    Debug.WriteLine($"barra izq  ex:{ex.Message}");
                    Util.ErrorMsg($"barra dere  ex:{ex.Message}");
                }

                try
                {

                    //lado 3) Dere -sup
                    if (this.TipoBarra_dere_sup != "")
                    {
                        BarraRoomDTO barraRoomDTO_dere = new BarraRoomDTO()
                        {
                            diametroEnMM = this.diametroEnMM,
                            LosaSeleccionada1 = this.LosaSeleccionada1,
                            EspesorLosaCm_1 = this.EspesorLosaCm_1,
                            Espaciamiento = this.Espaciamiento,
                            TipoBarraStr = this.TipoBarra_dere_sup,
                            ubicacionEnlosa = (ubicacionEnlosa == UbicacionLosa.Derecha ? UbicacionLosa.Derecha : UbicacionLosa.Superior),
                            _TipoRebar = TipoRebar.LOSA_SUP_S4
                        };

                        newBarralosa_dere = new BarraRoom(_uiapp,
                                                                   barraRoomDTO_dere,
                                                                   BarraRoomF1_Sup.ListaPtosPerimetroBarrasDereSUp, BarraRoomF1_Sup.CurvesPathreiforment_DereSup, this.ptoConMouseEnlosaF1_SUPDereSup);
                    }
                }
                catch (Exception ex)
                {
                    newBarralosa_dere = null;
                    Debug.WriteLine($"barra dere  ex:{ex.Message}");
                    Util.ErrorMsg($"barra dere  ex:{ex.Message}");
                }

            }
            catch (Exception ex)
            {

                statusbarra = Result.Failed;
                message = ex.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        public void BuscarDireccion_F1SUP()
        {
            if (ubicacionEnlosa == UbicacionLosa.Izquierda || ubicacionEnlosa == UbicacionLosa.Inferior)
            {
                TipoBarra_izq_Inf = "f1_SUP";
            }
            else
            { TipoBarra_dere_sup = "f1_SUP"; }

        }

        #region 2.2) 3 Metodos Crear PathReinforcement
        /// <summary>
        /// abstract method to create PathReinforcement
        /// </summary>
        /// <returns>new created PathReinforcement</returns>
        /// <param name="points">points used to create PathReinforcement</param>
        /// <param name="flip">used to specify whether new PathReinforcement is Filp  indica si se dibuja 
        /// hacia arriba o abajo de la curva </param>
        public PathReinforcement CreatePathReinforcement(bool flip, Floor m_data, IList<Curve> curvesPathreiforment_, int diametro_)
        {
            //Line curve;
            //IList<Curve> curves = new List<Curve>();
            ////configuracion
            //// 0  - 3
            //// 1 -  2
            //curve = Line.CreateBound(points[0], points[3]);
            //curves.Add(curve);
            ElementId pathReinforcementTypeId = TiposPathReinformentSymbolElement.ObtenerPathReinfDefaul(_doc);

            //1) asigna el tipo de la barra en funcion del diametro, que debe estar creados en la libreria
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametro_, _doc, true);
            if (rebarBarType == null)
            {
                //Util.ErrorMsg($"Error, no se encontro el tipo barra Ø{diametro_}");
                return null;
            }

            Element AUXSS = _doc.GetElement(pathReinforcementTypeId);
            //1.b)asigna RebarHookType defaul
            // ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_document);
            ElementId rebarHookTypeId = ElementId.InvalidElementId;

            //2)asigna RebarBarType defaul
            //ElementId rebarBarTypeId = RebarBarType.CreateDefaultRebarBarType(m_document);
            PathReinforcement result = null;

            try
            {
                bool isInvalidao = pathReinforcementTypeId.IsInvalid();
                bool isValidao = pathReinforcementTypeId.IsValid();

                result = PathReinforcement.Create(_doc, m_data, curvesPathreiforment_, flip, pathReinforcementTypeId, rebarBarType.Id, rebarHookTypeId, rebarHookTypeId);
            }
            catch (Exception)
            {
                if (!m_data.IsEstructural())
                {
                    Util.ErrorMsg($"Elemento seleccionado con ID:{m_data.Id} no esta definido como elemento estructural");
                }


                TiposPathReinformentSymbolElement.pathReinforcementTypeId = null;
                TiposRebarBarType.ListaFamilias = new Dictionary<string, RebarBarType>();
                pathReinforcementTypeId = TiposPathReinformentSymbolElement.ObtenerPathReinfDefaul(_doc);
                result = PathReinforcement.Create(_doc, m_data, curvesPathreiforment_, flip, pathReinforcementTypeId, rebarBarType.Id, rebarHookTypeId, rebarHookTypeId);

            }
            return result;

        }

        public void LayoutRebar_PathReinforcement(SolicitudBarraDTO solicitudBarra, DatosNuevaBarraDTO datosNuevaBarra)
        {
            if (m_createdPathReinforcement == null)
            {
                TaskDialog.Show("Error", "Referencia a PathDeRefuerzo null " + ListaPtosPerimetroBarras.Count);
                return;
            }
            // "Face", 0  ->  activa barra superior  - Top ( viene por defecto=
            // "Face", 1  ->  activa barra inferior  - Botton
            if (solicitudBarra.TipoBarra == "f9" || solicitudBarra.TipoBarra == "f1_SUP" ||
                solicitudBarra.TipoBarra == "f9a" || solicitudBarra.TipoBarra == "f10" || solicitudBarra.TipoBarra == "f10a" || solicitudBarra.TipoBarra == "f9" ||
                solicitudBarra.TipoBarra == "s1" || solicitudBarra.TipoBarra == "s3" || solicitudBarra.TipoBarra == "s2")
                ParameterUtil.SetParaInt(m_createdPathReinforcement, "Face", 0);
            else
                ParameterUtil.SetParaInt(m_createdPathReinforcement, "Face", 1);

            //int cantidadBArras = Convert.ToInt32((datosNuevaBarra.LargoRecorridoFoot + Util.MmToFoot(datosNuevaBarra.DiametroMM)) / datosNuevaBarra.EspaciamientoFoot);
            //double cantidadBArras2 = (datosNuevaBarra.LargoRecorridoFoot + Util.MmToFoot(datosNuevaBarra.DiametroMM)) / datosNuevaBarra.EspaciamientoFoot;
            //long valorSinDecimal = (long)cantidadBArras2;

            ParameterUtil.SetParaDouble(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, datosNuevaBarra.LargoPathreiforment);

            ParameterUtil.SetParaDouble(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_SPACING, datosNuevaBarra.EspaciamientoFoot);
            // ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.REBAR_SYSTEM_LAYOUT_RULE, 3);
            //ESPACIMEINTOS

            ParameterUtil.SetParaElementId(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_SHAPE_1, datosNuevaBarra.tipoRebarShapePrincipal.Id);

            // si es luz princiapl sube el path 0.03'
            if (datosNuevaBarra.IsLuzSecuandiria) //ParameterUtil.SetParaInt(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_ADDL_OFFSET, Util.CmToFoot(1.5));
            {
                double despla = (DiametroOrientacionPrincipal_mm != 0 ? Util.MmToFoot(DiametroOrientacionPrincipal_mm) : Util.CmToFoot(1));
                ParameterUtil.SetParaDouble(m_createdPathReinforcement, BuiltInParameter.PATH_REIN_ADDL_OFFSET, despla);
            }
            return;
        }

        public GenerarGeometriaAhorroDTO ObtenerGenerarGeometriaAhorroDTO()
        {
            return new GenerarGeometriaAhorroDTO()
            {
                _seleccionarLosaBarraRoom = this._seleccionarLosaBarraRoom,
                _refereciaRoomDatos = this._refereciaRoomDatos,
                IsLuzSecuandiria = this.IsLuzSecuandiria,
                TipoDireccionBarra_ = this.TipoDireccionBarra_,
                ubicacionEnlosa = this.ubicacionEnlosa,
                TipoBarraStr = this.TipoBarraStr,
                ListaPtosPerimetroBarras = this.ListaPtosPerimetroBarras
            };
        }

        #endregion

        #endregion

    }
}
