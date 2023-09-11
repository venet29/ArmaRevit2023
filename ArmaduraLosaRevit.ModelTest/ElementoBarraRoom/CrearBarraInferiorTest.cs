using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RTF.Applications;
using RTF.Framework;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;

namespace ArmaduraLosaRevit.ModelTest.ElementoBarraRoom
{
    [TestFixture]
    public class CrearBarraInferiorTest

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

        public CrearBarraInferiorTest()
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
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa112_Horixontal_anguloCero()
        {
        
            //int idRoom = 1167264;
            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-72.640365984, -4.052643633, 26.541994751);


            CrearBarraInferiorGEneral("f1", UbicacionLosa.Derecha, false, ptoMOuse, IdLosa);

        }

      
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_fxV2_Losa115_Horixontal_anguloCero()
        {
           
            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-72.008372696, 25.965871711, 26.541994751);

            CrearBarraInferiorGEneral("f1", UbicacionLosa.Derecha, false, ptoMOuse, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa112_Verical_anguloCero()
        {
            //Arrage 
            int IdLosa = 1111503;
  
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-71.456242179, -4.139254403, 26.541994751);
            CrearBarraInferiorGEneral("f1", UbicacionLosa.Superior, false, ptoMOuse, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa115_Verical_anguloCero()
        {
            //Arrage 
 
            int IdLosa = 1111503;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-72.778919408, 26.371574159, 26.541994751);

            CrearBarraInferiorGEneral("f1", UbicacionLosa.Superior, false, ptoMOuse, IdLosa);
        }


        //+45********************************************************************************
        //+45
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa112_Horixontal_angulo45()
        {
            //Arrage          
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-385.417359764, -30.669806023, 26.541994751);

            CrearBarraInferiorGEneral("f1", UbicacionLosa.Derecha, false, ptoMOuse, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa115_Horixontal_angulo45()
        {
            //Arrage 
         
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-403.909563670, -8.208956485, 26.541994751);
            //Act
            CrearBarraInferiorGEneral("f1", UbicacionLosa.Derecha, false, ptoMOuse, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa112_Vertical_angulo45()
        {
            //Arrage 
          
            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-385.417359764, -30.669806023, 26.541994751);

            CrearBarraInferiorGEneral("f1", UbicacionLosa.Superior, false, ptoMOuse, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa115_Vertical_angulo45()
        {
            //Arrage 

            int IdLosa = 1575157;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(-404.590052187, -9.105021976, 26.541994751);
            CrearBarraInferiorGEneral("f1", UbicacionLosa.Superior, false, ptoMOuse, IdLosa);
        }

        //-45********************************************************************************
        //-45
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa112_Horixontal_anguloMENOS45()
        {
            //Arrage 
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(242.426418207, 22.966186936, 26.541994751);
            CrearBarraInferiorGEneral("f1", UbicacionLosa.Derecha, false, ptoMOuse, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa115_Horixontal_anguloMENOS45()
        {
            //Arrage 
                    int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(262.221105744, 41.420962976, 26.541994751);
            CrearBarraInferiorGEneral("f1", UbicacionLosa.Derecha, false, ptoMOuse, IdLosa);
        }


        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa112_Vertical_anguloMENOS45()
        {                      
            //Arrage     
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(242.426418207, 22.966186936, 26.541994751);

            CrearBarraInferiorGEneral("f1", UbicacionLosa.Superior, false, ptoMOuse, IdLosa);
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void CrearBarraInferiroConPtoyLosa_Losa115_Vertical_anguloMENOS45()
        { 
            //Arrage 
            int IdLosa = 1574320;
            //pto conmouse
            XYZ ptoMOuse = new XYZ(262.947589884, 41.826750681, 26.541994751);
            CrearBarraInferiorGEneral("f1", UbicacionLosa.Superior, false, ptoMOuse, IdLosa);
        }
    


        private void CrearBarraInferiorGEneral(string tipobarra, UbicacionLosa ubicacion, bool IsBuscarTipoBarra, XYZ ptoMOuse, int IdLosa)
        {

            ElementId LosaID = new ElementId(IdLosa);
            Floor LosaSelecionado = (Floor)_doc.GetElement(LosaID);

            Result _resulatod0 = Result.Failed;

            DatosDiseñoDto _DatosDiseñoDto = new DatosDiseñoDto();
            BarraRoom newBarralosa = new BarraRoom(_commandData.Application, tipobarra, ubicacion, _DatosDiseñoDto, IsBuscarTipoBarra, ptoMOuse,null, LosaSelecionado, IsTest: true);   //f1_SUP
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
