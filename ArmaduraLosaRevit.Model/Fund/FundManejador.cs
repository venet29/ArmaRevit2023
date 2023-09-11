using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Fund.Entidad;
using ArmaduraLosaRevit.Model.PathReinf;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Fund.Servicios;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Fund.Intervalos;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Events;
using System;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Fund.WPFfund.DTO;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.Fund
{


    public partial class FundManejador
    {
        private UIApplication _uiapp;
        private bool iscaso_Intervalo;
        private Document _doc;
        private FundGeoDTO _datosPLanarface_fundacionSeleccionada;
        ISelectionFilter filtro;
        private DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales;

        public FundManejador(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            iscaso_Intervalo = false;
            _doc = _uiapp.ActiveUIDocument.Document;
        }

        public Result execute(DatosNuevaBarraDTO _datosNuevaBarraDTOInicialesInicial)
        {
            _datosNuevaBarraDTOIniciales = _datosNuevaBarraDTOInicialesInicial;
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            CreadorCirculo.ListaCirculos.Clear();
           
            Result resul = Result.Succeeded;
            try
            {

                if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return Result.Failed;


                iscaso_Intervalo = _datosNuevaBarraDTOIniciales.Iscaso_Intervalo;
                bool mientras = true;
                while (mientras)
                {
                    mientras = false;
                    //1)seleccionana

                    SeleccionarFundConMouse seleccionarFundConMouse = new SeleccionarFundConMouse(_uiapp);

                    ObtenetTipoFiltro(_datosNuevaBarraDTOIniciales);

                    // (_datosNuevaBarraDTOIniciales.TiposElemento=="" ? new FiltroFundaciones() : new FiltroFloor());
                    if (!seleccionarFundConMouse.M1_Selecconafund(filtro)) return Result.Cancelled;

                    CreadorCirculo.UltimoCirculoCreado = CreadorCirculo.CrearCirculo_DetailLine_ConTrans(Util.CmToFoot(4), seleccionarFundConMouse.PtoMOuse_sobreFundacion, _uiapp.ActiveUIDocument, XYZ.BasisX, XYZ.BasisY ,"BarraCirculo");

                    _datosPLanarface_fundacionSeleccionada = seleccionarFundConMouse.M1_SeleccionarCaraInferiorFund();


                    if (!_datosPLanarface_fundacionSeleccionada.IsOK) return SalirYBorrarCirculo(_doc); //ShaftIndividualNULL

                    FundConjunto _fundConjunto = new FundConjunto(_uiapp, _datosPLanarface_fundacionSeleccionada);
                    _fundConjunto.Ejecutar();

                    //2)obtiene objeto que representa shaft --geometria
                    FundIndividual FundIndividuas = _fundConjunto.FundacionUnicoSeleccoinado;

                    //2) selecion direccion barra  ,obtener pto inici y fin de barra con intersscion
                    PuntosSeleccionMouse _puntosSeleccionMouse = new PuntosSeleccionMouse(_uiapp);

                    if(_datosNuevaBarraDTOInicialesInicial.TipoDeDIreccionBarra=="borde")
                        if (!_puntosSeleccionMouse.MO_SeleccionarDIrecionCOnBorde()) return SalirYBorrarCirculo(_doc);

                    if (!_puntosSeleccionMouse.M1_SelecionarPtosDireccionBarra()) return SalirYBorrarCirculo(_doc);

                    if (!_puntosSeleccionMouse.M2_SelecionarPtosDireccionRecorrido()) return SalirYBorrarCirculo(_doc);

                    _datosNuevaBarraDTOIniciales.PtoMouse = seleccionarFundConMouse.PtoMOuse_sobreFundacion;

                    Obtener4ptosFund Obtener4ptosFund = new Obtener4ptosFund(_uiapp, _fundConjunto.ListaCaraAnalizada, _puntosSeleccionMouse);
                    Obtener4ptosFund.EjecutarObtener4ptos(_datosNuevaBarraDTOIniciales.TipoPataFun.ToString());

                    List<XYZ> ListaPtosPerimetroBarras = Obtener4ptosFund.ObtenerListaPtosPerimetroBarras();

                    if (!iscaso_Intervalo)
                    {
                        if (!_puntosSeleccionMouse.M3_SelecionarPtosDirectriz()) return SalirYBorrarCirculo(_doc);
                        _datosNuevaBarraDTOIniciales.PtoCodoDireztriz = _puntosSeleccionMouse.p1_origenDiretriz;
                        _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz = _puntosSeleccionMouse.p2_SentidoDiretriz;
                    }
                    else
                    {
                        XYZ dire4_3 = (ListaPtosPerimetroBarras[3] - ListaPtosPerimetroBarras[2]).GetXY0().Normalize();
                        XYZ dire2_1 = (ListaPtosPerimetroBarras[2] - ListaPtosPerimetroBarras[1]).GetXY0().Normalize();
                        _datosNuevaBarraDTOIniciales.PtoCodoDireztriz = _datosNuevaBarraDTOIniciales.PtoMouse + dire2_1 * 2 + dire4_3 * 4;
                        _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz = _datosNuevaBarraDTOIniciales.PtoCodoDireztriz + dire2_1 * 2;
                    }
                    //4)Obtener recorrido

                    List<DatosIntervalosFundDTO> LIStaDe_ListaPtosPerimetroBarras = new List<DatosIntervalosFundDTO>();
                

                    if (iscaso_Intervalo)
                    {
                        _datosNuevaBarraDTOIniciales.LargoPAtaIzqHook_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm((int)_datosNuevaBarraDTOIniciales.DiametroMM);
                        _datosNuevaBarraDTOIniciales.LargoPAtaDereHook_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm((int)_datosNuevaBarraDTOIniciales.DiametroMM);

                        ParametrosListaIntervalosDTo _ParametrosListaIntervalosDTo = new ParametrosListaIntervalosDTo()
                        {
                            uiapp = _uiapp,

                            _ubicacionPtoMouse = UbicacionPtoMouse.centro,
                            _tipoPosicionMouse = TipoPosicionMouse.segunMouse,
                            _Iscaso_intervalo = iscaso_Intervalo,
                            _diametroMM = (int)_datosNuevaBarraDTOIniciales.DiametroMM,
                            _espaciamiento = _datosNuevaBarraDTOIniciales.EspaciamientoFoot,
                            PtoSeleccionMouse1 = _datosNuevaBarraDTOIniciales.PtoMouse,
                            ListaPtosPerimetroBarras = ListaPtosPerimetroBarras,
                            TipoBarra = Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_datosNuevaBarraDTOIniciales._BarraTipo),
                            ubicacionEnlosa = UbicacionLosa.Inferior //valor colocado pro defecto                           
                        };

                        //NOTA FECHA:27-09-2021  CDV INDICO QUE LAS BARRAS MIENTRAS ES LA BARRA ESTE DENTRO DE LA LOSA TODO BIEN
                        //SOLO PARA PENDIENTES MAS ALTAS ( 45°) SE DEBE HACER  TRASLAPO EN EL CAMBIO DE PENDIENTE
                        GenerarListaIntervalosFund _GenerarListaIntervalos = new GenerarListaIntervalosFund(_ParametrosListaIntervalosDTo,
                                                                                                            Obtener4ptosFund.TipoPataFun);
                        if (!_GenerarListaIntervalos.M1_ObtenerIntervalosFund()) return Result.Cancelled;

                        LIStaDe_ListaPtosPerimetroBarras.Clear();
                        //var dd = _GenerarListaIntervalos.ListaIntervalosDTO.Select(c => c.ListaIntervalos).ToList();
                        LIStaDe_ListaPtosPerimetroBarras.AddRange(_GenerarListaIntervalos.ListaDatosIntervalosDTO);


                    }
                    else
                    {

                        (RebarHookType hookIZ, RebarHookType hookDere, double LArgoPataHOokIzq_cm, double LArgoPataHOokDere_cm, bool IsOK) = 
                            AyudaOBtenerHookYLargo.ObtenerHookFundaciones(_doc,(int)_datosNuevaBarraDTOIniciales.DiametroMM);
                        if (!IsOK) return Result.Cancelled;
                        _datosNuevaBarraDTOIniciales.rebarBarTypePrincipal_star = hookIZ;
                        _datosNuevaBarraDTOIniciales.rebarBarTypePrincipal_end = hookDere;
                        _datosNuevaBarraDTOIniciales.LargoPAtaIzqHook_cm = LArgoPataHOokIzq_cm;
                        _datosNuevaBarraDTOIniciales.LargoPAtaDereHook_cm = LArgoPataHOokDere_cm;



                        REcalcularPuntosLeaderPathRein _REcalcularPuntosLeader = new REcalcularPuntosLeaderPathRein(ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales, _doc.ActiveView.Scale);
                        if (_REcalcularPuntosLeader.Calcular())
                        {
                            _datosNuevaBarraDTOIniciales.PtoCodoDireztriz = _REcalcularPuntosLeader.PtoCodo;
                            _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz = _REcalcularPuntosLeader.PtoDireccion;
                            _datosNuevaBarraDTOIniciales.PtoTag = _REcalcularPuntosLeader.PtoTag;
                            _datosNuevaBarraDTOIniciales.LeaderEnd = _REcalcularPuntosLeader.PtoFree;
                        }

                        DatosIntervalosFundDTO _datosIntervalosFundDTO = new DatosIntervalosFundDTO()
                        {
                            LeaderEnd = _datosNuevaBarraDTOIniciales.LeaderEnd,
                            _listaptos = ListaPtosPerimetroBarras,
                            PtoMouse = _datosNuevaBarraDTOIniciales.PtoMouse,
                            PtoCodoDireztriz = _datosNuevaBarraDTOIniciales.PtoCodoDireztriz,
                            PtoDireccionDireztriz = _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz,
                            PtoTag = _datosNuevaBarraDTOIniciales.PtoTag,
                            tipoPataFun = Obtener4ptosFund.TipoPataFun,
                            rebarHookTypePrincipal_end = _datosNuevaBarraDTOIniciales.rebarBarTypePrincipal_end,
                            rebarHookTypePrincipal_star = _datosNuevaBarraDTOIniciales.rebarBarTypePrincipal_star,
                            LargoPAtaIzqHook = _datosNuevaBarraDTOIniciales.LargoPAtaIzqHook_cm,
                            LargoPAtaDereHook = _datosNuevaBarraDTOIniciales.LargoPAtaDereHook_cm,

                        };

                        LIStaDe_ListaPtosPerimetroBarras.Add(_datosIntervalosFundDTO);
                    }

                    //double PataMasLArgas_cm = Math.Max(_datosNuevaBarraDTOIniciales.LargoPAtaIzqHook_cm, _datosNuevaBarraDTOIniciales.LargoPAtaDereHook_cm);
                    //if (PataMasLArgas_cm  > Util.FootToCm(_datosPLanarface_fundacionSeleccionada.Espesor_foot) - 4 && 
                    //    LIStaDe_ListaPtosPerimetroBarras[0].tipoPataFun!= TipoPataFund.Sin)
                    //{
                    //    Util.ErrorMsg($"No se puede crear barras con pata de largo : {PataMasLArgas_cm}cm en fundacion id:{_datosPLanarface_fundacionSeleccionada.fundacion.Id}" +
                    //        $" de espesor :{Math.Round( Util.FootToCm(_datosPLanarface_fundacionSeleccionada.Espesor_foot),0)} cm.");
                    //    CreadorCirculo.BorrasCirculosCreado_COntrans(_doc);
                    //    return Result.Cancelled;
                    //}

                    //4)dibujar
                    //tengo que tener 4 ptos del poligono
                    foreach (DatosIntervalosFundDTO _datosIntervalosDTO in LIStaDe_ListaPtosPerimetroBarras)
                    {
                        Element fund = _datosPLanarface_fundacionSeleccionada.fundacion;
                        _datosNuevaBarraDTOIniciales.TipoPataFun = _datosIntervalosDTO.tipoPataFun;
                        _datosNuevaBarraDTOIniciales.PtoMouse = _datosIntervalosDTO.PtoMouse;

                        _datosNuevaBarraDTOIniciales.PtoCodoDireztriz = _datosIntervalosDTO.PtoCodoDireztriz;
                        _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz = _datosIntervalosDTO.PtoDireccionDireztriz;
                        _datosNuevaBarraDTOIniciales.PtoTag = _datosIntervalosDTO.PtoTag;
                        _datosNuevaBarraDTOIniciales.LeaderEnd = _datosIntervalosDTO.LeaderEnd;

                        _datosNuevaBarraDTOIniciales.IsLuzSecuandiria = true;
                        _datosNuevaBarraDTOIniciales.IsLibre = true;
                        _datosNuevaBarraDTOIniciales.EspesorElemento_foot = _datosPLanarface_fundacionSeleccionada.Espesor_foot;
                        _datosNuevaBarraDTOIniciales.tipodeHookEndPrincipal = _datosIntervalosDTO.rebarHookTypePrincipal_end?.Id;
                        _datosNuevaBarraDTOIniciales.tipodeHookStartPrincipal = _datosIntervalosDTO.rebarHookTypePrincipal_star?.Id;
                        _datosNuevaBarraDTOIniciales.LargoPAtaIzqHook_cm = _datosIntervalosDTO.LargoPAtaIzqHook;
                        _datosNuevaBarraDTOIniciales.LargoPAtaDereHook_cm = _datosIntervalosDTO.LargoPAtaDereHook;


                        CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, _datosIntervalosDTO._listaptos, _datosNuevaBarraDTOIniciales);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Util.DebugDescripcion(ex);
                resul = Result.Failed;
            }

            CreadorCirculo.BorrasCirculosCreado_COntrans(_doc);

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return resul;
        }

       
        private static Result SalirYBorrarCirculo(Document _doc)
        {
            CreadorCirculo.BorrasCirculosCreado_COntrans(_doc);
            return Result.Failed;
        }

        private void ObtenetTipoFiltro(DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales)
        {
            if (_datosNuevaBarraDTOIniciales.TiposElemento == "Fundacion")
                filtro = new FiltroFundaciones();
            else
                filtro = new FiltroFloor();
        }


    }
}