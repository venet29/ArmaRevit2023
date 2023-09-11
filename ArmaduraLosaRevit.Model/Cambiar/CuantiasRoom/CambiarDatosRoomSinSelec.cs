using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cambiar.CuantiasRoom
{

 
    public class CambiarDatosRoomSinSelec: CambiarDatosRoomBase, ICambiarDatosRoom
    {

#pragma warning disable CS0108 // 'CambiarDatosRoomSinSelec.ListaRoom' hides inherited member 'CambiarDatosRoomBase.ListaRoom'. Use the new keyword if hiding was intended.
        private List<Element> ListaRoom = new List<Element>();
#pragma warning restore CS0108 // 'CambiarDatosRoomSinSelec.ListaRoom' hides inherited member 'CambiarDatosRoomBase.ListaRoom'. Use the new keyword if hiding was intended.
       

        public CambiarDatosRoomSinSelec(ExternalCommandData commandData, RoomCuantiaDatosDto roomCuantiaDatosDto, List<Element> Lista,bool isTest = false)
            :base( commandData,  roomCuantiaDatosDto)
        {

            ListaRoom = Lista;
        }

        public void Ejecutar()
        {
            
            CambiarCuantiaRoom(ListaRoom);

            TaskDialog.Show("Actualizacion Cuantias", "Se pudo actualizar correctamente las cuantias de " + ListaRoom.Count +" Room");
        }


    }
}
