using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class AyudaRevisionCoud
    {
        #region 0) PROPIEDADES

        public ElementId revisionCloudId { get; set; }


        public Revision revision { get; set; }


        public static UIDocument uidoc;


        public static UIView uiView { get; set; }
        #endregion


        #region 2) METODOS

        public static void CrearNuve(View view, IList<Curve> curves)
        {


            try
            {
                using (Transaction t = new Transaction(uidoc.Document))
                {
                    t.Start("Create Revision Cloud-NH");

                    RevisionCloud.Create(uidoc.Document, view, Revision.Create(uidoc.Document).Id, curves);
                    ZoomRectagule(curves[0], 3);
#if DEBUG
                    uidoc.Document.Regenerate();
#endif
                    t.Commit();
#if DEBUG
                    uidoc.RefreshActiveView();
#endif
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }



        }


        public static void BorrarNuve(Document docs)
        {


            using (Transaction t = new Transaction(docs))
            {

                try
                {
                    t.Start("text-NH");

                    //Revision.GetAllRevisionIds()

                    t.Commit();
                }
                catch (Exception ex)
                {
                    string msj = ex.Message;
                }


            }

        }
        #endregion


        public static void ObtenerUIView()
        {
            View view = uidoc.ActiveView;
            IList<UIView> uiviews = uidoc.GetOpenUIViews();

            foreach (UIView uv in uiviews)
            {
                if (uv.ViewId.Equals(view.Id))
                {
                    uiView = uv;
                    break;
                }
            }

            Rectangle rect = uiView.GetWindowRectangle();
            IList<XYZ> corners = uiView.GetZoomCorners();
            XYZ p = corners[0];
            XYZ q = corners[1];
        }


        public static void ZoomRectagule(Curve curve, double ancho)
        {
            curve.GetPoint2(0);
            XYZ p1 = curve.GetPoint2(0);
            XYZ p2 = curve.GetPoint2(1);
            XYZ p3 = (p1 + p2) / 2;

            uiView.ZoomAndCenterRectangle(new XYZ(p3.X - ancho, p3.Y - ancho, p3.Z), new XYZ(p3.X + ancho, p3.Y + ancho, p3.Z));

        }

    }
}
