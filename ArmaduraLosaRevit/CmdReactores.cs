using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ConfiguracionesReactElev : IExternalCommand
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
            return FactoryReactorRebarCambiarTexto.ReactorRebarCambiarTexto(commandData, message, true);
        }


    }




    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ConfiguracionesReactPathReinLosa : IExternalCommand
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
            Result resultElev = Result.Failed;

            try
            {
                var lista1 = commandData.Application.GetRibbonPanels("Diseño de Losas").SelectMany(pa => pa.GetItems()).First(it => it.Name == "ReactorTextoPathReinf");

                if (lista1.ItemText == "Off")
                {
                  //  Manejador_UpDateMoverPathSymbol _manejadorUpdateRebar = new Manejador_UpDateMoverPathSymbol(commandData.Application);
                    Manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol(commandData.Application);

                    //Manejador_UpDateMoverTagPathSymbol _ManejadorTagPathSymbol = new Manejador_UpDateMoverTagPathSymbol(commandData.Application);
                    Manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol(commandData.Application);

                    //Manejador_UpDateEditPathReinf _Manejador_UpDateEditPathReinf = new Manejador_UpDateEditPathReinf(commandData.Application);
                    //_Manejador_UpDateEditPathReinf.CargarUpdateEditPathReinf();

                    lista1.ItemText = "On";

                  
                }
                else
                {
                   // Manejador_UpDateMoverPathSymbol _manejadorUpdateRebar = new Manejador_UpDateMoverPathSymbol(commandData.Application);
                    Manejador_UpDateMoverPathSymbol.DesCargarUpdatePathSymbol(commandData.Application);

                    //Manejador_UpDateMoverTagPathSymbol _ManejadorTagPathSymbol = new Manejador_UpDateMoverTagPathSymbol(commandData.Application);
                    Manejador_UpDateMoverTagPathSymbol.DescargarTagUpdatePathSymbol(commandData.Application);
                    lista1.ItemText = "Off";

                    //Manejador_UpDateEditPathReinf _Manejador_UpDateEditPathReinf = new Manejador_UpDateEditPathReinf(commandData.Application);
                    //_Manejador_UpDateEditPathReinf.DesCargarUpdatePathReinf();
                }


                resultElev = FactoryReactorRebarCambiarTexto.ReactorRebarCambiarTexto(commandData, message, false);

                if(resultElev!= Result.Succeeded) TaskDialog.Show("Error", "Error cargar Reactor Rebar " + lista1.ItemText);

                TaskDialog.Show("ok", "Reactor Losa PathReinforment y Rebar " + lista1.ItemText);

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


    public static class FactoryReactorRebarCambiarTexto
    {

        public static Result ReactorRebarCambiarTexto(ExternalCommandData commandData, string message, bool IsMensaje)
        {
            try
            {
                var lista1 = commandData.Application.GetRibbonPanels("Diseño Elevaciones").SelectMany(pa => pa.GetItems()).First(it => it.Name == "reactElevRebar");

                if (lista1.ItemText == "Off")
                {
                    //Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(commandData.Application);
                    Manejador_UpdateRebar.CargarUpdateREbar(commandData.Application);
                    lista1.ItemText = "On";
                }
                else
                {
                   // Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(commandData.Application);
                    Manejador_UpdateRebar.DesCargarUpdateREbar(commandData.Application);
                    lista1.ItemText = "Off";
                }

                if (IsMensaje) TaskDialog.Show("ok", "Reactor Elevacion " + lista1.ItemText);

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
