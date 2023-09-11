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

using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using planta_aux_C.Utiles;
using planta_aux_C.enumera;
using planta_aux_C.Elementos;
using planta_aux_C;
using planta_aux_C.Elemento_Losa;
using planta_aux_C.RutinasSoloPruebas;
using VARIOS_C;


[assembly: CommandClass(typeof(planta_aux_C.Rutinas.Rutinas_Polilineas))]


namespace planta_aux_C.Rutinas
{
   public  class Rutinas_Polilineas
    {


       [CommandMethod("oseg")]
       public static void TEST_PointOnPoly()
       {

           Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
           Editor ed = AcApp.DocumentManager.MdiActiveDocument.Editor;


           Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("osmode", 512);

           using (Transaction tr = db.TransactionManager.StartTransaction())
           {
               try
               {
                   Entity ename;
                   Point3d pt;
                
                   PromptEntityOptions peo = new PromptEntityOptions("\nSelect Pline: ");
                   peo.SetRejectMessage("\nYou have to select LWPOLYLINE!");
                   peo.AddAllowedClass(typeof(Polyline), false);
                   PromptEntityResult res = ed.GetEntity(peo);

                   if (res.Status != PromptStatus.OK) return;

                   ObjectId id = res.ObjectId;
                   // Convert to WCS incase selection was made 
                   // while in a UCS.
                   pt = res.PickedPoint;
                    ename = tr.GetObject(id, OpenMode.ForRead) as Entity;
                   // Transform from UCS to WCS

                    Buscar_Inicio_Fin_Poly(ed,  ename, pt);
              
               }

               catch (System.Exception ex)
               {

                   ed.WriteMessage("\n" + ex.Message + "\n" + ex.StackTrace);

               }

           }

       }


       // entrega o genera NH_PolilineaRef de una linia o poliliena
       public static NH_PolilineaRef Buscar_Inicio_Fin_Poly(Editor ed, Entity ename, Point3d pt)
       {
           NH_PolilineaRef elemento = new NH_PolilineaRef();

           Point3d ptWcs;
           CoordinateSystem3d cs = ed.CurrentUserCoordinateSystem.CoordinateSystem3d;
           Plane plan = new Plane(Point3d.Origin, cs.Zaxis);

           Matrix3d mat = Matrix3d.AlignCoordinateSystem(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis, cs.Origin, cs.Xaxis, cs.Yaxis, cs.Zaxis);
           ptWcs = pt.TransformBy(mat);


           if (ename is Polyline)
           {
               Polyline pline = ename as Polyline;
               if (pline == null)
               {
                   ed.WriteMessage("\nSelected Entity is not a Polyline");
                   return elemento;
               }

               Point3d clickpt = pline.GetClosestPointTo(ptWcs, false);
               for (int c = 0; c < pline.NumberOfVertices; c++)
               {
                   double segParam = new double();
                   // This is the test filter here...it uses the 
                   // nifty API OnSegmentAt
                   if (pline.OnSegmentAt(c, clickpt.Convert2d(plan), segParam))
                   {
                       ed.WriteMessage("\nSelected Segment: {0} \n", c + 1);
                       var lineaElem = pline.GetLineSegment2dAt(c);
                       elemento.ptIni = lineaElem.StartPoint;
                       elemento.ptFin = lineaElem.EndPoint;
                       elemento.linea = lineaElem;
                       elemento.poly = pline;
                       elemento.angle = comunes.coordenada__angulo_p1_p2_losa(elemento.ptIni, elemento.ptFin);
                       break;


                   }

               }
           }
           else if (ename is Line)
           {

               Line line = ename as Line;
               elemento.ptIni = line.StartPoint.Convert2d(plan);
               elemento.ptFin = line.EndPoint.Convert2d(plan);             
               elemento.line = line;
               elemento.angle = comunes.coordenada__angulo_p1_p2_losa(elemento.ptIni, elemento.ptFin);
           }
           return elemento ;
       }
    }



}
