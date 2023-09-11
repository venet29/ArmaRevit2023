using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Acotar.calculos
{
     public class CalcularDistanciaRebar
    {
        private XYZ p2_acotar;
        private SeleccionarRebarElemento seleccionarRebarElemento;

        public CalcularDistanciaRebar()
        {

        }

        public CalcularDistanciaRebar(XYZ p2_acotar, SeleccionarRebarElemento seleccionarRebarElemento)
        {
            this.p2_acotar = p2_acotar;
            this.seleccionarRebarElemento = seleccionarRebarElemento;
        }

        public sbyte distanciaAlargarPAth { get; internal set; }

        internal void Calculardistancia()
        {
            throw new NotImplementedException();
        }
    }
}
