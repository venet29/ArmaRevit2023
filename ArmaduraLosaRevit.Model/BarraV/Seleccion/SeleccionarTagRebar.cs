using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{
   public class SeleccionarTagRebar
    {
        private readonly UIApplication _uiapp;
        private UIDocument _uidoc;
        public IndependentTag independentTag { get; private set; }
        public SeleccionarTagRebar(UIApplication uiapp )
        {
            this._uiapp = uiapp;
            _uidoc = _uiapp.ActiveUIDocument;
        }



        public bool GetSelecionarRebarTag()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
    
            ISelectionFilter filtroRebarTag = new FiltroRebarTag();

            Reference referen;
            try
            {
                referen = _uidoc.Selection.PickObject(ObjectType.Element, filtroRebarTag, "Seleccionar el tag de la barra");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            if (referen == null) return false;
          //  PuntoSeleccionMouse = Util.PtoDeLevelDeGlobalPoint(referen.GlobalPoint, _uidoc.Document);

            Element r = _uidoc.Document.GetElement(referen);
            // si refere3ncia es null salir
            if (r == null) return false;

            //obtiene una referencia floor con la referencia r
            if (r is IndependentTag)
                independentTag = r as IndependentTag;

            return true;
        }
    }
}
