using ArmaduraLosaRevit.Model.FAMILIA;

using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.FAMILIA.Varios;
using System;
using ArmaduraLosaRevit.Model.ConfiguracionInicial;
using ArmaduraLosaRevit.Model.UpdateGenerar;

//using planta_aux_C.Elemento_Losa;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CargarFamiliasOtros : IExternalCommand
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
            UIApplication _uiapp = commandData.Application;


            var result = Util.InfoMsg_YesNo($"Confirma que de desea continuar con Comando 'Cargar Familias y configuracion Inicial '?.\nProceso podria durar unos minutos.");
            if (result == System.Windows.Forms.DialogResult.No) return Autodesk.Revit.UI.Result.Failed; 

            try
            {
                ManejadorConfiguracionInicialGeneral.cargar(_uiapp, false);
            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        
            UpdateGeneral.M5_DesCargarGenerar(_uiapp);
            try
            {
                //
                ManejadorCargarFAmilias manejadorCargarFAmilias = new ManejadorCargarFAmilias(commandData.Application);
               // manejadorCargarFAmilias.DuplicarFamilasReBarBarv2();
                manejadorCargarFAmilias.cargarFamilias_run();

                // cambiar pelota de directriz 
                PelotaDedirectriz PelotaDedirectriz = new PelotaDedirectriz(commandData.Application);
                PelotaDedirectriz.Ejecutar();

                TaskDialog.Show("ok", "Elementos cargados correctamente");
            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
            UpdateGeneral.M4_CargarGenerar(_uiapp);
            return Result.Succeeded;
        }
    }
}
