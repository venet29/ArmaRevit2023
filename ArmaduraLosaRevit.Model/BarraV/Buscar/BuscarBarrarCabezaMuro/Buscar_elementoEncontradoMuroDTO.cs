using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar.BuscarBarrarCabezaMuro
{
    public class Buscar_elementoEncontradoMuroDTO: Buscar_elementoEncontradoDTO
    {
        public XYZ _DireccionMuro { get; set; }
        public double _EspesorMuro { get;  set; }
        public XYZ _OrigenMuro { get;  set; }
        public XYZ _CentroCaraNormalInversaVista { get; internal set; }
    }
}
