using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System.Linq;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.Armadura
{
    internal class InfoProject
    {
        public static XYZ OBtenerbase_point(Document _doc)
        {
            FilteredElementCollector projectBasePointCollector = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_ProjectBasePoint);
            BasePoint projectBasePoint = projectBasePointCollector.First<Element>(e => e is BasePoint) as BasePoint;
            XYZ projectBasePointPosition = projectBasePoint.Position;


            var surveyPointPositionv2 =  BasePoint.GetSurveyPoint(_doc);
         var   projectBasePointPositiov2n= BasePoint.GetProjectBasePoint(_doc);

            return projectBasePointPosition;
        }

        public static XYZ OBtenerbase_SurveyPointPosition(Document _doc)
        {
            FilteredElementCollector surveyPointCollector = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_SharedBasePoint);
            BasePoint surveyPoint = surveyPointCollector.First<Element>(e => e is BasePoint) as BasePoint;
            XYZ surveyPointPosition = surveyPoint.Position;

            return surveyPointPosition;
        }
    }
}
