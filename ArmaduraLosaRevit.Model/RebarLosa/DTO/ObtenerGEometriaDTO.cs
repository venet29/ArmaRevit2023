using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.RebarLosa.DTO
{
    public class ObtenerGEometriaDTO
    {
        public List<XYZ> ListaPtosPerimetroBarras { get; set; }

        public int barraMenos { get; set; } = 0;
        public bool usarPoligonoOriginal { get; set; } = false;

        public List<XYZ> ListaPtosPerimetroBarrasParaDimensiones { get; set; }
        public TipoCaraObjeto TipoUbicacionFund { get; internal set; }
    }

}
