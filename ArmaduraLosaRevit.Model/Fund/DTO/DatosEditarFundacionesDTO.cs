using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.DTO
{
    public class DatosEditarFundacionesDTO
    {
        public PathReinforcement _PathReinforcement { get; set; }
        public int Diametro_mm { get; set; }
        public RebarBarType TypeDiametro { get; set; }
        public double _Espaciamiento_foot { get; set; }
        public TipoCaraUbicacion _TipoUbicacionFund { get; set; }
        public TipoCambioFund _TipoCambioFund { get; set; }
        public bool _IsCambioEspaciamiento { get; internal set; }
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; internal set; }
    }
}
