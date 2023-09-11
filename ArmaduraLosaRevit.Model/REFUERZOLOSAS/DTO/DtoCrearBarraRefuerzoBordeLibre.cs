using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO
{
    public class DtoCrearBarraRefuerzoBordeLibre
    {
        public TipoBorde TipoBorde { get; set; }
        public Floor floor { get; set; }
        public CalculoBarraRefuerzo br { get; set; }
        public IGeometriaTag _iGeometriaTagRefuerzo { get; set; }
        public int numeroBarra { get; set; }
        public string textREfuerzoBarra { get; set; }
        public TipoRebar _TipoRebar { get;  set; }
    }
}
