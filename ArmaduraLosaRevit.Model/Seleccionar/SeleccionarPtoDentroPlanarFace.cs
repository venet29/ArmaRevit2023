using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarPtoDentroPlanarFace
    {


        /// <summary>
        /// busca si pto esta al interior PlanarFace 
        /// </summary>
        /// <param name="pt"> pto de pelota de losa</param>
        /// <param name="PFace">planarface deonde se busca si contiene el pto</param>
        /// <returns> true-false </returns>
        public static bool EStaPuntoALInteriroDeCaraDeUnaLosa(XYZ pt, PlanarFace PFace)
        {
            if (PFace == null) return false;
            if (pt == null) return false;
            Transform Trans = PFace.ComputeDerivatives(new UV(0, 0));
            
            XYZ Pt = Trans.Inverse.OfPoint(pt);
            IntersectionResult Res = null;
            bool outval = PFace.IsInside(new UV(Pt.X, Pt.Y), out Res);
            return outval;
        }


        public static bool IsCaraNormalEnEjeZPositivo(XYZ VectorNormalCara)
        {
            XYZ Valorref = new XYZ(0, 0, 1);
            double resul = VectorNormalCara.DotProduct(Valorref);
            return (Util.IsSimilarValor(resul, 1, 0.001) ? true : false);
        }

        public static bool IsCaraNormalEnEjeZNegativo(XYZ VectorNormalCara)
        {
            XYZ Valorref = new XYZ(0, 0, -1);
            double resul = VectorNormalCara.DotProduct(Valorref);
            return (Util.IsSimilarValor(resul, 1, 0.001) ? true : false);
        }

    }
}
