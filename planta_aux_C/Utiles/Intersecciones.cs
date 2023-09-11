
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

using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using VARIOS_C;

namespace planta_aux_C.Utiles
{
    public class Intersecciones
    {
        public static Point2d ptoInterseccion { get; set; }
        public static Line LINE_auxDibujada { get; set; }

        public static bool InterSection4Point(Point3d pointer1a, Point3d pointer2a, Point3d pointer1b, Point3d pointer2b, Intersect tipo)
        {
            bool result = false;
         

            Line pl1 = null;
            Line pl2 = null;
         

                pl1 = new Line(pointer1a, pointer2a);
                pl2 = new Line(pointer1b, pointer2b);
                Point3dCollection pts3D = new Point3dCollection();
                //Get the intersection Points between line 1 and line 2
                pl1.IntersectWith(pl2, tipo, pts3D, IntPtr.Zero, IntPtr.Zero);
        
                if (pts3D.Count == 1)
                {
                    ptoInterseccion = new Point2d(pts3D[0].X, pts3D[0].Y);
                    return true;

                }
            
            return result;
        }


        //-----------------------------------------------------------------------------------------------------------------------------
        #region intersecccion Barra Polilinea

        /*
         ent_poligonoDeLosa -> poligono de losa
         acPoly_auxParaBarra -> linea de referecnia para intersectar ( es nulo se crea --  crea la Linea a contar del pto 'pto_refe' extendiendo a la izquierda 'largoExtensionIzq' y ala derecha 'largoExtensionDerecha' con un angulo 'anguloLinea'
        
        * solo se utiliza para crea linea auxiliar en caso de  'acPoly_auxParaBarra'=NULL
         'largoExtensionDerecha'= prolongacion izq
         'largoExtensionDerecha'= prolongacion dere
         'anguloLinea'= angulo de linea
         'pto_refe' = pto re refencia
        
         'dibujarBarras' = si dibuja o no dibuja linea de refeencia
         'TipoIntersect'= tipo de extension de la linea
        
        */

        // busca interseccion de una linea 'acPoly_auxParaBarra' con una polilinea de losa 'ent_poligonoDeLosa'(dentro de una distancia dada)
        // si 'acPoly_auxParaBarra'<> null  --->  crea la Linea a contar del pto 'pto_refe' extendiendo a la izquierda 'largoExtensionIzq' y ala derecha 'largoExtensionDerecha' con un angulo 'anguloLinea'
        public static Point3dCollection pto_interseccion_losa(Entity ent_poligonoDeLosa, Line acPoly_auxParaBarra, double largoExtensionIzq, double largoExtensionDerecha, double anguloLinea, Point3d pto_refe, bool dibujarBarras, bool isMostraPuntoInterseccion, Intersect TipoIntersect)
        {
            Point3d[] pts = new Point3d[2];
            Point3dCollection datos_ = new Point3dCollection();


            Database db = AcApp.DocumentManager.MdiActiveDocument.Database;
            Editor ed = AcApp.DocumentManager.MdiActiveDocument.Editor;
            CoordinateSystem3d cs = ed.CurrentUserCoordinateSystem.CoordinateSystem3d;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    //1) comprueba polilinea
                    if (ent_poligonoDeLosa == null)
                        return null;
                    //2) crea linea auxiliar          
                    //Line acLine = new Line(new Point3d(5, 5, 0), new Point3d(12, 3, 0));

                    if (acPoly_auxParaBarra == null)
                    {
                        acPoly_auxParaBarra = new Line(new Point3d(pto_refe.X - largoExtensionIzq * Math.Cos(anguloLinea), pto_refe.Y - largoExtensionIzq * Math.Sin(anguloLinea), 0),
                                                            new Point3d(pto_refe.X + largoExtensionDerecha * Math.Cos(anguloLinea), pto_refe.Y + largoExtensionDerecha * Math.Sin(anguloLinea), 0));
                        acPoly_auxParaBarra.ColorIndex = 1;
                    }

                    if (dibujarBarras)
                    {
                        // ' Open the Block table for read
                        BlockTable acBlkTbl;
                        acBlkTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                        // ' Open the Block table record Model space for write
                        BlockTableRecord acBlkTblRec;
                        acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                        acBlkTblRec.AppendEntity(acPoly_auxParaBarra);
                        tr.AddNewlyCreatedDBObject(acPoly_auxParaBarra, true);


                        Application.DocumentManager.MdiActiveDocument.Editor.UpdateScreen();
                        Application.DocumentManager.MdiActiveDocument.Editor.Regen();

                    }
                    // Entity ent_auxParaBarra = (Entity)tr.GetObject(acPoly_auxParaBarra.ObjectId, OpenMode.ForWrite);
                    if (acPoly_auxParaBarra == null)
                        return null;
                    LINE_auxDibujada = acPoly_auxParaBarra;
                    datos_ = buscar_Punto_Interseccion_BarrasEnPoligono(acPoly_auxParaBarra, ent_poligonoDeLosa, 0, pto_refe, TipoIntersect,false);
                    tr.Commit();
                }

