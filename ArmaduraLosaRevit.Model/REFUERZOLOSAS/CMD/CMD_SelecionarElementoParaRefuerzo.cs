using Autodesk.Revit.UI;
using System;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.CMD
{


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class SelecionarElementoRefuerzoViga : IExternalCommand
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

            DatosRefuerzoTipoVigaDTO datosRefuerzoTipoViga = new DatosRefuerzoTipoVigaDTO()
            {
                CantidadBarras = 5,
                diamtroBarraRefuerzo_MM = 10,
                diamtroEstribo_MM = 8,
                espacimientoEstribo_Cm = 20,
                tipobarra = "s1"

            };

            ManejadorRefuerzoTipoViga manejadorRefuerzoTipoViga = new ManejadorRefuerzoTipoViga(commandData.Application, datosRefuerzoTipoViga);

            return manejadorRefuerzoTipoViga.Ejecutar(); ;

        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class SelecionarRefuerzoCabezaMuro : IExternalCommand
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



            DatosRefuerzoCabezaMuroDTO datosRefuerzoCabezaMuroDTO = new DatosRefuerzoCabezaMuroDTO()
            {
                CantidadBarras = 3,
                diamtroBarraRefuerzo_MM = 16,
                diamtroBarraS1_MM = 8,
                espacimientoS1_Cm = 20,
                tipobarra="s1"

            };

            ManejadorRefuerzoCabezaMuro manejadorRefuerzoCabezaMuro = new ManejadorRefuerzoCabezaMuro(commandData.Application, datosRefuerzoCabezaMuroDTO);

            return manejadorRefuerzoCabezaMuro.Ejecutar(); ;
        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class SelecionarBordeMuro : IExternalCommand
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
            var _uiapp = commandData.Application;
            var _doc = commandData.Application.ActiveUIDocument.Document;


            DatosRefuerzoTipoBorde DatosRefuerzoTipoBord_d = new DatosRefuerzoTipoBorde()
            {
                CantidadBarras = 2,
                diamtroBarraRefuerzo_MM = 8,
                diamtroEstribo_MM = 8,
                espacimientoEstribo_Cm = 10
                
            };


            ManejadorRefuerzoTipoBorde manejadorRefuerzoTipoBorde = new ManejadorRefuerzoTipoBorde(_uiapp, DatosRefuerzoTipoBord_d);
            manejadorRefuerzoTipoBorde.Ejecutar();

            return Result.Succeeded;
        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class AgergarTagBArra : IExternalCommand
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
            var _uiapp = commandData.Application;
            var _doc = commandData.Application.ActiveUIDocument.Document;


            ManejadorModificarTag manejadorRefuerzoTipoBorde = new ManejadorModificarTag(_uiapp, 4);
            manejadorRefuerzoTipoBorde.EjecutarCambioContag();

            return Result.Succeeded;
        }

    }
}
