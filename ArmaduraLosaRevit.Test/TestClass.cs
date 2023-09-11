// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RTF.Applications;
using RTF.Framework;

namespace ArmaduraLosaRevit.Test
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        [TestModel(@"J:\_revit\PROYECTOS REVIT\REVITArmaduraLosaRevit\ArmaduraLosaRevit\ArmaduraLosaRevit.Test\bin\Debug\empty.rvt")]
        public void CanCreateAReferencePoint()
        {
            var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            Assert.IsTrue(true);
            
        }

    }
}
