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


//[assembly: CommandClass(typeof(planta_aux_C.RutinasSoloPruebas.Prueba_BuscarPolyLosaContigua))]


namespace planta_aux_C.RutinasSoloPruebas
{
   public class Prueba_BuscarPolyLosaContigua
    {

       // busca los dos ptos de intersecion de dos losas contiguas , ademas de nombre de esas losas
       // programa pide un punto de referecnia cona el desde el cual se dibuja una linea q busca los pitnos de interseccion, de las dos losas contiguas
       [CommandMethod("Prueba_dibujareBarraParaObtener_puntoParaDibujarSuple")]
       public void Prueba_dibujareBarraParaObtener_puntoParaDibujarSuple()
       {
           Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
           Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
           Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;


           Formulario Formulario_nh = new Formulario();

           Formulario_nh._largominimo = 60;
           Formulario_nh._largoAhorro_barra = 400;
           Formulario_nh._largoAhorro_recorrido = 150;


           PromptEntityOptions peo = new PromptEntityOptions("\nSelect a polyline: ");
           peo.SetRejectMessage("Only a polyline.");
           peo.AddAllowedClass(typeof(Polyline), true);
           PromptEntityResult per = acDocEd.GetEntity(peo);
           if (per.Status != PromptStatus.OK) return;

           PromptPointResult ppr = acDocEd.GetPoint("\nSelect a point");
           if (ppr.Status != PromptStatus.OK) return;


           PromptEntityOptions peo2 = new PromptEntityOptions("\nSelect a 2da polyline: ");
           peo2.SetRejectMessage("Only a polyline.");
           peo2.AddAllowedClass(typeof(Polyline), true);
           PromptEntityResult per2 = acDocEd.GetEntity(peo2);
           if (per2.Status != PromptStatus.OK) return;

           using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
           {
               Polyline pline = (Polyline)tr.GetObject(per.ObjectId, OpenMode.ForRead);
               BuscarPolyLosaContigua aux_BuscarPolyLosaContigua = new BuscarPolyLosaContigua();

               BuscarPolyLosaContigua.BuscaSiExistePoligonoLosaContigua_aux(per.ObjectId, ppr.Value, "vertical_a", Formulario_nh.ListaPOlilineaYEsferaLosa, 60, 60, tr, Formulario_nh.ListaPOlilineaYEsferaLosa.Where(c => c.PoligonoLosa == per2.ObjectId).FirstOrDefault(), tipoDeBarra.suple);

               tr.Commit();
           }

       }
     

    }
}
