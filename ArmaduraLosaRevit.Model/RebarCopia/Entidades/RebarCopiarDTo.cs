using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarCopia.Entidades
{
    public class RebarCopiarDTo
    {
        public Rebar Rebar { get; set; }
        public Element Elemento { get; set; }
    }
}
