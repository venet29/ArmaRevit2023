using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class EditarBarraDTO
    {
        public int diametro { get; set; }
        public int cantidad { get; set; }
        public TipoPataBarra tipobarraV { get; set; }
        public bool IsCambiarDiametroYEspacia { get; set; }

        public TipoCasobarra TipoCasobarra { get; set; }
        public View3D view3D_paraBuscar { get; set; }
        public View3D view3D_paraVisualizar { get; set; }
        public View viewActual { get; set; }
        public int ModificadorDireccionEnfierrado { get; set; } = 1;
    }
}
