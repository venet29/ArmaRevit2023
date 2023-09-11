using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Seleccionar.Ayuda;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Plumbing;
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
    public class SeleccionarPipes
    {
        private UIApplication _uiapp;
        private Document _doc;
        private UIDocument _uidoc;
        private View _view;
        private IList<Element> _listaElementsTagRebarSeleccionado;

        //  private IList<Element> _listaElementsTagRebarSeleccionado;
        public XYZ ptoMouseSObreElemento { get; set; }
        public XYZ ptoMouseCodo { get; set; }
        public XYZ ptoMouseFinal { get; set; }
        public List<Pipe> ListPipes { get; set; }

        public XYZ _direccionMoverTag { get; set; }
        public Element ElementoSeleccionado { get;  set; }
        public Pipe PipeContenedor_ { get;  set; }

        public SeleccionarPipes(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            ListPipes = new List<Pipe>();

        }
        public bool Ejecutar_codo_punto()
        {
            if (!SeleccionarPipes_()) return false;
            if (!SeleccionarPipesContenedorTAg()) return false;
          

            ElementoSeleccionado = PipeContenedor_;

            if (!SelecPto.SeleccionarDireccionMouse(_uidoc, ObjectSnapTypes.None, "3)Seleccionar punto de codo")) return false;
            ptoMouseCodo = SelecPto.ptoMouse;

            ptoMouseFinal = XYZ.Zero;
            if (SelecPto.SeleccionarDireccionMouse(_uidoc, ObjectSnapTypes.None, "4) Seleccionar punto de tag"))
                ptoMouseFinal= SelecPto.ptoMouse;

            ptoMouseCodo = new XYZ(ptoMouseCodo.X, ptoMouseFinal.Y-Util.CmToFoot(1.9), ptoMouseCodo.Z);

            return true;
        }

        public bool Ejecutar_puntoParaTexto()
        {
            if (!SeleccionarPipes_()) return false;
           // if (!SeleccionarPipesContenedorTAg()) return false;


           // ElementoSeleccionado = PipeContenedor_;

            //if (!SelecPto.SeleccionarDireccionMouse(_uidoc, ObjectSnapTypes.None, "3)Seleccionar punto de codo")) return false;
            //ptoMouseCodo = SelecPto.ptoMouse;

            ptoMouseFinal = XYZ.Zero;
            if (!SelecPto.SeleccionarDireccionMouse(_uidoc, ObjectSnapTypes.None, "2) Seleccionar punto de tag")) return false;

                ptoMouseFinal = SelecPto.ptoMouse;
            //ptoMouseCodo = new XYZ(ptoMouseCodo.X, ptoMouseFinal.Y - Util.CmToFoot(1.9), ptoMouseCodo.Z);

            return true;
        }



        private bool SeleccionarPipes_()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                ISelectionFilter f = new FiltroPipes();


                bool IsSeleccion1a1 = true;
                if (IsSeleccion1a1)
                {
                    var refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element, f, "1)Seleccionar Pipes");

                    if (refs_pickobjects == null) return false;
                    if (refs_pickobjects.Count == 0) return false;

                    foreach (Reference ref_elem in refs_pickobjects)
                    {
                        Element item = _uidoc.Document.GetElement(ref_elem.ElementId);
                        if (item is Pipe)
                            ListPipes.Add(item as Pipe);
                    }
                }
                else
                {
                    _listaElementsTagRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "1)Seleccionar Pipes");
                    if (_listaElementsTagRebarSeleccionado.Count == 0) return false;

                    foreach (var item in _listaElementsTagRebarSeleccionado)
                    {
                        if (item is Pipe)
                            ListPipes.Add(item as Pipe);
                    }
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


        private bool SeleccionarPipesContenedorTAg()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                ISelectionFilter f = new FiltroPipes();

                Reference DuctoContenedorRefer_ = _uidoc.Selection.PickObject(ObjectType.Element, f, "2)Seleccionar Pipe contenedor del tag:");
                //selecciona un objeto floor
                //_PtoInicioBaseBordeMuro = DuctoContenedor_.GlobalPoint;
                PipeContenedor_ = _doc.GetElement(DuctoContenedorRefer_.ElementId) as Pipe;
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
