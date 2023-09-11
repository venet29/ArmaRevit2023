using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Horizontal
{
    public class BarraVPataAmbosH : ABarrasElevH_SinTrans, IbarraBase
    {
        private double mitadDiam;
        private double largoPataInicial;
        private double largoPataFinal;
        public bool IsOk { get; set; }
        public BarraVPataAmbosH(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }

        public override void M0_CalcularCurva()
        {

            //b
            RedondearCentroA_1cm();
            RedondearPataInferiro_5cm();

            Curve ladoPataBajo = Line.CreateBound(_interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPataInicial, _interBArraDto.ptoini);

            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);
            //c
            Curve ladoPataArriba = Line.CreateBound(_interBArraDto.ptofinal, _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPataFinal);


            _interBArraDto.DireccionRecorridoBarra = Util.CrossProduct(XYZ.BasisZ, ((Line)ladoCentral).Direction);
            double angle = Util.AnguloEntre2PtosGrado90(_interBArraDto.DireccionRecorridoBarra, _interBArraDto.DireccionPataEnFierrado, true);

            _listcurve.Add(ladoPataBajo);
            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataArriba);


            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoPataBajo.Length + mitadDiam), 0) }+{ Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam * 2), 0) }+{ Math.Round(Util.FootToCm(ladoPataArriba.Length + mitadDiam), 0) })";

            double largoTotal = Math.Round(Util.FootToCm(ladoPataBajo.Length + mitadDiam), 0) + Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam * 2), 0) + Math.Round(Util.FootToCm(ladoPataArriba.Length + mitadDiam));

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";

        }

        private void RedondearPataInferiro_5cm()
        {
            mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2;
            //if (_interBArraDto.IsLargopata)
            //    largoPata = _interBArraDto.Largopata;
            //else
            //    largoPata = base.largoPata;

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


        private void RedondearCentroA_1cm()
        {
            double delta = 0;
            if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + Util.MmToFoot(_interBArraDto.diametroMM)))
                delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);

            _interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * delta;
        }
    }
}
