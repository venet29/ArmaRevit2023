using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Entidades
{
   public  class WrapperAreaRein
    {
        public Element element { get; set; }
        public string NombreView { get;  set; }
        public IList<ElementId> ListaRebarInsistem { get; internal set; }
    }
}
