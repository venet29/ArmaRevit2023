using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
     public  class IbarraBaseResultDTO
    {
        public Rebar _rebar { get; set; }

        public Orientacion OrientacionTagGrupoBarras { get; set; }
        public bool IsNoProloganLosaArriba { get;  set; }
        public XYZ ptoPosicionTAg { get;  set; }
        public bool IsConDirectriz { get; set; }
    }
}
