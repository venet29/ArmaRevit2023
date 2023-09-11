using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom
{
    public class BoundarySegmentCoordenadas
    {
        private int diametroBArra;

        #region 0) propiedades
        //private BoundarySegmentNH boundarySegmentNH;
        public XYZ startPont_offsetIntRoom { get; set; }
        public XYZ EndPoint_offseIntRoom { get; set; }


        //pto que guarda la interseccion de la linea aux 
        // con el borde del room en el diseño de barras manual
        public XYZ pointIntersccion { get; set; }
        //pto curve
        public XYZ StartPoint { get; set; }
        public XYZ EndPoint { get; set; }



        //pto para suples
        public XYZ StartPointSuples { get; set; }
        public XYZ EndPointSuples { get; set; }

        public XYZ desface { get; set; }

        //vectores perpendicuales
        public XYZ VectorInteriorRoom { get; set; } // vector que apunta hacia el interior del room
        public XYZ VectorExteriorRoom { get; set; } // vector que apunta hacia el exterior del room

        public double offSuperiorhaciaBajoLosa { get; set; }
        public double offInferiorHaciaArribaLosa { get; set; }
        #endregion

        #region 1) constructor
        public BoundarySegmentCoordenadas(WrapperBoundarySegment boundarySegmentNH, double offsetMoverbarra)
        {
           // this.boundarySegmentNH = boundarySegmentNH;
            this.StartPoint = boundarySegmentNH.boundarySegment.GetCurve().GetEndPoint(0);
            this.EndPoint = boundarySegmentNH.boundarySegment.GetCurve().GetEndPoint(1);
            desface = new XYZ(0, 0, 0);
            offSuperiorhaciaBajoLosa = Util.CmToFoot(2); // 3 pulgadas
            offInferiorHaciaArribaLosa = Util.CmToFoot(2); // 3 pulgadas


            GetOffset(offsetMoverbarra);
            diametroBArra = 10; 
        }
        #endregion

        #region 2) metodos
        /// <summary>
        /// obtienen los puntos offset hacia el interior de poligono
        /// </summary>
        public void GetOffset(double offsetMoverbarra)
        {
            XYZ sentidoBOrde = (EndPoint - StartPoint).Normalize();
            XYZ vectorZ = new XYZ(0, 0, 1);

            //XYZ vectorPerpeInterno = sentidoBOrde.CrossProduct(vectorZ);
            XYZ vectorPerpeInterno2 = vectorZ.CrossProduct(sentidoBOrde);
            VectorInteriorRoom = vectorPerpeInterno2.Normalize();
            VectorExteriorRoom = vectorPerpeInterno2.Normalize() * -1;

            double largoDesarrolo = UtilBarras.largo_L9_DesarrolloFoot_diamMM(diametroBArra) + 0.5 * StartPoint.DistanceTo(EndPoint);
            //linea desfazadas de la original
            startPont_offsetIntRoom = StartPoint + VectorInteriorRoom * offsetMoverbarra+ -largoDesarrolo* sentidoBOrde;
            EndPoint_offseIntRoom = EndPoint + VectorInteriorRoom * offsetMoverbarra + largoDesarrolo * sentidoBOrde;

            //extender largo de lines
            //XYZ[] Rresul = UtilBarras.extenderLineaDistancia(startPont_offsetIntRoom, EndPoint_offseIntRoom, 10);
            //startPont_offsetIntRoom = new XYZ(Rresul[0].X, Rresul[0].Y, Rresul[0].Z - offSuperiorhaciaBajoLosa);
            //EndPoint_offseIntRoom = new XYZ(Rresul[1].X, Rresul[1].Y, Rresul[1].Z - offSuperiorhaciaBajoLosa);

        }
        #endregion

    }
}
