using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Cambiar.CambiarEspesor;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Moq;

namespace ArmaduraLosaRevit.ModelTest.CambiarEspesorLosa
{
    [TestFixture]
    public class CmdCambiarEspesorAParametroPathTest
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;
        private Level _Level;

        public CmdCambiarEspesorAParametroPathTest()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _commandData = RevitTestExecutive.CommandData;
            _uiapp = RevitTestExecutive.CommandData.Application;

            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _app.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
            _opt = _app.Create.NewGeometryOptions();
            _Level = _doc.ActiveView.GenLevel;
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
        public void VeridicarCOmandoCOmpleto_CAmbiarEspesorLosa_cambiaEspesorEnRoomYPAthReiformen()
        {
            int idPathd = 1111503;
            ElementId RoomElem = new ElementId(idPathd);
            Floor floor = (Floor)_doc.GetElement(RoomElem);
            Level levelLOsa = _doc.GetElement(floor.LevelId) as Level;


            //viewPlant
            int idview =  1164163;
            ElementId viewElem = new ElementId(idview);
            View _view = (View)_doc.GetElement(viewElem);


            var ISeleccionarLosaConMoseMock = new Mock<ISeleccionarLosaConMouse>();
            ISeleccionarLosaConMoseMock.Setup(ss => ss.M1_SelecconarFloor()).Returns(floor);
            ISeleccionarLosaConMoseMock.Setup(ss => ss.M2_ObtenerEspesorLosaFoot()).Returns(15);

           // ISeleccionarLosaConMose ISeleccionarLosaConMose = new SeleccionarLosaConMose(_commandData);
            IManejadorPathCambioEspesor iManejadorPathCambioEspesor = new ManejadorPathCambioEspesor(_commandData);
            iManejadorPathCambioEspesor._view = _view;
            IManejadorRoomCambioEspesor iManejadorRoomCambioEspesor = new ManejadorRoomCambioEspesor(_commandData);
            CambiarEspesorLosa_CambiaPAthYRoom cambiarEspesorAParametroPath = new CambiarEspesorLosa_CambiaPAthYRoom(_commandData,
                                                                                                                     ISeleccionarLosaConMoseMock.Object,
                                                                                                                     iManejadorPathCambioEspesor,
                                                                                                                     iManejadorRoomCambioEspesor);
            cambiarEspesorAParametroPath.Ejecutar();


            Assert.IsTrue(cambiarEspesorAParametroPath.CAntidadRoomCAMbiados == 17);




        }


     


    }
}
