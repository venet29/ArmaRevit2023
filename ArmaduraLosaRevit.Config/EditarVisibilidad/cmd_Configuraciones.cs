
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad
{

        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ConfiguracionesRoomReference : IExternalCommand
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

                var lista1 = commandData.Application.GetRibbonPanels("Diseño de Losas").SelectMany(pa => pa.GetItems()).First(it => it.Name == "RoomReference");

                IVisibilidadView visibilidad =  VisibilidadView.Creador_Visibilidad(commandData.View, BuiltInCategory.OST_Rooms, "Reference");
                visibilidad.CambiarVisibilityBuiltInCategory();
                lista1.ItemText = (!visibilidad.EstadoActualHide() ? "On":"Off");

                TaskDialog.Show("ok", "Marca Room " + lista1.ItemText);

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
    public class ConfiguracionesBordeBarra : IExternalCommand
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
                var lista1 = commandData.Application.GetRibbonPanels("Diseño de Losas").SelectMany(pa => pa.GetItems()).First(it => it.Name == "BordeBarra");

                IVisibilidadView visibilidad =  VisibilidadView.Creador_Visibilidad(commandData.View, BuiltInCategory.OST_PathReinBoundary, "Boundary");
               
                visibilidad.CambiarVisibilityBuiltInCategory();
                lista1.ItemText = (!visibilidad.EstadoActualHide ()? "On" : "Off");

                TaskDialog.Show("ok", "Borde Barra" + lista1.ItemText);

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
    public class ConfiguracionesGrid : IExternalCommand
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
                //var lista1 = commandData.Application.GetRibbonPanels("Diseño de Losas").SelectMany(pa => pa.GetItems()).First(it => it.Name == "BordeBarra");

                var lista1 = commandData.Application.GetRibbonPanels("Diseño Elevaciones").SelectMany(pa => pa.GetItems()).First(it => it.Name == "GridElev");

                IVisibilidadView visibilidad = VisibilidadView.Creador_Visibilidad(commandData.View, BuiltInCategory.OST_Grids, "Grids");

                visibilidad.CambiarVisibilityBuiltInCategory();
                lista1.ItemText = (!visibilidad.EstadoActualHide() ? "On" : "Off");

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
    public class ConfiguracionesObtenerTodasCategorias : IExternalCommand
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

                //true oculta
                // false ver
                VisibilidadCategorias visibilidadView = new VisibilidadCategorias(commandData.Application.ActiveUIDocument.ActiveView);
                List<string> listaExclusion = new List<string>();

                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.Annotation, listaExclusion);

                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.AnalyticalModel, listaExclusion);

                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.Model, ListaExclusion.ListaExclusionLosa());


                // visibilidadView.CambiarVisibilityBuiltInCategory

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
