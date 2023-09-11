using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.Viewnh
{
    public class CreadorView
    {
   
        private UIApplication _uiapp;
        private Document _doc;
        private Level nivelactual;
        private View vistaactual;
        private ViewFamilyType _familiafloorplan;
        private ViewFamilyType _familia3D;
        private ViewFamilyType _familiaSection;
        private List<string> listaExclusion;

        public static bool IsMje  { get; set; } = true;
        public CreadorView(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document; ;
            Load();
            M1_Obtener3TiposFamilias();
        }
        private void Load()
        {
            vistaactual = _doc.ActiveView;
            if (vistaactual.ViewType != ViewType.FloorPlan) { return; }
            nivelactual = vistaactual.GenLevel;
        }
        private void M1_Obtener3TiposFamilias()
        {
            IList<Element> listatiposdevista = new FilteredElementCollector(_doc).OfClass(typeof(ViewFamilyType)).ToElements();
            _familiafloorplan = null;
            _familia3D = null;
            _familiaSection = null;

            foreach (Element evista in listatiposdevista)
            {
                ViewFamilyType vfamilytype = evista as ViewFamilyType;
                switch (vfamilytype.ViewFamily)
                {
                    case ViewFamily.FloorPlan:
                        _familiafloorplan = vfamilytype;
                        break;
                    case ViewFamily.ThreeDimensional:
                        _familia3D = vfamilytype;
                        break;
                    case ViewFamily.Section:
                        _familiaSection = vfamilytype;
                        break;
                }
            }
        }

        public void M2_CrearVIew3D(string nombre)
        {
            var view3d = SeleccionarView.ObtenerViewPOrNombre(_uiapp.ActiveUIDocument, nombre);
            if (view3d != null)
            {
               if(IsMje) Util.CambiarStadoSectionBo3d(_uiapp.ActiveUIDocument, nombre, false);
                IsMje = false;
                return;
            }

            View3D _Newvista3D = null;
            try
            {
                using (Transaction tr = new Transaction(_doc, "CREAR 3d"))
                {
                    tr.Start();
                    _Newvista3D = View3D.CreateIsometric(_doc, _familia3D.Id);
                    _Newvista3D.Name = nombre;
                    //vista3D.SetSectionBox(cajamuro);

                    _Newvista3D.CropBoxVisible = true;

                    _Newvista3D.DetailLevel = ViewDetailLevel.Coarse;
                    _Newvista3D.DisplayStyle = DisplayStyle.Wireframe;
                    _Newvista3D.Scale = 50;

                    ParameterUtil.SetParaInt(_Newvista3D, BuiltInParameter.VIEWER_ANNOTATION_CROP_ACTIVE, 0);
                    ParameterUtil.SetParaInt(_Newvista3D, BuiltInParameter.VIEWER_CROP_REGION_VISIBLE, 0);
                    ParameterUtil.SetParaInt(_Newvista3D, BuiltInParameter.VIEWER_CROP_REGION, 0);
                    ParameterUtil.SetParaInt(_Newvista3D, BuiltInParameter.VIEWER_MODEL_CLIP_BOX_ACTIVE, 0);
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }

            if (_Newvista3D != null)
            {
                VisibilidadCategorias visibilidadView = new VisibilidadCategorias(_Newvista3D);
                listaExclusion = new List<string>();
                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.Annotation, listaExclusion);

                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.AnalyticalModel, listaExclusion);

                CArgarListaExclusion(nombre);
                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.Model, listaExclusion);
            }
        }


        public void M2_ConfiguracionLosa(View _view)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "ConfiguracionLosa"))
                {
                    tr.Start();
                    //_view.CropBoxVisible = true;
                    _view.DetailLevel = ViewDetailLevel.Coarse;
                    _view.DisplayStyle = DisplayStyle.Wireframe;
                    //_view.ViewTemplateId = new ElementId(-1)
                    ;
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }

        public void M2_ConfiguracionElevacionDibujar(View _view)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "ConfiguracionElevacionExportar"))
                {
                    tr.Start();
                    //_view.CropBoxVisible = true;
                    _view.DetailLevel = ViewDetailLevel.Coarse;
                    _view.DisplayStyle = DisplayStyle.Wireframe;
                    //_view.DisplayStyle = DisplayStyle.Shading;
                    _view.get_Parameter(BuiltInParameter.VIEWER_BOUND_FAR_CLIPPING).Set(2);//clip  whitout line :2 //clip  whit line :1
                 //   _view.Scale = 50;
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }

        public void M2_SinViewTemplate(View _view)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();
                    Parameter par = _view.GetParameter2("View Template");
                    par.Set(new ElementId(-1));
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }

        public void M2_ViewTemplateElevacion(View _view,string name)
        {
            try
            {
                var _viewtemplElev = TiposView.ObtenerTiposView(name, _doc);
                if (_viewtemplElev == null)
                {
                    Util.ErrorMsg($"No se encontro template :{name}");
                    return;
                }

                using (Transaction tr = new Transaction(_doc, "View Template Elevaciones"))
                {
                    tr.Start();
                    Parameter par = _view.GetParameter2("View Template");
                    par.Set(_viewtemplElev.Id);
                    _doc.Regenerate();
                    tr.Commit();
                }

                _uiapp.ActiveUIDocument.RefreshActiveView();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }

        public void M2_ViewTemplateLosa(View _view, string name)
        {
            try
            {
                var _viewtemplElev = TiposView.ObtenerTiposView(name, _doc);

                if (_viewtemplElev == null)
                {
                    _viewtemplElev=TiposView.ObtenerTiposView_queCOntenga(_doc);
                }
                if (_viewtemplElev == null)
                {
                    Util.ErrorMsg($"No se encontro template :{name}");
                    return;
                }

                using (Transaction tr = new Transaction(_doc, "View Template Elevaciones"))
                {
                    tr.Start();
                    Parameter par = _view.GetParameter2("View Template");
                    par.Set(_viewtemplElev.Id);
                    _doc.Regenerate();
                    tr.Commit();
                }

                _uiapp.ActiveUIDocument.RefreshActiveView();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }


        public void M2_CrearVIew(FilteredElementCollector collector1)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "CREAR VISTAS MUROS"))
                {
                    tr.Start();
                    foreach (Element emuro in collector1)
                    {
                        Wall muro = emuro as Wall;
                        BoundingBoxXYZ cajamuro = muro.get_BoundingBox(null);//NULL SIGNIFICA QUE OBTIENE LA CAJA SIN IMPORTAR LA VISTA 3D

                        ViewPlan vistaplanta = ViewPlan.Create(_doc, _familiafloorplan.Id, nivelactual.Id);
                        vistaplanta.Name = "PLANTA MURO " + muro.Id.IntegerValue.ToString();
                        if (vistaplanta.CropBoxActive == false)
                        {
                            vistaplanta.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION).Set(1);
                        }
                        vistaplanta.CropBox = cajamuro;
                        vistaplanta.DetailLevel = ViewDetailLevel.Fine;
                        vistaplanta.DisplayStyle = DisplayStyle.ShadingWithEdges;
                        vistaplanta.Scale = 50;

                        View3D vista3D = View3D.CreateIsometric(_doc, _familia3D.Id);
                        vista3D.Name = "VISTA 3D MURO " + muro.Id.IntegerValue.ToString();
                        vista3D.SetSectionBox(cajamuro);
                        vista3D.DetailLevel = ViewDetailLevel.Fine;
                        vista3D.DisplayStyle = DisplayStyle.ShadingWithEdges;
                        vista3D.Scale = 50;

                        ViewSection vistaSeccion = ViewSection.CreateSection(_doc, _familiaSection.Id, cajamuro);
                        vistaSeccion.Name = "SECCION " + muro.Id.IntegerValue.ToString();
                        vistaSeccion.DetailLevel = ViewDetailLevel.Fine;
                        vistaSeccion.DisplayStyle = DisplayStyle.ShadingWithEdges;
                        vistaSeccion.Scale = 50;
                    }
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
            }
        }


        public bool Configurar3D_OcultarBarras()
        {
            try
            {
                List<string> listaInclusion = new List<string>() { "Structural Path Reinforcement", "Structural Rebar" };
                
                //VIEW
                List<string> ListaView = new List<string>() { "3D_NoEditarOp2", "3D_NoEditar", "{3D}", "{3D - masteringenieria.cl}", "{3D - delporteing@gmail.com}" };
                bool OcultarBrras = true;
                foreach (string itemview3d in ListaView)
                {
                    View3D view3d_3D_NoEditar = TiposView3D.ObtenerTiposView(itemview3d, _doc);
                    if (view3d_3D_NoEditar == null) continue;
                    VisibilidadCategorias visibilidadView = new VisibilidadCategorias(view3d_3D_NoEditar);
                    visibilidadView.CambiarEstado_ListaAplicar(OcultarBrras, listaInclusion);
                }

                //TEMPLES
                List<string> ListaTemplate = new List<string>() { "TIPO DE HORMIGON", "FILTRO REVISION ESPESORES" };
                foreach (string itemviewTempl in ListaTemplate)
                {
                    View _viewtemplElev = TiposView.ObtenerTiposView(itemviewTempl, _doc);
                    if (_viewtemplElev == null) continue;
                    VisibilidadCategorias visibilidadView = new VisibilidadCategorias(_viewtemplElev);
                    visibilidadView.CambiarEstado_ListaAplicar(OcultarBrras, listaInclusion);
                }

            }
            catch (Exception ex)
            {

                Util.InfoMsg($"Error en 'Configurar3D' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool CambiarDetalle(View vistaSeccion, ViewDetailLevel tipoDetalle, DisplayStyle tipoDisplayStyle)
        {

            if (vistaSeccion.DetailLevel == tipoDetalle && vistaSeccion.DisplayStyle == tipoDisplayStyle) return true;
            try
            {
                using (Transaction tr = new Transaction(_doc, "CREAR VISTAS MUROS"))
                {
                    tr.Start();
                    //  ViewSection vistaSeccion = ViewSection.CreateSection(_doc, _familiaSection.Id, cajamuro);
                    //  vistaSeccion.Name = "SECCION " + muro.Id.IntegerValue.ToString();
                    vistaSeccion.DetailLevel = tipoDetalle;
                    vistaSeccion.DisplayStyle = tipoDisplayStyle;
                    // vistaSeccion.DisplayStyle = DisplayStyle.ShadingWithEdges;
                    //  vistaSeccion.Scale = 50;
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
                return false;
            }
            return true;
        }

        private void CArgarListaExclusion(string nombreview)
        {
            listaExclusion.Add("Columns");
            listaExclusion.Add("Floors"); listaExclusion.Add("Common Edges"); listaExclusion.Add("Hidden Lines"); listaExclusion.Add("Interior Edges"); listaExclusion.Add("Slab Edges");
            listaExclusion.Add("Generic Models"); listaExclusion.Add("Overhead"); listaExclusion.Add("Opening Elevation"); listaExclusion.Add("Hidden Lines");
            listaExclusion.Add("Shaft Openings");

            listaExclusion.Add("Lines");

            listaExclusion.Add("Structural Beam Systems");
            listaExclusion.Add("Structural Framing");
            listaExclusion.Add("Structural Columns");
            if (nombreview == "3D_NoEditar")
            {
                listaExclusion.Add("Structural Rebar");
            }

            listaExclusion.Add("Structural Foundations");
            listaExclusion.Add("Walls");


        }

    }
}
