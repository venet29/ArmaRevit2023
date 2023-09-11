using ArmaduraLosaRevit.Model.EditarPath.Seleccion;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.DTO;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.EditarTipoPath
{


    public class PathReinformeCambioManejador
    {
        private UIApplication _uiapp;
        private Document _doc;
        //  private ExternalCommandData _commandData;

        #region 0) propiedades
        //  public bool IsDibujarPath { get; set; }
        public PathReinforcement _pathReinforcement { get; set; }

        public ContenedorDatosLosaDTO DatosLosaYpathInicialesDTO { get; set; }
        public XYZ _puntoSeleccionMouse { get; private set; }
        public XYZ _TagHeadPosition { get; set; }
        public PathReinSpanSymbol _pathReinforcementSymbol { get; set; }

        iCalculoDatosParaReinforment calculoDatosParaReinforment;
        private SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto;
        private double diametro;
        private double espaciamiento;
        private TipoPathReinfDTO tipoPathReinf;

        private CoordenadaPath _4pointPathReinf;
        private ContenedorDatosPathReinformeDTO _datosNuevoPathDereArribaDTO;

        #endregion

        #region 1) cotructor

        public PathReinformeCambioManejador(UIApplication _uiapp, SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._pathReinforcement = seleccionarPathReinfomentConPto.PathReinforcement;
            this._puntoSeleccionMouse = seleccionarPathReinfomentConPto.PuntoSeleccionMouse;
            this._seleccionarPathReinfomentConPto = seleccionarPathReinfomentConPto;

        }

        #endregion

        public bool M0_EjecutarCambioPath(TipoPathReinfDTO newTipoPathReinf, double diametro, double espaciamiento)
        {
            this.diametro = diametro;
            this.espaciamiento = espaciamiento;
            this.tipoPathReinf = newTipoPathReinf;
            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("ActualizandoPath-NH");
                    if (M1_GenerarCalculosGenerales(newTipoPathReinf))
                        M2_Crear2PathReinformentPorTraslapo();
                    transGroup.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error M0_EjecutarCambioPath ex:{ex.Message}");
                return false;
            }
            return true;

        }

        public bool M1_GenerarCalculosGenerales(TipoPathReinfDTO newTipoPathReinf)
        {
            try
            {

                //1
                ICalculoTiposTraslapos calculoTiposTraslapos = CalculoTiposTraslapos.CreatorCalculoTiposTraslapos(_seleccionarPathReinfomentConPto.PathReinforcement, _doc);
                if (!calculoTiposTraslapos.IsOk)
                {
                    Util.ErrorMsg($"Error en 'M1_GenerarCalculosGenerales' -> error al obtener datos traslapo");
                    return false;
                }
                // calculoTiposTraslapos.TipoPathReinf_DerArriba = newTipoPathReinf;

                //2
                CalculoDatoslosa calculoDatoslosa = new CalculoDatoslosa(_seleccionarPathReinfomentConPto, _doc);
                DatosLosaYpathInicialesDTO = calculoDatoslosa.ObtenerContenedorDatosLosa();
                if (!DatosLosaYpathInicialesDTO.IsOk)
                {
                    Util.ErrorMsg($"Error en 'M1_GenerarCalculosGenerales' -> error al obtener datos losa");
                    return false;
                }

                //3
                CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_seleccionarPathReinfomentConPto, _doc);
                _4pointPathReinf = pathReinformeCalculos.Calcular4PtosPathReinf();
                if (!pathReinformeCalculos.IsPtoOK)
                {
                    Util.ErrorMsg($"Error en 'M1_GenerarCalculosGenerales' -> error al obtener datos geometricos path");
                    return false;
                }

    

                //final
                calculoDatosParaReinforment =
                    FactoryITraslapo.CreateNewPathReinformentV2(pathReinformeCalculos.Obtener4pointPathReinf(), _puntoSeleccionMouse,
                                                                DatosLosaYpathInicialesDTO, calculoTiposTraslapos);

                if (calculoDatosParaReinforment.IsOK == false) return false;
                double aux_valor = ConstNH.CONST_DESPLA_ENTRE_BARRAS_TRASLAPO_CM;
                ConstNH.CONST_DESPLA_ENTRE_BARRAS_TRASLAPO_CM = 0;

                if (!calculoDatosParaReinforment.M1_Obtener2PathReinformeTraslapoDatos()) return false;

                ConstNH.CONST_DESPLA_ENTRE_BARRAS_TRASLAPO_CM = aux_valor;

                _datosNuevoPathDereArribaDTO = calculoDatosParaReinforment.datosNuevoPathDereArribaDTO;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en calculos generales ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public Result M2_Crear2PathReinformentPorTraslapo()
        {
            try
            {
                if (!calculoDatosParaReinforment.M2_IsPuedoDibujarPath()) return Result.Failed;
                //  calculoDatosParaReinforment.datosNuevoPathIzqAbajoDTO.AnguloRoomRad = DatosLosaYpathInicialesDTO.AnguloRoomRad;
                _datosNuevoPathDereArribaDTO.AnguloRoomRad = DatosLosaYpathInicialesDTO.AnguloRoomRad;
                _datosNuevoPathDereArribaDTO.coordenadas = _4pointPathReinf;
                _datosNuevoPathDereArribaDTO.Lista4ptosPAth = _4pointPathReinf.GetListaXYZ(); ;
                _datosNuevoPathDereArribaDTO.LargoPathreiforment = _4pointPathReinf.p2.DistanceTo(_4pointPathReinf.p3);
                _datosNuevoPathDereArribaDTO.ptoTagSoloTraslapo = _seleccionarPathReinfomentConPto._TagHeadPosition;
                _datosNuevoPathDereArribaDTO.datosLosaDTO.diametroEnFoot = Util.CmToFoot(diametro / 10);
                _datosNuevoPathDereArribaDTO.datosLosaDTO.tipoBarra = tipoPathReinf.Tipobarra;
                _datosNuevoPathDereArribaDTO.datosLosaDTO.ubicacionLosa = tipoPathReinf.Direccion;
                _datosNuevoPathDereArribaDTO.datosLosaDTO.Espaciamiento = Util.CmToFoot(espaciamiento);
                _datosNuevoPathDereArribaDTO.datosLosaDTO.TipoDireccionBarraVocal = tipoPathReinf.TipoDireccionBarraVocal;
                _datosNuevoPathDereArribaDTO.Prefijo_F = _seleccionarPathReinfomentConPto._Prefijo_F;
                // BarraRoom newBarralosa_IzqAbajo = M2_1_CrearNewBarraCambioTipo(calculoDatosParaReinforment.datosNuevoPathIzqAbajoDTO, new XYZ(0, 0, 0));

                BarraRoom newBarralosa_dere = M2_1_CrearNewBarraCambioTipo(_datosNuevoPathDereArribaDTO, new XYZ(0, 0, 0));
                if (newBarralosa_dere == null) return Result.Failed;

                _pathReinforcement = newBarralosa_dere.m_createdPathReinforcement;
                _pathReinforcementSymbol = newBarralosa_dere._PathReinSpanSymbol;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'M2_Crear2PathReinformentPorTraslapo' ex:{ex.Message}");

                return Result.Cancelled;
            }

            return Result.Succeeded;

        }

        private BarraRoom M2_1_CrearNewBarraCambioTipo(ContenedorDatosPathReinformeDTO contenedorDatosPathReinformeDTO, XYZ despla)
        {
            try
            {
                BarraRoom newBarralosa_ = new BarraRoom(_uiapp, contenedorDatosPathReinformeDTO);

                if (newBarralosa_.statusbarra == Result.Succeeded)
                {
                   Result  result=   newBarralosa_.CrearBarraPorTraslapo(despla);

                    if (result != Result.Succeeded) return null;
                }

                //borrar antiguo
                if (newBarralosa_ != null)
                    M2_2_BorrarPathReinfAntigua(newBarralosa_);

                if (VariablesSistemas.IsReSeleccionarPuntoRango)
                {
                    SelecionarPathSeleccionadoParteSuperior _SelecionarPathCreadoParteSuperior = new SelecionarPathSeleccionadoParteSuperior(_uiapp, newBarralosa_._PathReinSpanSymbol);
                    _SelecionarPathCreadoParteSuperior.Ejecutar();
                    return newBarralosa_;
                }
                return newBarralosa_;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en '' ex:{ex.Message}");
                return null;
            }

        }

        private void M2_2_BorrarPathReinfAntigua(BarraRoom newBarralosa_)
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Borrar pathreiforment por traslapo-NH");

                    if (newBarralosa_.statusbarra == Result.Succeeded && _pathReinforcement.IsValidObject)
                    {
                        _doc.Delete2(_pathReinforcement);
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }

        }
    }




}
