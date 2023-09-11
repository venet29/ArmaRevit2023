using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    public class SeleccionarRebarVisibilidad
    {

        private UIDocument _uidoc;
        private View _view;
#pragma warning disable CS0414 // The field 'SeleccionarRebarVisibilidad._isConsiderarTodasLasElevaciones' is assigned but its value is never used
        private bool _isConsiderarTodasLasElevaciones;
#pragma warning restore CS0414 // The field 'SeleccionarRebarVisibilidad._isConsiderarTodasLasElevaciones' is assigned but its value is never used

        private UIApplication _uiapp;
        private Document _doc;
        private readonly View _viewObtenerBArras;
        public DireccionVisualizacion _DireccionVisualizacion { get; set; }
        public OrientacionVisualizacion OrientacionVisualizacion { get; set; }

        public List<WrapperRebar> _lista_A_DeRebarVistaActualElevacion { get; set; }

        public List<Element> _lista_A_RebarNivelActual_ConrebarSystem { get; set; }

        public List<Element> _lista_A_TodasRebarNivelActual_MENOSRebarSystem { get; set; }

        public List<Element> _lista_A_TodosRebarNivelActual { get; set; }

        private List<Element> _lista_A_RebarTAgNivelActual;

        public List<ElementoPath> _lista_A_VisibilidadElementoREbarDTO { get; set; }
        public List<ElementoPath> _lista_A_VisibilidadElementoREbarDTO_preseleccionado { get; set; }
        public List<WrapperTagPath> _lista_C_DeRebarTagNivelActual { get; set; }

        private FilteredElementCollector _collectorRebarViewActual;


        private FilteredElementCollector _collectorPathReinfNivelActual;

        public SeleccionarRebarVisibilidad(UIApplication _uiapp, View _viewObtenerBArras)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._viewObtenerBArras = _viewObtenerBArras;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._view = this._uidoc.Document.ActiveView;

            if (_view is View3D)
                _isConsiderarTodasLasElevaciones = true;

            _lista_A_RebarNivelActual_ConrebarSystem = new List<Element>();
            _lista_A_RebarTAgNivelActual = new List<Element>();

            _lista_A_TodasRebarNivelActual_MENOSRebarSystem = new List<Element>();

            _lista_A_VisibilidadElementoREbarDTO = new List<ElementoPath>();
            _lista_A_VisibilidadElementoREbarDTO_preseleccionado = new List<ElementoPath>();
            _lista_C_DeRebarTagNivelActual = new List<WrapperTagPath>();
            _lista_A_TodosRebarNivelActual = new List<Element>();
        }




        public bool M1_Ejecutar_ConrebarSystem()
        {
            try
            {
                _lista_A_VisibilidadElementoREbarDTO.Clear();
                _lista_A_RebarNivelActual_ConrebarSystem.Clear();
                _lista_C_DeRebarTagNivelActual.Clear();
                M1_1_BuscarListaRebarEnVistaActual();
                if (!M1_2_buscarListaTagEnVistaActual()) return false;
                if (!M1_3_ObtenerListaRebar(_lista_A_RebarNivelActual_ConrebarSystem)) return false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool M1_Ejecutar_NoRebarSystem()
        {
            try
            {
                _lista_A_VisibilidadElementoREbarDTO.Clear();
                _lista_A_RebarNivelActual_ConrebarSystem.Clear();
                _lista_C_DeRebarTagNivelActual.Clear();
                M1_1_BuscarListaRebarEnVistaActual();
                if (!M1_2_buscarListaTagEnVistaActual()) return false;
                if (!M1_3_ObtenerListaRebar(_lista_A_TodasRebarNivelActual_MENOSRebarSystem)) return false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool M1_EjecutarPreSeleccion()
        {
            try
            {
                List<ElementId> refs_pickobjects = null;
                try
                {
                    ISelectionFilter f = new RebarUnicamenteSelectionFilter();
                    //   refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element,f, "SELECCION PICKOBJECTS: SELECCIONA UNO o VARIOS");
                    refs_pickobjects = _uidoc.Selection.GetElementIds().ToList();
                }
                catch (Exception)
                {
                    return false;
                }
                if (refs_pickobjects == null) return false;
                if (refs_pickobjects.Count == 0) return false;

                if (!M1_Ejecutar_NoRebarSystem()) return false;

                foreach (ElementId elemId in refs_pickobjects)
                {

                    ElementoPath resultElementoPath = _lista_A_VisibilidadElementoREbarDTO.Where(c => ((ElementoPathRebar)c)._rebar.Id.IntegerValue == elemId.IntegerValue).FirstOrDefault();

                    if (resultElementoPath == null) continue;
                    _lista_A_VisibilidadElementoREbarDTO_preseleccionado.Add(resultElementoPath);
                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        public bool M1_1_BuscarListaRebarEnVistaActual()
        {
            try
            {
                _collectorRebarViewActual = new FilteredElementCollector(_uidoc.Document, _viewObtenerBArras.Id);
                //para las familias en el para las instacias de familia en el plano
                _collectorRebarViewActual.OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsNotElementType();




                _lista_A_RebarNivelActual_ConrebarSystem = _collectorRebarViewActual.ToElements() as List<Element>;

                _lista_A_TodasRebarNivelActual_MENOSRebarSystem = _lista_A_RebarNivelActual_ConrebarSystem
                                                .Where(pt => AyudaSeleccionRebar_PathRein.PerteneceAView_TodasRebar_MenosSystem(pt, _view))
                                                .ToList();

                _lista_A_RebarNivelActual_ConrebarSystem = _lista_A_RebarNivelActual_ConrebarSystem
                                                .Where(pt => AyudaSeleccionRebar_PathRein.PerteneceAView_Rebar(pt, _view))
                                                .ToList();
                _lista_A_TodosRebarNivelActual.AddRange(_lista_A_RebarNivelActual_ConrebarSystem);

                if (_DireccionVisualizacion != DireccionVisualizacion.Ambos)
                {

                    string TipoAresaltar = (_DireccionVisualizacion.ToString() == "Uno" ? ConstNH.NOMBRE_BARRA_PRINCIPAL : ConstNH.NOMBRE_BARRA_SECUNADARIA);
                    _lista_A_RebarNivelActual_ConrebarSystem =
                        _lista_A_RebarNivelActual_ConrebarSystem.Where(pt => AyudaSeleccionRebar_PathRein.EsdireccionIzqDere(pt, "TipoDireccionBarra", TipoAresaltar, TipoAresaltar))
                                                            .ToList();
                }


                if (OrientacionVisualizacion != OrientacionVisualizacion.Ambos)
                {

                    if (OrientacionVisualizacion.ToString() == "H")
                    {
                        _lista_A_RebarNivelActual_ConrebarSystem =
                        _lista_A_RebarNivelActual_ConrebarSystem.Where(pt => AyudaSeleccionRebar_PathRein.EsdireccionIzqDere(pt, "BarraOrientacion", "Derecha", "Izquierda"))
                                                .ToList();
                    }
                    else
                    {
                        _lista_A_RebarNivelActual_ConrebarSystem =
                          _lista_A_RebarNivelActual_ConrebarSystem.Where(pt => AyudaSeleccionRebar_PathRein.EsdireccionIzqDere(pt, "BarraOrientacion", "Superior", "Inferior"))
                                                         .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool M1_2_buscarListaTagEnVistaActual()
        {
            try
            {
                _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document, _viewObtenerBArras.Id);
                //para las familias en el para las instacias de familia en el plano
                _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_RebarTags).WhereElementIsNotElementType();

                // _lista_A_RebarTAgNivelActual = _collectorPathReinfNivelActual.ToElements() as List<Element>;
                _lista_C_DeRebarTagNivelActual = _collectorPathReinfNivelActual.Select(c =>
                                                       new WrapperTagPath()
                                                       {
                                                           element = c,
                                                           idPathReinf = (c as IndependentTag).Obtener_GetTaggedLocalElementID().IntegerValue
                                                       }).ToList();

                Debug.WriteLine($" View :{_viewObtenerBArras.Name}    tag encontrado :{_lista_C_DeRebarTagNivelActual.Count}");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'Obtener Lista Rebar'  \nex:{ex.Message}");
                return false;
            }
            return true;
        }




        private bool M1_3_ObtenerListaRebar(List<Element> _lista_A_Rebar)
        {
            try
            {


                _lista_A_VisibilidadElementoREbarDTO.Clear();

                foreach (Element ee in _lista_A_Rebar)
                {
                    if (!(ee is Rebar)) continue;

                    Rebar _rebar = (Rebar)ee;
                    if (!_rebar.IsValidObject) continue;

                    //1.1)obtener valor de pathSymbol
                    //1.2)obtener tagsymbol
                    List<WrapperTagPath> _WrapperListPathTag = _lista_C_DeRebarTagNivelActual.Where(ps => ps.idPathReinf == _rebar.Id.IntegerValue).Select(cc => cc).ToList();

                    //2 asignar pathSymbol y  tagsymbol a 'ElementoPathRein'
                    ElementoPathRebar visibilidadElementoPathDTO = ElementoPathRebar.CrearVisibilidadElementoRebarhDTO(_doc, _rebar, _WrapperListPathTag.Select(cc => cc.element).ToList());
                    _lista_A_VisibilidadElementoREbarDTO.Add(visibilidadElementoPathDTO);

                    //3.1asignar al pathsymbol , el tipo de path

                    //3.2asignar al pathsymbol , el tipo de PathTag
                    if (_WrapperListPathTag.Count > 0) _WrapperListPathTag = _WrapperListPathTag.Select(s => { s.TipoPath = visibilidadElementoPathDTO.TipoBarra; return s; }).ToList();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'Obtener Lista Rebar'  \nex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool BuscarListaRebarEnVistaActualElevacion()
        {
            try
            {
                _collectorRebarViewActual = new FilteredElementCollector(_uidoc.Document, _viewObtenerBArras.Id);

                //para las familias en el para las instacias de familia en el plano
                _collectorRebarViewActual.OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsNotElementType();
                var list = _collectorRebarViewActual.OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsNotElementType().ToList();
                _lista_A_DeRebarVistaActualElevacion = _collectorRebarViewActual.Where(c => c is Rebar).Select(c =>
                                                 new WrapperRebar()
                                                 {
                                                     element = c,
                                                     NombreView = AyudaObtenerParametros.ObtenerNombreView(c),
                                                     ObtenerTipoBarra = AyudaObtenerParametros.ObtenerTipoBarraRebra(c),
                                                     IdBarraCopiar = AyudaObtenerParametros.ObtenerIdBarraCopiar(c)
                                                 }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error ex:{ex.Message}");
                return false;
            }
            return true;
        }



    }
}
