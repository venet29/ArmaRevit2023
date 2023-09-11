using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarCopia.Entidades
{
    public class ResulSelecionElementpCopiaDto
    {
        public XYZ ptoSobreCaraInferiorFundacion { get; set; }
        public XYZ PtoDesfase { get; set; }
        public List<ElementId> ListaElementoCopiados_id { get;  set; }
        public Element ElementoACopiar { get;  set; }
        public XYZ ptoSuperiorColumna { get; internal set; }
    }
}
