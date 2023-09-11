using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public enum Posicion
    {
        Indefinido,
        Superior,
            Inferior
    }
    public class RuledFaceDTo {

        public XYZ midpoint { get; set; }
        public XYZ normal { get; set; }
        public XYZ minxyz { get; set; }
        public XYZ direccionPLano { get; set; }
        public RuledFace ReludfaceNH { get; internal set; }
        public Posicion Posicion { get; set; } = Posicion.Indefinido;
    }
   public static class ExtensionRuledFace
    {
        public static RuledFaceDTo ObtenerDatosRuledFace(this RuledFace rf)
        {
            BoundingBoxUV b = rf.GetBoundingBox();
            UV p = b.Min;
            UV q = b.Max;
            UV midparam = p + 0.5 * (q - p);
            XYZ midpoint = rf.Evaluate(midparam);
            XYZ normal = rf.ComputeNormal(midparam);
            XYZ minxyz = rf.Evaluate(b.Min);
            XYZ direccionPLano = (midpoint - minxyz).Normalize();

            return new RuledFaceDTo()
            {
                normal = normal,
                direccionPLano = direccionPLano,
                midpoint = midpoint,
                minxyz = minxyz,
                ReludfaceNH = rf
            };
        }



    }


}
