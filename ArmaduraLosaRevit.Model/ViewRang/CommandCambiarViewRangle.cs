using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;

namespace ArmaduraLosaRevit.Model.ViewRang
{


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CommandCambiarViewRangle : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            View view = doc.ActiveView;


            //double ValorTopClipPlane, ValorCutPlane, ValorBottomClipPlane, ValorViewDepthPlane;
            //ValorTopClipPlane = 2f;
            //ValorCutPlane = -1f;
            //ValorBottomClipPlane = -2f;
            //ValorViewDepthPlane = -2f;


            IViewRangleClase viewRangleClase = ViewRangleClase.CreatorNuevoViewRange(doc, view);
            viewRangleClase.EditarParametroViewRange(2,2, 0,  -2);

            return Result.Succeeded;

        }

       
    }
}