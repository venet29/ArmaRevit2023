using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.DTO
{
    public class SeleccionMuroAreaPathDTO
    {
        public double _espesorMuroFoot { get; set; }
        public int diamtromallaVertical { get; set; }
        public List<XYZ> ListaPtosPAth { get; set; }
        // pto en la plano dentro del muro desde la cara del muro hacia elmuro una distacia del recubrimiento
        public XYZ ptoSobreMuro_masRecub { get; set; }
        public XYZ DIreccionMuroEnIgualSentidoVIewSecction { get; set; }
        public XYZ DireccionView_NORMA { get; set; }
        public XYZ OrigenView { get; set; }
        public XYZ RightDirection_NORMA { get; internal set; }
    }
}
