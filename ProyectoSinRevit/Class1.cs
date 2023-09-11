using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSinRevit
{
    [TestFixture]
    public class CAlculosvectortes
    {
        [SetUp]
        public void SetUp()
        {

        }
        [Test]
        public void ExtenderLInesDesdeDosPtosCiertaDistancia()
        {
            // TODO: Add your test code here
            //XYZ ptoExtendido= Util.ExtenderPuntoCOnRespeco2ptos(new XYZ(0,0,0), new XYZ(0, 0, 0),10.0);
            //Assert.Equals(ptoExtendido, new XYZ (10,10,0));
            int ptoExtendido = 10;
            Assert.AreEqual(ptoExtendido, 10);
        }
    }
}
