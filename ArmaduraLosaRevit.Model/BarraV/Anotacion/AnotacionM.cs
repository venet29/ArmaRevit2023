using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Anotacion
{
    public class AnotacionM
    {
        private readonly UIApplication uiapp;
        private Document doc;
        private View view;

        public AnotacionM(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            this.doc = uiapp.ActiveUIDocument.Document;
            this.view = doc.ActiveView;
        }

        public void crear(List<Rebar> rebarList)
        {
            if (rebarList.Count == 0) return;
            //var rebarList = GetElements<Element>(doc, new[] { 280427 }).ToList();

            //Assert.IsTrue(rebarList.Count > 0, "There are no rebars in the document!");

            IList<ElementId> elementIds = new List<ElementId>();

            foreach (Element rebar in rebarList)
            {
                elementIds.Add(rebar.Id);
            }

            MultiReferenceAnnotationType type = doc.GetElement(new ElementId(168968)) as MultiReferenceAnnotationType;
            //MultiReferenceAnnotationType type = doc.GetElement(new ElementId(168380)) as MultiReferenceAnnotationType;

            MultiReferenceAnnotationOptions options = new MultiReferenceAnnotationOptions(type);

            options.TagHeadPosition = new XYZ(0, 100, 0);
            options.DimensionLineOrigin = new XYZ(5, 5, 1);
            options.DimensionLineDirection = new XYZ(0, 1, 0);
            options.DimensionPlaneNormal = view.ViewDirection;
            options.SetElementsToDimension(elementIds);
            try
            {
                using (Transaction tran = new Transaction(doc))
                {
                    tran.Start("Create_Rebar_Vertical-NH");
                    var mra = MultiReferenceAnnotation.Create(doc, view.Id, options);
                    //var dimension = GetElement<Dimension>(doc, mra.DimensionId);
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }
    }
}
