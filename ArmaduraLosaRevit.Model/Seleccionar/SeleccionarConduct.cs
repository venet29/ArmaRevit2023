using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Seleccionar.Ayuda;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
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
    public class SeleccionarConduct
    {
        private UIApplication _uiapp;
        private Document _doc;
        private UIDocument _uidoc;
        private View _view;
        private IList<Element> _listaElementsTagRebarSeleccionado;
        public XYZ ptoMouseSObreElemento { get; set; }
        public XYZ ptoMouseCodo { get; set; }
        public XYZ ptoMouseFinal { get; set; }
        public List<Conduit> ListConduct { get; set; }

        public XYZ _direccionMoverTag { get; set; }
        public Element ElementoSeleccionado { get;  set; }
        public Conduit DuctoContenedor_ { get;  set; }

        public SeleccionarConduct(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            ListConduct = new List<Conduit>();

        }
        public bool Ejecutar()
        {
            if (!SeleccionarDuctos()) return false;
            if (!SeleccionarDuctoContenedorTAg()) return false;
          

            ElementoSeleccionado = DuctoContenedor_;

            if (!SelecPto.SeleccionarDireccionMouse(_uidoc, ObjectSnapTypes.None, "3)Seleccionar punto de codo")) return false;
            ptoMouseCodo = SelecPto.ptoMouse;

            ptoMouseFinal = XYZ.Zero;
            if (SelecPto.SeleccionarDireccionMouse(_uidoc, ObjectSnapTypes.None, "4) Seleccionar punto de tag"))
                ptoMouseFinal= SelecPto.ptoMouse;

            ptoMouseCodo = new XYZ(ptoMouseCodo.X, ptoMouseFinal.Y-Util.CmToFoot(1.9), ptoMouseCodo.Z);

            return true;
        }

        private bool SeleccionarDuctos()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                ISelectionFilter f = new FiltroConduit();
                //selecciona un objeto floor
                _listaElementsTagRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "1)Seleccionar Ductos");
                if (_listaElementsTagRebarSeleccionado.Count == 0) return false;

                foreach (var item in _listaElementsTagRebarSeleccionado)
                {
                    if (item is Conduit)
                        ListConduct.Add(item as Conduit);
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


        private bool SeleccionarDuctoContenedorTAg()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                ISelectionFilter f = new FiltroConduit();


                Reference DuctoContenedorRefer_ = _uidoc.Selection.PickObject(ObjectType.Element, f, "2)Seleccionar Ducto contenedor del tag:");
                //selecciona un objeto floor
                //_PtoInicioBaseBordeMuro = DuctoContenedor_.GlobalPoint;

                DuctoContenedor_ = _doc.GetElement(DuctoContenedorRefer_.ElementId) as Conduit;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
                return false;
            }
            return true;
            // if (_listaElementsRebarSeleccionado.Count > 0) ObtenerListaDeREbarNivelActualS();
        }
    }
}
