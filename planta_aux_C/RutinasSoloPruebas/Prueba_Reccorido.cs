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

//[assembly: CommandClass(typeof(planta_aux_C.RutinasSoloPruebas.Prueba_Reccorido))]


namespace planta_aux_C.RutinasSoloPruebas
{
    public class Prueba_Reccorido
    {

        [CommandMethod("Prueba_RecorridoCoordenadasIniciaYfinalRecorrido")]
        public void Prueba_borrarTodosElementosDelGrupo()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;


            PromptEntityOptions peo = new PromptEntityOptions("\nSelect a polyline: ");
            peo.SetRejectMessage("Only a polyline.");
            peo.AddAllowedClass(typeof(Polyline), true);
            PromptEntityResult per = acDocEd.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;

            RecorridoBarras Prueba_Reccorido_ = new RecorridoBarras();
            Prueba_Reccorido_.ObtenerCoordenadasRecorrido(per.ObjectId);

            AcApp.ShowAlertDialog("Puntos recorridos Inicial: " + Prueba_Reccorido_.xline1_tp.ToString() + "      final : " + Prueba_Reccorido_.xline2_tp.ToString());


        }

        [CommandMethod("Prueba_ExtenderRecorrido_2ptos")]
        public void Prueba_ExtenderRecorrido_2ptos()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;


            PromptEntityOptions peo = new PromptEntityOptions("\nSelect a polyline: ");
            peo.SetRejectMessage("Only a polyline.");
            peo.AddAllowedClass(typeof(Polyline), true);
            PromptEntityResult per = acDocEd.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;


            PromptPointResult ppr = acDocEd.GetPoint("\n1)Select a point1");
            if (ppr.Status != PromptStatus.OK) return;

            PromptPointResult ppr2 = acDocEd.GetPoint("\n2)Select a point2");
            if (ppr2.Status != PromptStatus.OK) return;


            RecorridoBarras Prueba_Reccorido_ = new RecorridoBarras();
            Prueba_Reccorido_.ExtenderCoordenadasRecorrido(per.ObjectId, ppr.Value, ppr2.Value);

        }


        [CommandMethod("Prueba_BUscarVectorPerpendocularDesddePuntoArecorrido")]
        public void Prueba_BUscarVectorPerpendocularDesddePuntoArecorrido()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;


            PromptEntityOptions peo = new PromptEntityOptions("\nSelect a polyline: ");
            peo.SetRejectMessage("Only a polyline.");
            peo.AddAllowedClass(typeof(Polyline), true);
            PromptEntityResult per = acDocEd.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;

            RecorridoBarras Prueba_Reccorido_ = new RecorridoBarras();
            Prueba_Reccorido_.ObtenerCoordenadasRecorrido(per.ObjectId);

            PromptPointResult ppr2 = acDocEd.GetPoint("\n2)Select a point2");
            if (ppr2.Status != PromptStatus.OK) return;

            //Intersecciones Intersecciones_ = new Intersecciones();
            //Intersecciones.
            var pto = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(Prueba_Reccorido_.xline1_tp, Prueba_Reccorido_.xline2_tp, ppr2.Value);


            //AcApp.ShowAlertDialog("Puntos recorridos Inicial: " + Prueba_Reccorido_.xline1_tp.ToString() + "      final : " + Prueba_Reccorido_.xline2_tp.ToString());


        }





        [CommandMethod("Prueba_MoverRecorridoptoselecionado")]
        public void Prueba_MoverRecorridoptoselecionado()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;


            PromptEntityOptions peo = new PromptEntityOptions("\nSelect a polyline: ");
            peo.SetRejectMessage("Only a polyline.");
            peo.AddAllowedClass(typeof(RotatedDimension), true);
            PromptEntityResult per = acDocEd.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;

            RecorridoBarras Prueba_Reccorido_ = new RecorridoBarras();
            Prueba_Reccorido_.ObtenerCoordenadasRecorrido(per.ObjectId);

            PromptPointResult ppr2 = acDocEd.GetPoint("\n2)Select a point2");
            if (ppr2.Status != PromptStatus.OK) return;

            //Intersecciones Intersecciones_ = new Intersecciones();
            //Intersecciones.
            var pto_perpendicular = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(Prueba_Reccorido_.xline1_tp, Prueba_Reccorido_.xline2_tp, ppr2.Value);


            Transform.MoveObject(per.ObjectId, pto_perpendicular, ppr2.Value);



        }


        [CommandMethod("Prueba_AlinearDosReccorido")]
        public void Prueba_AlinearDosReccorido()
        {
     
            RecorridoBarras RecorridoBarras_ = new RecorridoBarras();
            RecorridoBarras_.aux_AlinearDosReccorido();
        }

        [CommandMethod("Prueba_BorrarYRecorridoExtenderOtro")]
        public void Prueba_BorrarYRecorridoExtenderOtro()
        {
            RecorridoBarras RecorridoBarras_ = new RecorridoBarras();
            RecorridoBarras_.aux_BorrarYRecorridoExtenderOtro();

        }


        [CommandMethod("Prueba_UnirRecorridos")]
        public void Prueba_UnirRecorridos()
        {
            RecorridoBarras RecorridoBarras_ = new RecorridoBarras();
            RecorridoBarras_.aux_UnirRecorridos();

        }

    }
}
