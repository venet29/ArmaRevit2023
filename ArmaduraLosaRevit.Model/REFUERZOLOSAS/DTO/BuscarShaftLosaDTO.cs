using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.DTO
{
    public class BuscarShaftLosaDTO
    {
        public XYZ _PtoSObreFaceLosaShaft { get; set; }
        public double distancia { get; set; }
        public TipoElementoBArraV _TipoElementoBArraV { get; set; }
    }
}
