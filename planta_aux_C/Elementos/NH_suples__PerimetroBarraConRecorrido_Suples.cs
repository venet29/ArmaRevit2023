using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Autodesk;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;
using Autodesk.Windows;

using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using planta_aux_C.Utiles;



namespace planta_aux_C.Elementos
{
    public class NH_suples__PerimetroBarraConRecorrido_Suples
    {



        public Point2d Pt1 { get; set; }
        public Point2d Pt2 { get; set; }
        public string Direccion { get; set; }

        public Point2d Pt_InicialRecorrido { get; set; }
        public Point2d Pt_FinalRecorrido { get; set; }


        public Point2d Pt_PerimetroIni0 { get; set; }
        public Point2d Pt_PerimetroIni1 { get; set; }

        public Point2d Pt_PerimetroFin0 { get; set; }
        public Point2d Pt_PerimetroFin1 { get; set; }


        public float Angulo_losa { get; set; }
        public float LargoRecorrido { get; set; }


        public float LargoRecorrido_Sup_Dere { get; set; }
        public float LargoRecorrido_Abajo_Izq { get; set; }

        public Polyline acPolySuple_ { get; set; }
        //contructor
        public NH_suples__PerimetroBarraConRecorrido_Suples()
        {

        }
        public NH_suples__PerimetroBarraConRecorrido_Suples(Point2d Pt1, Point2d Pt2, Point2d Pt_InicialRecorrido, Point2d Pt_FinalRecorrido, float Angulo_losa, string Direccion)
        {
            this.Pt1 = Pt1;
            this.Pt2 = Pt2;
            this.Pt_InicialRecorrido = Pt_InicialRecorrido;
            this.Pt_FinalRecorrido = Pt_FinalRecorrido;
            this.Angulo_losa = Angulo_losa;
            this.Direccion = Direccion;
        }

        public void CalculaPerimetroBarraConRecorrido_Suples()
        {

            Point2d pt_ref;


            if (Intersecciones.InterSection4Point(new Point3d(Pt1.X, Pt1.Y, 0), new Point3d(Pt2.X, Pt2.Y, 0), new Point3d(Pt_InicialRecorrido.X, Pt_InicialRecorrido.Y, 0), new Point3d(Pt_FinalRecorrido.X, Pt_FinalRecorrido.Y, 0), Intersect.OnBothOperands))
            {
                pt_ref = new Point2d(Intersecciones.ptoInterseccion.X, Intersecciones.ptoInterseccion.Y);

                LargoRecorrido_Sup_Dere = (float)pt_ref.GetDistanceTo(Pt_InicialRecorrido); // distacioa superior , derecha
                LargoRecorrido_Abajo_Izq = (float)pt_ref.GetDistanceTo(Pt_FinalRecorrido); //bajo izaq


                int factor = 1;
                if (Angulo_losa < 0) factor = -1;
                //***************************************************************************************************
                float angle_ini = (Angulo_losa + (float)(Math.PI / 2 * factor));

                Pt_PerimetroIni0 = new Point2d(Pt1.X + LargoRecorrido_Sup_Dere * Math.Cos(angle_ini),
                                               Pt1.Y + LargoRecorrido_Sup_Dere * Math.Sin(angle_ini));

                Pt_PerimetroFin0 = new Point2d(Pt2.X + LargoRecorrido_Sup_Dere * Math.Cos(angle_ini),
                                               Pt2.Y + LargoRecorrido_Sup_Dere * Math.Sin(angle_ini));
                //****************************************************************************************************
                //angle = angulo pelota losa
                float angle_fin = (Angulo_losa - (float)(Math.PI / 2 * factor));
                Pt_PerimetroIni1 = new Point2d(Pt1.X + LargoRecorrido_Abajo_Izq * Math.Cos(angle_fin),
                                               Pt1.Y + LargoRecorrido_Abajo_Izq * Math.Sin(angle_fin));

                Pt_PerimetroFin1 = new Point2d(Pt2.X + LargoRecorrido_Abajo_Izq * Math.Cos(angle_fin),
                                               Pt2.Y + LargoRecorrido_Abajo_Izq * Math.Sin(angle_fin));


            }

        }


        public void dibujar_PoligonoCircuncribeBarra_suple(bool AgregarABaseDeDatos) // List(Of Point3d())
        {

            Document Doc = Application.DocumentManager.MdiActiveDocument;
            Database DB = Doc.Database;
            Editor Ed = Doc.Editor;
            // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            // clases



            // ***********************************************************************************************************



             acPolySuple_ = new Polyline();
            using (Transaction acTrans = DB.TransactionManager.StartTransaction())
            {

                // ' Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = (BlockTable)acTrans.GetObject(DB.BlockTableId, OpenMode.ForRead);

                // ' Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                // ' Create a polyline with two segments (3 points)

                acPolySuple_.AddVertexAt(0, Pt_PerimetroIni0, 0, 0, 0);
                acPolySuple_.AddVertexAt(1, Pt_PerimetroIni1, 0, 0, 0);
                acPolySuple_.AddVertexAt(2, Pt_PerimetroFin1, 0, 0, 0);
                acPolySuple_.AddVertexAt(3, Pt_PerimetroFin0, 0, 0, 0);
                acPolySuple_.Closed = true;
                // ' Add the new object to the block table record and the transaction
                if (AgregarABaseDeDatos)
                {
                    acBlkTblRec.AppendEntity(acPolySuple_);
                    acTrans.AddNewlyCreatedDBObject(acPolySuple_, true);
                    Application.DocumentManager.MdiActiveDocument.Editor.UpdateScreen();
                    Application.DocumentManager.MdiActiveDocument.Editor.Regen();
                }


                // 
                // ' Save the new object to the database
                acTrans.Commit();
            }


 
        }


    }
}
