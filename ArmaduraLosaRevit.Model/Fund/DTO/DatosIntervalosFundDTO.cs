using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.DTO
{
   public class DatosIntervalosFundDTO
    {
        public GenerarListaIntervalosDTo _generarListaIntervalosDTo { get; set; }
        public TipoPataFund tipoPataFun { get; set; }
        public XYZ PtoMouse { get;  set; }
        public XYZ PtoCodoDireztriz { get;  set; }
        public XYZ PtoDireccionDireztriz { get;  set; }
        public XYZ LeaderEnd { get; set; }
        public List<XYZ> _listaptos { get;  set; }

        public RebarHookType rebarHookTypePrincipal_star { get;  set; }
        public RebarHookType rebarHookTypePrincipal_end { get; set; }
        public double LargoPAtaIzqHook { get; set; }
        public double LargoPAtaDereHook { get; set; }
        public XYZ PtoTag { get; internal set; }





    }
}