                catch (System.Exception ex)
                {

                    ed.WriteMessage("\n" + ex.Message + "\n" + ex.StackTrace);

                }
                return datos_;
            }
        }

        // busca interseccion de una linea 'acPoly_auxParaBarra' con una polilinea de losa 'ent_poligonoDeLosa'(dentro de una distancia dada)
        // si 'acPoly_auxParaBarra'<> null  --->  crea la Linea a contar del pto 'pto_refe' extendiendo a la izquierda 'largoExtensionIzq' y ala derecha 'largoExtensionDerecha' con un angulo 'anguloLinea'
        public static Point3dCollection pto_interseccion_losa(ObjectId ObjectId_poligono_o_Line, Point3d pto_1, Point3d pto_2, bool dibujarBarras, bool isMostraPuntoInterseccion, Intersect TipoIntersect)
        {
            Point3d[] pts = new Point3d[2];
            Point3dCollection datos_ = new Point3dCollection();


            Database db = AcApp.DocumentManager.MdiActiveDocument.Database;
            Editor ed = AcApp.DocumentManager.MdiActiveDocument.Editor;
            CoordinateSystem3d cs = ed.CurrentUserCoordinateSystem.CoordinateSystem3d;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    //1) comprueba polilinea
                    if (ObjectId_poligono_o_Line == null)
                        return null;
                    Entity Polilinea_o_Line = tr.GetObject(ObjectId_poligono_o_Line, OpenMode.ForRead) as Entity;
                    //2) crea linea auxiliar          
                    //Line acLine = new Line(new Point3d(5, 5, 0), new Point3d(12, 3, 0));
                    Line acPoly_auxParaBarra= new Line(pto_1,pto_2);
                    acPoly_auxParaBarra.ColorIndex = 1;



                    if (dibujarBarras)
                    {
                        // ' Open the Block table for read
                        BlockTable acBlkTbl;
                        acBlkTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                        // ' Open the Block table record Model space for write
                        BlockTableRecord acBlkTblRec;
                        acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                        acBlkTblRec.AppendEntity(acPoly_auxParaBarra);
                        tr.AddNewlyCreatedDBObject(acPoly_auxParaBarra, true);


                        Application.DocumentManager.MdiActiveDocument.Editor.UpdateScreen();
                        Application.DocumentManager.MdiActiveDocument.Editor.Regen();

                    }
                    // Entity ent_auxParaBarra = (Entity)tr.GetObject(acPoly_auxParaBarra.ObjectId, OpenMode.ForWrite);
                    if (acPoly_auxParaBarra == null)
                        return null;
                    LINE_auxDibujada = acPoly_auxParaBarra;
                    datos_ = buscar_Punto_Interseccion_BarrasEnPoligono(acPoly_auxParaBarra, Polilinea_o_Line, 0, pto_1, TipoIntersect, true);
                    tr.Commit();
                }

                catch (System.Exception ex)
                {

                    ed.WriteMessage("\n" + ex.Message + "\n" + ex.StackTrace);

                }
                return datos_;
            }




        }



        public static Point3dCollection buscar_Punto_Interseccion_BarrasEnPoligono(Curve Barra, Entity poligonoDeLosa, double ANGLE, Point3d pto_inser, Intersect caso,bool todosLosaPtos)
        {

            Plane myPlaneWCS = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));
            IntPtr myintptr01 = new IntPtr();
            IntPtr myintptr02 = new IntPtr(); // , myPlaneWCS, pts, myintptr01, myintptr02)
            var rutinaspolilinea = new RutinasPolilinea();


            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            // datos_(0) =  es con menor valor de x   /   si completamente vertical y mas peqeño
            // datos_(1) =  es con mayor valor de x   /   si completamente vertical y mas grande
            Point3dCollection datos_ = new Point3dCollection();
            // Dim acPoly As Polyline = TryCast(acTrans.GetObject(Barra.ObjectId, OpenMode.ForRead), Polyline)


            // Rotar la p|olilínea 45 grados, alrededor de eje Z del SCU actual usando
            // el punto base (4,4.25,0)
            if (ANGLE != null && ANGLE != 0.0)
            {
                // Dim acPoly As Polyline = TryCast(Barra, Polyline)
                Matrix3d curUCSMatrix = acDoc.Editor.CurrentUserCoordinateSystem;
                CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;
                Barra.TransformBy(Matrix3d.Rotation(ANGLE, curUCS.Zaxis, pto_inser));
            }

            Point3dCollection pts = new Point3dCollection();
            Barra.IntersectWith(poligonoDeLosa, caso, myPlaneWCS, pts, myintptr01, myintptr02);

            if ((pts.Count % 2) == 0 & pts.Count != 0 & pts.Count != 2)
                // El número es par.
                pts = rutinaspolilinea.BuscarPtoMenorDistancia(pts, pto_inser);
            else if (pts.Count == 0)
                return pts;

            //datos_[0] = coordenada_PTO_[0];
            //datos_[1] = coordenada_PTO_[1];
            // Guardar el nuevo objeto
            return pts;
        }



        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------
        #region interseccion punto a linea (distancia mas cercana , perpendicular)
             
        public static Point3d PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(Point3d n1, Point3d n2, Point3d n3)
        {
            Point3d result2 = new Point3d();

            var aux = PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(new Point2d(n1.X, n1.Y), new Point2d(n2.X, n2.Y), new Point2d(n3.X, n3.Y));

            if (aux != null && (aux is Point2d))
            { return new Point3d(aux.X, aux.Y, 0); }
            else
            { return result2; }



        }

        public static float DistanciaPerpendicularmenteConLinea_desdeUnPunto(Point3d n1, Point3d n2, Point3d n3)
        {
            
           return DistanciaPerpendicularmenteConLinea_desdeUnPunto(new Point2d(n1.X, n1.Y), new Point2d(n2.X, n2.Y), new Point2d(n3.X, n3.Y));

        }
        public static float DistanciaPerpendicularmenteConLinea_desdeUnPunto(Point2d n1, Point2d n2, Point2d n3)
        {
         

            var aux = PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(new Point2d(n1.X, n1.Y), new Point2d(n2.X, n2.Y), new Point2d(n3.X, n3.Y));

            if (aux != null && (aux is Point2d))
            { return (float)aux.GetDistanceTo(new Point2d(n3.X, n3.Y)); }
            else
            { return -1.0f; }



        }

        // n1 y n2 definene la line, n3 defune el punto con el cual se busca result, que corresponde a punto que se intersectect perpendicular con la linea
        public static Point2d PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(Point2d n1, Point2d n2, Point2d n3)
        {
            Point2d result = new Point2d();
             Editor ed = AcApp.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                double x, y;
                var angle = comunes.coordenada__angulo_p1_p2_losa(n1, n2);

                if (Math.Abs(angle) < 0.0174532925199433)  //1 grado  ---> recorrido completamente horizontal
                {
                    x = n3.X;
                    y = n1.Y;
                }
                else
                { 
                //Primero hay que hallar la ecuación de la recta: Ax+By+C=0
                double A, B, C;
                A = n2.Y - n1.Y;
                B = -(n2.X - n1.X);
                C = -(n1.X * (n2.Y - n1.Y)) + (n1.Y * (n2.X - n1.X));
                //Tercero, hallar la recta perpendicular, a partir del punto y de la pendiente
                double A2, B2, C2;
                A2 = -B;
                B2 = A;
                C2 = (B * n3.X) - (A * n3.Y);
                //Último, hallar el punto de intersección de la recta con la perpendicular
             
                y = (((A2 * C / A) - C2) / (-(A2 * B / A) + B2));
                x = (((-B * y) - C) / A);
                //Nodo cruce = new Nodo(x, y);
                }
                result = new Point2d(x, y);

            }
            catch (System.Exception)
            {


            }


            return result;
        }

        #endregion
    }
}
