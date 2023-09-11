using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{


    public class SeleccionarOpeningConMouse
    {
        private Document _doc;
        private UIDocument _uidoc;
        private bool isTest;
        // private Floor _selecFloorMouse;
        public XYZ PtoMOuse { get; set; }
        public Opening _OpeningSelecciondo { get; set; }
        public bool _todoBien { get;  set; }

        public SeleccionarOpeningConMouse(UIApplication _UIapp, bool isTest = false)
        {
            this._doc = _UIapp.ActiveUIDocument.Document;
            this._uidoc = _UIapp.ActiveUIDocument;
            this.isTest = isTest;
            _todoBien = true;
        }



        public Opening M1_SelecconaOpening()
        {
            _OpeningSelecciondo = null;
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new JtElementsOfClassSelectionFilter<Opening>();
            //selecciona un objeto floor

            Reference r;
            try
            {
                r = _uidoc.Selection.PickObject(ObjectType.Face, f, "Seleccionar Opening");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                r = null;

            }
            // sirefere3ncia es null salir
            if (r == null) return null;
            //obtiene una referencia floor con la referencia r
            PtoMOuse= r.GlobalPoint;
            _OpeningSelecciondo = _doc.GetElement(r.ElementId) as Opening;
            //obtiene el nivel del la los
            return _OpeningSelecciondo;
        }
        public List<Element> M2_SelecconaShaftOpeningRectagulo()
        {
            _OpeningSelecciondo = null;
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new JtElementsOfClassSelectionFilter<Opening>();
            //selecciona un objeto floor

            List<Element> listaElementos;
            try
            {
                listaElementos = _uidoc.Selection.PickElementsByRectangle( f, "Seleccionar Opening").ToList();
            }

            catch (Exception )

            {
                listaElementos = new List<Element>();

            }
            // sirefere3ncia es null salir
            //var result = typeof(listaElementos[0]);
            //obtiene el nivel del la los
            return listaElementos;
        }
        public bool IsOk()
        {
            return (PtoMOuse == null || _OpeningSelecciondo == null ? false : true);
        }


    }
}
