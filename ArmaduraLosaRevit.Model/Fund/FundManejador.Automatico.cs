using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Fund.Entidad;
using ArmaduraLosaRevit.Model.PathReinf;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Fund.Servicios;
using ArmaduraLosaRevit.Model.RebarLosa;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Fund.WPFfund.DTO;

namespace ArmaduraLosaRevit.Model.Fund
{


    public partial class FundManejador
    {
        DatosNuevaBarraDTO DatosNuevaBarraDTO_auto;
        private List<XYZ> ListaPtosPerimetroBarras;
        private XYZ p1;
        private XYZ p2;
        private XYZ p3;
        private XYZ p4;
        private Element fund;
        private XYZ PtoMouse;
        private DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales_original;
        private SeleccionarFundConMouse seleccionarFundConMouse;
        private FundIndividual FundIndividuas;

        public Result executeAutomatico_fundacionCuadrada(DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales, CasosFundDTO _casosFundDTO)
        {
            try
            {
                _datosNuevaBarraDTOIniciales_original = _datosNuevaBarraDTOIniciales;
                // DatosNuevaBarraDTO_auto = _datosNuevaBarraDTOIniciales;
                //1)seleccionana
                seleccionarFundConMouse = new SeleccionarFundConMouse(_uiapp);

                ObtenetTipoFiltro(_datosNuevaBarraDTOIniciales);


                if (!seleccionarFundConMouse.M1_Selecconafund(filtro)) return Result.Cancelled;

                if (_casosFundDTO.InferiorVertical || _casosFundDTO.InferiorHorizontal)
                    _datosPLanarface_fundacionSeleccionada = seleccionarFundConMouse.M1_SeleccionarCaraInferiorFund();
                else
                    _datosPLanarface_fundacionSeleccionada = seleccionarFundConMouse.M2_SeleccionarCaraSuperiorFund();

                if (!_datosPLanarface_fundacionSeleccionada.IsOK) return Result.Succeeded;   //ShaftIndividualNULL

                FundConjunto _fundConjunto = new FundConjunto(_uiapp, _datosPLanarface_fundacionSeleccionada);
                if (!_fundConjunto.Ejecutar()) return Result.Cancelled;

                //2)obtiene objeto que representa shaft --geometria
                FundIndividuas = _fundConjunto.FundacionUnicoSeleccoinado;

                if (!FundIndividuas.Ordear4pto()) return Result.Cancelled;
                if (FundIndividuas == null || FundIndividuas.IsOk == false)
                {
                    return Result.Failed;
                }
                if (FundIndividuas.ListaVertices.Count != 4)
                {
                    Util.ErrorMsg("Para el enfierra automatico de dundaciones es necesario que sean rectangular (4 vertices)");
                    return Result.Failed;
                }

                if (!M1_AsignarPArametros(FundIndividuas)) return Result.Cancelled;

                // horizontal *** definir tipo de barras
                DatosNuevaBarraDTO_auto = new DatosNuevaBarraDTO();
                DatosNuevaBarraDTO_auto.TipoPataFun = TipoPataFund.Ambos;
                DatosNuevaBarraDTO_auto.PtoMouse = _datosPLanarface_fundacionSeleccionada.ptoSeleccionFund;
                if (_casosFundDTO.CasoTipoBArra == "Path")
                {

                    if (_casosFundDTO.InferiorHorizontal)
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;

                    if (_casosFundDTO.SuperiorHorizontal)
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                }
                else if (_casosFundDTO.CasoTipoBArra == "Rebar")
                {
                    if (_casosFundDTO.InferiorHorizontal)
                    {
                        DatosNuevaBarraDTO_auto.TipoBarra = "f11_fund";// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
                    }
                    if (_casosFundDTO.SuperiorHorizontal)
                    {
                        DatosNuevaBarraDTO_auto.TipoBarra = "f10_fund";// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                    }
                }

                ListaPtosPerimetroBarras.Clear();
                ListaPtosPerimetroBarras.Add(p1);
                ListaPtosPerimetroBarras.Add(p2);
                ListaPtosPerimetroBarras.Add(p3);
                ListaPtosPerimetroBarras.Add(p4);

                M2_1_ObtenerPtoMouseConPerimetroHH_inf(_casosFundDTO.SuperiorHorizontal);

                REcalcularPuntosLeaderRebar _REcalcularPuntosLeader = new REcalcularPuntosLeaderRebar(ListaPtosPerimetroBarras, DatosNuevaBarraDTO_auto, _doc.ActiveView.Scale);
                if (_REcalcularPuntosLeader.Calcular())
                {
                    DatosNuevaBarraDTO_auto.PtoCodoDireztriz = _REcalcularPuntosLeader.PtoCodo;
                    DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = _REcalcularPuntosLeader.PtoDireccion;
                    DatosNuevaBarraDTO_auto.PtoTag = _REcalcularPuntosLeader.PtoTag;
                    DatosNuevaBarraDTO_auto.LeaderEnd = _REcalcularPuntosLeader.PtoFree;
                }
                M2_CrearBarraHorizontal(_casosFundDTO, _casosFundDTO.CasoTipoBArra);


                ///* vertical
                DatosNuevaBarraDTO_auto = new DatosNuevaBarraDTO();
                DatosNuevaBarraDTO_auto.TipoPataFun = TipoPataFund.Ambos;
                DatosNuevaBarraDTO_auto.PtoMouse = _datosPLanarface_fundacionSeleccionada.ptoSeleccionFund;
                if (_casosFundDTO.CasoTipoBArra == "Path")
                {

                    if (_casosFundDTO.InferiorVertical)
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;

                    if (_casosFundDTO.SuperiorVertical)
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                }
                else if (_casosFundDTO.CasoTipoBArra == "Rebar")
                {
                    if (_casosFundDTO.InferiorVertical)
                    {
                        DatosNuevaBarraDTO_auto.TipoBarra = "f11_fund";// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
                    }
                    if (_casosFundDTO.SuperiorVertical)
                    {
                        DatosNuevaBarraDTO_auto.TipoBarra = "f10_fund";// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                    }
                }

                ListaPtosPerimetroBarras.Clear();

                ListaPtosPerimetroBarras.Add(p2);
                ListaPtosPerimetroBarras.Add(p3);
                ListaPtosPerimetroBarras.Add(p4);
                ListaPtosPerimetroBarras.Add(p1);
                FundIndividuas.ListaVertices = ListaPtosPerimetroBarras;

                M3_1_ObtenerPtoMouseConPerimetroV_inf(_casosFundDTO.SuperiorVertical);

                REcalcularPuntosLeaderRebar _REcalcularPuntosLeaderVertical = new REcalcularPuntosLeaderRebar(ListaPtosPerimetroBarras, DatosNuevaBarraDTO_auto, _doc.ActiveView.Scale);
                if (_REcalcularPuntosLeaderVertical.Calcular())
                {
                    DatosNuevaBarraDTO_auto.PtoCodoDireztriz = _REcalcularPuntosLeaderVertical.PtoCodo;
                    DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = _REcalcularPuntosLeaderVertical.PtoDireccion;
                    DatosNuevaBarraDTO_auto.PtoTag = _REcalcularPuntosLeaderVertical.PtoTag + (_casosFundDTO.SuperiorVertical ? _REcalcularPuntosLeaderVertical.Despla_tag_barraVErticaSup : XYZ.Zero);
                    DatosNuevaBarraDTO_auto.LeaderEnd = _REcalcularPuntosLeaderVertical.PtoFree;
                }

                M3_CrearBArrasVertical(_casosFundDTO, _casosFundDTO.CasoTipoBArra);
                //*******************************************************************************************************************************************

            }
            catch (System.Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Cancelled;
            }
            return Result.Succeeded;
        }

        private bool M1_AsignarPArametros(FundIndividual FundIndividuas)
        {
            try
            {
                fund = _datosPLanarface_fundacionSeleccionada.fundacion;
                if (fund == null) return false;

                ListaPtosPerimetroBarras = new List<XYZ>();
                p1 = FundIndividuas.ListaVertices[0];
                p2 = FundIndividuas.ListaVertices[1];
                p3 = FundIndividuas.ListaVertices[2];
                p4 = FundIndividuas.ListaVertices[3];
            }
            catch (System.Exception)
            {

                return false;
            }
            return true;
        }

        private bool M2_CrearBarraHorizontal(CasosFundDTO _casosFundDTO, string casoPathORebar)
        {
            if ((!_casosFundDTO.InferiorHorizontal) && (!_casosFundDTO.SuperiorHorizontal)) return true;
            //DatosNuevaBarraDTO_auto = new DatosNuevaBarraDTO();
            DatosNuevaBarraDTO_auto.DiametroMM = _datosNuevaBarraDTOIniciales_original.DiametroMM_fundH;
            DatosNuevaBarraDTO_auto.EspaciamientoFoot = _datosNuevaBarraDTOIniciales_original.Espaciamiento_fundH_Foot;


            ListaPtosPerimetroBarras.Clear();
            ListaPtosPerimetroBarras.Add(p1);
            ListaPtosPerimetroBarras.Add(p2);
            ListaPtosPerimetroBarras.Add(p3);
            ListaPtosPerimetroBarras.Add(p4);

            
  
            //DatosNuevaBarraDTO_auto.TipoPataFun = TipoPataFund.Ambos;

            if (casoPathORebar == "Rebar")
            {
                DatosNuevaBarraDTO_auto.LargoPAtaIzqHook_cm = FactoresLargoLeader.LargoPaTaIzq_cm; 
                DatosNuevaBarraDTO_auto.LargoPAtaDereHook_cm = FactoresLargoLeader.LargoPaTaDere_cm;
            }
            else
            {

                M2_1_ObtenerPtoMouseConPerimetroHH_inf(_casosFundDTO.SuperiorHorizontal);

                (RebarHookType hookIZ, RebarHookType hookDere, double LArgoPataHOokIzq_cm, double LArgoPataHOokDere_cm, bool IsOK) =
                     AyudaOBtenerHookYLargo.ObtenerHookFundaciones(_doc, (int)DatosNuevaBarraDTO_auto.DiametroMM);
                if (!IsOK) return false;
                DatosNuevaBarraDTO_auto.rebarBarTypePrincipal_star = hookIZ;
                DatosNuevaBarraDTO_auto.rebarBarTypePrincipal_end = hookDere;

                DatosNuevaBarraDTO_auto.LargoPAtaIzqHook_cm = LArgoPataHOokIzq_cm;
                DatosNuevaBarraDTO_auto.LargoPAtaDereHook_cm = LArgoPataHOokDere_cm;


                REcalcularPuntosLeaderPathRein _REcalcularPuntosLeader = new REcalcularPuntosLeaderPathRein(ListaPtosPerimetroBarras, DatosNuevaBarraDTO_auto, _doc.ActiveView.Scale);
                if (_REcalcularPuntosLeader.Calcular())
                {
                    DatosNuevaBarraDTO_auto.PtoCodoDireztriz = _REcalcularPuntosLeader.PtoCodo;
                    DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = _REcalcularPuntosLeader.PtoDireccion;
                    DatosNuevaBarraDTO_auto.PtoTag = _REcalcularPuntosLeader.PtoTag;
                    DatosNuevaBarraDTO_auto.LeaderEnd = _REcalcularPuntosLeader.PtoFree;
                }

            }

            DatosNuevaBarraDTO_auto.IsLuzSecuandiria = true;
            DatosNuevaBarraDTO_auto.IsLibre = true;




            if (casoPathORebar == "Path")
            {

                if (_casosFundDTO.InferiorHorizontal)
                {
                    // vertical    _     
                    //p1    p4      |
                    //p2    p3     _|

                    //DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
                    //_datosNuevaBarraDTOIniciales.PtoMouse =  seleccionarFundConMouse.PtoMOuse.AsignarZ(_doc.ActiveView.GenLevel.Elevation);
                    CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, DatosNuevaBarraDTO_auto);
                }

                if (_casosFundDTO.SuperiorHorizontal)
                { // vertical      _     
                  //p1    p4      |
                  //p2    p3      |_
                  // DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                    CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, DatosNuevaBarraDTO_auto);
                }
            }
            else if (casoPathORebar == "Rebar")
            {
                //DatosNuevaBarraDTO_auto.DiametroMM = _datosNuevaBarraDTOIniciales_original.DiametroMM_fundH;
                //DatosNuevaBarraDTO_auto.EspaciamientoFoot = _datosNuevaBarraDTOIniciales_original.Espaciamiento_fundH_Foot;

                if (_casosFundDTO.InferiorHorizontal)
                {
                    // vertical    _     
                    //p1    p4      |
                    //p2    p3     _|


                    // DatosNuevaBarraDTO_auto.TipoBarra = "f11_fund";// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                    // DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;

                    ManejadorRebarFund barraManejador_Izq_dere = new ManejadorRebarFund(_uiapp, DatosNuevaBarraDTO_auto, seleccionarFundConMouse);
                    barraManejador_Izq_dere.DibujarBarra(FundIndividuas, UbicacionLosa.Izquierda, TipoCaraUbicacion.Inferior);
                }

                if (_casosFundDTO.SuperiorHorizontal)
                {
                    // vertical    _     
                    //p1    p4      |
                    //p2    p3     _|


                    // DatosNuevaBarraDTO_auto.TipoBarra = "f10_fund";// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                    // DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;

                    ManejadorRebarFund barraManejador_Izq_dere = new ManejadorRebarFund(_uiapp, DatosNuevaBarraDTO_auto, seleccionarFundConMouse);
                    barraManejador_Izq_dere.DibujarBarra(FundIndividuas, UbicacionLosa.Izquierda, TipoCaraUbicacion.Superior);
                }
            }

            return true;
        }
        private void M2_1_ObtenerPtoMouseConPerimetroHH_inf(bool IsSuperiorHorizontal)
        {
            var ElemeSele = (IsSuperiorHorizontal ? TipoCaraObjeto.Superior : TipoCaraObjeto.Inferior);

            XYZ p1 = ListaPtosPerimetroBarras[0];
            XYZ p2 = ListaPtosPerimetroBarras[1];
            XYZ p3 = ListaPtosPerimetroBarras[2];
            XYZ p4 = ListaPtosPerimetroBarras[3];

            PtoMouse = XYZ.Zero;
            XYZ Direc3_2 = (p3 - p2).Normalize();
            double largo3_2 = p3.DistanceTo(p2);
            XYZ Direc1_2 = (p1 - p2).Normalize();
            double largo_2 = p1.DistanceTo(p2);

            if (IsSuperiorHorizontal)
            {
                PtoMouse = p4 - Direc3_2 * Util.CmToFoot(25) - Direc1_2 * Util.CmToFoot(22);

                DatosNuevaBarraDTO_auto.PtoMouse = PtoMouse;
                DatosNuevaBarraDTO_auto.PtoCodoDireztriz = PtoMouse + Direc1_2 * 2 - Direc3_2 * 1;
                DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = DatosNuevaBarraDTO_auto.PtoCodoDireztriz + Direc3_2 * 2;
            }
            else
            {
                PtoMouse = p2 + Direc3_2 * Util.CmToFoot(25) + Direc1_2 * Util.CmToFoot(22);

                DatosNuevaBarraDTO_auto.PtoMouse = PtoMouse;
                DatosNuevaBarraDTO_auto.PtoCodoDireztriz = PtoMouse - Direc1_2 * 2 + Direc3_2 * 1;
                DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = DatosNuevaBarraDTO_auto.PtoCodoDireztriz + Direc3_2 * 2;
            }
            
        }

        private bool M3_CrearBArrasVertical(CasosFundDTO _casosFundDTO, string casoPathORebar)
        {
            if ((!_casosFundDTO.InferiorVertical) && (!_casosFundDTO.SuperiorVertical)) return true;

            DatosNuevaBarraDTO_auto.DiametroMM = _datosNuevaBarraDTOIniciales_original.DiametroMM_fundV;
            DatosNuevaBarraDTO_auto.EspaciamientoFoot = _datosNuevaBarraDTOIniciales_original.Espaciamiento_fundV_Foot;



    


            if (casoPathORebar == "Rebar")
            {
                DatosNuevaBarraDTO_auto.LargoPAtaIzqHook_cm = FactoresLargoLeader.LargoPaTaIzq_cm;
                DatosNuevaBarraDTO_auto.LargoPAtaDereHook_cm = FactoresLargoLeader.LargoPaTaDere_cm; ;
            }
            else
            {
                
                M3_1_ObtenerPtoMouseConPerimetroV_inf(_casosFundDTO.SuperiorVertical);

                (RebarHookType hookIZ, RebarHookType hookDere, double LArgoPataHOokIzq_cm, double LArgoPataHOokDere_cm, bool IsOK) =
                     AyudaOBtenerHookYLargo.ObtenerHookFundaciones(_doc, (int)DatosNuevaBarraDTO_auto.DiametroMM);
                if (!IsOK) return false;
                DatosNuevaBarraDTO_auto.rebarBarTypePrincipal_star = hookIZ;
                DatosNuevaBarraDTO_auto.rebarBarTypePrincipal_end = hookDere;
                DatosNuevaBarraDTO_auto.LargoPAtaIzqHook_cm = LArgoPataHOokIzq_cm;
                DatosNuevaBarraDTO_auto.LargoPAtaDereHook_cm = LArgoPataHOokDere_cm;

                REcalcularPuntosLeaderPathRein _REcalcularPuntosLeader = new REcalcularPuntosLeaderPathRein(ListaPtosPerimetroBarras, DatosNuevaBarraDTO_auto, _doc.ActiveView.Scale);
                if (_REcalcularPuntosLeader.Calcular())
                {
                    DatosNuevaBarraDTO_auto.PtoCodoDireztriz = _REcalcularPuntosLeader.PtoCodo;
                    DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = _REcalcularPuntosLeader.PtoDireccion;
                    DatosNuevaBarraDTO_auto.PtoTag = _REcalcularPuntosLeader.PtoTag + (_casosFundDTO.SuperiorVertical ? _REcalcularPuntosLeader.Despla_tag_barraVErticaSup : XYZ.Zero);
                    DatosNuevaBarraDTO_auto.LeaderEnd = _REcalcularPuntosLeader.PtoFree;
                }
            }
            DatosNuevaBarraDTO_auto.IsLuzSecuandiria = true;
            DatosNuevaBarraDTO_auto.IsLibre = true;



            if (casoPathORebar == "Path")
            {
                if (_casosFundDTO.InferiorVertical)
                {   //horizo
                    //  p4   p3     |________|
                    //  p1   p2     
                    DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
                    CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, DatosNuevaBarraDTO_auto);
                }

                if (_casosFundDTO.SuperiorVertical)
                {
                    //horizo         ________
                    //  p4   p3     |        |
                    //  p1   p2     
                    DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                    CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, ListaPtosPerimetroBarras, DatosNuevaBarraDTO_auto);
                }
            }
            else if (casoPathORebar == "Rebar")
            {
                //   DatosNuevaBarraDTO_auto.DiametroMM = _datosNuevaBarraDTOIniciales_original.DiametroMM_fundV;
                //    DatosNuevaBarraDTO_auto.EspaciamientoFoot = _datosNuevaBarraDTOIniciales_original.Espaciamiento_fundV_Foot;
                if (_casosFundDTO.InferiorVertical)
                {

                    // vertical    _     
                    //p1    p4      |
                    //p2    p3     _|

                    //FundIndividuas.ListaVertices = ListaPtosPerimetroBarras;
                    //DatosNuevaBarraDTO_auto.TipoBarra = "f11_fund";// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                    //DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
                    ManejadorRebarFund barraManejador_Izq_dere = new ManejadorRebarFund(_uiapp, DatosNuevaBarraDTO_auto, seleccionarFundConMouse);
                    barraManejador_Izq_dere.DibujarBarra(FundIndividuas, UbicacionLosa.Inferior, TipoCaraUbicacion.Inferior);
                }

                if (_casosFundDTO.SuperiorVertical)
                {

                    // vertical      _   
                    //p1    p4      |
                    //p2    p3      |_

                    //FundIndividuas.ListaVertices = ListaPtosPerimetroBarras;
                    //DatosNuevaBarraDTO_auto.TipoBarra = "f10_fund";// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                    //DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                    ManejadorRebarFund barraManejador_Izq_dere = new ManejadorRebarFund(_uiapp, DatosNuevaBarraDTO_auto, seleccionarFundConMouse);
                    barraManejador_Izq_dere.DibujarBarra(FundIndividuas, UbicacionLosa.Inferior, TipoCaraUbicacion.Inferior);
                }
            }
            return true;
        }

