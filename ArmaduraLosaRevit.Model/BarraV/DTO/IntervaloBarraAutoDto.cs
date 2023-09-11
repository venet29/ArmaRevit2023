using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class IntervalosBarraAutoDto
    {

        public static bool moverPorTraslapo { get; set; }

        public XYZ PtoBordeMuro { get; set; }
        public XYZ PtoCentralSobreMuro { get; set; }
        

        //public bool moverPorTraslapo { get; set; }
        public bool AuxIsbarraIncial { get; set; }


        public int Inicial_diametroMM { get; set; }
        public int Inicial_Cantidadbarra { get; set; }
      
        public List<CoordenadasBarra> ListaCoordenadasBarra { get; set; }
        public double EspaciamientoREspectoBordeFoot { get;  set; }
        public string Pier { get;  set; }
        public string Story { get;  set; }
        public Orientacion orientacion { get;  set; }
        public Orientacion OrientacionTagGrupoBarras { get;  set; }
        public UbicacionEnPier ubicacionEnPier { get; set; }
        public double diametroIntervaloAnteriorMM { get; set; }
        public int Linea { get;  set; }

        public double x_referencia_ordenar { get; set; }  //variables soli se utliza para agrupar y dibijar en forma ordenada
        public double z_referencia_ordenar { get; set; }  //variables soli se utliza para agrupar y dibijar en forma ordenada
    }
}
