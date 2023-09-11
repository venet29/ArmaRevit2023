
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
   public class NH_Base__Barra
    {

        public string st_layer { get; set; }
        public Polyline acPoly { get;  set; }
        public Point2dCollection listaPuntos   { get;  set; }

        public NH_Base__Barra()
        {
            acPoly = new Polyline();
            listaPuntos = new Point2dCollection();
        }


        //2) metodos
        public Polyline dibujar_barra_pl(bool isAbierta)
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

                ObjectId acPolyObj = acPoly.ObjectId;
                acPoly.SetDatabaseDefaults();
                for (int i = 0; i < listaPuntos.Count; i++)
                {
                    acPoly.AddVertexAt(i, listaPuntos[i], 0, 0, 0);   
                }
                acPoly.Closed = isAbierta;
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