        private void M3_1_ObtenerPtoMouseConPerimetroV_inf(bool IsSuperiorVertical)
        {
            var ElemeSele = (IsSuperiorVertical ? TipoCaraObjeto.Superior : TipoCaraObjeto.Inferior);
            XYZ p1 = ListaPtosPerimetroBarras[0];
            XYZ p2 = ListaPtosPerimetroBarras[1];
            XYZ p3 = ListaPtosPerimetroBarras[2];
            XYZ p4 = ListaPtosPerimetroBarras[3];

            PtoMouse = XYZ.Zero;
            XYZ Direc3_2 = (p3 - p2).Normalize();
            double largo3_2 = p3.DistanceTo(p2);
            XYZ Direc1_2 = (p1 - p2).Normalize();
            double largo_2 = p1.DistanceTo(p2);

            if (IsSuperiorVertical)
            {
                PtoMouse = p1 + Direc3_2 * Util.CmToFoot(25) - Direc1_2 * Util.CmToFoot(22);
                DatosNuevaBarraDTO_auto.PtoMouse = PtoMouse;
                DatosNuevaBarraDTO_auto.PtoCodoDireztriz = PtoMouse + Direc1_2 * 2 + Direc3_2 * 1; ;
                DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = DatosNuevaBarraDTO_auto.PtoCodoDireztriz + Direc3_2 * 2;
            }
            else 
            {
                PtoMouse = p3 + -Direc3_2 * Util.CmToFoot(25) + Direc1_2 * Util.CmToFoot(22);
                DatosNuevaBarraDTO_auto.PtoMouse = PtoMouse;
                DatosNuevaBarraDTO_auto.PtoCodoDireztriz = PtoMouse - Direc1_2 * 2 - Direc3_2 * 2; ;
                DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = DatosNuevaBarraDTO_auto.PtoCodoDireztriz + Direc3_2 * 2;
            }
            
        }


    }
}