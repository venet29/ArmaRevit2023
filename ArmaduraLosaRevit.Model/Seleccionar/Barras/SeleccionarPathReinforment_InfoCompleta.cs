using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Copiar.model;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    public class SeleccionarPathReinforment_InfoCompleta
    {

        private UIDocument _uidoc;
        private Document _doc;
        private ElementoPathRein _elementoPathRein;

        public List<ElementoPathRein> ListaElementoPathRein { get;  set; }

        public SeleccionarPathReinforment_InfoCompleta(UIApplication uiapp)
        {
            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;

            ListaElementoPathRein = new List<ElementoPathRein>();
          //  _tiposRebarTagsEnView = new TiposRebarTagsEnView(_doc, _doc.ActiveView);
        }



        public bool GenerarListaCopiar_PathReinforment()
        {
            try
            {
           //     _tiposRebarTagsEnView.cargarListaDetag();

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
   
                    Element elem = _doc.GetElement(elemId);
                    if (elem == null) continue;
                    PathReinSpanSymbol _independentTag = elem as PathReinSpanSymbol;
                    if (_independentTag == null) continue;
                
                    _elementoPathRein = ElementoPathRein.ObtenerElementoPathReinConElement(_doc, elemId);
                    if(_elementoPathRein!=null)
                        ListaElementoPathRein.Add(_elementoPathRein);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"error al Generar Lista Copiar \n  ex: {ex.Message}");
                return false;
            }
            return true;
        }


        public bool GenerarListaSeleccionado()
        {
            try
            {
                //     _tiposRebarTagsEnView.cargarListaDetag();

                List<Element> refs_pickobjects = null;
                try
                {
                    // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                    ISelectionFilter f = new PathReinSymbolSelectionFilter();
                    //selecciona un objeto floor
                    refs_pickobjects = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar (pathreinforment) barras ").ToList();
                    //   refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element,f, "SELECCION PICKOBJECTS: SELECCIONA UNO o VARIOS");
                   // refs_pickobjects = _uidoc.Selection.().ToList();
                }
                catch (Exception)
                {
                    return false;
                }

                if (refs_pickobjects == null) return false;
                if (refs_pickobjects.Count == 0) return false;

                foreach (Element elemId in refs_pickobjects)
                {
                    _elementoPathRein = ElementoPathRein.ObtenerElementoPathReinConPathReinSpanSymbol(_doc, elemId);
                    if (_elementoPathRein != null) 
                        ListaElementoPathRein.Add(_elementoPathRein);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"error al Generar Lista Copiar \n  ex: {ex.Message}");
                return false;
            }
            return true;
        }
    }
}