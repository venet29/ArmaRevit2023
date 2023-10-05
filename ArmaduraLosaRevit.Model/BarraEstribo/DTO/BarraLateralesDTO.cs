using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
    public  class BarraLateralesDTO
    {
        public XYZ StartPoint_ { get; set; }
        public XYZ EndPoint_ { get; set; }
        public int DiamtroLat { get;  set; }
        public string TextoLat { get; set; } = "";
        public Element LateralCreada { get; set; }
        public TipoPataBarra TipoLateral { get;  set; }
        public XYZ PataStart { get;  set; }
        public XYZ PataEnd { get;  set; }
    }
}
