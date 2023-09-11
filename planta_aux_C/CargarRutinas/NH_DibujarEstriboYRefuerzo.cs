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
using planta_aux_C.Elementos.Refuerzo;

namespace planta_aux_C.CargarRutinas
{
   public class NH_DibujarEstriboYRefuerzo
    {


       public void aux_RL(int diametro_ref, int cantidadBarras_ref, int espaciamiento_ref, bool capa_inferior,
                        string Cantidad_Estr, int diametro_Estr,int espaciamiento_Estr)
       {
           Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
           Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
           Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;

           //---------------------------------------------------------------------------


           while (true)
           {                      

           PromptPointResult ppr = acDocEd.GetPoint("\n1)Select a point1");
           if (ppr.Status != PromptStatus.OK) return;

           PromptPointResult ppr2 = acDocEd.GetPoint("\n2)Select a point2");
           if (ppr2.Status != PromptStatus.OK) return;

           string _direccion_LINEA = "horizontal_i";


           if (Math.Abs(ppr.Value.X - ppr2.Value.X) < 1)
           { _direccion_LINEA = "horizontal_i"; }
           else
           { _direccion_LINEA = "vertical_b"; }


         //  var TipoSeleccionMouse_ = TipoDeSeleccion_direccion(ppr.Value, ppr2.Value);
                 
           //NH_DibujarEstriboYRefuerzo NH_Refuerzo_ = new NH_DibujarEstriboYRefuerzo();

         Dibujar_refuerzo_losa_C(ppr.Value,
                                                ppr2.Value,
                                                diametro_ref: diametro_ref,
                                                cantidadBarras_ref: cantidadBarras_ref,
                                                espaciamiento_ref: espaciamiento_ref,
                                                capa_inferior: capa_inferior,
                                                Cantidad_Estr: Cantidad_Estr,
                                                diametro_Estr: diametro_Estr,
                                                espaciamiento_Estr: espaciamiento_Estr,
                                                _direccion_LINEA: _direccion_LINEA,
                                                TipoPosicionTexto_: TipoPosicionTexto.ArribayBajo);

           }
       }

       private TipoSeleccionMouse TipoDeSeleccion_direccion(Point3d p1, Point3d p2)
       {
           TipoSeleccionMouse result = TipoSeleccionMouse.HaciaArriba;

           if (p2.Y == p1.Y)// vertical
           {
               if (p2.Y > p1.Y)
               { result = TipoSeleccionMouse.HaciaArriba; }//hacia Arriba 
               else
               { result = TipoSeleccionMouse.HaciaBajo; }//haciaBajo
           }
           else// horizontal
           {
                        if (p2.X > p1.X)
                        { result = TipoSeleccionMouse.HaciaDerecha; }//hacia derecha 
               else
                        { result = TipoSeleccionMouse.HaciaIzquierda; }//hacia iz
           }

           return result;
       }



