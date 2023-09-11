
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using ArmaduraLosaRevit.Model.AnalisisRoom.Servicios;

namespace ArmaduraLosaRevit.Model.Prueba
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class DibujarLargoMin : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData,ref string messages, ElementSet elements)
        {

            UIApplication app = commandData.Application;

            //crear servicio
            ServicioaCambiarLargoMinV2 modificarParametrosRoom = new ServicioaCambiarLargoMinV2(commandData);
            //seleccionar
            IEnumerable<Element> ListaRooms = new List<Element>();
            //ListaRooms = modificarParametrosRoom.roomSeleccionar.GetRoomSeleccionadosConRectaguloYFiltros(app.ActiveUIDocument);
            ListaRooms = modificarParametrosRoom.roomSeleccionar.GetRoomNivelActual(commandData.Application.ActiveUIDocument);
            //dibujar
            modificarParametrosRoom.DibujarLargosMin(ListaRooms);

           return Result.Succeeded;

        }

    }


    [TransactionAttribute(TransactionMode.Manual)]
    public class BorrarLargoMin : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            ServicioaCambiarLargoMinV2 modificarParametrosRoom = new ServicioaCambiarLargoMinV2(commandData);

            //seleccionar
            IEnumerable<Element> ListaRooms = new List<Element>();
           // ListaRooms = modificarParametrosRoom.roomSeleccionar.GetRoomSeleccionadosConRectaguloYFiltros(commandData.Application.ActiveUIDocument);
            ListaRooms = modificarParametrosRoom.roomSeleccionar.GetRoomNivelActual(commandData.Application.ActiveUIDocument);
            //borrar elemtos
            modificarParametrosRoom.BorrarLargosMin(ListaRooms);
            return Result.Succeeded;

        }

    }

}
