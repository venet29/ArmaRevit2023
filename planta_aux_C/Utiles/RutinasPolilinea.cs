using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.IO;
using Autodesk;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;


using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using VARIOS_C;

namespace planta_aux_C.Utiles
{

    public class RutinasPolilinea
    {
        private static Point2d Aux_PuntoCentroidePolyLinea(Database acCurDb, ObjectId objects_)
        {
            Point2d centroid;
            Transaction tr = acCurDb.TransactionManager.StartTransaction();
            using (tr)
            {

                DBObject ent = tr.GetObject(objects_, OpenMode.ForWrite);
                Polyline p = ent as Polyline;

                // add the polylines to the array
                DBObjectCollection dbobjs = new DBObjectCollection();
                dbobjs.Add(ent);
                // create solid to get region and centroid 
                Solid3d Solid = new Solid3d();
                Solid.Extrude(((Region)Region.CreateFromCurves(dbobjs)[0]), 1, 0);
                centroid = new Point2d(Solid.MassProperties.Centroid.X, Solid.MassProperties.Centroid.Y);
                Solid.Dispose();


                // Save the new objects to the database
                tr.Commit();
            }


            return centroid;
        }

        // busca el tramo de polilinea 'id' que contenga al punto 'pt'
        public static Point3d[] pto_inter3(Document doc, Point3d pt, ObjectId id, string direc)
        {
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Point3d[] datos_ = new Point3d[2];

            CoordinateSystem3d cs = ed.CurrentUserCoordinateSystem.CoordinateSystem3d;

            Plane plan = new Plane(Point3d.Origin, cs.Zaxis);

            int nCurVport = System.Convert.ToInt32(AcApp.GetSystemVariable("CVPORT"));
            int valor_ = System.Convert.ToInt32(AcApp.GetSystemVariable("OSMODE"));

            Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("osmode", 512);

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Entity ename;


                    Point3d ptWcs;

                    // Transform from UCS to WCS

                    Matrix3d mat = Matrix3d.AlignCoordinateSystem(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis, cs.Origin, cs.Xaxis, cs.Yaxis, cs.Zaxis);
                    ptWcs = pt.TransformBy(mat);

                    ename = tr.GetObject(id, OpenMode.ForRead) as Entity;

                    Polyline pline = ename as Polyline;



                    Curve curv;
                    curv = ename as Curve;
                    if (curv == null)
                    {
                        ed.WriteMessage("Could not cast object as Curve.");
                        goto ir_final;
                    }
                    if (!curv.IsWriteEnabled)
                        curv.UpgradeOpen();



                    Point3d clickpt = pline.GetClosestPointTo(ptWcs, false);

                    for (int c = 0; c <= pline.NumberOfVertices - 1; c++)
                    {
                        double segParam = new double();

                        // This is the test filter here...it uses the 

                        // nifty API OnSegmentAt

                        if (pline.OnSegmentAt(c, clickpt.Convert2d(plan), segParam))
                        {

                            // ed.WriteMessage(vbLf & "Selected Segment: {0} " & vbLf, c + 1)

                            LineSegment3d axzua_linea = pline.GetLineSegmentAt(c);
                            // axzua_linea = pline.GetLineSegmentAt(c)


                            // datos_(0)  es el mayor x si es horizontal o inclinda y si es complatamente vertical es y mayor altura

                            if (Math.Abs(axzua_linea.EndPoint.X - axzua_linea.StartPoint.X) < 0.1)
                            {
                                if (axzua_linea.StartPoint.Y > axzua_linea.EndPoint.Y)
                                {
                                    datos_[0] = new Point3d(axzua_linea.StartPoint.X, axzua_linea.StartPoint.Y, 0);
                                    datos_[1] = new Point3d(axzua_linea.EndPoint.X, axzua_linea.EndPoint.Y, 0);
                                }
                                else
                                {
                                    datos_[0] = new Point3d(axzua_linea.EndPoint.X, axzua_linea.EndPoint.Y, 0);
                                    datos_[1] = new Point3d(axzua_linea.StartPoint.X, axzua_linea.StartPoint.Y, 0);
                                }
                            }
                            else if (direc == "horizontal_i" | direc == "horizontal_d")
                            {
                                if (axzua_linea.EndPoint.Y < axzua_linea.StartPoint.Y)
                                {
                                    datos_[0] = new Point3d(axzua_linea.StartPoint.X, axzua_linea.StartPoint.Y, 0);
                                    datos_[1] = new Point3d(axzua_linea.EndPoint.X, axzua_linea.EndPoint.Y, 0);
                                }
                                else
                                {
                                    datos_[0] = new Point3d(axzua_linea.EndPoint.X, axzua_linea.EndPoint.Y, 0);
                                    datos_[1] = new Point3d(axzua_linea.StartPoint.X, axzua_linea.StartPoint.Y, 0);
                                }
                            }
                            else if (axzua_linea.EndPoint.X < axzua_linea.StartPoint.X)
                            {
                                datos_[0] = new Point3d(axzua_linea.StartPoint.X, axzua_linea.StartPoint.Y, 0);
                                datos_[1] = new Point3d(axzua_linea.EndPoint.X, axzua_linea.EndPoint.Y, 0);
                            }
                            else
                            {
                                datos_[0] = new Point3d(axzua_linea.EndPoint.X, axzua_linea.EndPoint.Y, 0);
                                datos_[1] = new Point3d(axzua_linea.StartPoint.X, axzua_linea.StartPoint.Y, 0);
                            }





                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message + "  " + ex.StackTrace);
                }

            ir_final:
                ;
            }


            Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("osmode", valor_);
            return datos_;
        }


