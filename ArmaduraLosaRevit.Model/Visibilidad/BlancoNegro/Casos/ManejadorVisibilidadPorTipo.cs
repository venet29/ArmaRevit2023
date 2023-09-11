using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Viewnh;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro
{
    public class ManejadorVisibilidadPorTipo
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        SeleccionarRebarVisibilidad _seleccionarRebarVisibilidad;
        private SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad;

        public ManejadorVisibilidadPorTipo(UIApplication application)
        {
            this._uiapp = application;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = this._uiapp.ActiveUIDocument.ActiveView;

        }


        //A)
        public Result M1_EjecutarListaVista_POrtipo(List<View> LitaView)
        {
            try
            {



                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarInvertirBlancoNegro1-NH");

                    for (int i = 0; i < LitaView.Count; i++)
                    {
                        View _viewAnalizada = LitaView[i];
                        _viewAnalizada.DesactivarViewTemplate_SinTrans();
                    }
                    t.Commit();

                }


                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarInvertirBlancoNegro2-NH");
                    for (int i = 0; i < LitaView.Count; i++)
                    {
                        View _viewAnalizada = LitaView[i];

                        _seleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _viewAnalizada);                   
                        if (!_seleccionarRebarVisibilidad.BuscarListaRebarEnVistaActualElevacion()) return Result.Failed;

                        _SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _viewAnalizada);
                        if (!_SelecPathReinVisibilidad.M1_ejecutar()) return Result.Failed;


                        M1_1_EjecutarColor_POrTipo(_viewAnalizada);
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al cambiar formato de entraga 'Blanco Negro' ex:{ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }


        private Result M1_1_EjecutarColor_POrTipo(View _view_analizado)
        {
            try
            {
                if (_view_analizado.ViewType == ViewType.Section)
                {
                    //modifica el vv
                    VisibilidadGraphicSettings_View_EntregaElevacion View_EntregaElevacion = new VisibilidadGraphicSettings_View_EntregaElevacion(_view_analizado);
                    View_EntregaElevacion.M1_ObtenerListaElementosVisibles(CategoryType.Model);
                    if (!View_EntregaElevacion.M2_CambiarColor_EntregaElev(true, CategoryType.Model)) return Result.Failed;

                    View_EntregaElevacion.M1_ObtenerListaElementosVisibles(CategoryType.Annotation);
                    if (!View_EntregaElevacion.M2_CambiarColor_EntregaElev(true, CategoryType.Annotation)) return Result.Failed;

                    //modifica los elementos seleccionados
                    VisibilidadTipoBarraElev _VisibilidadTipoBarraElev = new VisibilidadTipoBarraElev(_uiapp, _seleccionarRebarVisibilidad, _view_analizado);
                    _VisibilidadTipoBarraElev.Ejecutar(_view_analizado);
                }
                else if (_view_analizado.ViewType == ViewType.FloorPlan)
                {
                    //modifica el vv
                    VisibilidadGraphicSettings_View_EntregaLosa View_EntregaLOSA = new VisibilidadGraphicSettings_View_EntregaLosa(_view_analizado);

                    View_EntregaLOSA.M1_ObtenerListaElementosVisibles(CategoryType.Model);
                    if (!View_EntregaLOSA.M2_CambiarColor_EntregaElev(true, CategoryType.Model)) return Result.Failed;

                    View_EntregaLOSA.M1_ObtenerListaElementosVisibles(CategoryType.Annotation);
                    if (!View_EntregaLOSA.M2_CambiarColor_EntregaElev(true, CategoryType.Annotation)) return Result.Failed;

                    //modifica los elementos seleccionados
                    VisibilidadTipoBarraLosas _VisibilidadTipoBarraLosas = new VisibilidadTipoBarraLosas(_uiapp, _seleccionarRebarVisibilidad, _SelecPathReinVisibilidad, _view_analizado);
                    _VisibilidadTipoBarraLosas.Ejecutar(_view_analizado);

                }
                else if (_view_analizado.ViewType == ViewType.CeilingPlan)
                {
                    // ManejadorCmdVisibilidadElement.CmdM3_MostrarPAthNormal(_uiapp, _view);

                    CreadorView _CreadorView = new CreadorView(_uiapp);
                    _CreadorView.M2_ViewTemplateElevacion(_view_analizado, ConstNH.NOMBRE_VIEW_TEMPLATE_ESTRUC);
                    // _CreadorView.M2_SinViewTemplate(_view);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en el formato blanco negro ex:{ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }





    }
}
