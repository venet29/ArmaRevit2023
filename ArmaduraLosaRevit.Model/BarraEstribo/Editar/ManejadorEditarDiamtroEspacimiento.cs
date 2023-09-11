using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;



namespace ArmaduraLosaRevit.Model.BarraEstribo.Editar
{
    public class EditarDiamtroEspacimientoDTO
    {

        public double diametro_mm { get; set; }
        public double espaciamiento_foot { get; set; }
    }

    public class ManejadorEditarDiamtroEspacimiento
    {
        private UIApplication _uiapp;
        private DatosConfinamientoAutoDTO _datoEdicion;
        private Document _doc;
        private View _view;
        private RebarBarType _rebarBarType_estribo;
        private RebarBarType _rebarBarType_lateral;
        private RebarBarType _rebarBarType_traba;
        private UIApplication uiapp;
        private DatosMallasAutoDTO _datosMalla;
        private RebarBarType _rebarBarType_MallaH;
        private RebarBarType _rebarBarType_MallaV;

        public ManejadorEditarDiamtroEspacimiento(UIApplication uiapp, DatosConfinamientoAutoDTO configuracionInicialEstriboDTO)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._datoEdicion = configuracionInicialEstriboDTO;

        }

        public ManejadorEditarDiamtroEspacimiento(UIApplication uiapp, DatosMallasAutoDTO datosMalla)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._datosMalla = datosMalla;
        }

        public bool CalculosIniciales()
        {
            try
            {

                if (_datoEdicion.IsEstribo == true)
                {

                    _rebarBarType_estribo = TiposRebarBarType.getRebarBarType("Ø" + _datoEdicion.DiamtroEstriboMM, _doc, true);

                    if (_rebarBarType_estribo == null)
                    {
                        Util.ErrorMsg($"Error al obtener el tipo {"Ø " + _datoEdicion.DiamtroEstriboMM}  de estribo");
                        return false;
                    }
                }

                if (_datoEdicion.IsLateral == true)
                {

                    _rebarBarType_lateral = TiposRebarBarType.getRebarBarType("Ø" + _datoEdicion.DiamtroLateralEstriboMM, _doc, true);

                    if (_rebarBarType_lateral == null)
                    {
                        Util.ErrorMsg($"Error al obtener el tipo {"Ø " + _datoEdicion.DiamtroLateralEstriboMM}  de lateral");
                        return false;
                    }
                }

                if (_datoEdicion.IsTraba == true)
                {

                    _rebarBarType_traba = TiposRebarBarType.getRebarBarType("Ø" + _datoEdicion.DiamtroTrabaEstriboMM, _doc, true);

                    if (_rebarBarType_traba == null)
                    {
                        Util.ErrorMsg($"Error al obtener el tipo {"Ø " + _datoEdicion.DiamtroTrabaEstriboMM}  de traba");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ManejadorEditarDiamtroEspacimiento Inicial'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool CalculosIniciales_Malla()
        {
            try
            {

                if (_datosMalla.IsMallaH == true)
                {

                    _rebarBarType_MallaH = TiposRebarBarType.getRebarBarType("Ø" + _datosMalla.diametroH_mm, _doc, true);

                    if (_rebarBarType_MallaH == null)
                    {
                        Util.ErrorMsg($"Error al obtener el tipo {"Ø " + _datosMalla.diametroH_mm}  de estribo");
                        return false;
                    }
                }

                if (_datosMalla.IsMallaV == true)
                {

                    _rebarBarType_MallaV = TiposRebarBarType.getRebarBarType("Ø" + _datosMalla.diametroV_mm, _doc, true);

                    if (_rebarBarType_MallaV == null)
                    {
                        Util.ErrorMsg($"Error al obtener el tipo {"Ø " + _datosMalla.diametroV_mm}  de lateral");
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ManejadorEditarDiamtroEspacimiento inicial Malla'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool Ejecutar(string tipo)
        {
            try
            {
                //seleccionar
                SeleccionarRebarRectanguloWrapperRebar administrador_ReferenciaRoom = new SeleccionarRebarRectanguloWrapperRebar(_uiapp, TipoBarraGeneral.Elevacion, tipo);

                if (!administrador_ReferenciaRoom.M1_GetRoomSeleccionadosConRectaguloYFiltros()) return false;

                if (!administrador_ReferenciaRoom.M2_ObtenerListaWrapperRebar()) return false;

                if (!administrador_ReferenciaRoom.M3_b_ObtenerListaIDSeleccionadosElevaciones()) return false;

                using (Transaction tr = new Transaction(_doc, "EditarEstribo-NH"))
                {
                    tr.Start();

                    for (int i = 0; i < administrador_ReferenciaRoom.ListaWrapperRebarFiltro.Count; i++)
                    {
                        var elementBarra = administrador_ReferenciaRoom.ListaWrapperRebarFiltro[i];
                        Rebar rebarAnalizada = ((Rebar)elementBarra.element);

                        if (rebarAnalizada.LayoutRule == RebarLayoutRule.Single)
                        {
                            continue;
                        }
                        double diam_foot = rebarAnalizada.ObtenerDiametroFoot();
                        // calcularEspacimiento
                        double espaciamiento_foot = rebarAnalizada.MaxSpacing;
                        double CAntiadaBArras = rebarAnalizada.NumberOfBarPositions;
                        double largoEstribo = espaciamiento_foot * CAntiadaBArras;

                        if (elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES || elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO || elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_V)
                        {
                            if (_datoEdicion.IsEstribo == false) continue;

                            //cambia diamtro
                            if (!Util.IsSimilarValor(_datoEdicion.DiamtroEstriboMM, Util.FootToMm(diam_foot), 1))
                            {
                                rebarAnalizada.ChangeTypeId(_rebarBarType_estribo.Id);
                            }
                            //cambiar
                            int numeroBarras = (int)Math.Round((largoEstribo / Util.CmToFoot(_datoEdicion.espaciamientoEstriboCM)), 0);
                            rebarAnalizada.NumberOfBarPositions = numeroBarras;
                            rebarAnalizada.MaxSpacing = Util.CmToFoot(_datoEdicion.espaciamientoEstriboCM);
                        }
                        else if (elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_L || elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VL)
                        {
                            if (_datoEdicion.IsLateral == false) continue;

                            //cambia diamtro
                            if (!Util.IsSimilarValor(_datoEdicion.DiamtroLateralEstriboMM, Util.FootToMm(diam_foot), 1))
                            {
                                rebarAnalizada.ChangeTypeId(_rebarBarType_lateral.Id);
                            }
                        }
                        else if (elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_T || elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO_T)
                        {
                            if (_datoEdicion.IsTraba == false) continue;
                            if (!Util.IsSimilarValor(_datoEdicion.DiamtroTrabaEstriboMM, Util.FootToMm(diam_foot), 1))
                            {
                                rebarAnalizada.ChangeTypeId(_rebarBarType_traba.Id);
                            }

                            //cambiar
                            int numeroBarras = (int)Math.Round((largoEstribo / Util.CmToFoot(_datoEdicion.espaciamientoTrabaCM)), 0);
                            rebarAnalizada.NumberOfBarPositions = numeroBarras;
                            rebarAnalizada.MaxSpacing = Util.CmToFoot(_datoEdicion.espaciamientoTrabaCM);
                        }
                        else if (elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_H || elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_V)
                        {

                            double diam_mm = 0;
                            double espaciamiento_cm = 0;

                            var elementoCOnt = _doc.GetElement(rebarAnalizada.GetHostId());
                            if (elementoCOnt != null)
                            {
                                double espesor_foot = elementoCOnt.ObtenerEspesorConCaraVerticalVIsible_foot(_view);
                                _datosMalla.espesorFoot = espesor_foot;
                            }
                            
                            var NuevoTExtoMalla = _datosMalla.ObtenerTExto();
                            if (elementBarra.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_H)
                            {
                                diam_mm = _datosMalla.diametroH_mm;
                                espaciamiento_cm = _datosMalla.espaciemientoH_cm;
                                if (!Util.IsSimilarValor(diam_mm, Util.FootToMm(diam_foot), 1))
                                {
                                    rebarAnalizada.ChangeTypeId(_rebarBarType_MallaH.Id);
                                }
                            }
                            else
                            {
                                diam_mm = _datosMalla.diametroV_mm;
                                espaciamiento_cm = _datosMalla.espaciemientoV_cm;
                                if (!Util.IsSimilarValor(diam_mm, Util.FootToMm(diam_foot), 1))
                                {
                                    rebarAnalizada.ChangeTypeId(_rebarBarType_MallaV.Id);
                                }
                            }

                            var paramMallaPARA = ParameterUtil.FindParaByName(rebarAnalizada, "MallaRebarMuro");

                            if (NuevoTExtoMalla != "" && paramMallaPARA != null)
                            {
                    
                  
                                var textomalla = paramMallaPARA.AsString();
                                ParameterUtil.SetParaInt(rebarAnalizada, "MallaRebarMuro", NuevoTExtoMalla);//(30+100+30)
                            }
                            //cambia diamtro
                            //cambiar
                            int numeroBarras = (int)Math.Round((largoEstribo / Util.CmToFoot(espaciamiento_cm)), 0);
                            rebarAnalizada.NumberOfBarPositions = numeroBarras;
                            rebarAnalizada.MaxSpacing = Util.CmToFoot(espaciamiento_cm);
                        }
                    }

                    tr.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ManejadorEditarDiamtroEspacimiento'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }

}
