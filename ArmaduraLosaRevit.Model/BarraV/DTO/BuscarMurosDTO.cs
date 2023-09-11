using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class BuscarMurosDTO
    {

        public double _idElement { get; set; }
        public double _espesorMuro { get;  set; }
        public XYZ _ptobuscarWall_inferior { get; set; }
        public XYZ _ptobuscarWall_superior { get; set; }
        public XYZ _direccionMuro { get;  set; }


    }
}
