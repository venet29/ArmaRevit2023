using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionMur0
    {

        public static double ObtenerEspesorMuroFoot(this Wall wall, Document _doc)
        {
            var ReferenciaTipoWall = wall.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsElementId();
            var tipoFamiliaMuro = wall.WallType;// (WallType)_doc.GetElement(ReferenciaTipoWall);

            double EspesorElementoHost = tipoFamiliaMuro.Width;

            return EspesorElementoHost;
        }
        public static XYZ ObtenerDireccion(this Wall wall)
        {
            XYZ vector = ((Line)((LocationCurve)((wall).Location)).Curve).Direction;

            return vector;
        }


        public static XYZ ObtenerDireccionEnElSentidoView(this Wall ElementEncontrado, XYZ RightDirection)
        {
            XYZ _aux_direccionMuro = (ElementEncontrado as Wall).ObtenerDireccion();
            double factorDireccion = Util.GetProductoEscalar(RightDirection.GetXY0(), _aux_direccionMuro.GetXY0());

            if (Util.IsSimilarValor(factorDireccion, 0, 0.001)) //muro entrando en vista
                factorDireccion = 1;
            else
                factorDireccion = factorDireccion / Math.Abs(factorDireccion);

            _aux_direccionMuro = new XYZ(_aux_direccionMuro.X * factorDireccion, _aux_direccionMuro.Y * factorDireccion, _aux_direccionMuro.Z);
            return _aux_direccionMuro;
        }


        public static XYZ ObtenerOrigin(this Wall wall)
        {
            XYZ vector = ((Line)((LocationCurve)((wall).Location)).Curve).Origin;

            return vector;
        }



        public static Level ObtenerLevel(this Wall wall)
        {
            Document _doc = wall.Document;
            Level nivel = (Level)_doc.GetElement(wall.LevelId);

            return nivel;
        }

        public static double ObtenerLargo(this Wall wall)
        {
            double largo = 0;
            var locaCUrv = ((LocationCurve)((wall).Location)).Curve;

            if (locaCUrv is Line)
                largo = ((Line)locaCUrv).Length;
            else if (locaCUrv is Arc)
                largo = ((Arc)locaCUrv).ApproximateLength;
            else
            {
                Util.ErrorMsg($"No se pudo obtener tipo de elemento id:{wall.Id}para calcular largo . Se asigna largo Cero");
                return 0;
            }

            return largo;
        }

        public static double ObtenerAlturaMuroFoot(this Wall wall)
        {
            PlanarFace planarFaceinf = wall.ObtenerCaraInferior(false);
            PlanarFace planarFaceSup = wall.ObtenerCaraSuperior(false);
            if (planarFaceinf == null || planarFaceSup == null)
                return 0;
            else
                return Math.Abs(planarFaceSup.Origin.Z - planarFaceinf.Origin.Z);

        }

 
    }
}
