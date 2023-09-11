using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    public class SeleccionarPathSymbol
    {
        protected UIApplication _uiapp;
        protected UIDocument _uidoc;
        protected Document _doc;

        public XYZ PuntoSeleccionMouse { get; private set; }
        public PathReinSpanSymbol PathReinforcementSymbol { get; private set; }
        public XYZ UbicacionPathReinforcementSymbol { get; private set; }
        public SeleccionarPathSymbol(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
        }



        public bool Seleccionados1Path()
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new PathReinSymbolSelectionFilter();
                //selecciona un objeto floor
                Reference referen;
                try
                {
                    referen = _uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar path");
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    referen = null;
                }

                if (referen == null) return false;
                PuntoSeleccionMouse = Util.PtoDeLevelDeGlobalPoint(referen.GlobalPoint, _uidoc.Document);

                Element PathReinSymbol = _uidoc.Document.GetElement(referen);
                // si refere3ncia es null salir
                if (PathReinSymbol == null) return false;

                if (!(PathReinSymbol is PathReinSpanSymbol)) { return false; ; }

                PathReinforcementSymbol = PathReinSymbol as PathReinSpanSymbol;

                UbicacionPathReinforcementSymbol = PathReinforcementSymbol.TagHeadPosition;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
    }
}
