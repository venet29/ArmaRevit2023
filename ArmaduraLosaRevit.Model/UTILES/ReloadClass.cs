using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_ReloadClass : IExternalCommand
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


            ReloadClass _ReloadClass =new ReloadClass();
            _ReloadClass.ReloadLatestWithMessage(commandData.Application.ActiveUIDocument.Document);



            return Result.Succeeded;

        }
    }


    public  class ReloadClass
    {


        public  void ReloadLatestWithMessage(Document _doc)
        {
            // Tell user what we're doing
            TaskDialog td = new TaskDialog("Alert");
            td.MainInstruction = "Application 'Automatic element creator' needs to reload changes from central in order to proceed.";
            td.MainContent = "This will update your local with all changes currently in the central model.  This operation " +
                                "may take some time depending on the number of changes available on the central.";
            td.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

            TaskDialogResult result = td.Show();
            
            if (result == TaskDialogResult.Ok)
            {
                // There are no currently customizable user options for ReloadLatest.

                
                    try
                    {
                        using (TransactionGroup transGroup = new TransactionGroup(_doc))
                        {
                            transGroup.Start("reload-NH");
                            _doc.ReloadLatest(new ReloadLatestOptions());
                            //EjecutarLetraPararametroCambia();
                            transGroup.Assimilate();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Util.ErrorMsg($"Error al desplazar path superior ex:{ex.Message}");
                    }

                 
                TaskDialog.Show("Proceeding...", "Reload operation completed, proceeding with updates.");
            }
            else
            {
                TaskDialog.Show("Canceled.", "Reload operation canceled, so changes will not be made.  Return to this command later when ready to reload.");
            }
        }
    }
}
