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
[assembly: CommandClass(typeof(planta_aux_C.RutinasSoloPruebas.Prueba_Intersecciones))]


namespace planta_aux_C.RutinasSoloPruebas
{
    public class Prueba_Intersecciones
    {
        [CommandMethod("Prueba_Intersecion_POLY_2PTO")]
        public void Prueba_Intersecion_POLY_2PTO()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;

            //----------------------------------------------------------------------------
            PromptEntityOptions poly = new PromptEntityOptions("\nSelect a polyline: ");
            poly.SetRejectMessage("Only a polyline.");
            poly.AddAllowedClass(typeof(Polyline), true);
            PromptEntityResult poly_ent = acDocEd.GetEntity(poly);
            if (poly_ent.Status != PromptStatus.OK) return;

            //---------------------------------------------------------------------------

            PromptPointResult ppr = acDocEd.GetPoint("\n1)Select a point1");
            if (ppr.Status != PromptStatus.OK) return;

            PromptPointResult ppr2 = acDocEd.GetPoint("\n2)Select a point2");
            if (ppr2.Status != PromptStatus.OK) return;


            Point3dCollection isOn = Intersecciones.pto_interseccion_losa(poly_ent.ObjectId, ppr.Value, ppr2.Value, false, true, Intersect.OnBothOperands);
            foreach (Point3d pto in isOn)
            {
                if (ppr.Value.DistanceTo(pto) < 40)
                {

                    // dibujar_suple(casos_dibujar, "*A2867", ppr.Value, pto);
                    System.Windows.Forms.MessageBox.Show("polyline encontrada");
                }
            }

            // AcApp.ShowAlertDialog("Puntos recorridos Inicial: " + Prueba_Reccorido_.xline1_tp.ToString() + "      final : " + Prueba_Reccorido_.xline2_tp.ToString());


        }

      
    }
}

