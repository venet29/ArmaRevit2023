
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
   public  class NH_suples__DiseñoCirculo
    {
       //0)propiedad
       public float factor_escala { get; set; }
       public Circle acCirc2 { get; set; }
       public Circle acCirc { get; set; }
       //1) constructor

       public NH_suples__DiseñoCirculo()
       {
            //acCirc2 = new Circle();
            //acCirc = new Circle();
            factor_escala = 1;
       }

       //2) metodos 





       public void dibujar_circulo( Point3d pt_ref,  string tipo,  float angulo, float espacio)
       {
           Document Doc = Application.DocumentManager.MdiActiveDocument;
           Database DB = Doc.Database;
           Editor Ed = Doc.Editor;
           using (Transaction tr = DB.TransactionManager.StartTransaction())
           {

               // Abrir la tabla de bloques en modo lectura
               BlockTable acBlkTbl = (BlockTable)tr.GetObject(DB.BlockTableId, OpenMode.ForRead);

               // Abrir el registro del bloque de Espacio Modelo en modo escritura
               BlockTableRecord acBlkTblRec = (BlockTableRecord)tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

               if (tipo == "f16" | tipo == "f16a" | tipo == "f16b" | tipo == "f16c" | tipo == "f17" | tipo == "f17b" | tipo == "f18" | tipo == "f18a" | tipo == "f19" | tipo == "f19a" | tipo == "f19b" | tipo == "f20" | tipo == "f20a" | tipo == "f20b" | tipo == "f21" | tipo == "f22")
               {
                   // Dim split_ As String() = Replace(LCase(cuantia), "%%c", "").Split(New [Char]() {"a"c, CChar(vbTab)})
                   espacio = 20;

                 acCirc2 = new Circle();
                   acCirc2.SetDatabaseDefaults();
                   acCirc2.Center = new Point3d(pt_ref.X + Math.Sin(angulo) * espacio, pt_ref.Y - Math.Cos(angulo) * espacio, 0);
                   acCirc2.Radius = 5 * factor_escala;
                   acCirc2.Layer = "RANGOS";
                   acBlkTblRec.AppendEntity(acCirc2);
                   tr.AddNewlyCreatedDBObject(acCirc2, true);
                   //ents.Add(acCirc2.ObjectId);
               }
               else
                   espacio = 0;


               // Crear un circulo en 2,2 con radio 0.5
               acCirc = new Circle();
               acCirc.SetDatabaseDefaults();
               acCirc.Center = new Point3d(pt_ref.X - Math.Sin(angulo) * espacio, pt_ref.Y + Math.Cos(angulo) * espacio, 0);
               acCirc.Radius = 5 * factor_escala;
               acCirc.Layer = "RANGOS";
               // Añadir el nuevo objeto al registro de la tabla para bloques y a la
               // transaccion
               acBlkTblRec.AppendEntity(acCirc);
               tr.AddNewlyCreatedDBObject(acCirc, true);
               //ents.Add(acCirc.ObjectId);
               tr.Commit();
           }
       }




    }
}
