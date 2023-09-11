using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Shaft.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Shaft
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Cmd_Shaft : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


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
            UIApplication app = commandData.Application;
            Document _doc = app.ActiveUIDocument.Document;

            bool mientras = true;
            while (mientras)
            {
                mientras = false;
                //1)seleccionana
                SeleccionarOpeningConMouse seleccionarOpeningConMouse = new SeleccionarOpeningConMouse(app);
                seleccionarOpeningConMouse.M1_SelecconaOpening();
                if (!seleccionarOpeningConMouse.IsOk()) return Result.Succeeded;   //ShaftIndividualNULL

                //2)Obtienen datos del shaft correcto
                ShaftConjunto shaftGeom = new ShaftConjunto(_doc, seleccionarOpeningConMouse, _doc.ActiveView.GenLevel);
                shaftGeom.Ejecutar();

                //3)obtiene objeto que representa shaft --geometria
                ShaftIndividual ShaftIndividuas = shaftGeom.shaftUnicoSeleccoinado;
                if (ShaftIndividuas.IsOk == false)
                {
                    mientras = true;
                    continue;
                }
                if (!ShaftIndividuas.M2_IsMAs2Ptos())
                {
                    mientras = true;
                    continue;
                }

                //4)dibiuja shaft y cruz
                ShaftIndividuas.M3_CrearSeparacionRoom(commandData.Application);
                ShaftIndividuas.M4_CrearCruz();
                mientras = true;
            }
            return Result.Succeeded;
        }


    }
}
