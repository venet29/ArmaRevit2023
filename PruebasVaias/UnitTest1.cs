using System;
using ArmaduraLosaRevit.Model.Cubicacion.Ayuda;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PruebasVaias
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ObtenerufijodeTitulo()
        {

            var resutl =AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJE    5=22");
            Assert.IsTrue(resutl == "5=22");

            var resutla = AyudaObtenerNombreEjeElev.ObtenerSufijo("5=22");
            Assert.IsTrue(resutla == "5=22");



            var resut2 = AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJE5=22");
            Assert.IsTrue(resut2 == "5=22");

            var resut2b = AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJE5");
            Assert.IsTrue(resut2b == "5");

            var resut3 = AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJES5=22");
            Assert.IsTrue(resut3 == "5=22");

            var resut4 = AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJES   5=22");
            Assert.IsTrue(resut4 == "5=22");


             resutl = AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJE    5 = 22");
            Assert.IsTrue(resutl == "5=22");

             resut2 = AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJE5 = 22");
            Assert.IsTrue(resut2 == "5=22");

             resut3 = AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJES5 =  22");
            Assert.IsTrue(resut3 == "5=22");

             resut4 = AyudaObtenerNombreEjeElev.ObtenerSufijo("ELEVACION EJES   5  = 22");
            Assert.IsTrue(resut4 == "5=22");

        }


        [TestMethod]
        public void ObtenerPrefinodeTitulo()
        {

            var resutl = AyudaObtenerNombreEjeElev.ObtenerPrefijo("ELEVACION EJE    5=22");
            Assert.IsTrue(resutl == "ELEVACION EJE");

            var resut2b = AyudaObtenerNombreEjeElev.ObtenerPrefijo("5=22");
            Assert.IsTrue(resut2b == "");

            var resut2c = AyudaObtenerNombreEjeElev.ObtenerPrefijo("elev 5=22");
            Assert.IsTrue(resut2c == "");

            var resut2 = AyudaObtenerNombreEjeElev.ObtenerPrefijo("ELEVACION EJE5=22");
            Assert.IsTrue(resut2 == "ELEVACION EJE");

            var resut3 = AyudaObtenerNombreEjeElev.ObtenerPrefijo("ELEVACION EJES5=22");
            Assert.IsTrue(resut3 == "ELEVACION EJES");

            var resut4 = AyudaObtenerNombreEjeElev.ObtenerPrefijo("ELEVACION EJES   5=22");
            Assert.IsTrue(resut4 == "ELEVACION EJES");


            resutl = AyudaObtenerNombreEjeElev.ObtenerPrefijo("ELEVACION EJE    5 = 22");
            Assert.IsTrue(resutl == "ELEVACION EJE");

            resut2 = AyudaObtenerNombreEjeElev.ObtenerPrefijo("ELEVACION EJE5 = 22");
            Assert.IsTrue(resut2 == "ELEVACION EJE");

            resut3 = AyudaObtenerNombreEjeElev.ObtenerPrefijo("ELEVACION EJES5 =  22");
            Assert.IsTrue(resut3 == "ELEVACION EJES");

            resut4 = AyudaObtenerNombreEjeElev.ObtenerPrefijo("ELEVACION EJES   5  = 22");
            Assert.IsTrue(resut4 == "ELEVACION EJES");

        }
    }
}
