using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Entidades
{
   public  class WrapperTagPath
    {
        public Element element { get; set; }
        public int idPathReinf { get; set; }
        public TipoBarra TipoPath { get; internal set; }
    }
}
