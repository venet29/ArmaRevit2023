using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO
{
     public class BarraRoomDTO
    {
        public string TipoBarraStr { get; set; }
        public UbicacionLosa ubicacionEnlosa { get; set; }
        public int diametroEnMM { get; set; }
        public Floor LosaSeleccionada1 { get; set; }

        public double EspesorLosaCm_1 { get; set; }
        public double Espaciamiento { get; set; }
        public double AnguloBordeRoomYSegundoPtoMouseGrado { get; set; } = 0; //solo s1

        public TipoRebar _TipoRebar { get; set; }  //S4  O S1
        public double Angle_pelotaLosa1Grado { get; internal set; }
        public string Prefijo_cuantia { get; internal set; }
    }
}
