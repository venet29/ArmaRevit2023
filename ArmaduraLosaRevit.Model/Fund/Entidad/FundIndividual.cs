using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.Entidad
{
    public class FundIndividual
    {
        private  UIApplication uiapp;
        private  CurveLoop _curveLoop;

        //public Edge Borde { get; set; }
        public bool IsOk { get; set; }

        public List<XYZ> ListaVertices { get; set; }
        public List<BordeDefundacione> ListaBordeDefundacione { get; set; }

        private UV[] verticesUV;

        public FundIndividual(UIApplication uiapp, CurveLoop curveLoop)
        {
            this.uiapp = uiapp;
            this._curveLoop = curveLoop;

            int cantidadCurvas = curveLoop.Count();

            this.IsOk = true;
            this.verticesUV = new UV[(int)cantidadCurvas];
            this.ListaBordeDefundacione = new List<BordeDefundacione>();
            this.ListaVertices = new List<XYZ>();

            M0_GenerarListaPtos();
        }

        public FundIndividual()
        {
            this.IsOk = false;
        }

        private void M0_GenerarListaPtos()
        {
            int cont = 0;
            BordeDefundacione.count = 0;
            foreach (Curve _curve in _curveLoop)
            {
                if (_curve.Length < Util.CmToFoot(0.1)) continue;
                ListaBordeDefundacione.Add(new BordeDefundacione(_curve));
                XYZ ptoinicial = _curve.GetEndPoint(0);


                ptoinicial = new XYZ(ptoinicial.X, ptoinicial.Y, ptoinicial.Z);

                if (!ListaVertices.Contains(ptoinicial)) ListaVertices.Add(ptoinicial);

                UV nuevoUV = new UV(ptoinicial.X, ptoinicial.Y);
                if (!verticesUV.Contains(nuevoUV)) verticesUV[cont] = nuevoUV;
                cont += 1;
            }

        }

        public bool IsPtoDentroShaf(XYZ ptomouse)
        {
            UV nuevoUV = new UV(ptomouse.X, ptomouse.Y);
            PointInPoly _PointInPoly = new PointInPoly();
            IsOk = _PointInPoly.PolygonContains(verticesUV, nuevoUV);
            return IsOk;
        }

        public bool M2_IsMAs2Ptos()
        {
            if (ListaVertices == null) return false;
            return (ListaVertices.Count > 2 ? true : false);
        }

        public bool Ordear4pto()
        {
            try
            {
                if (ListaVertices.Count != 4)
                {
                    IsOk = false;
                    return false;
                }

                double Xmax = ListaVertices.Max(c => c.X);
                double Xmin = ListaVertices.Min(c => c.X);

                double Ymax = ListaVertices.Max(c => c.Y);
                double Ymin = ListaVertices.Min(c => c.Y);

                if ((ListaVertices.Count(c => Math.Abs(c.X - Xmax) < 0.001) != 2) ||
                     (ListaVertices.Count(c => Math.Abs(c.X - Xmin) < 0.001) != 2) ||
                     (ListaVertices.Count(c => Math.Abs(c.Y - Ymax) < 0.001) != 2) ||
                     (ListaVertices.Count(c => Math.Abs(c.Y - Ymin) < 0.001) != 2))
                {
                    IsOk = false;
                    return false;
                }

                XYZ p1_aux = ListaVertices.Where(c => Util.IsEqual(c.X, Xmin, 0.0001) && Util.IsEqual(c.Y, Ymax, 0.0001)).FirstOrDefault();
                XYZ p2_aux = ListaVertices.Where(c => Util.IsEqual(c.X, Xmin, 0.0001) && Util.IsEqual(c.Y, Ymin, 0.0001)).FirstOrDefault();
                XYZ p3_aux = ListaVertices.Where(c => Util.IsEqual(c.X, Xmax, 0.0001) && Util.IsEqual(c.Y, Ymin, 0.0001)).FirstOrDefault();
                XYZ p4_aux = ListaVertices.Where(c => Util.IsEqual(c.X, Xmax, 0.0001) && Util.IsEqual(c.Y, Ymax, 0.0001)).FirstOrDefault();
                if (p1_aux == null || p2_aux == null || p3_aux == null || p4_aux == null)
                {
                    IsOk = false;
                    return false;
                }

                ListaVertices[0] = p1_aux + new XYZ(+ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot, -ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot, 0);
                ListaVertices[1] = p2_aux + new XYZ(+ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot, +ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot, 0); ;
                ListaVertices[2] = p3_aux + new XYZ(-ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot, +ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot, 0); ;
                ListaVertices[3] = p4_aux + new XYZ(-ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot, -ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot, 0); ;
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al obtener 'Ordear4pto'. ex:{ex.Message} ");
                return false;
            }
            return true;
        }

    }


    public class BordeDefundacione
    {

        public static int count { get; set; } = 0;
        public Line Linea;

        public BordeDefundacione(Curve _curve)
        {
            count += 1;
            p1 = _curve.GetEndPoint(0);
            p2 = _curve.GetEndPoint(1);
            Debug.WriteLine($" cont:{count}    P1:{p1.REdondearString_foot(3)}    P2:{p2.REdondearString_foot(3)}  ");
            Linea = Line.CreateBound(_curve.GetEndPoint(0), _curve.GetEndPoint(1));
        }

        public XYZ p1 { get; set; }
        public XYZ p2 { get; set; }
    }
}
