using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Seleccionar;
using formNH = System.Windows.Forms;
using System.Linq;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ViewFilter.Model;
using ArmaduraLosaRevit.Model.ViewFilter.Servicios;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Visualizacion;
using ArmaduraLosaRevit.Model.ViewFilter.Analisis;
using ArmaduraLosaRevit.Model.Viewnh.posicion;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.ParametrosShare;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.ViewFilter.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    public class Methods_Filtro3D
    {
        private static Document _doc;
        private static View _view;
#pragma warning disable CS0169 // The field 'Methods_Filtro3D.listaView' is never used
        private static List<View> listaView;
#pragma warning restore CS0169 // The field 'Methods_Filtro3D.listaView' is never used

        public static void M1_EjecutarRutinas(Ui_Filtro3D _ui_Filtro3D, UIApplication _uiapp)
        {
            try
            {
                _doc = _uiapp.ActiveUIDocument.Document;
                _view = _uiapp.ActiveUIDocument.ActiveView;

                List<View> ListaView = new List<View>();
                
                if (_view is ViewSheet)
                {
                    ListaView = (_view as ViewSheet).ObteneListaVIew();
                }
                else
                    ListaView.Add(_view);

                _ui_Filtro3D.Hide();

                if (_ui_Filtro3D.BotonOprimido == "btn_OpcionRevisiones")
                {
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];
                        List<ParametrosFiltro> listapara1 = new List<ParametrosFiltro>() { _ui_Filtro3D.ObtenerOpcionRevision() };
                        CreadorViewFilter_3DRevisarCub _CreadorViewFilter_3DRevisarCub = new CreadorViewFilter_3DRevisarCub(_uiapp, item);
                        _CreadorViewFilter_3DRevisarCub.M2_CreateViewFilterRevisar(listapara1);
                    }
                }
                else if (_ui_Filtro3D.BotonOprimido == "btnBorrarFiltreRevision")
                {
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];

                        CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, item);
                        _CreadorViewFilter_Base.M3_BorrarFiltros(new List<string>(), ListaFiltro.ListaFiltroBorrarLargoRevision());
                        _CreadorViewFilter_Base.M3_BorrarFiltros_RebarYPAth(new List<string>(), ListaFiltro.ListaFiltroBorrarRevision_sinLArgo());
                    }

                }
                else if (_ui_Filtro3D.BotonOprimido == "btn_cargarDatosInternos")
                {
                    if (!(_view is View3D))
                    {
                        Util.InfoMsg("Comandos solo aplicable en Vistas 3D. Se recomienda ejecutar en una vista 3D  para cargar los datos en todos los fierros del proyecto");
                        _ui_Filtro3D.Show();
                        return;
                    }

                    ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
                    if (!_definicionManejador.EjecutarLargoRevision()) return;

                    CargarParametros _CargarParametros = new CargarParametros(_uiapp);
                    if (!_CargarParametros.Ejecutar_copiarType_BarLengh()) return;


                    if (!(AyudaObtenerPesoBArras.Ejecutar(_uiapp, _view))) return;

                }
                else if (_ui_Filtro3D.BotonOprimido == "btn_ActivarBarras")
                {
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];
                        VisualizacionRebar _VisualizacionRebar = new VisualizacionRebar(_uiapp, item);
                        //if (!_VisualizacionRebar.AsiganarView3D(item)) return;

                        var _VisibilizacionFilterDTO = _ui_Filtro3D.ObtenerSettingVisibilizacion();
                        if (!_VisualizacionRebar.CambiarVisualizacionFiltro_Path_rebar_sectBox(_VisibilizacionFilterDTO)) return;
                    }
                }
                else if (_ui_Filtro3D.BotonOprimido == "btn_Diamtros")
                {
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];
                        //*** para borrar filtros          
                        var listaTodos = ListaFiltro.ListaFiltroBorrarDiam();
                        CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, item);
                        _CreadorViewFilter_Base.M3_BorrarFiltros(new List<string>(), listaTodos);
                        //***

                        // A.1) para dejar solo las barras de diamtro selecionado
                        var ListaFiltroBarra = FactoryParametrosFiltro.ListaFiltroBarra;
                        var ListaDiamtrVisibles = _ui_Filtro3D.ListaDiamtr.ToList();


                        List<ParametrosFiltro> listapara2 = (from rp in ListaFiltroBarra
                                                             from ld in ListaDiamtrVisibles
                                                             where (ld.NombreFiltro == rp.Nombrefilfro)
                                                             select rp.CambiarVisibili(ld.IsVisible)).ToList();

                        // A.2) para dejar solo las barras de diamtro selecionado
                        CreadorViewFilter_3DColorDiam _VisualizacionRebar = new CreadorViewFilter_3DColorDiam(_uiapp, item);
                        _VisualizacionRebar.M2_CreateViewFilterRevisar_Diam(listapara2);

                        //---------------------------------------------------------------

                        // B.1) para dejar solo las barras de diamtro selecionado
                        var ListaFiltroBarra_Color = FactoryParametrosFiltro.ListaFiltroBarra_color;
                        var ListaDiamtrVisibles_Color = _ui_Filtro3D.ListaDiamtr.ToList();


                        List<ParametrosFiltro> listapara2_Color = (from rp in ListaFiltroBarra_Color
                                                                   from ld in ListaDiamtrVisibles_Color
                                                                   where (ld.NombreFiltro == rp.Nombrefilfro)
                                                                   select rp.CambiarVisibili(ld.IsVisible)).ToList();

                        // B.2) para dejar solo las barras de diamtro selecionado
                        CreadorViewFilter_3DColorDiam _VisualizacionRebar_Color = new CreadorViewFilter_3DColorDiam(_uiapp, item);
                        _VisualizacionRebar_Color.M2_CreateViewFilterRevisar_DiamColor(listapara2_Color);
                    }
                }
                else if (_ui_Filtro3D.BotonOprimido == "btn_Tipo_")
                {
                    // var ListaFiltroBarra = FactoryParametrosFiltro.ListaFiltroBarra;
                    var listaTipo = _ui_Filtro3D.ListaTipos
                                                    .Where(c => c.IsVisible)
                                                    .Select(c => FactoryParametrosFiltro.ObtenerBarraTipo(c.NombreFiltro)).ToList();

                    //borrar primero
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];
                        List<string> ListaNombreContein = ListaFiltro.ListaFiltroBorrarTipo();
                        CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, item);
                        _CreadorViewFilter_Base.M3_BorrarFiltros_RebarYPAth(new List<string>(), ListaNombreContein);

                        //crear
                        CreadorViewFilter_3DPorTipo _CreadorViewFilter_3DPorTipo = new CreadorViewFilter_3DPorTipo(_uiapp, item);
                        _CreadorViewFilter_3DPorTipo.M2_CreateViewFilterPorTipo(listaTipo, $"MostrarBarras_PorTipo-NH");
                    }

                }
                else if (_ui_Filtro3D.BotonOprimido == "btn_TipoView_")
                {


                    if (!(_view is View3D))
                    {
                        Util.ErrorMsg("Comandos solo aplicable en Vistas 3D");
                        _ui_Filtro3D.Show();
                        return;
                    }
                    // var ListaFiltroBarra = FactoryParametrosFiltro.ListaFiltroBarra;
                    var listaTipo = _ui_Filtro3D.ListaTipos
                                                    .Where(c => c.IsVisible)
                                                    .Select(c => FactoryParametrosFiltro.ObtenerFiltroView(c.NombreFiltro)).ToList();

                    //borrar primero

                    List<string> ListaNombreContein = ListaFiltro.ListaFiltroBorrarTipo();
                    CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, _view);
                    _CreadorViewFilter_Base.M3_BorrarFiltros_RebarYPAth(new List<string>(), ListaNombreContein);

                    //crear
                    CreadorViewFilter_3DPorTipo _CreadorViewFilter_3DPorTipo = new CreadorViewFilter_3DPorTipo(_uiapp, _view);
                    _CreadorViewFilter_3DPorTipo.M2_CreateViewFilterPorTipo(listaTipo, $"MostrarBarras_PorView-NH");


                }
             
                else if (_ui_Filtro3D.BotonOprimido == "btn_BorrarFiltreDiametros")
                {
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];

                        CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, _view);
                        _CreadorViewFilter_Base.M3_BorrarFiltros_RebarYPAth(new List<string>(), ListaFiltro.ListaFiltroBorrarDiamAll());
                    }
                }
                else if (_ui_Filtro3D.BotonOprimido == "btn_BorrarTipo_")
                {
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];

                        CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, item);
                        _CreadorViewFilter_Base.M3_BorrarFiltros_RebarYPAth(new List<string>(), ListaFiltro.ListaFiltroBorrarTipo());
                    }

                }
                else if (_ui_Filtro3D.BotonOprimido == "btn_BorrarTipoView_")
                {
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];
                        var listaTodos = ListaFiltro.ListaFiltroBorrarView();
                        CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, item);
                        _CreadorViewFilter_Base.M3_BorrarFiltros_RebarYPAth(new List<string>(), listaTodos);
                    }
                }
                else if (_ui_Filtro3D.BotonOprimido == "btn_BorrarTodosFiltre")
                {
                    for (int i = 0; i < ListaView.Count; i++)
                    {
                        var item = ListaView[i];

     


                        var listaTodos = ListaFiltro.ListaFiltroBorrarDiamAll();
                        listaTodos.AddRange(ListaFiltro.ListaFiltroBorrarLargoRevision());
                        CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, item);
                        _CreadorViewFilter_Base.M3_BorrarFiltros(new List<string>(), listaTodos);


                        listaTodos.AddRange(ListaFiltro.ListaFiltroBorrarTipo());
                        listaTodos.AddRange(ListaFiltro.ListaFiltroBorrarView());
                        // CreadorViewFilter_Borrar _CreadorViewFilter_Base = new CreadorViewFilter_Borrar(_uiapp, _view);
                        _CreadorViewFilter_Base.M3_BorrarFiltros_RebarYPAth(new List<string>(), listaTodos);


                        //borrar filtros de revision , tipo, largo, sin tipo
                        _CreadorViewFilter_Base.M3_BorrarFiltros(new List<string>(), ListaFiltro.ListaFiltroBorrarLargoRevision());
                        _CreadorViewFilter_Base.M3_BorrarFiltros_RebarYPAth(new List<string>(), ListaFiltro.ListaFiltroBorrarRevision_sinLArgo());
                    }
                }

                else if (_ui_Filtro3D.BotonOprimido == "btn_SeleccionarBarra")
                {

                    SeleccionarRebaroRebarInSystemElemento sre = new SeleccionarRebaroRebarInSystemElemento(_uiapp, _view);
                    if (sre.GetSelecionarRebaroRebarInsistem())
                    {
                        var nombreBarraView = ParameterUtil.FindParaByName(sre._ElementoSeleccion.Parameters, "NombreVista")?.AsString();
                        if (nombreBarraView == null) nombreBarraView = "";

                        PosicionView _PosicionView = new PosicionView(_uiapp);

                        _PosicionView.M2_ActivarBounderyPAthReinf();
                        _PosicionView.M3_ObtenerViewBarras(sre._ElementoSeleccion.Id.IntegerValue);


                        if (!sre._ElementoSeleccion.IsHidden(_uiapp.ActiveUIDocument.ActiveView))
                        {
                            List<ElementId> List = new List<ElementId>() { sre._ElementoSeleccion.Id };
                            _uiapp.ActiveUIDocument.Selection.SetElementIds(List);
                            _uiapp.ActiveUIDocument.ShowElements(List);
                        }

                        var nombreBarraTipo = ParameterUtil.FindParaByName(sre._ElementoSeleccion.Parameters, "BarraTipo")?.AsString();
                        if (nombreBarraTipo == null) nombreBarraTipo = "";

                        var dismstr = sre._ElementoSeleccion.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsValueString();
                        _ui_Filtro3D.Dispatcher.Invoke(() =>
                       _ui_Filtro3D.TiposBarras.Content = $"Tipo:{nombreBarraTipo.Replace("_", "__")}\nDiam:{dismstr}\nId:{sre._ElementoSeleccion.Id.IntegerValue}");

                    }
                }


                _ui_Filtro3D.Show();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                _ui_Filtro3D.Show();
                _ui_Filtro3D.Close();
            }
        }




    }
}