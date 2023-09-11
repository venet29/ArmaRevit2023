using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.GEOM
{
     public  class GeometrySupport
    {
        //0)PROPIEDADES

        /// <summary>
        /// a list to store the point
        /// </summary>
        private List<XYZ> m_points = new List<XYZ>();

        /// <summary>
        /// a list to store the edges 
        /// </summary>
        protected List<Line> m_edges = new List<Line>();


        //1) CONSTRUCTOR




        public GeometrySupport(List<XYZ> m_points, List<Line> m_edges)
        {
            this.m_points = m_points;
            this.m_edges = m_edges;
        }




        //2) METODOS

        /// <summary>
        /// Offset the points of the swept profile to make the points inside swept profile
        /// </summary>
        /// <param name="offset">Indicate how long to offset on two directions</param>
        /// <returns>The offset points</returns>
        public List<XYZ> OffsetPoints(double offset)
        {
            // Initialize the offset point list.
            List<XYZ> points = new List<XYZ>();

            // Get all points of the swept profile, and offset it in two related directions
            foreach (XYZ point in m_points)
            {
                // Get two related directions
                List<XYZ> directions = GetRelatedVectors(point);
                XYZ firstDir = directions[0];
                XYZ secondDir = directions[1];

                // offset the point in two directions
                XYZ movedPoint = GeomUtil.OffsetPoint(point, firstDir, offset);
                movedPoint = GeomUtil.OffsetPoint(movedPoint, secondDir, offset);

                // add the offset point into the array
                points.Add(movedPoint);
            }

            return points;
        }

        public List<Line> OffsetListaLineas(double offset)
        {
            // Initialize the offset point list.
            List<Line> ListaLine = new List<Line>();

            // Get all points of the swept profile, and offset it in two related directions
            foreach (XYZ Line in m_points)
            {
                //// Get two related directions
                //List<XYZ> directions = GetRelatedVectors(point);
                //XYZ firstDir = directions[0];
                //XYZ secondDir = directions[1];

                //// offset the point in two directions
                //XYZ movedPoint = GeomUtil.OffsetPoint(point, firstDir, offset);
                //movedPoint = GeomUtil.OffsetPoint(movedPoint, secondDir, offset);

                //Line auxLine ;

                //// add the offset point into the array
                //ListaLine.Add(Line);
            }

            return ListaLine;
        }


        /// <summary>
        /// Get two vectors, which indicate some edge direction which contain given point, 
        /// set the given point as the start point, the other end point of the edge as end
        /// </summary>
        /// <param name="point">A point of the swept profile</param>
        /// <returns>Two vectors indicate edge direction</returns>
        protected List<XYZ> GetRelatedVectors(XYZ point)
        {
            // Initialize the return vector list.
            List<XYZ> vectors = new List<XYZ>();

            // Get all the edges which contain this point.
            // And get the vector from this point to another point
            foreach (Line line in m_edges)
            {
                if (GeomUtil.IsEqual(point, line.GetEndPoint(0)))
                {
                    XYZ vector = GeomUtil.SubXYZ(line.GetEndPoint(1), line.GetEndPoint(0));
                    vectors.Add(vector);
                }
                if (GeomUtil.IsEqual(point, line.GetEndPoint(1)))
                {
                    XYZ vector = GeomUtil.SubXYZ(line.GetEndPoint(0), line.GetEndPoint(1));
                    vectors.Add(vector);
                }
            }

            // only two vectors(directions) should be found
            if (2 != vectors.Count)
            {
                throw new Exception("A point on swept profile should have only two directions.");
            }

            return vectors;
        }

    }
}
