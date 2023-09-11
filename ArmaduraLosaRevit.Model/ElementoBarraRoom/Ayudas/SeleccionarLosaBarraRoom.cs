using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
    public interface ISeleccionarLosaBarraRoom
    {
        XYZ PtoConMouseEnlosa1 { get; set; }
        Room RoomSelecionado1 { get; set; }
        Floor LosaSeleccionada1 { get; set; }
        Result selecconarUNRoom();
        bool ObtenerUNRoom();
    }
    public class SeleccionarLosaBarraRoom : ISeleccionarLosaBarraRoom
    {
        private UIDocument _uidoc;
        private Document _doc;
        private string TipoBarra;
#pragma warning disable CS0169 // The field 'SeleccionarLosaBarraRoom.rooms' is never used
        private List<Room> rooms;
#pragma warning restore CS0169 // The field 'SeleccionarLosaBarraRoom.rooms' is never used

        public Level LevelLosa1 { get; set; }
        public Level LevelLosa2 { get; set; }

        Reference Room1 = null;
        Reference Room2 = null;

        public Result statusbarra { get; private set; }
        public string message { get; private set; }

        public XYZ PtoConMouseEnlosa1 { get; set; }
        public XYZ PtoConMouseEnlosa2 { get; set; }
        public double AnguloBordeRoomYSegundoPtoMouseGrado { get; set; }
        public Room RoomSelecionado1 { get; set; }
        public Room RoomSelecionado2 { get; set; }

        public Floor LosaSeleccionada1 { get; set; }
        public Floor LosaSeleccionada2 { get; set; }

        public SeleccionarLosaBarraRoom(UIDocument uidoc, SolicitudBarraDTO solicitudBarraDTO = null)
        {
            this._uidoc = uidoc;
            this._doc = _uidoc.Document;
            if (solicitudBarraDTO != null)
                this.TipoBarra = solicitudBarraDTO.TipoBarra;
        }

        public SeleccionarLosaBarraRoom() { }
        public Result selecconarUNRoom()
        {
            try
            {

                // SelecionarRoom(ref Room1);

                //  var Seleccroom = _doc.GetElement(Room1.ElementId) as Room;
                //seleciona un losa y lo almacen en 'r_fx'
                if (SelecionarLOsa(ref Room1) != Result.Succeeded) return Result.Cancelled;
                //obtiene una referencia floor con la referencia r
                LosaSeleccionada1 = _doc.GetElement(Room1.ElementId) as Floor;            //obtiene el nivel del la losa
                LevelLosa1 = _doc.GetElement(LosaSeleccionada1.LevelId) as Level;
                //obtiene el pto de seleccion con el mouse sobre la losa
                PtoConMouseEnlosa1 = new XYZ(Room1.GlobalPoint.X, Room1.GlobalPoint.Y, LevelLosa1.ProjectElevation);
                //    pt_selecUV_r_fx = r_fx.UVPoint;

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error 'selecconarUNRoom'  ex:{ex.Message}");
                return Result.Failed; ;
            }

        }
        public Result selecconarDOSRoom()
        {
            try
            {
                selecconarUNRoom();

                if (SelecionarLOsa(ref Room2) != Result.Succeeded) return Result.Failed;

                //obtiene una referencia floor con la referencia r
                LosaSeleccionada2 = _doc.GetElement(Room2.ElementId) as Floor;            //obtiene el nivel del la losa
                LevelLosa2 = _doc.GetElement(LosaSeleccionada2.LevelId) as Level;

                if (Math.Abs(LevelLosa1.ProjectElevation - LevelLosa2.ProjectElevation) > Util.CmToFoot(1))
                {
                    Util.ErrorMsg($"niveles de losas seleccinadas estan en diferente nivel\n Losa1  nivel:{LevelLosa1.Name}  elevacion:{LevelLosa1.ProjectElevation}\n Losa2  nivel:{LevelLosa2.Name}  elevacion:{LevelLosa2.ProjectElevation}");
                    return Result.Cancelled;
                }
                //obtiene el pto de seleccion con el mouse sobre la losa
                PtoConMouseEnlosa2 = new XYZ(Room2.GlobalPoint.X, Room2.GlobalPoint.Y, LevelLosa2.ProjectElevation);
            }
            catch (Exception)
            {

                return Result.Failed;
            }
            //    pt_selecUV_r_fx = r_fx.UVPoint;
            return Result.Succeeded;

        }

        private Result SelecionarLOsa(ref Reference r)
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new JtElementsOfClassSelectionFilter<Floor>();
            try
            {
                r = _uidoc.Selection.PickObject(ObjectType.Face, f, "Please select a planar face to define work plane");
                // sirefere3ncia es null salir
                if (r == null)
                {
                    message += "Ningun elemtos seleccionado";
                    return statusbarra = Result.Cancelled;
                }
            }
            catch (Exception)
            {
                message += "Ningun elemtos seleccioando";
                return statusbarra = Result.Cancelled;
            }
            return statusbarra = Result.Succeeded;
        }

        private Result SelecionarRoom(ref Reference r)
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new JtElementsOfClassSelectionFilter<Room>();
            try
            {
                r = _uidoc.Selection.PickObject(ObjectType.Element, f, "Please select a planar face to define work plane");
                // sirefere3ncia es null salir
                if (r == null)
                {
                    message += "Ningun elemtos seleccionado";
                    return statusbarra = Result.Cancelled;
                }
            }
            catch (Exception)
            {
                message += "Ningun elemtos seleccioando";
                return statusbarra = Result.Cancelled;
            }
            return statusbarra = Result.Succeeded;
        }


        public void AsignarUnRoom(XYZ pointMouse, Floor floor)
        {
            if (pointMouse.IsZeroLength() && floor == null) return;
            //obtiene una referencia floor con la referencia r
            LosaSeleccionada1 = floor;            //obtiene el nivel del la losa
            LevelLosa1 = _doc.GetElement(LosaSeleccionada1.LevelId) as Level;
            PtoConMouseEnlosa1 = pointMouse;
        }

        public Result AsignarDOSRoom(XYZ pointMouse1, XYZ pointMouse2, Floor floor)
        {
            try
            {
                AsignarUnRoom(pointMouse1, floor);
                PtoConMouseEnlosa2 = pointMouse2;
            }
            catch (Exception)
            {

                return Result.Cancelled;
            }


            return Result.Succeeded;
        }


        public bool ObtenerUNRoom()
        {
            try
            {
                if (PtoConMouseEnlosa1 == null) return false;
                XYZ pointSobreLosa1cm = new XYZ(PtoConMouseEnlosa1.X, PtoConMouseEnlosa1.Y, PtoConMouseEnlosa1.Z + 1);
                RoomSelecionado1 = _doc.GetRoomAtPoint(pointSobreLosa1cm);

                
                if (RoomSelecionado1 == null)
                {

                    var listaRoomEnLosa = SeleccionarRoom.SeleccionarRoomNivelYEnLosa(_doc, LosaSeleccionada1);
                    RoomSelecionado1 = listaRoomEnLosa.Where(c => c.IsPointInRoom(pointSobreLosa1cm)).FirstOrDefault();
                    if (RoomSelecionado1 == null) return false;
                }
                if (RoomSelecionado1 == null) return false;

                string nombreLosa= RoomSelecionado1.ObtenerNumero_Losa();
                if (nombreLosa=="")
                {
                    var listaRoomEnLosa = SeleccionarRoom.SeleccionarRoomNivelYEnLosa(_doc, LosaSeleccionada1);
                    RoomSelecionado1 = listaRoomEnLosa.Where(c => c.IsPointInRoom(pointSobreLosa1cm)).FirstOrDefault();
                    if (RoomSelecionado1 == null) return false;
                }
                // RoomSelecionado1 = rooms.Where(rm => rm.IsPointInRoom(pointSobreLosa1cm)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener room en losa \n ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public void ObtenerDOSRoom()
        {
            ObtenerUNRoom();
            XYZ pointSobreLosa1cm = new XYZ(PtoConMouseEnlosa2.X, PtoConMouseEnlosa2.Y, PtoConMouseEnlosa2.Z + 1);
            RoomSelecionado2 = _doc.GetRoomAtPoint(pointSobreLosa1cm);
        }

        private List<Room> ObtenerListaRoom()
        {
            FilteredElementCollector collector = new FilteredElementCollector(_doc).OfClass(typeof(SpatialElement));
            return collector.Where<Element>(e => e is Room && e.LevelId == LosaSeleccionada1.LevelId).Select(rom => (Room)rom).ToList();
        }


    }
}
