using ArmaduraLosaRevit.Model.BorrarSeleccion.SharePArameter;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
namespace ArmaduraLosaRevit.Borrar
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class COmmanderBorrarShareParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            BorrarShareParameterForm borrarShareParameterForm = new BorrarShareParameterForm();
            borrarShareParameterForm.ShowDialog();

            if (borrarShareParameterForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                BorrarShareParameter BorrarShareParameter = new BorrarShareParameter(commandData.Application.ActiveUIDocument.Document);
                BorrarShareParameter.BorrarPArametro(borrarShareParameterForm.idElemet);
            }
            return Result.Succeeded;
        }
    }




}
