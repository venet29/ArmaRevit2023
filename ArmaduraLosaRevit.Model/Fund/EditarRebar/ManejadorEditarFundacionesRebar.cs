using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Extension.modelo;
using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.Fund.Servicios;
using ArmaduraLosaRevit.Model.Fund.WPFfund;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ArmaduraLosaRevit.Model.Fund.Editar
{
    public class ManejadorEditarFundacionesRebar
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ManejadorEditarFundacionesRebar(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        public Result EjecutarEdicionDatos()
        {
            Result result = Result.Succeeded;
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                Element el = Util.GetSingleSelectedElement(_uiapp.ActiveUIDocument);
                SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto;
                if (el is PathReinSpanSymbol)
                {
                    seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiapp.ActiveUIDocument, _uiapp.Application);
                    if (!seleccionarPathReinfomentConPto.AsignarPathReinformentSymbolFund(el))
                    {
                        //Util.ErrorMsg("Error al obtener parametros del PathReinforment previamente selecionado");
                        seleccionarPathReinfomentConPto = null;
                        return Result.Failed;
                    }

                    if (seleccionarPathReinfomentConPto.PathReinforcement == null) seleccionarPathReinfomentConPto = null;
                }
                else
                {
                    seleccionarPathReinfomentConPto = null;
                }

                // ManejadorWPF manejadorWPF = new ManejadorWPF(commandData, seleccionarPathReinfomentConPto, tabEditarPat);
                //  manejadorWPF.Execute();


                if (seleccionarPathReinfomentConPto == null) return Result.Failed;

                DatosEditarFundacionesDTO _datosEditarFundacionesDTO = new DatosEditarFundacionesDTO()
                {
                    _seleccionarPathReinfomentConPto = seleccionarPathReinfomentConPto,
                    _PathReinforcement = seleccionarPathReinfomentConPto.PathReinforcement,

                    Diametro_mm = 10,
                    _Espaciamiento_foot = Util.CmToFoot(30),
                    _IsCambioEspaciamiento = true,
                    _TipoCambioFund = TipoCambioFund.CambiarDatos,
                    _TipoUbicacionFund = TipoCaraUbicacion.Inferior
                };

                if (_datosEditarFundacionesDTO._TipoCambioFund == TipoCambioFund.CambiarDatos)
                {
                    CambiarDatosFund _CambiarDatosFund = new CambiarDatosFund(_uiapp, _datosEditarFundacionesDTO);
                    if (!_CambiarDatosFund.M1_ObtenerNuevoTipoDIametro()) return Result.Failed;

                    using (TransactionGroup transGroup = new TransactionGroup(_doc))
                    {
                        transGroup.Start("Editarfundacion-NH");

                        if (!_CambiarDatosFund.M3_RedefinirPathPorEspaciamiento()) return Result.Failed;
                        _CambiarDatosFund.M2_Editar();

                        transGroup.Assimilate();
                    }
                }
                else if (_datosEditarFundacionesDTO._TipoCambioFund == TipoCambioFund.CambiarGeom)
                {

                }
            }
            catch (Exception)
            {
                result = Result.Failed;
            }

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return result;
        }

        /// <summary>
        /// edicion de path
        /// selecciona un path y lo redibuja con un rebar
        /// </summary>        
        /// <returns></returns>
        public Result EjecutarEdicion_PathToRebar(DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales, CasosFundDTO _casosFundDTO)
        {
            Result result = Result.Succeeded;
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                Element el = Util.GetSingleSelectedElement(_uiapp.ActiveUIDocument);
                SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiapp);
                if (el is PathReinSpanSymbol)
                {
                    if (!seleccionarPathReinfomentConPto.AsignarPathReinformentSymbolFund(el))
                    {
                        //Util.ErrorMsg("Error al obtener parametros del PathReinforment previamente selecionado");
                        seleccionarPathReinfomentConPto = null;
                        return Result.Failed;
                    }

                    if (seleccionarPathReinfomentConPto.PathReinforcement == null) seleccionarPathReinfomentConPto = null;
                }
                else
                {
                    if (!seleccionarPathReinfomentConPto.SeleccionarPathReinforment(IsSaltarRoom: true))
                    {
                        //Util.ErrorMsg("Error al obtener parametros del PathReinforment previamente selecionado");
                        seleccionarPathReinfomentConPto = null;
                        return Result.Failed;
                    }
                }

                _datosNuevaBarraDTOIniciales.TipoPataFun = ObtenerTipoDeBArras(_datosNuevaBarraDTOIniciales.TipoPataFun, seleccionarPathReinfomentConPto);

                //a) casos
                if (seleccionarPathReinfomentConPto == null) return Result.Failed;

                var path = seleccionarPathReinfomentConPto.PathReinforcement;

                var hostId = path.GetHostId();
                if (hostId == null)
                {
                    return Result.Failed; ;
                }
                var hostElement = _doc.GetElement(hostId);

                //  hostElement.ob

                List<XYZ> listapto = path.ObtenerPtoPerimetro_NivelCaraHost();

                if (listapto.Count != 4)
                {
                    Util.ErrorMsg("Error al obtener coordenadas de pathReinforment");
                    return Result.Cancelled;
                }

                DatosRebarFundDTO _DatoRebarFundDTO = new DatosRebarFundDTO()
                {
                    CasosFundDTO = _casosFundDTO,
                    P1 = listapto[0],
                    P2 = listapto[1],
                    P3 = listapto[2],
                    P4 = listapto[3],
                    fund = hostElement,
                    PtoMOuse_sobreFundacion = seleccionarPathReinfomentConPto.UbicacionPathReinforcementSymbol,
                    TagHeadPosition = seleccionarPathReinfomentConPto._TagHeadPosition,
                    LeaderElbow = seleccionarPathReinfomentConPto._LeaderElbow,
                    IsLeaderElbow = seleccionarPathReinfomentConPto.IsLeaderElbow,
                    LeaderEnd = seleccionarPathReinfomentConPto._LeaderEnd,
                    IsLeaderEnd = seleccionarPathReinfomentConPto.IsLeaderEnd,

                };

                FundManejador _FundManejador = new FundManejador(_uiapp);

                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CambiarPathPorRebarFUND-NH");
                    if (_FundManejador.executeRebar_Fundacion(_datosNuevaBarraDTOIniciales, _DatoRebarFundDTO) == Result.Succeeded)
                    {
                        CompatibilityMethods.DeleteNh(_doc, seleccionarPathReinfomentConPto.PathReinforcement);
                    }
                    t.Assimilate();
                }
            }
            catch (Exception)
            {
                result = Result.Failed;
            }

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return result;
        }

        private TipoPataFund ObtenerTipoDeBArras(TipoPataFund tipoPataFun, SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto)
        {
            if (tipoPataFun == TipoPataFund.Auto)
            {
                HookPAthRein _HookPAthRein = seleccionarPathReinfomentConPto.PathReinforcement.ObtenerHooks();

                if (_HookPAthRein.rebarHookTypePrincipal_end != null && _HookPAthRein.rebarHookTypePrincipal_star != null)
                    return TipoPataFund.Ambos;
                else if (_HookPAthRein.rebarHookTypePrincipal_end != null)
                    return TipoPataFund.DereSup;
                else if (_HookPAthRein.rebarHookTypePrincipal_star != null)
                    return TipoPataFund.IzqInf;
                else
                    return TipoPataFund.Sin;
            }
            else
                return tipoPataFun;
        }
    }
}
