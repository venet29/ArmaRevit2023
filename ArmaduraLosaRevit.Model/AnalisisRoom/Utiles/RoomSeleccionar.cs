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

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Utiles
{
    public class RoomSeleccionar
    {
        public bool Isok { get; set; }
        public IEnumerable<Element> GetRoomSeleccionados1Room(UIDocument uidoc)
        {
            Isok = false;
            IList<Element> pickedElements = new List<Element>();
            //clase filtro para selecccionar
            ISelectionFilter f = new RoomSelectionFilte();
            //   pickedElement = uidoc.Selection.PickObject(ObjectType.Element,selFilter, "Selecionar Room con rectangulo");


            Reference referen;

            try
            {

                referen = uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar Room ");
            }
            catch (Exception)
            {

                return pickedElements; //SALIMOS DE LA MACRO
            }

            if (referen == null) return pickedElements;
            Element roomElem = uidoc.Document.GetElement(referen);
            // si refere3ncia es null salir
            if (roomElem == null) return pickedElements;
            if (!(roomElem is Room)) { return pickedElements; }


            pickedElements.Add(roomElem);

            Isok = true;
            return pickedElements;
        }

        /// <summary>
        /// metodod para seleccionar room con mouse y filtros
        /// </summary>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        public IEnumerable<Element> GetRoomSeleccionadosConRectaguloYFiltros(UIDocument uidoc)
        {
            Isok = true;
            IList<Element> pickedElements = new List<Element>();
            //clase filtro para selecccionar
            ISelectionFilter selFilter = new RoomSelectionFilte();
            try
            {
  
                pickedElements = uidoc.Selection.PickElementsByRectangle(selFilter, "Selecionar Room con rectangulo");
            }
            catch (Exception)
            {

                Isok = false; ;
            }

            return pickedElements;
        }



        /// <summary>
        /// Obtienen los elemntos de nivel actual
        /// </summary>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        public IEnumerable<Element> GetRoomNivelActual(UIDocument uidoc)
        {
            Document doc = uidoc.Document;
            Isok = true;
            View view = uidoc.Document.ActiveView;
            if (view.GenLevel == null)
            { 
                Isok = false;
                Util.ErrorMsg($"Error GetRoomNivelActual -> view.GenLevel == null");
                return new List<Element>();
            }
            // obtiene todos los room del nivel analisado
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(SpatialElement)).WhereElementIsNotElementType();
            IEnumerable<Element> rooms = collector.Where<Element>(e => e is Room && e.LevelId == view.GenLevel.Id && e.Location != null);


            return rooms;
        }

        public static IEnumerable<Room> GetRoomNivelActual2(UIDocument uidoc)
        {
            Document doc = uidoc.Document;

            View view = uidoc.Document.ActiveView;
            if (view.GenLevel == null)
            {
                Util.ErrorMsg($"Error GetRoomNivelActual2 -> view.GenLevel == null");
                return new List<Room>();
            }

            // obtiene todos los room del nivel analisado
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(SpatialElement));
            IEnumerable<Room> rooms = collector.Where<Element>(e => e is Room && e.LevelId == view.GenLevel.Id && e.Location != null).Select(r => r as Room);


            return rooms;
        }
        public static Room GetRoomConPtoNivelActual(UIDocument uidoc, XYZ ptoSeleccion)
        {
            List<Room> ListaRoomNivelActual = GetRoomNivelActual2(uidoc).ToList();

            Room RoomSeleccionado = ListaRoomNivelActual.Where(r => r.IsPointInRoom(ptoSeleccion)).FirstOrDefault();

            return RoomSeleccionado;
        }
    }
}
