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
using planta_aux_C.Rutinas;
using ESTRIBOS_MUROS;
using VARIOS_C.Datos;
using VARIOS_C;

[assembly: CommandClass(typeof(planta_aux_C.CargarRutinas.CrearRefPoligonoLosa))]

namespace planta_aux_C.CargarRutinas
{

    public class CrearRefPoligonoLosa
    {
        //          [CommandMethod("Prueba_CrearREferenciaLosa")]
        public void aux_Ref()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            comunes.Create_ALayer("REF_POLIGONO", 225);
            //---------------------------------------------------------------------------
            while (true)
            {


                PromptPointResult ppr = acDocEd.GetPoint("\n1)Select a point1");
                if (ppr.Status != PromptStatus.OK) return;

                PromptPointResult ppr2 = acDocEd.GetPoint("\n2)Select a point2");
                if (ppr2.Status != PromptStatus.OK) return;



                Point3d pto1 = ppr.Value;
                Point3d pto2 = ppr2.Value;
                //------------------------------------------------------------------------

                List<Point3d> list_pto_interseccion = new List<Point3d>();
                List<NH_PolilineaRef> list_pto_Rutinas_polilineas = new List<NH_PolilineaRef>();

                CODIGOS_DATOS_C codigo_varios_c = new CODIGOS_DATOS_C();

                //--------------------------------------------------------------------------
                List<ObjectId> lista_lineas_polineas = SelecionadoElementos(pto1, pto2, 0);


                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {

                    var salirSegundoFor = false;
                    foreach (var item in lista_lineas_polineas)
                    {
                        Point3dCollection isOn = Intersecciones.pto_interseccion_losa(item, pto1, pto2, false, true, Intersect.OnBothOperands);
                        foreach (Point3d pto in isOn)
                        {
                            bool aux_seguir = true;
                            foreach (var ptosGuardados in list_pto_interseccion)
                            {
                                // si el putnos est a menos de 30 cm no lo considera( esto para putnos cercanos y por si se repiten)
                                if (pto.DistanceTo(ptosGuardados) < 30)
                                {
                                    aux_seguir = false;
                                    break;
                                }
                            }

                            if (aux_seguir)
                            {
                                list_pto_interseccion.Add(pto);
                                Entity Polilinea_2 = acTrans.GetObject(item, OpenMode.ForRead) as Entity;
                                var aux_ = Rutinas_Polilineas.Buscar_Inicio_Fin_Poly(acDocEd, Polilinea_2, pto);
                                if (aux_ != null) list_pto_Rutinas_polilineas.Add(aux_);
                                if (list_pto_Rutinas_polilineas.Count == 2) salirSegundoFor = true; break;
                            }
                        }
                        if (salirSegundoFor) break;
                    }

                }


                // DIBUJAR  SI ENCUENTRA 2 INTERSECCIONES

                if (list_pto_Rutinas_polilineas.Count == 2)
                {
                    //reordenado los puntos para que los estribos u las barras se dibujen asobre un area cuadrada
                    ComunesLosa.BuscarAcortarLineasSiNoSonIgual(ref list_pto_Rutinas_polilineas);

                    NH_ref__Barra NH_ref__Barra_ = new NH_ref__Barra();
                    NH_ref__Barra_.st_layer = "REF_POLIGONO";



                    NH_ref__Barra_.rellenarListaPuntos(list_pto_Rutinas_polilineas);
                    NH_ref__Barra_.dibujar_barra_pl(true);
                }
            }
        }


        // revisar barras
        private List<ObjectId> SelecionadoElementos(Point3d pt_ini, Point3d pt_fin, double ANGLE)
        {

            // Crear una matriz de objetos TypedValue para definir el criterio de seleccion

            var osmode_ini = AcApp.GetSystemVariable("PICKSTYLE");
            Application.SetSystemVariable("PICKSTYLE", 0);

            // Get the current document editor
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            // Create a TypedValue array to define the filter criteria

            ANGLE = comunes.coordenada__angulo_p1_p2_losa(pt_ini, pt_fin);
            Point3dCollection values_ = new Point3dCollection();
            values_.Add(new Point3d(pt_ini.X - Math.Cos(ANGLE) * -60, pt_ini.Y - Math.Sin(ANGLE) * -60, 0));
            values_.Add(new Point3d(pt_ini.X - Math.Cos(ANGLE) * 60, pt_ini.Y - Math.Sin(ANGLE) * 60, 0));

            values_.Add(new Point3d(pt_fin.X + Math.Cos(ANGLE) * 60, pt_fin.Y + Math.Sin(ANGLE) * 60, 0));
            values_.Add(new Point3d(pt_fin.X + Math.Cos(ANGLE) * -60, pt_fin.Y + Math.Sin(ANGLE) * -60, 0));




            TypedValue[] acTypValAr = new TypedValue[22];

            // -----------------------------------
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "<or"), 0);

            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "<and"), 1);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "LWPOLYLINE"), 2);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "<or"), 3);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "REF LOSA"), 4);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "SHAFT"), 5);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "MUROS"), 6);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "VIGAS"), 7);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "ARRANQUES"), 8);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "or>"), 9);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "and>"), 10);

            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "<and"), 11);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 12);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "<or"), 13);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "REF LOSA"), 14);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "SHAFT"), 15);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "MUROS"), 16);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "VIGAS"), 17);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "ARRANQUES"), 18);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "or>"), 19);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "and>"), 20);

            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "or>"), 21);

            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            PromptSelectionResult acSSPrompt;

            if ((pt_ini == new Point3d(0, 0, 0)) && (pt_ini == new Point3d(0, 0, 0)))
            {
                acSSPrompt = acDocEd.SelectAll(acSelFtr);
            }
            else
            {
                acSSPrompt = acDocEd.SelectFence(values_, acSelFtr);
            }

            List<ObjectId> lista = new List<ObjectId>();
            // If the prompt status is OK, objects were selected
            if (acSSPrompt.Status == PromptStatus.OK)
            {
                SelectionSet acSSet = acSSPrompt.Value;
                lista = acSSet.GetObjectIds().ToList();
            }

            Application.SetSystemVariable("PICKSTYLE", osmode_ini);
            return lista;
        }
    }
}
