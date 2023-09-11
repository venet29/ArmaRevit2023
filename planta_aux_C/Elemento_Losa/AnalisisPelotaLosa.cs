using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO;
using Autodesk;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using deveAutoCad2018.VARIOS;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;
using Autodesk.Windows;
using planta_aux_C;

using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using VARIOS;
using planta_aux_C.Utiles;
using planta_aux_C.Elemento_Losa;
using planta_aux_C.enumera;
using planta_aux_C.Elementos;
using VARIOS_C;

namespace planta_aux_C.Elemento_Losa
{
    public class AnalisisPelotaLosa
    {
        public static List<NH_RefereciaLosaParaSuple> ListaPtos_Vertical_Suples { get; set; }
        public static List<NH_RefereciaLosaParaSuple> ListaPtos_Horizontal_Suples { get; set; }


        public static List<NH_RefereciaLosaParaBarra> ListaPtos_Vertical_Barra { get; set; }
        public static List<NH_RefereciaLosaParaBarra> ListaPtos_Horizontal_Barra { get; set; }


        //OBTIENE LOS PUNTO INTERNOS DE CADA TRAMO (PUNTO CENTRAL) DE UNA POLILINEA CERRADA, PARA PODER TRAZAR BARRA INTERNA
        public static List<Point3dCollection> ListaCoordenadasDibujarBarrasAutomatico(double largoToleranciaTramo, double porcentajeTramos, bool dibujar_pto_horizontal, bool dibujar_pto_vertical)
        {

            double factorGradoRadianes = Math.PI / 180;
            //porcentajeTramos: porcentaje (en el tramo) de la ubicacion del pto seleccionado del tramo
            Point3dCollection ListaPtos_Vertical = new Point3dCollection();
            Point3dCollection ListaPtos_Horizontal = new Point3dCollection();
            List<Point3dCollection> ListaBarras = new List<Point3dCollection>();

            ListaPtos_Vertical_Suples = new List<NH_RefereciaLosaParaSuple>();
            ListaPtos_Horizontal_Suples = new List<NH_RefereciaLosaParaSuple>();

            // Get the current document editor
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            // Create a TypedValue array to define the filter criteria
            TypedValue[] acTypValAr = new TypedValue[3];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.BlockName, "LOSA"), 1);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "0"), 2);
            // Assign the filter criteria to a SelectionFilter object
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            // Request for objects to be selected in the drawing area
            PromptSelectionResult acSSPrompt;

  
            acSSPrompt = acDocEd.GetSelection(acSelFtr);


            // If the prompt status is OK, objects were selected
            if (acSSPrompt.Status == PromptStatus.OK)
            {



                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                    // Open the Block table record Model space for write

                    SelectionSet acSSet1 ;
                

                    SelectionSet acSSet = acSSPrompt.Value;

                    foreach (ObjectId acObjId in acSSet.GetObjectIds())
                    {

                        if (acObjId.ObjectClass.DxfName.ToString() == "INSERT")
                        {

                            // Open the Block table for read
                            BlockReference blkRef = acTrans.GetObject(acObjId, OpenMode.ForRead) as BlockReference;
                            // Open the Block table record Model space for write
                            BlockTableRecord acBlkTblRec = acTrans.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite) as BlockTableRecord;

                            //               Dim blkRef As BlockReference = DirectCast(tr.GetObject(idObj_, OpenMode.ForWrite), BlockReference)
                            //Dim btr As BlockTableRecord = DirectCast(tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite), BlockTableRecord)
                            //btr.Dispose()
                            var tipo_losa = new string[10];
                            atributos atributo = new atributos();
                            atributo.obtener_atributos_c(acObjId, ref tipo_losa);

                            RutinasPolilinea RutinasPolilinea_ = new RutinasPolilinea();
                            Point3d UbicacionPelotaLosa;
                            Point2d UbicacionPelotaLosa2D;
                            float AnguloPelotaLosa;
                            UbicacionPelotaLosa = blkRef.Position;
                            UbicacionPelotaLosa2D= new Point2d(UbicacionPelotaLosa.X,UbicacionPelotaLosa.Y);
                            AnguloPelotaLosa = (float)comunes.coordenada__angulo_p1_p2_losa(blkRef.Rotation);


                            ObjectId[] acObjId_grup;
                            var GRUPO_ = new CODIGOS_GRUPOS();
                            acObjId_grup = GRUPO_.buscar_grupo(acObjId);

                            foreach (ObjectId id in acObjId_grup)
                            {
                                if (id.ObjectClass.DxfName.ToString() == "LWPOLYLINE")
                                {

                                    //CREAR POLILINEA INTERNA (OFFSET 5 CM)
                                    Polyline Polilinea_ = acTrans.GetObject(id, OpenMode.ForRead) as Polyline;
                                    var Polilinea_offset = Polilinea_.Offset(5, PolylineExtension.OffsetSide.In).ToList();
                                    Polilinea_.Dispose();

                                    //OBTIENE EL CENTROIDE DE POLILINES
                                    var aux_EjemplosPolilinea = new EjemplosPolilinea();
                                    Point2d centroid;
                                    centroid = aux_EjemplosPolilinea.Aux_PuntoCentroidePolyLinea(acCurDb, id);

                                    //trabajar en los segmentos
                                    Entity ename; Curve curv;
                                    //ename = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                                    ename = (Entity)Polilinea_offset[0];
                                    curv = ename as Curve;

                                    if (curv == null)
                                    {
                                        acDocEd.WriteMessage("\nCould not cast object as Curve.");
                                        return ListaBarras;
                                    }
                                    if (!curv.IsWriteEnabled)
                                        curv.UpgradeOpen();

                                    double startParam, endParam;

                                    // get the start param, usually it starts at 0 or 1
                                    startParam = curv.StartParam;
                                    acDocEd.WriteMessage("\nStartParam is: {0:f3}\n", startParam);

                                    // get the end param, for a polyline it's the total number of
                                    // vertex's -1
                                    endParam = curv.EndParam;
                                    acDocEd.WriteMessage("\nEndParam is: {0:f3}\n", endParam);

                                    // ITERA POR LOS TRAMOS DE LA POLILINEA
                                    var dimensiones_ = new dimensiones();
                                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                                    //BARRA VERTICAL
                                    for (double i = startParam; i < endParam; ++i)
                                    {
                                        // OBTIENE EL PTO MEDIO DEL SEGMENTO DE BARRA ANALIZADO
                                        Point3d pt;
                                        if (Math.Abs(curv.GetDistanceAtParameter(1 + i) - curv.GetDistanceAtParameter(i)) < largoToleranciaTramo) continue;
                                        pt = curv.GetPointAtParameter(i + porcentajeTramos);
                                        double anglue = dimensiones_.coordenada__angulo_p1_p2_DLL(curv.GetPointAtParameter(i), curv.GetPointAtParameter(i + 1), acDocEd, 0.0f);

                                        // 1.5707963267948966 =90°
                                        // 0.0174533          = 1°
                                        if (Math.Abs(anglue - (AnguloPelotaLosa + 90 * factorGradoRadianes)) > 0.0174533)
                                        {

                                            //var pt2D = new Position2d(pt.X, pt.Y);
                                            var valorAngle = comunes.angulo_entre_pt(UbicacionPelotaLosa,pt);
                                            var sin_Aux = Math.Sin(valorAngle);
                                            float v;
                                            if (float.TryParse(tipo_losa[5], out v))
                                            { v = Convert.ToSingle(tipo_losa[5]); }
                                            else
                                              v = 0;

                                            if (sin_Aux > 0)
                                            { ListaPtos_Vertical_Suples.Add(new NH_RefereciaLosaParaSuple(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt, UbicacionLosa.Superior, acObjId, id)); }
                                            else
                                            { ListaPtos_Vertical_Suples.Add(new NH_RefereciaLosaParaSuple(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt, UbicacionLosa.Inferior, acObjId, id)); } 
                                                                                                                                       
                                            ListaPtos_Vertical.Add(pt);

                                            if (dibujar_pto_vertical)
                                            {
                                                //  DIBUJAR PTO EN EL PTO MEDIO DE SEGMENTO DE LA TRAZO DE LA POLILINEA
                                                Circle cr = new Circle(pt, Vector3d.ZAxis, 2.5);
                                                cr.Layer = "NHREVISION";
                                                acBlkTblRec.AppendEntity(cr);
                                                acTrans.AddNewlyCreatedDBObject(cr, true);
                                                acDocEd.WriteMessage("\nPoint: {0}, {1}, {2}\n", pt[0], pt[1], pt[2]);
                                            }
                                        }

                                    }

                                    //BARRA HORIZONTAL
                                    for (double i = startParam; i < endParam; ++i)
                                    {
                                        // OBTIENE EL PTO MEDIO DEL SEGMENTO DE BARRA ANALIZADO
                                        Point3d pt;
                                        if (Math.Abs(curv.GetDistanceAtParameter(1 + i) - curv.GetDistanceAtParameter(i)) < largoToleranciaTramo) continue;
                                        pt = curv.GetPointAtParameter(i + porcentajeTramos);
                                        double anglue = dimensiones_.coordenada__angulo_p1_p2_DLL(curv.GetPointAtParameter(i), curv.GetPointAtParameter(i + 1), acDocEd, 0.0f);

                                        // 1.5707963267948966 =90°
                                        // 0.0174533          = 1°

                                        if (Math.Abs(anglue - AnguloPelotaLosa) > 0.0174533)
                                        {

                                            var valorAngle = comunes.angulo_entre_pt(UbicacionPelotaLosa, pt);
                                            var cos_Aux = Math.Cos(valorAngle);

                                            float v;
                                            if (float.TryParse(tipo_losa[5], out v))
                                            { v = Convert.ToSingle(tipo_losa[5]); }
                                            else
                                              v = 0;


                                            if (cos_Aux > 0)
                                            { ListaPtos_Horizontal_Suples.Add(new NH_RefereciaLosaParaSuple(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt, UbicacionLosa.Derecha, acObjId, id)); }
                                            else
                                            { ListaPtos_Horizontal_Suples.Add(new NH_RefereciaLosaParaSuple(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt, UbicacionLosa.Izquierda, acObjId, id)); }
                                            
                                            ListaPtos_Horizontal.Add(pt);

                                            if (dibujar_pto_horizontal)
                                            {
                                                //  DIBUJAR PTO EN EL PTO MEDIO DE SEGMENTO DE LA TRAZO DE LA POLILINEA
                                                Circle cr = new Circle(pt, Vector3d.ZAxis, 2.5);
                                                cr.Layer = "NHREVISION";
                                                acBlkTblRec.AppendEntity(cr);
                                                acTrans.AddNewlyCreatedDBObject(cr, true);
                                                acDocEd.WriteMessage("\nPoint: {0}, {1}\n", pt[0], pt[1], pt[2]);
                                            }
                                        }
                                    }



                                }
                            }

                        }
                    }

                    acTrans.Commit();
                }

            }
            else
            {
                AcApp.ShowAlertDialog("Number of objects selected: 0");
            }

            ListaBarras.Add(ListaPtos_Vertical);
            ListaBarras.Add(ListaPtos_Horizontal);

            return ListaBarras;
        }

        //OBTIENE LOS PUNTO INTERNOS DE CADA TRAMO (PUNTO CENTRAL) DE UNA POLILINEA CERRADA, PARA PODER TRAZAR BARRA INTERNA
        public static void ListaCoordenadasDibujarBarrasAutomatico(double largoToleranciaTramo, float porcentajeTramosBarraInferior, float porcentajeTramosBarraSuple,
                                                                    bool dibujar_pto_horizontal, bool dibujar_pto_vertical, List<NH_RefereciaLosa> ListaPOlilineaYEsferaLosa)
        {

            double factorGradoRadianes = Math.PI / 180;
            //porcentajeTramos: porcentaje (en el tramo) de la ubicacion del pto seleccionado del tramo
            //Point3dCollection ListaPtos_Vertical = new Point3dCollection();
            //Point3dCollection ListaPtos_Horizontal = new Point3dCollection();


            ListaPtos_Vertical_Barra = new List<NH_RefereciaLosaParaBarra>();
            ListaPtos_Horizontal_Barra = new List<NH_RefereciaLosaParaBarra>();

            ListaPtos_Vertical_Suples = new List<NH_RefereciaLosaParaSuple>();
            ListaPtos_Horizontal_Suples = new List<NH_RefereciaLosaParaSuple>();


            // Get the current document editor
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            // Create a TypedValue array to define the filter criteria

                 using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                    // Open the Block table record Model space for write

 

                    foreach (NH_RefereciaLosa NH_RefereciaLosa_ in ListaPOlilineaYEsferaLosa)
                    {
                        var acObjId =NH_RefereciaLosa_.PelotaLosa;

                        if (acObjId.ObjectClass.DxfName.ToString() == "INSERT")
                        {

                            // Open the Block table for read
                            BlockReference blkRef = acTrans.GetObject(acObjId, OpenMode.ForRead) as BlockReference;
                            // Open the Block table record Model space for write
                            BlockTableRecord acBlkTblRec = acTrans.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite) as BlockTableRecord;

                            //               Dim blkRef As BlockReference = DirectCast(tr.GetObject(idObj_, OpenMode.ForWrite), BlockReference)
                            //Dim btr As BlockTableRecord = DirectCast(tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite), BlockTableRecord)
                            //btr.Dispose()
                            var tipo_losa = new string[10];
                            atributos atributo = new atributos();
                            atributo.obtener_atributos_c(acObjId, ref tipo_losa);

                            RutinasPolilinea RutinasPolilinea_ = new RutinasPolilinea();
                            Point3d UbicacionPelotaLosa;
                            Point2d UbicacionPelotaLosa2D;
                            float AnguloPelotaLosa;
                            UbicacionPelotaLosa = blkRef.Position;
                            UbicacionPelotaLosa2D = new Point2d(UbicacionPelotaLosa.X, UbicacionPelotaLosa.Y);
                            AnguloPelotaLosa = (float)comunes.coordenada__angulo_p1_p2_losa(blkRef.Rotation);


                            ObjectId[] acObjId_grup;
                            var GRUPO_ = new CODIGOS_GRUPOS();
                            acObjId_grup = GRUPO_.buscar_grupo(acObjId);

                            foreach (ObjectId id in acObjId_grup)
                            {
                                if (id.ObjectClass.DxfName.ToString() == "LWPOLYLINE")
                                {

                                    //CREAR POLILINEA INTERNA (OFFSET 5 CM)
                                    Polyline Polilinea_ = acTrans.GetObject(id, OpenMode.ForRead) as Polyline;
                                    var Polilinea_offset = Polilinea_.Offset(5, PolylineExtension.OffsetSide.In).ToList();
                                    Polilinea_.Dispose();

                                    //OBTIENE EL CENTROIDE DE POLILINES
                                    var aux_EjemplosPolilinea = new EjemplosPolilinea();
                                    Point2d centroid;
                                    centroid = aux_EjemplosPolilinea.Aux_PuntoCentroidePolyLinea(acCurDb, id);

                                    //trabajar en los segmentos
                                    Entity ename; Curve curv;
                                    //ename = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                                    ename = (Entity)Polilinea_offset[0];
                                    curv = ename as Curve;

                                    if (curv == null)
                                    {
                                        acDocEd.WriteMessage("\nCould not cast object as Curve.");
                                        return ;
                                    }
                                    if (!curv.IsWriteEnabled)
                                        curv.UpgradeOpen();

                                    double startParam, endParam;

                                    // get the start param, usually it starts at 0 or 1
                                    startParam = curv.StartParam;
                                    acDocEd.WriteMessage("\nStartParam is: {0:f3}\n", startParam);

                                    // get the end param, for a polyline it's the total number of
                                    // vertex's -1
                                    endParam = curv.EndParam;
                                    acDocEd.WriteMessage("\nEndParam is: {0:f3}\n", endParam);

                                    // ITERA POR LOS TRAMOS DE LA POLILINEA
                                    var dimensiones_ = new dimensiones();
                                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                                    //BARRA VERTICAL
                                    for (double i = startParam; i < endParam; ++i)
                                    {
                                        // OBTIENE EL PTO MEDIO DEL SEGMENTO DE BARRA ANALIZADO
                                        Point3d pt_barra;
                                        Point3d pt_Suple;
                                        if (Math.Abs(curv.GetDistanceAtParameter(1 + i) - curv.GetDistanceAtParameter(i)) < largoToleranciaTramo) continue;
                                        pt_barra = curv.GetPointAtParameter(i + porcentajeTramosBarraInferior);
                                        pt_Suple = curv.GetPointAtParameter(i + porcentajeTramosBarraSuple);
                                        double anglue = dimensiones_.coordenada__angulo_p1_p2_DLL(curv.GetPointAtParameter(i), curv.GetPointAtParameter(i + 1), acDocEd, 0.0f);

                                        // 1.5707963267948966 =90°
                                        // 0.0174533          = 1°
                                        if (Math.Abs(anglue - (AnguloPelotaLosa + 90 * factorGradoRadianes)) > 0.0174533)
                                        {

                                            //var pt2D = new Position2d(pt.X, pt.Y);
                                            var valorAngle = comunes.angulo_entre_pt(UbicacionPelotaLosa, pt_barra);
                                            var sin_Aux = Math.Sin(valorAngle);
                                            float v;
                                            if (float.TryParse(tipo_losa[5], out v))
                                            { v = Convert.ToSingle(tipo_losa[5]); }
                                            else
                                                v = 0;
                                            //BARRA INFERIOR VERTICAL
                                            NH_RefereciaLosaParaBarra aux_ListaPtos_Vertical_Barra = null;
                                            if (sin_Aux > 0)
                                            { aux_ListaPtos_Vertical_Barra = new NH_RefereciaLosaParaBarra(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt_barra, UbicacionLosa.Superior, acObjId, id); }
                                            else
                                            { aux_ListaPtos_Vertical_Barra = new NH_RefereciaLosaParaBarra(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt_barra, UbicacionLosa.Inferior, acObjId, id); }
                                           
                                            ListaPtos_Vertical_Barra.Add(aux_ListaPtos_Vertical_Barra);
                                            NH_RefereciaLosa_.ListaPtos_Vertical_Barra.Add(aux_ListaPtos_Vertical_Barra);

                                            //BARRA SUPLE VERTICAL
                                            NH_RefereciaLosaParaSuple aux_ListaPtos_Vertical_Suple = null;
                                            if (sin_Aux > 0)
                                            { aux_ListaPtos_Vertical_Suple = new NH_RefereciaLosaParaSuple(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt_Suple, UbicacionLosa.Superior, acObjId, id); }
                                            else
                                            { aux_ListaPtos_Vertical_Suple = new NH_RefereciaLosaParaSuple(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt_Suple, UbicacionLosa.Inferior, acObjId, id); }

                                            ListaPtos_Vertical_Suples.Add(aux_ListaPtos_Vertical_Suple);
                                            NH_RefereciaLosa_.ListaPtos_Vertical_Suple.Add(aux_ListaPtos_Vertical_Suple);

                                


                                            if (dibujar_pto_vertical)
                                            {
                                                //  DIBUJAR PTO EN EL PTO MEDIO DE SEGMENTO DE LA TRAZO DE LA POLILINEA
                                                Circle cr = new Circle(pt_barra, Vector3d.ZAxis, 2.5);
                                                cr.Layer = "NHREVISION";
                                                acBlkTblRec.AppendEntity(cr);
                                                acTrans.AddNewlyCreatedDBObject(cr, true);
                                                acDocEd.WriteMessage("\nPoint: {0}, {1}, {2}\n", pt_barra[0], pt_barra[1], pt_barra[2]);
                                            }
                                        }

                                    }

                                    //BARRA HORIZONTAL
                                    for (double i = startParam; i < endParam; ++i)
                                    {
                                        // OBTIENE EL PTO MEDIO DEL SEGMENTO DE BARRA ANALIZADO
                                        Point3d pt_barra;
                                        Point3d pt_Suple;
                                        if (Math.Abs(curv.GetDistanceAtParameter(1 + i) - curv.GetDistanceAtParameter(i)) < largoToleranciaTramo) continue;
                                        pt_barra = curv.GetPointAtParameter(i + porcentajeTramosBarraInferior);
                                        pt_Suple = curv.GetPointAtParameter(i + porcentajeTramosBarraSuple);
                                        double anglue = dimensiones_.coordenada__angulo_p1_p2_DLL(curv.GetPointAtParameter(i), curv.GetPointAtParameter(i + 1), acDocEd, 0.0f);

                                        // 1.5707963267948966 =90°
                                        // 0.0174533          = 1°

                                        if (Math.Abs(anglue - AnguloPelotaLosa) > 0.0174533)
                                        {

                                            var valorAngle = comunes.angulo_entre_pt(UbicacionPelotaLosa, pt_barra);
                                            var cos_Aux = Math.Cos(valorAngle);

                                            float v;
                                            if (float.TryParse(tipo_losa[5], out v))
                                            { v = Convert.ToSingle(tipo_losa[5]); }
                                            else
                                                v = 0;

                                            // //BARRA INFERIOR HORIZONTAL
                                            NH_RefereciaLosaParaBarra aux_ListaPtos_Horizontal_Barra = null;
                                            if (cos_Aux > 0)
                                            { aux_ListaPtos_Horizontal_Barra = new NH_RefereciaLosaParaBarra(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt_barra, UbicacionLosa.Derecha, acObjId, id); }
                                            else
                                            { aux_ListaPtos_Horizontal_Barra = new NH_RefereciaLosaParaBarra(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt_barra, UbicacionLosa.Izquierda, acObjId, id); }

                                            ListaPtos_Horizontal_Barra.Add(aux_ListaPtos_Horizontal_Barra);
                                            NH_RefereciaLosa_.ListaPtos_Horizontal_Barra.Add(aux_ListaPtos_Horizontal_Barra);

                                            //BARRA SUPLE VERTICAL
                                            NH_RefereciaLosaParaSuple aux_ListaPtos_Horizontal_Suple = null;
                                            if (cos_Aux > 0)
                                            { aux_ListaPtos_Horizontal_Suple = new NH_RefereciaLosaParaSuple(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt_Suple, UbicacionLosa.Derecha, acObjId, id); }
                                            else
                                            { aux_ListaPtos_Horizontal_Suple = new NH_RefereciaLosaParaSuple(tipo_losa[0], v, AnguloPelotaLosa, UbicacionPelotaLosa, pt_Suple, UbicacionLosa.Izquierda, acObjId, id); }

                                            ListaPtos_Horizontal_Suples.Add(aux_ListaPtos_Horizontal_Suple);
                                            NH_RefereciaLosa_.ListaPtos_Horizontal_Suple.Add(aux_ListaPtos_Horizontal_Suple);


                                            if (dibujar_pto_horizontal)
                                            {
                                                //  DIBUJAR PTO EN EL PTO MEDIO DE SEGMENTO DE LA TRAZO DE LA POLILINEA
                                                Circle cr = new Circle(pt_barra, Vector3d.ZAxis, 2.5);
                                                cr.Layer = "NHREVISION";
                                                acBlkTblRec.AppendEntity(cr);
                                                acTrans.AddNewlyCreatedDBObject(cr, true);
                                                acDocEd.WriteMessage("\nPoint: {0}, {1}\n", pt_barra[0], pt_barra[1], pt_barra[2]);
                                            }
                                        }
                                    }



                                }
                            }

                        }
                    }

                    acTrans.Commit();
                }

     


        }



        //OBTIENE  datos interno- poligono
        // 1)ptos del poligono de losa
        // 2)nombre y datos de losa
        // 2.1) direcciones procipales
        // 3) largo y coordenadas de largo mayor
        // 4) largo y coordenadas de largo mejor
        public static List<NH_RefereciaLosa> ListaDatosPoligonoLosa(string tipoSelecion, float largoIzquierdo, float largoDerecho, bool isMostraPuntoInterseccion)
        {
            List<NH_RefereciaLosa> ListaPOlilineaYEsferaLosa = new List<NH_RefereciaLosa>();
            
            //porcentajeTramos: porcentaje (en el tramo) de la ubicacion del pto seleccionado del tramo

            // Get the current document editor
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            // Open the Block table record Model space for write
            BlockTableRecord acBlkTblRec;//= acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
            // Create a TypedValue array to define the filter criteria
            TypedValue[] acTypValAr = new TypedValue[3];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.BlockName, "LOSA"), 1);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "0"), 2);
            // Assign the filter criteria to a SelectionFilter object
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            // Request for objects to be selected in the drawing area
            PromptSelectionResult acSSPrompt;
            if (tipoSelecion == "SelectAll")
            {
                acSSPrompt = acDocEd.SelectAll(acSelFtr);
            }
            else
            {
                acSSPrompt = acDocEd.GetSelection(acSelFtr);
            }


            // If the prompt status is OK, objects were selected
            if (acSSPrompt.Status == PromptStatus.OK)
            {



                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Open the Block table for read
                    //BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                    // Open the Block table record Model space for write
                    SelectionSet acSSet = acSSPrompt.Value;
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                    foreach (ObjectId acObjId in acSSet.GetObjectIds())
                    {


                        if (acObjId.ObjectClass.DxfName.ToString() == "INSERT")
                        {
                            if (acObjId == null) continue;
                            // Open the Block table for read
                            BlockReference blkRef = acTrans.GetObject(acObjId, OpenMode.ForRead) as BlockReference;

                            // Open the Block table record Model space for write
                            //BlockTableRecord btr = acTrans.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite) as BlockTableRecord;

                            //               Dim blkRef As BlockReference = DirectCast(tr.GetObject(idObj_, OpenMode.ForWrite), BlockReference)
                            //Dim btr As BlockTableRecord = DirectCast(tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite), BlockTableRecord)
                            //btr.Dispose()

                            var tipo_losa = new string[10];
                            atributos atributo = new atributos();
                            atributo.obtener_atributos_c(acObjId, ref tipo_losa);


                            RutinasPolilinea RutinasPolilinea_ = new RutinasPolilinea();
                            Point3d UbicacionPelotaLosa;
                            float AnguloPelotaLosa;
                            UbicacionPelotaLosa = blkRef.Position;
                            AnguloPelotaLosa = (float)comunes.coordenada__angulo_p1_p2_losa(blkRef.Rotation);

                            ObjectId[] acObjId_grup;
                            var GRUPO_ = new CODIGOS_GRUPOS();
                            acObjId_grup = GRUPO_.buscar_grupo(acObjId);

                            if (acObjId_grup== null) continue ;
                            if(acObjId_grup.Length<=1) continue;

                             acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                            foreach (ObjectId id in acObjId_grup)
                            {
                                if (id.ObjectClass.DxfName.ToString() == "LWPOLYLINE")
                                {

                                   
                                    //CREAR POLILINEA INTERNA (OFFSET 5 CM)
                                    Polyline Polilinea_ = acTrans.GetObject(id, OpenMode.ForRead) as Polyline;

                                    if (Polilinea_ != null && Polilinea_.NumberOfVertices > 2 && Polilinea_.Closed == true)
                                    {
                                        var newRegistroLosa = new NH_RefereciaLosa();
                                        //lista con los vertices 
                                        newRegistroLosa.ListaVerticesPoligonoLosa = RutinasPolilinea.ListVerticesPolinea(id);
                                        newRegistroLosa.PoligonoLosa = id;
                                        newRegistroLosa.PelotaLosa = acObjId;
                                        newRegistroLosa.anguloPelotaLosa = AnguloPelotaLosa;
                                        newRegistroLosa.nombreLosa = tipo_losa[0];

                                        
                                        newRegistroLosa.posicionPelota = new Point2d_nh(UbicacionPelotaLosa.X, UbicacionPelotaLosa.Y);
                                        int v;
                                        if (Int32.TryParse(tipo_losa[5], out v))
                                        { newRegistroLosa.espesor = Convert.ToInt32(tipo_losa[5]); }
                                        else
                                            newRegistroLosa.espesor = 0;
                                        newRegistroLosa.direccionHorizontal = tipo_losa[3];
                                        newRegistroLosa.direccionVertical = tipo_losa[4];
                                        newRegistroLosa.cuantiaHorizontal = tipo_losa[1];
                                        newRegistroLosa.cuantiaVertical = tipo_losa[2];



                                        Entity Polilinea_2 = acTrans.GetObject(id, OpenMode.ForRead) as Entity;

                                        //interseccion de  proyectando linea en la direccion delosa
                                        newRegistroLosa.ListaPtoLineaDireccionLosa.Clear();
                                        Point3dCollection isOn = Intersecciones.pto_interseccion_losa(Polilinea_2, null, largoIzquierdo, largoDerecho, AnguloPelotaLosa, UbicacionPelotaLosa, false, isMostraPuntoInterseccion, Intersect.ExtendThis);
                                        if (isOn.Count == 2)
                                        {
                                            foreach (Point3d pto in isOn)
                                            {
                                                if (isMostraPuntoInterseccion)
                                                {

                                                    //  DIBUJAR PTO EN EL PTO MEDIO DE SEGMENTO DE LA TRAZO DE LA POLILINEA
                                                    Circle cr = new Circle(pto, Vector3d.ZAxis, 2.5);
                                                    cr.Layer = "NHREVISION";
                                                    acBlkTblRec.AppendEntity(cr);
                                                    acTrans.AddNewlyCreatedDBObject(cr, true);
                                                    acDocEd.WriteMessage("\nPoint: {0}, {1}\n", pto.X, pto.Y);

                                                }
                                                newRegistroLosa.ListaPtoLineaDireccionLosa.Add(new Point2d(pto.X, pto.Y));
                                            }
                                            newRegistroLosa.LargoHorizontal = Convert.ToSingle(newRegistroLosa.ListaPtoLineaDireccionLosa[0].GetDistanceTo(newRegistroLosa.ListaPtoLineaDireccionLosa[1]));



                                        }
                                        else
                                        {
                                            //AcApp.ShowAlertDialog("En Losa " + newRegistroLosa.nombreLosa.ToString() + " no se puedo calcular largo minimo en direccion de la losa");
                                            acDocEd.WriteMessage("En Losa " + newRegistroLosa.nombreLosa.ToString() + " no se puedo calcular largo minimo en direccion de la losa");
                                        }
                                        //interseccion de  proyectando linea en la direccion perpendicular losa de losa
                                        newRegistroLosa.ListaPtoLineaDireccionPerpLosa.Clear();
                                        Point3dCollection isOn2 = Intersecciones.pto_interseccion_losa(Polilinea_2, null, largoIzquierdo, largoDerecho, AnguloPelotaLosa + Math.PI / 2, UbicacionPelotaLosa, false, isMostraPuntoInterseccion, Intersect.ExtendThis);
                                        if (isOn2.Count == 2)
                                        {
                                            foreach (Point3d pto in isOn2)
                                            {
                                                if (isMostraPuntoInterseccion)
                                                {
                                                    //  DIBUJAR PTO EN EL PTO MEDIO DE SEGMENTO DE LA TRAZO DE LA POLILINEA
                                                    Circle cr = new Circle(pto, Vector3d.ZAxis, 2.5);
                                                    cr.Layer = "NHREVISION";
                                                    acBlkTblRec.AppendEntity(cr);
                                                    acTrans.AddNewlyCreatedDBObject(cr, true);
                                                    acDocEd.WriteMessage("\nPoint: {0}, {1}\n", pto.X, pto.Y);

                                                }
                                                newRegistroLosa.ListaPtoLineaDireccionPerpLosa.Add(new Point2d(pto.X, pto.Y));
                                            }

                                            newRegistroLosa.LargoVertical = Convert.ToSingle(newRegistroLosa.ListaPtoLineaDireccionPerpLosa[0].GetDistanceTo(newRegistroLosa.ListaPtoLineaDireccionPerpLosa[1]));

                                        }
                                        else
                                        {
                                            //AcApp.ShowAlertDialog("En Losa " + newRegistroLosa.nombreLosa.ToString() + " no se puedo calcular largo minimo en direccion perpendicular de la losa");
                                            acDocEd.WriteMessage("En Losa " + newRegistroLosa.nombreLosa.ToString() + " no se puedo calcular largo minimo en direccion de la losa");
                                        }

                                        ListaPOlilineaYEsferaLosa.Add(newRegistroLosa);
                                    }

                                }
                            }

                        }
                    }

                    acTrans.Commit();
                }

            }
            else
            {
                AcApp.ShowAlertDialog("Number of objects selected: 0");
            }

            return ListaPOlilineaYEsferaLosa;
        }

        

    }
}
