using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    public class EmpotramientoPatasLosaDTO
    {
  

        public TipoBarraRefuerzo TipoPataIzq { get; set; }

        public TipoBarraRefuerzo TipoPataDere { get; set; }
        public double factorLargoIni { get;  set; } = 1;
        public double factorLargoFin { get; set; } = 1;
    }
}
