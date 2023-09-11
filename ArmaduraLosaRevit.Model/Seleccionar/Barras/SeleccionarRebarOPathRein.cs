using ApiRevit.FILTROS;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    public class SeleccionarRebarOPathRein
    {

        private UIDocument _uidoc;
        private UIApplication _uiapp;
        public List<Element> ListaBarras { get; set; }

        public List<Element> ListaBarrasInSystem { get; set; }

        public SeleccionarRebarOPathRein(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this.ListaBarras = new List<Element>();
            this.ListaBarrasInSystem = new List<Element>();

        }


        public bool GetListaRebarPathRein()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new FiltroRebar_PathRein();

           //selecciona un objeto floor

            try
            {
                 var refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element, f, "Seleccionar barra");


                if (refs_pickobjects == null) return false;
                if (refs_pickobjects.Count == 0) return false;

                foreach (Reference ref_elem in refs_pickobjects)
                {
                    Element Element_pickobject_element = _uidoc.Document.GetElement(ref_elem.ElementId);

                    if (!((Element_pickobject_element is Rebar) || (Element_pickobject_element is PathReinforcement))) continue;

                    ListaBarras.Add(Element_pickobject_element);
                }
                //  PuntoSeleccionMouse = Util.PtoDeLevelDeGlobalPoint(referen.GlobalPoint, _uidoc.Document);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool GetListaRebarPathReinV2()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
        

            //ILogicalCombinationFilter f2 = SelFilter.GetLogicalAndFilter(new FiltroRebar(), new FiltroRebar_PathRein());

            ISelectionFilter f = new FiltroRebar_PathRein();
            //selecciona un objeto floor

            try
            {
                var refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element, f, "Seleccionar barra");


                if (refs_pickobjects == null) return false;
                if (refs_pickobjects.Count == 0) return false;

                foreach (Reference ref_elem in refs_pickobjects)
                {
                    Element Element_pickobject_element = _uidoc.Document.GetElement(ref_elem.ElementId);

                    if ((Element_pickobject_element is Rebar) || (Element_pickobject_element is PathReinforcement))
                        ListaBarras.Add(Element_pickobject_element);
                    else if (!(Element_pickobject_element is RebarInSystem))
                        ListaBarrasInSystem.Add(Element_pickobject_element);
                }
                //  PuntoSeleccionMouse = Util.PtoDeLevelDeGlobalPoint(referen.GlobalPoint, _uidoc.Document);

                _uidoc.Selection.SetElementIds(ListaBarras.Select(c=>c.Id).ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
