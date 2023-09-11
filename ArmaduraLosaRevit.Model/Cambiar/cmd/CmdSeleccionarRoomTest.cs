
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using ArmaduraLosaRevit.Model.AnalisisRoom.Servicios;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.Cambiar.CambiarEspesor.cmd
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class SeleccionarRoomInFloor : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData,ref string messages, ElementSet elements)
        {

            UIApplication app = commandData.Application;
            Document _doc = app.ActiveUIDocument.Document;
            Level _Level = _doc.ActiveView.GenLevel;

            SeleccionarLosaConPto SeleccionarLosaConPto = new SeleccionarLosaConPto(_doc);

            #region Solopara ver los producto cruz y producto pto

            XYZ P1 = new XYZ(0, 0, 1);
            XYZ P2 = new XYZ(0, 0, -1);

            var pr1 = P1.CrossProduct(P2);
            var pr2 = P1.CrossProduct(P1);
            var pr3 = P1.DotProduct(P2);
            var pr4 = P1.DotProduct(P1); 
            #endregion


            //crear losa
            int idPathd = 1111503;
            ElementId RoomElem = new ElementId(idPathd);
            Floor floor = (Floor)_doc.GetElement(RoomElem);

            //crear room fuera
            int idRoomFueraLosa = 1575453;
            ElementId ELRoomFueraLosa = new ElementId(idRoomFueraLosa);
            Room roomFuera = (Room)_doc.GetElement(ELRoomFueraLosa);

            XYZ ptobuscadoOut = ((LocationPoint)roomFuera.Location).Point;
            PuntoEnLosa resp1=  SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(ptobuscadoOut, floor);



            //crear room dentro
            int idRoomDentroLosa = 1167436;
            ElementId ELRoomDentrolLosa = new ElementId(idRoomDentroLosa);
            Room roomDentro = (Room)_doc.GetElement(ELRoomDentrolLosa);
            
            XYZ ptoBuscadaIN = ((LocationPoint)roomDentro.Location).Point;
            PuntoEnLosa resp2 = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(ptoBuscadaIN, floor);


            //pto null
            PuntoEnLosa respPtoNull = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(null, floor);
            //pto cero
            PuntoEnLosa respPtoCERO = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal( new XYZ(0,0,0) , floor);

            //losa floor
            PuntoEnLosa resptaFLOOR = SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(ptobuscadoOut, null);
            Debug.Assert(resptaFLOOR == PuntoEnLosa.losaNull);


            List< Room> listaDeRoomEnLosa=SeleccionarRoom.SeleccionarRoomNivelYEnLosa(_doc,  floor);
            Debug.Assert(listaDeRoomEnLosa.Count == 16);
            //Iguañ a 16  == listaDeRoomEnLosa.count

            return Result.Succeeded;

        }

    }


}
