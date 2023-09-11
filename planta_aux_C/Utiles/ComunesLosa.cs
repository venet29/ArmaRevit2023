
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VARIOS;
using Microsoft.VisualBasic;
using Autodesk;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;
using Autodesk.Windows;
using planta_aux_C;
using Autodesk.AutoCAD.Geometry;
using planta_aux_C.Rutinas;

namespace planta_aux_C.Utiles
{
    public class ComunesLosa
    {

        public static int largo_traslapo(int diam)
        {
            int largo_traslapo = 0;
            switch (diam)
            {
                case 6:
                    {
                        largo_traslapo = 35;
                        break;
                    }

                case 8:
                    {
                        largo_traslapo = 50;
                        break;
                    }

                case 10:
                    {
                        largo_traslapo = 60;
                        break;
                    }

                case 12:
                    {
                        largo_traslapo = 70;
                        break;
                    }

                case 16:
                    {
                        largo_traslapo = 95;
                        break;
                    }

                case 18:
                    {
                        largo_traslapo = 105;
                        break;
                    }

                case 22:
                    {
                        largo_traslapo = 160;
                        break;
                    }

                case 25:
                    {
                        largo_traslapo = 180;
                        break;
                    }

                case 28:
                    {
                        largo_traslapo = 205;
                        break;
                    }

                case 32:
                    {
                        largo_traslapo = 230;
                        break;
                    }

                case 36:
                    {
                        largo_traslapo = 260;
                        break;
                    }
            }

            return largo_traslapo;
        }


        public static void BuscarExternderLineasSiNoSonIgual(ref List<NH_PolilineaRef> lista)
        {
            NH_PolilineaRef poli1;
            NH_PolilineaRef poli2;
            if (lista.Count == 2)
            {
                 poli1 = lista.ElementAt(0);
                 poli2 = lista.ElementAt(1);
            }
            else
            {
                 poli1 = lista.ElementAt(0);
                 poli2 = lista.ElementAt(0);
            }
        

            //pol1 > poli2
            if (poli1.ptIni.GetDistanceTo(poli1.ptFin) > poli2.ptIni.GetDistanceTo(poli2.ptFin))
            {

                Point2d nuevoP1 = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(poli2.ptIni, poli2.ptFin, poli1.ptIni);
                Point2d nuevoP2 = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(poli2.ptIni, poli2.ptFin, poli1.ptFin);
                //poli2.ptIni_ext_para_dibujar = nuevoP1;
                //poli2.ptFin_ext_para_dibujar = nuevoP2;
                poli2.angle = poli1.angle;
                poli2.ptIni = nuevoP1;
                poli2.ptFin = nuevoP2;
            }
            else//poli2>poli1
            {
                Point2d nuevoP1 = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(poli1.ptIni, poli1.ptFin, poli2.ptIni);
                Point2d nuevoP2 = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(poli1.ptIni, poli1.ptFin, poli2.ptFin);
                //poli1.ptIni_ext_para_dibujar = nuevoP1;
                //poli1.ptFin_ext_para_dibujar = nuevoP2;
                poli1.ptIni = nuevoP1;
                poli1.ptFin = nuevoP2;
                poli1.angle = poli2.angle;
            }

        }



        public static void BuscarAcortarLineasSiNoSonIgual(ref List<NH_PolilineaRef> lista)
        {
            var poli1 = lista.ElementAt(0);
            var poli2 = lista.ElementAt(1);

            //pol1 < poli2
            if (poli1.ptIni.GetDistanceTo(poli1.ptFin) < poli2.ptIni.GetDistanceTo(poli2.ptFin))
            {

                Point2d nuevoP1 = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(poli2.ptIni, poli2.ptFin, poli1.ptIni);
                Point2d nuevoP2 = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(poli2.ptIni, poli2.ptFin, poli1.ptFin);
                //poli2.ptIni_ext_para_dibujar = nuevoP1;
                //poli2.ptFin_ext_para_dibujar = nuevoP2;
                poli2.angle = poli1.angle;
                poli2.ptIni = nuevoP1;
                poli2.ptFin = nuevoP2;
            }
            else//poli2>poli1
            {
                Point2d nuevoP1 = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(poli1.ptIni, poli1.ptFin, poli2.ptIni);
                Point2d nuevoP2 = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(poli1.ptIni, poli1.ptFin, poli2.ptFin);
                //poli1.ptIni_ext_para_dibujar = nuevoP1;
                //poli1.ptFin_ext_para_dibujar = nuevoP2;
                poli1.ptIni = nuevoP1;
                poli1.ptFin = nuevoP2;
                poli1.angle = poli2.angle;
            }

        }
    }
}
