using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;

namespace ArmaduraLosaRevit.ModelTest.ElementoBarraRoom
{
    [TestFixture]
    public class CrearBarraSupleTest

    {
        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;
        private TipoBarra _tipoBarra;
        private double deltaFail = 0;
        private SolicitudBarraDTO _solicitudDTO;
        private SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom;

        public CrearBarraSupleTest()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _commandData = RevitTestExecutive.CommandData;
            _uiapp = RevitTestExecutive.CommandData.Application;
         
            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _app.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
            _opt = _app.Create.NewGeometryOptions();
            _tipoBarra = TipoBarra.f1;
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


        //crear barras
        //00********************************************************************************
        //0
          
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSupleComando_Losa410Losa115_Horizontal_anguloCero()
        {
           
            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse1 = new XYZ(-85.435298935, 21.592288854, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(-77.933408244, 21.298097063, 26.541994751);
            CrearBarraSupleGEneral("s1", UbicacionLosa.Derecha, false, ptoMOuse1, ptoMOuse2, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSupleComando_Losa113Losa410_Vertical_anguloCero()
        {

            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse1 = new XYZ(-92.601982825, 7.038109898, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(-93.479574712, 13.912579684, 26.541994751);
            CrearBarraSupleGEneral("s1", UbicacionLosa.Derecha, false, ptoMOuse1, ptoMOuse2, IdLosa);
        }



        //crear barras
        //45********************************************************************************
        //0

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSupleComando_Losa410Losa115_Horizontal_angulo45()
        {

            int IdLosa = 1575157;
            XYZ ptoMOuse1 = new XYZ(-412.676361248, -20.177877601, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(-406.126218899, -14.072890557, 26.541994751);
            CrearBarraSupleGEneral("s1", UbicacionLosa.Derecha, false, ptoMOuse1, ptoMOuse2, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSupleComando_Losa113Losa410_Vertical_angulo45()
        {

            int IdLosa = 1575157;
            XYZ ptoMOuse1 = new XYZ(-406.743906447, -36.410227569, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(-412.356186753, -31.920403325, 26.541994751);
            CrearBarraSupleGEneral("s1", UbicacionLosa.Derecha, false, ptoMOuse1, ptoMOuse2, IdLosa);
        }

        //crear barras
        //-45********************************************************************************
        //0

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSupleComando_Losa410Losa115_Horizontal_anguloMENOS45()
        {

            int IdLosa = 1574320;
            XYZ ptoMOuse1 = new XYZ(251.748445936, 50.255902900, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(257.112429118, 46.274389610, 26.541994751);
            CrearBarraSupleGEneral("s1", UbicacionLosa.Derecha, false, ptoMOuse1, ptoMOuse2, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test1_EDB-2019-025-EST_detached_SOLOSUPLE.rvt")]
        public void CrearSupleComando_Losa113Losa410_Vertical_anguloMENOS45()
        {

            int IdLosa = 1574320;
            XYZ ptoMOuse1 = new XYZ(233.057452992, 44.726023330, 26.541994751);
            XYZ ptoMOuse2 = new XYZ(240.688686798, 51.030086039, 26.541994751);
            CrearBarraSupleGEneral("s1", UbicacionLosa.Derecha, false, ptoMOuse1, ptoMOuse2, IdLosa);
        }




        private void CrearBarraSupleGEneral(string tipobarra, UbicacionLosa ubicacion, bool IsBuscarTipoBarra, XYZ ptoMOuse1, XYZ ptoMOuse2, int IdLosa)
        {

            ElementId LosaID = new ElementId(IdLosa);
            Floor LosaSelecionado = (Floor)_doc.GetElement(LosaID);

            Result _resulatod0 = Result.Failed;
            DatosDiseñoDto _DatosDiseñoDto = new DatosDiseñoDto();
            BarraRoom newBarralosa = new BarraRoom(_commandData.Application, tipobarra, ubicacion, _DatosDiseñoDto, IsBuscarTipoBarra, ptoMOuse1, ptoMOuse2, LosaSelecionado, IsTest: true);   //f1_SUP
            if (newBarralosa.statusbarra == Result.Succeeded)
            {

                _resulatod0 = newBarralosa.CrearBarra(newBarralosa.CurvesPathreiforment,
                                        newBarralosa.LargoPathreiforment,
                                        newBarralosa.nombreSimboloPathReinforcement,
                                        newBarralosa.diametroEnMM,
                                        newBarralosa.Espaciamiento,
                                        XYZ.Zero);
            }

            Assert.IsTrue(_resulatod0 == Result.Succeeded);
        }
    }

}
