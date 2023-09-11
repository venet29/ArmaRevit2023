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
using System.Linq;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;

using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Seleccion
{
    public class SeleccionPtosEstriboMuro : SeleccionarElementosV
    {
        public List<XYZ> _listaPtosPAthArea;
#pragma warning disable CS0108 // 'SeleccionPtosEstriboMuro._view' hides inherited member 'SeleccionarElementosV._view'. Use the new keyword if hiding was intended.
        protected readonly View _view;
#pragma warning restore CS0108 // 'SeleccionPtosEstriboMuro._view' hides inherited member 'SeleccionarElementosV._view'. Use the new keyword if hiding was intended.
        protected Rebar _barra1;
        protected double _diamtroBarra1Foot;
        protected double _anchoEstribo1Foot;
        protected XYZ _ptobarra1;
        protected XYZ _ptobarra2;
        protected Rebar _barra2;
        protected double _diamtroBarra2Foot;
        protected double _anchoEstribo2Foot;

#pragma warning disable CS0108 // 'SeleccionPtosEstriboMuro._view3D_paraBuscar' hides inherited member 'SeleccionarElementosV._view3D_paraBuscar'. Use the new keyword if hiding was intended.
        protected View3D _view3D_paraBuscar;
#pragma warning restore CS0108 // 'SeleccionPtosEstriboMuro._view3D_paraBuscar' hides inherited member 'SeleccionarElementosV._view3D_paraBuscar'. Use the new keyword if hiding was intended.
        protected DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO;
        protected ISeleccionarNivel _seleccionarNivel;
        protected DireccionSeleccionMouse _direccionSeleccionMouse;
        protected Element _wallSeleccionado;
        private XYZ _direccionParalelaViewSecctioin;
        private XYZ _direccionPerpenEntradoHaciaViewSecction;
        private double _AnchoVisibleFoot;
        private double _largoRecorridoEstriboFoot;
        private List<double> ListaLevelSeleccionado;
        private List<EstriboMuroDTO> listaDeEstribos;
        private string _nombreFamilia;
        private XYZ aux_ptobarra2;
        private XYZ _ptoTag;
        protected XYZ _direccionBarra;
        protected double _espesorMuroVigaFoot;
        private List<BarraLateralesDTO> ListaLaterales;
        private List<BarraTrabaDTO> ListaTraba;
        private XYZ aux_ptobarra1;
#pragma warning disable CS0169 // The field 'SeleccionPtosEstriboMuro._porRecubrimieto' is never used
        private XYZ _porRecubrimieto;
#pragma warning restore CS0169 // The field 'SeleccionPtosEstriboMuro._porRecubrimieto' is never used
        //private XYZ _ptoSeleccionMouseCentroCaraMuro_Inicial;

        public bool IsOk { get; set; }

        public SeleccionPtosEstriboMuro(UIApplication _uiapp, View3D _view3D_paraBuscar,
                                        DatosConfinamientoAutoDTO configuracionInicialEstriboDTO, ISeleccionarNivel _seleccionarNivel)
            : base(_uiapp)
        {
            // _uidoc = _uiapp.ActiveUIDocument;
            _listaPtosPAthArea = new List<XYZ>();
            _view = _uiapp.ActiveUIDocument.Document.ActiveView;

            _direccionParalelaViewSecctioin = _view.RightDirection;
            _direccionPerpenEntradoHaciaViewSecction = -_view.ViewDirection;

            this._view3D_paraBuscar = _view3D_paraBuscar;
            this._configuracionInicialEstriboDTO = configuracionInicialEstriboDTO;
            this._seleccionarNivel = _seleccionarNivel;
            ListaLevelSeleccionado = new List<double>();
            listaDeEstribos = new List<EstriboMuroDTO>();
        }

        public virtual bool M1_Ejecutar()
        {
            IsOk = false;
            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;

            if (!M1_3a_SeleccionarRebarElement()) return false;

            if (!M1_4b_1_OrientacionSeleccion()) return false;

            if (!M1_4_BuscarMuroPerpendicularVIew()) return false;

            if (!M1_4a_ReCalcularP1P2_conRecubrimieto()) return false;

            if (!M1_4b_SeleccionarRebarElement()) return false;

            if (!M1_4c_ObtenerRangoLevelSeleccionado()) return false;

            if (!M1_5_GenerarVectores()) return false;

            M1_6_NombreFamilaTAG();

            IsOk = true;
            return true;

        }
        #region comandos :M1 Ejecutar
        public virtual bool M1_3a_SeleccionarRebarElement()
        {

            try
            {
                ISelectionFilter filtroRebar = new FiltroRebar();

                //1
                Reference ref_baar1 = _uidoc.Selection.PickObject(ObjectType.Element, filtroRebar, "Seleccionar primera Barra:");
                _ptoSeleccionMouseCentroCaraMuro = ref_baar1.GlobalPoint;


                _barra1 = (Rebar)_doc.GetElement(ref_baar1);

    
                if (!AyudaObtenerNormarPlanoVisisible.Obtener(_barra1, _view)) return false;
                
                _diamtroBarra1Foot = _barra1.ObtenerDiametroFoot();
                RebarShapeDrivenAccessor _barraGetShapeDrivenAccessor = _barra1.GetShapeDrivenAccessor();
                double delta1 = _diamtroBarra1Foot * 1.5 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 2;
                _anchoEstribo1Foot = _barraGetShapeDrivenAccessor.ArrayLength + delta1;
                // _anchoEstribo1Foot = _barraGetShapeDrivenAccessor.ArrayLength;

                if (!IsBarraVErtica(_barra1)) return false;

                Reference ref_baar2 = _uidoc.Selection.PickObject(ObjectType.Element, filtroRebar, "Seleccionar Segunda Barra Barra:");
                _ptobarra2 = ref_baar2.GlobalPoint;
                _ptobarra2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(AyudaObtenerNormarPlanoVisisible.FaceNormal, _ptoSeleccionMouseCentroCaraMuro, _ptobarra2);

                _barra2 = (Rebar)_doc.GetElement(ref_baar2);
                _diamtroBarra2Foot = _barra2.ObtenerDiametroFoot();
                RebarShapeDrivenAccessor _barraGetShapeDrivenAccessor2 = _barra2.GetShapeDrivenAccessor();
                double delta2 = _diamtroBarra2Foot * 1.5 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 2;
                _anchoEstribo2Foot = _barraGetShapeDrivenAccessor2.ArrayLength + delta2;
                //_anchoEstribo2Foot = _barraGetShapeDrivenAccessor2.ArrayLength;

                if (!IsBarraVErtica(_barra2)) return false;

                ///modificacnoes ajustes
                _anchoEstribo1Foot = Math.Max(_anchoEstribo1Foot, _anchoEstribo2Foot);
                _anchoEstribo2Foot = _anchoEstribo1Foot;
                _direccionBarra = (_ptobarra2.GetXY0() - _ptoSeleccionMouseCentroCaraMuro.GetXY0()).Normalize();


            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }

        public bool M1_3a_SeleccionarRebarMuroVigaElement()
        {
            try
            {
                ISelectionFilter filtroRebar = new FiltroRebar();

                //1
                //  Reference ref_baar1 = _uidoc.Selection.PickObject(ObjectType.Element, filtroRebar, "Seleccionar primera Barra:");
                //  _ptoSeleccionMouseCentroCaraMuro = ref_baar1.GlobalPoint;

                ISelectionFilter filtroMuro = new FiltroVIga_Muro_Rebar_Columna();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                Reference ref_baar1 = _uidoc.Selection.PickObject(ObjectType.PointOnElement, "Seleccionar primera Barra:");
                _ptoSeleccionMouseCentroCaraMuro = ref_baar1.GlobalPoint;

                Element _barraEleme1 = _doc.GetElement(ref_baar1);

                if (_barraEleme1 is Rebar)
                {
                    _barra1 = (Rebar)_doc.GetElement(ref_baar1);
                    _diamtroBarra1Foot = _barra1.ObtenerDiametroFoot();
                    RebarShapeDrivenAccessor _barraGetShapeDrivenAccessor = _barra1.GetShapeDrivenAccessor();
                    double delta1 = _diamtroBarra1Foot * 1.5 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 2;
                    _anchoEstribo1Foot = _barraGetShapeDrivenAccessor.ArrayLength + delta1;
                    if (!IsBarraVErtica(_barra1)) return false;

                    if (!AyudaObtenerNormarPlanoVisisible.Obtener(_barra1, _view)) return false;
                }
                else
                {
                    if (!AyudaObtenerNormarPlanoVisisible.Obtener(_barraEleme1, _view)) return false;
                }
                // _anchoEstribo1Foot = _barraGetShapeDrivenAccessor.ArrayLength;
                //-------------------------------------------------------------------------------------------------------------------


                Reference ref_baar2 = _uidoc.Selection.PickObject(ObjectType.Element, filtroRebar, "Seleccionar Segunda Barra Barra:");
                _ptobarra2 = ref_baar2.GlobalPoint;
                _ptobarra2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(AyudaObtenerNormarPlanoVisisible.FaceNormal, _ptoSeleccionMouseCentroCaraMuro, _ptobarra2);

                _barra2 = (Rebar)_doc.GetElement(ref_baar2);

                Element _barraEleme2 = _doc.GetElement(ref_baar2);
                if (_barraEleme2 is Rebar)
                {
                    _diamtroBarra2Foot = _barra2.ObtenerDiametroFoot();
                    RebarShapeDrivenAccessor _barraGetShapeDrivenAccessor2 = _barra2.GetShapeDrivenAccessor();
                    double delta2 = _diamtroBarra2Foot * 1.5 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 2;
                    _anchoEstribo2Foot = _barraGetShapeDrivenAccessor2.ArrayLength + delta2;
                    //_anchoEstribo2Foot = _barraGetShapeDrivenAccessor2.ArrayLength;

                    if (!IsBarraVErtica(_barra2)) return false;
                }
                ///modificacnoes ajustes
                _anchoEstribo1Foot = Math.Max(_anchoEstribo1Foot, _anchoEstribo2Foot);
                _anchoEstribo2Foot = _anchoEstribo1Foot;
                _direccionBarra = (_ptobarra2.GetXY0() - _ptoSeleccionMouseCentroCaraMuro.GetXY0()).Normalize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M1_4_BuscarMuroPerpendicularVIew()
        {
            _wallSeleccionado = _doc.GetElement(_barra1.GetHostId());

            if (_wallSeleccionado == null) return true;
            _espesorMuroVigaFoot = 0;
            if (_wallSeleccionado is Wall)
            {
                //---muro buscado hacia el centro de muro
                _espesorMuroVigaFoot = (_wallSeleccionado as Wall).ObtenerEspesorMuroFoot(_doc);
            }
            else if (_wallSeleccionado is FamilyInstance)
            {
                FamilyInstance _FamilyInstanceSelec = ((FamilyInstance)_wallSeleccionado);

                if (_FamilyInstanceSelec.StructuralType == StructuralType.Column) //viga
                    _espesorMuroVigaFoot = _FamilyInstanceSelec.ObtenerEspesorConPtos_foot(_ptoSeleccionMouseCentroCaraMuro, _view.ViewDirection);
                else if (_FamilyInstanceSelec.StructuralType == StructuralType.Beam)
                    _espesorMuroVigaFoot = ((FamilyInstance)_wallSeleccionado).ObtenerEspesorConPtos_foot(_ptoSeleccionMouseCentroCaraMuro, _view.ViewDirection);
                //---muro buscado hacia el centro de muro
            }
            if (_espesorMuroVigaFoot == 0)
            {
                _espesorMuroVigaFoot = _wallSeleccionado.ObtenerEspesorConCaraVerticalVIsible_foot(_view);
            }
            return (_espesorMuroVigaFoot > 0 ? true : false);
        }

        //desactivado pq se usa 
        protected bool M1_4a_ReCalcularP1P2_conRecubrimieto()
        {
            try
            {
                double maxDiamtro = Math.Max(_diamtroBarra1Foot, _diamtroBarra2Foot);
                if (_direccionSeleccionMouse == DireccionSeleccionMouse.IzqToDere)
                {
                    _ptoSeleccionMouseCentroCaraMuro = _ptoSeleccionMouseCentroCaraMuro + _view.ViewDirection * maxDiamtro - _view.RightDirection * (_diamtroBarra1Foot * 1 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 1);
                    _ptobarra2 = _ptobarra2 + _view.ViewDirection * maxDiamtro + _view.RightDirection * (_diamtroBarra2Foot * 1 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 1);
                }
                else
                {
                    _ptoSeleccionMouseCentroCaraMuro = _ptoSeleccionMouseCentroCaraMuro + _view.ViewDirection * maxDiamtro + _view.RightDirection * (_diamtroBarra1Foot * 1 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 1);
                    _ptobarra2 = _ptobarra2 + _view.ViewDirection * maxDiamtro - _view.RightDirection * (_diamtroBarra2Foot * 1 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) * 1);
                }
            }
            catch (Exception)
            {
                return false; ;
            }
            return true;
        }

        public bool M1_4b_SeleccionarRebarElement()
        {
            try
            {
                M1_4b_2_ReasignarSiP1MayorZqueP2();
                M1_4b_3_ReasignarSiP2MasCercaOriengeViewSeccion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        protected bool IsBarraVErtica(Rebar _rebar)
        {
            double diamBarraFoot = _rebar.ObtenerDiametroFoot();
            RebarShapeDrivenAccessor _barraGetShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
            _anchoEstribo1Foot = _barraGetShapeDrivenAccessor.ArrayLength + diamBarraFoot + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM);

            if (!AyudaCurveRebar.GetPrimeraRebarCurves(_rebar)) return false;

            var listapto1 = AyudaCurveRebar.ListacurvesSoloLineas[0];

            var curvaMAyorLargo = listapto1.MinBy(c => -c.ApproximateLength);

            if (!Util.IsVertical(((Line)curvaMAyorLargo).Direction))
            {
                Util.ErrorMsg("Barra1 Seleccionada No es completamente Vertical");
                return false;
            }
            return true;
        }

        protected bool M1_4b_1_OrientacionSeleccion()
        {
            try
            {
                XYZ direccionSelecion = (_ptobarra2.GetXY0() - _ptoSeleccionMouseCentroCaraMuro.GetXY0()).Normalize();
                double valor = Util.GetProductoEscalar(direccionSelecion, _RightDirection);

                if (0 < valor)
                {
                    _direccionSeleccionMouse = DireccionSeleccionMouse.IzqToDere;
                }
                else
                {
                    _direccionSeleccionMouse = DireccionSeleccionMouse.DereToIzq;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        protected void M1_4b_2_ReasignarSiP1MayorZqueP2()
        {
            //siempre que el _ptobarra1 sea el de menor Z
            if (_ptoSeleccionMouseCentroCaraMuro.Z > _ptobarra2.Z)
            {

                XYZ _ptobarra2_aux = _ptobarra2;
                Rebar _barr2_aux = _barra2;
                double _anchoEstribo2Foot_aux = _anchoEstribo2Foot;

                _ptobarra2 = _ptoSeleccionMouseCentroCaraMuro;
                _barra2 = _barra1;
                _anchoEstribo2Foot = _anchoEstribo1Foot;

                //cambio
                _ptoSeleccionMouseCentroCaraMuro = _ptobarra2_aux;
                _barra1 = _barr2_aux;
                _anchoEstribo1Foot = _anchoEstribo2Foot_aux;

            }
        }

        protected void M1_4b_3_ReasignarSiP2MasCercaOriengeViewSeccion()
        {
            XYZ direccionSelecion = (_ptobarra2.GetXY0() - _ptoSeleccionMouseCentroCaraMuro.GetXY0()).Normalize();
            double valor = Util.GetProductoEscalar(direccionSelecion, _RightDirection);
            if (0 > valor)
            {
                double zp1_aux = _ptoSeleccionMouseCentroCaraMuro.Z;
                double zp2_aux = _ptobarra2.Z;
                XYZ new_ptobarra1 = _ptobarra2.AsignarZ(zp1_aux);
                XYZ new_ptobarra2 = _ptoSeleccionMouseCentroCaraMuro.AsignarZ(zp2_aux);
                _ptoSeleccionMouseCentroCaraMuro = new_ptobarra1;
                _ptobarra2 = new_ptobarra2;
            }


        }
        protected bool M1_4c_ObtenerRangoLevelSeleccionado()
        {
            try
            {

                if (_configuracionInicialEstriboDTO.PtoSuperior == TipoSeleccionMouse.mouse && _configuracionInicialEstriboDTO.PtoInferior == TipoSeleccionMouse.mouse)
                {
                    ListaLevelSeleccionado.Add(_ptoSeleccionMouseCentroCaraMuro.Z);
                    ListaLevelSeleccionado.Add(_ptobarra2.Z);
                }
                else
                {
                    ListaLevelSeleccionado = ObtenerIntervalLevel(_seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(_doc.ActiveView),
                        Math.Min(_ptoSeleccionMouseCentroCaraMuro.Z, _ptobarra2.Z), Math.Max(_ptoSeleccionMouseCentroCaraMuro.Z, _ptobarra2.Z));

                    if (_configuracionInicialEstriboDTO.PtoSuperior == TipoSeleccionMouse.nivel && _configuracionInicialEstriboDTO.PtoInferior == TipoSeleccionMouse.mouse)
                    {
                        double ValorMin = ListaLevelSeleccionado.Min();
                        if (Math.Abs(ValorMin - _ptoSeleccionMouseCentroCaraMuro.Z) > ConstNH.CONST_DISTANCIA_MIN_SELECCIO_MOUSE_FOOT)
                        {
                            ListaLevelSeleccionado.Add(_ptoSeleccionMouseCentroCaraMuro.Z);
                            ListaLevelSeleccionado = ListaLevelSeleccionado.OrderBy(c => c).ToList();
                        }
                    }
                    else if (_configuracionInicialEstriboDTO.PtoSuperior == TipoSeleccionMouse.mouse && _configuracionInicialEstriboDTO.PtoInferior == TipoSeleccionMouse.nivel)
                    {
                        double ValorMaximo = ListaLevelSeleccionado.Max();
                        if (Math.Abs(ValorMaximo - _ptobarra2.Z) > ConstNH.CONST_DISTANCIA_MIN_SELECCIO_MOUSE_FOOT)
                        {
                            ListaLevelSeleccionado.Add(_ptobarra2.Z);
                            ListaLevelSeleccionado = ListaLevelSeleccionado.OrderBy(c => c).ToList();
                        }
                    }


                }



            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al seleccionar los nivel");
                ListaLevelSeleccionado.Clear();
            }

            return (ListaLevelSeleccionado.Count > 0 ? true : false);
        }


        protected bool M1_5_GenerarVectores()
        {
            _direccionParalelaViewSecctioin = _RightDirection;
            _direccionPerpenEntradoHaciaViewSecction = -_ViewNormalDirection6;
            //_anchoEstribo1Foot = _espesorMuroVigaFoot - 2 * Util.MmToFoot(ConstantesGenerales.CONST_RECUBRIMIENTO_BAse2cm_MM) - Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM);
            _AnchoVisibleFoot = _ptoSeleccionMouseCentroCaraMuro.GetXY0().DistanceTo(_ptobarra2.GetXY0());
            //  + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) + _diamtroBarra1Foot/2 + _diamtroBarra2Foot/2;
            _largoRecorridoEstriboFoot = Math.Abs(_ptoSeleccionMouseCentroCaraMuro.Z - _ptobarra2.Z);
            return true;
        }

        protected void M1_6_NombreFamilaTAG()
        {

            switch (_configuracionInicialEstriboDTO.tipoConfiguracionEstribo)
            {
                case TipoConfiguracionEstribo.Estribo:
                    _nombreFamilia = "MRA Rebar_PILAR";
                    break;
                case TipoConfiguracionEstribo.Estribo_Lateral:
                    break;
                case TipoConfiguracionEstribo.Estribo_Traba:
                    break;
                case TipoConfiguracionEstribo.Estribo_Lateral_Traba:
                    break;
                case TipoConfiguracionEstribo.Traba:
                    break;
                case TipoConfiguracionEstribo.EstriboMuro:
                    _nombreFamilia = "MRA Rebar_PILAR";
                    break;
                case TipoConfiguracionEstribo.EstriboMuro_Lateral:
                    break;
                case TipoConfiguracionEstribo.EstriboMuro_Traba:
                    break;
                case TipoConfiguracionEstribo.EstriboMuro_Lateral_Traba:
                    break;
                case TipoConfiguracionEstribo.EstriboMuroTraba:
                    break;
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


        public List<EstriboMuroDTO> M2_ObtenerEstriboMuroDTO()
        {
            _ptobarra1 = _ptoSeleccionMouseCentroCaraMuro;
            //_AnchoVisibleFoot = _ptoSeleccionMouseCentroCaraMuro.GetXY0().DistanceTo(_ptobarra2.GetXY0()) + _diamtroBarra2Foot / 2 + _diamtroBarra1Foot / 2;

            for (int i = 0; i < ListaLevelSeleccionado.Count - 1; i++)
            {
                M2_1_ObtenerDatosEstribo(i);

                // este opcion es para que no se vuelva a buscar el muro si solo se dibuja el estribo en un piso,osea seleccion mouse-mouse => solo dos nieveles)
                // si hay mas nieveles entoences se busca muro en cada nivel 
                if(ListaLevelSeleccionado.Count>2)
                    M2_2_ObtenerDatosEstribo();

                M2_2_ObtenerLateraleMuroDTO();

                M2_3_ObtenerTrabaMuroDTO();

                EstriboMuroDTO newvo_EstriboMuroDTO = new EstriboMuroDTO()
                {
                    OrigenEstribo = aux_ptobarra1,
                    ElementHost = _wallSeleccionado,
                    Espesor_ElementHostFoot = _espesorMuroVigaFoot,
                    DiamtroBarraEnMM = _configuracionInicialEstriboDTO.DiamtroEstriboMM,
                    EspaciamientoEntreEstriboFoot = Util.CmToFoot(_configuracionInicialEstriboDTO.espaciamientoEstriboCM),
                    _anchoEstribo1Foot = _anchoEstribo1Foot,
                    direccionParalelaViewSecctioin = _direccionParalelaViewSecctioin,
                    direccionPerpenEntradoHaciaViewSecction = _direccionPerpenEntradoHaciaViewSecction,
                    AnchoVisibleFoot = _AnchoVisibleFoot,
                    largoRecorridoEstriboFoot = _largoRecorridoEstriboFoot,
                    cantidadEstribo = _configuracionInicialEstriboDTO.cantidadEstribo,
                    NombreFamilia = _nombreFamilia,
                    tipoEstriboGenera = _configuracionInicialEstriboDTO.tipoEstriboGenera,
                    ListaLateralesDTO = ListaLaterales,
                    ListaTrabasDTO = ListaTraba,
                    direccionBarra = (_direccionSeleccionMouse == DireccionSeleccionMouse.DereToIzq ? _direccionBarra : -_direccionBarra),
                    direccionTag = -_direccionBarra,
                    DireccionSeleccionConMouse = _direccionSeleccionMouse,
                    TextoAUXTraba = (_configuracionInicialEstriboDTO.IsTraba == false ? "" : _configuracionInicialEstriboDTO.ObtenerTextBarra_Borrar()),
                    Posi1TAg = _ptoTag
                };

                listaDeEstribos.Add(newvo_EstriboMuroDTO);
            }

            return listaDeEstribos;

        }
        #region M2_ObtenerEstriboMuroDTO
        protected void M2_1_ObtenerDatosEstribo(int i)
        {

            XYZ desplazamietoCubriendoBarraVertical = -_direccionBarra * (Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) + _diamtroBarra1Foot)
                                          - _direccionPerpenEntradoHaciaViewSecction * (Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM) + _diamtroBarra1Foot);

            desplazamietoCubriendoBarraVertical = XYZ.Zero;

            aux_ptobarra1 = _ptoSeleccionMouseCentroCaraMuro.AsignarZ(ListaLevelSeleccionado[i]) + (Util.IsImPar(i) == true ?
                                                                                                        _direccionBarra * ConstNH.DESPLAZA_LATERALES_TRASLAPO :
                                                                                                        XYZ.Zero);
            aux_ptobarra1 = aux_ptobarra1 + desplazamietoCubriendoBarraVertical;

            aux_ptobarra2 = _ptobarra2.AsignarZ(ListaLevelSeleccionado[i + 1]) + (Util.IsImPar(i) == true ?
                                                                                     _direccionBarra * ConstNH.DESPLAZA_LATERALES_TRASLAPO :
                                                                                     XYZ.Zero);
            aux_ptobarra2 = aux_ptobarra2 + desplazamietoCubriendoBarraVertical;

            _ptoTag = M2_1_1_ObtenerPtoTagConfinamientoyPilar(aux_ptobarra1, aux_ptobarra2);
            _largoRecorridoEstriboFoot = Math.Abs(aux_ptobarra1.Z - aux_ptobarra2.Z);
        }

        private bool M2_2_ObtenerDatosEstribo()
        {
            try
            {
                var XpuntoBUScar = ((aux_ptobarra1 + aux_ptobarra2) / 2)
                                    .AsignarZ(aux_ptobarra1.Z + Util.CmToFoot(50))+_view.ViewDirection* Util.CmToFoot(5);

                BuscarMuros BuscarMuros = new BuscarMuros(_uiapp, Util.CmToFoot(45));
                XYZ ptoSobreMuro = XYZ.Zero;

                Element _wallDeBarras = _wallSeleccionado;
                (_wallSeleccionado, _espesorMuroVigaFoot, ptoSobreMuro)
                    = BuscarMuros.OBtenerRefrenciaMuro(_view3D_paraBuscar, XpuntoBUScar, -_view.ViewDirection);

                if (_wallSeleccionado == null && _wallDeBarras!=null)
                {
                    BuscarMuros.AsignarMuro(_wallDeBarras);
                    _wallSeleccionado = _wallDeBarras;
                    _espesorMuroVigaFoot =(_wallDeBarras as Wall).ObtenerEspesorMuroFoot(_doc);

                    //BuscarMuros.CentroCaraNormalInversaVistaMuro();
                    // ptoSobreMuro = BuscarMuros.CentroCaraNormalInversaVista;
                    (bool resulta, PlanarFace _planar) = _wallDeBarras.ObtenerCaraVerticalVIsible(_view);
                    ptoSobreMuro = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);
                  
                }

                if (_wallSeleccionado == null)
                {
                    Util.ErrorMsg("Error al obtener muro host para Estribo");
                    return false;
                }


                double recub = ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT;

                XYZ ptoSobreMuro_menosRecub = ptoSobreMuro - _view.ViewDirection * recub;

                if (!AyudaObtenerNormarPlanoVisisible.Obtener(_wallSeleccionado, _view)) return false;

                _ptoSeleccionMouseCentroCaraMuro = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(AyudaObtenerNormarPlanoVisisible.FaceNormal, ptoSobreMuro_menosRecub, aux_ptobarra1);
                aux_ptobarra1 = _ptoSeleccionMouseCentroCaraMuro;
                aux_ptobarra2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(AyudaObtenerNormarPlanoVisisible.FaceNormal, ptoSobreMuro_menosRecub, _ptobarra2);




                _anchoEstribo1Foot = _espesorMuroVigaFoot - recub * 2;
                _anchoEstribo2Foot = _espesorMuroVigaFoot - recub * 2;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }


        public XYZ M2_1_1_ObtenerPtoTagConfinamientoyPilar(XYZ aux_ptobarra1, XYZ aux_ptobarra2)
        {
            XYZ posicionTag = XYZ.Zero;
            if (_direccionSeleccionMouse == DireccionSeleccionMouse.DereToIzq)
            {
                // posicionTag = aux_ptobarra1 - _direccionBarra * (_AnchoVisibleFoot + Util.CmToFoot(12)) + new XYZ(0, 0, Util.CmToFoot(100));
                posicionTag = aux_ptobarra2.AsignarZ(aux_ptobarra1.Z) + -_direccionBarra * Util.CmToFoot(10) + new XYZ(0, 0, Util.CmToFoot(100));
            }
            else
                posicionTag = aux_ptobarra1 + -_direccionBarra * Util.CmToFoot(10) + new XYZ(0, 0, Util.CmToFoot(100));

            return posicionTag;
        }



        public void M2_2_ObtenerLateraleMuroDTO()
        {
            ListaLaterales = new List<BarraLateralesDTO>();

            if (_configuracionInicialEstriboDTO.IsLateral == true)
            {
                //   XYZ ptoInicialLateral_aux_ptobarra1 = _ptoSeleccionMouseCentroCaraMuro_Inicial + _porRecubrimieto;
                XYZ ptoInicialLateral_aux_ptobarra1 = aux_ptobarra1;// + _porRecubrimieto;

                //ObtenerIntervalosLateralesMuro_Service _obtenerIntervalosLaterales = new ObtenerIntervalosLateralesMuro_Service(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM, aux_ptobarra1, aux_ptobarra2);
                ObtenerIntervalosLateralesMuro_Service _obtenerIntervalosLaterales = new ObtenerIntervalosLateralesMuro_Service(_configuracionInicialEstriboDTO, ptoInicialLateral_aux_ptobarra1, aux_ptobarra2);
                ListaLaterales = _obtenerIntervalosLaterales.M3_ObtenerLateralesEstriboMuroDTO();
            }
        }
        public void M2_3_ObtenerTrabaMuroDTO()
        {
            ListaTraba = new List<BarraTrabaDTO>();



            if (_configuracionInicialEstriboDTO.IsTraba == true)
            {
                var confTraba = M2_3_1_ObtenerDatosParaTraba();

                ObtenerIntervalosTrabaMuro_Service _obtenerIntervalosLaterales = new ObtenerIntervalosTrabaMuro_Service(confTraba);

                ListaTraba = _obtenerIntervalosLaterales.M3_ObtenerTrabaEstriboMuroDTO();
            }
        }



        protected ConfiguracionBarraTrabaDTO M2_3_1_ObtenerDatosParaTraba()
        {

            return new ConfiguracionBarraTrabaDTO()
            {
                DiamtroTrabaEstriboMM = _configuracionInicialEstriboDTO.DiamtroEstriboMM,
                Ptobarra1 = aux_ptobarra1 + XYZ.BasisZ * Util.CmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM / 10.0f) * .9999 + _direccionPerpenEntradoHaciaViewSecction * Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM / 2),
                Ptobarra2 = aux_ptobarra2 + _direccionPerpenEntradoHaciaViewSecction * Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM / 2),
                EspesroMuroOVigaFoot = _espesorMuroVigaFoot,
                textoTraba = (_configuracionInicialEstriboDTO.IsTraba == false ? "" : _configuracionInicialEstriboDTO.ObtenerTextBarra_Borrar()),
                tipoTraba_posicion = _configuracionInicialEstriboDTO.tipoTraba_posicion,
                DireccionEntradoHaciaView = _direccionPerpenEntradoHaciaViewSecction,
                DireccionEnfierrrado = (_direccionSeleccionMouse == DireccionSeleccionMouse.DereToIzq ? -_direccionParalelaViewSecctioin : _direccionParalelaViewSecctioin),
                UbicacionTraba = (_direccionSeleccionMouse == DireccionSeleccionMouse.DereToIzq ? DireccionTraba.Derecha : DireccionTraba.Izquierda),
                CantidadTrabasTranversal = _configuracionInicialEstriboDTO.cantidadTraba,
                CantidadTrabasLongitudinal = 0,
                listaEspaciamientoTrabasTransversal = _configuracionInicialEstriboDTO.listaEspaciamientoTrabas
            };

        }
        #endregion



    }
}
