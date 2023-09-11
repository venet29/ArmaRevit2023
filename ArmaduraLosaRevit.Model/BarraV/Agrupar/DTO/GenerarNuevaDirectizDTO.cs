using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.DTO
{
    public class GenerarNuevaDirectizDTO
    {
        public XYZ ptoInserciontag   { get; set; }
        public Orientacion OrientacionSeleccion { get; set; }
        public XYZ DireccionMoverTag { get; set; }
    }
}
