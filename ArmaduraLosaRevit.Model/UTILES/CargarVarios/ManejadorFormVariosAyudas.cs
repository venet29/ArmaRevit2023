using ApiRevit.FILTROS;
using ArmaduraLosaRevit.Model.BarraV.AgregarEspMuro;
using ArmaduraLosaRevit.Model.DatosExcel;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.LosaEstructural;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;
using ArmaduraLosaRevit.Model.ParametrosShare.Actualizar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.TablasSchedule;
using ArmaduraLosaRevit.Model.UTILES.ActualizarNombreView;
using ArmaduraLosaRevit.Model.UTILES.Cambiar;
using ArmaduraLosaRevit.Model.ViewFilter;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using ArmaduraLosaRevit.Model.BarraV.Desglose.WPF;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.WPF;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.Fund;
using ArmaduraLosaRevit.Model.Cubicacion.WPF;
using ArmaduraLosaRevit.Model.Cubicacion;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Vigas;
using ArmaduraLosaRevit.Model.TablasSchedule.Wpf;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Borrar;
using ArmaduraLosaRevit.Model.RebarCopia;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.ViewFilter.Model;
using ArmaduraLosaRevit.Model.ViewFilter.Servicios;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Visualizacion;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.LevelNh;
using ArmaduraLosaRevit.Model.CopiaLocal;
using ArmaduraLosaRevit.Model.GRIDS.WPF_AgregraEJE;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar.Trabas.WPF;

namespace ArmaduraLosaRevit.Model.UTILES.CargarVarios
{
    public class ManejadorFormVariosAyudas
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public ManejadorFormVariosAyudas(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.ActiveView;


        }

