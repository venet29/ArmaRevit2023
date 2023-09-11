using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar.Model
{
    public  class WrapperBarrasLosa
    {
        public List<IndependentTag> Listatag { get; set; }
        public PathReinSpanSymbol pathReinSpanSymbol { get; set; }
        public PathReinforcement pathReinforcement { get; set; }
        public XYZ VectorDesplazamiento { get; internal set; }
        public XYZ VectorDesplazamientoPathSymbol { get; internal set; }

        internal List<ElementId> ObtenerListaIdPath()
        {
            List<ElementId> ele = new List<ElementId>();
            ele.Add(pathReinforcement.Id);
            ele.Add(pathReinSpanSymbol.Id);
            ele.AddRange(Listatag.Select(c => c.Id).ToList());
            return ele;
        }
    }
}
