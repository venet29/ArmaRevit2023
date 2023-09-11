﻿using Autodesk.Revit.UI;
//using planta_aux_C.Elemento_Losa;
using ArmaduraLosaRevit.Model.Pasadas.WPFPasada;
using ArmaduraLosaRevit.Model.EstadoDesarrollo.WPF;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit
{


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_CrearPAsadas : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


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
            ManejadorWPF_UI_Pasadas _ManejadorWPF_UI_Pasadas = new ManejadorWPF_UI_Pasadas(commandData.Application);
            return _ManejadorWPF_UI_Pasadas.Execute();

        }


    }




}

