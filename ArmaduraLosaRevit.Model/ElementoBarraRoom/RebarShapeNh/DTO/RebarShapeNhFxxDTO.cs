namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.DTO
{
    public class REbarShapeNHFXXDTO
    {
        public double pataIzq { get; set; }
        public double pataDere { get; set; }

        public double DesDereSup { get; set; }
        public double DesDereInf { get; set; }

        public double DesIzqSup { get; set; }
        public double DesIzqInf { get; set; }

        public static REbarShapeNHFXXDTO ConfigF20A(double pataIzq, double DesDereSup, double DesDereInf)
        {
            return new REbarShapeNHFXXDTO()
            {
                pataIzq = pataIzq,
                DesDereSup = DesDereSup,
                DesDereInf = DesDereInf

            };
        }
        public static REbarShapeNHFXXDTO ConfigF20B(double pataIzq, double DesDereSup, double DesDereInf)
            =>new REbarShapeNHFXXDTO() { pataIzq = pataIzq, DesDereSup = DesDereSup, DesDereInf = DesDereInf };
        

        public static REbarShapeNHFXXDTO ConfigF16A(double DesDereSup, double DesIzqInf)
            => new REbarShapeNHFXXDTO() { DesDereSup = DesDereSup, DesIzqInf = DesIzqInf };

    }
}
