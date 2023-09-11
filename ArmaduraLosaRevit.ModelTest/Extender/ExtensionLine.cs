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
    public class ExtensionLine
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;


        public ExtensionLine()
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
        public void ExtenederPtosDelineaUnaDistanciaAmbosLados()
        {
     
            //*****************************************************************************************************************
            //1 Arrange
            List<XYZ> list = new List<XYZ>();


            XYZ p1 = new XYZ(1, 1, 0);
            XYZ p2 = new XYZ(5, 5, 0);
            double largoInicial = p1.DistanceTo(p2);
            //Act    45 grados
            var r45 = UtilBarras.extenderLineaDistancia(p1, p2, 1);

            //assert
            Assert.IsTrue(r45[0].IsAlmostEqualTo(new XYZ(5.707106781, 5.707106781, 0.000000000)));
            Assert.IsTrue(r45[1].IsAlmostEqualTo(new XYZ(0.292893219, 0.292893219, 0.000000000)));
            Assert.IsTrue(Util.IsEqual(r45[0].DistanceTo(r45[1]), largoInicial + 2));

            //*****************************************************************************************************************
            //2 Arrange
            //Act  -135
            var resMenos135 = UtilBarras.extenderLineaDistancia(p2, p1, 1);

            //assert
            Assert.IsTrue(resMenos135[0].IsAlmostEqualTo(new XYZ(5.707106781, 5.707106781, 0.000000000)));
            Assert.IsTrue(resMenos135[1].IsAlmostEqualTo(new XYZ(0.292893219, 0.292893219, 0.000000000)));
            Assert.IsTrue(Util.IsEqual(resMenos135[0].DistanceTo(resMenos135[1]), largoInicial + 2));

            //*****************************************************************************************************************
            //3 Arrange
            XYZ p3 = new XYZ(1, 5, 0);
            XYZ p4 = new XYZ(5, 1, 0);
            largoInicial = p3.DistanceTo(p4);
            //Act  -45
            var resMenos45 = UtilBarras.extenderLineaDistancia(p3, p4, 1);

            Assert.IsTrue(resMenos45[0].IsAlmostEqualTo(new XYZ(5.707106781, 0.292893219, 0.000000000)));
            Assert.IsTrue(resMenos45[1].IsAlmostEqualTo(new XYZ(0.292893219, 5.707106781, 0.000000000)));
            Assert.IsTrue(Util.IsEqual(resMenos45[0].DistanceTo(resMenos45[1]), largoInicial + 2));


            //*****************************************************************************************************************
            //4 Arrange
            //Act  135
            var res135 = UtilBarras.extenderLineaDistancia(p4, p3, 1);

            //assert
            Assert.IsTrue(res135[0].IsAlmostEqualTo(new XYZ(5.707106781, 0.292893219, 0.000000000)));
            Assert.IsTrue(res135[1].IsAlmostEqualTo(new XYZ(0.292893219, 5.707106781, 0.000000000)));
            Assert.IsTrue(Util.IsEqual(res135[0].DistanceTo(res135[1]), largoInicial + 2));


            //*****************************************************************************************************************
            //5 Arrange
            XYZ p5 = new XYZ(1, 0, 0);
            XYZ p6 = new XYZ(1, 5, 0);
            largoInicial = p5.DistanceTo(p6);
            //Act  90 grados
            var r90 = UtilBarras.extenderLineaDistancia(p5, p6, 1);

            //assert
            Assert.IsTrue(r90[0].IsAlmostEqualTo(new XYZ(1, 6, 0.000000000)));
            Assert.IsTrue(r90[1].IsAlmostEqualTo(new XYZ(1, -1, 0.000000000)));
            Assert.IsTrue(Util.IsEqual(r90[0].DistanceTo(r90[1]), largoInicial + 2));

            //*****************************************************************************************************************
            // Arrange
            //Act  180 grados
            var r270 = UtilBarras.extenderLineaDistancia(p6, p5, 1);

            //assert
            Assert.IsTrue(r270[0].IsAlmostEqualTo(new XYZ(1, 6, 0.000000000)));
            Assert.IsTrue(r270[1].IsAlmostEqualTo(new XYZ(1, -1, 0.000000000)));
            Assert.IsTrue(Util.IsEqual(r270[0].DistanceTo(r270[1]), largoInicial + 2));


            //*****************************************************************************************************************
            //7 Arrange
            XYZ p7 = new XYZ(1, 1, 0);
            XYZ p8 = new XYZ(5, 1, 0);
            largoInicial = p8.DistanceTo(p7);
            //Act  90 grados
            var r0 = UtilBarras.extenderLineaDistancia(p7, p8, 1);

            //assert
            Assert.IsTrue(r0[0].IsAlmostEqualTo(new XYZ(6, 1, 0.000000000)));
            Assert.IsTrue(r0[1].IsAlmostEqualTo(new XYZ(0, 1, 0.000000000)));
            Assert.IsTrue(Util.IsEqual(r0[0].DistanceTo(r0[1]), largoInicial + 2));

            //*****************************************************************************************************************
            //8 Arrange
            //Act  180 grados
            var r180 = UtilBarras.extenderLineaDistancia(p8, p7, 1);

            //assert
            Assert.IsTrue(r180[0].IsAlmostEqualTo(new XYZ(6, 1, 0.000000000)));
            Assert.IsTrue(r180[1].IsAlmostEqualTo(new XYZ(0, 1, 0.000000000)));
            Assert.IsTrue(Util.IsEqual(r180[0].DistanceTo(r180[1]), largoInicial + 2));
        }

    }
}
