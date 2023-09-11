using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

using GeoElement = Autodesk.Revit.DB.GeometryElement;
using Element = Autodesk.Revit.DB.Element;

namespace ArmaduraLosaRevit.Model.Armadura
{

    // nose utiliza
    public class TipoSegmentsDimensions
    {
        
        public bool setSegmentsDimensions(Rebar rebar, Dictionary<int, int> segmentsDimensions/* millimeters*/)
        {
            Document document = rebar.Document;

            //retrieve rebar shape
            Autodesk.Revit.DB.Structure.RebarShape shape = document.GetElement(rebar.GetShapeId()) as Autodesk.Revit.DB.Structure.RebarShape;
            if (shape == null)
                return false;

            //retrieve shape definition
            RebarShapeDefinition shapeDefinition = shape.GetRebarShapeDefinition();
            if (!(shapeDefinition is RebarShapeDefinitionBySegments))
                return false;

            RebarShapeDefinitionBySegments shapeDefinitionBySegments = shapeDefinition as RebarShapeDefinitionBySegments;

            //retrieve each segment
            foreach (var segmentDimension in segmentsDimensions)
            {
                if (segmentDimension.Key < 0 || segmentDimension.Key >= shapeDefinitionBySegments.NumberOfSegments)
                    return false;

                RebarShapeSegment shapeSegment = shapeDefinitionBySegments.GetSegment(segmentDimension.Key);

                //look for length constraint
                IList<RebarShapeConstraint> shapeConstraints = shapeSegment.GetConstraints();
                foreach (RebarShapeConstraint shapeConstraint in shapeConstraints)
                {
                    RebarShapeConstraintSegmentLength shapeConstraintLength = shapeConstraint as RebarShapeConstraintSegmentLength;
                    if (shapeConstraintLength != null)
                    {
                        SharedParameterElement sharedParameter = document.GetElement(shapeConstraintLength.GetParamId()) as SharedParameterElement;
                        if (sharedParameter != null)
                        {
                            Parameter parameter = rebar.get_Parameter(sharedParameter.GuidValue);
                            if (parameter != null)
                            {
                                //parameter.Set(Unit.convertToApi(segmentDimension.Value, DisplayUnitType.DUT_MILLIMETERS));
                            }
                        }

                        break;
                    }
                }
            }

            return true;
        }



        public bool setSegmentsDimensions(RebarShape shape, Dictionary<int, int> segmentsDimensions/* millimeters*/)
        {
            Document document = shape.Document;

            //retrieve rebar shape
          //  Autodesk.Revit.DB.Structure.RebarShape shape = document.GetElement(rebar.GetShapeId()) as Autodesk.Revit.DB.Structure.RebarShape;
            if (shape == null)
                return false;

            //retrieve shape definition
            RebarShapeDefinition shapeDefinition = shape.GetRebarShapeDefinition();
            if (!(shapeDefinition is RebarShapeDefinitionBySegments))
                return false;

            RebarShapeDefinitionBySegments shapeDefinitionBySegments = shapeDefinition as RebarShapeDefinitionBySegments;

            //retrieve each segment
            foreach (var segmentDimension in segmentsDimensions)
            {
                if (segmentDimension.Key < 0 || segmentDimension.Key >= shapeDefinitionBySegments.NumberOfSegments)
                    return false;

                RebarShapeSegment shapeSegment = shapeDefinitionBySegments.GetSegment(segmentDimension.Key);

                //look for length constraint
                IList<RebarShapeConstraint> shapeConstraints = shapeSegment.GetConstraints();
                foreach (RebarShapeConstraint shapeConstraint in shapeConstraints)
                {
                    RebarShapeConstraintSegmentLength shapeConstraintLength = shapeConstraint as RebarShapeConstraintSegmentLength;
                    if (shapeConstraintLength != null)
                    {
                        SharedParameterElement sharedParameter = document.GetElement(shapeConstraintLength.GetParamId()) as SharedParameterElement;
                        if (sharedParameter != null)
                        {
                            //Parameter parameter = rebar.get_Parameter(sharedParameter.GuidValue);
                            //if (parameter != null)
                            //{
                            //    //parameter.Set(Unit.convertToApi(segmentDimension.Value, DisplayUnitType.DUT_MILLIMETERS));
                            //}
                        }

                        break;
                    }
                }
            }

            return true;
        }

    }
}
