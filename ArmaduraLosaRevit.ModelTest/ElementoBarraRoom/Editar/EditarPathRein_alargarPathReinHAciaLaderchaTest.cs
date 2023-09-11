using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Editar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model;
using ArmaduraLosaRevit.Model.EditarPath;

namespace ArmaduraLosaRevit.ModelTest.PelotaLosas
{
    [TestFixture]
    public class EditarPathRein_alargarPathReinHAciaLaderchaTest
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;


        public EditarPathRein_alargarPathReinHAciaLaderchaTest()
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
        public void alargarPathReinHAciaLadercha_roomAngulo0 ()
        {
            int idPathd = 1692284;

            double DeltaLargoFinal_LArgoInicial = AlarganfoDerecha(idPathd);

            Assert.IsTrue(Math.Abs(DeltaLargoFinal_LArgoInicial) == 5);
        }
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLadercha_roomAngulo45()
        {
            int idPathd = 1656041;

            double DeltaLargoFinal_LArgoInicial = AlarganfoDerecha(idPathd);

            Assert.IsTrue(Math.Abs(DeltaLargoFinal_LArgoInicial) == 5);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLadercha_roomAnguloMenos45()
        {
            int idPathd = 1656058;
           
           double DeltaLargoFinal_LArgoInicial= AlarganfoDerecha(idPathd);

            Assert.IsTrue(Math.Abs(DeltaLargoFinal_LArgoInicial) == 5);
        }

        private double AlarganfoDerecha(int idPathdl)
        {
            double largoInicial, largoFinal;
            try
            {
                Document _doc = _commandData.Application.ActiveUIDocument.Document;
                //seleccionar ´path
                int idPathd = 1647406;
                ElementId PAthElement = new ElementId(idPathd);
                PathReinforcement pathReinforcement = (PathReinforcement)_doc.GetElement(PAthElement);

                //obtener largo inicial barr
                Parameter largoPrimaria = ParameterUtil.FindParaByName(pathReinforcement.Parameters, "Primary Bar - Length");
                largoInicial = largoPrimaria.AsDouble();

                //alargar path
                EditarPathRein editarBarra = new EditarPathRein(_uiapp, null);
                editarBarra.EditarPath(5, DireccionEdicionPathRein.Derecha);

                //obtener largo final
                largoPrimaria = ParameterUtil.FindParaByName(pathReinforcement.Parameters, "Primary Bar - Length");
                largoFinal = largoPrimaria.AsDouble();

            }
            catch (System.Exception)
            {

                throw;
            }
            return largoFinal - largoInicial;
        }



    }
}
