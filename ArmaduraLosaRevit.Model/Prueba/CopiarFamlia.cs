using System;

using System.Collections.Generic;

using System.Text;

using System.Windows.Forms;



using Autodesk.Revit.DB;

using Autodesk.Revit.UI;

using Autodesk.Revit.ApplicationServices;

using Autodesk.Revit.Attributes;

using Autodesk.Revit.UI.Selection;
namespace ArmaduraLosaRevit.Model.Prueba
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class CopiarFamilia : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {

            UIApplication app = commandData.Application;
            Document doc = app.ActiveUIDocument.Document;
            Selection sel = app.ActiveUIDocument.Selection;
            Reference ref1 = sel.PickObject(ObjectType.Element, "pick a family instance");
            FamilyInstance inst = doc.GetElement(ref1) as FamilyInstance;

            if (inst == null)
            {
                messages = "No family instance was picked";
                return Result.Failed;
            }

            FamilySymbol sym = inst.Symbol;
            Document targetDoc = null;
            foreach (Document doc1 in app.Application.Documents)

            {
                if (doc.Title != doc1.Title)
                {
                    targetDoc = doc1;
                    break;
                }
            }

            IList<ElementId> ids = new List<ElementId>();
            foreach (var symbol in sym.Family.GetFamilySymbolIds())
            {
                ids.Add(symbol);
            }


            try
            {
                using (Transaction trans2 = new Transaction(targetDoc))
                {
                    trans2.Start("copyFamily-NH");

                    var lsit = ElementTransformUtils.CopyElements(doc, ids, targetDoc, null, new CopyPasteOptions());
                    trans2.Commit();
                    // uidoc.RefreshActiveView();
                } // fin trans 
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
            }


            return Result.Succeeded;

        }

    }
}
