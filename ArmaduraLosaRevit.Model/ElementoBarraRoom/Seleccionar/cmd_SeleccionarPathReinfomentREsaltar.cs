using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar
{
 
    [Transaction(TransactionMode.Manual)]

    public class cmd_Seleccionar1PathReinfomentREsaltar : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
       
            Document doc = uidoc.Document;

            SeleccionarPathReinfomentREsaltar _SeleccionarPathReinfomentREsaltar = new SeleccionarPathReinfomentREsaltar(uiapp);
            _SeleccionarPathReinfomentREsaltar.Ejecutar_seleccion1();

            return Result.Succeeded;
        }


    }


    [Transaction(TransactionMode.Manual)]
    public class cmd_SeleccionarMultiplePathReinfomentREsaltar : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            Document doc = uidoc.Document;

            SeleccionarPathReinfomentREsaltar _SeleccionarPathReinfomentREsaltar = new SeleccionarPathReinfomentREsaltar(uiapp);
            _SeleccionarPathReinfomentREsaltar.Ejecutar_seleccionMultiples();

            return Result.Succeeded;
        }


    }


}
