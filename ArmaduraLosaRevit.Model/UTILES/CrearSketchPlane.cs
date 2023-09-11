using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class CrearSketchPlane
    {

        public static SketchPlane crear(Document _doc, XYZ normalPlano, XYZ origin)
        {
            if (_doc == null) return null;
            Plane geomPlane = Plane.CreateByNormalAndOrigin(normalPlano, origin); // 2017

            // Create a sketch plane in current document
            SketchPlane sketch = SketchPlane.Create(_doc, geomPlane);
            return sketch;
        }

    }
}
