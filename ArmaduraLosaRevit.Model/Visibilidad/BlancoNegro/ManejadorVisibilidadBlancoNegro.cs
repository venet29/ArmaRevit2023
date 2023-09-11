using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Viewnh;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro
{
    public class ManejadorVisibilidadBlancoNegro
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public ManejadorVisibilidadBlancoNegro(UIApplication application)
        {
            this._uiapp = application;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = this._uiapp.ActiveUIDocument.ActiveView;
        }

        #region a) color normal

        //A)
        public Result M1_EjecutarListaVista_ColorNormal(List<View> LitaView)
        {
            try
            {
                for (int i = 0; i < LitaView.Count; i++)
                {
                    using (TransactionGroup t = new TransactionGroup(_doc))
                    {
                        t.Start("CambiarInvertirBlancoNegro-NH");

                        View _viewAnalizada = LitaView[i];
                        M1_1_EjecutarColorNormal(_viewAnalizada);
                        t.Assimilate();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al cambiar formato de entraga 'Blanco Negro' ex:{ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }


        private Result M1_1_EjecutarColorNormal(View _view)
        {
            try
            {
                //true oculta
                // false ver
                VisibilidadGraphicSettings_View_BlancoNegro visibilidadView = new VisibilidadGraphicSettings_View_BlancoNegro(_uiapp, _view);

                if (!visibilidadView.M1_CambiarColor_BlancoONegro(false, CategoryType.Model)) return Result.Failed;

                if (!visibilidadView.M1_CambiarColor_BlancoONegro(false, CategoryType.Annotation)) return Result.Failed;

                if (!visibilidadView.M2_CambiarColor_BlancoONegro_porElemento(false)) return Result.Failed;

                if (_view.ViewType == ViewType.Section)
                {
                    SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                    ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, seleccionarRebar);
                    ManejadorVisibilidad.M9_Restablecer_Color_BarrasElevacion();

                    // CreadorView _CreadorView = new CreadorView(_uiapp);
                    // _CreadorView.M2_ViewTemplateElevacion(_view, ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV);
                    //  _CreadorView.M2_SinViewTemplate(_view);
                    // Util.InfoMsg("Falta volver colore");
                }
                else if (_view.ViewType == ViewType.FloorPlan)
                {
                    ManejadorCmdVisibilidadElement.CmdM3_MostrarPAthNormal(_uiapp, _view);

                    // CreadorView _CreadorView = new CreadorView(_uiapp);
                    // _CreadorView.M2_ViewTemplateElevacion(_view, ConstNH.NOMBRE_VIEW_TEMPLATE_LOSA);
                    // _CreadorView.M2_SinViewTemplate(_view);
                }
                else if (_view.ViewType == ViewType.CeilingPlan)
                {
                    // ManejadorCmdVisibilidadElement.CmdM3_MostrarPAthNormal(_uiapp, _view);

                    CreadorView _CreadorView = new CreadorView(_uiapp);
                    _CreadorView.M2_ViewTemplateElevacion(_view, ConstNH.NOMBRE_VIEW_TEMPLATE_ESTRUC);
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


        #endregion
        #region b) BlancoNegro
        //B)

        public Result M2_EjecutarListaVista_BlancoNegro(List<View> LitaView)
        {
            try
            {


                // 
                for (int i = 0; i < LitaView.Count; i++)
                {
                    View _viewAnalizada = LitaView[i];
                    _viewAnalizada.DesactivarViewTemplate_ConTrans();
                }


                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CambiarInvertirBlancoNegro-NH");
                    for (int i = 0; i < LitaView.Count; i++)
                    {


                        View _viewAnalizada = LitaView[i];
                        M2_1_EjecutarBlancoNegro(_viewAnalizada);

                    }
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al cambiar formato de entraga 'Blanco Negro' ex:{ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }
        private Result M2_1_EjecutarBlancoNegro(View _view)
        {
            try
            {


                //true oculta
                // false ver
                VisibilidadGraphicSettings_View_BlancoNegro visibilidadView = new VisibilidadGraphicSettings_View_BlancoNegro(_uiapp,_view);
                List<string> listaExclusion = new List<string>();



                if (!visibilidadView.M1_CambiarColor_BlancoONegro(true, CategoryType.Model)) return Result.Failed;


                if (!visibilidadView.M1_CambiarColor_BlancoONegro(true, CategoryType.Annotation)) return Result.Failed;

                if (!visibilidadView.M2_CambiarColor_BlancoONegro_porElemento(true)) return Result.Failed;


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en volver formato normal de view ex:{ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }


        #endregion

    }
}
