
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.UpdateVistas
{

    [Transaction(TransactionMode.Manual)]
    public class Cmd_cargarUpdate_UpdaterNombreVistaEnBarras : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //ManejadorNombreVistaEnBarras _manejadorUpdateRebar = new ManejadorNombreVistaEnBarras(commandData.Application);
            ManejadorNombreVistaEnBarras.CargarUpdateView(commandData.Application);
            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Cmd_DescargarUpdate_UpdaterNombreVistaEnBarras : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //ManejadorNombreVistaEnBarras _manejadorUpdateRebar = new ManejadorNombreVistaEnBarras(commandData.Application);
            ManejadorNombreVistaEnBarras.DesCargarUpdateView(commandData.Application);

            return Result.Succeeded;

        }
    }

}
