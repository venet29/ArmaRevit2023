
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

namespace planta_aux_C.Elementos
{
   public  class NH_suples__Barra
    {
       // porpiedades
        public Point2d pt_ini { get; set; }
        public Point2d pt_fin { get; set; }
        public string st_layer { get; set; }
        public Polyline  acPoly { get; private set; }
       //1)
        public NH_suples__Barra(Point2d pt_ini, Point2d pt_fin, string st_layer)
       {
           acPoly = new Polyline();
           this.st_layer = st_layer;
           this.pt_ini = pt_ini;
           this.pt_fin = pt_fin;
       }
       public NH_suples__Barra()
       {
           acPoly = new Polyline();
       }
       //2) metodos
       public Polyline dibujar_barra_pl()
       {
           Document Doc = Application.DocumentManager.MdiActiveDocument;
           Database DB = Doc.Database;
           Editor Ed = Doc.Editor;
           Polyline acPoly = new Polyline();
           using (Transaction tr = DB.TransactionManager.StartTransaction())
           {
               // Abrir la tabla de bloques en modo lectura
               BlockTable acBlkTbl = (BlockTable)tr.GetObject(DB.BlockTableId, OpenMode.ForRead);
               // Abrir el registro del bloque de Espacio Modelo en modo escritura
               BlockTableRecord acBlkTblRec = (BlockTableRecord)tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

               // Crear un circulo en 2,2 con radio 0.5
               float dista1 = 20;

               ObjectId acPolyObj = acPoly.ObjectId;
               acPoly.SetDatabaseDefaults();
               acPoly.AddVertexAt(0, pt_ini, 0, 0, 0);
               acPoly.AddVertexAt(1, pt_fin, 0, 0, 0);
               acPoly.Layer = st_layer;

               // Añadir el nuevo objeto al registro de la tabla para bloques y a la
               // transaccion
               acBlkTblRec.AppendEntity(acPoly);
               tr.AddNewlyCreatedDBObject(acPoly, true);
               //if (ents != null)
               //    ents.Add(acPoly.ObjectId);
               tr.Commit();
           }
           return acPoly;
       }

    }
}
