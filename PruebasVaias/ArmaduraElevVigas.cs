using System;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data;
using ArmaduraLosaRevit.Model.Cubicacion.Ayuda;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PruebasVaias
{
    [TestClass]
    public class ArmaduraElevVigas
    {
        private string nombrearchivo1;
        private RepositoryAceroElecacionVigas _RepositoryAceroElecacion;

        public ArmaduraElevVigas()
        {

            nombrearchivo1 = @"J:\_revit\PROYECTO\VIGASAUTO\2020-033 Libertad\DatosIng\_ARM_Vigas_Eje7v2.json";
            _RepositoryAceroElecacion = new RepositoryAceroElecacionVigas(nombrearchivo1);
        }
        [TestMethod]
        public void obtenerBaseDatosViga()
        {

            _RepositoryAceroElecacion.ObtenerBArras_Info_Barras_Flexion_json();
        }


    }
}
