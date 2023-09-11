using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model;

namespace ArmaduraLosaRevit.ModelTest.PelotaLosas
{
    [TestFixture]
    public class UtilTest
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;


        public UtilTest()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _commandData = RevitTestExecutive.CommandData;
            _uiapp = RevitTestExecutive.CommandData.Application;

            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _app.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
            _opt = _app.Create.NewGeometryOptions();

        }


        /// <summary>
        /// Implements the FailuresProcessing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FailuresProcessing(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
            FailuresAccessor failuresAccessor = e.GetFailuresAccessor();
            IList<FailureMessageAccessor> failures = failuresAccessor.GetFailureMessages();

            foreach (FailureMessageAccessor f in failures)
            {
                FailureSeverity fseverity = failuresAccessor.GetSeverity();

                if (fseverity == FailureSeverity.Warning)
                {
                    failuresAccessor.DeleteWarning(f);
                }
                else
                {
                    failuresAccessor.ResolveFailure(f);
                    e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit);
                }
            }
            e.SetProcessingResult(FailureProcessingResult.Continue);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void VerificarElREsultadoENtrega_AnguloEntre2PtosGrados_para4direcciones()
        {

            double Angulo = Util.GetAnguloVectoresEnGrados_enPlanoXY(new XYZ(1, 0, 0));
            Assert.IsTrue(Angulo == 0);
            Angulo = Util.GetAnguloVectoresEnGrados_enPlanoXY(new XYZ(1, 1, 0));
            Assert.IsTrue(Angulo == 45);
            Angulo = Util.GetAnguloVectoresEnGrados_enPlanoXY(new XYZ(0, 1, 0));
            Assert.IsTrue(Angulo == 90);
            Angulo = Util.GetAnguloVectoresEnGrados_enPlanoXY(new XYZ(-1, 1, 0));
            Assert.IsTrue(Angulo == 135);
            Angulo = Util.GetAnguloVectoresEnGrados_enPlanoXY(new XYZ(-1, 0, 0));
            Assert.IsTrue(Angulo == 180);
            Angulo = Util.GetAnguloVectoresEnGrados_enPlanoXY(new XYZ(-1, -1, 0));
            Assert.IsTrue(Angulo == -135);
            Angulo = Util.GetAnguloVectoresEnGrados_enPlanoXY(new XYZ(0, -1, 0));
            Assert.IsTrue(Angulo == -90);
            Angulo = Util.GetAnguloVectoresEnGrados_enPlanoXY(new XYZ(1, -1, 0));
            Assert.IsTrue(Angulo == -45);
        }

    }
}
