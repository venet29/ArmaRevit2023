
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
    public class NH_suples__DiseñoREcorrido
    {
        //0) propiedades

        public RotatedDimension odim { get; set; }

        // coordenadas reales donde se dibujan el reccorido
        public Point2d punto_ini_rango_ { get; set; }
        public Point2d punto_fin_rango_ { get; set; }


        //1) constructur

        public NH_suples__DiseñoREcorrido()
        {
            this.odim = new RotatedDimension();
        }
        //2) metodos

        public void DrawRotDimension_PLANTA( Database db, Transaction tr, Point2d pt1, Point2d pt2, double offset, string dimStyleName, float angle)
        {
            RutinasPolilinea RutinasPolilinea_ = new RutinasPolilinea();
            Document doc = Application.DocumentManager.MdiActiveDocument;

            Editor ed = doc.Editor;


            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
            DimStyleTable dtb = (DimStyleTable)tr.GetObject(db.DimStyleTableId, OpenMode.ForRead);
            if (!dtb.Has(dimStyleName))
                return;
            DimStyleTableRecord dtr = (DimStyleTableRecord)tr.GetObject(dtb[dimStyleName], OpenMode.ForWrite);
            dtr.Dimclrd = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 9);
            dtr.Dimtoh = false;
            dtr.Dimlfac = 0.1;
            dtr.Dimtxt = 0.00001;
            dtr.Dimdle = 7;
            dtr.Dimdli = 3; // // Baseline Spacing

            // Suppress Ext line 1
            dtr.Dimse1 = false;
            // / Suppress Ext line 2
            dtr.Dimse2 = false;
            // // Extend Beyond Dim Lines
            dtr.Dimexe = 10;
            // dtr.Dimexo = 10
            // // Offset From Origin
            dtr.Dimexo = 0;
            // // Fixed Length Extension Lines
            dtr.Dimtad = 1;
            // dtr.DimfxlenOn = False

            // dtr.Dimfxlen = 2 '; // Length


            // Dim ang As Double = AngleFromXAxis(pt1, pt2)
            Point3d pt3; // = PolarPoint(pt2, ang + Math.PI / 2, offset)


            float angulobarra = (float)comunes.coordenada__angulo_p1_p2_losa(pt1, pt2);
            if (Math.Abs(pt1.Y - pt2.Y) < 0.5)
            {
                pt3 = new Point3d(pt2.X, pt2.Y - offset, 0);


                punto_ini_rango_ = new Point2d(pt1.X, pt1.Y - offset);
                punto_fin_rango_ = new Point2d(pt2.X, pt2.Y - offset);
            }
            else if (angulobarra > 0)
            {
                pt3 = new Point3d(pt2.X - Math.Cos(angulobarra - Math.PI / 2) * offset, pt2.Y - Math.Sin(angulobarra - Math.PI / 2) * offset, 0);

                punto_ini_rango_ = new Point2d(pt1.X - Math.Cos(angulobarra - Math.PI / 2) * offset, pt1.Y - Math.Sin(angulobarra - Math.PI / 2) * offset);
                punto_fin_rango_ = new Point2d(pt2.X - Math.Cos(angulobarra - Math.PI / 2) * offset, pt2.Y - Math.Sin(angulobarra - Math.PI / 2) * offset);
            }
            else
            {
                pt3 = new Point3d(pt2.X + Math.Cos(angulobarra - Math.PI / 2) * offset, pt2.Y - Math.Sin(angulobarra + Math.PI / 2) * offset, 0);

                punto_ini_rango_ = new Point2d(pt1.X + Math.Cos(angulobarra - Math.PI / 2) * offset, pt1.Y - Math.Sin(angulobarra + Math.PI / 2) * offset);
                punto_fin_rango_ = new Point2d(pt2.X + Math.Cos(angulobarra - Math.PI / 2) * offset, pt2.Y - Math.Sin(angulobarra + Math.PI / 2) * offset);
            }


            odim = new RotatedDimension(angulobarra, new Point3d(pt1.X, pt1.Y, 0), new Point3d(pt2.X, pt2.Y, 0), pt3, "", dtr.ObjectId);  // "<>"
            odim.SetDatabaseDefaults();
            odim.Layer = "RANGOS";
            odim.DimensionText = "";
            // odim.Rotation = angle + Math.PI * 90 / 180.0
            odim.Suffix = "";

            // 'change some properties of the dimension if it is needs here
            // '..........................................
            btr.AppendEntity(odim);
            tr.AddNewlyCreatedDBObject(odim, true);
            //ents.Add(odim.ObjectId);
            // commit transaction or do it in the main program
            tr.Commit();
        }


    }
}
