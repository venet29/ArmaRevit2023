using ArmaduraLosaRevit.Model.BarraAreaPath;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo;
using ArmaduraLosaRevit.Model.BarraMallaRebar;
using ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.Renombrar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Prueba.User;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Viewnh;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico
{
    public class ManejadorBarrasAutomaticas
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private View _view;


        private Ui_AutoElev ui_Barrav;
        private readonly string _rutaGuardarArchivos;
        Stopwatch timeMeasure = new Stopwatch();
        private View3D _view3D_buscar;

        public bool isDibujarBarras { get; private set; }

        private bool isDibujarMalla;
        private bool isDibujarConfin;
        private bool isDibujarEstriboMuro;
        private int CantidadBArrasDibujadas;
        private int CantidadMallas;
        private int CantidadEstribo;
        private int CantidadConfina;


        private int MultipleCantidadBArrasDibujadas;
        private int MultipleCantidadMallas;
        private int MultipleCantidadEstribo;
        private int MultipleCantidadConfina;
        private UtilStopWatch _utilStopWatch;

        public ManejadorBarrasAutomaticas(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            //UtilBarras.IsConNotificaciones = false;

            isDibujarBarras = true;
            isDibujarMalla = true;
            isDibujarConfin = true;
            isDibujarEstriboMuro = true;
        }

        public ManejadorBarrasAutomaticas(UIApplication uiapp, ref Ui_AutoElev ui_Barrav, string rutaGuardarArchivos = "") : this(uiapp)
        {
            this.ui_Barrav = ui_Barrav;
            this._rutaGuardarArchivos = rutaGuardarArchivos;
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            // UtilBarras.IsConNotificaciones = false;

            if (ui_Barrav.BotonOprimido == "Ejecutar")
            {

                isDibujarBarras = (bool)ui_Barrav.itemBarra.IsChecked; ;
                isDibujarMalla = (bool)ui_Barrav.itemMalla.IsChecked; ;
                isDibujarEstriboMuro = (bool)ui_Barrav.itemEstribo.IsChecked;
                isDibujarConfin = (bool)ui_Barrav.itemConf.IsChecked; ;
            }
            else//"Ejecutar_variasM"
            {

                isDibujarBarras = (bool)ui_Barrav.itemBarraM.IsChecked; ;
                isDibujarMalla = (bool)ui_Barrav.itemMallaM.IsChecked; ;
                isDibujarEstriboMuro = (bool)ui_Barrav.itemEstriboM.IsChecked;
                isDibujarConfin = (bool)ui_Barrav.itemConfM.IsChecked; ;
            }
        }

        public bool EjecutarImportacion()
        {
            timeMeasure.Start();
            if (_view.ViewType != ViewType.Section || (!Directory.Exists(ConstNH.CONST_COT)))
            {
                Util.ErrorMsg("Comando Se debe ejecutar en una SeccionView");
                return false;
            }

            #region Validar

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR EjecutarImportacion automatico ");

            if (!resultadoConexion)
            {
                Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                return false;
            }
            else if (!_ManejadorUsuarios.ObteneResultado())
            {
                Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                return false;
            }


            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return false;

            #endregion

            CreadorView _CreadorView = new CreadorView(_uiapp);
            _CreadorView.M2_ConfiguracionElevacionDibujar(_view);

            bool IniciaIsConNotificaciones = UtilBarras.IsConNotificaciones;
            UtilBarras.IsConNotificaciones = false;
            CantidadBArrasDibujadas = 0;
            CantidadMallas = 0;
            CantidadEstribo = 0;
            CantidadConfina = 0;

            try
            {
                //** para denombrar familia pathsymbol 13-04-2022
                ManejadorReNombrarFamPathSymbol _ManejadorReNombrar = new ManejadorReNombrarFamPathSymbol(_uiapp);
                if (_ManejadorReNombrar.IsFamiliasAntiguas())
                    _ManejadorReNombrar.Renombarra();
                //**

                if (!ConfuguracionViews()) return false;

                List<IntervalosBarraAutoDtoIMPORTAR> ListaIntervalosBarraAutoDtoIMPORTAR = CreadoJson.LeerArchivoJsonING();
                if (ListaIntervalosBarraAutoDtoIMPORTAR.Count == 0) return true;

                _utilStopWatch = new UtilStopWatch();
                DibujarBarras(ListaIntervalosBarraAutoDtoIMPORTAR);
                _utilStopWatch.TerminarYGuardarTxTTiempoTotal("EnfierraAutomatio");

            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error EjecutarImportacion: " + ex.Message);
                return false;
            }
            UtilBarras.IsConNotificaciones = IniciaIsConNotificaciones;



            CloseViews();
            TimeSpan ts = timeMeasure.Elapsed;
            timeMeasure.Stop();
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            System.Windows.Forms.MessageBox.Show($"Finaliza la Importacion y dibujo de barras Elevacion \n Tiempo Desarrollo: {elapsedTime}\nElementos dibujados:" +
                $"\n_ BArras:{CantidadBArrasDibujadas} \n_ MAllas:{CantidadMallas} (creada con 4 rebar cada una, total:{CantidadMallas * 4}) \n_ Estribo:{CantidadEstribo} " +
                $"\n_ Confi:{CantidadConfina}\n Total Barras Creadas:{CantidadBArrasDibujadas + CantidadMallas * 4 + CantidadEstribo + CantidadConfina} ", "Confirmation");
            // Util.InfoMsg($"Importacion de barras terminado. Tiempo {_utilStopWatch.}");
            return true;
        }
        public bool EjecutarVariasImportacion(List<VariasElevAutoDTO> lista)
        {
            timeMeasure.Start();
            if (_view.ViewType != ViewType.Section || (!Directory.Exists(ConstNH.CONST_COT)))
            {
                Util.ErrorMsg("Comando Se debe ejecutar en una SeccionView");
                return false;
            }

            #region Validar

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR EjecutarImportacion automatico ");

            if (!resultadoConexion)
            {
                Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                return false;
            }
            else if (!_ManejadorUsuarios.ObteneResultado())
            {
                Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                return false;
            }


            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return false;

            #endregion

            CreadorView _CreadorView = new CreadorView(_uiapp);
            _CreadorView.M2_ConfiguracionElevacionDibujar(_view);

            bool IniciaIsConNotificaciones = UtilBarras.IsConNotificaciones;
            UtilBarras.IsConNotificaciones = false;

            MultipleCantidadBArrasDibujadas = 0;
            MultipleCantidadMallas = 0;
            MultipleCantidadEstribo = 0;
            MultipleCantidadConfina = 0;
            UtilStopWatch _utilStopWatchGenetral = new UtilStopWatch();
            try
            {
                //** para denombrar familia pathsymbol 13-04-2022
                ManejadorReNombrarFamPathSymbol _ManejadorReNombrar = new ManejadorReNombrarFamPathSymbol(_uiapp);
                if (_ManejadorReNombrar.IsFamiliasAntiguas())
                    _ManejadorReNombrar.Renombarra();
                //**

                string NombreReporte = @"\reporte_" + DateTime.Now.ToString("MM_dd_yyyy Hmmss");

                LogNH.guardar_registro_txt($"Cargando archivo. Se analizan {lista.Count} vistas", _rutaGuardarArchivos, NombreReporte);
                _utilStopWatch = new UtilStopWatch();
                _utilStopWatchGenetral.StopYContinuar($"INICIANDO  ");
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CargarArmaduraAutoVariasElevaciones-NH");
                    int cont = 0;

                    foreach (VariasElevAutoDTO item in lista)
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        TimeSpan Auxts = timeMeasure.Elapsed;
                        string AuxelapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Auxts.Hours, Auxts.Minutes, Auxts.Seconds, Auxts.Milliseconds / 10);
                        LogNH.guardar_registro_txt($"cargando Elevacion{cont} de {lista.Count}, con nombre:{item.ViewSection.Name}   tiempo:{AuxelapsedTime}", _rutaGuardarArchivos, NombreReporte);
                        CantidadBArrasDibujadas = 0;
                        CantidadMallas = 0;
                        CantidadEstribo = 0;
                        CantidadConfina = 0;
                        List<IntervalosBarraAutoDtoIMPORTAR> ListaIntervalosBarraAutoDtoIMPORTAR = CreadoJson.LeerArchivoJsonING_conruta(item.NombreFileInfo.FullName);
                        if (ListaIntervalosBarraAutoDtoIMPORTAR.Count == 0)
                        {
                            _utilStopWatchGenetral.StopYContinuar($"{cont}) Elevacion {item.NombreView} cargada con ERROR");
                            continue;
                        }
                        _view = item.ViewSection;
                        //  _uiapp.ActiveUIDocument.ActiveView = _view;
                        if (!ConfuguracionViews()) continue;
                        if (DibujarBarras(ListaIntervalosBarraAutoDtoIMPORTAR, item.ViewSection))
                        {
                            MultipleCantidadBArrasDibujadas += CantidadBArrasDibujadas;
                            MultipleCantidadMallas += CantidadMallas;
                            MultipleCantidadEstribo += CantidadEstribo;
                            MultipleCantidadConfina += CantidadConfina;
                            _utilStopWatchGenetral.StopYContinuar($"{cont}) Elevacion {item.NombreView} cargada CORRECTAMENTE. Barras:{CantidadBArrasDibujadas} malla:{CantidadMallas * 4} confinamiento:{CantidadConfina}  Estribo:{CantidadEstribo}");
                        }
                        else
                            _utilStopWatchGenetral.StopYContinuar($"{cont}) Elevacion {item.NombreView} cargada con ERROR");
                        cont += 1;
                    }
                    t.Assimilate();
                }
                _utilStopWatch.TerminarYGuardarTxTTiempoTotal("EnfierraAutomatio", _rutaGuardarArchivos);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error EjecutarImportacion: " + ex.Message);
                return false;
            }
            UtilBarras.IsConNotificaciones = IniciaIsConNotificaciones;
            CloseViews();

            TimeSpan ts = timeMeasure.Elapsed;
            timeMeasure.Stop();
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            string mensajeFinal = $"Finaliza la Importacion y dibujo de barras Elevacion \n Tiempo Desarrollo: {elapsedTime}\nElementos dibujados:" +
                $"\n_ BArras:{MultipleCantidadBArrasDibujadas} \n_ MAllas:{MultipleCantidadMallas} (creada con 4 rebar cada una, total:{MultipleCantidadMallas * 4}) \n_ Estribo:{MultipleCantidadEstribo} " +
                $"\n_ Confi:{MultipleCantidadConfina}\n Total Barras Creadas:{MultipleCantidadBArrasDibujadas + MultipleCantidadMallas * 4 + MultipleCantidadEstribo + MultipleCantidadConfina} ";

            _utilStopWatchGenetral.StopYContinuar(mensajeFinal);
            LogNH.Limpiar_sbuilder();
            _utilStopWatchGenetral.TerminarYGuardarTxTTiempoTotal("EnfierraAutomatio_general", _rutaGuardarArchivos);
            System.Windows.Forms.MessageBox.Show(mensajeFinal, "Confirmation");
            // Util.InfoMsg($"Importacion de barras terminado. Tiempo {_utilStopWatch.}");
            return true;
        }



        private bool DibujarBarras(List<IntervalosBarraAutoDtoIMPORTAR> ListaIntervalosBarraAutoDtoIMPORTAR, View _viewDefu = null)
        {

            _utilStopWatch.IniciarMedicion();

            if (_viewDefu != null) _view = _viewDefu;

          //  if (ui_Barrav != null) ui_Barrav.Dispatcher.Invoke(() => ui_Barrav.TbDebug.Text = ui_Barrav.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " -> 1)creadno Datos "));

            CreadorListaIntervalosBarraAutoDto creadorListaIntervalosBarraAutoDto;
            try
            {
              //  if (ui_Barrav != null) ui_Barrav.Dispatcher.Invoke(() => ui_Barrav.TbDebug.Text = ui_Barrav.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " -> 2) obtener datos revit "));
                M12_MOverHacia(MOverSection.Avanzar);

                double deltaElevacionBasePoint = AyudaObtenerDeltaElevacion.ObtenerDeltaElevation(_uiapp);
                ArmaduraTrasformada ArmaduraTrasformada = new ArmaduraTrasformada(_view, deltaElevacionBasePoint);

                creadorListaIntervalosBarraAutoDto = new CreadorListaIntervalosBarraAutoDto(_doc,ListaIntervalosBarraAutoDtoIMPORTAR, ArmaduraTrasformada);
                creadorListaIntervalosBarraAutoDto.Ejecutar();

                M12_MOverHacia(MOverSection.Retroceder);

              //  if (ui_Barrav != null) ui_Barrav.Dispatcher.Invoke(() => ui_Barrav.TbDebug.Text = ui_Barrav.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " -> 3)Homologando con revit "));
            }
            catch (Exception ex)
            {
                M12_MOverHacia(MOverSection.Retroceder);
                Util.ErrorMsg("Error EjecutarImportacion: " + ex.Message);
                return false;
            }


            ManejadorBarraV_auto _manejadorBarraV_auto = null;

            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("EjecutarImportacion-NH");
                    if (isDibujarBarras)
                    {
                        //a)lista de barras verticales        
                        ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                        ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO = new ConfiguracionIniciaWPFlBarraVerticalDTO()
                        {
                            Inicial_Cantidadbarra = "3",
                            incial_ComoIniciarTraslapo_LineaPAr = 1,
                            incial_ComoIniciarTraslapo_LineaImpar = 2,//barra incio
                            inicial_ComoTraslapo = 2,
                            inicial_diametroMM = 22,
                            Document_ = _uiapp.ActiveUIDocument.Document,
                            inicial_tipoBarraV = Enumeraciones.TipoPataBarra.BarraVSinPatas,
                            CasoAnalisasBarrasElevacion_ = CasoAnalisasBarrasElevacion.Automatico,
                            inicial_IsDirectriz = false,
                            inicial_ISIntercalar = false,
                            Inicial_espacienmietoCm_EntreLineasBarras = "15",
                            ListaIntervaloBarraAutoDto = creadorListaIntervalosBarraAutoDto.ListaIntervalosBarraAutoDto,
                            IsDibujarTag = true,
                            TipoBarraRebar_ = TipoBarraVertical.Cabeza,
                            TipoBarraRebarHorizontal_ = TipoBarraVertical.MallaH,
                            BarraTipo = TipoRebar.ELEV_BA_V,
                            TipoSelecion = TipoSeleccion.ConMouse //se ocupa esta seleccio pq es la que siempre se utilizo
                        };

                        DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(_uiapp.ActiveUIDocument.ActiveView, DireccionRecorrido_.PerpendicularEntradoVista);


                      //  if (ui_Barrav != null) ui_Barrav.Dispatcher.Invoke(() => ui_Barrav.TbDebug.Text = ui_Barrav.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " -> 4) Dibujando barras Verticales "));
                        _utilStopWatch.StopYContinuar("0) inicio enfierra");
                        _manejadorBarraV_auto = new ManejadorBarraV_auto(_uiapp, _seleccionarNivel, confiEnfierradoDTO, _DireccionRecorrido);
                        _manejadorBarraV_auto.CrearBArraVErticalv2(IsDibujarTagBArras: false);
                        CantidadBArrasDibujadas += _manejadorBarraV_auto.CantidadBArrasDibujadas;
                        _utilStopWatch.StopYContinuar("1) Fin barra");
                      //  if (ui_Barrav != null) ui_Barrav.Dispatcher.Invoke(() => ui_Barrav.TbDebug.Text = ui_Barrav.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " -> 5) Dibujando malla"));

                    }

                    if (isDibujarMalla)
                    {
                        //b) lista malla

                        List<IntervalosMallaDTOAuto> ListIntervalosMallaDTOAuto = new List<IntervalosMallaDTOAuto>();
                        bool EntrarCOnBarra = true;
                        if (EntrarCOnBarra) //diseño con rebar
                        {

                            ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                            ManejadorBarraVMallaAuto _ManejadorBarraVMallaAuto = new ManejadorBarraVMallaAuto(_uiapp, creadorListaIntervalosBarraAutoDto.ListaIntervalosMAllaAutoDto, _seleccionarNivel);
                            _ManejadorBarraVMallaAuto.CrearBArra();
                            CantidadMallas += _ManejadorBarraVMallaAuto.CantidadMallasDibujadas;
                        }
                        else // diseño con areapath
                        {
                            ManejadorMallaMuroAuto manejadorMallaMuro = new ManejadorMallaMuroAuto(_uiapp, creadorListaIntervalosBarraAutoDto.ListaIntervalosMAllaAutoDto);
                            manejadorMallaMuro.CrearMallaMuro();
                        }
                        _utilStopWatch.StopYContinuar("2) Fin Mallas");

                    }

                    if (isDibujarConfin)
                    {
                       // if (ui_Barrav != null) ui_Barrav.Dispatcher.Invoke(() => ui_Barrav.TbDebug.Text = ui_Barrav.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " -> 6) Dibujando confi "));
                        //C)CONF

                        ManejadorEstriboCOnfinamientoAuto manejadorEstriboCOnfinamientoAuto = new ManejadorEstriboCOnfinamientoAuto(_uiapp, null, creadorListaIntervalosBarraAutoDto.ListaIntervalosConfinaminetoAutoDto);
                        manejadorEstriboCOnfinamientoAuto.CrearConfinaminetoMuro();
                        CantidadConfina += manejadorEstriboCOnfinamientoAuto.CantidadDibujadas;
                        _utilStopWatch.StopYContinuar("3) Fin confinamiento");
                    }

                    if (isDibujarEstriboMuro)
                    {
                       // if (ui_Barrav != null) ui_Barrav.Dispatcher.Invoke(() => ui_Barrav.TbDebug.Text = ui_Barrav.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " -> 7) Dibujando barras Estribo "));
                        //D)  ESTRIBO MURO
                        CantidadEstribo = creadorListaIntervalosBarraAutoDto.ListaIntervalosEstriboMuroAutoDto.Count;
                        ManejadorEstriboCOnfinamientoAuto manejadorEstriboMuroAuto = new ManejadorEstriboCOnfinamientoAuto(_uiapp, null, creadorListaIntervalosBarraAutoDto.ListaIntervalosEstriboMuroAutoDto);
                        manejadorEstriboMuroAuto.CrearEstriboMuro();
                        CantidadEstribo += manejadorEstriboMuroAuto.CantidadDibujadas;
                        _utilStopWatch.StopYContinuar("4) Fin estribo");
                    }
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }

            //NOTA:se deja el dibujar los taslapos fuera de la transaccion grupal pq presentaba problema al obtener referenci de la barrar
            if (VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras && _manejadorBarraV_auto!=null)
            {
                try
                {
                    using (Transaction t = new Transaction(_doc))
                    {
                        t.Start("dibujar Traslapo-NH");
                        foreach (var _manejadorTraslapo in _manejadorBarraV_auto.ListaTraslapo)
                        {
                            _manejadorTraslapo.M4_DibujarTraslapoSinTrans();
                        }
                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
                }
            }
            //fin denota
            _utilStopWatch.StopYContinuar("5) Fin DImnensiones");

            return true;
        }

        private bool ConfuguracionViews()
        {
            try
            {
                CreadorView _creadorView = new CreadorView(_uiapp);
                _creadorView.CambiarDetalle(_view, ViewDetailLevel.Coarse, DisplayStyle.Wireframe);

                _view3D_buscar = TiposFamilia3D.Get3DBuscar(_doc);
                if (_view3D_buscar == null) return false;

                string activeView = _view.Name;


                if (_view.Pinned)
                {
                    Util.ErrorMsg($"Error en configuracion View. View con pin");
                    return false;
                }



                _uiapp.ActiveUIDocument.ActiveView = _view3D_buscar;
                ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(_uiapp);
                _ManejadorVisibilidadElemenNoEnView.Ejecutar(_view3D_buscar, _view.Name);

                _uiapp.ActiveUIDocument.ActiveView = _view;

                using (Transaction tr = new Transaction(_doc, "Regenerate_nh"))
                {

                    tr.Start();
                    _doc.Regenerate();
                    _uiapp.ActiveUIDocument.RefreshActiveView();
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en configuracion View   ex:{ex}");
                return false;
            }
            return true;
        }
        private bool CloseViews()
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "CS-NH"))
                {

                    tr.Start();

                    var pCurrView = _view3D_buscar;
                    // _uiapp.ActiveUIDocument.RequestViewChange(pCurrView);
                    var lViews = _uiapp.ActiveUIDocument.GetOpenUIViews();
                    foreach (var pView in lViews)
                    {
                        if (pView.ViewId == pCurrView.Id)
                            pView.Close();
                    }
                    tr.Commit();

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en configuracion View   ex:{ex}");
                return false;
            }
            return true;
        }
        public void M12_MOverHacia(MOverSection _enu)
        {
            Parameter p2 = _view.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR);
            double mitad = p2.AsDouble() / 2;

            Element ss = _doc.GetElement(new ElementId(_view.Id.IntegerValue - 1)) as Element;


            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CMOver barra-NH");
                    ElementTransformUtils.MoveElement(_doc, ss.Id, (_enu == MOverSection.Avanzar ? -1 : 1) * _view.ViewDirection * mitad);
                    //ElementTransformUtils.MoveElement(_doc, ss.Id, ss.ViewDirection* p2.AsDouble());
                    t.Commit();
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puede mover barra");
            }
        }
    }
}
