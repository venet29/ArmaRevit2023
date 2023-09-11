using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Seleccionar.Ayuda;
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
    public class SeleccionarTagRebarVerticales
    {
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private View _view;
        private List<Element> _listaElementsTagRebarSeleccionado;
        public XYZ ptoMouse { get; set; }
        public List<IndependentTag> ListIndependentTag { get; set; }

        public XYZ _direccionMoverTag { get; set; }

        public SeleccionarTagRebarVerticales(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            ListIndependentTag = new List<IndependentTag>();

        }
        public bool M1_EjecutarVertical()
        {
            if (!M1a_SeleccionarTagRebar("rectagulo")) return false;

            if (!SelecPto.SeleccionarDireccionMouse(_uidoc, ObjectSnapTypes.None, "Seleccionar  punto de codo de leader.")) return false;

            ptoMouse= SelecPto.ptoMouse;
            return true;
        }

        private bool M1a_SeleccionarTagRebar(string tipoSeleccion="rectagulo")
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                ISelectionFilter f = new FiltroRebarTagAgrupar();
                _listaElementsTagRebarSeleccionado = new List<Element>();
                //selecciona un objeto floor
                if (tipoSeleccion == "rectagulo")
                {
                    _listaElementsTagRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar TagRebar").ToList();
                }
                else
                {
                    f = new FiltroRebarTagHorizontalAgrupar();
                   var refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element, f, "Seleccionar TagRebar");


                    if (refs_pickobjects == null) return false;
                    if (refs_pickobjects.Count == 0) return false;

                    foreach (Reference ref_elem in refs_pickobjects)
                    {
                        Element Element_pickobject_element = _uidoc.Document.GetElement(ref_elem.ElementId);

                        if ((Element_pickobject_element is IndependentTag))
                            _listaElementsTagRebarSeleccionado.Add(Element_pickobject_element);          
                    }
                }

                if (_listaElementsTagRebarSeleccionado.Count == 0) return false;

                foreach (var item in _listaElementsTagRebarSeleccionado)
                {
                    if (item is IndependentTag)
                        ListIndependentTag.Add(item as IndependentTag);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
                return false;
            }
            return true;
            // if (_listaElementsRebarSeleccionado.Count > 0) ObtenerListaDeREbarNivelActualS();
        }


        public bool EjecutarHorizontal()
        {
            if (!M1a_SeleccionarTagRebar("Individual")) return false;

            if (!SelecPto.SeleccionarDireccionMouse(_uidoc, ObjectSnapTypes.None, " Seleccionar  punto de codo de leader.")) return false;

            ptoMouse = SelecPto.ptoMouse;
            return true;
        }


    }
}
