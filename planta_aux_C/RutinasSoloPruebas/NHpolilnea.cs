using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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


//[assembly: CommandClass(typeof(planta_aux_C.RutinasSoloPruebas.NHpolilnea))]



namespace planta_aux_C.RutinasSoloPruebas
{
    public class NHpolilnea
    {
        //1) propiedades------------------------------------------------------------------------------------------------------------------
        
        public Line acPoly_auxParaBarra { get; set; }
        public bool dibujarBarras { get; set; }


        //2) constructo------------------------------------------------------------------------------------------------------------------
        
        public NHpolilnea()
        {
            acPoly_auxParaBarra = null;
            dibujarBarras = false;
        }



        //3) metodos------------------------------------------------------------------------------------------------------------------
        

        // ruta de prueba para encontrar desde un pto1 y a pto2 distancia definida una poliliena de losa 
        [CommandMethod("NH_InterPoliConPto", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void PointOnPolyline()
        {

            Document doc = AcApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peo = new PromptEntityOptions("\nSelect a polyline");
            peo.SetRejectMessage("Please select a polyline");
            peo.AddAllowedClass(typeof(Polyline), true);
            PromptEntityResult per = ed.GetEntity(peo);
            dibujarBarras = false;
            bool IsdibujarPuntosIntersecciones=false;

            if (per.Status != PromptStatus.OK)
                return;
            PromptPointResult ppr = ed.GetPoint("\nSelect a point");
            if (ppr.Status != PromptStatus.OK)
                return;

            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                Polyline polyline = tr.GetObject(per.ObjectId, OpenMode.ForRead) as Polyline;
                if (polyline != null)
                {
                    Point3dCollection isOn = Intersecciones.pto_interseccion_losa(polyline, acPoly_auxParaBarra, 40.0, 40.0, 0.0, ppr.Value, dibujarBarras, IsdibujarPuntosIntersecciones, Intersect.ExtendThis);
                    foreach (Point3d pto in isOn)
                    {
                        if (ppr.Value.DistanceTo(pto) < 40)
                        {

                            // dibujar_suple(casos_dibujar, "*A2867", ppr.Value, pto);
                            System.Windows.Forms.MessageBox.Show("polyline encontrada");
                        }
                    }
                }
                tr.Commit();
            }
        }



    }
}

