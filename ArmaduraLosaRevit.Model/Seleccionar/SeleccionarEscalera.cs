using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
  public class SeleccionarEscalera
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public SeleccionarEscalera(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public XYZ PuntoSeleccionMouse { get; private set; }
        public Stairs escaleraSeleccionada { get; private set; }

        public Stairs SeleccionarStairs(ObjectType objectType)
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new JtElementsOfClassSelectionFilter<Stairs>();
            //selecciona un objeto floor
            Reference referen;
            try
            {
                referen = _uiapp.ActiveUIDocument.Selection.PickObject(objectType, f, "Seleccionar escalera");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                referen = null;
            }

            if (referen == null) return null;
            PuntoSeleccionMouse = referen.GlobalPoint;

            Element r = _doc.GetElement(referen);
            // si refere3ncia es null salir
            if (r == null) return null;

            //obtiene una referencia floor con la referencia r
            if (r is Stairs)
                escaleraSeleccionada = r as Stairs;
            return escaleraSeleccionada;
        }


     
    }
}
