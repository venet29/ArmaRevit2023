using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
//using planta_aux_C.Elemento_Losa;

namespace ArmaduraLosaRevit.Model.BorrarSeleccion.BorrarPath
{
    // crea un objetco : RoomsData
    // aun nose analiza  


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_SeleccionarPathReinfomentRectangulo : IExternalCommand
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

          //  IVisibilidad visibilidad = Visibilidad.Creador_Visibilidad(commandData.View, BuiltInCategory.OST_PathReinBoundary, "Boundary");
           // bool bordeBarraHideActual = visibilidad.EstadoActualHide();
           // visibilidad.AsignarVisibilityBuiltInCategory(false);
          
            try
            {
                SeleccionarPathReinfomentRectangulo administrador_ReferenciaRoom = new SeleccionarPathReinfomentRectangulo(commandData.Application);
                if(administrador_ReferenciaRoom.SeleccionadosMultiplesPathReinConRectaguloYFiltros())
                    administrador_ReferenciaRoom.BorrarPathReinfSeleccionado();

              //  visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
            catch (Exception e)
            {
                message = e.Message.ToString();
                return Autodesk.Revit.UI.Result.Cancelled;
            }
            finally
            {
                //visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_Seleccionar1PathReinfoment : IExternalCommand
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

            //  IVisibilidad visibilidad = Visibilidad.Creador_Visibilidad(commandData.View, BuiltInCategory.OST_PathReinBoundary, "Boundary");
            // bool bordeBarraHideActual = visibilidad.EstadoActualHide();
            // visibilidad.AsignarVisibilityBuiltInCategory(false);

            try
            {
                SeleccionarPathReinfomentRectangulo administrador_ReferenciaRoom = new SeleccionarPathReinfomentRectangulo(commandData.Application);
                bool Seguir = true;
                while (Seguir)
                {


                    if (administrador_ReferenciaRoom.Seleccionados1Path())
                    {
                        Seguir = administrador_ReferenciaRoom.BorrarPathReinfSeleccionado();
                    }
                    else
                        Seguir = false;

                }

                //  visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
            catch (Exception e)
            {
                message = e.Message.ToString();
                return Autodesk.Revit.UI.Result.Cancelled;
            }
            finally
            {
                //visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_SOLOSeleccionar1PathReinfoment : IExternalCommand
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

            //  IVisibilidad visibilidad = Visibilidad.Creador_Visibilidad(commandData.View, BuiltInCategory.OST_PathReinBoundary, "Boundary");
            // bool bordeBarraHideActual = visibilidad.EstadoActualHide();
            // visibilidad.AsignarVisibilityBuiltInCategory(false);

            try
            {
                SeleccionarPathReinfomentRectangulo administrador_ReferenciaRoom = new SeleccionarPathReinfomentRectangulo(commandData.Application);
                if (administrador_ReferenciaRoom.Seleccionados1Path())
                { }

                //  visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
            catch (Exception e)
            {
                message = e.Message.ToString();
                return Autodesk.Revit.UI.Result.Cancelled;
            }
            finally
            {
                //visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }

}
