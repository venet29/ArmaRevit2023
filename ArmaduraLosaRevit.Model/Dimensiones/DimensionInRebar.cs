using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;

namespace TestRebar
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class TestRebar : IExternalCommand
    {
        UIApplication m_uiApp;
        Document m_doc;

        ElementId elementId = ElementId.InvalidElementId;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                initStuff(commandData);
                if (m_doc == null)
                    return Result.Failed;

                m_uiApp = commandData.Application;
                Selection sel = m_uiApp.ActiveUIDocument.Selection;
                Reference refr = sel.PickObject(ObjectType.Element);

                Rebar rebar = m_doc.GetElement(refr.ElementId) as Rebar;

                Line rebarSeg = null;
                bool bOk = getRebarSegment(rebar, out rebarSeg);
                if (!bOk)
                    return Result.Failed;

                Options options = new Options();
                // the view in which you want to place the dimension
                options.View = m_uiApp.ActiveUIDocument.ActiveView;
                options.ComputeReferences = true; // produce references
                options.IncludeNonVisibleObjects = true;
                GeometryElement wholeRebarGeometry = rebar.get_Geometry(options);
                Reference refForEndOfBar = getReferenceForEndOfBar(wholeRebarGeometry, rebarSeg);

                Reference refGrid = getGridRef();

                ReferenceArray refArray = new ReferenceArray();
                refArray.Append(refForEndOfBar);
                refArray.Append(refGrid);

                double dist = 10;

                // a line parallel with our rebar segment somewhere in space
                Line dimLine = rebarSeg.CreateOffset(dist, XYZ.BasisY) as Line;

                using (Transaction tr = new Transaction(m_doc))
                {
                    tr.Start("Create Dimension");
                    m_doc.Create.NewDimension(
                      m_uiApp.ActiveUIDocument.ActiveView,
                      dimLine, refArray);
                    tr.Commit();
                }
            }
            catch (Exception e)
            {
                TaskDialog.Show("exception", e.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private Reference getGridRef()
        {
            ElementId idGrd = new ElementId(397028);
            Element elemGrid = m_doc.GetElement(idGrd);
            Options options = new Options();
            // the view in which you want to place the dimension
            options.View = m_uiApp.ActiveUIDocument.ActiveView;
            options.ComputeReferences = true; // produce references
            options.IncludeNonVisibleObjects = true;
            GeometryElement wholeGridGeometry
              = elemGrid.get_Geometry(options);
            IList<Reference> allRefs = new List<Reference>();
            foreach (GeometryObject geomObj in wholeGridGeometry)
            {
                Line refLine = geomObj as Line;
                if (refLine != null && refLine.Reference != null)
                    return refLine.Reference;
            }

            return null;
        }

        private Reference getReferenceForEndOfBar(
          GeometryElement geom,
          Line rebarSeg)
        {
            foreach (GeometryObject geomObj in geom)
            {
                Solid sld = geomObj as Solid;
                if (sld != null)
                {
                    // I'll get the references from curves;
                    continue;
                }
                else
                {
                    Line refLine = geomObj as Line;
                    if (refLine != null && refLine.Reference != null)
                    {
                        // We found one reference. 
                        // Let's see if it is the correct one. 
                        // The correct referece need to be perpendicular 
                        // to rebar segement and the end point of rebar 
                        // segment should be on the reference curve.
                        double dotProd = refLine.Direction.DotProduct(
                          rebarSeg.Direction);
                        if (Math.Abs(dotProd) != 0)
                            continue; // curves are not perpendicular.

                        XYZ endPointOfRebar = rebarSeg.GetEndPoint(1);
                        IntersectionResult ir = refLine.Project(
                          endPointOfRebar);
                        if (ir == null)
                            continue; // end point of rebar segment is not on the reference curve.

                        if (Math.Abs(ir.Distance) != 0)
                            continue; // end point of rebar segment is not on the reference curve.

                        return refLine.Reference;
                    }
                }
            }
            return null;
        }

        private bool getRebarSegment(
          Rebar rebar,
          out Line rebarSeg)
        {
            rebarSeg = null;
            IList<Curve> rebarSegments
              = rebar.GetCenterlineCurves(false, true, true,
              MultiplanarOption.IncludeOnlyPlanarCurves, 0);
            if (rebarSegments.Count != 1)
                return false;

            rebarSeg = rebarSegments[0] as Line;
            if (rebarSeg == null)
                return false;

            return true;
        }

        void initStuff(ExternalCommandData commandData)
        {
            m_uiApp = commandData.Application;
            m_doc = m_uiApp.ActiveUIDocument.Document;
        }
    }
}