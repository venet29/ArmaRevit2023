
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
using planta_aux_C.Elemento_Losa;


namespace planta_aux_C.Elemento_Losa
{


    /// <summary>
    /// clase creada solo para dibuajr barra con dos puntos se agrego pq se necesitaba buscar si existia una
    /// losa contigua con distancia cero, ejempo las que exiten en pasillo tb se creo Clase 'LosaContigua' 
    /// donde se guardan los datos
    /// </summary>
    /// <param name="searchKey"></param>
    /// <returns></returns>

    public class BuscarPolyLosaContigua_NoStatic
    {

        public LosaContigua losaContigua;
        public LosaContigua LosaContigua_
        {
            get { return losaContigua; }
            set { losaContigua = value; }
        }



        public BuscarPolyLosaContigua_NoStatic()
        {
            LosaContigua_ = new LosaContigua();
        }

        #region comandas

        public bool BuscaSiExistePoligonoLosaContigua(ObjectId polilineaLosaActua, Point3d ptoRefe, string _direccion_LINEA)
        {
            // Get the current document editor
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            // Create a TypedValue array to define the filter criteria

            if (LosaContigua_ == null)
            { LosaContigua_ = new LosaContigua(); }


            TypedValue[] acTypValAr = new TypedValue[3];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.BlockName, "LOSA"), 1);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "0"), 2);
            // Assign the filter criteria to a SelectionFilter object
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            // Request for objects to be selected in the drawing area
            PromptSelectionResult acSSPrompt;
            acSSPrompt = acDocEd.SelectAll(acSelFtr);
            // If the prompt status is OK, objects were selected
            if (acSSPrompt.Status == PromptStatus.OK)
            {

                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Open the Block table for read
                    BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                    // Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec;//= acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    SelectionSet acSSet = acSSPrompt.Value;

                    foreach (ObjectId acObjId in acSSet.GetObjectIds())
                    {
                        if (acObjId == null || acObjId.IsErased == true) continue;
                        if (acObjId.ObjectClass.DxfName.ToString() == "INSERT")
                        {

                            // Open the Block table for read
                            BlockReference blkRef = acTrans.GetObject(acObjId, OpenMode.ForRead) as BlockReference;
                            // Open the Block table record Model space for write
                            BlockTableRecord btr = acTrans.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite) as BlockTableRecord;
                            btr.Dispose();
                            //               Dim blkRef As BlockReference = DirectCast(tr.GetObject(idObj_, OpenMode.ForWrite), BlockReference)
                            //Dim btr As BlockTableRecord = DirectCast(tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite), BlockTableRecord)
                            //btr.Dispose()


                            Point3d UbicacionPelotaLosa;
                            double AnguloPelotaLosa;
                            UbicacionPelotaLosa = blkRef.Position;
                            AnguloPelotaLosa = blkRef.Rotation;

                            if (_direccion_LINEA.Contains("vert")) AnguloPelotaLosa = AnguloPelotaLosa + Math.PI / 2;

                            //RutinasPolilinea refe_RutinasPolilinea = new RutinasPolilinea();
                            //refe_RutinasPolilinea.Zoom(new Point3d(), new Point3d(), UbicacionPelotaLosa, 1);

                            ObjectId[] acObjId_grup;
                            var GRUPO_ = new CODIGOS_GRUPOS();
                            acObjId_grup = GRUPO_.buscar_grupo(acObjId);

                            if (acObjId_grup == null || acObjId_grup.Length < 2) continue;

                            foreach (ObjectId id in acObjId_grup)
                            {
                                if (id.ObjectClass.DxfName.ToString() == "LWPOLYLINE" && polilineaLosaActua != id)
                                {
                                    //CREAR POLILINEA INTERNA (OFFSET 5 CM)
                                    Polyline Polilinea_ = acTrans.GetObject(id, OpenMode.ForRead) as Polyline;

                                    if (Polilinea_ != null && Polilinea_.NumberOfVertices > 2 && Polilinea_.Closed == true)
                                    {

                                        Point3dCollection isOn = Intersecciones.pto_interseccion_losa(Polilinea_, null, 60.0, 60.0, AnguloPelotaLosa, ptoRefe, false, false, Intersect.OnBothOperands);
                                        if (isOn != null & isOn.Count > 0)
                                        {
                                            foreach (Point3d ppr in isOn)
                                            {
                                                if (ptoRefe.DistanceTo(ppr) < 60)
                                                {
                                                    LosaContigua_.polilineaLosaActua = polilineaLosaActua;
                                                    LosaContigua_.polilineaLosaContigua = id;
                                                    LosaContigua_.diatancia = ptoRefe.DistanceTo(ppr);
                                                    LosaContigua_.ptoInterseccion_LosaContigua = ppr;
                                                    LosaContigua_.IsLosaContigua = true;
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                    //acTrans.Commit();
                }

            }
            else
            {
                AcApp.ShowAlertDialog("Number of objects selected: 0");
            }

            return false;
        }

        #endregion
    }


    public class LosaContigua
    {

        public ObjectId polilineaLosaActua { get; set; }
        public ObjectId polilineaLosaContigua { get; set; }
        public double diatancia { get; set; }
        public Point3d ptoInterseccion_LosaContigua { get; set; }

        public bool IsLosaContigua { get; set; }

        public Point3d NewPunto { get; set; }
        public double angle { get; set; }

        public LosaContigua()
        {
            IsLosaContigua = false;
        }

        public void CalcularNuevoPto()
        {
            if (IsLosaContigua)
            { 
            
                
                     

            }
        }

    }


}