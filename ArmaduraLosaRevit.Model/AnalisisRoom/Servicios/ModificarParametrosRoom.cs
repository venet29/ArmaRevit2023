using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Servicios
{
    public class ModificarParametrosRoom
    {
        public IEnumerable<Element> ListaRooms = new List<Element>();
        private RoomSeleccionar roomSeleccionar;
        public UIDocument uidoc;
        private Document _doc;

        public ModificarParametrosRoom(UIDocument uidoc)
        {
            roomSeleccionar = new RoomSeleccionar();
            this.uidoc = uidoc;
            this._doc = uidoc.Document;
        }


        public void cambiarDireccionPrincipal()
        {    
            //ListaRooms = roomSeleccionar.GetRoomSeleccionadosConRectaguloYFiltros(uidoc);
            ListaRooms = roomSeleccionar.GetRoomSeleccionados1Room(uidoc);
            foreach (Room room in ListaRooms)
            {
                ReferenciaRoom newRegistroLosa = new ReferenciaRoom(_doc,room);
                //obtiene largo minimo
                newRegistroLosa.RefereciaRoomDatos.CambiarDireccionesPrincipal(uidoc.Document);
            }
        }

        void RecalcularLargosMinimo()
        {

            ListaRooms = roomSeleccionar.GetRoomSeleccionadosConRectaguloYFiltros(uidoc);

            foreach (Room room in ListaRooms)
            {
                ReferenciaRoom newRegistroLosa = new ReferenciaRoom(_doc,room);
                newRegistroLosa.RefereciaRoomDatos.GetLargoMin();
            }

        }





    }
}
