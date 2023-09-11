using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.BarraV.UpDate;

namespace ArmaduraLosaRevit.Model.RebarLosa.CMD
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class CMD_ManejadorLosaREbarInclinadas : IExternalCommand
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

            probar_fx_sx _probar_fx_sx = new probar_fx_sx();
            _probar_fx_sx.Text = "Losa inclinadas";
            _probar_fx_sx.ShowDialog();

            ManejadorUpdateRebar _manejadorUpdateRebar = new ManejadorUpdateRebar(commandData.Application);
            _manejadorUpdateRebar.DesCargarUpdateREbar();
            if (_probar_fx_sx.IsOK)
            { 
                TipoBarra varsad = EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, _probar_fx_sx.tipobarra);
                ManejadorRebarLosaInclinada barraManejador = new ManejadorRebarLosaInclinada(commandData.Application);
                barraManejador.BarraInferiores(varsad, _probar_fx_sx.direccion);
            }
            _manejadorUpdateRebar.CargarUpdateREbar();

            return Result.Succeeded;

        }

    }
}
