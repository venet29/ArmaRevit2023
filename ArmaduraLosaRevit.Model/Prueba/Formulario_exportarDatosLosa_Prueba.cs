using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Automatico;

namespace ArmaduraLosaRevit.Model.Prueba
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    //[Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Formulario_ExportartBarras : IExternalCommand
    {
        /// <summary>
        /// clases barras para para definir las barras, para poder modificar las familias para luego dibujarlos
        /// en el mrevit
        /// </summary>


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
                //crea formulario con:
                //  lista de los room con:
                //       -datos generales, + lista vertices+ lista de borde     

                Formulario formulario = new Formulario(commandData);
                formulario.ExportardatosParaIngenieria();

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
    //[Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Formulario_ImportatBarrasSUPLES : IExternalCommand
    {
        /// <summary>
        /// clases barras para para definir las barras, para poder modificar las familias para luego dibujarlos
        /// en el mrevit
        /// </summary>


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
                //Formulario formulario = new Formulario(commandData);
                //ConfiguracionAhorro configuracionAhorro = new ConfiguracionAhorro("s2");
                //formulario.ImportarBarrasParaDibujo(TipoConfiguracionBarra.suple, configuracionAhorro, false);


                FormularioAUTOSUP formularioAUTO = new FormularioAUTOSUP();
                formularioAUTO.ShowDialog();

                if (formularioAUTO.Ok)
                {
                    Formulario formulario = new Formulario(commandData);
                    formulario.ImportarBarrasParaDibujo(TipoConfiguracionBarra.suple, formularioAUTO.configuracionAhorro);
                }


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
    //[Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Formulario_ImportatBarrasINFERIOR : IExternalCommand
    {
        /// <summary>
        /// clases barras para para definir las barras, para poder modificar las familias para luego dibujarlos
        /// en el mrevit
        /// </summary>


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external applicationNN 
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

                VariablesSistemasDTO vdto = new VariablesSistemasDTO()
                {
                    IsConAhorro = VariablesSistemas.IsConAhorro,
                    IsVerificarEspesor = VariablesSistemas.IsVerificarEspesor,

                    IsDibujarS4 = VariablesSistemas.IsDibujarS4,
                    IsAjusteBarra_Recorrido = VariablesSistemas.IsAjusteBarra_Recorrido,
                    IsAjusteBarra_Largo = VariablesSistemas.IsAjusteBarra_Largo,
                    IsReSeleccionarPuntoRango = false,
                    LargoBarras_cm = VariablesSistemas.LargoBarras_cm,
                    LargoRecorrido_cm = VariablesSistemas.LargoRecorrido_cm,
                    tipoPorF1 = VariablesSistemas.tipoPorF1.ToString(),
                    tipoPorF3 = VariablesSistemas.tipoPorF3.ToString(),
                    tipoPorF4 = VariablesSistemas.tipoPorF4.ToString()
                };

                FormularioAUTO formularioAUTO = new FormularioAUTO(vdto);
                formularioAUTO.ShowDialog();

                if (formularioAUTO.Ok)
                {
                    VariablesSistemas.IsConAhorro = formularioAUTO.IsAhorro;
                    VariablesSistemas.IsVerificarEspesor = formularioAUTO.IsConVerificar;
                    
                    VariablesSistemas.LargoBarras_cm = formularioAUTO.LargoBarras;
                    VariablesSistemas.LargoRecorrido_cm = formularioAUTO.LargoRecorrido;

                    VariablesSistemas.tipoPorF1 = formularioAUTO.tipoPorF1;
                    VariablesSistemas.tipoPorF3 = formularioAUTO.tipoPorF3;
                    VariablesSistemas.tipoPorF4 = formularioAUTO.tipoPorF4;

                    Formulario formulario = new Formulario(commandData);
                    formulario.ImportarBarrasParaDibujo(TipoConfiguracionBarra.refuerzoInferior, formularioAUTO.configuracionAhorro, formularioAUTO.IsSoloCopiarDatos);
                }


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
