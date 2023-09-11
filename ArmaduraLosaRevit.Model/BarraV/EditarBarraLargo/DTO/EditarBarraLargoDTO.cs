using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class EditarBarraLargoDTO
    {
        public double largoExtender_cm { get; set; }

        public bool IsUsarMouse { get; set; }
        public double DeltaUsarMouse_cm { get; set; }
    }
}