        public Point3dCollection BuscarPtoMenorDistancia(Point3dCollection lista, Point3d pto_refe)
        {
            Point3dCollection p3ds = new Point3dCollection();
            bool aux_izq_bajo = true;
            bool aux_dere_arriba = true;

            Point3d pto_inf = new Point3d(0, 0, 0);
            Point3d pto_sup = new Point3d(0, 0, 0);
            float dist_inf = 1000000;
            float dist_sup = 1000000;

            foreach (Point3d p in lista)
            {
                if (Math.Abs(pto_refe.X - p.X) < 0.5)
                {
                    if (p.Y > pto_refe.Y)
                    {
                        if (dist_sup > p.DistanceTo(pto_refe))
                        {
                            dist_sup = (float)p.DistanceTo(pto_refe);
                            pto_sup = p;
                        }
                    }
                    else if (dist_inf > p.DistanceTo(pto_refe))
                    {
                        dist_inf = (float)p.DistanceTo(pto_refe);
                        pto_inf = p;
                    }
                }
                else if (p.X > pto_refe.X)
                {
                    if (dist_sup > p.DistanceTo(pto_refe))
                    {
                        dist_sup = (float)p.DistanceTo(pto_refe);
                        pto_sup = p;
                    }
                }
                else if (dist_inf > p.DistanceTo(pto_refe))
                {
                    dist_inf = (float)p.DistanceTo(pto_refe);
                    pto_inf = p;
                }
            }



            Point3d[] coordenada_PTO_ = new Point3d[2];
            coordenada_PTO_ = comunes.coordenada_modificar(pto_inf, pto_sup);

            p3ds.Add(coordenada_PTO_[0]);
            p3ds.Add(coordenada_PTO_[1]);


            return p3ds;
        }

 
        // BUSCA SOBRE UNA POLILINEA LA PROYECCION DE ALGUN PUNTO EXTERNO
        public string pto_inter2( Document doc, Database db, Point3d pt,  ObjectId id,  string direc)
        {
     

            Editor ed = doc.Editor;
            string pto_inter2 = "";

            CoordinateSystem3d cs = ed.CurrentUserCoordinateSystem.CoordinateSystem3d;

            Plane plan = new Plane(Point3d.Origin, cs.Zaxis);

            int valor_ = System.Convert.ToInt32(AcApp.GetSystemVariable("OSMODE"));
            Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("osmode", 512);

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Entity ename;
                    Point3d ptWcs;

                    // Transform from UCS to WCS
                    Matrix3d mat = Matrix3d.AlignCoordinateSystem(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis, cs.Origin, cs.Xaxis, cs.Yaxis, cs.Zaxis);
                    ptWcs = pt.TransformBy(mat);
                    ename = tr.GetObject(id, OpenMode.ForRead) as Entity;
                    Polyline pline = ename as Polyline;

                    Curve curv;
                    curv = ename as Curve;
                    if (curv == null)
                    {
                        ed.WriteMessage("\r Could not cast object as Curve.");
                        return pto_inter2;
                    }
                    if (!curv.IsWriteEnabled)
                        curv.UpgradeOpen();

                    Point3d clickpt = pline.GetClosestPointTo(ptWcs, false);
                    for (int c = 0; c <= pline.NumberOfVertices - 1; c++)
                    {
                        double segParam = new double();
                        // This is the test filter here...it uses the 
                        // nifty API OnSegmentAt
                        if (pline.OnSegmentAt(c, clickpt.Convert2d(plan), segParam))
                        {
                            // ed.WriteMessage(vbLf & "Selected Segment: {0} " & vbLf, c + 1)
                            LineSegment3d axzua_linea = pline.GetLineSegmentAt(c);
                            // axzua_linea = pline.GetLineSegmentAt(c)
                            Point3d pt1, pt2;
                            Point3d[] coordenada_PTO_ = new Point3d[2];
                            coordenada_PTO_ = comunes.coordenada_modificar( new Point3d(axzua_linea.EndPoint.X, axzua_linea.EndPoint.Y, 0), new Point3d(axzua_linea.StartPoint.X, axzua_linea.StartPoint.Y, 0));    // OBTENER P1 Y P2 ORDENADOS
                            pt1 = coordenada_PTO_[0];
                            pt2 = coordenada_PTO_[1];
                            pto_inter2 = pt1.X + "," + pt1.Y + "," + pt2.X + "," + pt2.Y;
                            // If direc = "horizontal" Then
                            break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("" + ex.Message + "" + ex.StackTrace);
                }

                }
            Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("osmode", valor_);
            return pto_inter2;
        }
     
