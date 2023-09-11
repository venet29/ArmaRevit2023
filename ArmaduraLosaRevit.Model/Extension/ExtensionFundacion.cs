using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
  public static  class ExtensionFundacion
    {

   
        public static double ObtenerAnchoFundacionFoot(this Floor floor,Document _doc)
        {

            var ReferenciaTipoWall = floor.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsElementId();
            var tipoFamiliaMuro = (WallType)_doc.GetElement(ReferenciaTipoWall);
            double EspesorElementoHost = tipoFamiliaMuro.Width;

            return EspesorElementoHost;

        }

        public static double ObtenerLargoFund(this Floor floor, Document _doc)
        {

            var ReferenciaTipoWall = floor.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsElementId();
            var tipoFamiliaMuro = (WallType)_doc.GetElement(ReferenciaTipoWall);
            double EspesorElementoHost = tipoFamiliaMuro.Width;

            return EspesorElementoHost;

        }

        public static XYZ ObtenerOrigin(this Floor floor)
        {
            XYZ vector = ((Line)((LocationCurve)((floor).Location)).Curve).Origin;

            return vector;
        }

        public static Level ObtenerLevel(this Floor floor)
        {
            Document _doc = floor.Document;
            Level nivel = (Level)_doc.GetElement(floor.LevelId);

            return nivel;
        }
    }
}
