using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.Cubicacion.Seleccionar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using ArmaduraLosaRevit.Model.TablasSchedule.Tipos;
using ArmaduraLosaRevit.Model.TablasSchedule.Usos;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Visualizacion;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.TablasSchedule
{
    public class ManejadorSchedule
    {
        private readonly UIApplication _uiapp;
        private readonly List<LevelDTO> _Lista_Level_Habilitados;
        private Document _doc;
        private string _ListaPlanos;
        private string _listaLosaMateria;
        private string _listamuroMateria;
        private string _listavigaMateria;
        private string _listaFundMAterial;
        private View3D view3D_Visualizar;

        public ScheduleLeer _scheduleNH_ListaPlanos { get; private set; }
        public ScheduleLeer _scheduleNH_MUROS { get; private set; }
        public ScheduleLeer _scheduleNH_LOSAS { get; private set; }
        public ScheduleLeer _scheduleNH_SUPERFICIE { get; private set; }
        public ScheduleLeer _scheduleNH_VIGA { get; private set; }
        public ScheduleLeer _scheduleNH_FUND { get; private set; }
        public TablasHormigoYModaje _TablasHormigoYModaje { get; set; }

        public ManejadorSchedule(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._Lista_Level_Habilitados = new List<LevelDTO>();
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._ListaPlanos = "LISTADO DE PLANOS";
            this._listaLosaMateria = "CUBICACION DE LOSAS POR MATERIAL";
            this._listamuroMateria = "CUBICACION DE MUROS POR MATERIAL";
            this._listavigaMateria = "CUBICACION DE VIGAS POR MATERIAL";
            this._listaFundMAterial = "CUBICACION DE FUNDACIONES POR MATERIAL";
        }



        public bool CrearSchedule_CubicacionBarras()
        {
            try
            {
                //1)
                if (!Obtener3D()) return false;

                _uiapp.ActiveUIDocument.ActiveView = view3D_Visualizar;

                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CrearSchedule_CubicacionBarras-NH");




                    //2)mostara barras
                    VisualizacionRebar _VisualizacionRebar = new VisualizacionRebar(_uiapp, view3D_Visualizar);
                    if (!_VisualizacionRebar.EjecutarCAmbiarBoundery())
                    {
                        t.RollBack();
                        return false;
                    }
                    if (!_VisualizacionRebar.CambiarVisualizacion_Path_rebar_sectBox(EstadoVista.visualizar))
                    {
                        t.RollBack();
                        return false;
                    }

                    //3)
                    //if (!AgregarPEsoAbarras()) return false;
                    if (!(AyudaObtenerPesoBArras.Ejecutar(_uiapp, view3D_Visualizar)))
                    {
                        t.RollBack();
                        return false;
                    }

                    //4
                    CubicacionBarras _CubicacionBarras = new CubicacionBarras(_uiapp, "Cubicacion Barras");
                    if (!_CubicacionBarras.CrearSchedule_CubicacionBarras())
                    {
                        t.RollBack();
                        return false;
                    }

                    CubicacionBarras _CubicacionBarras_BarraTipo = new CubicacionBarras(_uiapp, "Cubicacion Barras Verificar sinBarraTipo");
                    if (_CubicacionBarras_BarraTipo.CrearSchedule_CubicacionBarras())
                    {
                        if (!_CubicacionBarras_BarraTipo.agregarFiltro_CubicacionBarras("BarraTipo", "", ScheduleFilterType.HasNoValue))
                        {
                            t.RollBack();
                            return false;
                        }
                    }

                    CubicacionBarras _CubicacionBarras_sinNombreVista = new CubicacionBarras(_uiapp, "Cubicacion Barras Verificar sinNombreVista");
                    if (_CubicacionBarras_sinNombreVista.CrearSchedule_CubicacionBarras())
                    {
                        if (!_CubicacionBarras_sinNombreVista.agregarFiltro_CubicacionBarras("NombreVista", "", ScheduleFilterType.HasNoValue))
                        {
                            t.RollBack();
                            return false;
                        }
                    }

                    CubicacionBarras _CubicacionBarras_IgualNoNE = new CubicacionBarras(_uiapp, "Cubicacion Barras Verificar IgualNoNE");
                    if (_CubicacionBarras_IgualNoNE.CrearSchedule_CubicacionBarras())
                    {
                      if(!  _CubicacionBarras_IgualNoNE.agregarFiltro_CubicacionBarras("BarraTipo", "NONE", ScheduleFilterType.Equal))
                        {
                            t.RollBack();
                            return false;
                        }
                    }

                    FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc);
                    filteredElementCollector.OfClass(typeof(Rebar));
                    var rebar1 = filteredElementCollector.Cast<Rebar>().FirstOrDefault();

                    if (rebar1 != null)
                    {
                        var elementLength = rebar1.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH);

                        var elemId = elementLength.Id;
                        CubicacionBarras _CubicacionBarras_LargoMayor12 = new CubicacionBarras(_uiapp, "Cubicacion Barras Verificar LargoMayor12");
                        if (_CubicacionBarras_LargoMayor12.CrearSchedule_CubicacionBarras())
                        {
                          if(!  _CubicacionBarras_LargoMayor12.agregarFiltro_CubicacionBarras("Bar Length", Util.CmToFoot(1200), ScheduleFilterType.GreaterThanOrEqual, elemId))
                            {
                                t.RollBack();
                                return false;
                            }
                        }
                    }

                    CubicacionBarrasResumen _CubicacionBarrasResumen = new CubicacionBarrasResumen(_uiapp, "Cubicacion Barras Resumen PorBarraTipo");
                    if (!_CubicacionBarrasResumen.CrearSchedule_CubicacionBarrasResumen())
                    {
                        t.RollBack();
                        return false;
                    }

                    //5mostara barras
                    //OcultarBarras();
                    _VisualizacionRebar.CambiarVisualizacion_Path_rebar_sectBox(EstadoVista.Ocultar);
                    Util.InfoMsg($"Schedule 'cubiacion de barras' creado");
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al crear Schedule ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void OcultarBarras()
        {
            IVisibilidadView PathRein_ = VisibilidadView.Creador_Visibilidad(view3D_Visualizar, BuiltInCategory.OST_PathRein, "Structural Path Reinforcement");

            PathRein_.CambiarVisibilityBuiltInCategory();

            IVisibilidadView Rebar_ = VisibilidadView.Creador_Visibilidad(view3D_Visualizar, BuiltInCategory.OST_Rebar, "Structural Rebar");

            Rebar_.CambiarVisibilityBuiltInCategory();
        }

        private bool AgregarPEsoAbarras()
        {
            try
            {



                var SeleccionarRebar_PathReinforment = new SeleccionarRebar_PathReinforment(_uiapp, view3D_Visualizar);

                if (!SeleccionarRebar_PathReinforment.M0_CArgar_PAthReinforment()) return false;
                if (!SeleccionarRebar_PathReinforment.M0_Cargar_rebar()) return false;

                if (!SeleccionarRebar_PathReinforment.M1_Ejecutar_rebar())
                {
                    Util.ErrorMsg("Error al obtener Lista Rebar");
                    return false;
                }
                if (!SeleccionarRebar_PathReinforment.M1_Ejecutar_PAthReinforment())
                {
                    Util.ErrorMsg("Error al obtener Lista PAthReinforment");
                    return false;
                }

                if (!SeleccionarRebar_PathReinforment.M0_Cargar_rebar()) return false;

                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();

                    for (int i = 0; i < SeleccionarRebar_PathReinforment._lista_A_TodasRebarNivelActual_MENOSRebarSystem.Count; i++)
                    {
                        var rebar_ = SeleccionarRebar_PathReinforment._lista_A_ElementoREbarDTO[i];

                        double pesoBArra = rebar_._rebar.ObtenerPeso();

                        if (Util.IsSimilarValor(pesoBArra, 0, 0.01)) continue;

                        ParameterUtil.SetParaDoubleNH(rebar_._rebar, "PesoBarra", pesoBArra);
                    }


                    for (int i = 0; i < SeleccionarRebar_PathReinforment._lista_A_ElementoPathReinformentDTO.Count; i++)
                    {
                        var rPAth_ = SeleccionarRebar_PathReinforment._lista_A_ElementoPathReinformentDTO[i];

                        for (int j = 0; j < rPAth_._lista_A_DeRebarInSystem.Count; j++)
                        {

                            var rebInsys = (RebarInSystem)rPAth_._lista_A_DeRebarInSystem[j];
                            double pesoBArra = ((RebarInSystem)rPAth_._lista_A_DeRebarInSystem[j]).ObtenerPeso();

                            if (Util.IsSimilarValor(pesoBArra, 0, 0.01)) continue;

                            ParameterUtil.SetParaDoubleNH(rebInsys, "PesoBarra", pesoBArra);
                        }

                    }

                    tr.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        private bool Obtener3D()
        {
            try
            {
                view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);

                if (view3D_Visualizar == null)
                {
                    Util.InfoMsg("No se encontro vista 3d:{3D} para obtener obtener las barras.\nSe utiliza vista 3D: '3D_NoEditar'.");
                    view3D_Visualizar = TiposFamilia3D.Get3DBuscar(_doc);
                    if (view3D_Visualizar == null)
                    {
                        Util.InfoMsg("No se encontro vista 3d:'3D_NoEditar' para obtener obtener las barras.\nSe cancela en proceso");
                        return false;
                    }
                }

                _uiapp.ActiveUIDocument.ActiveView = view3D_Visualizar;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.InfoMsg("No se encontro vista 3d");
                return false;
            }
            return true;
        }

        public bool ObtenerDatosExcel_Volument_moldaje(List<LevelDTO> _lista_Level_habilitados, bool isMensaje = true)
        {

            try
            {
                //1
                ViewSchedule schedule_losa = TiposViewSchedule.ObtenerViewSchedule(_listaLosaMateria, _doc);
                if (schedule_losa != null)
                {
                    _scheduleNH_LOSAS = new ScheduleLeer(_uiapp, schedule_losa, _lista_Level_habilitados);
                    _scheduleNH_LOSAS.ObtenerDatosTAblas();


                    _scheduleNH_SUPERFICIE = new ScheduleLeer(_uiapp);
                    // obtiene las superficies
                    for (int i = 0; i < _scheduleNH_LOSAS.listaPtos.Count; i++)
                    {
                        DatosTablasDTO obj = _scheduleNH_LOSAS.listaPtos[i];
                        var NewOIbj = new object[] { obj.OrdeElevacion, obj.nivel, obj.area };
                        _scheduleNH_SUPERFICIE.listaPtosObj.Add(NewOIbj);
                    }
                }
                else
                    Util.ErrorMsg($"No se encontro Schedules  '{_listaLosaMateria}'");

                //2
                ViewSchedule schedule_listaPLanos = TiposViewSchedule.ObtenerViewSchedule(_ListaPlanos, _doc);
                if (schedule_listaPLanos != null)
                {
                    _scheduleNH_ListaPlanos = new ScheduleLeer(_uiapp, schedule_listaPLanos, _lista_Level_habilitados);
                    _scheduleNH_ListaPlanos.ObtenerListaPlanos();
                }
                else
                    Util.ErrorMsg($"No se encontro Schedules  '{_ListaPlanos}'");

                //3
                ViewSchedule schedule_muro = TiposViewSchedule.ObtenerViewSchedule(_listamuroMateria, _doc);
                if (schedule_muro != null)
                {
                    _scheduleNH_MUROS = new ScheduleLeer(_uiapp, schedule_muro, _lista_Level_habilitados);
                    _scheduleNH_MUROS.ObtenerDatosTAblas();
                }
                else
                    Util.ErrorMsg($"No se encontro Schedules  '{_listamuroMateria}'");

                //4
                ViewSchedule schedule_viga = TiposViewSchedule.ObtenerViewSchedule(_listavigaMateria, _doc);
                if (schedule_viga != null)
                {
                    _scheduleNH_VIGA = new ScheduleLeer(_uiapp, schedule_viga, _lista_Level_habilitados);
                    _scheduleNH_VIGA.ObtenerDatosTAblas();
                }
                else
                    Util.ErrorMsg($"No se encontro Schedules  '{_listavigaMateria}'");

                //5
                ViewSchedule schedule_fund = TiposViewSchedule.ObtenerViewSchedule(_listaFundMAterial, _doc);
                if (schedule_fund != null)
                {
                    _scheduleNH_FUND = new ScheduleLeer(_uiapp, schedule_fund, _lista_Level_habilitados);
                    _scheduleNH_FUND.ObtenerDatosTAblas();

                }
                else
                    Util.ErrorMsg($"No se encontro Schedules  '{_listaFundMAterial}'");


                _TablasHormigoYModaje = new TablasHormigoYModaje(_scheduleNH_MUROS, _scheduleNH_LOSAS, _scheduleNH_ListaPlanos, _scheduleNH_VIGA, _scheduleNH_FUND);
                _TablasHormigoYModaje.obtenerTablas();

                if (isMensaje)
                    Util.InfoMsg($"Excel 'Volumen Y moldaje' creado");
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al crear Schedule ex:{ex.Message}");
                return false;
            }
            return true;
        }


    }
}
