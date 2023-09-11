#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Events;
using System;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraAreaPath.Intervalo;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraMallaRebar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model;
#endregion // Namespaces



namespace ArmaduraLosaRevit.Model.BarraMallaRebar
{
    public class ManejadorBarraVMallaAuto : ManejadorBarraVMalla_BASE
    {

        private List<IntervalosMallaDTOAuto> _listIntervalosMallaDTOAuto;



        private View3D _view3D_buscar;
        private View3D _view3D_paraVisualizar;
        public int CantidadMallasDibujadas { get; internal set; }
        public ManejadorBarraVMallaAuto(UIApplication uiapp, List<IntervalosMallaDTOAuto> ListIntervalosMallaDTOAuto, ISeleccionarNivel _seleccionarNivel)
            : base(uiapp, _seleccionarNivel)
        {

            this._uiapp = uiapp;
            this._listIntervalosMallaDTOAuto = ListIntervalosMallaDTOAuto;
            this._uidoc = uiapp.ActiveUIDocument;
        }

  

        public override void CrearBArra()
        {

            if (_listIntervalosMallaDTOAuto.Count == 0) return;
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();

            try
            {
                UtilStopWatch _utilStopWatch = new UtilStopWatch();
                _utilStopWatch.IniciarMedicion();

                UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                if (!CalculosIniciales()) return;

                ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                List<Level> listaLevel = _seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(_doc.ActiveView);

                _utilStopWatch.StopYContinuar("1) botener Intervalos");

                for (int i = 0; i < _listIntervalosMallaDTOAuto.Count; i++)
                {
                    
                   // _utilStopWatch.StopYContinuar($"1)--->{i} dentro selec botener Intervalos");
                    IntervalosMallaDTOAuto newIntervalosMallaDTOAuto = _listIntervalosMallaDTOAuto[i];
                    SeleccionMuroAreaPathAuto SeleccionMuroAreaPath = new SeleccionMuroAreaPathAuto(_uiapp, newIntervalosMallaDTOAuto, _view3D_buscar);
                    SeleccionMuroAreaPath.Ejecutar_SeleccionarMuroPtoAuto();
                    newIntervalosMallaDTOAuto.IsOk = SeleccionMuroAreaPath.IsOk;
                }

                List<IntervalosMallaDTOAuto> listOK = _listIntervalosMallaDTOAuto.Where(c => c.IsOk == true).ToList();
                InterrvalosPtosPathAreaAuto interrvalosPtosPathArea = new InterrvalosPtosPathAreaAuto(_uiapp, listOK, listaLevel, _view3D_buscar);
                interrvalosPtosPathArea.Ejecutar_auto();

                //5
                _utilStopWatch.StopYContinuar("2) generar datos");
                bool entrar = true;
                if (entrar)
                {
                    _confiWPFEnfierradoDTO = AyudaMallaAUTO.ObtenerConfiguracionIniciaWPF(_doc, CasoAnalisasBarrasElevacion.Automatico);
                    if (!M1_CalculosIniciales()) return;
                    
                    List<BarraVMallaAutoDTO> ListaMallas = new List<BarraVMallaAutoDTO>();
                    int kk = 0;
                    foreach (IntervalosMallaDTO item in interrvalosPtosPathArea.ListaIntervalosMallaDTO)
                    {
                        kk = kk + 1;
                        //  _utilStopWatch.StopYContinuar($"2)--->{kk} dentro ---generar datos");
                      //  _confiWPFEnfierradoDTO = AyudaMallaAUTO.ObtenerConfiguracionIniciaWPF(_doc, CasoAnalisasBarrasElevacion.Automatico);
                       
                        DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(_view, DireccionRecorrido_.ParaleloDerechaVista, 100);

                        DatosMallasAutoDTO _datosMallasAutoDTO = new DatosMallasAutoDTO()
                        {
                            diametroH_mm = item._datosMallasDTO.diametroH_mm,
                            diametroV_mm = item._datosMallasDTO.diametroV_mm,
                            paraCantidadLineasV = item._datosMallasDTO.ObtenerNUmeroMallas_tipoMallaV(), //   AyudaMallaAUTO.ObtenerNUmeroMallas("E.D."),//tipo_mallaV.Text
                            paraCantidadLineasH = item._datosMallasDTO.ObtenerNUmeroMallas_tipoMallaH(),//AyudaMallaAUTO.ObtenerNUmeroMallas("E.D."),
                            espaciemientoH_cm = item._datosMallasDTO.espaciemientoH_cm,
                            espaciemientoV_cm = item._datosMallasDTO.espaciemientoV_cm,
                            tipoMallaH = item._datosMallasDTO.tipoMallaH,
                            tipoMallaV = item._datosMallasDTO.tipoMallaH,
                            tipoSeleccionInf = TipoSeleccionMouse.nivel,
                            tipoSeleccionSup = TipoSeleccionMouse.nivel,
                            espesorFoot = item.EspesorMuroFoot
                        };

                        _DireccionRecorrido.LargoRecorridoCm = Util.FootToCm(item.ListaPtos[0].DistanceTo(item.ListaPtos[1]));
                        //obs2)
                        if (item._muroSeleccionado.IsValidObject == false)
                        {
                            item._muroSeleccionado = _doc.GetElement(new ElementId(item._muroSeleccionadoId));
                        }

                        SeleccionarElementosV _seleccionarElementos = new SeleccionarElementosV(_uiapp, _confiWPFEnfierradoDTO, _listaLevelTotal);
                        _seleccionarElementos.LargoRecorridoHorizontalSeleccionCM = _DireccionRecorrido.LargoRecorridoCm;
                        _seleccionarElementos._largoMuroFoot = item._largoMuroFoot;
                        _seleccionarElementos._espesorMuroFoot = item.EspesorMuroFoot;
                        _seleccionarElementos._ElemetSelect = item._muroSeleccionado;
                        _seleccionarElementos.AsignarPtosDeseleccion_soloMallaAuto(item.ListaPtos[0], item.ListaPtos);
                        _seleccionarElementos.IsCoronacion = item._datosMallasDTO.IsBuscarCororonacion;

                        SelecionarPtoSup selecionarPtoSup = new SelecionarPtoSup(_uiapp, _confiWPFEnfierradoDTO, _listaLevelTotal)
                        {
                            _PtoInicioIntervaloBarra = item.ListaPtos[0],
                            _PtoFinalIntervaloBarra = item.ListaPtos[2],
                            _PtoInicioIntervaloBarra_mallaVertiva = item.ListaPtos_MallaV[0],
                            _PtoFinalIntervaloBarra_mallaVertiva = item.ListaPtos_MallaV[1],
                            _PtoFinalIntervaloBarra_ProyectadoCaraMuroHost = item.ListaPtos[2],
                            _ptoSeleccionMouseCentroCaraMuro = (item.ListaPtos[0] + item.ListaPtos[2]) / 2
                        };
                        selecionarPtoSup.ObtenerIntervalLevel();
                        if (selecionarPtoSup.ListaLevelIntervalo.Count == 0) continue;

                        RecalcularEspaciamientoLineasBarrasVertical(_seleccionarElementos, 3);

                        DatosMuroSeleccionadoDTO _datosmuroSeleccionadoDTO = _seleccionarElementos.M5_OBtenerElementoREferenciaDTO(_DireccionRecorrido);

                        ConfiguracionIniciaWPFlBarraVerticalDTO copy = (ConfiguracionIniciaWPFlBarraVerticalDTO)_confiWPFEnfierradoDTO.Clone();
                        BarraVMallaAutoDTO _ManejadorBarraVMallaAutoDTO =

                            new BarraVMallaAutoDTO(copy, _datosMallasAutoDTO, _DireccionRecorrido, selecionarPtoSup, _datosmuroSeleccionadoDTO);

                        ListaMallas.Add(_ManejadorBarraVMallaAutoDTO);
                    }

                    _listaCreadorBarrasV.Clear();
                    _utilStopWatch.StopYContinuar("3) dibujar");
                    try
                    {
                        using (Transaction tr = new Transaction(_doc, "generando malla auto"))
                        {
                            tr.Start();
                            for (int i = 0; i < ListaMallas.Count; i++)
                            {
                                _utilStopWatch.StopYContinuar($"3)--->{i} dentro dibujar");
                                var item = ListaMallas[i];
                                DibujarBarraMalla_sinTrans _DibujarBarraMalla = new DibujarBarraMalla_sinTrans(_uiapp, item.ConfiWPFEnfierradoDTO, item.DatosMallasAutoDTO, item.DireccionRecorrido);
                                _DibujarBarraMalla.CrearMAllaConRebar_Sintras(item.SelecionarPtoSup, item.DatosmuroSeleccionadoDTO);
                                _listaCreadorBarrasV.AddRange(_DibujarBarraMalla._listaCreadorBarrasV);
                            }

                            CantidadMallasDibujadas = _listaCreadorBarrasV.Count;
                            tr.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ex: {ex.Message} ");
                    }



                    _utilStopWatch.StopYContinuar("4) color");
                    M7_CAmbiarColor();

                   // _utilStopWatch.TerminarYGuardarTxTTiempoTotal("EnfierraAutomatio --malla");
                    _listaCreadorBarrasV.Clear();
                }


                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            }
            catch (Exception ex)
            {
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }

#if DEBUG
            LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, ConstNH.rutaLogNh);
#endif
            return;
        }



        private bool CalculosIniciales()
        {
            _view3D_buscar = TiposFamilia3D.Get3DBuscar(_doc);
            _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);

            if (_view3D_buscar == null) return true;
            if (_view3D_paraVisualizar == null) return true;


            //   ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(_uiapp);
            //    _ManejadorVisibilidadElemenNoEnView.Ejecutar(_view3D_buscar, base._view.Name);

            return true;

        }
    }
}
