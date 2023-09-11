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
using planta_aux_C.CargarRutinas;
using planta_aux_C.Elementos.Refuerzo;

[assembly: CommandClass(typeof(planta_aux_C.CargarRutinas.Prueba_Intersecciones_refuerzo))]

namespace planta_aux_C.CargarRutinas
{
    public class Prueba_Intersecciones_refuerzo
    {
        [CommandMethod("Prueba_Intersecion_2PTO_posible_InterseccionMurro_")]
        public void Prueba_Intersecion_2PTO_posible_InterseccionMurro_()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
            List<Point3d> list_pto_interseccion = new List<Point3d>();
            List<NH_PolilineaRef> list_pto_Rutinas_polilineas = new List<NH_PolilineaRef>();

            //---------------------------------------------------------------------------

            PromptPointResult ppr = acDocEd.GetPoint("\n1)Select a point1");
            if (ppr.Status != PromptStatus.OK) return;

            PromptPointResult ppr2 = acDocEd.GetPoint("\n2)Select a point2");
            if (ppr2.Status != PromptStatus.OK) return;


            //--------------------------------------------------------------------------
            NH_DibujarEstriboYRefuerzo NH_Refuerzo_ = new NH_DibujarEstriboYRefuerzo();
            List<ObjectId> lista_lineas_polineas = NH_Refuerzo_.SelecionadoElementos(ppr.Value, ppr2.Value, 0);
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {

                var salirSegundoFor = false;
                foreach (var item in lista_lineas_polineas)
                {
                    Point3dCollection isOn = Intersecciones.pto_interseccion_losa(item, ppr.Value, ppr2.Value, false, true, Intersect.OnBothOperands);
                    foreach (Point3d pto in isOn)
                    {
                        if (!list_pto_interseccion.Contains(pto))
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

            if (list_pto_Rutinas_polilineas.Count == 2)
            {
                NH_Estribos estribo = new NH_Estribos(list_pto_Rutinas_polilineas, "E.D.@8a20");
                ObjectIdCollection ents = new ObjectIdCollection();
                MUROS MURO_ = new MUROS();
                MURO_.HACTH_MALLA("", "8", estribo.listaPuntnos, (float)estribo.angle, ref ents, "CESTRIBO_E", 0, 137, true);
                ESTRIBOS Estribo_ = new ESTRIBOS();
                Estribo_.dibujar_texto(0, (float)estribo.posiconTexto_superior.X, (float)estribo.posiconTexto_superior.Y, estribo.Cuantia, ref ents, "ESTRIBO", 2, (float)estribo.angle);
            }

            System.Windows.Forms.MessageBox.Show("Intersecciones encontradas  encontrada: " + list_pto_interseccion.Count);
            // AcApp.ShowAlertDialog("Puntos recorridos Inicial: " + Prueba_Reccorido_.xline1_tp.ToString() + "      final : " + Prueba_Reccorido_.xline2_tp.ToString());
        }

  
        //*************

        [CommandMethod("Prueba_Intersecion_2PTO_2")]
        public void Prueba_Intersecion_2PTO_posible_InterseccionMurro_creaSoloBarrasRefuerzos()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;
    
            //---------------------------------------------------------------------------

            PromptPointResult ppr = acDocEd.GetPoint("\n1)Select a point1");
            if (ppr.Status != PromptStatus.OK) return;

            PromptPointResult ppr2 = acDocEd.GetPoint("\n2)Select a point2");
           if (ppr2.Status != PromptStatus.OK) return;
            
            string  _direccion_LINEA= "horizontal_i";

            //Dim texto As String = ""
            //If cbx_capa_inferior_ = True Then
            //    texto = "F=F'=" & NUD_cantidad_ & "%%c" & cbx_diametro_
            //Else
            //    texto = "F'=" & NUD_cantidad_ & "%%c" & cbx_diametro_
            //End If

            if (Math.Abs(ppr.Value.X - ppr2.Value.X) < 1 )
            {_direccion_LINEA = "horizontal_i";}
            else
            {_direccion_LINEA = "vertical_b";}


            NH_DibujarEstriboYRefuerzo NH_Refuerzo_ = new NH_DibujarEstriboYRefuerzo();

            NH_Refuerzo_.Dibujar_refuerzo_losa_C(ppr.Value, 
                                                 ppr2.Value,
                                                 diametro_ref: 8, 
                                                 cantidadBarras_ref:6, 
                                                 espaciamiento_ref:20,
                                                 capa_inferior: true,
                                                 Cantidad_Estr: "E.D.",
                                                 diametro_Estr: 8,
                                                 espaciamiento_Estr: 10, 
                                                 _direccion_LINEA: _direccion_LINEA,
                                                 TipoPosicionTexto_: TipoPosicionTexto.TodoArriba);


        }

   
    }
}