        //obtiene una lista con los puntos de polilinea o entidad que tieneen los vertices
        //Polyline   >  Point2d
        //Polyline2d >  Vertex2d
        //Polyline3d >  PolylineVertex3d
        public static List<Point2d> ListVerticesPolinea(ObjectId ObjectId_)
       {

        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor ed = doc.Editor;
        Database db = doc.Database;

        List<Point2d> Lista = new List<Point2d>();

        if (ObjectId_ != null)
        {

            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {

                DBObject obj = tr.GetObject(ObjectId_, OpenMode.ForRead);

                // If a "lightweight" (or optimized) polyline

                Polyline lwp = obj as Polyline;

                if (lwp != null)
                {

                    // Use a for loop to get each vertex, one by one
                    int vn = lwp.NumberOfVertices;
                    for (int i = 0; i < vn; i++)
                    {
                        // Could also get the 3D point here
                        Point2d pt = lwp.GetPoint2dAt(i);
                        Lista.Add(new Point2d(pt.X, pt.Y));
                        ed.WriteMessage("\n" + pt.ToString());
                    }
              
                }

                else
                {
                    // If an old-style, 2D polyline
                    Polyline2d p2d = obj as Polyline2d;
                    if (p2d != null)
                    {
                        // Use foreach to get each contained vertex
                        foreach (ObjectId vId in p2d)
                        {
                            Vertex2d v2d = (Vertex2d)tr.GetObject(vId, OpenMode.ForRead);
                            Lista.Add(new Point2d(v2d.Position.X, v2d.Position.Y));
                            ed.WriteMessage("\n" + v2d.Position.ToString());
                        }
                    }

                    else
                    {
                        // If an old-style, 3D polyline
                        Polyline3d p3d = obj as Polyline3d;
                        if (p3d != null)
                        {
                            // Use foreach to get each contained vertex
                            foreach (ObjectId vId in p3d)
                            {
                                PolylineVertex3d v3d = (PolylineVertex3d)tr.GetObject(vId, OpenMode.ForRead);
                                Lista.Add(new Point2d(v3d.Position.X, v3d.Position.Y));
                                ed.WriteMessage("\n" + v3d.Position.ToString());
                            }
                        }
                    }

                }
                // Committing is cheaper than aborting
                tr.Commit();
            }

        }

        return Lista;

    }




    }
}

