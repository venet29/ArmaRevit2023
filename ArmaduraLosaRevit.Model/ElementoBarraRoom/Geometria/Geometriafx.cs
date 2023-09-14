using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Geometria
{
    public class Geometriafx
    {

        private XYZ pf1;
        private XYZ pf2;
        private XYZ pf3;
        private XYZ pf4;
        private readonly XYZ ptoSelect_tras;
        private TipoOrientacionBarra _tipoOrientacionBarra;
        private Transform Invertrans1;
        private Transform InverTrans2_rotacion;
        private XYZ p1;
        private XYZ p2;
        private XYZ p3;
        private XYZ p4;
        private double LargoRecorrido_axu;

        public List<XYZ> Lista { get; set; }
        public XYZ pt1Inv { get; set; }
        public XYZ pt2Inv { get; set; }
        public XYZ pt3Inv { get; set; }
        public XYZ pt4Inv { get; set; }



        public Geometriafx(Transform invertrans1, Transform inverTrans2_rotacion, XYZ p1, XYZ p2, XYZ p3, XYZ p4, XYZ ptoSelect_tras, TipoOrientacionBarra _TipoOrientacionBarra)
        {
            this.Invertrans1 = invertrans1;
            this.InverTrans2_rotacion = inverTrans2_rotacion;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.ptoSelect_tras = ptoSelect_tras;
            Lista = new List<XYZ>();
            this._tipoOrientacionBarra = _TipoOrientacionBarra;
        }

        public bool Generar4Ptos()
        {
            try
            {
                LargoRecorrido_axu = 0;
                //NOta LargoRecorridoY , LargoRecorridoX viene del diseño automatico,
                //para diseño manual con mouse LargoRecorridoY=0 , LargoRecorridoX=0

                if (_tipoOrientacionBarra == TipoOrientacionBarra.Horizontal)
                    LargoRecorrido_axu = BarraRoomGeometria.LargoRecorridoY;
                else
                    LargoRecorrido_axu = BarraRoomGeometria.LargoRecorridoX;

                M1_AnalizandoP1_P4_EnY();

                //analizar inferiro
                M2_AnalaizarP2_P3_enY();

                p1 = pf1.ObtenerCopia();
                p2 = pf2.ObtenerCopia();
                p3 = pf3.ObtenerCopia();
                p4 = pf4.ObtenerCopia();

                M3_AnalizandoP1_P2_EnX();

                M4_AnalaizarP4_P3_enX();

                if (DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO && false && RedonderLargoBarras.RedondearPtos_5MasCercano_Alt(pf1, pf2, pf3, pf4))//TipoBarraInicial == "fxninguno" &&
                {
                    pf1 = RedonderLargoBarras.CoordenadaPath4.p1;
                    pf2 = RedonderLargoBarras.CoordenadaPath4.p2;
                    pf3 = RedonderLargoBarras.CoordenadaPath4.p3;
                    pf4 = RedonderLargoBarras.CoordenadaPath4.p4;
                }

                pt1Inv = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf1));
                pt2Inv = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf2));
                pt3Inv = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf3));
                pt4Inv = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(pf4));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void M1_AnalizandoP1_P4_EnY()
        {
            if (p4.Y > p1.Y)
            {
                //NOTA REVISAR POR P1 Y P4 QUEDA IGUAL CUANDO BBarraRoomGeometria.LargoRecorridoY DISTINTO DE CERO
                if (BarraRoomGeometria.LargoRecorridoY != 0)// && ptoSelect_tras.Y + LargoRecorrido_axu / 2 < p1.Y)
                                                            //pf1 = new XYZ(p1.X, ptoSelect_tras.Y + LargoRecorrido_axu / 2, 0);
                {
                    double valory = (ptoSelect_tras.Y + LargoRecorrido_axu < p1.Y ? ptoSelect_tras.Y + LargoRecorrido_axu : p1.Y);
                    pf1 = new XYZ(p1.X, valory, p1.Z);
                }
                else
                    pf1 = p1;

                pf4 = Util.Intersection(Line.CreateBound(p3.GetXY0(), p4.GetXY0()), Line.CreateBound(new XYZ(-1000, pf1.Y, 0), new XYZ(1000, pf1.Y, 0))).AsignarZ(p4.Z);
            }
            else if (p4.Y <= p1.Y)
            {
                if (BarraRoomGeometria.LargoRecorridoY != 0)// && ptoSelect_tras.Y + LargoRecorrido_axu / 2 < p4.Y)
                {
                    double valory = (ptoSelect_tras.Y + LargoRecorrido_axu < p4.Y ? ptoSelect_tras.Y + LargoRecorrido_axu : p4.Y);
                    pf4 = new XYZ(p4.X, valory, p4.Z);
                }
                else
                    pf4 = p4;
                pf1 = Util.Intersection(Line.CreateBound(p1.GetXY0(), p2.GetXY0()), Line.CreateBound(new XYZ(-1000, pf4.Y, 0), new XYZ(1000, pf4.Y, 0))).AsignarZ(p1.Z);
            }
        }

        private void M2_AnalaizarP2_P3_enY()
        {
            if (p2.Y > p3.Y)
            {
                //NOTA REVISAR POR P1 Y P2 QUEDA IGUAL CUANDO BarraRoomGeometria.LargoRecorridoY DISTINTO DE CERO
                if (BarraRoomGeometria.LargoRecorridoY != 0)// && ptoSelect_tras.Y - LargoRecorrido_axu / 2 > p2.Y)
                {
                    double valory = (ptoSelect_tras.Y - LargoRecorrido_axu > p2.Y ? ptoSelect_tras.Y - LargoRecorrido_axu : p2.Y);
                    pf2 = new XYZ(p2.X, valory, p2.Z);
                }
                else
                    pf2 = p2;


                pf3 = Util.Intersection(Line.CreateBound(p3.GetXY0(), p4.GetXY0()), Line.CreateBound(new XYZ(-1000, pf2.Y, 0), new XYZ(1000, pf2.Y, 0))).AsignarZ(p3.Z);
            }
            else if (p2.Y <= p3.Y)
            {

                if (BarraRoomGeometria.LargoRecorridoY != 0)// && ptoSelect_tras.Y - LargoRecorrido_axu / 2 > p3.Y)
                {
                    double valory = (ptoSelect_tras.Y - LargoRecorrido_axu  > p3.Y ? ptoSelect_tras.Y - LargoRecorrido_axu : p3.Y);
                    pf3 = new XYZ(p3.X, valory, p3.Z);
                }
                else
                    pf3 = p3;
                pf2 = Util.Intersection(Line.CreateBound(p1.GetXY0(), p2.GetXY0()), Line.CreateBound(new XYZ(-1000, pf3.Y, 0), new XYZ(1000, pf3.Y, 0))).AsignarZ(p2.Z);
            }
        }

        private void M3_AnalizandoP1_P2_EnX()
        {
            if (p2.X > p1.X)
            {
                if (BarraRoomGeometria.LargoRecorridoX != 0)// && ptoSelect_tras.Y + LargoRecorrido_axu / 2 < p1.Y)
                                                            //pf1 = new XYZ(p1.X, ptoSelect_tras.Y + LargoRecorrido_axu / 2, 0);
                {
                    //nota dos lineas sin compreobar pq estan para enfierrado automatico
                    double valorX = (ptoSelect_tras.X - LargoRecorrido_axu > p2.X ? ptoSelect_tras.X - LargoRecorrido_axu : p2.X);
                    pf2 = new XYZ(valorX,p2.Y, p2.Z);
                }
                else
                    pf2 = p2;

                pf1 = Util.Intersection(Line.CreateBound(p1.GetXY0(), p4.GetXY0()), Line.CreateBound(new XYZ(pf2.X ,- 1000,  0), new XYZ(pf2.X,1000, 0))).AsignarZ(p1.Z);
            }
            else if (p2.X  <= p1.X)
            {
                if (BarraRoomGeometria.LargoRecorridoX != 0)// && ptoSelect_tras.Y + LargoRecorrido_axu / 2 < p4.Y)
                {
                    //nota dos lineas sin compreobar pq estan para enfierrado automatico
                    double valorx = (ptoSelect_tras.X - LargoRecorrido_axu > p1.X ? ptoSelect_tras.X - LargoRecorrido_axu : p1.X);
                    pf1 = new XYZ(valorx, p1.Y, p1.Z);
                }
                else
                    pf1 = p1;
                pf2 = Util.Intersection(Line.CreateBound(p2.GetXY0(), p3.GetXY0()), Line.CreateBound(new XYZ(pf1.X, -1000, 0), new XYZ(pf1.X, 1000, 0))).AsignarZ(p2.Z);
            }
        }


        private void M4_AnalaizarP4_P3_enX()
        {
            if (p4.X > p3.X)
            {

                if (BarraRoomGeometria.LargoRecorridoX != 0)// && ptoSelect_tras.Y - LargoRecorrido_axu / 2 > p2.Y)
                {
                    double valorx = (ptoSelect_tras.Y + LargoRecorrido_axu < p3.X ? ptoSelect_tras.X + LargoRecorrido_axu : p3.X);
                    pf3 = new XYZ(valorx,p3.Y, p3.Z);
                }
                else
                    pf3 = p3;


                pf4 = Util.Intersection(Line.CreateBound(p1.GetXY0(), p4.GetXY0()), Line.CreateBound(new XYZ( pf3.X, -1000, 0), new XYZ( pf3.X, 1000, 0))).AsignarZ(p4.Z);
            }
            else if (p4.X <= p3.X)
            {

                if (BarraRoomGeometria.LargoRecorridoX != 0)// && ptoSelect_tras.Y - LargoRecorrido_axu / 2 > p3.Y)
                {
                    double valorX = (ptoSelect_tras.X + LargoRecorrido_axu < p4.Y ? ptoSelect_tras.X + LargoRecorrido_axu : p4.X);
                    pf4 = new XYZ(valorX,p4.Y,  p4.Z);
                }
                else
                    pf4 = p4;

                pf3 = Util.Intersection(Line.CreateBound(p2.GetXY0(), p3.GetXY0()), Line.CreateBound(new XYZ( pf4.X, -1000, 0), new XYZ( pf4.X, 1000, 0))).AsignarZ(p3.Z);
            }
        }
    }
}
