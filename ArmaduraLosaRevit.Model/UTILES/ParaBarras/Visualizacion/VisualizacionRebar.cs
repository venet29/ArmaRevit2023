using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Visualizacion
{
    enum EstadoVista
    {
        Ocultar, //fuerza a ocultar
        visualizar, //fuerza a visulaizar
        Cambiar // cambia de estado
    }
    class VisualizacionRebar
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View view_Visualizar;

        public VisualizacionRebar(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }
        public VisualizacionRebar(UIApplication _uiapp, View view3D_Visualizar)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this.view_Visualizar = view3D_Visualizar;
        }
        //public bool Obtener3D(string nombre3d)
        //{
        //    try
        //    {
        //        view_Visualizar = TiposFamilia3D.Get3DBuscar(_doc, nombre3d);
        //        //  view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
        //        if (view_Visualizar == null)
        //        {
        //            Util.InfoMsg("No se encontro vista 3d:{3D} para obtener obtener las barras.\nSe utiliza vista 3D: '3D_NoEditar'.");
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        public bool AsiganarView3D(View3D _view)
        {
            try
            {
                view_Visualizar = _view;
                //  view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
                if (view_Visualizar == null)
                {
                    Util.InfoMsg("No se encontro vista 3d:{3D} para obtener obtener las barras.\nSe utiliza vista 3D: '3D_NoEditar'.");
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool CambiarVisualizacion_Path_rebar_sectBox(EstadoVista estado)
        {
            try
            {
                if (view_Visualizar == null)
                {
                    Util.InfoMsg("No se encontro vista 3d:{3D} para obtener obtener las barras.");
                    return false;
                }
                if (view_Visualizar is View3D)
                    TiposFamilia3D.DesactivarSectionBox(_doc, (View3D)view_Visualizar, false);

                VisibilidadView PathRein_ = VisibilidadView.Creador_Visibilidad_SinInterfase(view_Visualizar, BuiltInCategory.OST_PathRein, "Structural Path Reinforcement");
                if (PathRein_ == null) return false;

                VisibilidadView Rebar_ = VisibilidadView.Creador_Visibilidad_SinInterfase(view_Visualizar, BuiltInCategory.OST_Rebar, "Structural Rebar");
                if (Rebar_ == null) return false;

                switch (estado)
                {
                    case EstadoVista.Ocultar:
                        {
                            if (!PathRein_.EstadoActualHide())
                                PathRein_.CambiarVisibilityBuiltInCategory();
                            if (!Rebar_.EstadoActualHide())
                                Rebar_.CambiarVisibilityBuiltInCategory();

                            break;
                        }
                    case EstadoVista.visualizar:
                        {
                            if (PathRein_.EstadoActualHide())
                                PathRein_.CambiarVisibilityBuiltInCategory();
                            if (Rebar_.EstadoActualHide())
                                Rebar_.CambiarVisibilityBuiltInCategory();

                            break;
                        }
                    case EstadoVista.Cambiar:
                        {
                            PathRein_.CambiarVisibilityBuiltInCategory();
                            Rebar_.CambiarVisibilityBuiltInCategory();

                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }


        public bool CambiarVisualizacionFiltro_Path_rebar_sectBox(VisibilizacionFilterDTO _visibilizacionFilterDTO)
        {
            try
            {
                if (view_Visualizar == null)
                {
                    Util.InfoMsg("No se encontro vista 3d:{3D} para obtener obtener las barras.");
                    return false;
                }

                // SectionBox
                if (view_Visualizar is View3D)
                    TiposFamilia3D.DesactivarSectionBox(_doc, (View3D)view_Visualizar, _visibilizacionFilterDTO.SectionBox);

                VisibilidadView PathRein_ = VisibilidadView.Creador_Visibilidad_SinInterfase(view_Visualizar, BuiltInCategory.OST_PathRein, "Structural Path Reinforcement");
                if (PathRein_ == null) return false;

                VisibilidadView Rebar_ = VisibilidadView.Creador_Visibilidad_SinInterfase(view_Visualizar, BuiltInCategory.OST_Rebar, "Structural Rebar");
                if (Rebar_ == null) return false;

                view_Visualizar.MOdificarCropRegion_ConTras(_visibilizacionFilterDTO.CropRegion);

                //path
                if (_visibilizacionFilterDTO.IsPAth)
                {
                    if (PathRein_.EstadoActualHide())
                        PathRein_.CambiarVisibilityBuiltInCategory();
                }
                else
                {
                    if (!PathRein_.EstadoActualHide())
                        PathRein_.CambiarVisibilityBuiltInCategory();
                }
                //rebar
                if (_visibilizacionFilterDTO.IsRebar)
                {
                    if (Rebar_.EstadoActualHide())
                        Rebar_.CambiarVisibilityBuiltInCategory();
                }
                else
                {
                    if (!Rebar_.EstadoActualHide())
                        Rebar_.CambiarVisibilityBuiltInCategory();
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }


        //
        public bool EjecutarCAmbiarBoundery()
        {
            try
            {
                if (view_Visualizar == null)
                {
                    Util.InfoMsg("No se encontro vista 3d:{3D} para obtener obtener las barras.");
                    return false;
                }

                var lista1 = _uiapp.GetRibbonPanels("Diseño de Losas").SelectMany(pa => pa.GetItems()).First(it => it.Name == "BordeBarra");


                IVisibilidadView visibilidad = VisibilidadView.Creador_Visibilidad(view_Visualizar, BuiltInCategory.OST_PathReinBoundary, "Boundary");

                bool result = visibilidad.EstadoActualHide();
                if (visibilidad.EstadoActualHide())
                    visibilidad.CambiarVisibilityBuiltInCategory();
                lista1.ItemText = (!visibilidad.EstadoActualHide() ? "On" : "Off");
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }


    }


}
