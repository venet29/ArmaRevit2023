#region Namespaces
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.ObjectModel;
#endregion // Namespaces



namespace ApiRevit.FILTROS
{
    [Transaction(TransactionMode.Manual)]
    public class SelectionExamples : IExternalCommand
    {
        #region Implementation of IExternalCommand

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Selection sel = uiDoc.Selection;

            try
            {
                //
                // Select Wall or Floor
                //
                Reference pickedReference = sel.PickObject(ObjectType.Element, SelFilter.GetElementFilter(typeof(Wall), typeof(Floor)), "Select Wall or Floor");
                if (pickedReference == null) return Result.Failed;
                Element firstElement = doc.GetElement(pickedReference);

                TaskDialog.Show("Result", "First Selection: " + firstElement.Category.Name + ": " + firstElement.Name + " (" + firstElement.Id + ")");

                //
                // Select anything intersecting:
                //
                ElementFilter filter = new ElementIntersectsElementFilter(firstElement);
                ElementFilter notTheFirst = new ExclusionFilter(new Collection<ElementId> { firstElement.Id });
                ISelectionFilter intersectionFilter = SelFilter.GetElementFilter(filter).And(SelFilter.GetElementFilter(notTheFirst));
                pickedReference = sel.PickObject(ObjectType.Element, intersectionFilter, "Select anything intersecting the first picked Element");
                if (pickedReference == null) return Result.Failed;
                Element secondElement = doc.GetElement(pickedReference);
                TaskDialog.Show("Result", "Second Selection: " + secondElement.Category.Name + ": " + secondElement.Name + " (" + secondElement.Id + ")");

                //
                // Select colums or beams or foundations within 20 feet of the second element in any direction, 
                // but not if they do intersect with the first element
                //
                ICollection<BuiltInCategory> categories = new[] {
                                                                  BuiltInCategory.OST_StructuralColumns,          BuiltInCategory.OST_Columns,
                                                                  BuiltInCategory.OST_StructuralFraming,          BuiltInCategory.OST_StructuralFoundation,
                                                                };
                ElementFilter catFilter = new ElementMulticategoryFilter(categories);
                BoundingBoxXYZ box = secondElement.get_BoundingBox(null);
                XYZ vector = new XYZ(20, 20, 20);
                Outline outline = new Outline(box.Min - vector, box.Max + vector);
                ElementFilter boxFilter = new BoundingBoxIntersectsFilter(outline);
                ISelectionFilter selectionFilter = intersectionFilter.Not().And(SelFilter.GetElementFilter(catFilter), SelFilter.GetElementFilter(boxFilter));
                pickedReference = sel.PickObject(ObjectType.Element, selectionFilter, "Select Column, Beam, Foundation within 20 feet of second element, not intersecting first element");
                if (pickedReference == null) return Result.Failed;
                Element thirdElement = doc.GetElement(pickedReference);
                TaskDialog.Show("Result", "Third Selection: " + thirdElement.Category.Name + ": " + thirdElement.Name + " (" + thirdElement.Id + ")");

                //
                // Select Face of thirdElement with normal parallel to BasisZ
                //
                selectionFilter = SelFilter.GetReferenceFilter((reference, xyz) =>
                {
                    if (reference.ElementId != thirdElement.Id) return false;
                    Face face = thirdElement.GetGeometryObjectFromReference(reference) as Face;
                    if (face == null) return false;
                    XYZ normal = face.ComputeNormal(UV.Zero);
                    XYZ crossProduct = normal.CrossProduct(XYZ.BasisZ);
                    return (crossProduct.IsAlmostEqualTo(XYZ.Zero));
                });
                pickedReference = sel.PickObject(ObjectType.Face, selectionFilter, "Select Face of third element with normal parallel to z-Axis");
                Face pickedFace = thirdElement.GetGeometryObjectFromReference(pickedReference) as Face;
                XYZ pickedNormal = pickedFace.ComputeNormal(UV.Zero);
                TaskDialog.Show("Result", "Fourth Selection: " + pickedFace.GetType().Name + " (" + pickedReference.ConvertToStableRepresentation(doc) + ")\n" + "Normal: (" + pickedNormal.X.ToString("0.###") + " / " + pickedNormal.Y.ToString("0.###") + " / " + pickedNormal.Z.ToString("0.###") + ")");

                //
                // Now select any Face with an X oder Y-Normal from any previously selected Element
                //
                IElementSelectionFilter idFilter = SelFilter.GetElementFilter(firstElement.Id, secondElement.Id, thirdElement.Id);
                IReferenceSelectionFilter xFilter = SelFilter.GetFaceNormalFilter(doc, XYZ.BasisX, true);
                IReferenceSelectionFilter yFilter = SelFilter.GetFaceNormalFilter(doc, XYZ.BasisY, true);
                ILogicalCombinationFilter logicalFilter = idFilter.And(xFilter.Or(yFilter));
#if DEBUG
                logicalFilter.ExecuteAll = true;
#endif
                pickedReference = sel.PickObject(ObjectType.Face, logicalFilter, "Now select any Face with an x oder y-Normal from any previously selected Element");
                Element element = doc.GetElement(pickedReference);
                pickedFace = element.GetGeometryObjectFromReference(pickedReference) as Face;
                pickedNormal = pickedFace.ComputeNormal(UV.Zero);
                TaskDialog.Show("Result", "Fifth Selection: " + pickedFace.GetType().Name + " (" + pickedReference.ConvertToStableRepresentation(doc) + ")\n" + "Normal: (" + pickedNormal.X.ToString("0.###") + " / " + pickedNormal.Y.ToString("0.###") + " / " +
                  pickedNormal.Z.ToString("0.###") + ")");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("Cancelled", "User cancelled");
                return Result.Cancelled;
            }
            return Result.Succeeded;
        }
        #endregion
    }
}

