using ArmaduraLosaRevit.Model.Fund.DTO;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.Fund.DTO
{
    public class DatosRebarFundDTO
    {
        public CasosFundDTO CasosFundDTO { get; set; }
        public XYZ P1 { get; set; }
        public XYZ P2 { get; set; }
        public XYZ P3 { get; set; }
        public XYZ P4 { get; set; }
        public Element fund { get; set; }
        public XYZ PtoMOuse_sobreFundacion { get; internal set; }
        public XYZ TagHeadPosition { get; internal set; }
        public XYZ LeaderElbow { get; internal set; }
        public XYZ LeaderEnd { get; internal set; }
        public bool IsLeaderElbow { get; internal set; }
        public bool IsLeaderEnd { get; internal set; }
    }
}