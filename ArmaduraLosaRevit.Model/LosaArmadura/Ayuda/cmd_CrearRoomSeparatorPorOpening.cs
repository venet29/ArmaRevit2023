using ArmaduraLosaRevit.Model.Elementos_viga;
using ArmaduraLosaRevit.Model.LosaArmadura.Cmd;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.LosaArmadura.Ayuda
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class cmd_CrearRoomSeparatorPorOpening : IExternalCommand
    {

        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


        //private static ExternalCommandData m_revit;
        //private static Document doc;


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
            Debug.Print("probando debug");

            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            Cmd_DibujarLineasSeparacicionRoomOpening dibujarLineasSeparacicionRoomOpening = new Cmd_DibujarLineasSeparacicionRoomOpening();
            dibujarLineasSeparacicionRoomOpening.Execute_NH_ListaOpening(commandData);

            return Result.Succeeded;
        }




    }


}
