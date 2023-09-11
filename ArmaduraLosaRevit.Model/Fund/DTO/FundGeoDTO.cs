using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.DTO
{
    public class FundGeoDTO
    {
        public Element fundacion { get; set; }
        public PlanarFace FaceAnalizada { get; set; }
        public string CaraAnalizada { get; set; }
        public double Espesor_foot { get; set; }
        public XYZ ptoSeleccionFund { get; set; }
        public bool IsOK { get; internal set; }
    }
}
