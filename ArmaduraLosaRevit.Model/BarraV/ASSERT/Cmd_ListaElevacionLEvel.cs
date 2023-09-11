#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class cmd_ListaElevacionLEvel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
             

              ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(commandData.Application);
              _seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(commandData.Application.ActiveUIDocument.ActiveView);

            //  List<Level> list = _seleccionarNivel.ObtenerListaNivelPOrelevacionNombre(uiapp.ActiveUIDocument);

            return Result.Succeeded;

        }
    }
}

