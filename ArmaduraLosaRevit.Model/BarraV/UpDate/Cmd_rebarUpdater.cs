
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.UpDate
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd_rebarUpdater : IExternalCommand
    {
        class SimpleUpdater : IUpdater
        {
            const string Id = "d42d28af-d2cd-4f07-8873-e7cfb61903d8";

            UpdaterId _updater_id { get; set; }

            public SimpleUpdater(Document doc, AddInId addInId)
            {
                _updater_id = new UpdaterId(addInId, new Guid(Id));

                RegisterUpdater(doc);
                RegisterTriggers();
            }

            public void RegisterUpdater(Document doc)
            {
                if (UpdaterRegistry.IsUpdaterRegistered(_updater_id, doc))
                {
                    UpdaterRegistry.RemoveAllTriggers(_updater_id);
                    UpdaterRegistry.UnregisterUpdater(_updater_id, doc);
                }
                UpdaterRegistry.RegisterUpdater(this, doc);
            }

            public void RegisterTriggers()
            {
                if (null != _updater_id && UpdaterRegistry.IsUpdaterRegistered(_updater_id))
                {
                    UpdaterRegistry.RemoveAllTriggers(_updater_id);
                    UpdaterRegistry.AddTrigger(_updater_id, new ElementCategoryFilter(BuiltInCategory.OST_Walls), Element.GetChangeTypeAny());
                }
            }

            public void Execute(UpdaterData data)
            {
                Document doc = data.GetDocument();

                ICollection<ElementId> ids = data.GetModifiedElementIds();

                IEnumerable<string> names = ids.Select<ElementId, string>(id => doc.GetElement(id).Name);

                TaskDialog.Show("Simple Updater", string.Join<string>(",", names));
            }

            public string GetAdditionalInformation()
            {
                return "NA";
            }

            public ChangePriority GetChangePriority()
            {
                return ChangePriority.MEPFixtures;
            }

            public UpdaterId GetUpdaterId()
            {
                return _updater_id;
            }

            public string GetUpdaterName()
            {
                return "SimpleUpdater";
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            AddInId addInId = uiapp.ActiveAddInId;

            SimpleUpdater su = new SimpleUpdater(doc, addInId);

            return Result.Succeeded;
        }
    }
}
