using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom
{
    /// <summary>
    /// DEFINICIO:
    /// clase que se utiliza para enfierra automatico de barras superiore
    ///
    /// </summary>
    public class BoundarySegmentHandler
    {
        #region 0) propiedades
        private Room room;

        // almacena todos los refuerzon que contendra el room// refuezo, cabeza muro, tipo viga( estribo+barras laterales), borde libre 
        public BoundarySegmentRoomsGeom newlistaBS { get; set; }
        //contiene los elemtos del borde del perimetro del room 
        public IList<IList<BoundarySegment>> boundaries { get; set; }

        #endregion
        #region 1)constructor
        public BoundarySegmentHandler(Room room, IList<IList<BoundarySegment>> boundaries, BoundarySegmentRoomsGeom newlistaBS)
        {
            this.room = room;
            this.boundaries = boundaries;
            this.newlistaBS = newlistaBS;
        }

        #endregion
    }
}
