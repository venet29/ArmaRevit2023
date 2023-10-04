
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

using ArmaduraLosaRevit.Model.BarraV.Intersecciones;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using System.Diagnostics;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Buscar.BuscarBarrarCabezaMuro;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Calculos;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class IntervaloBarrasDTO
    {
        protected DatosMuroSeleccionadoDTO _DatosElementSeleccionadoDTO;
        protected ConfiguracionIniciaWPFlBarraVerticalDTO _confWPFiEnfierradoDTO;
        protected ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO;

        public RecalcularPtosYEspaciamieto_Horquilla RecalcularPtosYEspaciamieto_Horqu { get; set; }
        public ParametrosInternoRebarDTO _parametrosInternoRebarDTO { get; set; }
        public TipoBarraVertical TipoBarraVertical_ { get; private set; }
        public TipoPataBarra _tipoLineaMallaH { get; set; }
        public double _nuevaLineaCantidadbarra { get; set; }
        // espaciamiento  en el sentido del recorrido dde la barra. cabela muro: perpendicular view, malla: paralelo a view
        protected double espaciamientoFoot_RecorridoBarras;
        public Element ElementoHost { get; set; }
        protected double espaciamietoRecorridoBarraFoot;
        public double EspesorElementoHost { get; set; }        
        public double LargoEspesorMalla_SinRecub_foot { get; set; }
        public XYZ ViewDirectionEntradoView { get; set; } // vector perpendiculaer entrado en la pantalla o vista
        public XYZ DireccionPataEnFierrado { get; set; } //direccion enn que se dibujan la patas de las barras
        private bool IsLargoRecorrido;
        public XYZ DireccionRecorridoBarra { get; set; } //direccion en la qe se extiende el set de barrras
        protected double _espaciamientoREspectoBordeFoot;
        public XYZ ptoini { get; set; }
        public XYZ ptofinal { get; set; }
        public XYZ ptoPosicionTAg { get; set; }
        // public XYZ ptoPosicionTAgAuto { get; set; }
        public TipoPataBarra tipobarraV { get; set; }
        public RebarHookType tipoHookInicial { get; set; }
        public RebarHookType tipoHookFinal { get; set; }
        public int diametroMM { get; set; }
        public Orientacion OrientacionTagGrupoBarras { get; set; }
        public Orientacion Orientacion { get; set; }
        // espaciamiento  en el sentido del recorrido dde la barra. cabela muro: perpendicular view, malla: paralelo a view
        public double espaciamientoRecorridoBarrasFoot { get; set; }
        public double recorridoBarrar { get; set; }
        public List<Curve> listaCurve { get; set; }
        public View3D _view3D_paraBuscar { get; set; }
        public View3D view3D_paraVisualizar { get; set; }
        public View _viewActual { get; set; }
        public bool IsbarraIncial { get; set; }
        public bool IsProloganLosaBajo { get; set; }
        public bool IsNoProloganLosaArriba { get; set; }
        public bool IsBuscarCoronacion { get; set; }
       // public bool IsLargopata { get; set; } = false;
        public bool IsIsDirectriz { get; set; }
        public bool IsFundacion { get; set; }//solo barra horizontal
        public double Largopata { get; set; }
        public bool IsUltimoTramoCOnMouse { get; set; }
        public TipoRebar BarraTipo { get; internal set; }
        protected double _auxDesplazamientoTag = 0;
        private TipoPataBarra TipoPataBarra_Inicial;
        public TipoPaTaMalla _tipoPataMallaInicial { get; set; }
        public TipoPaTaMalla _tipoPataMallaFinal { get; set; }
        public IntervaloBarras_HorqDTO _intervaloBarras_HorqDTO { get; set; }
        public IntervaloBarrasDTO()
        {

        }


        //a)vertical manual
        public IntervaloBarrasDTO(DatosMuroSeleccionadoDTO muroSeleccionadoDTO,
                                 ConfiguracionIniciaWPFlBarraVerticalDTO confWPFiEnfierradoDTO)
        {
            _parametrosInternoRebarDTO = new ParametrosInternoRebarDTO();
            this.TipoBarraVertical_ = confWPFiEnfierradoDTO.TipoBarraRebar_;
            this._tipoLineaMallaH = confWPFiEnfierradoDTO.tipobarraH;
            this._DatosElementSeleccionadoDTO = muroSeleccionadoDTO;
            this._confWPFiEnfierradoDTO = confWPFiEnfierradoDTO;
            this.ElementoHost = muroSeleccionadoDTO.elementoContenedor;
            this.espaciamietoRecorridoBarraFoot = Util.ConvertirStringInDouble(confWPFiEnfierradoDTO.EspaciamietoRecorridoBarraFoot);
            this.ViewDirectionEntradoView = muroSeleccionadoDTO.NormalEntradoView.RedondearSIHAYCERO();
            this.DireccionRecorridoBarra = muroSeleccionadoDTO.DireccionRecorridoBarra.RedondearSIHAYCERO();
            this.DireccionPataEnFierrado = muroSeleccionadoDTO.DireccionPataEnFierrado.RedondearSIHAYCERO();
            this.IsIsDirectriz = (confWPFiEnfierradoDTO.inicial_IsDirectriz ? true : false);
            this.IsLargoRecorrido = (muroSeleccionadoDTO.IsLargoRecorrido ? true : false);

            this.IsBuscarCoronacion = (muroSeleccionadoDTO.IsCoronacion ? true : false);
            this.EspesorElementoHost = muroSeleccionadoDTO.EspesorElemetoHost;

            this._parametrosInternoRebarDTO._texToCantidadoBArras = confWPFiEnfierradoDTO.Inicial_Cantidadbarra;
            this._parametrosInternoRebarDTO._texToTipoDiam = confWPFiEnfierradoDTO.inicial_diametroMM.ToString();
            this._nuevaLineaCantidadbarra = confWPFiEnfierradoDTO.NuevaLineaCantidadbarra;
            this.espaciamientoFoot_RecorridoBarras = Util.ConvertirStringInDouble(confWPFiEnfierradoDTO.EspaciamietoRecorridoBarraFoot);
            this._espaciamientoREspectoBordeFoot = confWPFiEnfierradoDTO.EspaciamientoREspectoBordeFoot;
            this._viewActual = confWPFiEnfierradoDTO.viewActual;
            this._view3D_paraBuscar = confWPFiEnfierradoDTO.view3D_paraBuscar;
            this.view3D_paraVisualizar = confWPFiEnfierradoDTO.view3D_paraVisualizar;
            this.diametroMM = confWPFiEnfierradoDTO.inicial_diametroMM;

            this._auxDesplazamientoTag = Util.CmToFoot(confWPFiEnfierradoDTO.IntervalosEspaciamiento.Sum() - confWPFiEnfierradoDTO.IntervalosEspaciamiento[0]);
            this.IsbarraIncial = false;
            this.IsProloganLosaBajo = false;
            this.IsFundacion = false; //solo barra horizontal
            this.tipoHookInicial = null;
            this.tipoHookFinal = null;
            M1_1_AsignarEspaciamientoYrecorrido();

        }
        //b) vertical automatico
        public IntervaloBarrasDTO(DatosMuroSeleccionadoDTO muroSeleccionadoDTO,
                                  ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO,
                                  IntervalosBarraAutoDto newIntervaloBarraAutoDto)
        {
            _parametrosInternoRebarDTO = new ParametrosInternoRebarDTO();

            this._DatosElementSeleccionadoDTO = muroSeleccionadoDTO;
            this.ElementoHost = muroSeleccionadoDTO.elementoContenedor;

            this.ViewDirectionEntradoView = muroSeleccionadoDTO.NormalEntradoView.RedondearSIHAYCERO();
            this.DireccionRecorridoBarra = muroSeleccionadoDTO.DireccionRecorridoBarra.RedondearSIHAYCERO();
            this.DireccionPataEnFierrado = muroSeleccionadoDTO.DireccionPataEnFierrado.RedondearSIHAYCERO();
            this.IsLargoRecorrido = (muroSeleccionadoDTO.IsLargoRecorrido ? true : false);
            this.EspesorElementoHost = muroSeleccionadoDTO.EspesorElemetoHost;
            this.IsIsDirectriz = (confiWPFEnfierradoDTO.inicial_IsDirectriz ? true : false); ;
            this._parametrosInternoRebarDTO._texToCantidadoBArras = newIntervaloBarraAutoDto.Inicial_Cantidadbarra.ToString();
            this._parametrosInternoRebarDTO._texToTipoDiam = newIntervaloBarraAutoDto.Inicial_diametroMM.ToString();
            this._nuevaLineaCantidadbarra = newIntervaloBarraAutoDto.Inicial_Cantidadbarra;
            this._espaciamientoREspectoBordeFoot = newIntervaloBarraAutoDto.EspaciamientoREspectoBordeFoot;
            this.espaciamientoFoot_RecorridoBarras = Util.ConvertirStringInDouble(confiWPFEnfierradoDTO.EspaciamietoRecorridoBarraFoot);
            this._viewActual = confiWPFEnfierradoDTO.viewActual;
            this._view3D_paraBuscar = confiWPFEnfierradoDTO.view3D_paraBuscar;
            this.view3D_paraVisualizar = confiWPFEnfierradoDTO.view3D_paraVisualizar;
            this.diametroMM = newIntervaloBarraAutoDto.Inicial_diametroMM;
            this.OrientacionTagGrupoBarras = newIntervaloBarraAutoDto.OrientacionTagGrupoBarras;
            this.Orientacion = newIntervaloBarraAutoDto.orientacion;

            this._auxDesplazamientoTag = Util.CmToFoot(confiWPFEnfierradoDTO.IntervalosEspaciamiento.Sum() - confiWPFEnfierradoDTO.IntervalosEspaciamiento[0]);
            this.IsbarraIncial = false;
            this.IsProloganLosaBajo = false;
            this.IsFundacion = false; //solo barra horizontal
            this.tipoHookInicial = null;
            this.tipoHookFinal = null;
           // this.IsLargopata = false;


            M1_1_AsignarEspaciamientoYrecorrido();

        }

        //c) horizonatal manual
        public IntervaloBarrasDTO(DatosMuroSeleccionadoDTO muroSeleccionadoDTO, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO)
        {
            this._DatosElementSeleccionadoDTO = muroSeleccionadoDTO;
            _parametrosInternoRebarDTO = new ParametrosInternoRebarDTO();

            this._confiEnfierradoDTO = confiEnfierradoDTO;
            _parametrosInternoRebarDTO._texToCantidadoBArras = confiEnfierradoDTO.Inicial_Cantidadbarra;
            _parametrosInternoRebarDTO._texToTipoDiam = confiEnfierradoDTO.incial_diametroMM.ToString();

            this.espaciamientoFoot_RecorridoBarras = Util.ConvertirStringInDouble(confiEnfierradoDTO.EspaciamietoRecorridoBarraFoot);

            this._nuevaLineaCantidadbarra = confiEnfierradoDTO.NuevaLineaCantidadbarra;
            this._espaciamientoREspectoBordeFoot = confiEnfierradoDTO.EspaciamientoREspectoBordeFoot;
            this._viewActual = confiEnfierradoDTO.viewActual;
            this._view3D_paraBuscar = confiEnfierradoDTO.view3D_paraBuscar;
            this.view3D_paraVisualizar = confiEnfierradoDTO.view3D_paraVisualizar;

            this.diametroMM = confiEnfierradoDTO.incial_diametroMM;
            this.IsIsDirectriz = (confiEnfierradoDTO.incial_IsDirectriz ? true : false);
            this.ElementoHost = muroSeleccionadoDTO.elementoContenedor;

            this.ViewDirectionEntradoView = muroSeleccionadoDTO.NormalEntradoView;
            this.DireccionRecorridoBarra = muroSeleccionadoDTO.DireccionRecorridoBarra.RedondearSIHAYCERO();
            this.DireccionPataEnFierrado = muroSeleccionadoDTO.DireccionPataEnFierrado.RedondearSIHAYCERO();
            this.IsLargoRecorrido = (muroSeleccionadoDTO.IsLargoRecorrido ? true : false);
            this.EspesorElementoHost = muroSeleccionadoDTO.EspesorElemetoHost;
            this.LargoEspesorMalla_SinRecub_foot = muroSeleccionadoDTO.LargoEspesorMalla_SinRecub_foot;
            this.tipoHookInicial = null;
            this.IsFundacion = _DatosElementSeleccionadoDTO.IsFundacion;
            this.tipoHookFinal = null;
           // this.IsLargopata = false;
            this._tipoPataMallaInicial = TipoPaTaMalla.bordeMuro;
            this._tipoPataMallaFinal = TipoPaTaMalla.bordeMuro;
            M1_1_AsignarEspaciamientoYrecorrido();

            TipoPataBarra_Inicial = TipoPataBarra.buscar; //TipoPataBarra.BarraVSinPatas;
        }


        public bool M1_AsignarElementoHost(BuscarMuros buscarMuros, Document _doc)
        {
            try
            {
                if (buscarMuros == null) return false;
                this.ElementoHost = buscarMuros.WalloVigaElementHost;
                if (!IsLargoRecorrido) this.EspesorElementoHost = buscarMuros._espesorMuro;// (this.ElementoHost as Wall).ObtenerEspesorMuroFoot(_doc);

                M1_1_AsignarEspaciamientoYrecorrido();

                if (!M1_2_BuscarPtoInicioBase(buscarMuros.CentroCaraNormalSaliendoVista)) return false;//buscarMuros.PuntoSobreFAceHost
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
        public bool M1_AsignarElementoHost(BuscarViga buscarViga, Document _doc)
        {
            try
            {
                if (buscarViga == null) return false;
                if (buscarViga._vigaElementHost == null) return false;

                this.ElementoHost = buscarViga._vigaElementHost;
                if (!IsLargoRecorrido) this.EspesorElementoHost = buscarViga._espesorMuro;// ((FamilyInstance)buscarViga._vigaElementHost).ObtenerEspesorVigaFoot();

                M1_1_AsignarEspaciamientoYrecorrido();

                if (!M1_2_BuscarPtoInicioBase(buscarViga.CentroCaraNormalSaliendoVista)) return false;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        public void M1_AsignarElementoHost(BuscarColumna buscarColumna, Document _doc)
        {
            if (buscarColumna == null) return;
            this.ElementoHost = buscarColumna._vigaElementHost;
            if (!IsLargoRecorrido) this.EspesorElementoHost = buscarColumna._espesorMuro;// ((FamilyInstance)buscarViga._vigaElementHost).ObtenerEspesorVigaFoot();

            M1_1_AsignarEspaciamientoYrecorrido();

            M1_2_BuscarPtoInicioBase(buscarColumna.PuntoSobreFAceHost);
        }
        private void M1_1_AsignarEspaciamientoYrecorrido()
        {
            if (_nuevaLineaCantidadbarra == 1)
            {
                throw new NotImplementedException("Solo Una linea de barra.Caso no implementado");
            }
            else
            {
                if (IsLargoRecorrido)
                {

                    this.espaciamientoRecorridoBarrasFoot = espaciamientoFoot_RecorridoBarras;
                    this.recorridoBarrar = (this.EspesorElementoHost
                                            - Util.MmToFoot(diametroMM) - Util.MmToFoot(diametroMM));
                }
                else
                {

                    if (_nuevaLineaCantidadbarra == 1)
                        this.espaciamientoRecorridoBarrasFoot = -1;
                    else
                        this.espaciamientoRecorridoBarrasFoot = (this.EspesorElementoHost
                         - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) * 2.0
                         - Util.MmToFoot(diametroMM)) / (_nuevaLineaCantidadbarra - 1);

                    this.recorridoBarrar = (this.EspesorElementoHost
                                            - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) * 2.0
                                            - Util.MmToFoot(diametroMM));
                }
            }
        }

        public bool M1_2_BuscarPtoInicioBase(XYZ ptoSObreCaraMuro)
        {
            try
            {
                if (ptoSObreCaraMuro == null) return false;
                Plane plano = Plane.CreateByNormalAndOrigin(-this.ViewDirectionEntradoView, ptoSObreCaraMuro);
                _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = plano.ProjectOnto(_DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost).Redondear8();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public XYZ M1_2_BuscarPtoSobreCaraMuro(XYZ ptoSObreCaraMuro, XYZ _ptoAnalisado)
        {
            try
            {
                Plane plano = Plane.CreateByNormalAndOrigin(-this.ViewDirectionEntradoView, ptoSObreCaraMuro);
                _ptoAnalisado = plano.ProjectOnto(_ptoAnalisado);
            }
            catch (Exception)
            {
                return XYZ.Zero;
            }
            return _ptoAnalisado;
        }

        public void M2_AsiganrCoordenadasV(double ziniFoot, double zfinFoot, bool IsmoverTraslapo, bool IsPrimeraBarra)
        {
            //   double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) / 2 + (IsmoverTraslapo ? Util.MmToFoot(diametroMM) : 0);
            double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) / 2.0f;
            double espaciamientoporTraslapo = (IsmoverTraslapo ? Util.MmToFoot(diametroMM) : 0);
            //   double espaciamientoPrimeraBarraBordeFoot = (IsPrimeraBarra ? Util.CmToFoot((diametroMM) / 10f) : 0);
            this.tipobarraV = TipoPataBarra_Inicial; //TipoPataBarra.BarraVSinPatas;

            this.ptoini = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot) +
                                    ViewDirectionEntradoView * (Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) + Util.MmToFoot(diametroMM / 2f)) +
                                   _DatosElementSeleccionadoDTO.DireccionLineaBarra * (espaciamientoBordeFoot + espaciamientoporTraslapo);
            //(diametroMM) / 10

            this.ptoPosicionTAg = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(zfinFoot - ConstNH.CONST_DISTANCIA_BAJATAG_Foot - _espaciamientoREspectoBordeFoot) +
                                    ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) +
                                    DireccionPataEnFierrado * espaciamientoBordeFoot
                                    - _DatosElementSeleccionadoDTO.DireccionLineaBarra * _auxDesplazamientoTag;

            this.ptofinal = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(zfinFoot) +
                                    ViewDirectionEntradoView * (Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) + Util.MmToFoot(diametroMM / 2f)) +
                                    _DatosElementSeleccionadoDTO.DireccionLineaBarra * (espaciamientoBordeFoot + espaciamientoporTraslapo);
            RedonderA6Dig();

        }

        //para malla V manual
        public void M2_AsiganrCoordenadasV_reccorridoParaleloView(double ziniFoot, double zfinFoot, bool IsmoverTraslapo, Document _doc)
        {
            //   double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) / 2 + (IsmoverTraslapo ? Util.MmToFoot(diametroMM) : 0);
            double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) * 1.5;
            double espaciamientoporTraslapo = (IsmoverTraslapo ? -Util.MmToFoot(diametroMM) / 2.0f : Util.MmToFoot(diametroMM) / 2.0f);
            this.tipobarraV = TipoPataBarra_Inicial; // TipoPataBarra.BarraVSinPatas;//_confWPFiEnfierradoDTO.inicial_tipoBarraV;//

            double desplazamietoTotal = _DatosElementSeleccionadoDTO.DesplazamientoVerticalFoot;

            BuscardorBarrasCabezaVertical _buscardorBarrasCabezaVertical = new BuscardorBarrasCabezaVertical(_doc, _view3D_paraBuscar,
                              _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(zfinFoot - Util.CmToFoot(30) - UtilBarras.largo_traslapoFoot_diamMM(diametroMM)),
                              _DatosElementSeleccionadoDTO.EspesorElemetoHost,
                              diametroMM);
            if (_buscardorBarrasCabezaVertical.BuscarREbar(_viewActual.RightDirection))
            {
                double espaciemientoV = espaciamietoRecorridoBarraFoot;
                _nuevaLineaCantidadbarra = Util.ParteEnteraInt(_buscardorBarrasCabezaVertical.largoRecorrido_resultado / espaciemientoV);
                double _parteDeci = Util.ParteDecimal(_buscardorBarrasCabezaVertical.largoRecorrido_resultado / espaciemientoV);
                if (_parteDeci < ConstNH.CONST_FACTOR_DISMINUIR_1BARRA) _nuevaLineaCantidadbarra -= 1;

                double DesplazamientoVerticalFoot_aux = espaciemientoV * ((_parteDeci < ConstNH.CONST_FACTOR_DISMINUIR_1BARRA ? 2 : 1) + _parteDeci) / 2;
                desplazamietoTotal = _buscardorBarrasCabezaVertical.DesplazamientoRespecInicio + DesplazamientoVerticalFoot_aux;
                // Debug.WriteLine($"_parteDeci:{_parteDeci} \n despl :{DesplazamientoVerticalFoot_aux}   \n largorecorrido :{_buscardorBarrasCabezaVertical.largoRecorrido_resultado}  " ); 
            }

            this.ptoini = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot) +
                                    ViewDirectionEntradoView * (espaciamientoBordeFoot) +
                                   _viewActual.RightDirection * (espaciamientoporTraslapo + desplazamietoTotal);
            //(diametroMM) / 10

            this.ptofinal = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(zfinFoot) +
                                    ViewDirectionEntradoView * (espaciamientoBordeFoot) +
                                    _viewActual.RightDirection * (espaciamientoporTraslapo + desplazamietoTotal);


            this.ptoPosicionTAg = (ptofinal + ptoini) / 2 + _viewActual.RightDirection * _buscardorBarrasCabezaVertical.largoRecorrido_resultado * 0.4;

            //redondear
            RedonderA6Dig();


            _parametrosInternoRebarDTO._IsMalla = true;
            _parametrosInternoRebarDTO._cuantiaMalla = _confWPFiEnfierradoDTO.CuantiaMalla;
            _parametrosInternoRebarDTO._IdMalla = "Ma" + _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot).REdondearString_foot(3);
        }

        //para malla V  auto
        public void M2_AsiganrCoordenadasV_reccorridoParaleloView_Auto(double ziniFoot, double zfinFoot, XYZ ptoIni_mallaVertical, XYZ ptoFin_mallaVertical, bool IsmoverTraslapo, Document _doc)
        {
            //   double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) / 2 + (IsmoverTraslapo ? Util.MmToFoot(diametroMM) : 0);
            double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) * 1.5;
            double espaciamientoporTraslapo = (IsmoverTraslapo ? -Util.MmToFoot(diametroMM) / 2.0f : Util.MmToFoot(diametroMM) / 2.0f);
            this.tipobarraV = TipoPataBarra_Inicial; // TipoPataBarra.BarraVSinPatas;

            double desplazamietoTotal = _DatosElementSeleccionadoDTO.DesplazamientoVerticalFoot;

            double largoRecorrido = ptoIni_mallaVertical.GetXY0().DistanceTo(ptoFin_mallaVertical.GetXY0());
            double DesplazamientoRespecInicio = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.GetXY0().DistanceTo(ptoIni_mallaVertical.GetXY0());

            double espaciemientoV = espaciamietoRecorridoBarraFoot;
            _nuevaLineaCantidadbarra = Util.ParteEnteraInt(largoRecorrido / espaciemientoV);
            double _parteDeci = Util.ParteDecimal(largoRecorrido / espaciemientoV);
            if (_parteDeci < ConstNH.CONST_FACTOR_DISMINUIR_1BARRA) _nuevaLineaCantidadbarra -= 1;

            double DesplazamientoVerticalFoot_aux = espaciemientoV * ((_parteDeci < ConstNH.CONST_FACTOR_DISMINUIR_1BARRA ? 2 : 1) + _parteDeci) / 2;
            desplazamietoTotal = DesplazamientoRespecInicio + DesplazamientoVerticalFoot_aux;

            this.ptoini = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot) +
                                    ViewDirectionEntradoView * (espaciamientoBordeFoot) +
                                   _viewActual.RightDirection * (espaciamientoporTraslapo + desplazamietoTotal);
            //(diametroMM) / 10

            this.ptofinal = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(zfinFoot) +
                                    ViewDirectionEntradoView * (espaciamientoBordeFoot) +
                                    _viewActual.RightDirection * (espaciamientoporTraslapo + desplazamietoTotal);

            this.ptoPosicionTAg = (ptofinal + ptoini) / 2 + _viewActual.RightDirection * largoRecorrido * 0.4;

            //redondear
            RedonderA6Dig();

            _parametrosInternoRebarDTO._IsMalla = true;
            _parametrosInternoRebarDTO._cuantiaMalla = _confWPFiEnfierradoDTO.CuantiaMalla;
            _parametrosInternoRebarDTO._IdMalla = "Ma" + _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot).REdondearString_foot(3);
        }

        //para malla H manual
        public void M2_AsiganrCoordenadasH_reccorridoParaleloView(double ziniFoot, double zfinFoot, SelecionarPtoSup selecionarPtoSup)
        {
            //   double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) / 2 + (IsmoverTraslapo ? Util.MmToFoot(diametroMM) : 0);
            double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + Util.MmToFoot(diametroMM) / 2f;


            this.tipobarraV = _confWPFiEnfierradoDTO.tipobarraH;// _tipoLineaMallaH;
            if (_tipoLineaMallaH == TipoPataBarra.BarraVPataFinal || _tipoLineaMallaH == TipoPataBarra.BarraVPataInicial)
            {
                if (_tipoPataMallaInicial == TipoPaTaMalla.bordeMuro)
                    tipoHookInicial = TipoRebarHookType.ObtenerHook("Rebar Hook 135", _confWPFiEnfierradoDTO.Document_);
                if (_tipoPataMallaFinal == TipoPaTaMalla.bordeMuro)
                    tipoHookFinal = TipoRebarHookType.ObtenerHook("Rebar Hook 135", _confWPFiEnfierradoDTO.Document_);
                //this.IsLargopata = true;

          
            }

            this.LargoEspesorMalla_SinRecub_foot = _DatosElementSeleccionadoDTO.LargoEspesorMalla_SinRecub_foot;

            this.ptoini = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot) +
                                    ViewDirectionEntradoView * (espaciamientoBordeFoot);

            this.ptofinal = selecionarPtoSup._PtoFinalIntervaloBarra_ProyectadoCaraMuroHost.AsignarZ(ziniFoot) +
                                    ViewDirectionEntradoView * (espaciamientoBordeFoot);
            this._nuevaLineaCantidadbarra = ((int)((zfinFoot - ziniFoot) / espaciamientoRecorridoBarrasFoot));

            if (IsUltimoTramoCOnMouse) _nuevaLineaCantidadbarra += 1;
            this.DireccionRecorridoBarra = new XYZ(0, 0, 1);

            this.ptoPosicionTAg = (this.ptoini + this.ptofinal) / 2;


            //redondear
            RedonderA6Dig();

            _parametrosInternoRebarDTO._IsMalla = true;
            _parametrosInternoRebarDTO._cuantiaMalla = _confWPFiEnfierradoDTO.CuantiaMalla;
            _parametrosInternoRebarDTO._IdMalla = "Ma" + _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot).REdondearString_foot(3).Replace(" ", "").Replace(",", "&");
        }




        //barra vertrical auto
        public void M2_AsiganrCoordenadasVAuto(CoordenadasBarra item, bool IsmoverTraslapo, double diamtroIntervaloAnteriorMM)
        {

            this.IsProloganLosaBajo = item.IsProloganLosaBajo;
            this.IsNoProloganLosaArriba = item.IsNoProloganLosaArriba;
            this.IsBuscarCoronacion = item.IsBuscarCororonacion;

            XYZ desplaPorTraslapo = DireccionPataEnFierrado * (IsmoverTraslapo ? Util.MmToFoot(diamtroIntervaloAnteriorMM) : 0);
            this.tipobarraV = TipoPataBarra_Inicial; // TipoPataBarra.BarraVSinPatas;

            XYZ ptoInicial_aux = M1_2_BuscarPtoSobreCaraMuro(_DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost, item.ptoIni_foot);
            XYZ ptoFin_aux = M1_2_BuscarPtoSobreCaraMuro(_DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost, item.ptoFin_foot);

            this.ptoini = ptoInicial_aux + ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM + (diametroMM / 2) / 10f) + desplaPorTraslapo;


            this.ptoPosicionTAg = _DatosElementSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(item.ptoFin_foot.Z) +
                                  new XYZ(0, 0, -ConstNH.CONST_DISTANCIA_BAJATAG_Foot) +
                                  ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM + (diametroMM) / 10f);


            this.ptofinal = ptoFin_aux + ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM + (diametroMM / 2) / 10f) + desplaPorTraslapo;

            //redondear
            RedonderA6Dig();

        }

        //solo para barrara horizonatal . No incluye laterales ni malla horizonal
        internal void M2_AsiganrCoordenadasH(XYZ ptzini, XYZ ptoFin, bool IsmoverTraslapo, bool IsPrimeraBarra)
        {
            double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + (IsmoverTraslapo ? ConstNH.CONST_DESVIACION_TRASLAPOFOOT : 0);
            double espaciamientoPrimeraBarraBordeFoot = (IsPrimeraBarra ? Util.CmToFoot((diametroMM) / 10f) : 0);
            this.tipobarraV = TipoPataBarra_Inicial; // TipoPataBarra.BarraVSinPatas;
            this.ptoini = ptzini +
                        ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_HORI_CM + (diametroMM) / 20f) +
                        DireccionPataEnFierrado * (espaciamientoBordeFoot + espaciamientoPrimeraBarraBordeFoot);

            this.ptoPosicionTAg = (ptoFin + ptzini) / 2 + ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_HORI_CM + (diametroMM) / 10f) + DireccionPataEnFierrado * (espaciamientoBordeFoot + espaciamientoPrimeraBarraBordeFoot);

            this.ptofinal = ptoFin +
                    ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_HORI_CM + (diametroMM) / 20f) +
                    DireccionPataEnFierrado * (espaciamientoBordeFoot + espaciamientoPrimeraBarraBordeFoot);
            RedonderA6Dig();
        }


        /// <summary>
        /// los ptos inicials y finales se asigna en poscion horizontal en la viga o muro, pero no le asigna coordenada en z
        /// </summary>

        internal void M2_AsiganrCoordenadasH_vigaAuto(XYZ ptzini, XYZ ptoFin, bool IsmoverTraslapo, bool IsPrimeraBarra)
        {
            double espaciamientoBordeFoot = _espaciamientoREspectoBordeFoot + (IsmoverTraslapo ? ConstNH.CONST_DESVIACION_TRASLAPOFOOT : 0);
            double espaciamientoPrimeraBarraBordeFoot = (IsPrimeraBarra ? Util.CmToFoot((diametroMM) / 10f) : 0);
            this.tipobarraV = TipoPataBarra_Inicial; // TipoPataBarra.BarraVSinPatas;
            this.ptoini = ptzini +
                        ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_HORI_CM + (diametroMM) / 20f);
            //   DireccionPataEnFierrado * (espaciamientoBordeFoot + espaciamientoPrimeraBarraBordeFoot);

            this.ptoPosicionTAg = (ptoFin + ptzini) / 2 + ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_HORI_CM + (diametroMM) / 10f);
            //+ DireccionPataEnFierrado * (espaciamientoBordeFoot + espaciamientoPrimeraBarraBordeFoot);

            this.ptofinal = ptoFin +
                    ViewDirectionEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_HORI_CM + (diametroMM) / 20f);
            //DireccionPataEnFierrado * (espaciamientoBordeFoot + espaciamientoPrimeraBarraBordeFoot);
            RedonderA6Dig();
        }

        public void BuscarPatasAmbosLadosVertical(UIApplication _uipp, BuscarMurosDTO _muroHost50cmSobrePtoinicialDTO, TipoBarraVertical _TipoBarraVertical = TipoBarraVertical.Cabeza)
        {
            if (_TipoBarraVertical == TipoBarraVertical.MallaH ||
                _TipoBarraVertical == TipoBarraVertical.Cabeza_Horquilla ||
                _TipoBarraVertical == TipoBarraVertical.Cabeza_BarraVHorquilla) return;
            TiposDeBarraPorInterseccionVertical TiposDeBarraPorInterseccion =
                new TiposDeBarraPorInterseccionVertical(_uipp, _view3D_paraBuscar, _muroHost50cmSobrePtoinicialDTO, this, _DatosElementSeleccionadoDTO);

            TiposDeBarraPorInterseccion.BuscarInterseccion();
            if (_TipoBarraVertical == TipoBarraVertical.MallaV)
            {
                if (TiposDeBarraPorInterseccion.ResultTipobarraVInferior != TipoPataBarra.BarraVSinPatas)
                    ptoini = TiposDeBarraPorInterseccion._ptoini;
                else
                {
                    if (TiposDeBarraPorInterseccion._ptoini.Z < ptoini.Z)
                        ptoini = ptoini.AsignarZ(TiposDeBarraPorInterseccion._ptoini.Z);
                }
            }
            else if (_TipoBarraVertical == TipoBarraVertical.Cabeza)
                ptoini = TiposDeBarraPorInterseccion._ptoini;


            ptofinal = TiposDeBarraPorInterseccion._ptofinal;
            tipobarraV = TiposDeBarraPorInterseccion.ResultTipoBarraV;

            RedonderA6Dig();

        }

        public void BuscarPatasAmbosLadosHorizontal(UIApplication _uipp, EmpotramientoPatasDTO _empotramientoPatasDTO)
        {

            TiposDeBarraPorInterseccionHorizontal TiposDeBarraPorInterseccion =
                new TiposDeBarraPorInterseccionHorizontal(_uipp, _view3D_paraBuscar, this, _empotramientoPatasDTO, _confiEnfierradoDTO);

            TiposDeBarraPorInterseccion.BuscarInterseccion();
            ptoini = TiposDeBarraPorInterseccion._ptoini;
            ptofinal = TiposDeBarraPorInterseccion._ptofinal;
            tipobarraV = TiposDeBarraPorInterseccion.ResultTipoBarraV;

            //redondear
            RedonderA6Dig();

        }


        protected void RedonderA6Dig()
        {
            this.ptoini = this.ptoini.Redondear8();
            this.ptoPosicionTAg = this.ptoPosicionTAg.Redondear8();
            this.ptofinal = this.ptofinal.Redondear8();
        }
    }
}
