using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Shaft
{
    interface IShaftIndividual
    {
        List<XYZ> vertices { get; set; }
        bool IsOk { get; set; }
        bool IsPtoDentroShaf(XYZ ptomouse);
   
    }
}
