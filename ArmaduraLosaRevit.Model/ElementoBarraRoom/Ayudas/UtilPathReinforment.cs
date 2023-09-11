using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
    public class UtilPathReinforment
    {
        public static XYZ ObtenerOrigenPathReinf(PathReinforcement pathReinforcement,Document _doc)
        {
            if (pathReinforcement == null) return null;
            ElementId elId = pathReinforcement.GetCurveElementIds()[0];

            ModelLine MOdeLine = _doc.GetElement2(elId) as ModelLine;
            if (MOdeLine == null) return XYZ.Zero;
            Line LineCUrve = MOdeLine.GeometryCurve as Line;
            XYZ ptOrigen = LineCUrve.Origin;
            return ptOrigen;
        }

        public static XYZ ObtenerDireccionPathReinf(PathReinforcement pathReinforcement, Document _doc)
        {
            if (pathReinforcement == null) return null;
            ElementId elId = pathReinforcement.GetCurveElementIds()[0];

            ModelLine MOdeLine = _doc.GetElement2(elId) as ModelLine;
            if (MOdeLine == null) return XYZ.Zero;
            Line LineCUrve = MOdeLine.GeometryCurve as Line;
            XYZ ptODireccion = LineCUrve.Direction;
            return ptODireccion;
        }
        public static XYZ ObtenerPtoFinalModelCurveDePathreinf(PathReinforcement pathReinforcement, Document _doc)
        {
            if (pathReinforcement == null) return null;
            ElementId elId = pathReinforcement.GetCurveElementIds()[0];

            ModelLine MOdeLine = _doc.GetElement2(elId) as ModelLine;
            if (MOdeLine == null) return XYZ.Zero;
            Line LineCUrve = MOdeLine.GeometryCurve as Line;
            return LineCUrve.GetEndPoint(1);


        }

        public static Line ObtenerModelCurveDePathreinf(PathReinforcement pathReinforcement, Document _doc)
        {
            if (pathReinforcement == null) return null;

            ElementId elId = pathReinforcement.GetCurveElementIds()[0];

            ModelLine MOdeLine = _doc.GetElement2(elId) as ModelLine;
            if (MOdeLine == null) return null;
            Line LineCUrve = MOdeLine.GeometryCurve as Line;
            return LineCUrve;


        }
    }

}
