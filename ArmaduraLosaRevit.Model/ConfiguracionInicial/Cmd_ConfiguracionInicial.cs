using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Viewnh;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.ConfiguracionInicial.WPF;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial
{
    //[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    //[Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //public class Cmd_ConfiguracionesInicial_General : IExternalCommand
    //{
    //    /// <summary>
    //    /// Implement this method as an external command for Revit.
    //    /// </summary>nn
    //    /// <param name="commandData">An object that is passed to the external application 
    //    /// which contains data related to the command, 
    //    /// such as the application object and active view.</param>
    //    /// <param name="message">A message that can be set by the external application 
    //    /// which will be displayed if a failure or cancellation is returned by 
    //    /// the external command.</param>
    //    /// <param name="elements">A set of elements to which the external application 
    //    /// can add elements that are to be highlighted in case of failure or cancellation.</param>
    //    /// <returns>Return the status of the external command. 
    //    /// A result of Succeeded means that the API external method functioned as expected. 
    //    /// Cancelled can be used to signify that the user cancelled the external operation 
    //    /// at some point. Failure should be returned if the application is unable to proceed with 
    //    /// the operation.</returns>
    //    public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
    //    {
    //        try
    //        {
    //            ManejadorConfiguracionInicialGeneral.cargar(commandData.Application);
    //            return Result.Succeeded;
    //        }
    //        catch (Exception ex)
    //        {
    //            // If there are something wrong, give error information and return failed
    //            message = ex.Message;
    //            return Autodesk.Revit.UI.Result.Failed;
    //        }
    //    }

    //}

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Cmd_ConfiguracionInicial_Losa : IExternalCommand
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
                ManejadorConfiguracionInicialLosa _ManejadorConfiguracionInicialLosa = new ManejadorConfiguracionInicialLosa(commandData.Application);
                _ManejadorConfiguracionInicialLosa.cargar();
                return Result.Succeeded;
            }

            catch (Exception ex)
            {
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
    }



}
