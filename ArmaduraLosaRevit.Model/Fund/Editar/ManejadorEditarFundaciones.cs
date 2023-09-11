using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.Fund.Editar
{
    public class ManejadorEditarFundaciones
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ManejadorEditarFundaciones(UIApplication uiapp)
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

                DatosEditarFundacionesDTO _datosEditarFundacionesDTO = new DatosEditarFundacionesDTO() {
                    _seleccionarPathReinfomentConPto= seleccionarPathReinfomentConPto,
                    _PathReinforcement = seleccionarPathReinfomentConPto.PathReinforcement,

                    Diametro_mm = 10,
                    _Espaciamiento_foot = Util.CmToFoot(30),
                    _IsCambioEspaciamiento=true,
                    _TipoCambioFund=TipoCambioFund.CambiarDatos,
                    _TipoUbicacionFund =TipoCaraUbicacion.Inferior 
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

        public Result EjecutarEdicionForma()
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

    }
}
