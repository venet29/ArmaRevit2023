using Autodesk.Revit.UI;
using System;
using System.Windows;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Microsoft.VisualBasic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;


using System.Windows.Media;
using ArmaduraLosaRevit.Model.Conector;
using Autodesk.Revit.DB.Events;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;

namespace ArmaduraLosaRevit.Model.Prueba
{
    public class prueba_fx_sx
    {

   //     public static double angulo_refe { get; set; }

      //  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
       // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
        //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
        public class Barraf1 : IExternalCommand
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

                var datos = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                probar_fx_sx probar_fx_sx = new probar_fx_sx();

                probar_fx_sx.ShowDialog();

                DatosDiseño.DISENO_VALIDAR_ESPESOR = TipoValidarEspesor.NOVerificarEspesorMenor15;

                bool valor_antiguo = VariablesSistemas.IsAjusteBarra_Recorrido;
                VariablesSistemas.IsAjusteBarra_Recorrido = probar_fx_sx.IsAjusteBarra_Recorrido;


                CargarBarraRoomDTO _CargarBarraRoomDTO=  probar_fx_sx.ObtenerCargarBarraRoomDTO();
                if (_CargarBarraRoomDTO == null) return Result.Failed;


                Result result = CargarBarraRoom.Cargar(commandData.Application, _CargarBarraRoomDTO);

                VariablesSistemas.IsAjusteBarra_Recorrido = valor_antiguo;
                return result;

            }
        }
    }
}
