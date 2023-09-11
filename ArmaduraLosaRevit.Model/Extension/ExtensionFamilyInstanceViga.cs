using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionFamilyInstanceViga
    {
        public static double ObtenerEspesorVigaFoot(this FamilyInstance viga)
        {
            if (viga.StructuralType != StructuralType.Beam)
            {
                Util.ErrorMsg($"Elemento FamilyInstance con id:{viga.Id} y id:{viga.StructuralType} no sepuede considerar como viga");
                return 0;
            }
            Parameter espesor = ParameterUtil.FindParaByName(viga.Symbol.Parameters, "b");

            if (espesor == null) espesor = ParameterUtil.FindParaByName(viga.Symbol.Parameters, "Ancho");

            double EspesorViga = 0;
            if (espesor == null)
            {
                Util.ErrorMsg($"Espesor de Viga id:{viga.Id} no encontrada");
                EspesorViga = 0;
            }
            else
            { EspesorViga = espesor.AsDouble(); }


            return EspesorViga;
        }






        public static XYZ ObtenerDireccionEnelSentidoView(this FamilyInstance familyInstance)
        {
            XYZ vector = ((Line)((LocationCurve)((familyInstance).Location)).Curve).Direction;

            return vector;
        }

        public static double ObtenerLevel_valorZ(this FamilyInstance familyInstance)
        {
            Parameter valorparametrer = ParameterUtil.FindParaByName(familyInstance, "Reference Level Elevation");
            double ValorLevel = 0;
            if (valorparametrer != null)
            {
                ValorLevel = valorparametrer.AsDouble();
            }

            return ValorLevel;
        }

        public static Level ObtenerLevel(this FamilyInstance familyInstance)
        {
            Document _doc = familyInstance.Document;
            Level _level = null;
            Parameter valorparametrer = ParameterUtil.FindParaByName(familyInstance, "Reference Level");

            if (valorparametrer != null)
            {
                if (valorparametrer.AsElementId().ToString() != "-1")
                    _level = _doc.GetElement(valorparametrer.AsElementId()) as Level;
            }

            return _level;
        }

        public static XYZ ObtenerOrigin(this FamilyInstance familyInstance)
        {
            XYZ vector = XYZ.Zero;
            if (familyInstance.Location is LocationPoint)
            {
                LocationPoint lpoint = familyInstance.Location as LocationPoint;
                vector = lpoint.Point;
            }
            else
            {
                LocationCurve lcurve = familyInstance.Location as LocationCurve;
                vector = ((Line)(lcurve.Curve)).Origin;
            }


            return vector;
        }
        public static XYZ ObtenerDireccionEnElSentidoView(this FamilyInstance viga, XYZ RightDirection)
        {
            XYZ _aux_direccionelemento = viga.ObtenerDireccionEnelSentidoView();

            if(_aux_direccionelemento==null)
                return RightDirection;

            double factorDireccion = Util.GetProductoEscalar(RightDirection.GetXY0(), _aux_direccionelemento.GetXY0());

            if (factorDireccion == 0)
                factorDireccion = 1;
            else
                factorDireccion = factorDireccion / Math.Abs(factorDireccion);

            _aux_direccionelemento = new XYZ(_aux_direccionelemento.X * factorDireccion, _aux_direccionelemento.Y * factorDireccion, _aux_direccionelemento.Z);
            return _aux_direccionelemento;
        }
    }
}
