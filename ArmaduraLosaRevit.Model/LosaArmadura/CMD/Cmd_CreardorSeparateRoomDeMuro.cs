using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

//using planta_aux_C.Elemento_Losa;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Elementos_viga;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Events;
using ArmaduraLosaRevit.Model.ViewRang;
using ArmaduraLosaRevit.Model.UTILES;
using System.Diagnostics;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit.Model.LosaArmadura.Cmd
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Cmd_CreardorSeparateRoomDeMuro : IExternalCommand
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
            HelperSeleccinarMuro seleccionarMuroConMouse = new HelperSeleccinarMuro(commandData.Application);
            View view = commandData.Application.ActiveUIDocument.Document.ActiveView;
          
            ISeleccionarLosaConMouse seleccionarLosaConMouse = new SeleccionarLosaConMouse(commandData.Application);

            CreardorSeparateRoomDeMuro crearRoomConPelotaLosa =
                new CreardorSeparateRoomDeMuro(commandData, view,seleccionarMuroConMouse, seleccionarLosaConMouse);
            return crearRoomConPelotaLosa.Execute();
        }


    }

}
