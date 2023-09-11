using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class CoordenadasLetra
    {
        
        public CoordenadasLetra(XYZnh A, XYZnh B, XYZnh C, XYZnh F, XYZnh L, XYZnh D, XYZnh E)
        {
            this.A = A.GetXYZnh_foot();
            this.B = B.GetXYZnh_foot();
            this.C = C.GetXYZnh_foot();
            this.F = F.GetXYZnh_foot();
            this.L = L.GetXYZnh_foot();
            this.D = D.GetXYZnh_foot();
            this.E = E.GetXYZnh_foot();
        }

        public XYZnh A { get; set; }
        public XYZnh B { get; set; }
        public XYZnh C { get; set; }
        public XYZnh F { get; set; }
        public XYZnh L { get; set; }
        public XYZnh D { get; set; }
        public XYZnh E { get; set; }
    }
}
