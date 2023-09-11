using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Automatico;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.ViewportnNH.model
{
    public class ViewGeom
    {
        private UIApplication uiapp;
        private Document _doc;

        public ViewDTO viewDTO_ { get; set; }
        public ViewGeom(UIApplication uiapp, ViewDTO view_)
        {
            this.uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.viewDTO_ = view_;
        }

        public BoundingBoxXYZ box { get; private set; }
        public XYZ PtoMin { get; private set; }
        public XYZ PtoMax { get; private set; }
        public double Ancho { get; private set; }
        public double Alto { get; private set; }
        public XYZ ptoMinReal { get; private set; }
        public XYZ ptoMaxReal { get; private set; }
        public XYZ PtoMax_Trasns { get; private set; }
        public XYZ PtoMin_Trasns { get; private set; }
        public double X_sheet { get; internal set; }
        public double Y_sheet { get; internal set; }

        public bool CAlcularGeometria(double delta_AnnotationCrop = 0)
        {
            try
            {
                /*
                  pMin --
                     |    |  alto
                       -- PMAX
                      ancho  
                 */

                ViewCropRegionShapeManager vcrShapeMgr = viewDTO_.View_.GetCropRegionShapeManager();
                var nnn=viewDTO_.View_.CropBox;
                box = viewDTO_.View_.get_BoundingBox(null);
                PtoMin = box.Min - box.Min.X * XYZ.BasisX;
                PtoMax = box.Max +(delta_AnnotationCrop - box.Min.X) * XYZ.BasisX;
                Ancho = Math.Abs(PtoMax.X - PtoMin.X);
                Alto = Math.Abs(PtoMax.Y - PtoMin.Y+ delta_AnnotationCrop);

                var soloprueba = viewDTO_.View_.CropBox;

                ptoMinReal = nnn.Transform.OfPoint(nnn.Min);
                ptoMaxReal = nnn.Transform.OfPoint(nnn.Max);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CAlcularGeometria'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
