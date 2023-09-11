using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.Extension;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.CreaLine
{
    public class CrearLIneaAux
    {
        private readonly Document _doc;
#pragma warning disable CS0169 // The field 'CrearLIneaAux._UIapp' is never used
        private readonly UIApplication _UIapp;
#pragma warning restore CS0169 // The field 'CrearLIneaAux._UIapp' is never used
        private XYZ p1;
        private XYZ p2;
        private Element line_styles_Magenta;
        private ModelCurve mc;



        public CrearLIneaAux(Document _doc)
        {
            this._doc = _doc;
            line_styles_Magenta = TiposLineaPattern.ObtenerTipoLinea("MAGENTA", _doc);

        }

        public void CrearLinea( ptosLinea p1, ptosLinea p2)
        {
            this.p1 = new XYZ(p1.x, p1.y, p1.z);
            this.p2 = new XYZ(p2.x, p2.y, p2.z);
            CrearLinea(p1, p2);
        }
        public void CrearLinea(XYZ p1, XYZ p2)
        {
            if (p1.DistanceTo(p2) < 0.1) return;
            modelarlineas(p1, p2);
        }
        private ModelCurve modelarlineas( XYZ p1, XYZ p2)
        {
            Debug.WriteLine($"p1:{p1.REdondearString_foot(3)} -- p2:{p2.REdondearString_foot(3)}  ");
            try
            {
                using (Transaction tr = new Transaction(_doc, "LineaModelAyuda-NH"))
                {
                    tr.Start();
                    Curve curvamodel = Line.CreateBound(p1, p2);
                    if (curvamodel.Length < _doc.Application.ShortCurveTolerance) { return null; }
                    XYZ vectorlinea = new XYZ(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
                    Plane plano = Plane.CreateByNormalAndOrigin(p1.CrossProduct(p2), p1);
                    SketchPlane sk = SketchPlane.Create(_doc, plano);
                     mc = _doc.Create.NewModelCurve(curvamodel, sk);

                    if (line_styles_Magenta != null)
                    {
                        mc.LineStyle = line_styles_Magenta;
                        CrearLineStyle.ReadElementOverwriteLinePattern_sinTrasn(mc.LineStyle, _doc);
                    }

                    tr.Commit();
                }
            }
            catch (Exception)
            {

                return null;
            }

            return mc;
        }
    }
}
