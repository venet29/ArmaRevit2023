using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    //pts para trabajr con ls trasformadas
    //entonces siempre seran horizontales
    public  class EstriboRefuerzoDTO
    {
        //name  Estr1 , Estr2... de abajo haci arriba
        public string name { get; set; }

        public TipoEstriboRefuerzoLosa cantidadEstribo { get; set; }
        public XYZ Origen { get; }
        public Line Curve { get; private set; }
        public double AnchoFoot { get; set; }
        public double EspaciamientoEntreEstribo_Foot { get; private set; }
        public int DiamtroBarraEnMM { get; private set; }
        public TipoRebar BarraTipo { get; internal set; }

        public EstriboRefuerzoDTO(XYZ origen, XYZ pini, XYZ pfin, double anchoFoot, double espaciamientoEntreEstribo_Foot, int diamtroBarraEnMM, TipoEstriboRefuerzoLosa cantidadEstribo, TipoRebar BarraTipo)
        {
            Origen = origen;
            Curve = Line.CreateBound(pini, pfin);
            AnchoFoot = anchoFoot;
            EspaciamientoEntreEstribo_Foot = espaciamientoEntreEstribo_Foot;
            DiamtroBarraEnMM = diamtroBarraEnMM;
            this.cantidadEstribo = cantidadEstribo;
            this.BarraTipo = BarraTipo;
        }
    }
}
