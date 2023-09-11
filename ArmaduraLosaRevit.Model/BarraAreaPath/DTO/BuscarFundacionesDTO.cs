using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.DTO
{
    public class BuscarFundacionesDTO
    {
        public int diametroMM { get; internal set; }
        public XYZ DireccionEnFierrado { get; internal set; }
        public object _ptoini { get; internal set; }
    }
}
