using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Seleccionar;

namespace ArmaduraLosaRevit.ModelTest.PelotaLosas
{
    [TestFixture]
    public class SelecionarElementoParaRefuerzoTest
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;


        public SelecionarElementoParaRefuerzoTest()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _commandData = RevitTestExecutive.CommandData;
            _uiapp = RevitTestExecutive.CommandData.Application;

            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            //_app.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
            _opt = _app.Create.NewGeometryOptions();
            _uidoc.ActiveView = (View)_doc.GetElement(new ElementId(1164163));
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
        public void SelecionarDosMurosEn45GradosParaRefuerzoTest()
        {
            XYZ ptoInMuro1 = new XYZ(-365.27, 14.69, 26.541994095);
            int idMuro1 = 1574958;
            XYZ ptoInMuro2 = new XYZ(-360.99, 18.9701, 26.541994095);
            int idMuro2 = 1575024;


            XYZ Borde1p1 = new XYZ(-364.636901855, 15.794794083, 26.541994095);
            XYZ Borde1p2 = new XYZ(-364.172912598, 15.330813408, 26.541994095);

            XYZ Borde2p1 = new XYZ(-361.667419434, 17.836309433, 26.541994095);
            XYZ Borde2p2= new XYZ(-362.131408691, 18.300291061, 26.541994095);


            CalcularSelecionarDosMurosEn45GradosParaRefuerzoTest(ptoInMuro1, idMuro1, ptoInMuro2, idMuro2, Borde1p1, Borde1p2, Borde2p1, Borde2p2);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void SelecionarDosMurosEn0GradosParaRefuerzoTest()
        {
            XYZ ptoInMuro1 = new XYZ(-54.058420509, 14.529403107, 28.541994751);
            int idMuro1 = 784348;
            XYZ ptoInMuro2 = new XYZ(-49.275859214, 14.475055820, 28.541994751);
            int idMuro2 = 784380;


            XYZ Borde1p1 = new XYZ(-53.494094849, 14.763779640, 26.541994095);
            XYZ Borde1p2 = new XYZ(-53.494094849, 14.107611656, 26.541994095);

            XYZ Borde2p1 = new XYZ(-49.950786591, 14.763779640, 26.541994095);
            XYZ Borde2p2 = new XYZ(-49.950786591, 14.107611656, 26.541994095);


            CalcularSelecionarDosMurosEn45GradosParaRefuerzoTest(ptoInMuro1, idMuro1, ptoInMuro2, idMuro2, Borde1p1, Borde1p2, Borde2p1, Borde2p2);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void SelecionarDosMurosEnGradosParaRefuerzoTest()
        {
            XYZ ptoInMuro1 = new XYZ(252.419589823, 30.155170584, 28.541994751);
            int idMuro1 = 1574153;
            XYZ ptoInMuro2 = new XYZ(255.607058232, 26.967702176, 28.541994751);
            int idMuro2 = 1574155;


            XYZ Borde1p1 = new XYZ(252.727615356, 29.407293320, 26.541994095);
            XYZ Borde1p2 = new XYZ(253.192352295, 29.870508194, 26.541994095);

            XYZ Borde2p1 = new XYZ(255.228973389, 26.897668839, 26.541994095);
            XYZ Borde2p2 = new XYZ(255.693725586, 27.360885620, 26.541994095);


            CalcularSelecionarDosMurosEn45GradosParaRefuerzoTest(ptoInMuro1, idMuro1, ptoInMuro2, idMuro2, Borde1p1, Borde1p2, Borde2p1, Borde2p2);
        }

        public void CalcularSelecionarDosMurosEn45GradosParaRefuerzoTest(XYZ ptoInMuro1 , int idMuro1, XYZ ptoInMuro2, int idMuro2 , XYZ Borde1p1, XYZ Borde1p2, XYZ Borde2p1, XYZ Borde2p2)
        {

            //muros 1
            SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo1 = new SeleccinarMuroRefuerzo(_commandData.Application);
            if (!helperSeleccinarMuroRefuerzo1.EjecutarSeleccion(ptoInMuro1, idMuro1)) return; ;


            //muro 2
            SeleccinarMuroRefuerzo helperSeleccinarMuroRefuerzo2 = new SeleccinarMuroRefuerzo(_commandData.Application);
            if (!helperSeleccinarMuroRefuerzo2.EjecutarSeleccion(ptoInMuro2, idMuro2)) return;
            // helperSeleccinarMuroRefuerzo2.SeleccionarElemento();

            //intersectar1
            helperSeleccinarMuroRefuerzo1.GetBordeIntersectaConPto(helperSeleccinarMuroRefuerzo2.pto1SeleccionadoConMouse);
            Assert.IsTrue(helperSeleccinarMuroRefuerzo1.ListaPtosBordeMuroIntersectado[0].DistanceTo(Borde1p1) < 0.01);
            Assert.IsTrue(helperSeleccinarMuroRefuerzo1.ListaPtosBordeMuroIntersectado[1].DistanceTo(Borde1p2) < 0.01);

            //intersectar2
            helperSeleccinarMuroRefuerzo2.GetBordeIntersectaConPto(helperSeleccinarMuroRefuerzo1.pto1SeleccionadoConMouse);
            Assert.IsTrue(helperSeleccinarMuroRefuerzo2.ListaPtosBordeMuroIntersectado[0].DistanceTo(Borde2p1) < 0.01);
            Assert.IsTrue(helperSeleccinarMuroRefuerzo2.ListaPtosBordeMuroIntersectado[1].DistanceTo(Borde2p2) < 0.01);


        }

    }
}
