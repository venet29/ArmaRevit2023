using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.AnalisisRoom.Servicios;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit
{

        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class LargoMinCambiar : IExternalCommand
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

                UIApplication app = commandData.Application;

                ModificarParametrosRoom modificarParametrosRoom = new ModificarParametrosRoom(app.ActiveUIDocument);
                modificarParametrosRoom.cambiarDireccionPrincipal();


                //borrar elemtos si existen
                ServicioaCambiarLargoMinV2 servicioaCambiarLargoMin = new ServicioaCambiarLargoMinV2(commandData);
                //borrar elemtos
                servicioaCambiarLargoMin.BorrarLargosMin(modificarParametrosRoom.ListaRooms);


                //dibujar
                servicioaCambiarLargoMin.DibujarLargosMin();

                // redib8uja
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



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class LargoMinDibujar : IExternalCommand
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
                UIApplication app = commandData.Application;

                //crear servicio
                ServicioaCambiarLargoMinV2 modificarParametrosRoom = new ServicioaCambiarLargoMinV2(commandData);
                //seleccionar
                IEnumerable<Element> ListaRooms = new List<Element>();
                //ListaRooms = modificarParametrosRoom.roomSeleccionar.GetRoomSeleccionadosConRectaguloYFiltros(app.ActiveUIDocument);
                ListaRooms = modificarParametrosRoom.roomSeleccionar.GetRoomNivelActual(commandData.Application.ActiveUIDocument);
                //dibujar
                modificarParametrosRoom.DibujarLargosMin(ListaRooms);

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


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class LargoMinBorrar : IExternalCommand
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

                UIApplication app = commandData.Application;
                ServicioaCambiarLargoMinV2 modificarParametrosRoom = new ServicioaCambiarLargoMinV2(commandData);

                //seleccionar
                IEnumerable<Element> ListaRooms = new List<Element>();
               // ListaRooms = modificarParametrosRoom.roomSeleccionar.GetRoomSeleccionadosConRectaguloYFiltros(commandData.Application.ActiveUIDocument);
                ListaRooms = modificarParametrosRoom.roomSeleccionar.GetRoomNivelActual(commandData.Application.ActiveUIDocument);
                //borrar elemtos
                modificarParametrosRoom.BorrarLargosMin(ListaRooms);
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
