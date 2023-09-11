using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.LosaArmadura;

using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model;
using Moq;

namespace ArmaduraLosaRevit.ModelTest.PelotaLosas
{
    [TestFixture]
    public class CreardorCrearRoomConPelotaLosaTest
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;


        public CreardorCrearRoomConPelotaLosaTest()
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
        public void CreardorCrearRoomConPelotaLosa_losa100_angulo45()
        {

            Debug.Print("probando debug");

            {
                List<AnotationGeneralPelotaLosa> LISTA = ObtenerListaConAnotacionPElotaLOSa(_commandData);
                var _seleccionAnotationPelotaLosa = new Mock<ISeleccionAnotationPelotaLosa>();
                _seleccionAnotationPelotaLosa.Setup(x => x.GetAnotationPelotaLosaFromViewWithMouse(It.IsAny<UIDocument>()))
                                              .Returns(LISTA);

                var resul2 = _seleccionAnotationPelotaLosa.Object.GetAnotationPelotaLosaFromViewWithMouse(_commandData.Application.ActiveUIDocument);
                //viewPlant
                int idview = 1164163;
                ElementId viewElem = new ElementId(idview);
                View _view = (View)_doc.GetElement(viewElem);

                CreardorCrearRoomConPelotaLosa crearRoomConPelotaLosa = new CreardorCrearRoomConPelotaLosa(_commandData, _view, _seleccionAnotationPelotaLosa.Object);
                Result resul = crearRoomConPelotaLosa.Execute();
                Assert.IsTrue(resul == Result.Succeeded);

            }


        }

        private List<AnotationGeneralPelotaLosa> ObtenerListaConAnotacionPElotaLOSa(ExternalCommandData commandData)
        {
            int idPelota = 1696130;
            ElementId ElementAnotationID = new ElementId(idPelota);
            var ElementAnotation = commandData.Application.ActiveUIDocument.Document.GetElement(ElementAnotationID);

            List<AnotationGeneralPelotaLosa> LISTA = new List<AnotationGeneralPelotaLosa>();

            XYZ pointUbicacion = ((Location)ElementAnotation.Location as LocationPoint).Point;
            ParameterSet pars = ElementAnotation.Parameters;
            var espesor = ParameterUtil.FindParaByName(pars, "ESPESOR");
            var numero = ParameterUtil.FindParaByName(pars, "NUMERO");
            var angle = ParameterUtil.FindParaByName(pars, "ANGULO");
            AnotationGeneralPelotaLosa item_ = new AnotationGeneralPelotaLosa()
            { PelotaLosa = ElementAnotation, Numero = numero.AsString(), Espesor = espesor.AsString(), PointUbicacion = pointUbicacion, Angulo = angle.AsDouble() };
            LISTA.Add(item_);
            return LISTA;
        }
    }
}