       public void Dibujar_refuerzo_losa_C(Point3d pto1, Point3d pto2,
                                            int diametro_ref, int cantidadBarras_ref, int espaciamiento_ref, bool capa_inferior,
                                            string Cantidad_Estr, int diametro_Estr, int espaciamiento_Estr,  string _direccion_LINEA,
                                            TipoPosicionTexto TipoPosicionTexto_)
        {

            string grupo_referencia = "*A2867";
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            //------------------------------------------------------------------------

            List<Point3d> list_pto_interseccion = new List<Point3d>();
            List<NH_PolilineaRef> list_pto_Rutinas_polilineas = new List<NH_PolilineaRef>();
            comunes.Create_ALayer("CESTRIBO_E");
            comunes.Create_ALayer("CESTRIBO_L");
            comunes.Create_ALayer("ESTRIBO");
            CODIGOS_DATOS_C codigo_varios_c = new CODIGOS_DATOS_C();
            //--------------------------------------------------------------------------




           //
           list_pto_Rutinas_polilineas= GenerarListaBarrasIntersactadas(ref pto1, ref pto2, acDocEd, acCurDb, list_pto_interseccion);


            // DIBUJAR  SI ENCUENTRA 2 INTERSECCIONES

            if (list_pto_Rutinas_polilineas.Count == 2)
            {
                //reordenado los puntos para que los estribos u las barras se dibujen asobre un area cuadrada
                ComunesLosa.BuscarExternderLineasSiNoSonIgual(ref list_pto_Rutinas_polilineas);
                CODIGOS_GRUPOS GRUPO_ = new CODIGOS_GRUPOS();
                ObjectIdCollection ents = new ObjectIdCollection();
                //BARRAS
                NH_Refuerzo NH_Refuerzo_ = new NH_Refuerzo(list_pto_Rutinas_polilineas, cantidadBarras: cantidadBarras_ref, espaciamiento: espaciamiento_ref, diametro: diametro_ref, capa_inferior: capa_inferior, tiporefuerzo: Tiporefuerzo.ConEstribos);

                float distanciaMin = 10000;
                foreach (var item in NH_Refuerzo_.ListabarrasRefuerzos)
                {
                    NH_suples__Barra NH_suples__Barra_ = new NH_suples__Barra(item.ptIni, item.ptFin, "BARRAS");
                    var poli_creada = NH_suples__Barra_.dibujar_barra_pl();


                    if (poli_creada == null) continue;

                    ents.Add(poli_creada.ObjectId);
                    Point2d pt1 = poli_creada.GetPoint2dAt(1);
                    Point2d pt2 = poli_creada.GetPoint2dAt(0);
                    object poli_creada_ = (object)poli_creada;
                    item.poly = poli_creada;

                    float distacia_ = Intersecciones.DistanciaPerpendicularmenteConLinea_desdeUnPunto(pt1, pt2, NH_Refuerzo_.lineaReferenciaSuperior_ultima.ptFin);

                    if (distanciaMin > distacia_)
                    {
                        distanciaMin = distacia_;
                        NH_Refuerzo_.lineaReferenciaSuperior_ultima.poly = poli_creada;
                    }

                    codigo_varios_c.addData_PROG_LOSA(poli_creada.ObjectId, "R1", ref pt1, ref pt2, diametro_ref, (int)poli_creada.GetPoint3dAt(1).DistanceTo(poli_creada.GetPoint3dAt(0)), poli_creada.GetPoint3dAt(1).DistanceTo(poli_creada.GetPoint3dAt(0)).ToString(), 6, _direccion_LINEA, 0.0f, 00.0f);// ', largo, My1Commands.datos_barra(0))


                }
                //--------TEXTO BARRA
                Rutinas_Textos Rutinas_Textos_ = new Rutinas_Textos();

                if (TipoPosicionTexto_ == TipoPosicionTexto.TodoArriba)
                {
                    Rutinas_Textos_.porcentaje_texto_inical = 0.15f;
                    Rutinas_Textos_.porcentaje_texto_Final = 0.85f;
                }
                else
                {
                    Rutinas_Textos_.porcentaje_texto_inical = 0.4f;
                    Rutinas_Textos_.porcentaje_texto_Final = 0.65f;
                }

                Rutinas_Textos_.dibujar_texto_pl2(ref ents,
                                                  NH_Refuerzo_.lineaReferenciaSuperior_ultima.poly.ObjectId,
                                                 (float)NH_Refuerzo_.lineaReferenciaSuperior_ultima.angle, NH_Refuerzo_.cuantia, "r1", _direccion_LINEA, "a_",
                                                 (float)NH_Refuerzo_.lineaReferenciaSuperior_ultima.poly.Length);

                GRUPO_.creacion_grupo(ref ents, grupo_referencia);

                //ESTRIBO
                ents.Clear();
                NH_Estribos estribo = new NH_Estribos(NH_Refuerzo_, Cantidad_Estr, diametro_Estr, espaciamiento_Estr);
                MUROS MURO_ = new MUROS();
                string aux_espaciamiento = espaciamiento_Estr.ToString();
                MURO_.HACTH_MALLA("", aux_espaciamiento, estribo.listaPuntnos, (float)estribo.angle_estribo, ref ents, "CESTRIBO_E", 0, 7, true);
                ESTRIBOS Estribo_ = new ESTRIBOS();

                if (TipoPosicionTexto_ == TipoPosicionTexto.TodoArriba)
                {
                    Estribo_.dibujar_texto(0, (float)estribo.posiconTexto_superior.X, (float)estribo.posiconTexto_superior.Y, estribo.Cuantia, ref ents, "ESTRIBO", 2, (float)estribo.angle);
                }
                else
                { Estribo_.dibujar_texto(0, (float)estribo.posiconTexto_inferior.X, (float)estribo.posiconTexto_inferior.Y, estribo.Cuantia, ref ents, "ESTRIBO", 2, (float)estribo.angle); }

                GRUPO_.creacion_grupo(ref ents, grupo_referencia);
            }
            else if (list_pto_Rutinas_polilineas.Count == 1)
            {
                //reordenado los puntos para que los estribos u las barras se dibujen asobre un area cuadrada
                ComunesLosa.BuscarExternderLineasSiNoSonIgual(ref list_pto_Rutinas_polilineas);
                CODIGOS_GRUPOS GRUPO_ = new CODIGOS_GRUPOS();
                ObjectIdCollection ents = new ObjectIdCollection();
                //BARRAS
                NH_Refuerzo NH_Refuerzo_ = new NH_Refuerzo(list_pto_Rutinas_polilineas, cantidadBarras: cantidadBarras_ref, espaciamiento: espaciamiento_ref, diametro: diametro_ref, capa_inferior: capa_inferior,tiporefuerzo: Tiporefuerzo.CabezaMuro);

                float distanciaMin = 10000;
                foreach (var item in NH_Refuerzo_.ListabarrasRefuerzos)
                {
                    NH_suples__Barra NH_suples__Barra_ = new NH_suples__Barra(item.ptIni, item.ptFin, "BARRAS");
                    var poli_creada = NH_suples__Barra_.dibujar_barra_pl();

                    if (poli_creada == null) continue;

                    ents.Add(poli_creada.ObjectId);
                    Point2d pt1 = poli_creada.GetPoint2dAt(1);
                    Point2d pt2 = poli_creada.GetPoint2dAt(0);
                    object poli_creada_ = (object)poli_creada;
                    item.poly = poli_creada;

                    float distacia_ = Intersecciones.DistanciaPerpendicularmenteConLinea_desdeUnPunto(pt1, pt2, NH_Refuerzo_.lineaReferenciaSuperior_ultima.ptFin);

                    if (distanciaMin > distacia_)
                    {
                        distanciaMin = distacia_;
                        NH_Refuerzo_.lineaReferenciaSuperior_ultima.poly = poli_creada;
                    }

                    codigo_varios_c.addData_PROG_LOSA(poli_creada.ObjectId, "R1", ref pt1, ref pt2, diametro_ref, (int)poli_creada.GetPoint3dAt(1).DistanceTo(poli_creada.GetPoint3dAt(0)), poli_creada.GetPoint3dAt(1).DistanceTo(poli_creada.GetPoint3dAt(0)).ToString(), 6, _direccion_LINEA, 0.0f, 00.0f);// ', largo, My1Commands.datos_barra(0))


                }
                //--------TEXTO BARRA
                Rutinas_Textos Rutinas_Textos_ = new Rutinas_Textos();

                if (TipoPosicionTexto_ == TipoPosicionTexto.TodoArriba)
                {
                    Rutinas_Textos_.porcentaje_texto_inical = 0.25f;
                    Rutinas_Textos_.porcentaje_texto_Final = 0.75f;
                }
                else
                {
                    Rutinas_Textos_.porcentaje_texto_inical = 0.25f;
                    Rutinas_Textos_.porcentaje_texto_Final = 0.75f;
                }

                Rutinas_Textos_.dibujar_texto_pl2(ref ents,
                                                  NH_Refuerzo_.lineaReferenciaSuperior_ultima.poly.ObjectId,
                                                 (float)NH_Refuerzo_.lineaReferenciaSuperior_ultima.angle, NH_Refuerzo_.cuantia, "r1", _direccion_LINEA, "a_",
                                                 (float)NH_Refuerzo_.lineaReferenciaSuperior_ultima.poly.Length);

                GRUPO_.creacion_grupo(ref ents, grupo_referencia);

              
            }




        }

