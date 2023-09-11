using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;

namespace ArmaduraLosaRevit.Model.AnalisisRoom
{
    public class RoomFunciones
    {

        //  static double _tolerance = Util.MmToFoot(1.2);

        //contructores

        //metodos


        public static List<BoundarySegmentHandler> ListaRoomDatosGenerales(UIApplication _uiapp , string tipoSelecion)
        {
            
            Document doc = _uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = _uiapp.ActiveUIDocument;
            //View vista = doc.ActiveView;
            //FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(SpatialElement));
            //IEnumerable<Element> Listarooms = collector.Where<Element>(e => e is Room && e.LevelId == vista.GenLevel.Id);

            //lista con los room del nivel actual de losa
            //crear objeto seleccionar
            RoomSeleccionar roomSeleccionar = new RoomSeleccionar();
            IEnumerable<Element> ListaRooms = new List<Element>();


            if (tipoSelecion == "SelectConMouse")
            {
                ListaRooms = roomSeleccionar.GetRoomSeleccionadosConRectaguloYFiltros(uidoc);
            }

            else if (tipoSelecion == "SelectAllTodosLosNiveles")
            {
                ListaRooms = SeleccionarRoom.SeleccionarRoomNivel(doc, doc.ActiveView.GenLevel.Id);
            }
            else if (tipoSelecion == "GetSelectionAllNivelActual") //selecicona todos los del planta analizados
            {
                ListaRooms = roomSeleccionar.GetRoomNivelActual(uidoc);
            }


            //lista de las room, se entrega como resultado
            List<BoundarySegmentHandler> listaRoom = new List<BoundarySegmentHandler>();

            foreach (Room room in ListaRooms)
            {
                PruebaBorrar(room);
                // obtiene puntos del poligono de room
                List<XYZ> boundary_pts = RoomFuncionesPuntos.ListRoomVertice(room);
                ParameterSet pars = room.Parameters;
                //2
                double aux_angle_ = 0.0;
                double.TryParse(ParameterUtil.FindValueParaByName(pars, "Angulo", room.Document), out aux_angle_);
                double Angle_pelotaLosa1_ = Util.RadianeToGrados(aux_angle_);


                IList<IList<BoundarySegment>> boundaries = room.GetBoundarySegments(new SpatialElementBoundaryOptions()); // 2012

                int n = 0;

                if (null != boundaries)
                {
                    //n = boundaries.Size; // 2011
                    n = boundaries.Count; // 2012
                }

                if (0 < n)
                {

                    //selcciona la losa que contiene el room
                    Floor FloorDeRoomAnalizado = RoomFuncionesSeleccionarLosa.ObtenerFloor_ConUNRoom(room);
                    if (FloorDeRoomAnalizado == null) continue;

                    //foreach( BoundarySegmentArray b in boundaries ) // 2011
                    BoundarySegmentRoomsGeom boundarySegmentRoomsGeom = new BoundarySegmentRoomsGeom(doc);
                    // contiene una lista que tiene la lista de los boundery de los room, puede tener mas de una lista, por presencia de shaft, openin o muro dentro del room

                    foreach (IList<BoundarySegment> b in boundaries) // 2012
                    {
                        // analiza una lista que contienen los bordes de un boundary
                        BoundarySegmentRoomsGeom boundarySegmentRoomsGeom_internos = new BoundarySegmentRoomsGeom(b, _uiapp, room.Name, FloorDeRoomAnalizado);

                        //reorden los lados del poligonos para juntos los similes
                        //newlistaBS_aux.Reordendar();
                        boundarySegmentRoomsGeom.ListaWrapperBoundarySegment.AddRange(boundarySegmentRoomsGeom_internos.ListaWrapperBoundarySegment);
                    }

                    //crear nuevo objeto para alamcenar en lista
                    BoundarySegmentHandler boundarySegmentHandler = new BoundarySegmentHandler(room, boundaries, boundarySegmentRoomsGeom);
                    //agregar room al la lista
                    listaRoom.Add(boundarySegmentHandler);

                }//fin if
            }//fin foreach

            return listaRoom;
        }

