using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarRoom
    {
        public static List<Room> ListaroomsInFloor { get;  set; }
        public static Floor _floorAnalizado { get;  set; }

        public static IEnumerable<Element> SeleccionarRoomNivel(Document doc, ElementId nivelId)
        {
            //View vista = doc.ActiveView;
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(SpatialElement));
            IEnumerable<Element> Listarooms = collector.Where<Element>(e => e is Room && e.LevelId == nivelId);

            return Listarooms;
        }


        public static List<Room> SeleccionarRoomNivelYEnLosa(Document doc, Floor floor)
        {
            //if (_floorAnalizado == floor && ListaroomsInFloor.Count > 0)
            //{
            //    Util.InfoMsg($"cantidad room preseleccionado  :{ListaroomsInFloor.Count}");
            //    return ListaroomsInFloor;
            //}

            _floorAnalizado = floor;

            Level levelLOsa = doc.GetElement(floor.LevelId) as Level;

            SeleccionarLosaConPto SeleccionarLosaConPto = new SeleccionarLosaConPto(doc);

            //room en nivel
            List<Element> Listarooms = SeleccionarRoomNivel(doc, levelLOsa.Id).ToList();

            //rom dentro de losa
          ListaroomsInFloor = Listarooms.Cast<Room>()
                                               .Where(cc => cc.Location != null && SeleccionarLosaConPto.EjecturaEstaPtoDentroDeLosaHorizontal(((LocationPoint)cc.Location).Point + (new XYZ(0, 0, 1)), floor) == PuntoEnLosa.PtoDentroLosa)
                                               .ToList();

            return ListaroomsInFloor;
        }
    }
}
