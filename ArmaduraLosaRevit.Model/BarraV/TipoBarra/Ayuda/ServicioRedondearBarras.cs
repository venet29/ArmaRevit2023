using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Ayuda
{
   public class ServicioRedondearBarrasELEv
    {
        private readonly IntervaloBarrasDTO _interBArraDto;

        private XYZ _Direccion_ptoIniToPtoFin;
        private double mitadDiam;
        public double largoPata { get; private set; }
        public double largoPataFinal { get; private set; }
        public double largoPataInicial { get; private set; }

        public ServicioRedondearBarrasELEv(IntervaloBarrasDTO _interBArraDto, double largoPata)
        {
            this._interBArraDto = _interBArraDto;
            this.largoPata = largoPata;
            this._Direccion_ptoIniToPtoFin = (_interBArraDto.ptofinal - _interBArraDto.ptoini).Normalize();
        }


        public void RedondearPataAmbosLados_5cm() // para caso para en ambos lados
        {
            mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2;
            //if (_interBArraDto.IsLargopata)
            //    largoPata = _interBArraDto.Largopata;


            double delta5 = 0;
            if (RedonderLargoBarras.RedondearFoot5_masArriba((_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + 2 * mitadDiam) + largoPata * 2))
                delta5 = RedonderLargoBarras.DeltaDesplazaminetoRedondeoFoot;

            double largoPatado_doble = largoPata * 2 + delta5;

            if (Util.IsPar((int)Math.Round(Util.FootToCm(largoPatado_doble), 0)))
            {
                largoPataFinal = Util.CmToFoot(Util.FootToCm(largoPatado_doble) / 2.0) - mitadDiam;
                largoPataInicial = Util.CmToFoot(Util.FootToCm(largoPatado_doble) / 2.0) - mitadDiam;
            }
            else
            {
                largoPataFinal = Util.CmToFoot(Math.Ceiling(Util.FootToCm(largoPatado_doble) / 2.0)) - mitadDiam;
                largoPataInicial = Util.CmToFoot(Math.Floor(Util.FootToCm(largoPatado_doble) / 2.0)) - mitadDiam;
            }

            //largoPata = largoPata - mitadDiam;
        }
        public void RedondearConUnaPata_SUperiorOInferior_5cm()// para caso patas_superior o pata_infeor de barras verticales 
        {
            mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2;
            //if (_interBArraDto.IsLargopata)
            //    largoPata = _interBArraDto.Largopata;

            double delta5 = 0;
            if (RedonderLargoBarras.RedondearFoot5_masArriba((_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + mitadDiam) + largoPata))
                delta5 = RedonderLargoBarras.DeltaDesplazaminetoRedondeoFoot;

            largoPata = largoPata + delta5;

            largoPata = largoPata - mitadDiam;
        }

   

        public void RedondearCentroA_1cm(double factoDiv=1.00)
        {
            double delta = 0;
            if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + Util.MmToFoot(_interBArraDto.diametroMM)/ factoDiv))
                delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);

            _interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * delta;
        }


        public void RedondearCentroA5()
        {
            double delta5 = 0;
            if (RedonderLargoBarras.RedondearFoot5_masArriba(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal)))
                delta5 = RedonderLargoBarras.DeltaDesplazaminetoRedondeoFoot;

            _interBArraDto.ptoini = _interBArraDto.ptoini - _Direccion_ptoIniToPtoFin * (delta5 / 2);
            _interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * (delta5 / 2);
        }
    }
}
