
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraV.ColorRebar;
using System.IO;
using ArmaduraLosaRevit.Model.BarraV.Dimensiones;
using ArmaduraLosaRevit.Model.BarraV.ServicioManejadorBarraH;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Traslapos;
using ArmaduraLosaRevit.Model.BarraV.AppState;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.model;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.ayuda;
using ArmaduraLosaRevit.Model.Prueba.User;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;

namespace ArmaduraLosaRevit.Model.BarraV
{

    //clase los para barras horizontales // no incido laterales  ni barras horrizontales de muro
    public class ManejadorBarraH
    {
        protected UIApplication _uiapp;
        protected UIDocument _uidoc;
        protected View _view;
        protected Document _doc;
        protected ISeleccionarNivel _seleccionarNivel;
        protected ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO;
        protected List<IbarraBase> _listaDebarra;
        // private List<Level> _listaLevelTotal; //no se esta usando se desactiva pq para 
        //List<ABarrasV> listaBarrasV;
#pragma warning disable CS0169 // The field 'ManejadorBarraH._traslapoDTO' is never used
        private TraslapoDTO _traslapoDTO;
#pragma warning restore CS0169 // The field 'ManejadorBarraH._traslapoDTO' is never used
        private List<TraslapoDTO> ListaTraslapo;
        protected ManejadorTraslapo _manejadorTraslapo;
        protected List<Rebar> _listaRebar;
        protected List<PtoDimesionesTraslapoDTO> _listaDimensiones;
        protected DireccionTraslapoH _ubicacionTraslapo;
        protected List<XYZ> _listaptoTramo;
        protected DatosMuroSeleccionadoDTO _vigaSeleccionadoDTO;
        protected SeleccionarElementosH _seleccionarElementos;
        public ElementoSeleccionado _elementoSeleccionado { get; set; }
        public bool IsdibujarDimension { get; set; }

        protected bool IsdibujarTraslapo;

