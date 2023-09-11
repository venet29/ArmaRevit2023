using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class CoordenadasElementoDTO
    {
        public XYZ _ptoCentroElemento_conrecub { get; set; }
        public XYZ _ptobarra1Inf_conrecub { get;  set; }
        public XYZ _ptobarra1Sup_conrecub { get;  set; }
        public XYZ _ptobarra2Inf_conrecub { get;  set; }
        public XYZ _ptobarra2Sup_conrecub { get;  set; }
        public Element _ElemetSelect { get;  set; }
        public XYZ _PtoInicioBaseBordeViga_ProyectadoCaraMuroHost { get; internal set; }
    }
}
