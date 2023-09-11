using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
   public  class CoordenadasBarra
    {
        public bool IsProloganLosaBajo { get; set; }
        public bool IsNoProloganLosaArriba { get; set; }
        public XYZ ptoIni_foot { get; set; }
        public XYZ ptoFin_foot { get; set; }
        public XYZ ptoBusqueda_muro { get; set; }

        public XYZ ptoIni_pier_foot { get; set; }
        public XYZ ptoFin_pier_foot { get; set; }

        public int line { get; set; }

        public TipoPataBarra tipoBarraV { get; set; }
        public bool AuxIsbarraIncial { get;  set; }
        public XYZ ptoIni_MallaVertical { get;  set; }
        public XYZ ptoFin_MallaVertical { get;  set; }


        public bool IsUltimoTramoCOnMouse { get; set; }
        public bool IsBuscarCororonacion { get;  set; }
        public Orientacion OrientacionTag { get; set; }

        public CoordenadasBarra()
        {
        }


        public CoordenadasBarra(XYZ xYZ1, XYZ xYZ2, TipoPataBarra tipobarraInicial, bool IsUltimoTramoCOnMouse = false)
        {
            this.ptoIni_foot = xYZ1;
            this.ptoFin_foot = xYZ2;
            double ZunTercio = Math.Min(xYZ2.Z, xYZ1.Z)+ Math.Abs(xYZ2.Z- xYZ1.Z)*1/3;
            this.ptoBusqueda_muro = ((xYZ2 + xYZ1) / 2).AsignarZ(ZunTercio);
            this.IsUltimoTramoCOnMouse = IsUltimoTramoCOnMouse;
            this.tipoBarraV = tipobarraInicial;
            this.IsProloganLosaBajo = false;
            this.IsNoProloganLosaArriba = false;
            this.IsBuscarCororonacion = false;
            this.AuxIsbarraIncial = false;
        }
    }
}
