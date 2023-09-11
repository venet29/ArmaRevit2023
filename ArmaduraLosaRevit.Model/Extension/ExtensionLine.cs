using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionLine
    {


        public static XYZ ProjectExtendidaXY0(this Line _line, XYZ ptoProyect)
        {
            Line lineaAux = _line.ExtenderLineaXY0(500);

            IntersectionResult ptoProy = lineaAux.Project(ptoProyect.GetXY0());

            return ptoProy.XYZPoint.AsignarZ(ptoProyect.Z);
        }

        public static XYZ ProjectExtendida3D(this Line _line, XYZ ptoProyect)
        {
            Line lineaAux = _line.ExtenderLinea3D(500);

            IntersectionResult ptoProy = lineaAux.Project(ptoProyect);

            return ptoProy.XYZPoint;
        }
        public static XYZ ProjectSINExtendida3D(this Line _line, XYZ ptoProyect)
        {
            //Nota el no extender solo implica q va a seleccionar el pt mas extremo
            /*
                                 Line line = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(0, 5, 0));
                    var result = line.Project(new XYZ(1, 10, 0));

          ptoProy.XYZPoint   es  =>  new (0,5,0)
                NO DEVUELVE ERROR
             */

            IntersectionResult ptoProy = _line.Project(ptoProyect);

            return ptoProy.XYZPoint;
        }

        public static (bool, XYZ) IsProject_enplanoXY0(this Line _line, XYZ ptoProyect)
        {

            Line _lineGetXY0 = GetXY0(_line);
            XYZ ptoProyectXY0 = ptoProyect.GetXY0();

            IntersectionResult ptoProy = _lineGetXY0.Project(ptoProyectXY0);

            return (!ptoProy.XYZPoint.IsAlmostEqualTo(XYZ.Zero), ptoProy.XYZPoint);
        }
        

        public static double ObtenerDistancia_enPlanoXY0(this Line _line, XYZ ptoProyect)
        {
            XYZ  ptoInterssion_planoXY0= ProjectExtendidaXY0(_line, ptoProyect);

            return ptoInterssion_planoXY0.GetXY0().DistanceTo(ptoProyect.GetXY0());
        }
        public static Line ExtenderLineaXY0(this Line _line, double extensionCadaLadoFoot)
        {
            XYZ pt1 = _line.GetPoint2(0).GetXY0();
            XYZ pt2 = _line.GetPoint2(1).GetXY0();
            var r0 = UtilBarras.extenderLineaDistancia(pt1, pt2, extensionCadaLadoFoot);
            // Util.ExtenderPuntoRespectoOtroPtosConAngulo
            return Line.CreateBound(r0[0], r0[1]);
        }
        public static Line ExtenderLinea3D(this Line _line, double extensionCadaLadoFoot)
        {
            XYZ pt1 = _line.GetPoint2(0);
            XYZ pt2 = _line.GetPoint2(1);

            XYZ direccion = (pt2 - pt1).Normalize();

            // Util.ExtenderPuntoRespectoOtroPtosConAngulo
            return Line.CreateBound(pt1 - direccion * extensionCadaLadoFoot / 2, pt2 + direccion * extensionCadaLadoFoot / 2);
        }
        public static Line GetXY0(this Line _line)
        {
            XYZ pt1 = _line.GetPoint2(0).GetXY0();
            XYZ pt2 = _line.GetPoint2(1).GetXY0();
            return Line.CreateBound(pt1, pt2);
        }
        public static Line AsignarXY0(this Line _line)
        {
            return Line.CreateBound(_line.GetPoint2(0).GetXY0(), _line.GetPoint2(1).GetXY0());
        }
        public static bool IsIgualDireccion(this Curve _lineOriginal, Line _line2)
        {
            if (!(_lineOriginal is Line)) return false;

            Line _line = (_lineOriginal as Line);
            XYZ pt1 = _line.GetPoint2(0);
            XYZ pt2 = _line.GetPoint2(1);
            bool result = _line.Direction.IsAlmostEqualTo(_line2.Direction);
            return result;
        }


        public static bool IsEqual(this Curve _lineOriginal, Line _line2)
        {
            if (!(_lineOriginal is Line)) return false;
            if (!(_line2 is Line)) return false;

            Line _lineOrigianl = (_lineOriginal as Line);
            XYZ pt1 = _lineOrigianl.GetPoint2(0);
            XYZ pt2 = _lineOrigianl.GetPoint2(1);
            bool result = _lineOrigianl.Direction.IsAlmostEqualTo(_line2.Direction);

            bool resultLargo = Util.IsEqual(_lineOrigianl.Length, _line2.Length);

            bool resultaPtoInicial = pt1.IsAlmostEqualTo(_line2.GetEndPoint(0));
            return result && resultLargo && resultaPtoInicial;
        }

        public static Line ExtenderFin(this Line _line, double extensionFinLadoFoot)
        {

            XYZ pt1 = _line.GetEndPoint(0);
            XYZ pt2 = _line.GetEndPoint(1);
            XYZ _Direccion = (pt2 - pt1).Normalize();

            //larogo de los extremos = largo_curve+redioARc + diam/2
            Line _line0New = Line.CreateBound(pt1, pt2 + _Direccion * (extensionFinLadoFoot));
            // Util.ExtenderPuntoRespectoOtroPtosConAngulo
            return _line0New;
        }
        public static Line ExtenderInicio(this Line _line, double extensionInicioLadoFoot)
        {

            XYZ pt1 = _line.GetEndPoint(0);
            XYZ pt2 = _line.GetEndPoint(1);
            XYZ _Direccion = (pt1 - pt2).Normalize();

            //larogo de los extremos = largo_curve+redioARc + diam/2
            Line _line0New = Line.CreateBound(pt1 + _Direccion * (extensionInicioLadoFoot), pt2);
            // Util.ExtenderPuntoRespectoOtroPtosConAngulo
            return _line0New;
        }

        public static Line ExtenderInicioFin(this Line _line, double extensionInicioLadoFoot, double extensionFinLadoFoot)
        {

            XYZ pt1 = _line.GetEndPoint(0);
            XYZ pt2 = _line.GetEndPoint(1);
            XYZ _Direccion = (pt2 - pt1).Normalize();

            //larogo de los extremos = largo_curve+redioARc + diam/2
            Line _line0New = Line.CreateBound(pt1 - _Direccion * (extensionInicioLadoFoot), pt2 + _Direccion * (extensionFinLadoFoot));
            // Util.ExtenderPuntoRespectoOtroPtosConAngulo
            return _line0New;
        }
        public static XYZ ObtenerDireccion(this Line _line)
        {
            XYZ pt1 = _line.GetEndPoint(0);
            XYZ pt2 = _line.GetEndPoint(1);
            return (pt2 - pt1).Normalize();
        }


    }
}
