using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.ModelTest.ElementoBarraRoom
{
    public class aux_listaPtoDTO
    {

        public XYZ p1 { get; set; }
        public XYZ p2 { get; set; }
        public XYZ p3 { get; set; }
        public XYZ p4 { get; set; }

        public bool IsAssertPerimetro { get; set; }
        public XYZ p1_perimetro { get; set; }
        public XYZ p2_perimetro { get; set; }
        public XYZ p3_perimetro { get; set; }
        public XYZ p4_perimetro { get; set; }

        public double largoPath { get; set; }

        public aux_listaPtoDTO()
        {
            IsAssertPerimetro = false;
        }

        public aux_listaPtoDTO(XYZ p1, XYZ p2, XYZ p3, XYZ p4, XYZ p1_perimetro, XYZ p2_perimetro, XYZ p3_perimetro, XYZ p4_perimetro, bool IsAssertPerimetro)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p1_perimetro = p1_perimetro;
            this.p2_perimetro = p2_perimetro;
            this.p3_perimetro = p3_perimetro;
            this.p4_perimetro = p4_perimetro;
            this.IsAssertPerimetro = IsAssertPerimetro;
        }
    }
}
