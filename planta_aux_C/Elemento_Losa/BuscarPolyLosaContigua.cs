
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
    public class BuscarPolyLosaContigua
    {
        public tipoDeBarra tipoDeBarra_;
        public tipoDeBarra TipoDeBarra_
        {
            get { return tipoDeBarra_; }
            set { tipoDeBarra_ =value; }
        }
          
        public List<PtosCrearSuples> ListasSuples { get; set; }
        public static PtosCrearSuples SingleSuples { get; set; }
                
        #region comandas buscar esfera


        public static bool BuscaSiExistePoligonoLosaContigua(ObjectId polilineaLosaActua, Point3d ptoRefe, string _direccion_LINEA)
        {
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

                                        Point3dCollection isOn = Intersecciones.pto_interseccion_losa(Polilinea_, null, 60.0, 60.0, AnguloPelotaLosa, ptoRefe, false,false, Intersect.OnBothOperands);
                                        if (isOn != null & isOn.Count > 0)
                                        {
                                            foreach (Point3d ppr in isOn)
                                            {
                                                if (ptoRefe.DistanceTo(ppr) < 60)
                                                {

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

        public static bool BuscaSiExistePoligonoLosaContigua(ObjectId polilineaLosaActua, Point3d ptoRefe, string _direccion_LINEA, List<NH_RefereciaLosa> ListaPOlilineaYEsferaLosa, float distanciaParaBuscarLosaCOntigua_izq_abajo, float distanciaParaBuscarLosaCOntigua_dere_arriba, tipoDeBarra tipoDeBarra_)
        {

            // Get the current document editor
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            // RutinasPolilinea RutinasPolilinea_ = new RutinasPolilinea();


            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                bool result = false;
                foreach (NH_RefereciaLosa id in ListaPOlilineaYEsferaLosa)
                {
                    if (id.PoligonoLosa.ObjectClass.DxfName.ToString() == "LWPOLYLINE" && polilineaLosaActua != id.PoligonoLosa)
                    {

                     result= BuscaSiExistePoligonoLosaContigua_aux(polilineaLosaActua, ptoRefe, _direccion_LINEA, ListaPOlilineaYEsferaLosa, distanciaParaBuscarLosaCOntigua_izq_abajo, distanciaParaBuscarLosaCOntigua_dere_arriba, acTrans, id, tipoDeBarra_);
                        if (result) return true;
                    }
                }


                if (tipoDeBarra_ == tipoDeBarra.refuerzoInferior)
                {
                    result = BuscaSiExistePoligonoLosaContigua_aux(polilineaLosaActua, ptoRefe, _direccion_LINEA, ListaPOlilineaYEsferaLosa, distanciaParaBuscarLosaCOntigua_izq_abajo, distanciaParaBuscarLosaCOntigua_dere_arriba, acTrans, ListaPOlilineaYEsferaLosa.Where(c => c.PoligonoLosa == polilineaLosaActua).FirstOrDefault(), tipoDeBarra.refuerzoInferior_autoInterseccion);
                    if (result) return true;
                }

                acTrans.Commit();
            }



            return false;
        }

        //BuscaSiExistePoligonoLosaContigua_aux  -->  solo en esta clase, se utliza solo por  'BuscaSiExistePoligonoLosaContigua'
        public static bool BuscaSiExistePoligonoLosaContigua_aux(ObjectId polilineaLosaActua, Point3d ptoRefe, string _direccion_LINEA, List<NH_RefereciaLosa> ListaPOlilineaYEsferaLosa, float distanciaParaBuscarLosaCOntigua_izq_abajo, float distanciaParaBuscarLosaCOntigua_dere_arriba, Transaction acTrans, NH_RefereciaLosa id, tipoDeBarra tipoDeBarra_)
        {

            if (id == null) return false;

            Polyline Polilinea_ = acTrans.GetObject(id.PoligonoLosa, OpenMode.ForRead) as Polyline;

            if (Polilinea_ != null && Polilinea_.NumberOfVertices > 2 && Polilinea_.Closed == true)
            {
                // RutinasPolilinea_.Zoom_ENTITY(Polilinea_);
                double AnguloPelotaLosa = id.anguloPelotaLosa;
                if (_direccion_LINEA.Contains("vert")) AnguloPelotaLosa = AnguloPelotaLosa + Math.PI / 2;

                Point3dCollection isOn = Intersecciones.pto_interseccion_losa(Polilinea_, null, distanciaParaBuscarLosaCOntigua_izq_abajo, distanciaParaBuscarLosaCOntigua_dere_arriba, AnguloPelotaLosa, ptoRefe, false,false, Intersect.OnBothOperands);
                if (isOn != null & (isOn.Count == 1|| isOn.Count == 2 ))
                {
                    foreach (Point3d ppr in isOn)
                    {
                        if (ptoRefe.DistanceTo(ppr) < 70 )
                        {
                            if (isOn.Count == 2 && tipoDeBarra_ == tipoDeBarra.refuerzoInferior_autoInterseccion && ptoRefe.DistanceTo(ppr) < 10)  continue; // si hay 2 interseciones osea se interseca asi misma dos veces por lomenos tiene q tener un espesor de separacion
                            if (isOn.Count == 1 && tipoDeBarra_ == tipoDeBarra.refuerzoInferior_autoInterseccion ) return false; // solo se pemite dos intersciones con en consigomismo
                            if (isOn.Count == 1 && tipoDeBarra_ == tipoDeBarra.suple && ptoRefe.DistanceTo(ppr)< 6) return false; //seccion de suple en pasillo o pasada de puerta, no lo considera
                            //if (isOn.Count == 1 && tipoDeBarra_ == tipoDeBarra.refuerzoInferior && ptoRefe.DistanceTo(ppr) < 3) return false; // si hay 1 interseciones osea se interseca asi misma 1 veces esto pasas cuando dos poligonos de losa estan encontacto (ubicacion de puestas y pasillo), espaciamiento debe cer 0
                            //if (tipoDeBarra_ == tipoDeBarra.refuerzoInferior && ppr.DistanceTo(ptoRefe) < 10) return false; // caso mas normal , busca losa contigua y tiene q tener un espero mayor q 10

                            var aux_losaActual = ListaPOlilineaYEsferaLosa.Where(c => c.PoligonoLosa == polilineaLosaActua).FirstOrDefault();
                            if (aux_losaActual == null) return false;
                            // ptoRefe = pto donde se analiza el traslapo
                            //ppr = pto de poligono de losa contigua donde se produce la interseccion con suple



                            if (tipoDeBarra_ == tipoDeBarra.refuerzoInferior || tipoDeBarra_ == tipoDeBarra.refuerzoInferior_autoInterseccion) // caso refuerzo losa 
                            { SingleSuples = new PtosCrearSuples(ptoRefe, ppr, id.nombreLosa, aux_losaActual.nombreLosa); }
                            else if (Intersecciones.LINE_auxDibujada.EndPoint.DistanceTo(ppr) > Intersecciones.LINE_auxDibujada.StartPoint.DistanceTo(ppr)) // caso suple
                            { SingleSuples = new PtosCrearSuples(ptoRefe, Intersecciones.LINE_auxDibujada.StartPoint, aux_losaActual.nombreLosa, id.nombreLosa); }
                            else
                            { SingleSuples = new PtosCrearSuples(ptoRefe, Intersecciones.LINE_auxDibujada.EndPoint, aux_losaActual.nombreLosa,id.nombreLosa ); }//caso suple
                            return true;
                        }
                    }


                }
                else if (isOn != null & isOn.Count > 2 )
                {
                    AcApp.ShowAlertDialog("Numero de insterseciones de barra y polilinea de losa mayor programado: " + isOn.Count);
                }
            }

            return false;
        }
       
        
        
        
        #endregion


        




    }




}
