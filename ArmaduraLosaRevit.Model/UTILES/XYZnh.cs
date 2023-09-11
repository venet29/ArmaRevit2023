
using ImportRevit.App.Ayuda;

namespace ImportRevit.Model
{
    public class XYZnh
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public XYZnh()
        {

        }
    
        public XYZnh(double X, double Y,double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

    
        public XYZnh GetXY0()
        {
            return new XYZnh(X, Y, 0);
        }

        public XYZnh Get_cm()
        {
            return new XYZnh( Util.FootToCm( X), Util.FootToCm(Y), Util.FootToCm(Z));
        }
    }


}
