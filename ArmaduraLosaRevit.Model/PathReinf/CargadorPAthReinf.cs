using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.PathReinf.DTO;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.PathReinf
{
    public class CargadorPAthReinf
    {
        public static ManejadorPathReinf _manejadorPathReinf { get; set; }

        public static bool CrearBarraFundaciones(UIApplication _uiapp, Element fund, List<XYZ> ListaPtosPerimetroBarras, DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales)
        {
            try
            {


                Document _doc = _uiapp.ActiveUIDocument.Document;
                List<Curve> curvesPathreiforment_ = new List<Curve>();
                curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[3], ListaPtosPerimetroBarras[2]));
                //ejcutar

                double nombreLargoPata_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm((int)_datosNuevaBarraDTOIniciales.DiametroMM);

                if (_datosNuevaBarraDTOIniciales.EspesorElemento_foot - 4 >Util.CmToFoot(nombreLargoPata_cm) &&
                    _datosNuevaBarraDTOIniciales.TipoPataFun != TipoPataFund.Sin)
                {

                    Util.ErrorMsg($"No se puede crear barras con pata de largo:{nombreLargoPata_cm} " +
                        $"en fundacion id:{fund.Id} de espesor :{Math.Round(Util.FootToCm(_datosNuevaBarraDTOIniciales.EspesorElemento_foot), 0)}.");
                    return false;
                }

                //1)
                string tipoBArra = "f11";
                switch (_datosNuevaBarraDTOIniciales.TipoPataFun)
                {
                    case TipoPataFund.IzqInf:
                        tipoBArra = "f11a";
                        break;
                    case TipoPataFund.DereSup:
                        tipoBArra = "f11b";
                        break;
                    case TipoPataFund.Ambos:
                        tipoBArra = "f11";
                        break;
                    case TipoPataFund.Sin:
                        tipoBArra = "f3";
                        break;
                    default:
                        tipoBArra = "f11";
                        break;
                }

                SolicitudBarraDTO _solicitudBarraDTO = new SolicitudBarraDTO(_uiapp, tipoBArra, UbicacionLosa.Izquierda, TipoConfiguracionBarra.refuerzoInferior, false);

                //3)
                PathReinfSeleccionDTO _pathReinfSeleccionDTO = new PathReinfSeleccionDTO();
                _pathReinfSeleccionDTO.Angle_pelotaLosa1Grado = 0;
                _pathReinfSeleccionDTO.ElementoSeleccionada1 = fund;
                _pathReinfSeleccionDTO.ListaPtosPerimetroBarras = ListaPtosPerimetroBarras;
                _pathReinfSeleccionDTO.EspesorCm_1 = 15;
                _pathReinfSeleccionDTO.ptoConMouse = _datosNuevaBarraDTOIniciales.PtoMouse;

                _pathReinfSeleccionDTO.PtoCodoDireztriz = _datosNuevaBarraDTOIniciales.PtoCodoDireztriz;
                _pathReinfSeleccionDTO.PtoDireccionDireztriz = _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz;
                _pathReinfSeleccionDTO.PtoTag = _datosNuevaBarraDTOIniciales.PtoTag;
                _pathReinfSeleccionDTO.IsLadoLibre = true;
                _pathReinfSeleccionDTO.PtoLadoLibre = _datosNuevaBarraDTOIniciales.LeaderEnd;



                double largoBarra = ListaPtosPerimetroBarras[2].DistanceTo(ListaPtosPerimetroBarras[1]);



                //4)objetos con los datos para dibujar barra
                DatosNuevaBarraDTO _datosNuevaBarraDTO = new DatosNuevaBarraDTO()
                {
                    LargoPathreiforment = largoBarra,
                    LargoMininoLosa = largoBarra,
                    CentroPAth = new XYZ(ListaPtosPerimetroBarras.Average(c => c.X), ListaPtosPerimetroBarras.Average(c => c.Y), ListaPtosPerimetroBarras.Average(c => c.Z)),
                    LargoRecorridoFoot = ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]),
                    IsLuzSecuandiria = false,
                    EspaciamientoFoot = _datosNuevaBarraDTOIniciales.EspaciamientoFoot,
                    DiametroMM = _datosNuevaBarraDTOIniciales.DiametroMM,
                    CurvesPathreiforment = curvesPathreiforment_,
                    tipodeHookStartPrincipal = (_datosNuevaBarraDTOIniciales.rebarBarTypePrincipal_star != null ? _datosNuevaBarraDTOIniciales.rebarBarTypePrincipal_star.Id : TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{nombreLargoPata_cm }", _doc).Id),
                    tipodeHookEndPrincipal = (_datosNuevaBarraDTOIniciales.rebarBarTypePrincipal_end != null ? _datosNuevaBarraDTOIniciales.rebarBarTypePrincipal_end.Id : TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{nombreLargoPata_cm }", _doc).Id),
                    LargoPAtaIzqHook_cm = _datosNuevaBarraDTOIniciales.LargoPAtaIzqHook_cm,
                    LargoPAtaDereHook_cm = _datosNuevaBarraDTOIniciales.LargoPAtaDereHook_cm,
                    TipoCaraObjeto_ = _datosNuevaBarraDTOIniciales.TipoCaraObjeto_,
                    TipoPataFun = _datosNuevaBarraDTOIniciales.TipoPataFun,
                    LargoTotal = _datosNuevaBarraDTOIniciales.LargoTotal_Fund(largoBarra),
                    LargoParciales = _datosNuevaBarraDTOIniciales.LargoParciales_Fund(largoBarra),
                    _BarraTipo = (_datosNuevaBarraDTOIniciales.TipoCaraObjeto_ == TipoCaraObjeto.Inferior ? TipoRebar.FUND_BA_INF : TipoRebar.FUND_BA_SUP),
                    IsAcortarCUrva = _datosNuevaBarraDTOIniciales.IsAcortarCUrva
                };



                // rebarHookTypeId = TiposRebar_Shape_Hook.getRebarHookType("Standard - 90 deg.", _doc).Id;
                _manejadorPathReinf = new ManejadorPathReinf(_uiapp);
                _manejadorPathReinf.EjecutarParaFundaciones(_solicitudBarraDTO, _pathReinfSeleccionDTO, _datosNuevaBarraDTO);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }

        public static bool CrearSupleMuro(UIApplication _uiapp, Element _Elemento, List<XYZ> ListaPtosPerimetroBarras, DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales, bool IsTipoREfuerzo)
        {

            try
            {


                Document _doc = _uiapp.ActiveUIDocument.Document;
                View _view = _doc.ActiveView;
                List<Curve> curvesPathreiforment_ = ObtenerCurvaPAth(ListaPtosPerimetroBarras, _view, _datosNuevaBarraDTOIniciales);
                if (curvesPathreiforment_.Count != 1)
                {
                    Util.ErrorMsg($"Error al curva de pathRein");
                    return false;
                }

                //ejcutar
                //1)
                SolicitudBarraDTO _solicitudBarraDTO = new SolicitudBarraDTO(_uiapp, "f3_refuezoSuple",
                    (_datosNuevaBarraDTOIniciales.orientacionBarraSupleMuro == TipoOrientacionBarraSupleMuro.Horizontal ? UbicacionLosa.Izquierda : UbicacionLosa.Superior),
                    TipoConfiguracionBarra.refuerzoInferior, false);

                //3)
                PathReinfSeleccionDTO _pathReinfSeleccionDTO = new PathReinfSeleccionDTO();
                _pathReinfSeleccionDTO.Angle_pelotaLosa1Grado = 0;
                _pathReinfSeleccionDTO.ElementoSeleccionada1 = _Elemento;
                _pathReinfSeleccionDTO.ListaPtosPerimetroBarras = ListaPtosPerimetroBarras;
                _pathReinfSeleccionDTO.EspesorCm_1 = Util.FootToCm(_datosNuevaBarraDTOIniciales.EspesorElemento_foot);
                _pathReinfSeleccionDTO.ptoConMouse = _datosNuevaBarraDTOIniciales.PtoMouse;
                _pathReinfSeleccionDTO.PtoCodoDireztriz = _datosNuevaBarraDTOIniciales.PtoCodoDireztriz;
                _pathReinfSeleccionDTO.PtoDireccionDireztriz = _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz;

                double largoBarra = (_datosNuevaBarraDTOIniciales.orientacionBarraSupleMuro == TipoOrientacionBarraSupleMuro.Vertical
                                    ? ListaPtosPerimetroBarras[2].DistanceTo(ListaPtosPerimetroBarras[1])
                                    : ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]));
                //4)objetos con los datos para dibujar barra
                DatosNuevaBarraDTO _datosNuevaBarraDTO = new DatosNuevaBarraDTO()
                {
                    LargoPathreiforment = largoBarra,
                    LargoMininoLosa = largoBarra,
                    LargoRecorridoFoot = ListaPtosPerimetroBarras[0].DistanceTo(ListaPtosPerimetroBarras[1]),
                    IsLuzSecuandiria = false,
                    EspaciamientoFoot = _datosNuevaBarraDTOIniciales.EspaciamientoFoot,
                    DiametroMM = _datosNuevaBarraDTOIniciales.DiametroMM,
                    CurvesPathreiforment = curvesPathreiforment_,
                    tipodeHookStartPrincipal = ElementId.InvalidElementId,// TipoRebarHookType.ObtenerHook("Hook_PataFundacion", _doc).Id,
                    tipodeHookEndPrincipal = ElementId.InvalidElementId, // TipoRebarHookType.ObtenerHook("Hook_PataFundacion", _doc).Id,
                    TipoCaraObjeto_ = _datosNuevaBarraDTOIniciales.TipoCaraObjeto_,
                    TipoPataFun = _datosNuevaBarraDTOIniciales.TipoPataFun,
                    LargoTotal = _datosNuevaBarraDTOIniciales.LargoTotal_(largoBarra),
                    LargoParciales = _datosNuevaBarraDTOIniciales.LargoParciales_(largoBarra),
                    _BarraTipo = _datosNuevaBarraDTOIniciales._BarraTipo

                };

                // rebarHookTypeId = TiposRebar_Shape_Hook.getRebarHookType("Standard - 90 deg.", _doc).Id;
                ManejadorPathReinf _manejadorPathReinf = new ManejadorPathReinf(_uiapp);

                DatosDiseño.IS_PATHREIN_AJUSTADO = false;
                DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO = false;
                if (_manejadorPathReinf.Ejecutar(_solicitudBarraDTO, _pathReinfSeleccionDTO, _datosNuevaBarraDTO))
                {
                    //crear texto
                    CrearTextoSuple(_uiapp, _datosNuevaBarraDTOIniciales, IsTipoREfuerzo, _view);
                }

                DatosDiseño.IS_PATHREIN_AJUSTADO = VariablesSistemas.IsAjusteBarra_Recorrido;
                DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO = VariablesSistemas.IsAjusteBarra_Largo;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }

        private static List<Curve> ObtenerCurvaPAth(List<XYZ> ListaPtosPerimetroBarras, View _view, DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales)
        {
            List<Curve> curvesPathreiforment_ = new List<Curve>();
            try
            {


                if (_datosNuevaBarraDTOIniciales.orientacionBarraSupleMuro == TipoOrientacionBarraSupleMuro.Vertical)
                {
                    XYZ direcionPAth = (ListaPtosPerimetroBarras[2] - ListaPtosPerimetroBarras[3]).Normalize();

                    XYZ verific = Util.CrossProduct(direcionPAth, _view.ViewDirection);

                    if (_datosNuevaBarraDTOIniciales.tipo_sentido == SentidoSupleMuro.Normal)
                    {
                        if (verific.Z < 0)
                            curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[3], ListaPtosPerimetroBarras[2]));
                        else
                            curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[2], ListaPtosPerimetroBarras[3]));
                    }
                    else
                    {
                        if (verific.Z < 0)
                            curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[2], ListaPtosPerimetroBarras[3]));
                        else
                            curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[3], ListaPtosPerimetroBarras[2]));
                    }


                }
                else
                {
                    //direccionpath
                    XYZ direcionPAth = (ListaPtosPerimetroBarras[1] - ListaPtosPerimetroBarras[2]).Normalize();
                    XYZ direccionDebarra = (ListaPtosPerimetroBarras[0] - ListaPtosPerimetroBarras[1]).Normalize();
                    //producto cruz path y hacia derehca
                    XYZ verific = Util.CrossProduct(direcionPAth, _view.ViewDirection);

                    double resulProduPunto = Util.GetProductoEscalar(verific, direccionDebarra);
                    if (_datosNuevaBarraDTOIniciales.tipo_sentido == SentidoSupleMuro.Normal)
                    {
                        if (resulProduPunto > 0)
                            curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[1], ListaPtosPerimetroBarras[2]));
                        else
                            curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[2], ListaPtosPerimetroBarras[1]));
                    }
                    else
                    {
                        if (resulProduPunto > 0)
                            curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[2], ListaPtosPerimetroBarras[1]));
                        else
                            curvesPathreiforment_.Add(Line.CreateBound(ListaPtosPerimetroBarras[1], ListaPtosPerimetroBarras[2]));
                    }

                }
            }
            catch (Exception)
            {
                curvesPathreiforment_.Clear();
            }


            return curvesPathreiforment_;
        }

        private static bool CrearTextoSuple(UIApplication _uiapp, DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales, bool IsTipoREfuerzo, View _view)
        {
            try
            {

                string textoBARRAS = (IsTipoREfuerzo ? "(SUPLE  EXTERIOR)" : "(SUPLE  INTERIOR)");

                CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.AcotarBarra, TipoCOloresTexto.rojo);
                XYZ p3_texto = _datosNuevaBarraDTOIniciales.PtoMouse + _view.RightDirection * Util.CmToFoot(10) + new XYZ(0, 0, -Util.CmToFoot(0));
                if (_view.Scale == 75)
                    p3_texto = _datosNuevaBarraDTOIniciales.PtoMouse + _view.RightDirection * Util.CmToFoot(10) + new XYZ(0, 0, -Util.CmToFoot(50));
                else if (_view.Scale == 100)
                    p3_texto = _datosNuevaBarraDTOIniciales.PtoMouse + _view.RightDirection * Util.CmToFoot(10) + new XYZ(0, 0, -Util.CmToFoot(75));

                _CrearTexNote.M1_CrearConTrans(p3_texto, textoBARRAS, (_datosNuevaBarraDTOIniciales.orientacionBarraSupleMuro == TipoOrientacionBarraSupleMuro.Vertical ? Math.PI / 2 : 0));
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }
    }
}
