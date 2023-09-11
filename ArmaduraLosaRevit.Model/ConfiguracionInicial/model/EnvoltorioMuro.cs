using ArmaduraLosaRevit.Model.BarraV.Buscar;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial.model
{
    public class EnvoltorioMuro
    {
        public Element Muros { get; internal set; }
        public BuscarElementosArriba Busqueda { get; internal set; }
    }
}
