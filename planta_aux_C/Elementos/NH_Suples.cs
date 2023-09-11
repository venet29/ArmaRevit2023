
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

using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using planta_aux_C.Utiles;
using VARIOS_C;

namespace planta_aux_C.Elementos
{
    public class NH_Suples
    {
        //0)ppropiedades
        // actualmente nose para que se utiliza este propiedades y es igual Pt_InicialRecorrido,Pt_FinalRecorrido
        public Point2d punto_dim1 { get; set; }
        public Point2d punto_dim2 { get; set; }

        public Point2d punto_ini_dim_ { get; set; }
        public Point2d punto_fin_dim_ { get; set; }



        //coordenadas donde se dibuja el recorrido, pero no es lo mismo que el puntoque aparece en el dibujo
        public Point2d Pt_InicialRecorrido { get; set; }
        public Point2d Pt_FinalRecorrido { get; set; }


        //coordenadas de barra 
        public Point2d punto_inicial { get; set; }
        public Point2d punto_fin { get; set; }

        public string _direccion_LINEA { get; set; }

        public float angle { get; set; }

        //elemntos del suple --------------------------------------------------------
        public NH_suples__BarraSuple NH_BarraSuple { get; set; }
        public NH_suples__DiseñoREcorrido NH_Recorrido { get; set; }
        public NH_suples__DiseñoCirculo NH_RecorridoCirculo { get; set; }
        public Polyline NH_BarraSuple_aux { get; set; }
        public NH_suples__PerimetroBarraConRecorrido_Suples PerimetroBarraConRecorrido_Suples_ { get; set; }
        //---------------------------------------------------------------------------


        //1) contrutores


        public NH_Suples()
        {
            NH_BarraSuple = new NH_suples__BarraSuple();
            NH_Recorrido = new NH_suples__DiseñoREcorrido();
            NH_RecorridoCirculo = new NH_suples__DiseñoCirculo();
        }
        //2) metodos

        public void Dibujar_dimesion_suple2(ref ObjectId id, ref ObjectId id2, Point2d pt1, Point2d pt2, Point2d pt_ref, string direccion, float angle, Point3d pt1_, Point3d pt2_)
        {
            RutinasPolilinea RutinasPolilinea_ = new RutinasPolilinea();
            Document Doc = Application.DocumentManager.MdiActiveDocument;
            Database DB = Doc.Database;
            Editor Ed = Doc.Editor;
            Plane plane = new Plane(Point3d.Origin, Vector3d.ZAxis);
            // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            // clases

            dimensiones dimension = new dimensiones();

            // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            int factor_sentido = 1;

            if (angle < 0 & (direccion == "horizontal_i" | direccion == "horizontal_d"))
                factor_sentido = -1;

            if (direccion == "horizontal_i" | direccion == "horizontal_d")  //(float)(angle + (float)(Math.PI * 90 / (double)180))
                angle = (float)comunes.coordenada__angulo_p1_p2_losa((angle + Math.PI * 90 / 180));

            if (pt1_ == new Point3d(0, 0, 0) | pt2_ == new Point3d(0, 0, 0))
            {

                // Dim pini, pfin As String

                Point2d[] pini = new Point2d[2];
                Point2d[] pfin = new Point2d[2];

                Point2d[] coordenada_PTO_ = new Point2d[2];
                coordenada_PTO_ = comunes.coordenada_modificar(pt1, pt2);    // OBTENER P1 Y P2 ORDENADOS
                pt1 = pt1;
                pt2 = coordenada_PTO_[1];

                // pt1 siempre sera el menor x o menor y 
                // pt2 siempre sera el mayor x o mayor y

                string[] split_ini = RutinasPolilinea_.pto_inter2(Doc, DB, new Point3d(pt1.X, pt1.Y, 0), id, direccion).Split(new Char[] { ',', '\t' });
                string[] split_fin = RutinasPolilinea_.pto_inter2(Doc, DB, new Point3d(pt2.X, pt2.Y, 0), id2, direccion).Split(new Char[] { ',', '\t' });


                pini[0] = new Point2d(Convert.ToDouble(split_ini[0].ToString()), Convert.ToDouble(split_ini[1].ToString()));
                pini[1] = new Point2d(Convert.ToDouble(split_ini[2].ToString()), Convert.ToDouble(split_ini[3].ToString()));
                pfin[0] = new Point2d(Convert.ToDouble(split_fin[0]), Convert.ToDouble(split_fin[1]));
                pfin[1] = new Point2d(Convert.ToDouble(split_fin[2]), Convert.ToDouble(split_fin[3]));
                // 

                if (pini.Length == 2 & pfin.Length == 2)
                {
                    if ((Math.Abs(pini[0].X - pini[1].X) < 0.1 | Math.Abs(pini[0].Y - pini[1].Y) < 0.1) & (Math.Abs(pfin[0].X - pfin[1].X) < 0.1 | Math.Abs(pfin[0].Y - pfin[1].Y) < 0.1))
                    {
                        float[] dinosaurs;
                        if (direccion == "horizontal_i" | direccion == "horizontal_d")
                        {
                            dinosaurs = new[] { Convert.ToSingle(pini[0].Y), System.Convert.ToSingle(pini[1].Y), System.Convert.ToSingle(pfin[0].Y), System.Convert.ToSingle(pfin[1].Y) };
                            System.Array.Sort<float>(dinosaurs);
                            Pt_FinalRecorrido = new Point2d(pt_ref.X + 0, dinosaurs[1]);
                            Pt_InicialRecorrido = new Point2d(pt_ref.X + 0, dinosaurs[2]);
                        }
                        else
                        {
                            dinosaurs = new[] { System.Convert.ToSingle(pini[0].X), System.Convert.ToSingle(pini[1].X), System.Convert.ToSingle(pfin[0].X), System.Convert.ToSingle(pfin[1].X) };
                            System.Array.Sort<float>(dinosaurs);
                            Pt_InicialRecorrido = new Point2d(dinosaurs[1], pt_ref.Y - 0);
                            Pt_FinalRecorrido = new Point2d(dinosaurs[2], pt_ref.Y - 0);
                        }
                    }
                    else
                    {
                        float dista_ini_0 = (float)pini[0].GetDistanceTo(pt1);
                        float dista_fin_0 = (float)pfin[0].GetDistanceTo(pt2);

                        float dista_ini_1 = (float)pini[1].GetDistanceTo(pt1);
                        float dista_fin_1 = (float)pfin[1].GetDistanceTo(pt2);

                        if (dista_ini_0 < dista_fin_0)
                            Pt_InicialRecorrido = pini[0];
                        else
                            Pt_InicialRecorrido = new Point2d(pt1.X - Math.Cos(angle) * dista_fin_0, pt1.Y - Math.Sin(angle) * dista_fin_0);

                        if (dista_ini_1 < dista_fin_1)
                            Pt_FinalRecorrido = pini[1];
                        else
                            Pt_FinalRecorrido = new Point2d(pt1.X + Math.Cos(angle) * dista_fin_1, pt1.Y + Math.Sin(angle) * dista_fin_1);
                    }

                    using (Transaction tr = DB.TransactionManager.StartTransaction())
                    {
                        NH_Recorrido.DrawRotDimension_PLANTA(DB, tr, Pt_InicialRecorrido, Pt_FinalRecorrido, 25, "RANGOS", angle);
                    }
                }
                else
                {
                }
            }
            else
            {
                Pt_InicialRecorrido = new Point2d(pt1_.X, pt1_.Y);
                Pt_FinalRecorrido = new Point2d(pt2_.X, pt2_.Y);

                punto_dim1 = new Point2d(pt1_.X, pt1_.Y);
                punto_dim2 = new Point2d(pt2_.X, pt2_.Y);
                using (Transaction tr = DB.TransactionManager.StartTransaction())
                {
                    NH_Recorrido.DrawRotDimension_PLANTA(DB, tr, Pt_InicialRecorrido, Pt_FinalRecorrido, 25, "RANGOS", angle);
                }
            }

            //punto_ini_dim_ = new Point2d(punto_ini.X, punto_ini.Y);
            //punto_fin_dim_ = new Point2d(punto_fin.X, punto_fin.Y);
        }

