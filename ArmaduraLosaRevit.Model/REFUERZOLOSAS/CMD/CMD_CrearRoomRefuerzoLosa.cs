using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Enumeraciones;

using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzoAuto;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.CMD
{


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class CrearRoomRefuerzoLosaAUTOMATICO : IExternalCommand
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
                AyudaRevisionCoud.uidoc = commandData.Application.ActiveUIDocument;
                AyudaRevisionCoud.ObtenerUIView();
                //obtiene las al confifuracion de los room 
                List<BoundarySegmentHandler> lista = RoomFunciones.ListaRoomDatosGenerales(commandData.Application, "SelectConMouse");

                //ElementId linemodel = new ElementId(784352);
                //Element ee = commandData.Application.ActiveUIDocument.Document.GetElement(linemodel);
                //BoundarySegment bs = (BoundarySegment)ee;

               // WrapperBoundarySegment ww = new WrapperBoundarySegment(0, ee as BoundarySegment, doc, nombreRoom, floor, Util.CmToFoot(2), 10);

                //dibujas los refuerzo
                BarraRefuerzoDibujar barraRefuerzoDibujar = new BarraRefuerzoDibujar();
                barraRefuerzoDibujar.DibujarREfuerzosLosa(lista);
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
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class ListaRoomDAtosGenerales_probarpto : IExternalCommand
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
                Document doc = commandData.Application.ActiveUIDocument.Document;
                View view = doc.ActiveView;
                Level nivelActual = view.GenLevel;
                ICollection<ElementId> listaOpening = SeleccionarOpening.GetOpeningFromLevelId(doc, nivelActual);
                View3D elem3d = TiposFamilia3D.Get3DBuscar(doc);
                if (elem3d == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial");
                    return Result.Failed;
                }
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_ShaftOpening);
                //ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.All, elem3d);
                ReferenceIntersector ri = new ReferenceIntersector(listaOpening, FindReferenceTarget.All, elem3d);

                List<Element> floors = SeleccionElement.GetElementoFromLevel(commandData.Application.ActiveUIDocument.Document, typeof(Opening), nivelActual);

                ReferenceWithContext ref2 = ri.FindNearest(new XYZ(-53.2, -5.29, 23.6031), new XYZ(1, 0, 0));
                ReferenceWithContext ref3 = ri.FindNearest(new XYZ(-54.98, -5.29, 24.6031), new XYZ(1, 0, 0));
                IList<ReferenceWithContext> ref4 = ri.Find(new XYZ(-52.98, -5.29, 24.6031), new XYZ(-1, 0, 0));

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