       //  con los puntos selecciondos busca las barras para generar el refuerzo de muro(busca 2 pililineas) o refuerzo de muro (busca 1 linea)
       private List<NH_PolilineaRef> GenerarListaBarrasIntersactadas(ref Point3d pto1, ref Point3d pto2, Editor acDocEd, Database acCurDb, List<Point3d> list_pto_interseccion)
       {
           List<NH_PolilineaRef> list_pto_Rutinas_polilineas = new List<NH_PolilineaRef>(); 
            
           

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
                           aux_.PuntoSelecionInicialMouse = pto1;
                           aux_.PuntoSelecionFinalMouse = pto2;
                          // aux_.TipoSeleccionMouse_=TipoSeleccionMouse TipoSeleccionMouse_
                           if (aux_ != null) list_pto_Rutinas_polilineas.Add(aux_);
                           if (list_pto_Rutinas_polilineas.Count == 2) salirSegundoFor = true; break;
                       }
                   }
                   if (salirSegundoFor) break;
               }

           }

           return list_pto_Rutinas_polilineas;
       }

        // revisar barras
        public List<ObjectId> SelecionadoElementos(Point3d pt_ini, Point3d pt_fin, double ANGLE)
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


            acDocEd.WriteMessage("Seleccionan :  POLYLINE  (REF LOSA,SHAFT,MUROS,VIGAS,ARRANQUES)");
            acDocEd.WriteMessage("Seleccionan :  LINE  (REF LOSA,SHAFT,MUROS,VIGAS,ARRANQUES)");
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




       //----------------------------------------------------------------------------------------------------------------------


     

    }
}
