using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Prueba.ErrorNh
{
 
    public class RevitApplication : IExternalApplication
    {
        private SyntheticFailureReplacement failureReplacement;

        public Result OnStartup(UIControlledApplication application)
        {
            Util.InfoMsg("OnStartup");
            failureReplacement = new SyntheticFailureReplacement();

            application.ControlledApplication.FailuresProcessing += ControlledApplicationOnFailuresProcessing;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            Util.InfoMsg("OnShutdown");
            application.ControlledApplication.FailuresProcessing -= ControlledApplicationOnFailuresProcessing;

            return Result.Succeeded;
        }

        private void ControlledApplicationOnFailuresProcessing(object sender, FailuresProcessingEventArgs e)
        {
            var failuresAccessor = e.GetFailuresAccessor();

            var failureMessages = failuresAccessor
              .GetFailureMessages(FailureSeverity.Error)
              .Where(x => x.GetFailureDefinitionId() == BuiltInFailures.DPartFailures.DeletingDPartWillDeleteMorePartsError)
              .ToList();

            if (failureMessages.Any())
            {
                var failureHandlingOptions = failuresAccessor.GetFailureHandlingOptions();

                failureHandlingOptions.SetClearAfterRollback(true);

                failuresAccessor.SetFailureHandlingOptions(failureHandlingOptions);

                e.SetProcessingResult(FailureProcessingResult.ProceedWithRollBack);

                failureReplacement.PostFailure(failureMessages.SelectMany(x => x.GetFailingElementIds()));
            }
        }
    }

    public class SyntheticFailureReplacement : IExternalEventHandler
    {
        private readonly ExternalEvent externalEvent;

        private readonly List<ElementId> failingElementIds = new List<ElementId>();

        private readonly FailureDefinitionId failureDefinitionId = new FailureDefinitionId(new Guid("bc0dc2ef-d928-42e4-9c9b-521cb822d3fd"));

        public SyntheticFailureReplacement()
        {
            externalEvent = ExternalEvent.Create(this);

            FailureDefinition.CreateFailureDefinition(failureDefinitionId, FailureSeverity.Warning, "My accurate message replacement");
        }

        public void PostFailure(IEnumerable<ElementId> failingElements)
        {
            failingElementIds.Clear();
            failingElementIds.AddRange(failingElements);

            externalEvent.Raise();
        }

        public void Execute(UIApplication app)
        {
            var document = app.ActiveUIDocument.Document;

            using (var transaction = new Transaction(document, "auxiliary transaction"))
            {
                var failureHandlingOptions = transaction.GetFailureHandlingOptions();

                failureHandlingOptions.SetForcedModalHandling(false);

                transaction.SetFailureHandlingOptions(failureHandlingOptions);

                transaction.Start();

                var failureMessage = new FailureMessage(failureDefinitionId);

                failureMessage.SetFailingElements(failingElementIds);

                document.PostFailure(failureMessage);

                transaction.Commit();
            }
        }

        public string GetName() => nameof(SyntheticFailureReplacement);
    }
}
