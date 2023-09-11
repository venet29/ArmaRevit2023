using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO
{
    public class PathSymbolFDTO
    {
        public FamilySymbol fmName { get; set; }
        public List<string> ListaFamilia { get; set; }
    }
}
