using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar.BuscarBarrarCabezaMuro
{
    public class rebarVerticalEncontrada
    {
        public int Contador { get; set; }
        public Rebar _Rebar { get; set; }
        public double Proximidad { get; set; }

        public double DesplazamietoRespetoAnterior { get; set; }

        public XYZ ptoAnterior { get; set; }
        public XYZ ptoActual { get; set; }
        public double DesplazamientoRespecInicio { get; set; }
        public double DesplazamientoRespecFinal { get; set; }
        public double DesplazamietoRespetoPosterior { get; set; }
    }
}