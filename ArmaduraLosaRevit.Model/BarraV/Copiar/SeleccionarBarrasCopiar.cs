using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Copiar.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar
{

    public class SeleccionarBarrasREbar_InfoComppleta: SeleccionarRebarRectanguloWrapperRebar
    {
#pragma warning disable CS0108 // 'SeleccionarBarrasREbar_InfoComppleta._uiapp' hides inherited member 'SeleccionarRebarRectanguloWrapperRebar._uiapp'. Use the new keyword if hiding was intended.
        private UIApplication _uiapp;
#pragma warning restore CS0108 // 'SeleccionarBarrasREbar_InfoComppleta._uiapp' hides inherited member 'SeleccionarRebarRectanguloWrapperRebar._uiapp'. Use the new keyword if hiding was intended.
        private readonly double _valorZ;
#pragma warning disable CS0108 // 'SeleccionarBarrasREbar_InfoComppleta._uidoc' hides inherited member 'SeleccionarRebarRectanguloWrapperRebar._uidoc'. Use the new keyword if hiding was intended.
        private UIDocument _uidoc;
#pragma warning restore CS0108 // 'SeleccionarBarrasREbar_InfoComppleta._uidoc' hides inherited member 'SeleccionarRebarRectanguloWrapperRebar._uidoc'. Use the new keyword if hiding was intended.
#pragma warning disable CS0108 // 'SeleccionarBarrasREbar_InfoComppleta._doc' hides inherited member 'SeleccionarRebarRectanguloWrapperRebar._doc'. Use the new keyword if hiding was intended.
        private Document _doc;
#pragma warning restore CS0108 // 'SeleccionarBarrasREbar_InfoComppleta._doc' hides inherited member 'SeleccionarRebarRectanguloWrapperRebar._doc'. Use the new keyword if hiding was intended.
        private View _view;
        public List<ElementoRebar_Elev> ListaBarrasRebar_InfoCompleta { get;  set; }

        private TiposRebarTagsEnView _tiposRebarTagsEnView;

        public SeleccionarBarrasREbar_InfoComppleta(UIApplication uiapp, double valorZ=0.0d)
            :base(uiapp,TipoBarraGeneral.Elevacion, "soloElementoViga")
        {
            this._uiapp = uiapp;
            this._valorZ = valorZ;
            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
            this._view = _doc.ActiveView;
    
            ListaBarrasRebar_InfoCompleta = new List<ElementoRebar_Elev>();
            _tiposRebarTagsEnView = new TiposRebarTagsEnView(_uiapp, _doc.ActiveView);
        }

        public bool GenerarLista()
        {
            try
            {                
                _tiposRebarTagsEnView.M0_BuscarRebarTagInView();
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

                foreach (ElementId elemId in refs_pickobjects)
                {
                    Element Element_pickobject_element = _uidoc.Document.GetElement(elemId);

                    if (!(Element_pickobject_element is Rebar)) continue;

                    List<IndependentTag> listaTag = _tiposRebarTagsEnView.M1_BuscarEnColecctorPorRebar(Element_pickobject_element.Id);

                    ElementoRebar_Elev _WrapperBarrasElevaciones_inicial = new ElementoRebar_Elev((Rebar)Element_pickobject_element, listaTag,_view);
                    _WrapperBarrasElevaciones_inicial.ObtenerPArametros(_valorZ);

                    if(_WrapperBarrasElevaciones_inicial.IsOk)
                        ListaBarrasRebar_InfoCompleta.Add(_WrapperBarrasElevaciones_inicial);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"error al Generar Lista Copiar \n  ex: {ex.Message}");
                return false;
            }
            return true;
        }

        public bool SeleccionarBarrasViga_paraCOpiar()
        {
            bool result = true;
            try
            {
                var listaActualBarrasRebarSeleccionadas = _uidoc.Selection.GetElementIds().ToList();
                if (!SeleccionarBarrasViga()) return false;    
                var listaNUevosRebrSelecciondos=ListaWrapperRebarFiltro.Select(c => c.element.Id).ToList();
                listaNUevosRebrSelecciondos.AddRange(listaActualBarrasRebarSeleccionadas);
                _uidoc.Selection.SetElementIds(listaNUevosRebrSelecciondos);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en Seleccionar para copiar ex:{ex.Message}");
                result = false;
            }
            return result;
        }

        public bool SeleccionarBarrasViga()
        {
            bool result = true;
            try
            {
                if (!M1_GetRoomSeleccionadosConRectaguloYFiltros()) return false;
                if (M2_ObtenerListaWrapperRebar())
                {
                    if (tipocaso == TipoBarraGeneral.Elevacion)
                        M3_b_ObtenerListaIDSeleccionadosElevaciones();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en SeleccionarBarrasViga ex:{ex.Message}");
                result = false;
            }
            return result;
        }
    }
}