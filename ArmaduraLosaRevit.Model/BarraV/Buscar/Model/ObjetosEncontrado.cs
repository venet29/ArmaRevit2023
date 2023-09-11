using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar.Model
{
    public class ObjetosEncontrado
    {
        public double Proximity { get; internal set; }
        public XYZ PuntoSobreFAceHost { get; internal set; }
        public Element ElementEncontrado { get; internal set; }
    }
}
