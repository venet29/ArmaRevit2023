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

//[assembly: CommandClass(typeof(planta_aux_C.RutinasSoloPruebas.Prueba_Borrar_Elemntos))]

namespace planta_aux_C.RutinasSoloPruebas
{
    public class Prueba_Borrar_Elemntos
    {

        [CommandMethod("Prueba_borrarTodosElementosDelGrupo")]
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

            BorrarElementos BorrarElementos_ = new BorrarElementos();
            BorrarElementos_.BorrarTodosElementos(per.ObjectId);
        }
     
    }
}
