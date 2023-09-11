using Autodesk.Revit.UI;
using System.Collections.Generic;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

//using planta_aux_C.Elemento_Losa;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Prueba;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit.Model.Seleccionar
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CommandSeleccionarRebarConPto : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian

        List<XYZ> ListaPtos = new List<XYZ>();
        private static ExternalCommandData m_revit;
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
            Options opt = app.Create.NewGeometryOptions();
            m_revit = revit;
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new JtElementsOfClassSelectionFilter<Rebar>();
            //selecciona un objeto floor
           // Reference r = uidoc.Selection.PickObject(ObjectType.Element, f, "Please select a planar face to define work plane");

            Element r = Util.GetSingleSelectedElement(uidoc);
            // sirefere3ncia es null salir
            if (r == null)
                return Result.Succeeded;

            //RebarShapeDefinitionBySegments
            //RebarShapeDefinitionBySegments Class
            //https://thebuildingcoder.typepad.com/blog/2011/06/the-revit-structure-2012-api.html

            //obtiene una referencia floor con la referencia r
            Rebar selecFloor = r as Rebar;

            var dic = new Dictionary<int, int>();
            dic.Add(0, 2);
            dic.Add(1, 2);

            return Result.Succeeded;
        }

    }


   

}