        public void Ejecutar(System.Reflection.Assembly assembly)
        {

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            try
            {
                FormVariosAyuda _FormVariosAyuda = new FormVariosAyuda();

                _FormVariosAyuda.ShowDialog();

                if (_FormVariosAyuda.tipoDeEjecucion == "prueba")
                {

                    BuscadorRutasLocal _BuscadorRutasLocal = new BuscadorRutasLocal(_uiapp);
                    _BuscadorRutasLocal.COpiarArchivoREspaldo();

                    //ExtenderLevel _ExtenderLevel = new ExtenderLevel(_uiapp);
                    //_ExtenderLevel.Extender();


                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "AgregarEjes") 
                {
                    ManejadorWPF_AgregarEje _ManejadorWPF_AgregarEje = new ManejadorWPF_AgregarEje(_uiapp);
                    _ManejadorWPF_AgregarEje.Execute();

                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "copiarFundaciones")
                {
                    ManejadorCopiaFundaciones _ManejadorCopiaFundaciones = new ManejadorCopiaFundaciones(_uiapp);
                    _ManejadorCopiaFundaciones.EjecutarLosaFunda();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "copiarcolumna")
                {
                    ManejadorCopiaFundaciones _ManejadorCopiaFundaciones = new ManejadorCopiaFundaciones(_uiapp);
                    _ManejadorCopiaFundaciones.EjecutarColumna();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "ListaFamilias")
                {
                    LimpiandoListas.ListaFamilias();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizarFund")
                {
                    FundManejadorCambiarHook _FundManejadorCambiarHook = new FundManejadorCambiarHook(_uiapp);
                    _FundManejadorCambiarHook.EjecutarActualizar();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizarBarraTipoTodos")
                {
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.EjecutarTodoVIewSection_actulizarBarraTipo();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizarBarraTipo1")
                {
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.Ejecutar_enVistaActual();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizarNOmbreyItipoPAthReinf")
                {
                    //para 3D
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.Ejecutar1VIewSection_actulizarNOmbreVistaTipoBArra_PAthreinf();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizarNumeroBarro")
                {
                    ManejadorActualizar _ManejadorActualizar = new ManejadorActualizar(_uiapp);
                    _ManejadorActualizar.Ejecutar_ActualizarNUmeroBArra();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "ActualizarNbarrastodas")
                {
                    ManejadorActualizar _ManejadorActualizar = new ManejadorActualizar(_uiapp);
                    _ManejadorActualizar.Ejecutar_ActualizarNUmeroBArraTOdasLAsvistas();
                }

                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizarLargoFundaciones")
                {
                    ManejadorActualizar _ManejadorActualizar = new ManejadorActualizar(_uiapp);
                    _ManejadorActualizar.Ejecutar_ActualizarLargoFundaciones();
                }

                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizaNombreVista_seleccionar")
                {
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.Ejecuta_Seleccionar_actulizarNOmbreVista(_uiapp.ActiveUIDocument.ActiveView);
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizaNombreVistaTodos")
                {
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.EjecutarTodoVIewSection_actulizarNOmbreVista();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "actualizaNombre1_vistaActual")
                {
                    ManejadorVisibilidadActualizarNombreVista _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadActualizarNombreVista(_uiapp);
                    _ManejadorVisibilidadElemenNoEnView.Ejecutar();
                }

                else if (_FormVariosAyuda.tipoDeEjecucion == "NombreVista_solopath_en3D")
                {
                    var _view = _uiapp.ActiveUIDocument.ActiveView;
                    SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);



                    ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, null, seleccionarRebar, null, _view, _view.Name);
                    ManejadorVisibilidad.M6_ActualizarNombreVista_soloReBArInsystemtemdesdePAthrein();
                }




                else if (_FormVariosAyuda.tipoDeEjecucion == "genararId_Correlativo_1_view")
                {
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.EjecutarViewActual_GenerarID_CCorrelativo();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "AgregarEspesor_All_view")
                {
                    ManejadorAgregarEspesor _ManejadorAgregarEspesor = new ManejadorAgregarEspesor(_uiapp);
                    _ManejadorAgregarEspesor.EjecutarAllView();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "AgregarEspesor_1_view")
                {
                    ManejadorAgregarEspesor _ManejadorAgregarEspesor = new ManejadorAgregarEspesor(_uiapp);
                    _ManejadorAgregarEspesor.Ejecutar1View();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "agregarFIltroBarra_1view")
                {
                    CreadorViewFilter_OcultarBarrasNoDeVista _CreadorViewFilter = new CreadorViewFilter_OcultarBarrasNoDeVista(_uiapp);
                    _CreadorViewFilter.ObtenerNombreAntiguoVista(_uiapp.ActiveUIDocument.ActiveView);
                    _CreadorViewFilter.M2_CreateViewFilterTodos(_uiapp.ActiveUIDocument.ActiveView);
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "agregarFIltroBarra_Allview")
                {
                    CreadorViewFilterAllView _CreadorViewFilter = new CreadorViewFilterAllView(_uiapp);
                    _CreadorViewFilter.M1_CreateViewFilterEnTodasVistas();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "aCONFIGURACION ELEVA")
                {
                    Util.InfoMsg("Comando desactivado"); return;
                    //  ManejadorConfiguracionInicialElevacion.cargar_SoloCancino(_uiapp);
                }


                else if (_FormVariosAyuda.tipoDeEjecucion == "versiondll" && assembly != null)
                {

                    LogNH.Nombrdll(assembly);
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "UnobscuredInView")
                {
                    CambiarUnobscuredInView_path_rebar _CambiarUnobscuredInView_path_rebar = new CambiarUnobscuredInView_path_rebar(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
                    _CambiarUnobscuredInView_path_rebar.Ejecutar();
                }

                else if (_FormVariosAyuda.tipoDeEjecucion == "cambiarUsuario")
                {
                    Util.InfoMsg($"Pre nombrede 3d : { VariablesSistemas.CONST_NOMBRE_3D_PARA_BUSCAR.Replace("3D_NoEditar", "User")}");
                    TiposFamilia3D.Limpiar3DBuscar();
                    if (_FormVariosAyuda.nombreUSer == "Usuario1")
                    {
                        VariablesSistemas.CONST_NOMBRE_3D_PARA_BUSCAR = "3D_NoEditar";
                        ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR = VariablesSistemas.CONST_NOMBRE_3D_PARA_BUSCAR;
                    }
                    else if (_FormVariosAyuda.nombreUSer == "Usuario2")
                    {
                        VariablesSistemas.CONST_NOMBRE_3D_PARA_BUSCAR = "3D_NoEditarOp2";
                        ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR = VariablesSistemas.CONST_NOMBRE_3D_PARA_BUSCAR;
                    }

                    Util.InfoMsg($"Post nombrede 3d : { VariablesSistemas.CONST_NOMBRE_3D_PARA_BUSCAR.Replace("3D_NoEditar", "User")}");

                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "Cubicar" || _FormVariosAyuda.tipoDeEjecucion == "schedule")
                {

                    // Util.InfoMsg("Rutina desactivada");
                    //FormCub _FormCub = new FormCub();
                    //_FormCub.ShowDialog();

                    Ui_Schedule _FormCub = new Ui_Schedule(_uiapp);
                    _FormCub.ShowDialog();

                    if (_FormCub.casoTipo == "btn_crearExcel")
                    {

                        ManejadorWPF_Cub manejadorWPF = new ManejadorWPF_Cub(_uiapp);
                        manejadorWPF.Execute();

                    }
                    else if (_FormCub.casoTipo == "btn_crearSchedules")
                    {
                        ManejadorSchedule _ManejadorSchedule = new ManejadorSchedule(_uiapp);
                        _ManejadorSchedule.CrearSchedule_CubicacionBarras();
                    }

                    //ManejadorCub _ManejadorCub = new ManejadorCub(_uiapp);
                    //_ManejadorCub.Ejecutar();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "datosschedule")
                {
                    ManejadorSchedule _ManejadorSchedule = new ManejadorSchedule(_uiapp);
                    _ManejadorSchedule.ObtenerDatosExcel_Volument_moldaje(new List<LevelDTO>());

                    ExportarExcelDatos _ExportarExcelDatos = new ExportarExcelDatos(new ManejadorCubDTO());
                    _ExportarExcelDatos.M4_GuardareExcelHormigon(null, _ManejadorSchedule);
                }

                else if (_FormVariosAyuda.tipoDeEjecucion == "button_WPF_Level")
                {
                    ManejadorWPF_Cub manejadorWPF = new ManejadorWPF_Cub(_uiapp);
                    manejadorWPF.Execute();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "cambiarNombreVista")
                {
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.Ejecutar_CambirNombreVista("NombreVista", "NombreVista2");
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "volverNombreVista")
                {
                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.Ejecutar_CambirNombreVista("NombreVista2", "NombreVista");
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "SelecAyuda")
                {
                    ISelectionFilter filtroMuro = SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));

                    Reference ref_pickobject_element = _uiapp.ActiveUIDocument.Selection.PickObject(ObjectType.Element, filtroMuro, "Seleccionar Cara de Muro:");
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "SelecConfiguracion")
                {
                    IVisibilidadView PathRein_ = VisibilidadView.Creador_Visibilidad_SinInterfase(_view, BuiltInCategory.OST_PathRein, "Structural Path Reinforcement");
                    if (PathRein_ == null) return;

                    IVisibilidadView Rebar_ = VisibilidadView.Creador_Visibilidad_SinInterfase(_view, BuiltInCategory.OST_Rebar, "Structural Rebar");
                    if (Rebar_ == null) return;
                    // TaskDialog.Show("ok", "ayuda configuracion" );
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "ActualizarSeleccionar")
                {
                    ManejadoCambiarNOmbreView _ManejadoCambiarNOmbreView = new ManejadoCambiarNOmbreView(_uiapp);
                    _ManejadoCambiarNOmbreView.Ejecutar();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "cargartag")
                {

                    configTagNH _configTagNH = new configTagNH()
                    {
                        desplazamientoPathReinSpanSymbol = XYZ.Zero,
                        IsDIrectriz = false,
                        tagOrientation = TagOrientation.Vertical,
                        nombrefamilia = "12mm_50",
                        namefamilyEspecifico = "TAG MUROS (CORTO)_elev"

                    };
                    CreardorTagEspesor _CreardorTagEspesor = new CreardorTagEspesor(_uiapp);
                    _CreardorTagEspesor.C1CargandoIteracionSeleccion(_configTagNH);

                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "ocultarTraba")
                {
                    View _view = _uiapp.ActiveUIDocument.ActiveView;
                    SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);

                    //   VisibilidadElement VisibilidadElement = new VisibilidadElemen(commandData.Application);


                    ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, seleccionarRebar);
                    ManejadorVisibilidad.M10_Ocultar_Color_BarrasElevacion_traba_estrivovigaYLaterales(true);
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "MostrarTraba")
                {

                    SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);

                    //   VisibilidadElement VisibilidadElement = new VisibilidadElemen(commandData.Application);


                    ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, seleccionarRebar);
                    ManejadorVisibilidad.M10_Ocultar_Color_BarrasElevacion_traba_estrivovigaYLaterales(false);
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "actualize7.5")
                {

                    ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    _ManejadorActualizarBarraTipo.Ejecutar_enVistaActual_tarba7_5();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "cambiarParametroTrabas")
                {
                    Manejador_CambiarTag.Ejecutar(_uiapp);
                    // .ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                    //_ManejadorActualizarBarraTipo.Ejecutar_enVistaActual_FOrmatoTraba();
                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "Desglose_elev")
                {
                    ManejadorWPFDesglose _ManejadorWPFDesglose = new ManejadorWPFDesglose(_uiapp);
                    _ManejadorWPFDesglose.Execute();

                }

                else if (_FormVariosAyuda.tipoDeEjecucion == "Ver_barra_otra_vista")
                {
                    CreadorViewFilter_DejarBarrasNoDeVista _ManejadorWPFDesglose = new CreadorViewFilter_DejarBarrasNoDeVista(_uiapp);
                    _ManejadorWPFDesglose.M2_CreateViewFilterLosa(_view);
                }

                else if (_FormVariosAyuda.tipoDeEjecucion == "BorrarTodosFiltros")
                {


                    BorrarTodosFiltrosNot _BorrarTodosFiltrosNot = new BorrarTodosFiltrosNot(_uiapp);
                    _BorrarTodosFiltrosNot.BOrrarTodosFiltros();

                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "AgregarFormato")
                {

                    ManejadorWPF_FormatoEntrega _ManejadorWPF_FormatoEntrega = new ManejadorWPF_FormatoEntrega(_uiapp);
                    _ManejadorWPF_FormatoEntrega.Execute();

                }
                else if (_FormVariosAyuda.tipoDeEjecucion == "cambiarToArial")
                {

                    MAnejadorCambiarLetraArial _MAnejadorCambiarLetraArial = new MAnejadorCambiarLetraArial(_uiapp);
                    _MAnejadorCambiarLetraArial.Cambiar(true);

                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
        }
    }
}
