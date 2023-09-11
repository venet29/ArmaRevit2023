using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using WinForms = System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda
{
    public class UtilDesglose
    {
        const string _caption = "Mensajeria";

        public const double _eps = 1.0e-9;
        const double _FootToCm = 30.48;
        const double _inchToMm = 25.4;
        const double _footToMm = 12 * _inchToMm;
        const double _footToMeter = _footToMm * 0.001;
        const double _gradosToRadianes = Math.PI / 180;
        const double _sqfToSqm = _footToMeter * _footToMeter;
        const double _cubicFootToCubicMeter = _footToMeter * _sqfToSqm;

        /// <summary>
        ///35) Convert a given length in feet to millimetres.
        /// </summary>
        public static double FootToCm(double length)
        {
            return length * _footToMm;
        }
        public static double CmToFoot(double length)
        {
            return length / _FootToCm;
        }

        public static bool IsmasVertical(XYZ direccion)
        {

            return (Math.Abs(direccion.Z) > Math.Abs(direccion.X) && Math.Abs(direccion.Z) > Math.Abs(direccion.Y) ? true : false);
        }

        public static bool IsmasHorizontal(XYZ direccion)
        {

            return (!IsmasVertical(direccion));
        }

        public static bool IsmasVertical(Line _line)
        {
            XYZ direccion = (_line.GetEndPoint(1) - _line.GetEndPoint(0)).Normalize();
            return IsmasVertical(direccion);
        }

        public static bool IsCollinear_barraDesglose(Line a, Line b, double diamFoot)
        {
          //  XYZ v = a.Direction;
            //XYZ w = b.Origin - a.Origin;
            XYZ PtoInter = b.ProjectExtendida3D(a.Origin);
            return IsParallel(a.Direction, b.Direction) && PtoInter.DistanceTo(a.Origin) < diamFoot;
        }
        public static void ErrorMsg(string msg)
        {
            Debug.WriteLine(msg);
            WinForms.MessageBox.Show(msg,
              _caption,
              WinForms.MessageBoxButtons.OK,
              WinForms.MessageBoxIcon.Error);
        }
        public static bool IsParallel(XYZ p, XYZ q, double tolera = 0.005)
        {
            //   return p.CrossProduct(q).IsZeroLength();    => menor IsZeroLength()<1.10-9
            var largo = p.CrossProduct(q).GetLength();
            return IsEqual(largo, 0, tolera);
        }
        public static bool IsParallelIgualSentido(XYZ p, XYZ q, double tolera = 0.005)
        {
            //   return p.CrossProduct(q).IsZeroLength();    => menor IsZeroLength()<1.10-9
            var resul = GetProductoEscalar(p, q);
            return IsSimilarValor(resul,1,0.01);
        }

        public static bool IsParallelOpuestos(XYZ p, XYZ q, double tolera = 0.005)
        {
            //   return p.CrossProduct(q).IsZeroLength();    => menor IsZeroLength()<1.10-9
            var resul = GetProductoEscalar(p, q);
            return IsSimilarValor(resul, -1, 0.01);
        }

        public static bool IsZero(double a, double tolerance = _eps)
        {
            return tolerance > Math.Abs(a);
        }
        public static bool IsEqual(double a, double b, double tolerance = _eps)
        {
            return IsZero(b - a, tolerance);
        }

        public static bool IsSimilarValor(double valor, double valor2, double _toleraci = _eps)
        {

            if (Math.Abs(valor - valor2) < _toleraci)
            { return true; }
            else
            { return false; }

        }


        //RUTINA TESTEADA:develve el angulo entre dos pto 
        //resultado (Vectardelta.Y >=0) entre 0 ,45 ,90,180
        //resultado (Vectardelta.Y <0) -45,-90,-135 
        public static double AnguloEntre2PtosGrados_enPlanoXY(XYZ p1, XYZ p2)
        {
            XYZ Vectardelta = (new XYZ(p2.X, p2.Y, 0) - new XYZ(p1.X, p1.Y, 0));
            double result = RadianeToGrados(Vectardelta.AngleTo(new XYZ(1, 0, 0)));
            return (Vectardelta.Y >= 0 ? result : -result);
        }

        public static double RadianeToGrados(double radianes)
        {
            return radianes / _gradosToRadianes;
        }

        //p1 p2 (ang)
        // vectore igual y paralelos =1
        // vectore opuestos y paralelos =-1
        //  p1 y p2 perpendiculaes  = 0
        //  a*b = |a||b|cos(ang)
        //https://www.fisicalab.com/apartado/producto-escalar
        public static double GetProductoEscalar(XYZ p1, XYZ p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;

        }

    }
}
