using ArmaduraLosaRevit.Model.Enumeraciones;
#pragma warning disable CS0105 // The using directive for 'ArmaduraLosaRevit.Model.Enumeraciones' appeared previously in this namespace
using ArmaduraLosaRevit.Model.Enumeraciones;
#pragma warning restore CS0105 // The using directive for 'ArmaduraLosaRevit.Model.Enumeraciones' appeared previously in this namespace
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Entidades
{
   public  class WrapperPathSymbol
    {
        public Element element { get; set; }
        public int idPathReinf { get; set; }
        public TipoBarra TipoPath { get; internal set; }
    }
}
