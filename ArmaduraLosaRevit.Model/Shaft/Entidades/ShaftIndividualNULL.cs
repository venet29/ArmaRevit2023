using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Shaft.Entidades
{
    public class ShaftIndividualNULL : IShaftIndividual
    {
        public List<XYZ> vertices { get; set; } = new List<XYZ>();
        public bool IsOk { get; set; } = false;

        public bool IsPtoDentroShaf(XYZ ptomouse)=>  false; 
    }
}
