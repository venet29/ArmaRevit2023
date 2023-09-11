using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ConfiguracionInicial;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.LinkNh;
using ArmaduraLosaRevit.Model.Pasadas.Ayuda;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using ArmaduraLosaRevit.Model.Pasadas.Servicio;
using ArmaduraLosaRevit.Model.Pin;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Viewnh.posicion;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Pasadas.WPFPasada
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_WPFPasada
    {

        public static void EjecutarPAsadas_wpf(UI_Pasadas _ui, UIApplication _uiapp)
        {

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR CrearBArraVErtical ");

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

            UtilStopWatch _utilStopWatch = new UtilStopWatch();
            // if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);


            var _uidoc = _uiapp.ActiveUIDocument;
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            Document _doc = _uiapp.ActiveUIDocument.Document;

            View3D elem3d = _view as View3D;
            if (null == elem3d)
            {
                Util.ErrorMsg("Debe ejecutar comando en un View3D");
                return;
            }

            UpdateGeneral.M3_DesCargarBarras(_uiapp);
            try
            {
                List<EnvoltorioBase> ListaEnvoltorioPipes_aux = new List<EnvoltorioBase>();
                List<EnvoltorioBase> ListaEnvoltorioPipes_SOLOAgregados_aux = new List<EnvoltorioBase>();


                List<View> lista_view = new List<View>() { _uiapp.ActiveUIDocument.ActiveView };
                List<string> ListaEjes = new List<string>();
                string _BotonOprimido = _ui.BotonOprimido;
                _ui.Nombre3D = _ui.SelectView3d;// "{3D - delporteing@gmail.com}";

                //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
                DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);

                ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                configuracionInicial.AgregarParametrosSharePasadas();

                _ui.Hide();

                if (_BotonOprimido == "Crear_TodasPasadas")
                {
                    UtilBarras.IsConNotificacionesOff();
                    LogNH.Limpiar_sbuilder();
                    LogNH.Agregar_registro("Inicio Crear_TodasPasadas_" + Util.InfoFechaHoraActual());

                    UtilitarioFallasDialogosCarga.Cargar(_uiapp);
                    _utilStopWatch.IniciarMedicion();
                    //cambiar estado pasadas
                    IVisibilidadView visibilidad = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_GenericModel, "Generic Models");
                    visibilidad.AsignarVisibilityBuiltInCategory(false);

                    BuscarPasadas_ConEspecialidades(_ui, _uiapp, elem3d);


                    if (!AyudaObtenerPasadas.ObtenerPasadas(_doc, _CreadorExtStore)) return;
                    var listaPasdas3D_ = AyudaObtenerPasadas.ObtenerListaPAsadas(_uiapp);

                    //B) si hay pasadas Rojas
                    if (AyudaObtenerPasadas.ListaPasdasElem_Rojo.Count > 0)
                    {
                        ServicioCrearOpenin _ServicionCrearOpenin_pasadasRojas = new ServicioCrearOpenin(_uiapp, _ui.ListaEnvoltorioMEP);
                        if (_ServicionCrearOpenin_pasadasRojas.M1_ObtenerInterseccion_PasadasRojas(_ui.Nombre3D, 0, _ui.ObtenerTIpoElementoIntersectar()))
                        {
                            //var listaPasdas3D_ = AyudaObtenerPasadas.ObtenerListaPAsadas(_uiapp);
                            _ServicionCrearOpenin_pasadasRojas.M3_DibujarSOLOPasadas_PasadasaRojas(listaPasdas3D_.Where(c => c.EstadoPasada == "").ToList());
                            //_ui.ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(_ServicionCrearOpenin.ListaEnvoltorioPipes);
                            //_ui.ListaEnvoltorioPipesTodos.AddRange(_ServicionCrearOpenin.ListaEnvoltorioPipes_SOLOAgregados);
                            ListaEnvoltorioPipes_aux.AddRange(_ServicionCrearOpenin_pasadasRojas.ListaEnvoltorioPipes);
                            ListaEnvoltorioPipes_SOLOAgregados_aux.AddRange(_ServicionCrearOpenin_pasadasRojas.ListaEnvoltorioPipes_SOLOAgregados);
                        }
                    }


                    //A)
                    ServicioCrearOpenin _ServicionCrearOpenin = new ServicioCrearOpenin(_uiapp, _ui.ListaEnvoltorioMEP);
                    if (_ServicionCrearOpenin.M1_ObtenerInterseccion(_ui.Nombre3D, 0, _ui.ObtenerTIpoElementoIntersectar()))
                    {
                        _ServicionCrearOpenin.M3_DibujarSOLOPasadas(listaPasdas3D_);
                        // CrearLIstaFiltros(_ui);
                        ListaEnvoltorioPipes_aux.AddRange(_ServicionCrearOpenin.ListaEnvoltorioPipes);
                        ListaEnvoltorioPipes_SOLOAgregados_aux.AddRange(_ServicionCrearOpenin.ListaEnvoltorioPipes_SOLOAgregados);

                        //_ui.ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(_ServicionCrearOpenin.ListaEnvoltorioPipes);
                        //_ui.ListaEnvoltorioPipesTodos.AddRange(_ServicionCrearOpenin.ListaEnvoltorioPipes_SOLOAgregados);
                    }



                    if (ListaEnvoltorioPipes_aux.Count > 0)
                        _ui.ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(ListaEnvoltorioPipes_aux);

                    if (ListaEnvoltorioPipes_SOLOAgregados_aux.Count > 0)
                        _ui.ListaEnvoltorioPipesTodos.AddRange(ListaEnvoltorioPipes_SOLOAgregados_aux);

                    CrearLIstaFiltros_PorEStado(_ui);

                    Util.InfoMsg($"Proceso terminado de CREACION de PASADAS. Se analizador {_ui.ListaEnvoltorioMEP.Count} elemtos de especialidades.\n Duracion comando: {_utilStopWatch.Terminar_formatoMje()}");
                    UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                    _ui.RecargarLIstaCOmpleta();

                    UtilBarras.IsConNotificacionesON();
                    LogNH.Guardar_registro("PAsadas_Opcion-Crear_TodasPasadas", Util.InfoRutaMisdocumentos());

                }
                else if (_BotonOprimido == "Revision_TodasPasadas")
                {
                    UtilBarras.IsConNotificacionesOff();
                    LogNH.Limpiar_sbuilder();
                    LogNH.Agregar_registro("Inicio Revision_TodasPasadas" + Util.InfoFechaHoraActual());

                    UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                    _utilStopWatch.IniciarMedicion();
                    //cambiar estado pasadas
                    IVisibilidadView visibilidad = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_GenericModel, "Generic Models");
                    visibilidad.AsignarVisibilityBuiltInCategory(false);

                    BuscarPasadas_ConEspecialidades(_ui, _uiapp, elem3d);

                    if (!AyudaObtenerPasadas.ObtenerPasadas(_doc, _CreadorExtStore)) return;

                    ServicioCrearOpenin _ServicionCrearOpenin = new ServicioCrearOpenin(_uiapp, _ui.ListaEnvoltorioMEP);
                    if (_ServicionCrearOpenin.M1_ObtenerInterseccion(_ui.Nombre3D, 0, _ui.ObtenerTIpoElementoIntersectar()))
                    {
                        var listaPasdas3D_ = AyudaObtenerPasadas.ObtenerListaPAsadas(_uiapp);
                        _ServicionCrearOpenin.M4_Revision_SOLOPasadas(listaPasdas3D_);
                        CrearLIstaFiltros_PorEStado(_ui);

                        _ui.ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(_ServicionCrearOpenin.ListaEnvoltorioPipes);
                        _ui.ListaEnvoltorioPipesTodos.AddRange(_ServicionCrearOpenin.ListaEnvoltorioPipes_SOLOAgregados);
                    }


                    Util.InfoMsg($"Proceso Terminado de REVISION de PASADAS. Se analizador {_ui.ListaEnvoltorioMEP.Count} elemtos de especialidades.\n Duracion comando: {_utilStopWatch.Terminar_formatoMje()}");

                    UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                    _ui.RecargarLIstaCOmpleta();

                    UtilBarras.IsConNotificacionesON();
                    LogNH.Guardar_registro("PAsadas_Opcion-Revision_TodasPasadas", Util.InfoRutaMisdocumentos());
                }

                else if (_BotonOprimido == "Crear_TodasShaft")
                {
                    UtilBarras.IsConNotificacionesOff();
                    LogNH.Limpiar_sbuilder();
                    LogNH.Agregar_registro("Inicio Crear_TodasShaft");

                    _utilStopWatch.IniciarMedicion();
                    UtilitarioFallasDialogosCarga.Cargar(_uiapp);
                    // agregar opcion que verifiue no no falta opciones por revisar
                    bool continuar = true;

                    if (!AyudaObtenerPasadas.ObtenerPasadas(_doc, _CreadorExtStore)) return;
                    UtilBarras.IsConNotificacionesON();
                    if (AyudaObtenerPasadas.ListaPasdasElem_Naranjo.Count > 0)
                    {
                        Util.ErrorMsg($"Se encontraron {AyudaObtenerPasadas.ListaPasdasElem_Naranjo.Count} pasadas NARANJO sin VALIDAR. Valide y vuelva a ejecutar comando.");
                        continuar = false;
                    }
                    if (AyudaObtenerPasadas.ListaPasdasElem_Azul.Count > 0)
                    {
                        Util.ErrorMsg($"Se encontraron {AyudaObtenerPasadas.ListaPasdasElem_Azul.Count} pasadas AZULES sin REVISAR. Revisar y vuelva a ejecutar comando.");
                        continuar = false;
                    }
                    if (AyudaObtenerPasadas.ListaPasdasElem_Rojo.Count > 0)
                    {
                        if (AyudaObtenerPasadas.ListaPasdasElem_Verde.Count > 0)
                        {
                            var result = Util.InfoMsg_YesNo($"Se encontraron {AyudaObtenerPasadas.ListaPasdasElem_Rojo.Count} pasadas ROJAS Creadas. Puede que losa Shaft ya esten creados.\n\n " +
                                $" NOTA: Se encontraron {AyudaObtenerPasadas.ListaPasdasElem_Verde.Count} pasadas VERDE Creadas. Puedo ocurrir, que al crear los Shaft previamente," +
                                $" el comando genero algun error quedando la rutina inconclusa.\n Desea Continuar");


                            if (result == System.Windows.Forms.DialogResult.No)
                                continuar = false;
                            else
                                continuar = true;
                        }
                        else
                        {
                            Util.ErrorMsg($"Se encontraron {AyudaObtenerPasadas.ListaPasdasElem_Rojo.Count} pasadas ROJAS Creadas. Puede que losa Shaft ya esten creados.\n\n");
                            continuar = false;
                        }
                    }


                    UtilBarras.IsConNotificacionesOff();


                    if (AyudaObtenerPasadas.ListaPasdasElem_Verde.Count == 0 || _ui.ListaEnvoltorioPipesTodos.Count == 0)
                    {
                        Util.InfoMsg("No se encontron pasadas para generar Shaft.");
                        continuar = false;
                    }

                    ServicioCrearOpenin _ServicionCrearOpenin = new ServicioCrearOpenin(_uiapp, _ui.ListaEnvoltorioMEP);
                    if (continuar && _ServicionCrearOpenin.M1_ObtenerInterseccion(_ui.Nombre3D, _ui.AreaMaxAutovalid_foot2, _ui.ObtenerTIpoElementoIntersectar()))
                    {
                        ServicioCambiarPasada _ServicioCambiarPasada = new ServicioCambiarPasada(_uiapp, _ui.ListaEnvoltorioPipesTodos);
                        _ServicioCambiarPasada.CambiarTodasPAsadaAROJO();

                        _ServicionCrearOpenin.DibujarSOLOShaft();
                        CambiarEstadoShaftConError(_ui.ListaEnvoltorioMEP);

                        _ServicioCambiarPasada.CambiarEstado_TodasPAsadaAROJO();

                        CrearLIstaFiltros_PorEStado(_ui);

                        Util.InfoMsg($"Proceso terminado de CREACION de SHAFT. Se analizador {_ui.ListaEnvoltorioMEP.Count} elemtos de especialidades.\n Duracion comando: {_utilStopWatch.Terminar_formatoMje()}");
                    }


                    UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                    _ui.RecargarLIstaCOmpleta();

                    UtilBarras.IsConNotificacionesON();
                    LogNH.Guardar_registro("PAsadas_Opcion-Crear_TodasShaft", Util.InfoRutaMisdocumentos());
                }
                else if (_BotonOprimido == "CrearIndividual")
                {
                    UtilBarras.IsConNotificacionesOff();
                    LogNH.Limpiar_sbuilder();
                    LogNH.Agregar_registro("Inicio Crear_TodasShaft");

                    _utilStopWatch.IniciarMedicion();
                    UtilitarioFallasDialogosCarga.Cargar(_uiapp);


                    if (false)
                        Util.InfoMsg("En desarrollo rutina dibujar shaft individual");
                    else
                    {
                        var pipe = _ui._EnvoltorioPipesSeleccionado;
                        ObservableCollection<EnvoltorioBase> listaEnvoltorioPipes2 = new ObservableCollection<EnvoltorioBase>();
                        listaEnvoltorioPipes2.Add(pipe);
                        ServicioCrearOpenin _ServicionCrearOpenin = new ServicioCrearOpenin(_uiapp, listaEnvoltorioPipes2);

                        if (_ServicionCrearOpenin.M1_ObtenerInterseccion(_ui.Nombre3D, 0, _ui.ObtenerTIpoElementoIntersectar()))
                        {
                            _ServicionCrearOpenin.M2_DibujarShaft_Pasadas("PASADA_RECTANGULAR", _ui.AreaMaxAutovalid_foot2);

                            ServicioCambiarPasada _ServicioCambiarPasada = new ServicioCambiarPasada(_uiapp, listaEnvoltorioPipes2.ToList());
                            _ServicioCambiarPasada.CambiarTodasPAsadaAROJO();                            
                            CambiarEstadoShaftConError(listaEnvoltorioPipes2);

                            //_ServicioCambiarPasada.CambiarEstado_TodasPAsadaAROJO();
                            CrearLIstaFiltros_PorEStado(_ui);
                        }
                    }

                    UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                    _ui.RecargarLIstaCOmpleta();

                    UtilBarras.IsConNotificacionesON();
                    LogNH.Guardar_registro("PAsadas_Opcion-Crear_TodasShaft", Util.InfoRutaMisdocumentos());
                }
                else if (_BotonOprimido == "Crear_BorrarShafopenin")
                {

                    var ListaBorrar = SeleccionarOpening.SeleccionarAll(_doc).ToList();

                    List<ElementId> ListaBorrarId = SeleccionarOpening.SeleccionarAll(_doc)
                                                                      .Where(c => _CreadorExtStore.M3_OBtenerResultado_String(c, "estado", "SubFieldTest21") != "")
                                                                      .Select(c => c.Id).ToList();
                    using (Transaction t = new Transaction(_doc))
                    {
                        t.Start("crearSection-NH");
                        _doc.Delete(ListaBorrarId);
                        t.Commit();
                    }
                    _ui.ResetarLIstas();

                    _ui.CantidadElement = 0;
                }
                else if (_BotonOprimido == "CambiarPasada")
                {

                    ServicioCambiarPasada _ServicioCambiarPasada = new ServicioCambiarPasada(_uiapp, _ui.ListaEnvoltorioPipesTodos);
                    _ServicioCambiarPasada.RevisarPAsada(_ui.TextBox_Revisar.Text);

                    CrearLIstaFiltros_PorEStado(_ui);
                    _ui.RecargarLIstaCOmpleta();
                }
                else if (_BotonOprimido == "RechazarPasada")
                {

                    ServicioCambiarPasada _ServicioCambiarPasada = new ServicioCambiarPasada(_uiapp, _ui.ListaEnvoltorioPipesTodos);
                    _ServicioCambiarPasada.RechazarPAsada(_ui.TextBox_Rechasar.Text);


                    CrearLIstaFiltros_PorEStado(_ui);
                    _ui.RecargarLIstaCOmpleta();
                }
                else if (_BotonOprimido == "Crear_BorrarSoloPasadas")
                {
                    //DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                    //CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                    List<ElementId> ListaBorrarId = new List<ElementId>();

                    if (AyudaObtenerPasadas.ObtenerPasadas(_doc, _CreadorExtStore))
                    {
                        ListaBorrarId.AddRange(AyudaObtenerPasadas.listaPasdas_Azul);
                        ListaBorrarId.AddRange(AyudaObtenerPasadas.listaPasdas_Rojo);
                        ListaBorrarId.AddRange(AyudaObtenerPasadas.listaPasdas_Verde);
                        ListaBorrarId.AddRange(AyudaObtenerPasadas.listaPasdas_Naranjo);
                        ListaBorrarId.AddRange(AyudaObtenerPasadas.listaPasdas_Gris);
                    }

                    using (Transaction t = new Transaction(_doc))
                    {
                        t.Start("crearSection-NH");
                        _doc.Delete(ListaBorrarId);
                        t.Commit();
                    }
                    _ui.ResetarLIstas();

                    _ui.CantidadElement = 0;
                }
                else if (_BotonOprimido == "SeleccionarPipe")
                {

                    var lsiat = SeleccionarOpening.SeleccionarAll(_doc);

                    PosicionView _PosicionView = new PosicionView(_uiapp);
                    _PosicionView.View = _view;


                    Transform transform = Transform.Identity;
                    transform.Origin = XYZ.Zero;
                    transform.BasisX = XYZ.BasisX;
                    transform.BasisY = XYZ.BasisY;
                    transform.BasisZ = XYZ.BasisZ;

                    BoundingBoxXYZ _newBoxXYZ = new BoundingBoxXYZ();
                    _newBoxXYZ.Transform = transform;

                    var pipe = _ui._EnvoltorioPipesSeleccionado;
                    var pt1 = _ui._EnvoltorioPipesSeleccionado.PMin;
                    var pt2 = _ui._EnvoltorioPipesSeleccionado.PMax;

                    XYZ direcBOuderingbox = (pt2 - pt1).Normalize();

                    double dif = (_ui.ZMaxNivel - _ui.ZminNivel);

                    _newBoxXYZ.Min = (pt1 - direcBOuderingbox * _ui.AnchoSeleccionValor_foot); ;// _ui._EnvoltorioPipesSeleccionado.PMin;
                    _newBoxXYZ.Max = (pt2 + direcBOuderingbox * _ui.AnchoSeleccionValor_foot);// _ui._EnvoltorioPipesSeleccionado.PMax;
                    //generar Zoom
                    _PosicionView.M3_ObtenerZOOMViewBarras(_newBoxXYZ);

                    // cambiar coord para ver corte
                    _newBoxXYZ.Min = (pt1 - direcBOuderingbox * _ui.AnchoSeleccionValor_foot).AsignarZ(_ui.ZminNivel - dif * 0.1);// _ui._EnvoltorioPipesSeleccionado.PMin;
                    _newBoxXYZ.Max = (pt2 + direcBOuderingbox * _ui.AnchoSeleccionValor_foot).AsignarZ(_ui.ZMaxNivel + dif * 0.1);// _ui._EnvoltorioPipesSeleccionado.PMax;

                    var sexBox = ((View3D)_view).GetSectionBox();

                    try
                    {
                        using (Transaction t = new Transaction(_doc))
                        {
                            t.Start("crearSection-NH");
                            ((View3D)_view).SetSectionBox(_newBoxXYZ);
                            t.Commit();
                        }

                        var listaUi = _uiapp.ActiveUIDocument.GetOpenUIViews().Where(c => c.ViewId == _view.Id).FirstOrDefault();
                        if (listaUi != null)
                        {
                            XYZ Dmin = pt1 - direcBOuderingbox * _ui.AnchoSeleccionValor_foot - XYZ.BasisZ;// _ui._EnvoltorioPipesSeleccionado.PMin;
                            XYZ Dmax = pt2 + direcBOuderingbox * _ui.AnchoSeleccionValor_foot + XYZ.BasisZ;// _ui._EnvoltorioPipesSeleccionado.PMax;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ex:{ex.Message}");
                    }
                    if (pipe.PasadaId != -1)
                    {
                        List<ElementId> list = new List<ElementId>() { new ElementId(pipe.PasadaId) };
                        _uiapp.ActiveUIDocument.Selection.SetElementIds(list);
                    }
                    // creacion de la linea dehabilitada por generaba mensaje de error y revit queda tomado 
                    if (pipe != null && false)
                    {
                        List<EnvoltorioBase> lista = new List<EnvoltorioBase>();
                        lista.Add(pipe);
                        CrearLinea(_doc, lista);
                    }
                }
                else if (_BotonOprimido == "DibujarLinea")
                {
                    var pipe = _ui._EnvoltorioPipesSeleccionado;

                    List<EnvoltorioBase> lista = new List<EnvoltorioBase>();
                    lista.Add(pipe);
                    CrearLinea(_doc, lista);
                }
                else if (_BotonOprimido == "Visibilidad")
                {
                    ManejadorVisibilidadLink _ManejadorVisibilidadLink = new ManejadorVisibilidadLink(_uiapp);
                    _ManejadorVisibilidadLink.Ejecutar(_ui.listaRevitLink.Where(c => c.RevitLinkInstanc != null)
                                                                                             .Select(c => c.RevitLinkInstanc.Id).ToList(), _ui.ISvisibilidad);
                    _ui.ISvisibilidad = !_ui.ISvisibilidad;
                }
                else if (_BotonOprimido == "VisibilidadPasadas")
                {

                    IVisibilidadView visibilidad = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_GenericModel, "Generic Models");
                    visibilidad.CambiarVisibilityBuiltInCategory();
                }
                else if (_BotonOprimido == "infoLink")
                {
                    if (_ui.LinkSeleccionado.Nombre.ToLower() != "todos")
                    {
                        var trasnd = _ui.LinkSeleccionado.RevitLinkInstanc.GetTotalTransform();
                        Util.InfoMsg($"Nombre:{_ui.LinkSeleccionado.Nombre}\n IsTranslation: {trasnd.IsTranslation}\nOrigin:{trasnd.Origin.REdondearString_foot(4)}");
                    }
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message}");
            }

            _ui.Show();
            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }

        private static void BuscarPasadas_ConEspecialidades(UI_Pasadas _ui, UIApplication _uiapp, View3D elem3d)
        {
            List<LinkDOcumentosDTO> ListaLink = new List<LinkDOcumentosDTO>();

            if (_ui.LinkSeleccionado.Nombre.ToLower() == "todos")
                ListaLink.AddRange(_ui.listaRevitLink.Where(c => c.Pathname.ToLower() != "todos"));
            else
                ListaLink.Add(_ui.LinkSeleccionado);


            ManejadorPin _ManejadorPin = new ManejadorPin(_uiapp);
            _ManejadorPin.EjecutarParaLIstaElemeto(true, ListaLink.Select(x => x.RevitLinkInstanc as Element).ToList());


            // limpiar
            _ui.IdFiltrado.Text = "";
            //  _ui.ListaEnvoltorioPipesTodos.Clear();
            var listaElementoExistente = _ui.ListaEnvoltorioPipesTodos.Select(c => c._elemento.Id).ToList();
            _ui.ListaFiltroTipo.Clear();

            ObservableCollection<EnvoltorioBase> AuxListaMEPObser = new ObservableCollection<EnvoltorioBase>();
            List<EnvoltorioBase> listaMeps = new List<EnvoltorioBase>();
            List<string> listaTipo = new List<string>();
            List<string> listaEjes = new List<string>();
            for (int i = 0; i < ListaLink.Count; i++)
            {
                var link = ListaLink[i];
                link.Inicio = _ui.Inicio;
                link.Cantidad = _ui.Cantidad;


                ServicioBuscarPasadas _BuscarPasadas = new ServicioBuscarPasadas(_uiapp, listaElementoExistente);
                if (!_BuscarPasadas.ObtenerListaDUctosPipen_EnLink(elem3d, link)) continue;

                listaMeps.AddRange(_BuscarPasadas.ListaMEPObser);
                _ui.ListaEnvoltorioPipesTodos.AddRange(_BuscarPasadas.ListaMEPObser);
                //*OBTENER GROPO DE TIPOD
                listaTipo.AddRange(listaMeps.GroupBy(c => c.NombreDucto).Select(r => r.Key).ToList());
            }

            //fuera for
            listaTipo.Add("");
            listaTipo.Reverse();

            listaEjes = listaMeps.GroupBy(c => c.ejesGrilla).Select(c => c.Key).ToList();
            listaEjes.Add("");
            listaEjes.Reverse();

            _ui.ListaEjes = new ObservableCollection<string>(listaEjes.OrderBy(x => x).ToList());
            _ui.ListaFiltroTipo = new ObservableCollection<string>(listaTipo);

            //elementos 'listBox'
            _ui.ListaEnvoltorioMEP.Clear();
            _ui.ListaEnvoltorioMEP = new ObservableCollection<EnvoltorioBase>(listaMeps.OrderBy(c => c.ejesGrilla));   //_BuscarPasadas.ListaMEPObser;
            _ui.CantidadElement = listaMeps.Count;// _BuscarPasadas.ListaMEPObser.Count;
        }

        private static void CrearLIstaFiltros_PorEStado(UI_Pasadas _ui)
        {
            var listaCreado = _ui.ListaEnvoltorioPipesTodos.GroupBy(c => c.EstadoShaft).Select(r => r.Key).ToList();
            listaCreado.Add("");
            listaCreado.Reverse();
            _ui.ListaFiltroCreados.Clear();
            _ui.ListaFiltroCreados = new ObservableCollection<string>(listaCreado);

        }

        private static void CambiarEstadoShaftConError(ObservableCollection<EnvoltorioBase> ListaEnvoltorioMEP)
        {
            int cont1 = 0;
            int cont2 = 0;

            foreach (var item in ListaEnvoltorioMEP)
            {
                cont1 = cont1 + 1;
                cont2 = -1;
                for (int i = 0; i < item.ListaPasadas.Count; i++)
                {
                    cont2 = cont2 + 1;
                    var pasada = item.ListaPasadas[i];
                    if (pasada.OpeningCreado == null)
                    {
                        item.EstadoShaft = "Error";
                        item.Comentario = "Shaft creado, pero con estado NULL";
                        item.ColorEstadoShaft = "Red";
                        continue;
                    }

                    if (!pasada.OpeningCreado.IsValidObject)
                    {
                        item.EstadoShaft = "Error";
                        item.Comentario = "Shaft creado, pero tiene estado de objeto invalido";
                        item.ColorEstadoShaft = "Red";
                    }
                }
            }
        }

        private static bool CrearLinea(Document _doc, List<EnvoltorioBase> lista)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Asignar estado visibilidad-NH");
                    foreach (var item in lista)
                    {
                        CrearModeLineAyuda.modelarlinea_sinTrans(_doc, item.Pto1, item.Pto2);
                    }
                    t.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}