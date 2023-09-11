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

using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model;

namespace ArmaduraLosaRevit.ModelTest.CambiarEspesorLosa
{
    [TestFixture]
    public class CambiarEspesorLosa_CambiaParametroEspesorPathTest
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;


        public CambiarEspesorLosa_CambiaParametroEspesorPathTest()
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
        public void CrearUnObjeto_PathCambioEspesorDTO()
        {

            Debug.Print("probando debug");
            int idPathd = 1647406;
            ElementId PAthElement = new ElementId(idPathd);
            PathReinforcement pathReinforcement = (PathReinforcement)_doc.GetElement(PAthElement);
            CambiarEspesorCambio(pathReinforcement, "C_,D_");

            PathCambioEspesorDTO pathCambioEspesorDTO = new PathCambioEspesorDTO(pathReinforcement,true);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores.Count==2);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores[0] == "C_");
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores[1] == "D_");


            CambiarEspesorCambio(pathReinforcement, "C_");
            pathCambioEspesorDTO = new PathCambioEspesorDTO(pathReinforcement, true);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores.Count == 1);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores[0] == "C_");
           
  

        }
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearUnObjeto_PathCambioEspesorDTO_ComprobandoVAcio()
        {

            Debug.Print("probando debug");
            int idPathd = 1647406;
            ElementId PAthElement = new ElementId(idPathd);
            PathReinforcement pathReinforcement = (PathReinforcement)_doc.GetElement(PAthElement);
  
            CambiarEspesorCambio(pathReinforcement, "");
            PathCambioEspesorDTO pathCambioEspesorDTO = new PathCambioEspesorDTO(pathReinforcement, true);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores.Count == 0);


        }
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearUnObjeto_PathCambioEspesorDTO_CasosCOnError()
        {

            Debug.Print("probando debug");
            int idPathd = 1647406;
            ElementId PAthElement = new ElementId(idPathd);
            PathReinforcement pathReinforcement = (PathReinforcement)_doc.GetElement(PAthElement);
  

            CambiarEspesorCambio(pathReinforcement, "pC_");
            PathCambioEspesorDTO pathCambioEspesorDTO = new PathCambioEspesorDTO(pathReinforcement, true);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores.Count == 0);
            Assert.IsTrue(pathCambioEspesorDTO.cantidadError == 1);

            CambiarEspesorCambio(pathReinforcement, "Cd_");
            pathCambioEspesorDTO = new PathCambioEspesorDTO(pathReinforcement, true);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores.Count == 0);
            Assert.IsTrue(pathCambioEspesorDTO.cantidadError == 1);

            CambiarEspesorCambio(pathReinforcement, "d_");
            pathCambioEspesorDTO = new PathCambioEspesorDTO(pathReinforcement, true);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores.Count == 0);
            Assert.IsTrue(pathCambioEspesorDTO.cantidadError == 1);

            CambiarEspesorCambio(pathReinforcement, ",");
            pathCambioEspesorDTO = new PathCambioEspesorDTO(pathReinforcement, true);
            Assert.IsTrue(pathCambioEspesorDTO.ListaEspesores.Count == 0);
            Assert.IsTrue(pathCambioEspesorDTO.cantidadError == 2);
        }

        //cambia previamente el parametro para ver si lee correctamente y no se generan errores
        public void CambiarEspesorCambio(Element PAth,string txto)
        {

            try
            {
                using (Transaction trans = new Transaction(_uidoc.Document))
                {
                    trans.Start("CAmbiando espesor");
                    ParameterUtil.SetParaInt(PAth, "EspesorCambio", txto);
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                //Util.ErrorMsg("Error al sectibar SectionBox");

            }
        }


        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void Cmd_CAmbiarPArametrosEspesor_PAthReingEnLOSa_1pathREinf()
        {

            Debug.Print("probando debug");
            int idPathd = 1647406;
            ElementId PAthElement = new ElementId(idPathd);
            PathReinforcement pathReinforcement = (PathReinforcement)_doc.GetElement(PAthElement);
            List<PathReinforcement> lista = new List<PathReinforcement>();
            lista.Add(pathReinforcement);

            ManejadorPathCambioEspesor ManejadorPathCambioEspesor = new ManejadorPathCambioEspesor(_commandData, true);
            ManejadorPathCambioEspesor.M4_ObtenerLosEspesoresCAmbiar(lista);
            ManejadorPathCambioEspesor.M5_CAmbiarPArametroEspesorPath(Util.CmToFoot(60));

            Assert.IsTrue(ManejadorPathCambioEspesor.cantidadError== 0);
            Assert.IsTrue(ManejadorPathCambioEspesor.cantidadPathCambiado == 1);

        }

    }
}
