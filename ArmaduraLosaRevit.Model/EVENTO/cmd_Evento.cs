using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Editar
{



    [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class ActivarEvento : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            Document _doc = commandData.Application.ActiveUIDocument.Document;



            commandData.Application.Application.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(EventNH.Controladorevento_DocumentChanged);
            return Result.Succeeded;
        }
    }


    [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class DesActivarEvento : IExternalCommand

    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            Document _doc = commandData.Application.ActiveUIDocument.Document;
            commandData.Application.Application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(EventNH.Controladorevento_DocumentChanged);
            return Result.Succeeded;
        }
    }
    public class EventNH
    {
        public static void Controladorevento_DocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            Document doc = e.GetDocument();

            var transactionName = e.GetTransactionNames().ToList();

            var lisADD = e.GetAddedElementIds();
            var lisErra = e.GetDeletedElementIds();
            var  lis =e.GetModifiedElementIds();
            foreach (var item in lis)
            {
                ElementId PAthElement = new ElementId(item.IntegerValue);
                Element ell = doc.GetElement(PAthElement);
            }
        }
    }

}
