
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


namespace planta_aux_C.Utiles
{
    public class Transform
    {


        public static void MoveObject(ObjectId ObjectId_,Point3d acPt3d_inicial, Point3d acPt3d_final)
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],OpenMode.ForWrite) as BlockTableRecord;


                    Entity enty = (Entity)acTrans.GetObject(ObjectId_, OpenMode.ForWrite);


                    Vector3d acVec3d = acPt3d_inicial.GetVectorTo(acPt3d_final);

                    enty.TransformBy(Matrix3d.Displacement(acVec3d));

                // Save the new objects to the database
                acTrans.Commit();
            }
        }
    }
}
