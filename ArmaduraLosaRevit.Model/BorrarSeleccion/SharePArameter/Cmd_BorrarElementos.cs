using ArmaduraLosaRevit.Model.BorrarSeleccion.SharePArameter;
using ArmaduraLosaRevit.Model.ParametrosShare;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BorrarSeleccion.SharePArameter
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class COmmanderBorrarShareParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            BorrarShareParameterForm borrarShareParameterForm = new BorrarShareParameterForm();
            borrarShareParameterForm.ShowDialog();

            if (borrarShareParameterForm.IsCrearLista)
            {
                ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(commandData.Application, RutaArchivoCompartido: "ParametrosNH");
                if (!_definicionManejador.M1_ShareCrearLista()) return Result.Failed;
            }
            else if (borrarShareParameterForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                BorrarShareParameter BorrarShareParameter = new BorrarShareParameter(commandData.Application.ActiveUIDocument.Document);
                BorrarShareParameter.BorrarId(borrarShareParameterForm.idElemet);
            }
            return Result.Succeeded;
        }
    }




}
