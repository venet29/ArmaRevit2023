
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Automatico;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
#pragma warning disable CS0105 // The using directive for 'ArmaduraLosaRevit.Model.BarraV.Ayuda' appeared previously in this namespace
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
#pragma warning restore CS0105 // The using directive for 'ArmaduraLosaRevit.Model.BarraV.Ayuda' appeared previously in this namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class CoordenadasBarraIMPORTAR
    {
        public XYZnh ptoIni { get; set; }
        public XYZnh ptoFin { get; set; }
        public XYZnh ptoIni_pier { get; set; }
        public XYZnh ptoFin_pier { get; set; }
        public XYZnh ptoIni_MallaVertical { get; set; }
        public XYZnh ptoFinal_MallaVertical { get; set; }
        public TipoPataBarra tipoBarraV { get; set; }
        public bool IsProloganLosaBajo { get; set; }
        public bool IsNoProloganLosaArriba { get; set; }
        public bool IsBuscarCororonacion { get; set; }
        public bool AuxIsbarraIncial { get; set; }

        public Orientacion OrientacionTag { get; set; }

        //solo malla       
        public DatosMallasAutoDTO DatosMallasDTO_ { get; set; }

        public CoordenadasBarraIMPORTAR()
        {
        }


        public CoordenadasBarra ObtenerCoordenadasBarra(ArmaduraTrasformada _armaduraTrasformada)
        {
            ptoIni_pier = ptoIni_pier.AsignarZ(ptoIni.Z);
            ptoFin_pier = ptoIni_pier.AsignarZ(ptoFin.Z);

            var aux_ptoIni_MallaVertical = (ptoIni_MallaVertical != null ? ptoIni_MallaVertical : ptoIni);
            var aux_ptoFinal_MallaVertical = (ptoFinal_MallaVertical != null ? ptoFinal_MallaVertical : ptoFin);
            return new CoordenadasBarra()
            {
                ptoIni_foot = _armaduraTrasformada.Ejecutar(ptoIni.GetXYZ_cmTofoot()),
                ptoFin_foot = _armaduraTrasformada.Ejecutar(ptoFin.GetXYZ_cmTofoot()),
                ptoIni_MallaVertical = _armaduraTrasformada.Ejecutar(aux_ptoIni_MallaVertical.GetXYZ_cmTofoot()),
                ptoFin_MallaVertical = _armaduraTrasformada.Ejecutar(aux_ptoFinal_MallaVertical.GetXYZ_cmTofoot()),
                ptoIni_pier_foot = _armaduraTrasformada.Ejecutar(ptoIni_pier.GetXYZ_cmTofoot()),
                ptoFin_pier_foot = _armaduraTrasformada.Ejecutar(ptoFin_pier.GetXYZ_cmTofoot()),
                OrientacionTag=this.OrientacionTag,
                tipoBarraV = TipoPataBarra.buscar,// TipoPataBarra.BarraVSinPatas,
                IsNoProloganLosaArriba = this.IsNoProloganLosaArriba,
                IsBuscarCororonacion = this. IsBuscarCororonacion,
                IsProloganLosaBajo = this.IsProloganLosaBajo,
                AuxIsbarraIncial = this.AuxIsbarraIncial
            };
        }


        public CoordenadasBarra ObtenerCoordenadasBarra(ArmaduraTrasformada _armaduraTrasformada,double deltaZ_cm)
        {
            ptoIni = ptoIni - new XYZnh(0,0 ,deltaZ_cm);
            ptoFin = ptoFin - new XYZnh(0, 0, deltaZ_cm);

            ptoIni_pier = ptoIni_pier.AsignarZ(ptoIni.Z);
            ptoFin_pier = ptoIni_pier.AsignarZ(ptoFin.Z);

            var aux_ptoIni_MallaVertical = (ptoIni_MallaVertical != null ? ptoIni_MallaVertical - new XYZnh(0, 0, deltaZ_cm) : ptoIni);
            var aux_ptoFinal_MallaVertical = (ptoFinal_MallaVertical != null ? ptoFinal_MallaVertical - new XYZnh(0, 0, deltaZ_cm) : ptoFin);
            return new CoordenadasBarra()
            {
                ptoIni_foot = _armaduraTrasformada.Ejecutar(ptoIni.GetXYZ_cmTofoot()),
                ptoFin_foot = _armaduraTrasformada.Ejecutar(ptoFin.GetXYZ_cmTofoot()),
                ptoIni_MallaVertical = _armaduraTrasformada.Ejecutar(aux_ptoIni_MallaVertical.GetXYZ_cmTofoot()),
                ptoFin_MallaVertical = _armaduraTrasformada.Ejecutar(aux_ptoFinal_MallaVertical.GetXYZ_cmTofoot()),
                ptoIni_pier_foot = _armaduraTrasformada.Ejecutar(ptoIni_pier.GetXYZ_cmTofoot()),
                ptoFin_pier_foot = _armaduraTrasformada.Ejecutar(ptoFin_pier.GetXYZ_cmTofoot()),
                OrientacionTag = this.OrientacionTag,
                tipoBarraV = TipoPataBarra.buscar,// TipoPataBarra.BarraVSinPatas,
                IsNoProloganLosaArriba = this.IsNoProloganLosaArriba,
                IsBuscarCororonacion = this.IsBuscarCororonacion,
                IsProloganLosaBajo = this.IsProloganLosaBajo,
                AuxIsbarraIncial = this.AuxIsbarraIncial
            };
        }


    }
}
