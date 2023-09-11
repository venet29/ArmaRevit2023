using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using System.Diagnostics;
using System.Collections;
using ArmaduraLosaRevit.Model.Elementos_viga;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model;
//using planta_aux_C.Elemento_Losa;


namespace ArmaduraLosaRevit.Model.LosaArmadura.Cmd
{
    // CLASE QUE BUSCA LAS VIGAS EN LA VISTA (VIEW) ANALIZADA, OBTIENE SU GEOMETRIA
    // Y REDIBUJA LA VIGA CON SeparacicionRoom AL NIVEL DE VIEW

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Cmd_DibujarLineasSeparacicionRoom : IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

              


            //NH_ListaVIgas CREA UN OBJECTO PARA ANALISIS LISTA  'List<ProfileBeam> ListaProfileBeam'
            NH_ListaVIgas listaVIgas = new NH_ListaVIgas(commandData);

            /// 1)obtiene lista de vigas en el nivel de trabajo 
            /// 2)obtiene su  geometria ProfileBeam
            listaVIgas.GetVigaPoligonos(doc.ActiveView);

            if (listaVIgas.ListaProfileBeam.Count > 0)
            {
                Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document, "Classe_prueba_vigas");
                trans.Start();
                try
                {
                    //borrar las lineas room separation
                    listaVIgas.BorrarTodas();

                    //Genera LAs lineas de separacion de rooms
                    listaVIgas.DibujarLineasSeparacicionRoom();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    trans.RollBack();
                    return Autodesk.Revit.UI.Result.Cancelled;
                }
                trans.Commit();
                return Autodesk.Revit.UI.Result.Succeeded;
            }

            return Result.Succeeded;
        }
    }
}
