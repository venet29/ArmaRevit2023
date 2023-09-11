using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;
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
    public class CambiarDatosRoomConSelec : CambiarDatosRoomBase, ICambiarDatosRoom
    {

#pragma warning disable CS0108 // 'CambiarDatosRoomConSelec.ListaRoom' hides inherited member 'CambiarDatosRoomBase.ListaRoom'. Use the new keyword if hiding was intended.
        private List<Element> ListaRoom;
#pragma warning restore CS0108 // 'CambiarDatosRoomConSelec.ListaRoom' hides inherited member 'CambiarDatosRoomBase.ListaRoom'. Use the new keyword if hiding was intended.

        public CambiarDatosRoomConSelec(ExternalCommandData commandData, RoomCuantiaDatosDto roomCuantiaDatosDto, bool isTest = false)
             : base(commandData, roomCuantiaDatosDto)
        {
 
            ListaRoom = new List<Element>();
        }

        public void Ejecutar()
        {
            SeleccionarRoom();
            CambiarCuantiaRoom(ListaRoom);

            TaskDialog.Show("Actualizacion Cuantias", "Se pudo actualizar correctamente las cuantias de " + ListaRoom.Count +" Room");
        }

        private void SeleccionarRoom()
        {
            Util.CambiarStadoSectionBo3d(_uidoc, ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR, false);


            ReferenciaRoomHandler administrador_ReferenciaRoom = new ReferenciaRoomHandler(_uiapp);

            // calcula todos los datos de la room, datos room, largos minimos, suples y tipo de losa
            administrador_ReferenciaRoom.ReferenciaRoomListas.GetLista_RefereciaRoom("SelectConMouse");

            if (administrador_ReferenciaRoom == null) return;

            foreach (var item in administrador_ReferenciaRoom.ReferenciaRoomListas.Lista_RefereciaRoom)
            {
                ListaRoom.Add(item.RefereciaRoomDatos.Room1);
            }

    
        }


    }
}
