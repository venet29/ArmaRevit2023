using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;

//using planta_aux_C.Elemento_Losa;
using System.Diagnostics;
using ArmaduraLosaRevit;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Elemento_Losa;
using ArmaduraLosaRevit.Model.Elementos_viga;
using ArmaduraLosaRevit.Model.Seleccionar;
using System.CodeDom;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit.Model.Seleccionar.cmd
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]


    public class CmdSeleccionarLosa_ConPto : IExternalCommand
    {
        List<XYZ> ListaPtos = new List<XYZ>();
#pragma warning disable CS0169 // The field 'CmdSeleccionarLosa_ConPto.m_revit' is never used
        private static ExternalCommandData m_revit;
#pragma warning restore CS0169 // The field 'CmdSeleccionarLosa_ConPto.m_revit' is never used
        private static Document doc;
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            UIApplication uiapp = revit.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            doc = uidoc.Document;

            ISeleccionarLosaConMouse seleccionarLosaConMouse = new SeleccionarLosaConMouse(uiapp);
            seleccionarLosaConMouse.M1_SelecconarFloor();
            seleccionarLosaConMouse.M2_ObtenerEspesorLosaFoot();

          
            return Result.Succeeded;
        }


    }



}
