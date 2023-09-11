using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraV.Ayuda
{
    public class XYZnh
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public XYZnh()
        {

        }
        public XYZnh(XYZ xyz)
        {
            X = (Util.IsEqual(xyz.X, 0) ? 0 : xyz.X);
            Y = (Util.IsEqual(xyz.Y, 0) ? 0 : xyz.Y);
            Z = xyz.Z;
        }


        public XYZnh(double X, double Y, double Z = 0)
        {
            this.X = (Util.IsEqual(X, 0) ? 0 : X);
            this.Y = (Util.IsEqual(Y, 0) ? 0 : Y);
            this.Z = Z;
        }

        public XYZ GetXYZ()
        {
            return new XYZ(X, Y, Z);
        }
        public XYZ GetXY0()
        {
            return new XYZ(X, Y, 0);
        }



 

        public XYZ GetXYZ_cmTofoot()
        {
            return new XYZ(Util.CmToFoot(X), Util.CmToFoot(Y), Util.CmToFoot(Z));
        }

        public XYZ GetXYZ_mmTofoot()
        {
            return new XYZ(Util.MmToFoot(X), Util.MmToFoot(Y), Util.MmToFoot(Z));
        }

        public XYZnh GetXYZnh_foot()
        {
            return new XYZnh(Util.CmToFoot(X), Util.CmToFoot(Y), Util.CmToFoot(Z));
        }
        public XYZnh AsignarZ(double znuevo)
        {
            return new XYZnh(X, Y, znuevo);
        }

        public override string ToString()
        {
            return $"{Math.Round(X, 4)} , {Math.Round(Y, 4)} , {Math.Round(Z, 4)}";
        }

        public static XYZnh ObtenerCOpia(XYZnh pt) => new XYZnh(pt.X, pt.Y, pt.Z);



        /// <summary>
        /// adds two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        public static XYZnh operator +(XYZnh va, XYZnh vb)
        {
            return new XYZnh(va.X + vb.X, va.Y + vb.Y, va.Z + vb.Z);
        }

        /// <summary>
        /// subtracts two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        /// <returns>subtraction of two vector</returns>
        public static XYZnh operator -(XYZnh va, XYZnh vb)
        {
            return new XYZnh(va.X - vb.X, va.Y - vb.Y, va.Z - vb.Z);
        }

        /// <summary>
        /// multiplies a vector by a floating type value
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="factor">multiplier of floating type</param>
        /// <returns> the result vector </returns>
        public static XYZnh operator *(XYZnh v, float factor)
        {
            return new XYZnh(v.X * factor, v.Y * factor, v.Z * factor);
        }

        /// <summary>
        /// divides vector by an floating type value
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="factor">floating type value</param>
        /// <returns> vector divided by a floating type value </returns>
        public static XYZnh operator /(XYZnh v, float factor)
        {
            return new XYZnh(v.X / factor, v.Y / factor, v.Z / factor);
        }



    }

    public static class extesion_XYZNH
    {
        public static XYZnh Normaze(this XYZnh v)
        {
            XYZ Aux = new XYZ(v.X, v.Y, v.Z);

            var aux2 = Aux.Normalize();
            return new XYZnh(aux2.X, aux2.Y, aux2.Z);
        }

    }


}
