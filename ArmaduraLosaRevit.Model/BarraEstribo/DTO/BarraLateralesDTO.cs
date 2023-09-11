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
        public XYZ _startPont_ { get; set; }
        public XYZ _endPoint { get; set; }
        public int _diamtroLat { get;  set; }
        public string _textoLat { get; set; } = "";
        public Element LateralCreada { get; set; }
    }
}
