using ArmaduraLosaRevit.Model.Conector;
using ArmaduraLosaRevit.Model.Cambiar.CuantiasRoom;

using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.EditarPath;

//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SeleccionarFormularioPath : IExternalCommand
    {
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            // EditarPathReinFormv2 editarPathReinForm = new EditarPathReinFormv2();

            // CambiarCuantia CambiarCuantia = new CambiarCuantia();
            //  CambiarCuantia.ShowDialog();
            // Form1_prueba Form1_prueba = new Form1_prueba();
            // Form1_prueba.ShowDialog();
            EditarPathReinManejador3 EditarPathReinManejador = new EditarPathReinManejador3(commandData.Application);
            EditarPathReinManejador.Ejecutar();

            return Result.Succeeded;
        }
    }

}

