using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.ConfiguracionInicial.Elemento;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion
{
    public class SeleccionPtosEstriboConfinamientoAuto : SeleccionarElementosV
    {
        public List<XYZ> _listaPtosPAthArea;
        private IntervalosConfinaDTOAuto newIntervalosConfinaminetoDTOAuto;
        private DatosConfinamientoAutoDTO _datosConfinaDTO;

        public EstriboMuroDTO _EstriboMuroDTO { get; set; }
        public bool Isok { get; set; }
        private XYZ ptoDiseñoMuro;
        private string _nombreFamilia;
        private double _largoRecorridoEstriboFoot;
        private XYZ _ptoTag;
        private XYZ _direccionParalelaViewSecctioin;
        private XYZ _direccionPerpenEntradoHaciaViewSecction;
        private UtilStopWatch _utilStopWatch;

        //  private readonly DatosMallasDTO _datosMallasDTO;

        public bool IsOk { get; set; }

        private XYZ _direccionBarra;
        private List<BarraLateralesDTO> ListaLaterales;
        private List<BarraTrabaDTO> ListaTraba;
        private XYZ _p1;
        private XYZ _PtoTAgAux;
        private XYZ _p2;

        public EstriboMuroDTO newvo_EstriboMuroDTO { get; set; }
        public double _AnchoVisibleFoot { get; private set; }
        public double _anchoEstribo1Foot { get; private set; }

        public SeleccionPtosEstriboConfinamientoAuto(UIApplication _uiapp, IntervalosConfinaDTOAuto newIntervalosConfinaminetoDTOAuto, View3D view3D_paraBuscar, bool ConTransaccionAlCrearSketchPlane = true) : base(_uiapp, ConTransaccionAlCrearSketchPlane)
        {
            // _uidoc = _uiapp.ActiveUIDocument;
            _listaPtosPAthArea = new List<XYZ>();
            _view3D_paraBuscar = view3D_paraBuscar;
            this.newIntervalosConfinaminetoDTOAuto = newIntervalosConfinaminetoDTOAuto;
            _datosConfinaDTO = newIntervalosConfinaminetoDTOAuto._datosConfinaDTO; ;
            this.IsOk = true;

            _view = _uiapp.ActiveUIDocument.Document.ActiveView;

            _direccionParalelaViewSecctioin = _view.RightDirection;
            _direccionPerpenEntradoHaciaViewSecction = -_view.ViewDirection;
            //  this._datosMallasDTO = _datosMallasDTO;

            _utilStopWatch = new UtilStopWatch();

        }



        public bool Ejecutar_SeleccionarEstriboPtoAuto()
        {
            try
            {

                if (!M2_1_ObtenerDatosEstribo()) return false;

                M2_2_ObtenerLateraleMuroDTO();

                M2_3_ObtenerTrabaMuroDTO();

                newvo_EstriboMuroDTO = new EstriboMuroDTO()
                {
                    OrigenEstribo = newIntervalosConfinaminetoDTOAuto.ListaPtos[0],
                    ElementHost = _ElemetSelect,
                    Espesor_ElementHostFoot = _espesorMuroFoot,
                    DiamtroBarraEnMM = _datosConfinaDTO.DiamtroEstriboMM,
                    EspaciamientoEntreEstriboFoot = Util.CmToFoot(_datosConfinaDTO.espaciamientoEstriboCM),
                    _anchoEstribo1Foot = _anchoEstribo1Foot,
                    direccionParalelaViewSecctioin = _RightDirection,
                    direccionPerpenEntradoHaciaViewSecction = -_ViewNormalDirection6,
                    AnchoVisibleFoot = _AnchoVisibleFoot,
                    largoRecorridoEstriboFoot = _largoRecorridoEstriboFoot,
                    cantidadEstribo = ObtenerTextoEstribo(_datosConfinaDTO.cantidadEstribo),
                    NombreFamilia = _nombreFamilia,
                    tipoEstriboGenera = _datosConfinaDTO.tipoEstriboGenera,
                    ListaLateralesDTO = ListaLaterales,
                    ListaTrabasDTO = ListaTraba,
                    direccionBarra = (_datosConfinaDTO.direccionSeleccionMOuse == DireccionSeleccionMouse.DereToIzq ? _direccionBarra : -_direccionBarra),
                    direccionTag = _direccionBarra,
                    DireccionSeleccionConMouse = _datosConfinaDTO.direccionSeleccionMOuse,
                    TextoAUXTraba = (_datosConfinaDTO.IsTraba == false ? "" : _datosConfinaDTO.ObtenerTextBarra_Borrar()),
                    Posi1TAg = _ptoTag
                };

            }
            catch (Exception)
            {
                IsOk = false;
            }
            return IsOk;
        }

        private bool M2_1_ObtenerDatosEstribo() //solo se usa en estribo de muro
        {
            try
            {

                _p1 = newIntervalosConfinaminetoDTOAuto.ListaPtos[0];
                _PtoTAgAux = _p1.ObtenerCopia();
                _p2 = newIntervalosConfinaminetoDTOAuto.ListaPtos[1];
                XYZ ptoCentro = _datosConfinaDTO.centroPier.GetXYZ();
                _direccionBarra = (_datosConfinaDTO.direccionSeleccionMOuse == DireccionSeleccionMouse.DereToIzq ?
                                                                                    (_p2.GetXY0() - _p1.GetXY0()) :
                                                                                    (_p2.GetXY0() - _p1.GetXY0())).Normalize();

                _AnchoVisibleFoot = _p1.GetXY0().DistanceTo(_p2.GetXY0()) - Util.MmToFoot(_datosConfinaDTO.DiamtroEstriboMM / 10);

                BuscarFundaciones(ptoCentro);

                M1_ObtenerPtoEnmuroAuto(ptoCentro);


                (bool result, dynamic dina) = AnalisisMuros.DesAsignar(_ElemetSelect);
                if (result)
                {
                    if (dina.tipoAnononimo == ConstNH.CONST_TIPO_ANONIMO_ESTRIBOCORONACION)
                    {
                        if (dina.IsSobreCoronacion == "si")
                        {
                            if (dina.AlturaMaxima - _p2.Z < Util.CmToFoot(150))
                            {
                                _p2 = _p2.AsignarZ(((double)dina.AlturaMaxima) - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM));
                                _datosConfinaDTO.IsExtenderLatFin = false;
                            }
                        }
                        else if (dina.IsCoronacion == "si")
                        {
                            _datosConfinaDTO.IsExtenderLatFin = false;
                        }
                    }
                }


                if (_ptoSeleccionMouseCentroCaraMuro == null) return false;
                if (_ptoSeleccionMouseCentroCaraMuro.IsAlmostEqualTo(XYZ.Zero)) return false;

                if (!M1_4_BuscarPtoInicioBase(_p1)) return IsOk = false;
                newIntervalosConfinaminetoDTOAuto.ListaPtos[0] = ptoDiseñoMuro + (-_ViewNormalDirection6) * ConstNH.RECUBRIMIENTO_MALLA_foot;

                if (!M1_4_BuscarPtoInicioBase(_p2)) return IsOk = false;

                newIntervalosConfinaminetoDTOAuto.ListaPtos[1] = ptoDiseñoMuro + (-_ViewNormalDirection6) * ConstNH.RECUBRIMIENTO_MALLA_foot;

                M1_6_NombreFamilaTAG();



                _largoRecorridoEstriboFoot = Math.Abs(newIntervalosConfinaminetoDTOAuto.ListaPtos[0].Z - newIntervalosConfinaminetoDTOAuto.ListaPtos[1].Z);

                _ptoTag = ObtenerPtoTagConfinamientoyPilar(_PtoTAgAux);


                _anchoEstribo1Foot = _espesorMuroFoot - ConstNH.RECUBRIMIENTO_MALLA_foot * 2;

            }
            catch (Exception)
            {
                Util.ErrorMsg($"Error {errNh.e10003} ");
                return false;
            }
            return true;
        }

        private void BuscarFundaciones(XYZ ptoCentro)
        {
            try
            {
                XYZ PtoBusqueda_zmasbajo = ptoCentro.AsignarZ(Math.Min(_p1.Z, _p2.Z));
                BuscarFundacionLosa BuscarMuros = new BuscarFundacionLosa(_uiapp, UtilBarras.largo_L9_DesarrolloFoot_diamMM(_datosConfinaDTO.DiamtroEstriboMM));
                if (BuscarMuros.OBtenerRefrenciaFundacionSegunVector(_view3D_paraBuscar, PtoBusqueda_zmasbajo, new XYZ(0, 0, -1)))
                {
                    if (newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.tipoEstriboGenera == TipoEstriboGenera.EMuro)
                    {
                        if (_p1.Z > _p2.Z)
                            _p2 = _p2.AsignarZ(BuscarMuros._PtoSObreCaraInferiorFund.Z + ConstNH.RECUBRIMIENTO_FUNDACIONES_foot + Util.MmToFoot(_datosConfinaDTO.DiamtroEstriboMM) / 2);
                        else
                            _p1 = _p1.AsignarZ(BuscarMuros._PtoSObreCaraInferiorFund.Z + ConstNH.RECUBRIMIENTO_FUNDACIONES_foot + Util.MmToFoot(_datosConfinaDTO.DiamtroEstriboMM) / 2);
                    }
                    else if (newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.tipoEstriboGenera == TipoEstriboGenera.EConfinamiento)
                    {
                        //para dibujar dimension que acota despalzamoneto dentro de fundaciones
                        newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.IsDImensionPorBajarFUndacion = true;
                        var AuxDireccion = (_datosConfinaDTO.direccionSeleccionMOuse == DireccionSeleccionMouse.DereToIzq ? _direccionBarra : -_direccionBarra);

                        if (_datosConfinaDTO.direccionSeleccionMOuse == DireccionSeleccionMouse.DereToIzq)
                        {
                            newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.ptoInicialDimension = _p1.ObtenerCopia().AsignarZ(BuscarMuros._PtoSObreCaraSuperiorFund.Z) + AuxDireccion * 1.3;
                            newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.ptoFinalDimension = _p1.ObtenerCopia().AsignarZ(BuscarMuros._PtoSObreCaraSuperiorFund.Z) - XYZ.BasisZ * (ConstNH.RECUBRIMIENTO_FUNDACIONES_CONFINAMINETO_ABAJO_foot) + AuxDireccion * 1.3;
                        }
                        else
                        {
                            newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.ptoInicialDimension = _p2.ObtenerCopia().AsignarZ(BuscarMuros._PtoSObreCaraSuperiorFund.Z) + AuxDireccion * 1;
                            newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.ptoFinalDimension = _p2.ObtenerCopia().AsignarZ(BuscarMuros._PtoSObreCaraSuperiorFund.Z) - XYZ.BasisZ * (ConstNH.RECUBRIMIENTO_FUNDACIONES_CONFINAMINETO_ABAJO_foot) + AuxDireccion * 1;
                        }

                        // bajar coodenad p1 5cm sobre cara inferiro fundaciones *****************
                        _p1 = _p1.AsignarZ(BuscarMuros._PtoSObreCaraSuperiorFund.Z - ConstNH.RECUBRIMIENTO_FUNDACIONES_CONFINAMINETO_ABAJO_foot - Util.MmToFoot(_datosConfinaDTO.DiamtroEstriboMM) / 2);

                        //if (_p1.Z > _p2.Z)
                        //    _p2 = _p2.AsignarZ(BuscarMuros._PtoSObreCaraSuperiorFund.Z - ConstNH.RECUBRIMIENTO_FUNDACIONES_CONFINAMINETO_ABAJO_foot - Util.MmToFoot(_datosConfinaDTO.DiamtroEstriboMM) / 2);
                        //else

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write("ex:" + ex.Message);
            }
        }

        private List<BarraLateralesDTO> M2_2_ObtenerLateraleMuroDTO()
        {
            try
            {
                ListaLaterales = new List<BarraLateralesDTO>();
                if (_datosConfinaDTO.IsLateral == true)
                {
                    ObtenerIntervalosLateralesMuro_Service _obtenerIntervalosLaterales = new ObtenerIntervalosLateralesMuro_Service(_datosConfinaDTO, newIntervalosConfinaminetoDTOAuto.ListaPtos[0], newIntervalosConfinaminetoDTOAuto.ListaPtos[1]);
                    ListaLaterales = _obtenerIntervalosLaterales.M3_ObtenerLateralesEstriboMuroDTO();
                }
            }
            catch (Exception)
            {

                Util.ErrorMsg("Error al obtener lista Laterales");
            }
            return ListaLaterales;
        }
        private void M2_3_ObtenerTrabaMuroDTO()
        {
            ListaTraba = new List<BarraTrabaDTO>();



            if (_datosConfinaDTO.IsTraba == true)
            {
                var confTraba = M2_3_1_ObtenerDatosParaTraba();

                ObtenerIntervalosTrabaMuro_Service _obtenerIntervalosLaterales = new ObtenerIntervalosTrabaMuro_Service(confTraba);

                ListaTraba = _obtenerIntervalosLaterales.M3_ObtenerTrabaEstriboMuroDTO();
            }
        }

        private ConfiguracionBarraTrabaDTO M2_3_1_ObtenerDatosParaTraba()
        {

            return new ConfiguracionBarraTrabaDTO()
            {
                DiamtroTrabaEstriboMM = _datosConfinaDTO.DiamtroTrabaEstriboMM,
                Ptobarra1 = newIntervalosConfinaminetoDTOAuto.ListaPtos[0] + new XYZ(0, 0, 1) * Util.MmToFoot(_datosConfinaDTO.DiamtroEstriboMM) + _direccionPerpenEntradoHaciaViewSecction * Util.MmToFoot(_datosConfinaDTO.DiamtroEstriboMM / 2),
                Ptobarra2 = newIntervalosConfinaminetoDTOAuto.ListaPtos[1] + _direccionPerpenEntradoHaciaViewSecction * Util.MmToFoot(_datosConfinaDTO.DiamtroEstriboMM / 2),
                EspesroMuroOVigaFoot = _datosConfinaDTO.espesor,
                tipoTraba_posicion = (_datosConfinaDTO.cantidadTraba_long > 0 ? TipoTraba_posicion.B : TipoTraba_posicion.A),
                textoTraba = (_datosConfinaDTO.IsTraba == false ? "" : _datosConfinaDTO.ObtenerTextBarra_Borrar()),
                DireccionEntradoHaciaView = _direccionPerpenEntradoHaciaViewSecction,
                DireccionEnfierrrado = (_datosConfinaDTO.direccionSeleccionMOuse == DireccionSeleccionMouse.DereToIzq ? -_direccionParalelaViewSecctioin : _direccionParalelaViewSecctioin),
                UbicacionTraba = (_datosConfinaDTO.direccionSeleccionMOuse == DireccionSeleccionMouse.DereToIzq ? DireccionTraba.Derecha : DireccionTraba.Izquierda),
                CantidadTrabasTranversal = _datosConfinaDTO.cantidadTraba,
                CantidadTrabasLongitudinal = _datosConfinaDTO.cantidadTraba_long,
                listaEspaciamientoTrabasTransversal = _datosConfinaDTO.listaEspaciamientoTrabas
            };

        }
        private string ObtenerTextoEstribo(string cantidadEstribo)
        {
            if (cantidadEstribo == "1")
            { return "E."; }
            else if (cantidadEstribo == "2")
            { return "E.D."; }
            else if (cantidadEstribo == "3")
            { return "E.T."; }
            else if (cantidadEstribo == "4")
            { return "E.C."; }

            return "0";
        }

        public XYZ M1_ObtenerPtoEnmuroAuto(XYZ ptoBuscarMuro)
        {
            try
            {

                if (!M1_1_CrearWorkPLane_EnCentroViewSecction())
                {
                    Util.ErrorMsg("No se pudo obtener pto sobre cara de muro:");
                    IsOk = false;
                    return XYZ.Zero;
                }

                if (!M1_3_SeleccionarMuroElementAuto(ptoBuscarMuro + _ViewNormalDirection6 * ConstNH.CONST_DISTANCIA_RETROCESO_BUSQUEDA_FOOT_PERPENDICULAR_VIEW, _direccionBarra))
                {
                    if (UtilBarras.IsConNotificaciones)
                        Util.ErrorMsg("No se pudo obtener pto sobre cara de muro:");
                    else
                        Debug.WriteLine("No se pudo obtener pto sobre cara de muro:");
                    IsOk = false;
                    return XYZ.Zero;
                }

                newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.espesor = _espesorMuroFoot;

            }
            catch (Exception ex)
            {

                Util.ErrorMsg("Error, No se pudo obtener pto sobre cara de muro ex:" + ex.Message);
                IsOk = false;
                return XYZ.Zero;
            }

            return _ptoSeleccionMouseCentroCaraMuro;
        }


        public virtual bool M1_4_BuscarPtoInicioBase(XYZ pto)
        {
            try
            {

                Plane plano = Plane.CreateByNormalAndOrigin(base._view.ViewDirection, _ptoSeleccionMouseCentroCaraMuro);
                ptoDiseñoMuro = plano.ProjectOnto(pto);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void M1_6_NombreFamilaTAG()
        {

            switch (newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.tipoConfiguracionEstribo)
            {
                case TipoConfiguracionEstribo.Estribo:
                    _nombreFamilia = "MRA Rebar_PILAR";
                    break;
                case TipoConfiguracionEstribo.Estribo_Lateral:
                    _nombreFamilia = "MRA Rebar_PILAR";
                    break;
                case TipoConfiguracionEstribo.Estribo_Traba:
                    _nombreFamilia = "MRA Rebar_PILAR";
                    break;
                case TipoConfiguracionEstribo.Estribo_Lateral_Traba:
                    _nombreFamilia = "MRA Rebar_PILAR";
                    break;
                case TipoConfiguracionEstribo.Traba:
                    _nombreFamilia = "MRA Rebar_PILAR";
                    break;
                //muro
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

        private XYZ ObtenerPtoTagConfinamientoyPilar(XYZ aux_ptobarra1)
        {
            XYZ posicionTag = XYZ.Zero;
            if (newIntervalosConfinaminetoDTOAuto._datosConfinaDTO.direccionSeleccionMOuse == DireccionSeleccionMouse.DereToIzq)
                posicionTag = aux_ptobarra1 + _direccionBarra * (_AnchoVisibleFoot + Util.CmToFoot(12)) + new XYZ(0, 0, Util.CmToFoot(100));
            else
                posicionTag = aux_ptobarra1 + -_direccionBarra * Util.CmToFoot(10) + new XYZ(0, 0, Util.CmToFoot(100));

            return posicionTag;
        }
    }
}
