using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.Seleccionar;


namespace ArmaduraLosaRevit.Model.AnalisisRoom.Utiles
{
    public class RoomFuncionesPuntos
    {

        static double _tolerance = Util.MmToFoot(1.2);
        public static XYZ posicionRoom;
        //contructores

        //metodos





        /// <summary>
        /// revuelve una lista con los puntos del contorno del poligono de losa
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        static public List<XYZ> ListRoomVertice(Room room)
        {
            SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();

            string nr = room.Number;
            string name = room.Name;
            double area = room.Area;

            posicionRoom = RoomFunciones.ptoLocationRoom(room);
            //obtienen coordenadas de los BoundingBoxXYZ
            BoundingBoxXYZ bb = room.get_BoundingBox(null);

            //obtiene los bordes del room
            IList<IList<BoundarySegment>> boundary = room.GetBoundarySegments(opt);

            //obtienen los puntos de los bordes
            List<XYZ> boundary_pts = GetBoundaryPoints(boundary);

            //listas pjntos del bordes 
            return boundary_pts;
        }


        /// <summary>
        /// Return room boundary points retrieved 
        /// from the room boundary segments. 
        /// </summary>
        static List<XYZ> GetBoundaryPoints(IList<IList<BoundarySegment>> boundary)
        {
            List<XYZ> pts = new List<XYZ>();

            int n = boundary.Count;

            if (1 > n)
            {
                Debug.Print("Boundary contains no loops");
            }
            else
            {
                if (1 < n)
                {
                    Debug.Print("Boundary contains {0} loop{1}; " + "skipping all but first.", n, Util.PluralSuffix(n));
                }

                foreach (IList<BoundarySegment> loop in boundary)
                {
                    foreach (BoundarySegment seg in loop)
                    {

                        Curve c = seg.GetCurve();
                        AddNewPoints(pts, c.Tessellate());
                    }

                    double z = pts[0].Z;

                    foreach (XYZ p in pts)
                    {
                        Debug.Assert(Util.IsEqual(p.Z, z, _tolerance), "expected horizontal room boundary");
                    }

                    // Break after first loop, which is hopefully 
                    // the exterior one, and hopefully the only one.
                    // Todo: add better handling for more complex cases.

                    break;
                }
            }
            return pts;
        }


     

        /// <summary>
        /// Add new points to the list.
        /// Skip the first new point if it equals the last 
        /// old existing one. Actually, we can test all points 
        /// and always ignore very close consecutive ones.
        /// </summary>
        static void AddNewPoints(IList<XYZ> pts, IList<XYZ> newpts)
        {
            foreach (XYZ p in newpts)
            {
                if (0 == pts.Count || !Util.IsEqual(p, pts.Last(), _tolerance))
                {
                    pts.Add(p);
                }
            }
        }



  

    }

}
