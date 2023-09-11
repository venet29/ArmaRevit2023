using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
    public class IntervalosConfinaDTOAuto
    {
        public DatosConfinamientoAutoDTO _datosConfinaDTO { get; set; }

        public List<Curve> ListaCurvaPAthArea { get;  set; }

        public Element _muroSeleccionado { get; set; }
        public List<XYZ> ListaPtos { get;  set; }
        public string Pier { get; internal set; }
        public string Story { get; internal set; }
    }
}
