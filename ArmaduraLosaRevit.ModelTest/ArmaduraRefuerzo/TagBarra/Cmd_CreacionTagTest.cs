using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.ModelTest.PelotaLosas
{
    [TestFixture]
    public class Cmd_CreacionTagTest
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;


        public Cmd_CreacionTagTest()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _commandData = RevitTestExecutive.CommandData;
            _uiapp = RevitTestExecutive.CommandData.Application;

            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _app.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
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
        public void ObtenerLaUbicacionDeLosTag_A_B_C_D_E_L_F_Utilizando4PtosPathReinYPtoSelecconMOuse()
        {

            XYZ ptosMOuse = new XYZ(-47.930218797, 195.982167118, 26.541994751);
            List<XYZ> listaPtosBorde = new List<XYZ>();
            listaPtosBorde.Add(new XYZ(-54.166666667, 201.464566929, 26.541994751));
            listaPtosBorde.Add(new XYZ(-54.166666667, 192.015748032, 26.541994751));
            listaPtosBorde.Add(new XYZ(-37.483595801, 192.015748032, 26.541994751));
            listaPtosBorde.Add(new XYZ(-37.483595801, 201.464566929, 26.541994751));


            SolicitudBarraDTO _solicitudBarraDTO = new SolicitudBarraDTO(_uiapp, "f4", UbicacionLosa.Derecha, TipoConfiguracionBarra.refuerzoInferior, false);
            IGeometriaTag _geometriaTag = FactoryGeomTag.CrearGeometriaTag(_doc, ptosMOuse, _solicitudBarraDTO, listaPtosBorde);

            Assert.IsTrue(_geometriaTag != null);
            //calcula ptos
            _geometriaTag.M1_ObtnerPtosInicialYFinalDeBarra(Util.GradosToRadianes(0));
            _geometriaTag.M2_CAlcularPtosDeTAg();


            Assert.IsTrue(_geometriaTag.listaTag.Count == 7);


            Assert.IsTrue(_geometriaTag.listaTag[0].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_A_50_0");
            Assert.IsTrue(_geometriaTag.listaTag[0].posicion.DistanceTo(new XYZ(-52.362204724, 196.769568693, 26.541994751)) < 0.1);

            Assert.IsTrue(_geometriaTag.listaTag[1].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_B_50_0");
            Assert.IsTrue(_geometriaTag.listaTag[1].posicion.DistanceTo(new XYZ(-54.560367454, 196.211825911, 26.541994751)) < 0.1);

            Assert.IsTrue(_geometriaTag.listaTag[2].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_C_50_0");
            Assert.IsTrue(_geometriaTag.listaTag[2].posicion.DistanceTo(new XYZ(-49.406596750, 195.686891528, 26.541994751)) < 0.1);

            Assert.IsTrue(_geometriaTag.listaTag[3].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_F_50_0");
            Assert.IsTrue(_geometriaTag.listaTag[3].posicion.DistanceTo(new XYZ(-49.570638745, 196.211825911, 26.541994751)) < 0.1);

            Assert.IsTrue(_geometriaTag.listaTag[4].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_L_50_0");
            Assert.IsTrue(_geometriaTag.listaTag[4].posicion.DistanceTo(new XYZ(-46.289798850, 196.244634310, 26.541994751)) < 0.1);

            Assert.IsTrue(_geometriaTag.listaTag[5].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_D_50_0");
            Assert.IsTrue(_geometriaTag.listaTag[5].posicion.DistanceTo(new XYZ(-37.057086614, 196.211825911, 26.541994751)) < 0.1);

            Assert.IsTrue(_geometriaTag.listaTag[6].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_E_50_0");
            Assert.IsTrue(_geometriaTag.listaTag[6].posicion.DistanceTo(new XYZ(-38.959973753, 196.769568693, 26.541994751)) < 0.1);

        }
      


    }
}