        private static void PruebaBorrar(Room room)
        {
            var geos = room.GetGeometricObjects2();

            foreach (GeometryObject geo in geos)
            {
                Solid sol = geo as Solid;
                if (sol != null && sol.Faces.Size > 0)
                {
                    var cara = sol.Faces.get_Item(0);
                    BoundingBoxUV bbv = cara.GetBoundingBox();
                }

            }
        }





        /// <summary>
        /// a struct to store the value of property Area and Department name
        /// </summary>
        public struct DepartmentInfo
        {
            string m_departmentName;
            int m_roomAmount;
            double m_departmentAreaValue;
            // List<XYZ> roomsPoligonoLosa ;
            /// <summary>
            /// the name of department
            /// </summary>
            public string DepartmentName
            {
                get
                {
                    return m_departmentName;
                }
            }

            ///// <summary>
            // /// get 
            // /// </summary>
            // public List<XYZ> RoomsPoligonoLosa
            // {
            //     get
            //     {
            //         return roomsPoligonoLosa;
            //     }
            //     set
            //     {
            //         roomsPoligonoLosa =value;
            //     }
            // }

            /// <summary>
            /// get the amount of rooms in the department
            /// </summary>
            public int RoomsAmount
            {
                get
                {
                    return m_roomAmount;
                }
            }

            /// <summary>
            /// the total area of the rooms in department
            /// </summary>
            public double DepartmentAreaValue
            {
                get
                {
                    return m_departmentAreaValue;
                }
            }

            /// <summary>
            /// constructor
            /// </summary>
            public DepartmentInfo(string departmentName, int roomAmount, double areaValue)
            {
                m_departmentName = departmentName;
                m_roomAmount = roomAmount;
                m_departmentAreaValue = areaValue;
                //List<XYZ> roomsPoligonoLosa = new List<XYZ>();
            }
        }


        //2) obtiene LA POSICION DE LA PELOTA LOSA 
        static public XYZ ptoLocationRoom(Room room)
        {


            Location loc = room.Location;
            LocationPoint lp = loc as LocationPoint;
            XYZ p = (null == lp) ? XYZ.Zero : lp.Point;
            return p;
        }


        // 4) actualizar parametros de elemetos
        static public void AgregarParametrosRoom(Document doc, Element room, string name, string valor)
        {


            try
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Create AgregraParametros-NH");
                    ParameterUtil.SetParaInt(room, name, valor);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                TaskDialog.Show("Error", message);

            }


            //throw new NotImplementedException();
        }


        static public void AgregarParametrosRoom(Document doc, Element room, string name, double valor)
        {


            try
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Create AgregraParametros-NH");
                    ParameterUtil.SetParaInt(room, name, valor);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                TaskDialog.Show("Error", message);

            }


            //throw new NotImplementedException();
        }

        /// <summary>
        /// get the room property and Department property according the property name
        /// </summary>
        /// <param name="room">a instance of room class</param>
        /// <param name="paraEnum">the property name</param>
        static public string GetProperty(Room room, BuiltInParameter paraEnum)
        {
            string propertyValue = null;  //the value of parameter 

            // get the parameter via the parameterId
            Parameter param = room.get_Parameter(paraEnum);
            if (null == param)
            {
                return "";
            }
            // get the parameter's storage type
            StorageType storageType = param.StorageType;
            switch (storageType)
            {
                case StorageType.Integer:
                    int iVal = param.AsInteger();
                    propertyValue = iVal.ToString();
                    break;
                case StorageType.String:
                    String stringVal = param.AsString();
                    propertyValue = stringVal;
                    break;
                case StorageType.Double:
                    Double dVal = param.AsDouble();
                    dVal = Math.Round(dVal, 2);
                    propertyValue = dVal.ToString();
                    break;
                default:
                    break;
            }
            return propertyValue;
        }







    }

}
