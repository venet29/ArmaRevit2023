


using Autodesk.Revit.DB;

using Autodesk.Revit.UI;

using Autodesk.Revit.Attributes;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

namespace ArmaduraLosaRevit.Model.Seleccionar.cmd
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class CMD_CambiarRebarCoverType : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData,ref string messages, ElementSet elements)
        {

            UIApplication app = commandData.Application;

            SeleccionarRebarCoverType modificarParametrosRoom = new SeleccionarRebarCoverType(app);
            modificarParametrosRoom.ObtenerListaRebarCoverType();
            modificarParametrosRoom.CambiarRecubrimiento(ConstNH.CONST_RECUBRIMIENTO_BAse2cm_MM);

            // redib8uja
            return Result.Succeeded;

        }

    }
}
