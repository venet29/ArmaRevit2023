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

namespace ArmaduraLosaRevit.Model.GRIDS.NombreEje
{
   public class SeleccionarGrid
    {
        private readonly UIApplication _uiapp;
        private UIDocument _uidoc;
        public Grid _grids { get;  set; }
        public XYZ PuntoSeleccionMouse { get; set; }
        public SeleccionarGrid(UIApplication uiapp )
        {
            this._uiapp = uiapp;
            _uidoc = _uiapp.ActiveUIDocument;
        }



        public bool GetSelecionarGrid()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
    
            ISelectionFilter filtroGrids = new FiltroGrids();

            Reference referen;

            try
            {
                referen = _uidoc.Selection.PickObject(ObjectType.Element, filtroGrids, "Seleccionar el eje");

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        
            if (referen == null) return false;

             PuntoSeleccionMouse = Util.PtoDeLevelDeGlobalPoint(referen.GlobalPoint, _uidoc.Document);

            Element r = _uidoc.Document.GetElement(referen);
            // si refere3ncia es null salir
            if (r == null) return false;

            //obtiene una referencia floor con la referencia r
            if (r is Grid)
                _grids = r as Grid;

            return true;
        }
    }
}
