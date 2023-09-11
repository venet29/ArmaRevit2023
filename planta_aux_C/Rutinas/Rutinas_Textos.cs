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
using VARIOS_C.Datos;
using VARIOS_C;


namespace planta_aux_C.Rutinas
{
   public  class Rutinas_Textos
   {

       #region 0) Propiedades

       public Point2d Punto_inicial_texto_inf_ { get; set; }
       public Point2d Punto_inicial_texto_sup_ { get; set; }
       public int factor_escala { get; set; }

       public float porcentaje_texto_inical { get; set; }
       public float porcentaje_texto_Final { get; set; }

       public float porcentaje_texto_inical_barra_r1 { get; set; }
       public float porcentaje_texto_Final_barra_r1 { get; set; }

       #endregion


       #region 1) Contructores
       public Rutinas_Textos()
       {
           factor_escala = 1;

           porcentaje_texto_inical = 0.27f;
           porcentaje_texto_Final = 0.8f;


           porcentaje_texto_inical_barra_r1 = 0.15f;
           porcentaje_texto_Final_barra_r1 = 0.85f;
       }
       #endregion


       // cambia el texto de forma mas interactiva
        public bool dibujar_texto_pl2(ref ObjectIdCollection ents,  ObjectId poly_obj,  float angle,  string texto,  string casos,  string direcci, string posi_barra, float largo_para_texto)
        {
            Document Doc = Application.DocumentManager.MdiActiveDocument;
            Database DB = Doc.Database;
            Editor Ed = Doc.Editor;
         //   CDatos_Internos Cdatos_int = new CDatos_Internos();
         //   Class1_PL clases_class1 = new Class1_PL();
            CODIGOS_DATOS CODIGOS_DATOS_ = new CODIGOS_DATOS();
            bool resultado = false;

            using (Transaction tr = DB.TransactionManager.StartTransaction())
            {
                // Abrir la tabla de bloques en modo lectura
                // Dim e1 As Single = 13
                Point2d pto0 = new Point2d();
                Point2d pto1 = new Point2d();
                Point2d pto2 = new Point2d();
                Point2d pto3 = new Point2d();
                Point2d pto4 = new Point2d();
                Point2d pto5 = new Point2d();

                Point2d pto_a = new Point2d();/* TODO Change to default(_) if this is not a reference type */
                Point2d pto_b = new Point2d();/* TODO Change to default(_) if this is not a reference type */
                Point2d pto_c = new Point2d();/* TODO Change to default(_) if this is not a reference type */
                Point2d pto_d = new Point2d();/* TODO Change to default(_) if this is not a reference type */
                Point2d pto_e = new Point2d();/* TODO Change to default(_) if this is not a reference type */
                Point2d pto_f = new Point2d();/* TODO Change to default(_) if this is not a reference type */

                try
                {
                    int color = -1;
                    if (texto.IndexOf( "*",0) != -1)
                        color = 1;
                    else
                        color = -1;
                    texto = texto.Replace( "*", "");

                    string[] tipo_barra = new string[10];
                    tipo_barra[0] = casos;

                    string[] tipo_caso_barra = new string[10];
                    CDatos_Internos_C.reescribir_datos_barra_losa(ref poly_obj, tipo_barra,ref tipo_caso_barra, ref pto0, ref pto1, ref pto2, ref pto3, ref pto4, ref pto5, true);

                    // tipo_caso_barra = clases_class1.TIPO_caso_BARRA_ind_losa(poly, casos, pto0, pto1, pto2, pto3, pto4, pto5)
                    double angle1 = Convert.ToDouble(tipo_caso_barra[5]);
                    float x_text_c, y_text_c;
                    float x_text_c2, y_text_c2;

                    //float porcentaje_texto_inical = 0.27f;
                    //float porcentaje_texto_Final = 0.8f;

                    casos = tipo_caso_barra[0];
                    direcci = tipo_caso_barra[2];
                    Polyline poly = tr.GetObject(poly_obj, OpenMode.ForWrite) as Polyline;
                    switch (poly.NumberOfVertices)
                    {
                        case 2:
                            {
                                pto_d = pto1;  // CASO C
                                pto_c = pto0; // CASO D
                                break;
                            }

                        case 3:
                            {
                                if (direcci == "horizontal_i" | direcci == "vertical_b")
                                {
                                    pto_e = pto2;
                                    pto_d = pto1; // CASO C
                                    pto_c = pto0; // CASO D
                                }
                                else
                                {
                                    pto_d = pto2; // CASO C
                                    pto_c = pto1; // CASO D
                                    pto_b = pto0; // CASO D
                                }

                                break;
                            }

                        case 4:
                            {
                                if (direcci == "horizontal_i" | direcci == "vertical_b")
                                {
                                    if (casos == "f1")
                                    {
                                        pto_f = pto3;
                                        pto_e = pto2;
                                        pto_d = pto1; // CASO C
                                        pto_c = pto0; // CASO D
                                    }
                                    else if (casos == "f10")
                                    {
                                        pto_e = pto3;
                                        pto_d = pto2;   // CASO C
                                        pto_c = pto1; // CASO D
                                        pto_b = pto0;
                                    }
                                    else if (casos == "F9a")
                                    {
                                        pto_e = pto3;
                                        pto_d = pto2; // CASO C
                                        pto_c = pto1; // CASO D
                                        pto_b = pto0;
                                    }
                                }
                                else if (casos == "f1")
                                {
                                    pto_d = pto3; // CASO C
                                    pto_c = pto2; // CASO D
                                    pto_b = pto1;
                                    pto_a = pto0;
                                }
                                else if (casos == "f10")
                                {
                                    pto_d = pto2;   // CASO C
                                    pto_c = pto1; // CASO D
                                }
                                else if (casos == "F9a")
                                {
                                    pto_d = pto2; // CASO C
                                    pto_c = pto1; // CASO D
                                }

                                break;
                            }

                        case 5:
                            {
                                if (direcci == "horizontal_i" | direcci == "vertical_b")
                                {
                                    if (casos == "f0" | casos == "f7" | casos == "s3a")
                                    {
                                        pto_f = pto4; // CASO C
                                        pto_e = pto3; // CASO C
                                        pto_d = pto2; // CASO C
                                        pto_c = pto1; // CASO D
                                        pto_b = pto0;
                                    }
                                }
                                else if (casos == "f0" | casos == "f7" | casos == "s3b")
                                {
                                    pto_e = pto4; // CASO C
                                    pto_d = pto3; // CASO C
                                    pto_c = pto2; // CASO D
                                    pto_b = pto1; // CASO D
                                    pto_a = pto0; // CASO D
                                }

                                break;
                            }

                        case 6:
                            {
                                pto_f = pto5; // CASO C
                                pto_e = pto4; // CASO C
                                pto_d = pto3; // CASO C
                                pto_c = pto2; // CASO D
                                pto_b = pto1; // CASO D
                                pto_a = pto0; // CASO D

                                if (casos == "s1" | casos == "s3")
                                {
                                    porcentaje_texto_inical = porcentaje_texto_inical_barra_r1;
                                    porcentaje_texto_Final = porcentaje_texto_Final_barra_r1;
                                }

                                break;
                            }

                        default:
                            {
                                // does some processing... 
                                break;
                            }
                    }
                    // ***************************************
                    ObjectId[] acObjId_grup;
                    CODIGOS_GRUPOS GRUPO_ = new CODIGOS_GRUPOS();
                    acObjId_grup = GRUPO_.buscar_grupo(poly_obj);
                    string aux_barra = CDatos_Internos_C.getData_xdata_losa(poly_obj);
                    // ******************

                    if (ents == null/* TODO Change to default(_) if this is not a reference type */ )
                    {
                        foreach (ObjectId idObj in acObjId_grup)
                        {
                            if (idObj.ObjectClass.DxfName.ToString() == "TEXT")
                            {
                                DBText acEnt_barra_aux = tr.GetObject(idObj, OpenMode.ForWrite) as DBText;
                                string aux_texto = CDatos_Internos_C.getData_xdata_losa(idObj);

                                if (aux_barra == "a_" & aux_texto.IndexOf( "a_",0) != -1)
                                {
                                    if ("a_ab" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_a.GetDistanceTo(pto_b), 0).ToString();
                                    else if ("a_bc" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_b.GetDistanceTo(pto_c), 0).ToString();
                                    else if ("a_cd" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_c.GetDistanceTo(pto_d), 0).ToString();
                                    else if ("a_de" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_d.GetDistanceTo(pto_e), 0).ToString();
                                    else if ("a_ef" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_e.GetDistanceTo(pto_f), 0).ToString();
                                    else if ("a_largo" == aux_texto)
                                    {
                                        acEnt_barra_aux.TextString = "L=" + Math.Round(poly.Length, MidpointRounding.AwayFromZero); resultado = true;
                                    }
                                }
                                else if (aux_barra == "b_" & aux_texto.IndexOf("b_",0) != -1)
                                {
                                    if ("b_ab" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_a.GetDistanceTo(pto_b), 0).ToString();
                                    else if ("b_bc" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_b.GetDistanceTo(pto_c), 0).ToString();
                                    else if ("b_cd" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_c.GetDistanceTo(pto_d), 0).ToString();
                                    else if ("b_de" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_d.GetDistanceTo(pto_e), 0).ToString();
                                    else if ("b_ef" == aux_texto)
                                        acEnt_barra_aux.TextString = Math.Round(pto_e.GetDistanceTo(pto_f), 0).ToString();
                                    else if ("b_largo" == aux_texto)
                                    {
                                        acEnt_barra_aux.TextString = "L=" + Math.Round(poly.Length, MidpointRounding.AwayFromZero); resultado = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ( casos!= null && casos.ToLower() == "r1")
                        {
                            for (int i = 0; i < ents.Count; i++)
			                {
                                ObjectId idObj = ents[i];
			                        if (idObj.IsErased == false && idObj.ObjectClass.DxfName.ToString() == "LWPOLYLINE")
                                    CDatos_Internos_C.add_xdata_progn2("name_largo_texto",ref idObj, posi_barra);
			                }
                        }
                        else
                            CDatos_Internos_C.add_xdata_progn2("name_largo_texto", ref poly_obj, posi_barra);

                        if (casos.ToLower() == "r1")
                        {
                            Punto_inicial_texto_inf_ = pto_c;
                            Punto_inicial_texto_sup_ = pto_d;
                        }


                        resultado = false;


                        Point2d pto_inicia_texto;
                        if (posi_barra == "a_")
                            pto_inicia_texto = Punto_inicial_texto_sup_;
                        else
                            pto_inicia_texto = Punto_inicial_texto_inf_;


                        // largo_para_texto = Math.Max(pto_c.GetDistanceTo(pto_d), largo_para_texto)

                        x_text_c = (float)(pto_inicia_texto.X + largo_para_texto * Math.Cos(angle1) * porcentaje_texto_inical + 10 * Math.Cos(Math.PI / (double)2 + angle1));
                        y_text_c = (float)(pto_inicia_texto.Y + largo_para_texto * Math.Sin(angle1) * porcentaje_texto_inical + 10 * Math.Sin(Math.PI / (double)2 + angle1));

                        // CASO D
                        x_text_c2 = (float)(pto_inicia_texto.X + largo_para_texto * Math.Cos(angle1) * porcentaje_texto_Final + 10 * Math.Cos(Math.PI / (double)2 + angle1));
                        y_text_c2 = (float)(pto_inicia_texto.Y + largo_para_texto * Math.Sin(angle1) * porcentaje_texto_Final + 10 * Math.Sin(Math.PI / (double)2 + angle1));

                        angle = (float)angle1;

                        dibujar_texto(0, x_text_c, y_text_c, texto,ref ents, "TEXTO", color, angle);
                        var aux_ultimo_elemto =comunes.ultimo_elemto_texto_losa();
                        CDatos_Internos_C.add_xdata_progn2("name_largo_texto", ref aux_ultimo_elemto, posi_barra + "cua");
                      
                      
                        dibujar_texto(0, x_text_c2, y_text_c2, "L=" + Math.Round(poly.Length, MidpointRounding.AwayFromZero).ToString(), ref ents, "TEXTO", -1, angle);
                        var aux_ultimo_elemto2 = comunes.ultimo_elemto_texto_losa();
                        CDatos_Internos_C.add_xdata_progn2("name_largo_texto",ref  aux_ultimo_elemto2, posi_barra + "largo");

                        if (poly.NumberOfVertices > 2)
                        {
                            x_text_c2 = (float)(pto_inicia_texto.X + largo_para_texto * Math.Cos(angle1) * porcentaje_texto_Final - 10 * Math.Cos(Math.PI / (double)2 + angle1));
                            y_text_c2 = (float)(pto_inicia_texto.Y + largo_para_texto * Math.Sin(angle1) * porcentaje_texto_Final - 10 * Math.Sin(Math.PI / (double)2 + angle1));
                            dibujar_texto(0, x_text_c2, y_text_c2, Math.Round(pto_d.GetDistanceTo(pto_c), MidpointRounding.AwayFromZero).ToString(), ref ents, "TEXTO", -1, angle);
                              var aux_ultimo_elemto3 =comunes.ultimo_elemto_texto_losa();
                            CDatos_Internos_C.add_xdata_progn2("name_largo_texto", ref aux_ultimo_elemto3,posi_barra + "cd");
                        }
                        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                        Point2d pto_refe = new Point2d(0, 0);
                        // a-b
                        if (pto_a != pto_refe & pto_b != pto_refe)
                        {
                            int direc = 1;
                            if ((comunes.angulo_entre_pt(pto_c, pto_b) < 0.1))
                                direc = -1;

                            x_text_c2 = (float)(pto_a.X + pto_a.GetDistanceTo(pto_b) * Math.Cos(angle1) * 0.5 + 10 * Math.Cos(Math.PI / (double)2 + angle1) * direc);
                            y_text_c2 = (float)(pto_a.Y + pto_a.GetDistanceTo(pto_b) * Math.Sin(angle1) * 0.5 + 10 * Math.Sin(Math.PI / (double)2 + angle1) * direc);
                            dibujar_texto(0, x_text_c2, y_text_c2, Math.Round(pto_a.GetDistanceTo(pto_b), MidpointRounding.AwayFromZero).ToString(), ref ents, "TEXTO", -1, angle);

                              var aux_ultimo_elemto4 =comunes.ultimo_elemto_texto_losa();
                            CDatos_Internos_C.add_xdata_progn2("name_largo_texto", ref aux_ultimo_elemto4, posi_barra + "ab");
                        }

                        // e-f
                        if (pto_e != pto_refe  & pto_f != pto_refe)
                        {
                            int direc = 1;
                            if ((comunes.angulo_entre_pt( pto_d, pto_e) < 0.1))
                                direc = -1;
                            x_text_c2 = (float)(pto_e.X + pto_f.GetDistanceTo(pto_e) * Math.Cos(angle1) * 0.5 + 10 * Math.Cos(Math.PI / (double)2 + angle1) * direc);
                            y_text_c2 = (float)(pto_e.Y + pto_f.GetDistanceTo(pto_e) * Math.Sin(angle1) * 0.5 + 10 * Math.Sin(Math.PI / (double)2 + angle1) * direc);
                            dibujar_texto(0, x_text_c2, y_text_c2, Math.Round(pto_e.GetDistanceTo(pto_f), MidpointRounding.AwayFromZero).ToString(), ref ents, "TEXTO", -1, angle);
                           
                              var aux_ultimo_elemto5 =comunes.ultimo_elemto_texto_losa();
                            CDatos_Internos_C.add_xdata_progn2("name_largo_texto",ref aux_ultimo_elemto5, posi_barra + "ef");
                        }
                        // xxxxxxxxxxxxxxxxxxxxxxx
                        // b-c
                        if (pto_b != pto_refe & pto_c != pto_refe )
                        {
                            double angulo_c_b = comunes.angulo_entre_pt( pto_c, pto_b);
                            x_text_c2 = (float)(pto_c.X + pto_b.GetDistanceTo(pto_c) * Math.Cos(angulo_c_b) * 0.5 + 10 * Math.Cos(angle1));
                            y_text_c2 = (float)(pto_c.Y + pto_b.GetDistanceTo(pto_c) * Math.Sin(angulo_c_b) * 0.5 + 10 * Math.Sin(angle1));
                            dibujar_texto(0, x_text_c2, y_text_c2, Math.Round(pto_c.GetDistanceTo(pto_b), MidpointRounding.AwayFromZero).ToString(), ref ents, "TEXTO", -1, angle);
                                    var aux_ultimo_elemto6 =comunes.ultimo_elemto_texto_losa();
                            CDatos_Internos_C.add_xdata_progn2("name_largo_texto", ref aux_ultimo_elemto6, posi_barra + "bc");
                        }
                        // d-e
                        if (pto_e != pto_refe & pto_d != pto_refe )
                        {
                            double angulo_d_e = comunes.angulo_entre_pt(pto_d, pto_e);
                            x_text_c2 = (float)(pto_d.X + pto_d.GetDistanceTo(pto_e) * Math.Cos(angulo_d_e) * 0.5 - 10 * Math.Cos(angle1));
                            y_text_c2 = (float)(pto_d.Y + pto_d.GetDistanceTo(pto_e) * Math.Sin(angulo_d_e) * 0.5 - 10 * Math.Sin(angle1));
                            dibujar_texto(0, x_text_c2, y_text_c2, Math.Round(pto_d.GetDistanceTo(pto_e), MidpointRounding.AwayFromZero).ToString(),ref ents, "TEXTO", -1, angle);
                           var aux_ultimo_elemto6 =comunes.ultimo_elemto_texto_losa();
                            CDatos_Internos_C.add_xdata_progn2("name_largo_texto", ref aux_ultimo_elemto6, posi_barra + "de");
                        }
                    }

                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception Ex)
                {
                    tr.Dispose();
                    Application.ShowAlertDialog("Error: 'reac_modif_barra_final'"  + Ex.Message);
                }
                finally
                {
                    int asas = 0;
                }


                return resultado;
            }
        }


        public void dibujar_texto(int orientacion, float x2_text, float y2_text, string strVal, ref ObjectIdCollection ents_dt, string texto_layer, int num_color, float giro_texto)
        {
            // Obtener el documento y la base de datos actuales
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            // If Not Directory.Exists(cots) Then
            // Global.System.Windows.Forms.Application.Exit()
            // End Iftras
            using (acDoc.LockDocument())


            // Iniciar una transaccion
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {

                // Abrir la tabla de bloques en modo lectura
                BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);

                // Abrir el registro del bloque de Espacio Modelo en modo escritura
                BlockTableRecord acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite);


                Point3d acPtIns = new Point3d(x2_text, y2_text, 0);
                Point3d acPtAlign = new Point3d(x2_text, y2_text, 0);

                // Crear el objeto de texto
                DBText acText = new DBText();
                acText.SetDatabaseDefaults();
                acText.Position = acPtIns;
                acText.Height = 10 * factor_escala;
                acText.WidthFactor = 0.9;
                acText.Layer = texto_layer;
                acText.TextString = strVal;
                if (num_color != -1)
                    acText.ColorIndex = num_color;
                // 90grados
                acText.Rotation = giro_texto; // 0.785 * 2
                // Establecer la alineacion del texto
                // textAlign(0) = TextHorizontalMode.TextLeft
                acText.HorizontalMode = TextHorizontalMode.TextMid;

                if (acText.HorizontalMode != TextHorizontalMode.TextLeft)
                    acText.AlignmentPoint = acPtAlign;

                acText.AdjustAlignment(acCurDb);

                ObjectId objId = acBlkTblRec.AppendEntity(acText);
                ents_dt.Add(objId);

                acTrans.AddNewlyCreatedDBObject(acText, true);
                acTrans.Commit();
            }
        }

    }
}
