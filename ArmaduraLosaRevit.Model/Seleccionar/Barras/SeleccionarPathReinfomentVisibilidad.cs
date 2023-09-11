using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{

    /// <summary>
    /// clase para selecconar objetos pathreinforment en vista
    /// -para ocultar y desocultar
    /// </summary>
    public class SeleccionarPathReinfomentVisibilidad
    {
        private readonly UIApplication _uiapp;

        private bool _isConsiderarTodasLasElevaciones;
        private UIDocument _uidoc;
        private Document _doc;
        private View _view;

        //lista que se creo por mostrar por orietacion y si es primaria o secundaria.
        //esta lista guarda los pathreinforme que ya esta visibles para ocultarlos 
        public List<Element> _lista_A_DeRebarInSystemNivelActual_preOcultar { get; set; }
        public List<Element> _lista_A_DePathReinfNivelActual { get; set; }
        public List<Element> _lista_A_DeRebarInSystem { get; set; }

        public List<Element> _lista_A_DeRebar { get; set; }
        public List<ElementoPath> _lista_A_VisibilidadElementoPathDTO { get; set; } // este el la case base de A,B,C 
        public  List<ElementoPathRein> _lista_A_VisibilidadElementoPathReinDTO { get; set; }// elemento creado posteriomente, deberia reemplzar a  '_lista_A_VisibilidadElementoPathDTO' q es incmpleto

        // public List<Element> _lista_B_DePathSymbolNivelActual { get; set; }
        public  List<WrapperPathSymbol> _lista_B_DePathSymbolNivelActual { get; set; }
        public  List<WrapperTagPath> _lista_C_DePathTagNivelActual { get; set; }
        public DireccionVisualizacion _DireccionVisualizacion { get; set; }
        public OrientacionVisualizacion OrientacionVisualizacion { get; set; }

        private FilteredElementCollector _collectorPathReinfNivelActual;


        public SeleccionarPathReinfomentVisibilidad(UIApplication _uiapp,View _view, bool IsConsiderarTodasLasElevaciones = false)
        {
            this._uiapp = _uiapp;
     
            this._isConsiderarTodasLasElevaciones = IsConsiderarTodasLasElevaciones;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = this._uidoc.Document;
            this._view = _view;

            if (_view is View3D)
                _isConsiderarTodasLasElevaciones = true;

            _lista_A_DeRebarInSystemNivelActual_preOcultar = new List<Element>();
            _lista_A_DePathReinfNivelActual = new List<Element>();
            _lista_A_DeRebarInSystem = new List<Element>();
            _lista_A_DeRebar = new List<Element>();
            _lista_A_VisibilidadElementoPathDTO = new List<ElementoPath>();
            _lista_A_VisibilidadElementoPathReinDTO = new List<ElementoPathRein>(); 
            _lista_B_DePathSymbolNivelActual = new List<WrapperPathSymbol>();
            _lista_C_DePathTagNivelActual = new List<WrapperTagPath>();
            _DireccionVisualizacion = DireccionVisualizacion.Ambos;
            OrientacionVisualizacion = OrientacionVisualizacion.Ambos;
        }
                
        #region EjecutarBusqueda

        public bool M1_ejecutar()
        {
            try
            {
                _lista_A_DeRebarInSystemNivelActual_preOcultar.Clear();
                _lista_A_DePathReinfNivelActual.Clear();
                _lista_A_DeRebarInSystem.Clear();

                _lista_A_VisibilidadElementoPathDTO.Clear(); // 
                _lista_A_VisibilidadElementoPathReinDTO.Clear();
                _lista_B_DePathSymbolNivelActual.Clear();
                _lista_C_DePathTagNivelActual.Clear();

                SeleccionarPathReinfomentVisibilidadStatic._lista_A_VisibilidadElementoPathReinDTO.Clear ();
                SeleccionarPathReinfomentVisibilidadStatic._lista_B_DePathSymbolNivelActual.Clear();
                SeleccionarPathReinfomentVisibilidadStatic._lista_C_DePathTagNivelActual.Clear();

                M1_1_buscarListaPathReinforcement_EnVistaActual();
                M1_1b_buscarListaPathRein_RebarInSystem_EnVistaActual();
                //M1_2_ObtenerListaRebarInSystem();
                M1_3_buscarListaPathSymbolEnVistaActual();
                M1_4_buscarListaPathTagEnVistaActual();

                M1_2_ObtenerListaPathRein_conPathSymbol_yTag();

                SeleccionarPathReinfomentVisibilidadStatic._lista_A_VisibilidadElementoPathReinDTO= _lista_A_VisibilidadElementoPathReinDTO;
                SeleccionarPathReinfomentVisibilidadStatic._lista_B_DePathSymbolNivelActual = _lista_B_DePathSymbolNivelActual;
                SeleccionarPathReinfomentVisibilidadStatic._lista_C_DePathTagNivelActual = _lista_C_DePathTagNivelActual;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }



        private void M1_1_buscarListaPathReinforcement_EnVistaActual()
        {

            _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document, _view.Id);
            //para las familias en el para las instacias de familia en el plano
            _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsNotElementType();
            _lista_A_DePathReinfNivelActual = _collectorPathReinfNivelActual.ToElements() as List<Element>;
            _lista_A_DePathReinfNivelActual = _lista_A_DePathReinfNivelActual
                                            .Where(pt => AyudaSeleccionRebar_PathRein.PerteneceAView_PathAndRebarInSystem(pt, _view, _isConsiderarTodasLasElevaciones))
                                            .ToList();



            if (_DireccionVisualizacion != DireccionVisualizacion.Ambos)
            {

                string TipoAresaltar = (_DireccionVisualizacion.ToString() == "Uno" ? ConstNH.NOMBRE_BARRA_PRINCIPAL : ConstNH.NOMBRE_BARRA_SECUNADARIA);
                _lista_A_DePathReinfNivelActual =
                    _lista_A_DePathReinfNivelActual.Where(pt => AyudaSeleccionRebar_PathRein.EsdireccionIzqDere(pt, "TipoDireccionBarra", TipoAresaltar, TipoAresaltar))
                                                        .ToList();
            }


            if (OrientacionVisualizacion != OrientacionVisualizacion.Ambos)
            {

                if (OrientacionVisualizacion.ToString() == "H")
                {
                    _lista_A_DePathReinfNivelActual =
                    _lista_A_DePathReinfNivelActual.Where(pt => AyudaSeleccionRebar_PathRein.EsdireccionIzqDere(pt, "IDTipoDireccion", "Derecha", "Izquierda"))
                                            .ToList();
                }
                else
                {
                    _lista_A_DePathReinfNivelActual =
                      _lista_A_DePathReinfNivelActual.Where(pt => AyudaSeleccionRebar_PathRein.EsdireccionIzqDere(pt, "IDTipoDireccion", "Superior", "Inferior"))
                                                     .ToList();
                }
            }

        }



        private void M1_1b_buscarListaPathRein_RebarInSystem_EnVistaActual()
        {

            _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document, _view.Id);
            //para las familias en el para las instacias de familia en el plano
            _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsNotElementType();
            _lista_A_DeRebarInSystemNivelActual_preOcultar = _collectorPathReinfNivelActual.ToElements() as List<Element>;
            _lista_A_DeRebarInSystemNivelActual_preOcultar = _lista_A_DeRebarInSystemNivelActual_preOcultar
                                            .Where(pt => AyudaSeleccionRebar_PathRein.PerteneceAView_PathAndRebarInSystem(pt, _view, _isConsiderarTodasLasElevaciones))
                                            .ToList();




        }


        //NOTA RUTINA MUY IMPORTANTE : crea una lista con un objeto 'ElementoPathRein' que contiene los 3 elmentos, pathrein,path symbl y el tag
        private void M1_2_ObtenerListaPathRein_conPathSymbol_yTag()
        {
            _lista_A_VisibilidadElementoPathDTO.Clear();
            _lista_A_VisibilidadElementoPathReinDTO.Clear();

            foreach (var ee in _lista_A_DePathReinfNivelActual)
            {
                if (!(ee is PathReinforcement)) continue;

                PathReinforcement _createdPathReinforcement = (PathReinforcement)ee;
                if (!_createdPathReinforcement.IsValidObject) continue;

                //1.1)obtener valor de pathSymbol
                WrapperPathSymbol _WrapperPathSymbol = _lista_B_DePathSymbolNivelActual.Where(ps => ps.idPathReinf == _createdPathReinforcement.Id.IntegerValue).Select(cc => cc).FirstOrDefault();
                if (_WrapperPathSymbol == null)
                {
                    ConstNH.sbLog.AppendLine($"-------- Pathreinforment :{_createdPathReinforcement.Id.IntegerValue}   no se encontro pathsymbol");
                    continue;
                }
                //1.2)obtener tagsymbol
                List<WrapperTagPath> _WrapperListPathTag = _lista_C_DePathTagNivelActual.Where(ps => ps.idPathReinf == _createdPathReinforcement.Id.IntegerValue).Select(cc => cc).ToList();
                if (_WrapperListPathTag == null)
                {
                    ConstNH.sbLog.AppendLine($"-------- Pathreinforment :{_createdPathReinforcement.Id.IntegerValue}   no se encontro listaTag");
                    continue;
                }

                //2 asignar pathSymbol y  tagsymbol a 'ElementoPathRein'
                ElementoPathRein visibilidadElementoPathDTO = ElementoPathRein.ObtenerElementoPathRein(_doc,
                                                                                                                _createdPathReinforcement,
                                                                                                                (_WrapperPathSymbol != null ? _WrapperPathSymbol.element : null),
                                                                                                                _WrapperListPathTag.Select(cc => cc.element).ToList());
                _lista_A_VisibilidadElementoPathDTO.Add(visibilidadElementoPathDTO);
                _lista_A_VisibilidadElementoPathReinDTO.Add(visibilidadElementoPathDTO);

                //3.1asignar al pathsymbol , el tipo de path
                if (_WrapperPathSymbol != null) _WrapperPathSymbol.TipoPath = visibilidadElementoPathDTO.TipoBarra;
                //3.2asignar al pathsymbol , el tipo de PathTag
                if (_WrapperListPathTag.Count > 0) _WrapperListPathTag = _WrapperListPathTag.Select(s => { s.TipoPath = visibilidadElementoPathDTO.TipoBarra; return s; }).ToList();
            }

            _lista_A_DeRebarInSystem = _lista_A_VisibilidadElementoPathReinDTO.SelectMany(vv => vv._lista_A_DeRebarInSystem).ToList();
        }
        private void M1_4_buscarListaPathTagEnVistaActual()
        {

            _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document, _view.Id);
            //para las familias en el para las instacias de familia en el plano
            _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_PathReinTags).WhereElementIsNotElementType();

            //  _lista_C_DePathTagNivelActual = _collectorPathReinfNivelActual.ToElements() as List<Element>;
            _lista_C_DePathTagNivelActual = _collectorPathReinfNivelActual.Select(c =>
                                                   new WrapperTagPath()
                                                   {
                                                       element = c,
                                                       idPathReinf = (c as IndependentTag).Obtener_GetTaggedLocalElementID().IntegerValue
                                                   }).ToList();

            Debug.WriteLine($" View :{_uidoc.Document.ActiveView.Name}    tagRein encontrado :{_lista_C_DePathTagNivelActual.Count}");

            // var _collectorPathReinfNivelActual_indtag = new FilteredElementCollector(_uidoc.Document, _uidoc.Document.ActiveView.Id);
            //  _collectorPathReinfNivelActual_indtag.OfClass(typeof(IndependentTag)).WhereElementIsNotElementType();
            //   var _lista_C_DePathTagNivelActual2 = _collectorPathReinfNivelActual_indtag.ToElements() as List<Element>;

        }


        private void M1_3_buscarListaPathSymbolEnVistaActual()
        {

            _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document, _view.Id);
            //para las familias en el para las instacias de familia en el plano
            _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_PathReinSpanSymbol).WhereElementIsNotElementType();

            _lista_B_DePathSymbolNivelActual = _collectorPathReinfNivelActual.Select(c =>
                                                        new WrapperPathSymbol()
                                                        {
                                                            element = c,
                                                            idPathReinf = (c as PathReinSpanSymbol).Obtener_GetTaggedLocalElementID().IntegerValue
                                                        }).ToList();
        }

        #endregion
        
        public List<ElementoPath> M2_ObtenerElemntosConPAthSymbol_Visible()
        {
            List<ElementoPath> listaREsul = _lista_A_VisibilidadElementoPathDTO.
                                                Where(cc => M2_1_ISDentrolistaPathSymbolNivelVisible(cc)).ToList();
            return listaREsul;
        }
        public List<ElementoPathRein> M2_ObtenerElemntosConPAthRe_inVisible()
        {
            List<ElementoPathRein> listaREsul = _lista_A_VisibilidadElementoPathReinDTO.
                                                Where(cc => M2_1_ISDentrolistaPathSymbolNivelVisible(cc)).ToList();
            return listaREsul;
        }
        public List<ElementoPathRein> M2_ObtenerElemntosConPAthSymbolInvisibleVisible()
        {
            List<ElementoPathRein> listaREsul = _lista_A_VisibilidadElementoPathReinDTO.
                                                    Where(cc => !M2_1_ISDentrolistaPathSymbolNivelVisible(cc)).
                                                    Select(p => p as ElementoPathRein).ToList();
            return listaREsul;
        }
        private bool M2_1_ISDentrolistaPathSymbolNivelVisible(ElementoPath cc)
        {
            if (!(cc is ElementoPathRein)) return false;

            var ccpath = cc as ElementoPathRein;
            bool result = _lista_B_DePathSymbolNivelActual.Exists(ee => ee.idPathReinf == ccpath._pathReinforcement.Id.IntegerValue);
            return result;
        }

   

    }
}
