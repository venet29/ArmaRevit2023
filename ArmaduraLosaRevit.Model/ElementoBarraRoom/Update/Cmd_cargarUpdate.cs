using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.Visibilidad.UpdateVistas;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_cargarUpDateTextoBordePathSymbol : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //Manejador_UpDateMoverPathSymbol _manejadorUpdateRebar = new Manejador_UpDateMoverPathSymbol(commandData.Application);
            Manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol(commandData.Application);

            //Manejador_UpDateMoverTagPathSymbol _ManejadorTagPathSymbol = new Manejador_UpDateMoverTagPathSymbol(commandData.Application);
            Manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol(commandData.Application);

            //Manejador_UpDateEditPathReinf _Manejador_UpDateEditPathReinf = new Manejador_UpDateEditPathReinf(commandData.Application);
            //_Manejador_UpDateEditPathReinf.CargarUpdateEditPathReinf();
            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_DescargarUpDateTextoBordePathSymbol : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Manejador_UpDateMoverPathSymbol _manejadorUpdateRebar = new Manejador_UpDateMoverPathSymbol(commandData.Application);
            Manejador_UpDateMoverPathSymbol.DesCargarUpdatePathSymbol(commandData.Application);
            //Manejador_UpDateMoverTagPathSymbol _ManejadorTagPathSymbol = new Manejador_UpDateMoverTagPathSymbol(commandData.Application);
            Manejador_UpDateMoverTagPathSymbol.DescargarTagUpdatePathSymbol(commandData.Application);
            //Manejador_UpDateEditPathReinf _Manejador_UpDateEditPathReinf = new Manejador_UpDateEditPathReinf(commandData.Application);
            //_Manejador_UpDateEditPathReinf.DesCargarUpdatePathReinf();
            return Result.Succeeded;

        }
    }

}
