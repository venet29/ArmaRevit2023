using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

using RTF.Applications;
using RTF.Framework;


namespace ArmaduraLosaRevit.Test
{

    [TestFixture]
    public class Traslapo
    {
        [SetUp]
        public void Setup()
        {
            //startup logic executed before every test
        }

        [TearDown]
        public void Shutdown()
        {
            //shutdown logic executed after every test
        }
        //J:\_revit\_TEST\RevitTestFramework-Revit2019\RevitTestFramework-Revit2019\bin\AnyCPU\Debug\empty_nh.rvt
        // [TestModel(@"./empty_nh.rvt")]
        
        [TestModel(@"J:\_revit\PROYECTOS REVIT\REVITArmaduraLosaRevit\ArmaduraLosaRevit\ArmaduraLosaRevit.Test\*.rvt")]
        public void SePudoSeleccionarElemntoConElmouse()
        {
            var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            Assert.IsTrue(true);
            //Document doc;
            //UIApplication uiapp;
            //UIDocument uidoc;
            //Application app;

            //uiapp = RevitTestExecutive.CommandData.Application;
            //    uidoc = uiapp.ActiveUIDocument;
            //    app = uiapp.Application;
            //    doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;

            //var SeleccionarPathReinfomentConPtomock = new Mock<SeleccionarPathReinfomentConPto>();
            //SeleccionarPathReinfomentConPtomock.Setup(Sp => Sp.puntoSeleccionMouse)
            //    .Returns(new XYZ(10, 10, 10));
            //Assert.Equals(SeleccionarPathReinfomentConPtomock.Object.puntoSeleccionMouse, new XYZ(10, 10, 10));
        }



        [Test]
        [TestModel(@"J:\_revit\PROYECTOS REVIT\REVITArmaduraLosaRevit\ArmaduraLosaRevit\ArmaduraLosaRevit.Test\*.rvt")]
        public void CanCreateAReferencePoint()
        {
            var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            Assert.IsTrue(true);
            Line line;
            using (var t = new Transaction(doc))
            {
                if (t.Start("Test one.") == TransactionStatus.Started)
                {
                    //create a reference point
                    //var pt = doc.FamilyCreate.NewReferencePoint(new XYZ(5, 5, 5));


                    //Line line = creApp.NewLine( p, q, true ); // 2013
                    line = Line.CreateBound(new XYZ(5, 5, 5), new XYZ(5, 15, 5)); // 2020

                    if (t.Commit() != TransactionStatus.Committed)
                    {
                        t.RollBack();
                    }
                }
                else
                {
                    throw new Exception("Transaction could not be started.");
                }
            }

            //verify that the point was created
            //var collector = new FilteredElementCollector(doc);
            ////collector.OfClass(typeof (ReferencePoint));
            //collector.OfClass(typeof(CurveElement));
            //int contador = collector.ToElements().Count;
            Assert.IsTrue(true);
        }

    }
}
