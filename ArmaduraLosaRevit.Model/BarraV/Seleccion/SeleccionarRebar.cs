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

namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{
   public class SeleccionarRebar
    {
        private readonly UIApplication _uiapp;
        private UIDocument _uidoc;
        public Rebar _Rebar { get;  set; }
        public XYZ PuntoSeleccionMouse { get; set; }
        public SeleccionarRebar(UIApplication uiapp )
        {
            this._uiapp = uiapp;
            _uidoc = _uiapp.ActiveUIDocument;
        }



        public bool GetSelecionarRebar()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
    
            ISelectionFilter filtroRebarTag = new FiltroRebar();

            Reference referen;

            try
            {
                referen = _uidoc.Selection.PickObject(ObjectType.Element, filtroRebarTag, "Seleccionar el tag de la barra -Element");

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
            if (r is Rebar)
                _Rebar = r as Rebar;

            return true;
        }
    }
}
