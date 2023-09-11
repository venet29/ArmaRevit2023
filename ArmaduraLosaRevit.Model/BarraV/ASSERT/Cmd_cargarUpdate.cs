using ArmaduraLosaRevit.Model.BarraV.UpDate;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_cargarUpdate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            // Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(commandData.Application);
            Manejador_UpdateRebar.CargarUpdateREbar(commandData.Application);
            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_DescargarUpdate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(commandData.Application);
            Manejador_UpdateRebar.DesCargarUpdateREbar(commandData.Application);

            return Result.Succeeded;

        }
    }

}
