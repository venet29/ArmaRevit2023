using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionPuntoXYZ
    {
        public static bool IsLargoCero(this XYZ pt, double tole=0.0001) =>  (Util.IsSimilarValor(pt.GetLength(),0,tole) ?true:false);
        public static bool IsDistintoLargoCero(this XYZ pt, double tole = 0.0001) => (Util.IsSimilarValor(pt.GetLength(), 0, tole) ? false : true);
        public static XYZ GEtNewXYZNH(this XYZ pt) => new XYZ(pt.X, pt.Y, pt.Z);
        public static XYZ GetXY0(this XYZ pt) => (pt == null?null: new XYZ(pt.X, pt.Y, 0));
        public static XYZ InvX(this XYZ pt) => new XYZ(-pt.X, pt.Y, pt.Z);
        public static XYZ InvY(this XYZ pt) => new XYZ(pt.X, -pt.Y, pt.Z);
        public static XYZ InvZ(this XYZ pt) => new XYZ(pt.X, pt.Y, -pt.Z);
        public static XYZnh GEtXYZnh(this XYZ pt) => new XYZnh(-pt.X, pt.Y, pt.Z);
        public static XYZ DedondearZA4(this XYZ pt) => new XYZ(pt.X, pt.Y, Math.Round( pt.Z,4));
        public static double GetAngleXY0(this XYZ pt, bool EnGrado)
        {
            XYZ ptoxy = new XYZ(pt.X, pt.Y, 0);
            double angulrad = Util.AnguloEntre2PtosGrados_enPlanoXY(new XYZ(0, 0, 0), ptoxy);
            return angulrad;
        }

        public static XYZ ObtenerCopia(this XYZ pt, int tipo = 0) {

            if (tipo==1)
               return new XYZ(Math.Abs(pt.X), Math.Abs(pt.Y), Math.Abs(pt.Z));
            else if (tipo == -1)
                return new XYZ(-Math.Abs(pt.X), -Math.Abs(pt.Y), -Math.Abs(pt.Z));
            else
                return new XYZ(pt.X, pt.Y, pt.Z);
        }

        public static XYZ ProjectExtendidaXY0(this XYZ pt, XYZ direccionbool, XYZ ptoProyect)
        {
            pt = pt.GetXY0();
            direccionbool = direccionbool.GetXY0();
            Line lineaexteidia = Line.CreateBound(pt - direccionbool * 100, pt.GetXY0() + direccionbool * 100);

            return lineaexteidia.ProjectExtendidaXY0(ptoProyect.GetXY0());
        }


        public static double GetAngleEnZ_respectoPlanoXY(this XYZ pt, bool EnGrado=false)
        {
            //pt = pt.Normalize();
            //XYZ  ptaux = pt.Normalize();
            double distaciaXY = Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
            
            double angleRad = Math.Atan2(pt.Z, distaciaXY);
            double angulGrado = Util.RadianeToGrados(angleRad);
            
            return (EnGrado?angulGrado:angleRad);
        }

        public static XYZ GetVectorXZ(this XYZ pt)
        {
            double anguloz = pt.GetAngleEnZ_respectoPlanoXY();

            XYZ result= new XYZ(Math.Cos(anguloz),Math.Sin(anguloz), 0);

            return result;
        }

        public static XYZ AsignarZ(this XYZ pt, double znuevo)
        {
            try
            {
                if (pt == null)
                {
                    return XYZ.Zero;
                }
                return new XYZ(pt.X, pt.Y, znuevo);
            }
            catch (Exception)
            {

                return XYZ.Zero;
            }
         
        }

        public static XYZ SumarZ(this XYZ pt, double znuevo)
        {
            return new XYZ(pt.X, pt.Y,pt.Z+ znuevo);
        }


        public static XYZ Redondear(this XYZ pt, int decimales)
        {
            return new XYZ(Math.Round(pt.X, decimales), Math.Round(pt.Y, decimales), Math.Round(pt.Z, decimales));
        }


        public static XYZ RedondearSIHAYCERO(this XYZ pt)
        {
            return new XYZ((Math.Abs(pt.X)<ConstNH.TOLERANCIACERO?0: pt.X),
                            (Math.Abs(pt.Y) < ConstNH.TOLERANCIACERO ? 0 : pt.Y),
                            (Math.Abs(pt.Z) < ConstNH.TOLERANCIACERO ? 0 : pt.Z));
        }


        public static XYZ Redondear8(this XYZ pt)
        {
            return Redondear( pt, ConstNH.CONST_REDONDEAR_VIEW);
        }


        public static string REdondearString_foot(this XYZ pt, int decimales)
        {
            return $"{Math.Round(pt.X, decimales)}, {Math.Round(pt.Y, decimales)}, {Math.Round(pt.Z, decimales)}";
        }
        public static string REdondearString_cm(this XYZ pt, int decimales)
        {
            return $"{Math.Round(Util.FootToCm( pt.X), decimales)}, {Math.Round(Util.FootToCm(pt.Y), decimales)}, {Math.Round(Util.FootToCm(pt.Z), decimales)}";
        }
        public static System.Windows.Point GetWindowsPointXY0(this XYZ pt)
        {
            return new System.Windows.Point(pt.X, pt.Y);
        }


    }
}
