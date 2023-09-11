using System;

using System.Collections.Generic;

using System.Text;

using System.Windows.Forms;



using Autodesk.Revit.DB;

using Autodesk.Revit.UI;

using Autodesk.Revit.ApplicationServices;

using Autodesk.Revit.Attributes;

using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.Prueba
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class BuscarEspesorLosaVar : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {

            UIApplication app = commandData.Application;
            Document doc = app.ActiveUIDocument.Document;
            Selection sel = app.ActiveUIDocument.Selection;
            ISelectionFilter filtroRebar = new FiltroFloor();
            Reference ref1 = sel.PickObject(ObjectType.Element, filtroRebar, "pick a family instance");
            Floor selloor = doc.GetElement(ref1) as Floor;

            XYZ ptoSeleccion = ref1.GlobalPoint;


           double espesor= selloor.ObtenerEspesorConPtosFloor(ptoSeleccion);

            return Result.Succeeded;

        }

    }
}
