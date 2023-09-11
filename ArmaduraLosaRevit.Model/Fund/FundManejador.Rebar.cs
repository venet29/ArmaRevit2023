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
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

namespace ArmaduraLosaRevit.Model.Fund
{

    public partial class FundManejador
    {


        public Result executeRebar_Fundacion(DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales, DatosRebarFundDTO _DatoRebarFundDTO)
        {
            try
            {
                CasosFundDTO _casosFundDTO = _DatoRebarFundDTO.CasosFundDTO;
                _datosNuevaBarraDTOIniciales_original = _datosNuevaBarraDTOIniciales;
                //1)seleccionana
                seleccionarFundConMouse = new SeleccionarFundConMouse(_uiapp);

                seleccionarFundConMouse.PtoMOuse_sobreFundacion = _DatoRebarFundDTO.PtoMOuse_sobreFundacion;
                seleccionarFundConMouse._elementSelecciondo = _DatoRebarFundDTO.fund;
                seleccionarFundConMouse.M3_ObtenerCarasSUperiorInferior();

                fund = _DatoRebarFundDTO.fund;

                ListaPtosPerimetroBarras = new List<XYZ>();
                p1 = _DatoRebarFundDTO.P1;
                p2 = _DatoRebarFundDTO.P2;
                p3 = _DatoRebarFundDTO.P3;
                p4 = _DatoRebarFundDTO.P4;



                ListaPtosPerimetroBarras.Add(p1);
                ListaPtosPerimetroBarras.Add(p2);
                ListaPtosPerimetroBarras.Add(p3);
                ListaPtosPerimetroBarras.Add(p4);

                FundIndividuas = new FundIndividual()
                {
                    ListaVertices = ListaPtosPerimetroBarras,
                };

                // horizontal *** definir tipo de barras
                DatosNuevaBarraDTO_auto = new DatosNuevaBarraDTO();
                double angulo = Util.angulo_entre_pt_Grado_XY0(p2, p3);

                GenerarCoordenadasTAg(_DatoRebarFundDTO);

                if (-45 < angulo && angulo < 45)
                {
                    if (_casosFundDTO.InferiorHorizontal)
                    {
                        DatosNuevaBarraDTO_auto.TipoBarra = BuscarTipoBarraInferior(_datosNuevaBarraDTOIniciales_original.TipoPataFun);// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
                    }
                    if (_casosFundDTO.SuperiorHorizontal)
                    {
                        DatosNuevaBarraDTO_auto.TipoBarra = BuscarTipoBarraSuperior(_datosNuevaBarraDTOIniciales_original.TipoPataFun);// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                    }
                    _datosNuevaBarraDTOIniciales_original.DiametroMM_fundH = _datosNuevaBarraDTOIniciales_original.DiametroMM;
                    _datosNuevaBarraDTOIniciales_original.Espaciamiento_fundH_Foot = _datosNuevaBarraDTOIniciales_original.EspaciamientoFoot;

                    M2_CrearBarraHorizontal(_DatoRebarFundDTO.CasosFundDTO, _casosFundDTO.CasoTipoBArra);
                }
                else
                {
                    if (_casosFundDTO.InferiorHorizontal)
                    {
                        _casosFundDTO.InferiorHorizontal = false;
                        _casosFundDTO.InferiorVertical = true;
                        DatosNuevaBarraDTO_auto.TipoBarra = BuscarTipoBarraInferior(_datosNuevaBarraDTOIniciales_original.TipoPataFun);// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Inferior;
                    }
                    else
                    {
                        _casosFundDTO.SuperiorHorizontal = false;
                        _casosFundDTO.SuperiorVertical = true;
                        DatosNuevaBarraDTO_auto.TipoBarra = BuscarTipoBarraSuperior(_datosNuevaBarraDTOIniciales_original.TipoPataFun);// (_casosFundDTO.InferiorHorizontal ? "f11" : "f10");
                        DatosNuevaBarraDTO_auto.TipoCaraObjeto_ = TipoCaraObjeto.Superior;
                    }
                    _datosNuevaBarraDTOIniciales_original.DiametroMM_fundV = _datosNuevaBarraDTOIniciales_original.DiametroMM;
                    _datosNuevaBarraDTOIniciales_original.Espaciamiento_fundV_Foot = _datosNuevaBarraDTOIniciales_original.EspaciamientoFoot;

                    M3_CrearBArrasVertical(_DatoRebarFundDTO.CasosFundDTO, _DatoRebarFundDTO.CasosFundDTO.CasoTipoBArra);
                }
            }
            catch (System.Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Cancelled;
            }
            return Result.Succeeded;
        }

        private void GenerarCoordenadasTAg(DatosRebarFundDTO _DatoRebarFundDTO)
        {
            DatosNuevaBarraDTO_auto.PtoMouse = _DatoRebarFundDTO.PtoMOuse_sobreFundacion;

            if (_DatoRebarFundDTO.IsLeaderElbow)
                DatosNuevaBarraDTO_auto.PtoCodoDireztriz = _DatoRebarFundDTO.LeaderElbow;

            XYZ vector_p2_p3 = (p2 - p3).Normalize();
            double productoEsaclar = Util.GetProductoEscalar(vector_p2_p3, (_DatoRebarFundDTO.TagHeadPosition - _DatoRebarFundDTO.PtoMOuse_sobreFundacion));

            DatosNuevaBarraDTO_auto.PtoDireccionDirectriz = (productoEsaclar > 0 ? vector_p2_p3 : -vector_p2_p3);
            XYZ vector_p1_p2 = (p1 - p2).Normalize();
            double productoEsaclar2 = Util.GetProductoEscalar(vector_p1_p2, (_DatoRebarFundDTO.LeaderElbow - _DatoRebarFundDTO.PtoMOuse_sobreFundacion));
          
            //// se podr
            //if(Util.IsNOParaleloX(vector_p2_p3))
            //    vector_p1_p2 = (productoEsaclar2 > 0 ? -vector_p1_p2 * Util.CmToFoot(11) : -vector_p1_p2 * Util.CmToFoot(11));
            //else
            //    vector_p1_p2 = (productoEsaclar2 > 0 ? vector_p1_p2 * Util.CmToFoot(2.15) : vector_p1_p2 * Util.CmToFoot(2.15));

            vector_p1_p2 = (Util.IsNOParaleloX(vector_p2_p3) ? -vector_p1_p2 * Util.CmToFoot(11) : vector_p1_p2 * Util.CmToFoot(2.15));

            DatosNuevaBarraDTO_auto.PtoTag = _DatoRebarFundDTO.TagHeadPosition + vector_p1_p2;

            if (_DatoRebarFundDTO.IsLeaderEnd)
                DatosNuevaBarraDTO_auto.LeaderEnd = _DatoRebarFundDTO.LeaderEnd;
        }

        private string BuscarTipoBarraSuperior(TipoPataFund tipoPataFun)
        {
            switch (tipoPataFun)
            {
                case TipoPataFund.IzqInf:
                    return "f10A_fund";
                case TipoPataFund.DereSup:
                    return "f10B_fund";
                case TipoPataFund.Ambos:
                    return "f10_fund";
                case TipoPataFund.Sin:
                    return "f3_fund";
                case TipoPataFund.Auto:
                    return "f10_fund";
                default:
                    return "";
            }
        }

        private string BuscarTipoBarraInferior(TipoPataFund tipoPataFun)
        {
            switch (tipoPataFun)
            {
                case TipoPataFund.IzqInf:

                    return "f11A_fund";
                case TipoPataFund.DereSup:
                    return "f11B_fund";
                case TipoPataFund.Ambos:
                    return "f11_fund";
                case TipoPataFund.Sin:
                    return "f3_fund";
                case TipoPataFund.Auto:
                    return "f11_fund";
                default:
                    return "";
            }
        }
    }
}