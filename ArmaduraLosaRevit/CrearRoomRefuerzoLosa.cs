using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

//using planta_aux_C.Elemento_Losa;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzoAuto;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CrearRoomRefuerzoLosa : IExternalCommand
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
            try
            {
                AyudaRevisionCoud.uidoc = commandData.Application.ActiveUIDocument;
                AyudaRevisionCoud.ObtenerUIView();

                //obtiene las confifuracion de los room seleccionado
                List<BoundarySegmentHandler> lista = RoomFunciones.ListaRoomDatosGenerales(commandData.Application, "SelectConMouse");
                //dibujas los refuerzo
                BarraRefuerzoDibujar barraRefuerzoDibujar = new BarraRefuerzoDibujar();
                barraRefuerzoDibujar.DibujarREfuerzosLosa(lista);

                TaskDialog.Show("Carga familia ", "Elementos necesarios para el diseño de losas cargados correctamente.");

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }



    }



}
