using System;

using System.Collections.Generic;

using System.Text;

using System.Windows.Forms;



using Autodesk.Revit.DB;

using Autodesk.Revit.UI;

using Autodesk.Revit.ApplicationServices;

using Autodesk.Revit.Attributes;

using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.AnalisisRoom.Servicios;

namespace ArmaduraLosaRevit.Model.Prueba
{
   // [TransactionAttribute(TransactionMode.Manual)]

    public class CambiarDireccion : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData,ref string messages, ElementSet elements)
        {

            UIApplication app = commandData.Application;

            ModificarParametrosRoom modificarParametrosRoom= new ModificarParametrosRoom(app.ActiveUIDocument);
            modificarParametrosRoom.cambiarDireccionPrincipal();


            //borrar elemtos si existen
            ServicioaCambiarLargoMinV2 servicioaCambiarLargoMin = new ServicioaCambiarLargoMinV2(commandData);
            //borrar elemtos
            servicioaCambiarLargoMin.BorrarLargosMin(modificarParametrosRoom.ListaRooms);


            //dibujar
            servicioaCambiarLargoMin.DibujarLargosMin();

            // redib8uja
            return Result.Succeeded;

        }

    }
}
