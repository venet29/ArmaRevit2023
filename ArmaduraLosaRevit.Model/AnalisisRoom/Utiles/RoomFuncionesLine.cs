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
    public class RoomFuncionesLine
    {

        static double _tolerance = Util.MmToFoot(1.2);
        public static XYZ posicionRoom;
        //contructores

        /// <summary>
        /// devuelve lista con curvas o BoundarySegment desplazada  offset hacia el interior del room
        /// </summary>
        /// <param name="room"></param>
        /// <param name="offset"> cantiad desplazada</param>
        /// <param name="ptoCentro"> punto central del room</param>
        /// <returns></returns>
        static public List<Line> GetBoundaryToLine(Room room)
        {
            List<Line> edgeLines = new List<Line>();
            SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();
            IList<IList<BoundarySegment>> boundary = room.GetBoundarySegments(opt);

            
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
                        edgeLines.Add((Line)seg.GetCurve());
                    }
                    break;
                }
            }
            return edgeLines;
        }

        static public List<Line> GetListaToLine(List<XYZ> lista)
        {
            List<Line> edgeLinesOffset = new List<Line>();
            int n = lista.Count;

            if (1 > n)
            {
                Debug.Print("Lista contains no loops");
            }
            else
            {

                for (int i = 0; i < n - 1; i++)
                {
                    edgeLinesOffset.Add(Line.CreateBound(lista[i], lista[i + 1]));
                }
                if (lista[0].DistanceTo(lista[n - 1]) > 5)
                    edgeLinesOffset.Add(Line.CreateBound(lista[0], lista[n - 1]));
            }
            return edgeLinesOffset;
        }





  

    }

}
