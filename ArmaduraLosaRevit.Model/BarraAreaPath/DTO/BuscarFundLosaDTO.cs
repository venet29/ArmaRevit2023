using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.DTO
{
    public class Buscar_elementoEncontradoDTO
    {
        public XYZ PtoSObreCaraInferiorFundLosa { get; set; }
        public XYZ PtoSObreCaraSuperiorFundLosa { get; set; }
        
        public double distancia { get; set; }
        public TipoElementoBArraV _TipoElementoBArraV { get; set; }
        public PlanarFace _PlanarFace { get; internal set; }
    }
}
