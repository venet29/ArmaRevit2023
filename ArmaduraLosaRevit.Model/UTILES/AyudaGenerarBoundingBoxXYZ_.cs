using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda
{
    public class AyudaGenerarBoundingBoxXYZ
    {

        public bool CrearFetailViewSinTrans(Document _doc, Wall _wall)
        {
            try
            {

                var vft = TiposViewFamily.ObtenerTiposViewFamily(ViewFamily.Detail, _doc);
                var sectionBox = GetSectionViewPerpendiculatToWall(_wall);
                ViewSection.CreateDetail(_doc, vft.Id, sectionBox);

            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }


        BoundingBoxXYZ GetSectionViewPerpendiculatToWall(Wall wall)
        {
            LocationCurve lc = wall.Location as LocationCurve;

            // Using 0.5 and "true" to specify that the 
            // parameter is normalized places the transform
            // origin at the center of the location curve

            Transform curveTransform = lc.Curve.ComputeDerivatives(0.5, true);

            // The transform contains the location curve
            // mid-point and tangent, and we can obtain
            // its normal in the XY plane:

            XYZ origin = curveTransform.Origin;
            XYZ viewdir = curveTransform.BasisX.Normalize();
            XYZ up = XYZ.BasisZ;
            XYZ right = up.CrossProduct(viewdir);




            // Set up view transform, assuming wall's "up" 
            // is vertical. For a non-vertical situation 
            // such as section through a sloped floor, the 
            // surface normal would be needed

            Transform transform = Transform.Identity;
            transform.Origin = origin;
            transform.BasisX = right;
            transform.BasisY = up;
            transform.BasisZ = viewdir;

            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;

            // Min & Max X values define the section
            // line length on each side of the wall.
            // Max Y is the height of the section box.
            // Max Z (5) is the far clip offset.

            double d = wall.WallType.Width;
            BoundingBoxXYZ bb = wall.get_BoundingBox(null);
            double minZ = bb.Min.Z;
            double maxZ = bb.Max.Z;
            double h = maxZ - minZ;

            sectionBox.Min = new XYZ(-2 * d, -1, 0);
            sectionBox.Max = new XYZ(2 * d, h + 1, 5);

            return sectionBox;
        }


        public static BoundingBoxXYZ GetSectionViewPerpendiculatToWall(Curve linecurve, double ancho, double altura, View _view)
        {

            // Using 0.5 and "true" to specify that the 
            // parameter is normalized places the transform
            // origin at the center of the location curve

            Transform curveTransform = linecurve.ComputeDerivatives(0.5, true);

            // The transform contains the location curve
            // mid-point and tangent, and we can obtain
            // its normal in the XY plane:

            XYZ origin = curveTransform.Origin;
            XYZ viewdir = curveTransform.BasisX.Normalize();
            viewdir = new XYZ(0, 0, -1);
            XYZ up = XYZ.BasisZ;
            up = _view.ViewDirection;
            XYZ right = up.CrossProduct(viewdir);

            // Set up view transform, assuming wall's "up" 
            // is vertical. For a non-vertical situation 
            // such as section through a sloped floor, the 
            // surface normal would be needed

            Transform transform = Transform.Identity;
            transform.Origin = origin;
            transform.BasisX = right;
            transform.BasisY = up;
            transform.BasisZ = viewdir;

            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;

            // Min & Max X values define the section
            // line length on each side of the wall.
            // Max Y is the height of the section box.
            // Max Z (5) is the far clip offset.

            double d = ancho;// wall.WallType.Width;
                             //    BoundingBoxXYZ bb = wall.get_BoundingBox(null);
                             //  double minZ = bb.Min.Z;
                             //  double maxZ = bb.Max.Z;
            double h = altura;// maxZ - minZ;

            sectionBox.Min = new XYZ(-ancho / 2, -altura, -Util.CmToFoot(10));
            sectionBox.Max = new XYZ(ancho / 2, altura, Util.CmToFoot(10));

            return sectionBox;
        }


        public static BoundingBoxXYZ GetSectionconDIreccion(View _view,  GenerarBoundingBoxXYZDTO _generarBoxDTO ,
                                                double ancho_Z_foot = 0)
        {

            if (ancho_Z_foot == 0)
                ancho_Z_foot = ConstNH.CONST_ANCHO_CORTE_DESGLOSE;

            XYZ origin = _generarBoxDTO.origien;

            XYZ direccionX__entrandoView = _generarBoxDTO.direccionY_paralelaVIew.CrossProduct(_generarBoxDTO.direccionZ).Normalize();

            Transform transform = Transform.Identity;
            transform.Origin = _generarBoxDTO.origien;
            transform.BasisX = direccionX__entrandoView;
            transform.BasisY = _generarBoxDTO.direccionY_paralelaVIew;
            transform.BasisZ = _generarBoxDTO.direccionZ;

            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;


            sectionBox.Min = new XYZ(_generarBoxDTO.xmin , _generarBoxDTO.ymin, _generarBoxDTO.zmin);
            sectionBox.Max = new XYZ(_generarBoxDTO.xmax, _generarBoxDTO.ymax, _generarBoxDTO.zmax);

            return sectionBox;
        }


     
    }
    public class GenerarBoundingBoxXYZDTO
    {

        public XYZ origien { get; set; }

        public XYZ direccionZ { get; set; }
        public XYZ direccionY_paralelaVIew { get; set; }

        public double xmin { get; set; }
        public double ymin { get; set; }
        public double zmin { get; set; }
        public double xmax { get; set; }
        public double ymax { get; set; }
        public double zmax { get; set; }

    }
}
