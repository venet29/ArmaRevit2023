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
    public class CreadorEditarPathRein
    {

        private Document _doc;
        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Application _app;
        private Options _opt;
        private int _ValorDespla;
        private int _idPathd45;
        private int _idPathdMENOS45;

        public CreadorEditarPathRein()
        {
            _doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            _commandData = RevitTestExecutive.CommandData;
            _uiapp = RevitTestExecutive.CommandData.Application;

            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _app.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
            _opt = _app.Create.NewGeometryOptions();
            _ValorDespla = 5;

            
             _idPathd45 = 1656041;
            _idPathdMENOS45 = 1656058;
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

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange
                int idPathd = 1692284;
                double largoInicial = 21.0793963254592;
                XYZ OrigenFinal_esperado = new XYZ(-76.3648293963255, 9.64566929133856, 26.5419947506562);

                //act
                PathReinforcement path = EjecutarMoverPath(idPathd, DireccionEdicionPathRein.Derecha, 5);

                double LargoFinal = ObtenerLArgoPath(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial)- _ValorDespla) <0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }



        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaIzquirda_roomAngulo0()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange
                int idPathd = 1692284;
                double largoInicial = 21.0793963254592;
                XYZ OrigenFinal_esperado = new XYZ(-81.3648293963255, 9.64566929133856, 26.5419947506562);

                //act
                PathReinforcement path = EjecutarMoverPath(idPathd, DireccionEdicionPathRein.Izquierda, 5);

                double LargoFinal = ObtenerLArgoPath(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaSuperior_roomAngulo0()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange
                int idPathd = 1692284;
                double largoInicial = 21.981627296588;
                XYZ OrigenFinal_esperado = new XYZ(-81.3648293963255, 14.64566929133856, 26.5419947506562);

                //act
                PathReinforcement path = EjecutarMoverPath(idPathd, DireccionEdicionPathRein.Superior, 5);

                double LargoFinal = ObtenerLargoModelCurve(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaInferior_roomAngulo0()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange
                int idPathd = 1692284;
                double largoInicial = 21.981627296588;
                XYZ OrigenFinal_esperado = new XYZ(-81.3648293963255, 9.64566929133856, 26.5419947506562);

                //act
                PathReinforcement path = EjecutarMoverPath(idPathd, DireccionEdicionPathRein.Inferior, 5);

                double LargoFinal = ObtenerLargoModelCurve(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }



        //45**************************************************************************************

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaDerecha_roomAngulo45()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange
                PathReinforcement pathInicial   = ObtenerPathReinf(_idPathd45);
                double largoInicial = ObtenerLArgoPath(pathInicial);

                XYZ OrigenFinal_esperado = new XYZ(-396.584294807685, -23.390706706716, 26.5419947506562);

                //act
                PathReinforcement path = EjecutarMoverPath(pathInicial, DireccionEdicionPathRein.Derecha, 5);

                double LargoFinal = ObtenerLArgoPath(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }



        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaIzquirda_roomAngulo45()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange

                PathReinforcement pathInicial = ObtenerPathReinf(_idPathd45);
                double largoInicial = ObtenerLArgoPath(pathInicial);
                //es el mismo que original  (-400.119828713617, -26.9262406126487, 26.5419947506562)
                XYZ OrigenFinal_esperado = M1_1_ObtenerOrigenPathReinf(pathInicial);

                //act
                PathReinforcement path = EjecutarMoverPath(pathInicial, DireccionEdicionPathRein.Izquierda, 5);

                double LargoFinal = ObtenerLArgoPath(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaSuperior_roomAngulo45()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange

                PathReinforcement pathInicial = ObtenerPathReinf(_idPathd45);
                double largoInicial = ObtenerLargoModelCurve(pathInicial);//21.981627296588
                XYZ OrigenFinal_esperado = new XYZ(-403.65536261955, -23.390706706716, 26.5419947506562);

                //act
                PathReinforcement path = EjecutarMoverPath(pathInicial, DireccionEdicionPathRein.Superior, 5);

                double LargoFinal = ObtenerLargoModelCurve(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaInferior_roomAngulo45()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange

                PathReinforcement pathInicial = ObtenerPathReinf(_idPathd45);
                double largoInicial = ObtenerLargoModelCurve(pathInicial);
                XYZ OrigenFinal_esperado = M1_1_ObtenerOrigenPathReinf(pathInicial);

                //act
                PathReinforcement path = EjecutarMoverPath(pathInicial, DireccionEdicionPathRein.Inferior, 5);

                double LargoFinal = ObtenerLargoModelCurve(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }



        //45**************************************************************************************

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaDerecha_roomAnguloMENOS45()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange
                PathReinforcement pathInicial = ObtenerPathReinf(_idPathdMENOS45);
                double largoInicial = ObtenerLArgoPath(pathInicial);

                XYZ OrigenFinal_esperado = new XYZ(248.371837537548, 34.2422029051054, 26.5419947506562);

                //act
                PathReinforcement path = EjecutarMoverPath(pathInicial, DireccionEdicionPathRein.Derecha, 5);

                double LargoFinal = ObtenerLArgoPath(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }



        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaIzquirda_roomAnguloMENOS45()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange

                PathReinforcement pathInicial = ObtenerPathReinf(_idPathdMENOS45);
                double largoInicial = ObtenerLArgoPath(pathInicial);
                //es el mismo que original  (-400.119828713617, -26.9262406126487, 26.5419947506562)
                XYZ OrigenFinal_esperado = M1_1_ObtenerOrigenPathReinf(pathInicial);

                //act
                PathReinforcement path = EjecutarMoverPath(pathInicial, DireccionEdicionPathRein.Izquierda, 5);

                double LargoFinal = ObtenerLArgoPath(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }

        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaSuperior_roomAnguloMENOS45()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange

                PathReinforcement pathInicial = ObtenerPathReinf(_idPathdMENOS45);
                double largoInicial = ObtenerLargoModelCurve(pathInicial);//21.981627296588
                XYZ OrigenFinal_esperado = new XYZ(248.383496602098, 41.3132611049942, 26.5419947506562);

                //act
                PathReinforcement path = EjecutarMoverPath(pathInicial, DireccionEdicionPathRein.Superior, 5);

                double LargoFinal = ObtenerLargoModelCurve(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }
        [Test]
        [TestModel(@"j:/_revit/PROYECTO/2020/PROYECTO BUCHEFT/Test2_EDB-2019-025-EST_detachedTest.rvt")]
        public void alargarPathReinHAciaLaInferior_roomAnguloMENOS45()
        {

            using (TransactionGroup transGroup = new TransactionGroup(_doc))
            {
                transGroup.Start("Transaction Group");
                //arrange

                PathReinforcement pathInicial = ObtenerPathReinf(_idPathdMENOS45);
                double largoInicial = ObtenerLargoModelCurve(pathInicial);
                XYZ OrigenFinal_esperado = M1_1_ObtenerOrigenPathReinf(pathInicial);

                //act
                PathReinforcement path = EjecutarMoverPath(pathInicial, DireccionEdicionPathRein.Inferior, 5);

                double LargoFinal = ObtenerLargoModelCurve(path);
                XYZ newOrigen = M1_1_ObtenerOrigenPathReinf(path);

                //asser
                Assert.IsTrue(Math.Abs(Math.Abs(LargoFinal - largoInicial) - _ValorDespla) < 0.001);
                Assert.IsTrue(OrigenFinal_esperado.DistanceTo(newOrigen) < 0.1);
                transGroup.RollBack();
            }
        }





        //******************************************************************************************************************
        public PathReinforcement EjecutarMoverPath(int idPathd, DireccionEdicionPathRein direccionEdicionPathRein,double desplazamiento)
        {
            //seleccionar
            PathReinforcement pathReinforcement = ObtenerPathReinf(idPathd);
            try
            {
                //desplzar
                EditarPathRein editarPathRein = new EditarPathRein(_uiapp, null);
                editarPathRein.EditarPath(desplazamiento, direccionEdicionPathRein);
            }
            catch (System.Exception ex)
            {
                pathReinforcement = null;
            }
            return pathReinforcement;
        }

        public PathReinforcement EjecutarMoverPath(PathReinforcement pathReinforcement, DireccionEdicionPathRein direccionEdicionPathRein, double desplazamiento)
        {
            //seleccionar

            try
            {
                //desplzar
                EditarPathRein editarPathRein = new EditarPathRein(_uiapp, null);
                editarPathRein.EditarPath(desplazamiento, direccionEdicionPathRein);
            }
            catch (System.Exception ex)
            {
                pathReinforcement = null;
            }
            return pathReinforcement;
        }


        private PathReinforcement ObtenerPathReinf(int idPathd)
        {
            ElementId PAthElement = new ElementId(idPathd);
            PathReinforcement pathReinforcement = (PathReinforcement)_doc.GetElement(PAthElement);
            return pathReinforcement;
        }

        private double ObtenerLArgoPath(PathReinforcement path)
        {
            Parameter largoPrimaria = ParameterUtil.FindParaByName(path.Parameters, "Primary Bar - Length");
            double LargoFinal = largoPrimaria.AsDouble();
            return LargoFinal;
        }

        private XYZ M1_1_ObtenerOrigenPathReinf(PathReinforcement pathReinforcement)
        {

            ElementId elId = pathReinforcement.GetCurveElementIds()[0];

            ModelLine MOdeLine = _doc.GetElement2(elId) as ModelLine;
            if (MOdeLine == null) return XYZ.Zero;
            Line LineCUrve = MOdeLine.GeometryCurve as Line;
            XYZ ptOrigen = LineCUrve.Origin;
            return ptOrigen;
        }


        protected double ObtenerLargoModelCurve(PathReinforcement pathReinforcement)
        {

            ElementId elId = pathReinforcement.GetCurveElementIds()[0];

            ModelLine MOdeLine = _doc.GetElement2(elId) as ModelLine;
            if (MOdeLine == null) return 0;
            Line LineCUrve = MOdeLine.GeometryCurve as Line;
     
            return LineCUrve.Length;
        }

    }
}
