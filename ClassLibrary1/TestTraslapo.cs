using NUnit.Framework;
using ArmaduraLosaRevit.Model.Cubicacion.Ayuda;

namespace RTF.Tests
{
    [TestFixture]
    public class Class1
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


        [Test]
        public void CanCreateAReferencePoint()
        {

            int x = 2;
            int y = 1;
            Assert.IsTrue(y==x);

        }

        [Test]
        public void CanCreateAReferencePoint2()
        {
            var Result = AyudaObtenerNombreEjeElev.Obtener("ELEVACION EJE 5=22");


            Assert.IsTrue(Result!="");

        }
        //[Test]
        //[TestModel(@"./bricks.rfa")]
        //public void ModelHasTheCorrectNumberOfBricks()
        //{
        //    var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;

        //    var fec = new FilteredElementCollector(doc);
        //    fec.OfClass(typeof(FamilyInstance));

        //    var bricks = fec.ToElements()
        //        .Cast<FamilyInstance>()
        //        .Where(fi => fi.Symbol.Family.Name == "brick");

        //    Assert.AreEqual(bricks.Count(), 4);
        //}

    }
}
