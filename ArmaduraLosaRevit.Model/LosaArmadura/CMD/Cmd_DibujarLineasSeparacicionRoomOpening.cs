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
    public class Cmd_DibujarLineasSeparacicionRoomOpening : IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
     
      
            //NH_ListaVIgas CREA UN OBJECTO PARA ANALISIS LISTA  'List<ProfileBeam> ListaProfileBeam'
            NH_ListaOpening listaOPening = new NH_ListaOpening(commandData.Application);

            /// 1)obtiene lista de vigas en el nivel de trabajo 
            /// 2)obtiene su  geometria ProfileBeam
            listaOPening.GetOpeningPoligonos();

            //return Result.Succeeded;

            if (listaOPening.ListaProfileOpening.Count > 0)
            {
                Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document, "Classe_prueba_open");
                trans.Start();
                try
                {
                    //borrar las lineas room separation
                   // listaOPening.BorrarTodas();

                    //Genera LAs lineas de separacion de rooms
                    listaOPening.DibujarLineasSeparacicionRoom();
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



        public Result Execute_NH_ListaOpening(ExternalCommandData commandData)
        {


            //NH_ListaVIgas CREA UN OBJECTO PARA ANALISIS LISTA  'List<ProfileBeam> ListaProfileBeam'
            NH_ListaOpening listaOPening = new NH_ListaOpening(commandData.Application);

            /// 1)obtiene lista de vigas en el nivel de trabajo 
            /// 2)obtiene su  geometria ProfileBeam
            listaOPening.GetOpeningPoligonos();

            //return Result.Succeeded;

            if (listaOPening.ListaProfileOpening.Count > 0)
            {
                Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document, "Classe_prueba_open");
                trans.Start();
                try
                {
                    //borrar las lineas room separation
                    //listaOPening.BorrarTodas();

                    //Genera LAs lineas de separacion de rooms
                    listaOPening.DibujarLineasSeparacicionRoom();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Revit", ex.Message);
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
