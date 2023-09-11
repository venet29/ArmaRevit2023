using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.PathReinf.DTO
{
    public class PathReinfSeleccionDTO
    {
        public XYZ ptoConMouse { get; set; }
        public double Angle_pelotaLosa1Grado { get;  set; }
        public Element ElementoSeleccionada1 { get; set; }
        public double EspesorCm_1 { get;  set; }
        public List<XYZ> ListaPtosPerimetroBarras { get;  set; }


        //solo fund 
        public XYZ PtoCodoDireztriz { get; set; }
        public XYZ PtoDireccionDireztriz { get; set; }
        public XYZ PtoLadoLibre { get; set; }
        public bool IsLadoLibre { get; set; }
        public XYZ PtoTag { get;  set; }
    }
}
