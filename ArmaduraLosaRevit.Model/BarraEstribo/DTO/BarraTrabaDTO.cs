using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
    public  class BarraTrabaDTO
    {
        public XYZ _startPont_ { get; set; }
        public XYZ _endPoint { get; set; }
        public int _diamtroTraba { get;  set; }
        public double LargoTrabaTrasnversal { get; set; }
        public string _textoTraba { get; set; } = "";


        public double _espaciamiento { get; set; }
        public RebarHookOrientation _ubicacionHook { get; set; }
        public TipoTraba _tipo { get; set; }
        public Rebar TrabaCreada { get; internal set; }

        public BarraTrabaDTO()
        {

        }
        public BarraTrabaDTO(double espaciamiento, RebarHookOrientation ubicacion, TipoTraba tipo, int diametro,double LargoTrabaTrasnversal=0)
        {
            this._espaciamiento = espaciamiento;
            this._ubicacionHook = ubicacion;
            this._tipo = tipo;
            this._diamtroTraba = diametro;
            this.LargoTrabaTrasnversal = LargoTrabaTrasnversal;
        }
    }
}