        public ManejadorBarraH(UIApplication uiapp, ISeleccionarNivel seleccionarNivel, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO)
        {
            this._uiapp = uiapp;
            this._uidoc = uiapp.ActiveUIDocument;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this._doc = _uidoc.Document;
            this._seleccionarNivel = seleccionarNivel;
            this._configuracionInicialBarraHorizontalDTO = confiEnfierradoDTO;
            _listaDebarra = new List<IbarraBase>();
            //  _listaLevelTotal = new List<Level>();
            _listaRebar = new List<Rebar>();
            _listaDimensiones = new List<PtoDimesionesTraslapoDTO>();

            _ubicacionTraslapo = confiEnfierradoDTO.DireccionTraslapoH_;
            IsdibujarDimension = true;
            IsdibujarTraslapo = true;
            ListaTraslapo = new List<TraslapoDTO>();
            _manejadorTraslapo = new ManejadorTraslapo(uiapp);
        }
        /// <summary>
        /// comando necesitad definir 2 parametros importate
        /// -->  DatosMuroSeleccionadoDTO _vigaSeleccionadoDTO   -- configuracion elementos
        /// -->  List<XYZ> _listaptoTramo   --> intervalos de los ptos inicial y final de barras
        /// </summary>
        public virtual void CrearBArraHorizontalTramo()
        {

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR CrearBArraHorizontalTramov2 ");

            if (!resultadoConexion)
            {
                Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                return;
            }
            else if (!_ManejadorUsuarios.ObteneResultado())
            {
                Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                return;
            }


            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return;

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
          //  UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            _listaRebar.Clear();
            try
            {


                AppManejadorBarraHState.ReseteaEstados();
                if (!M1_CalculosIniciales(1) || (!Directory.Exists(ConstNH.CONST_COT))) return;

                DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(_view, DireccionRecorrido_.PerpendicularEntradoVista);

                _seleccionarElementos = new SeleccionarElementosH(_uiapp, _configuracionInicialBarraHorizontalDTO, _DireccionRecorrido);

                if (!_seleccionarElementos.M1_ObtenerPtoinicio_RefuerzoBorde()) return;


                AppManejadorBarraHState.VerificarEstado(_seleccionarElementos);
                #region MyRegion
                if (!EjecutarCAlculos()) return;

                if (!GenerarIntervalosBarras()) return;

                ConfiguracionTAgBarraDTo confBarraTagSindirectriz = Obtener_ConfiguracionTAgBarraDTo.Ejecutar(_vigaSeleccionadoDTO.DireccionEnFierrado, false, XYZ.Zero);

                #endregion

                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("CrearGrupoBarraHorizontal-NH");
                    M6_DibujarBarras(confBarraTagSindirectriz);

                    ColorRebarAsignar _ColorRebar = new ColorRebarAsignar(_uiapp, _listaRebar);
                    _ColorRebar.M7_CAmbiarColor();

                    transGroup.Assimilate();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
          //  UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            return;
        }
        private bool EjecutarCAlculos()
        {
            try
            {
                _vigaSeleccionadoDTO = _seleccionarElementos.M2_OBtenerElementoREferenciaDTO();
                if (_vigaSeleccionadoDTO == null) return false;

                //_elementoSeleccionado = _seleccionarElementos._elementoSeleccionado;
                //2
                ListaPtoTramo _ListaPtoTramo = new ListaPtoTramo(_uiapp, _configuracionInicialBarraHorizontalDTO);
                if (_ListaPtoTramo.M2_ListaPtoTramo())
                    _listaptoTramo = _ListaPtoTramo._listaptoTramo;
                else
                {
                    _listaptoTramo = new List<XYZ>();
                    _listaptoTramo.Add(_seleccionarElementos.CurvaBorde.GetEndPoint(0));
                    _listaptoTramo.Add(_seleccionarElementos.CurvaBorde.GetEndPoint(1));
                }

                //mejor pasar aa otra caso
                for (int i = 0; i < _listaptoTramo.Count - 1; i++)
                {
                    if (_listaptoTramo[i].DistanceTo(_listaptoTramo[i + 1]) < ConstNH.CONST_LARGOMIN_BARRA_FOOT)
                        return false;

                    _listaptoTramo[i] = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(_vigaSeleccionadoDTO.NormalEntradoView, _vigaSeleccionadoDTO.ptoSeleccionMouseCentroCaraMuro6, _listaptoTramo[i]);
                }

                if(_listaptoTramo.Count>0)
                    _listaptoTramo[_listaptoTramo.Count - 1] = 
                        ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(_vigaSeleccionadoDTO.NormalEntradoView, _vigaSeleccionadoDTO.ptoSeleccionMouseCentroCaraMuro6, _listaptoTramo[_listaptoTramo.Count - 1]);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'SeleccionarElementosHAuto'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        protected bool GenerarIntervalosBarras()
        {
            try
            {
                //3 seleccionar segundo pto
                for (int j = 0; j < _listaptoTramo.Count - 1; j++)
                {
                    SelecionarPtoHorizontal selecionarPtoSup = M4_SeleccionarSegundoPtoHorizontalTramo(_listaptoTramo[j], _listaptoTramo[j + 1]);
                    if (selecionarPtoSup == null) return false;
                    //if (selecionarPtoSup.VerificarPtos()) return;

                    XYZ _PtoInicioSobrePLanodelViga_aux = _vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;

                    AsignarTipoTraslapo _AsignarTipoTraslapo = new AsignarTipoTraslapo(_uiapp, _configuracionInicialBarraHorizontalDTO, _ubicacionTraslapo, _listaptoTramo);
                    if (!_AsignarTipoTraslapo.M3_AsignarTipoTraslapo(j)) continue;
                    _configuracionInicialBarraHorizontalDTO._empotramientoPatasDTO = _AsignarTipoTraslapo._empotramientoPatasDTO;


                    _vigaSeleccionadoDTO.soloTag1 = true;
                    for (int i = 0; i < _configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras.Length; i++)
                    {
                        _configuracionInicialBarraHorizontalDTO.LineaBarraAnalizada = i + 1;


                        M3_AsignarCAntidadEspaciemientoNuevaLineaBarra(i, (VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras ? 0 : j), _vigaSeleccionadoDTO.TipoElementoSeleccionado);

                        IGenerarIntervalosH igenerarIntervalos =
                            FactoryGenerarIntervalos.CrearGeneradorDeIntervalosH(_uiapp, _configuracionInicialBarraHorizontalDTO, selecionarPtoSup, _vigaSeleccionadoDTO);
                        igenerarIntervalos.M1_ObtenerIntervaloBarrasDTO();
                        igenerarIntervalos.M2_GenerarListaBarraHorizontal();
                        var aux_listaDebarra = igenerarIntervalos.ListaIbarraHorizontal;

                        if (igenerarIntervalos.ListaIntervaloBarrasDTO.Count == 0) continue;

                        _listaDebarra.AddRange(aux_listaDebarra);

                        _listaDimensiones.Add(new PtoDimesionesTraslapoDTO()
                        {
                            P1 = igenerarIntervalos.ListaIntervaloBarrasDTO[0].ptoini,
                            P2 = igenerarIntervalos.ListaIntervaloBarrasDTO[0].ptofinal
                        });
                        _vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = _PtoInicioSobrePLanodelViga_aux;
                        _vigaSeleccionadoDTO.soloTag1 = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'GenerarIntervalosBarras'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        protected bool GenerarIntervalosBarras_paraREfurzo()
        {
            try
            {
                _seleccionarElementos = new SeleccionarElementosH();
                _seleccionarElementos.PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = _vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;
                //3 seleccionar segundo pto
                SelecionarPtoHorizontal selecionarPtoSup = M4_SeleccionarSegundoPtoHorizontalTramo(_listaptoTramo[0], _listaptoTramo[1]);
                if (selecionarPtoSup == null) return false;
                //if (selecionarPtoSup.VerificarPtos()) return;

                XYZ _PtoInicioSobrePLanodelViga_aux = _vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;

                AsignarTipoTraslapo _AsignarTipoTraslapo = new AsignarTipoTraslapo(_uiapp, _configuracionInicialBarraHorizontalDTO, _ubicacionTraslapo, _listaptoTramo);
                if (!_AsignarTipoTraslapo.M3_AsignarTipoTraslapo(0)) return false; ;

                _configuracionInicialBarraHorizontalDTO._empotramientoPatasDTO = _AsignarTipoTraslapo._empotramientoPatasDTO;


                _vigaSeleccionadoDTO.soloTag1 = true;

                int lineaAnalizada = _configuracionInicialBarraHorizontalDTO.LineaBarraAnalizada-1;

                _configuracionInicialBarraHorizontalDTO.NuevaLineaCantidadbarra = _configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras[0];
                _configuracionInicialBarraHorizontalDTO.EspaciamientoREspectoBordeFoot += (Util.CmToFoot(_configuracionInicialBarraHorizontalDTO.IntervalosEspaciamiento[0] * lineaAnalizada)
                                                                                        - Util.MmToFoot(_configuracionInicialBarraHorizontalDTO.incial_diametroMM) + ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT);
                _configuracionInicialBarraHorizontalDTO.NumeroBarraLinea = _configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras[0];

                // M3_AsignarCAntidadEspaciemientoNuevaLineaBarra(lineaAnalizada, 0, _vigaSeleccionadoDTO.TipoElementoSeleccionado);
           
                IGenerarIntervalosH igenerarIntervalos =
                    FactoryGenerarIntervalos.CrearGeneradorDeIntervalosH_RefuerzoVIga(_uiapp, _configuracionInicialBarraHorizontalDTO, selecionarPtoSup, _vigaSeleccionadoDTO);
                igenerarIntervalos.M1_ObtenerIntervaloBarrasDTO();
                igenerarIntervalos.M2_GenerarListaBarraHorizontal();
                var aux_listaDebarra = igenerarIntervalos.ListaIbarraHorizontal;
                _listaDebarra.AddRange(aux_listaDebarra);

                _listaDimensiones.Add(new PtoDimesionesTraslapoDTO()
                {
                    P1 = igenerarIntervalos.ListaIntervaloBarrasDTO[0].ptoini,
                    P2 = igenerarIntervalos.ListaIntervaloBarrasDTO[0].ptofinal
                });
                _vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = _PtoInicioSobrePLanodelViga_aux;
                _vigaSeleccionadoDTO.soloTag1 = false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'GenerarIntervalosBarras_paraREfurzo'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        protected bool M1_CalculosIniciales(int cantidadNivelesMIn)
        {
            try
            {
                if (!(_uidoc.Document.ActiveView is ViewSection))
                {
                    const string _instructions = "Comando debe ejecutarse en una vista VIEW SECTION";
                    Util.ErrorMsg(_instructions);
                    return false;
                }

                _configuracionInicialBarraHorizontalDTO.M1_ObtenerIntervalosDireccionMuro();
                // _listaLevelTotal = this._seleccionarNivel.ObtenerListaNivelOrdenadoPorElevacion(_view);  //no se esta usando. se desactiva.
                if (_configuracionInicialBarraHorizontalDTO.IntervalosEspaciamiento.Length > 1)
                {
                    IsdibujarDimension = false;
                    IsdibujarTraslapo = false;
                }
                // if (_listaLevelTotal.Count < cantidadNivelesMIn) return false;
                if (_configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras.Length == 0) return false;
                if (_configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras.Length == 0) return false;

                if (_configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras.Length != _configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras.Length) return false;



                View3D _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);
                if (_view3D_paraVisualizar == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial: 3D Visualizar");
                    return false;
                }
                View3D _view3D = TiposFamilia3D.Get3DBuscar(_doc);
                if (_view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial: 3D Buscar");
                    return false;
                }
                _configuracionInicialBarraHorizontalDTO.view3D_paraBuscar = _view3D;
                _configuracionInicialBarraHorizontalDTO.view3D_paraVisualizar = _view3D_paraVisualizar;
                _configuracionInicialBarraHorizontalDTO.viewActual = _uidoc.Document.ActiveView;

                AyudaManejadorTraslapo.Reset();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error En los calculos iniciales: {ex.Message}");
                return false;
            }
            return true;
        }

        protected void M3_AsignarCAntidadEspaciemientoNuevaLineaBarra(int i, int j, ElementoSeleccionado tipoelemto)
        {
            _configuracionInicialBarraHorizontalDTO.NuevaLineaCantidadbarra = _configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras[i];
            if (i == 0 && tipoelemto != ElementoSeleccionado.Barra)
                _configuracionInicialBarraHorizontalDTO.EspaciamientoREspectoBordeFoot = (_vigaSeleccionadoDTO.IsFundacion == true ? ConstNH.RECUBRIMIENTO_FUNDACIONES_foot : ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT)
                                                                                         + (j % 2 == 0 ? 0 : Util.CmToFoot(1));
            else
                _configuracionInicialBarraHorizontalDTO.EspaciamientoREspectoBordeFoot += (Util.CmToFoot(_configuracionInicialBarraHorizontalDTO.IntervalosEspaciamiento[i]) - Util.MmToFoot(_configuracionInicialBarraHorizontalDTO.incial_diametroMM));

            _configuracionInicialBarraHorizontalDTO.NumeroBarraLinea = _configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras[i];
        }

        public SelecionarPtoHorizontal M4_SeleccionarSegundoPtoHorizontalTramo(XYZ _PtoInicioIntervaloBarra, XYZ _PtoFinalIntervaloBarra)
        {

            SelecionarPtoHorizontal selecionarPtoHorizontal = new SelecionarPtoHorizontal(_uiapp, _configuracionInicialBarraHorizontalDTO, _PtoInicioIntervaloBarra, _PtoFinalIntervaloBarra);
            selecionarPtoHorizontal.MoverPtosSobrePLanoCaraViga(_seleccionarElementos);
            if (selecionarPtoHorizontal.IsConError)
            {
                Util.ErrorMsg($"Error Al seleccionar el punto superior n° {_configuracionInicialBarraHorizontalDTO.LineaBarraAnalizada}");
                return null;
            }
            return selecionarPtoHorizontal;
        }

        protected void M6_DibujarBarras(ConfiguracionTAgBarraDTo confBarraTagSindirectriz)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CrearBArraH");
                    int i = 0;
                    foreach (IbarraBase item in _listaDebarra)
                    {
                        _manejadorTraslapo.AgregarbarrasInicialTraslpoa();

                        if (!item.M1_DibujarBarra()) continue;
                        if (item.IsSoloTag)
                        {
                            item.M2_DibujarTags(confBarraTagSindirectriz);
                        }


                        IbarraBaseResultDTO _resutItem = item.GetResult();
                        ElementId idreabar = _resutItem._rebar.Id;// item.M3_ObtenerIdRebar();
                        if (idreabar != null)
                        {
                            _listaRebar.Add(_resutItem._rebar);
                            if (IsdibujarTraslapo)
                                _manejadorTraslapo.AgregarbarrasFinalTraslpoa(i, _resutItem._rebar);
                        }
                        else
                            _manejadorTraslapo.Reset();
                        i = 1 + i;
                    }
                    if (IsdibujarDimension)
                    {
                        DibujarDimensiones _DibujarDimensiones = new DibujarDimensiones(_uiapp, _listaDimensiones, _vigaSeleccionadoDTO.DireccionEnFierrado);
                        _DibujarDimensiones.Ejecutar();
                    }
                    t.Commit();
                }

                if (IsdibujarTraslapo)
                    _manejadorTraslapo.M4_DibujarTraslapoConTrans();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }




    }
}
