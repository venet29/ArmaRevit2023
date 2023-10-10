using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using System.Linq;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Seleccion
{
    public class SeleccionPtosEstriboViga : SeleccionarElementosV
    {
        public List<XYZ> _listaPtosPAthArea;

        private Rebar _barra1;
        protected double _anchoEstribo1Foot_extremoAextremo; // ancho del extremos de cada lado -- largo mas extremo
                                                             // protected XYZ _ptobarra1;
                                                             //  protected XYZ _ptobarra2;

        public XYZ _ptobarra1 { get; set; }
        public XYZ _ptobarra2 { get; set; }


        private Rebar _barra2;
        protected double _anchoEstribo2Foot_extremoAextrem; // ancho del extremos de cada lado -- largo mas extremo


        protected DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO;

        private DireccionSeleccionMouse _direccionSeleccionMouse;

        protected Element _wallSeleccionado;
        protected double _espesorMuroVigaFoot;
        private XYZ _direccion1DeEstribo;
        private XYZ _direccionPerpenEntradoHaciaViewSecction;
        private double _AnchoVisibleFoot;
        private double _largoRecorridoEstriboFoot;
        protected List<Level> ListaLevelSeleccionado;
        protected List<EstriboMuroDTO> listaDeEstribos;

        //private List<double> listaEspac;
        private string _nombreFamilia;
        private XYZ _ptoTag;
        // private XYZ _direccionBarra;
        private List<BarraTrabaDTO> ListaTraba;
        public BuscarFundacionLosaV2 BuscarFundacionLosaInf { get; set; }
        public BuscarFundacionLosaV2 BuscarFundacionLosaSup { get; set; }
        public bool IsOk { get; set; }

        public SeleccionPtosEstriboViga(UIApplication _uiapp, View3D _view3D_paraBuscar_, DatosConfinamientoAutoDTO configuracionInicialEstriboDTO)
            : base(_uiapp)
        {
            // _uidoc = _uiapp.ActiveUIDocument;
            _listaPtosPAthArea = new List<XYZ>();
            _view3D_paraBuscar = _view3D_paraBuscar_;
            this._configuracionInicialEstriboDTO = configuracionInicialEstriboDTO;

            ListaLevelSeleccionado = new List<Level>();
            listaDeEstribos = new List<EstriboMuroDTO>();

        }


        #region 1) rutina PRincipal

        public bool M1_Ejecutar_SeleccionarRebar()
        {
            IsOk = false;
            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            //  if (!M1_2_SeleccionarBordeMuro()) return false;
            //if (!M1_3_SeleccionarMuroElement()) return false;
            //if (!M1_4_BuscarPtoInicioBase()) return false;
            if (!M1_3_SeleccionarRebarElement()) return false;

            //if (!M1_3a_ObtenerRangoLevelSeleccionado()) return false;

            if (!(M1_4_BuscarMuroPerpendicularVIew((_ptobarra1 + _ptobarra2) / 2))) return false;

            if (!(M1_4_2_BuscarPuntosSegunMargen((_ptobarra1 + _ptobarra2) / 2)))// busca en el centro 
            {
                if (!(M1_4_2_BuscarPuntosSegunMargen(_ptobarra1)))// pto inical barras inicial
                {
                    if (!(M1_4_2_BuscarPuntosSegunMargen(_ptobarra2))) // pto final 
                    {
                        return false;
                    }
                }
            }

            if (!M1_5_GenerarVectores()) return false;

            M1_6_NombreFamilaTAG();

            IsOk = true;
            return true;

        }

        #region comandos :M1_3_SeleccionarRebarElement
        public bool M1_3_SeleccionarRebarElement()
        {

            try
            {
                ISelectionFilter filtroRebar = new FiltroRebar();

                //1
                Reference ref_baar1 = _uidoc.Selection.PickObject(ObjectType.Element, filtroRebar, "Seleccionar primera Barra:");
                _ptobarra1 = ref_baar1.GlobalPoint;


                _barra1 = (Rebar)_doc.GetElement(ref_baar1);
                if (_barra1.Quantity == 1)
                {
                    Util.ErrorMsg("Grupo de barra tiene una unica barra. Se necesitan minimo 2 para obtener espesor de estribo");
                    return false;
                }

                if (!AyudaObtenerNormarPlanoVisisible.Obtener(_barra1, _view)) return false;

                double _diamBarraFoot1 = _barra1.ObtenerDiametroFoot();
                //buscar pto centro barra
                if (AyudaObtenerPtoEnRebar.Obtener(_barra1, _ptobarra1))
                {
                    _ptobarra1 = _ptobarra1.AsignarZ(AyudaObtenerPtoEnRebar.ptoMASCercano.Z);
                }
                // _ptobarra1 = _ptoSeleccionMouseCentroCaraMuro.ObtenerCopia();

                double diamtroEstribo_Foot = Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM);

                RebarShapeDrivenAccessor _barraGetShapeDrivenAccessor = _barra1.GetShapeDrivenAccessor();
                _anchoEstribo1Foot_extremoAextremo = _barraGetShapeDrivenAccessor.ArrayLength + _diamBarraFoot1 + diamtroEstribo_Foot * 2;
                if (!Util_IsBarraNOVertica(_barraGetShapeDrivenAccessor)) return false;

                //2
                Reference ref_baar2 = _uidoc.Selection.PickObject(ObjectType.Element, filtroRebar, "Seleccionar Segunda Barra Barra:");
                _ptobarra2 = ref_baar2.GlobalPoint;
                _ptobarra2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(AyudaObtenerNormarPlanoVisisible.FaceNormal, _ptobarra1, _ptobarra2);

                _barra2 = (Rebar)_doc.GetElement(ref_baar2);

                if (_barra2.Quantity == 1)
                {
                    Util.ErrorMsg("Segunda barra seleccionada no puede ser barra unica porque se puede obtener espesor de estribo. Modifique distribucion de barras ");
                    return false;
                }
                double _diamBarraFoot2 = _barra2.ObtenerDiametroFoot();

                //buscar pto centro barra
                if (AyudaObtenerPtoEnRebar.Obtener(_barra2, _ptobarra2))
                {
                    _ptobarra2 = _ptobarra2.AsignarZ(AyudaObtenerPtoEnRebar.ptoMASCercano.Z);
                }


                RebarShapeDrivenAccessor _barraGetShapeDrivenAccessor2 = _barra2.GetShapeDrivenAccessor();
                _anchoEstribo2Foot_extremoAextrem = _barraGetShapeDrivenAccessor2.ArrayLength + _diamBarraFoot2 * 1.5 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 2;
                if (!Util_IsBarraNOVertica(_barraGetShapeDrivenAccessor2)) return false;


                ///modificacnoes ajustes
                //_anchoEstribo1Foot = Math.Max(_anchoEstribo1Foot, _anchoEstribo2Foot);
                _anchoEstribo2Foot_extremoAextrem = _anchoEstribo1Foot_extremoAextremo;

                double _diamEstribo_foot = Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM);
                var _DiametrosBarrasDTO = TipoDiametrosBarrasDTO.ObtenerListaDiametro().Where(c => c.diametro_mm == _configuracionInicialEstriboDTO.DiamtroEstriboMM).FirstOrDefault();
                double maxDiamtro = Math.Max(_diamBarraFoot1, _diamBarraFoot2);
                //  maxDiamtro = 0;

                double desplaPorDiam = maxDiamtro / 2 + Util.MmToFoot(_DiametrosBarrasDTO.Standard_Bend_Diameter_mm / 2);
                if (_ptobarra1.Z < _ptobarra2.Z)
                {
                    _ptobarra1 = _ptobarra1 + _view.ViewDirection * (maxDiamtro * 0.5 + diamtroEstribo_Foot * 1.5) + new XYZ(0, 0, -desplaPorDiam);
                    _ptobarra2 = _ptobarra2 + _view.ViewDirection * (maxDiamtro * 0.5 + diamtroEstribo_Foot * 1.5) + new XYZ(0, 0, desplaPorDiam);
                }
                else
                {
                    _ptobarra1 = _ptobarra1 + _view.ViewDirection * (maxDiamtro * 0.5 + diamtroEstribo_Foot) + new XYZ(0, 0, +desplaPorDiam);
                    _ptobarra2 = _ptobarra2 + _view.ViewDirection * (maxDiamtro * 0.5 + diamtroEstribo_Foot) + new XYZ(0, 0, -desplaPorDiam);
                }

                M1_3_1_OrientacionSeleccion();
                M1_3_2_ReasignarSiP1MayorZqueP2();
                M1_3_3_ReasignarSiP2MasCercaOriengeViewSeccion();

                _ptoSeleccionMouseCentroCaraMuro = (_ptobarra1 + _ptobarra2) / 2;
                //  _ptoSeleccionMouseCaraMuro = (_ptobarra1 + _ptobarra2) / 2;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }

        private bool Util_IsBarraNOVertica(RebarShapeDrivenAccessor _barraGetShapeDrivenAccessor)
        {

            var listapto1 = _barraGetShapeDrivenAccessor.ComputeDrivingCurves().ToList();
            var curvaMAyorLargo = listapto1.MinBy(c => -c.ApproximateLength);

            if (Util.IsVertical(((Line)curvaMAyorLargo).Direction))
            {
                Util.ErrorMsg("Barra1 Seleccionada No debe ser Vertical");
                return false;
            }
            return true;
        }

        protected void M1_3_1_OrientacionSeleccion()
        {
            XYZ direccionSelecion = (_ptobarra2.GetXY0() - _ptobarra1.GetXY0()).Normalize();
            double valor = Util.GetProductoEscalar(direccionSelecion, _RightDirection);

            if (0 < valor)
            {
                _direccionSeleccionMouse = DireccionSeleccionMouse.IzqToDere;
                // _textoTag = "Izquierda";
            }
            else
            {
                _direccionSeleccionMouse = DireccionSeleccionMouse.DereToIzq;
                //_textoTag = "Derecha";
            }
        }

        protected void M1_3_2_ReasignarSiP1MayorZqueP2()
        {      //configuracion correcta   
               //  *     p2
               //  p1    *
               //siempre que el _ptobarra1 sea el de menor Z
            if (_ptobarra1.Z > _ptobarra2.Z)
            {

                XYZ _ptobarra2_aux = _ptobarra2;
                Rebar _barr2_aux = _barra2;
                double _anchoEstribo2Foot_aux = _anchoEstribo2Foot_extremoAextrem;

                _ptobarra2 = _ptobarra1;
                _barra2 = _barra1;
                _anchoEstribo2Foot_extremoAextrem = _anchoEstribo1Foot_extremoAextremo;

                //cambio
                _ptobarra1 = _ptobarra2_aux;
                _barra1 = _barr2_aux;
                _anchoEstribo1Foot_extremoAextremo = _anchoEstribo2Foot_aux;
            }
        }

        protected void M1_3_3_ReasignarSiP2MasCercaOriengeViewSeccion()
        {// si p2 superior es mas a la izq
         //configuracion correcta   
         //  *     p2
         //  p1    *

            XYZ direccionSelecion = (_ptobarra2.GetXY0() - _ptobarra1.GetXY0()).Normalize();
            double valor = Util.GetProductoEscalar(direccionSelecion, _RightDirection);

            if (valor < 0)
            {
                double zp1_aux = _ptobarra1.Z;
                double zp2_aux = _ptobarra2.Z;
                XYZ new_ptobarra1 = _ptobarra2.AsignarZ(zp1_aux);
                XYZ new_ptobarra2 = _ptobarra1.AsignarZ(zp2_aux);
                _ptobarra1 = new_ptobarra1;
                _ptobarra2 = new_ptobarra2;
            }


        }

        #endregion

        public bool M1_4_BuscarMuroPerpendicularVIew(XYZ ptoBUsqueda)
        {
            //mueve el to 50 cm hacia arriba respecto al pto inicial de la barra
            // ptoBUsqueda = ptoBUsqueda + new XYZ(0, 0, Util.CmToFoot(50));
            BuscarMurosoViga BuscarVigaMuro = new BuscarMurosoViga(_uiapp, Util.CmToFoot(45));
            XYZ ptobusquedaMuro = ptoBUsqueda +
                                _ViewNormalDirection6 * Util.CmToFoot(10);//mueve pto 2 cm separado de la cara del muero inicial. 2cm hacia la vista 

            _wallSeleccionado = BuscarVigaMuro.OBtenerRefrenciaMuroOViga(_view3D_paraBuscar, ptobusquedaMuro, -_ViewNormalDirection6);



            if (_wallSeleccionado == null)
            {
                Util.InfoMsg("Error al seleccionar viga host.Seleccionar viga manualmente");

                SeleccionarViga _SeleccionarViga = new SeleccionarViga();
                if (_SeleccionarViga.M1_3_SeleccionarViga(_uidoc))
                {
                    _wallSeleccionado = _SeleccionarViga._ElemetSelect;
                    BuscarVigaMuro.AsignarMuroOviga(_SeleccionarViga._ElemetSelect, new DatosMuroSeleccionadoDTO()
                    {
                        ptoSeleccionMouseCentroCaraMuro6 = _SeleccionarViga._ptoSeleccionMouseCentroCaraViga,
                        PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = _SeleccionarViga._ptoSeleccionMouseCentroCaraViga

                    });
                }
                else
                    return false;
            }


            if (_wallSeleccionado == null)
            {
                Util.ErrorMsg("Error a buscar viga host. Verificar rango de vista. err:10001");
                _espesorMuroVigaFoot = BuscarVigaMuro._espesorMuroFoot;
            }
            else
            { _espesorMuroVigaFoot = BuscarVigaMuro._espesorMuroFoot; }

            return (_wallSeleccionado == null ? false : true);
        }

        protected bool M1_4_2_BuscarPuntosSegunMargen(XYZ ptoBUsqueda)
        {
            try
            {
                if (_configuracionInicialEstriboDTO.TipoDiseñoEstriboViga == TipoDisenoEstriboVIga.SoloBarra) return true;

                double recub = ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT;

                var caraSUperior = _wallSeleccionado.ObtenerPLanarFAce_superior(false);
                _ptobarra2 = _ptobarra2.AsignarZ(caraSUperior.Origin.Z - recub);

                var caraInferior = _wallSeleccionado.ObtenerCaraInferior(false);
                _ptobarra1 = _ptobarra1.AsignarZ(caraInferior.Origin.Z + recub);

                var facemaScERCANO = _wallSeleccionado.ObtenerfaceMAsCercanaConDireccion_foot(ptoBUsqueda, _view.ViewDirection);

                if (facemaScERCANO == null) return false;

                var puntooInicial = facemaScERCANO.GetPtosIntersFaceUtilizarPlanoNh(_ptobarra1);
                var puntooFinal = facemaScERCANO.GetPtosIntersFaceUtilizarPlanoNh(_ptobarra2);

                //a)
                _ptobarra1 = puntooInicial - _view.ViewDirection * recub;
                _ptobarra2 = puntooFinal - _view.ViewDirection * recub;

                //b) obtener valores en Z

                BuscarFundacionLosaInf = new BuscarFundacionLosaV2(_uiapp, Util.CmToFoot(40));
                if (BuscarFundacionLosaInf.OBtenerRefrenciaLosa(_view3D_paraBuscar, _ptobarra1, new XYZ(0, 0, -1), TipoCaraObjeto.Inferior))
                {
                    Buscar_elementoEncontradoDTO elemetSelecc = BuscarFundacionLosaInf._listaFundLosaEncontrado.OrderBy(c => c.distancia).FirstOrDefault();

                    if (elemetSelecc._PlanarFace.FaceNormal.Z == -1)
                        _ptobarra1 = _ptobarra1.AsignarZ(elemetSelecc.PtoSObreCaraInferiorFundLosa.Z + recub);
                }



                BuscarFundacionLosaSup = new BuscarFundacionLosaV2(_uiapp, Util.CmToFoot(40));
                if (BuscarFundacionLosaSup.OBtenerRefrenciaLosa(_view3D_paraBuscar, _ptobarra2, new XYZ(0, 0, 1), TipoCaraObjeto.Superior))
                {
                    Buscar_elementoEncontradoDTO elemetSelecc = BuscarFundacionLosaSup._listaFundLosaEncontrado.OrderBy(c => c.distancia).FirstOrDefault();

                    if (elemetSelecc._PlanarFace.FaceNormal.Z == 1)
                        _ptobarra2 = _ptobarra2.AsignarZ(elemetSelecc.PtoSObreCaraSuperiorFundLosa.Z - recub);
                }

                var espesor = _wallSeleccionado.ObtenerEspesorConPtos_foot(ptoBUsqueda, -_view.ViewDirection);
                _anchoEstribo1Foot_extremoAextremo = espesor - recub * 2;
                _anchoEstribo2Foot_extremoAextrem = espesor - recub * 2;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }


        //  NOTA : ESTE VALOR DE producto escalar y entre ptos de una barra y su normal genera error enla barra(no se dibuja)
        //Util.GetProductoEscalar((-item._startPont_+ item._endPoint).Normalize(),norm)
        //0.00047686005754649097
        protected bool M1_5_GenerarVectores()
        {
            _direccion1DeEstribo = new XYZ(0, 0, -1); // direcicion 1 para la barra
            _direccionPerpenEntradoHaciaViewSecction = -Util.GetVectorPerpendicular2((_ptobarra2.GetXY0() - _ptobarra1.GetXY0()).Normalize()).Normalize(); // -_ViewNormalDirection6;// direccion 2 para para definir estribo 
            _largoRecorridoEstriboFoot = _ptobarra1.GetXY0().DistanceTo(_ptobarra2.GetXY0());
            _AnchoVisibleFoot = Math.Abs(_ptobarra1.Z - _ptobarra2.Z);
            return true;
        }

        protected void M1_6_NombreFamilaTAG()
        {

            switch (_configuracionInicialEstriboDTO.tipoConfiguracionEstribo)
            {

                case TipoConfiguracionEstribo.EstriboViga:
                    _nombreFamilia = "MRA Rebar_VIGA";
                    break;
                case TipoConfiguracionEstribo.EstriboViga_Lateral:
                    break;
                case TipoConfiguracionEstribo.EstriboViga_Traba:
                    break;
                case TipoConfiguracionEstribo.EstriboViga_Lateral_Traba:
                    break;
                case TipoConfiguracionEstribo.VigaTraba:
                    break;
            }

        }

        #endregion

        #region 2) datos VIgaDTO

        public List<EstriboMuroDTO> M2_ObtenerEstriboVigaDTO(Func<XYZ, XYZ, XYZ> funciotag)
        {
            // _ptobarra1 = _ptoSeleccionMouseCentroCaraMuro;
            _ptoTag = funciotag(_ptobarra1, _ptobarra2);

            List<BarraLateralesDTO> ListaLat = new List<BarraLateralesDTO>();

            if (_configuracionInicialEstriboDTO.IsLateral == true)
            {
                _configuracionInicialEstriboDTO.espesor = _espesorMuroVigaFoot;
                _configuracionInicialEstriboDTO.ElementoSeleccionado = _wallSeleccionado;
                ObtenerIntervalosLateralesViga_Service _obtenerIntervalosLaterales =
         new ObtenerIntervalosLateralesViga_Service(_uiapp, _view3D_paraBuscar, _configuracionInicialEstriboDTO, _ptobarra1, _ptobarra2, _view);
                ListaLat = _obtenerIntervalosLaterales.M3_ObtenerLateralesEstriboVigaDTO_buscarPAta();
            }


            M2_1_ObtenerTrabaMuroDTO();

            EstriboMuroDTO newvo_EstriboMuroDTO = new EstriboMuroDTO()
            {
                OrigenEstribo = _ptobarra1 + -_direccion1DeEstribo.Normalize() * _AnchoVisibleFoot, // desde donde se crea barra
                ElementHost = _wallSeleccionado,
                Espesor_ElementHostFoot = _espesorMuroVigaFoot,
                DiamtroBarraEnMM = _configuracionInicialEstriboDTO.DiamtroEstriboMM,
                EspaciamientoEntreEstriboFoot = Util.CmToFoot(_configuracionInicialEstriboDTO.espaciamientoEstriboCM),
                _anchoEstribo1Foot = _anchoEstribo1Foot_extremoAextremo,
                direccionParalelaViewSecctioin = _direccion1DeEstribo,
                direccionPerpenEntradoHaciaViewSecction = _direccionPerpenEntradoHaciaViewSecction,
                AnchoVisibleFoot = _AnchoVisibleFoot,
                largoRecorridoEstriboFoot = _largoRecorridoEstriboFoot,
                cantidadEstribo = _configuracionInicialEstriboDTO.cantidadEstribo,
                NombreFamilia = _nombreFamilia,
                tipoEstriboGenera = _configuracionInicialEstriboDTO.tipoEstriboGenera,
                Posi1TAg = _ptoTag,
                TextoAUXTraba = (_configuracionInicialEstriboDTO.IsTraba == false ? "" : _configuracionInicialEstriboDTO.ObtenerTextBarra_Borrar()),
                ListaLateralesDTO = ListaLat,
                ListaTrabasDTO = ListaTraba,
            };

            listaDeEstribos.Add(newvo_EstriboMuroDTO);

            return listaDeEstribos;

        }
        private void M2_1_ObtenerTrabaMuroDTO()
        {
            ListaTraba = new List<BarraTrabaDTO>();
            if (_configuracionInicialEstriboDTO.IsTraba == true)
            {
                var confTraba = M2_1_1_ObtenerDatosParaTraba();

                ObtenerIntervalosTrabaMuro_Service _obtenerIntervalosLaterales = new ObtenerIntervalosTrabaMuro_Service(confTraba);

                ListaTraba = _obtenerIntervalosLaterales.M3_ObtenerTrabaEstriboMuroDTO();
            }
        }

        private ConfiguracionBarraTrabaDTO M2_1_1_ObtenerDatosParaTraba()
        {

            XYZ aux_ptobarra1 = _ptobarra1;
            XYZ aux_ptobarra2 = _ptobarra2;
            //    double _espesorMuroVigaFoot = 0;
            // DireccionSeleccionMouse _direccionSeleccionMouse = DireccionSeleccionMouse.DereToIzq;

            return new ConfiguracionBarraTrabaDTO()
            {
                DiamtroTrabaEstriboMM = _configuracionInicialEstriboDTO.DiamtroEstriboMM,
                Ptobarra1 = aux_ptobarra1 + XYZ.BasisZ * Util.CmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM / 10.0f) * .9999 + _direccionPerpenEntradoHaciaViewSecction * Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM / 2),
                Ptobarra2 = aux_ptobarra2 + _direccionPerpenEntradoHaciaViewSecction * Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM / 2),
                EspesroMuroOVigaFoot = _espesorMuroVigaFoot,
                textoTraba = (_configuracionInicialEstriboDTO.IsTraba == false ? "" : _configuracionInicialEstriboDTO.ObtenerTextBarra_Borrar()),
                tipoTraba_posicion = _configuracionInicialEstriboDTO.tipoTraba_posicion,
                DireccionEntradoHaciaView = _direccionPerpenEntradoHaciaViewSecction,
                DireccionEnfierrrado = (_direccionSeleccionMouse == DireccionSeleccionMouse.DereToIzq ? -_view.RightDirection : _view.RightDirection),
                UbicacionTraba = (_direccionSeleccionMouse == DireccionSeleccionMouse.DereToIzq ? DireccionTraba.Derecha : DireccionTraba.Izquierda),
                CantidadTrabasTranversal = _configuracionInicialEstriboDTO.cantidadTraba + _configuracionInicialEstriboDTO.cantidadTraba_long,
                CantidadTrabasLongitudinal = 0,
                listaEspaciamientoTrabasTransversal = _configuracionInicialEstriboDTO.listaEspaciamientoTrabas
            };

        }

        #endregion

        public XYZ ObtenerPtoTagViga(XYZ aux_ptobarra1, XYZ aux_ptobarra2)
        {
            return (aux_ptobarra1 + aux_ptobarra2) / 2;
        }
    }
}