        public void Dibujar_Circulo()
        {
            RutinasPolilinea RutinasPolilinea_ = new RutinasPolilinea();
            Document Doc = Application.DocumentManager.MdiActiveDocument;
            Database DB = Doc.Database;
            Editor Ed = Doc.Editor;



            Plane myPlaneWCS = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));
            IntPtr myintptr01 = new IntPtr();
            IntPtr myintptr02 = new IntPtr(); // , myPlaneWCS, pts, myintptr01, myintptr02)


            using (Transaction tr = DB.TransactionManager.StartTransaction())
            {
                Point3dCollection pts2 = new Point3dCollection();

                //RECORRIDO
                NH_suples__Barra NH_Barra_recorrido = new NH_suples__Barra(NH_Recorrido.punto_ini_rango_, NH_Recorrido.punto_fin_rango_, "BARRAS");
                Curve cur2_recorrido = NH_Barra_recorrido.dibujar_barra_pl();

                //BARRAS    
                NH_suples__Barra NH_Barra_2 = new NH_suples__Barra(punto_inicial, punto_fin, "BARRAS");
                Polyline acPoly = NH_Barra_2.dibujar_barra_pl();

                cur2_recorrido.IntersectWith(acPoly, Intersect.ExtendThis, myPlaneWCS, pts2, myintptr01, myintptr02);
                cur2_recorrido.Erase();
                acPoly.Erase();

                if (pts2.Count != 0)
                {

                    if (_direccion_LINEA == "vertical")
                        // PLANTA_.dibujar_circulo(acObjIdColl_borra_v2, New Point3d(min_datos(0)(0), min_datos(0)(1) - 10, 0), "", 0, PLANTA_.punto_cua_losa)
                        NH_RecorridoCirculo.dibujar_circulo(pts2[0], "", 0, 0);
                    else
                        // PLANTA_.dibujar_circulo(acObjIdColl_borra_v2, New Point3d(min_datos(0)(0) - 10, min_datos(0)(1), 0), "", 0, PLANTA_.punto_cua_losa)
                        NH_RecorridoCirculo.dibujar_circulo(pts2[0], "", 0, 0);
                }
                tr.Commit();
            }
        }

        public void Obtener_Area_que_Abarca_suple()
        {
            PerimetroBarraConRecorrido_Suples_ = new NH_suples__PerimetroBarraConRecorrido_Suples(punto_inicial, punto_fin, Pt_InicialRecorrido, Pt_FinalRecorrido, angle, _direccion_LINEA);
            PerimetroBarraConRecorrido_Suples_.CalculaPerimetroBarraConRecorrido_Suples();
            PerimetroBarraConRecorrido_Suples_.dibujar_PoligonoCircuncribeBarra_suple(false);

        }



    }
}
