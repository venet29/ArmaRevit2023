using ArmaduraLosaRevit.Model.BarraV.UpDate;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Tag.UpDate
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_cargarUpDateRecalcularLargoMin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            ManejadorUpDateRecalcularLargoMin _manejadorUpdateRebar = new ManejadorUpDateRecalcularLargoMin(commandData.Application);
            _manejadorUpdateRebar.CargarRecalcularLargoMin();
            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_DescargarUpDateRecalcularLargoMin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ManejadorUpDateRecalcularLargoMin _manejadorUpdateRebar = new ManejadorUpDateRecalcularLargoMin(commandData.Application);
            _manejadorUpdateRebar.DesCargarRecalcularLargoMin();

            return Result.Succeeded;

        }
    }

}
