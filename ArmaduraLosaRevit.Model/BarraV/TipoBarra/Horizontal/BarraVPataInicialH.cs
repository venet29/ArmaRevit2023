using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Ayuda;
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
    public class BarraVPataInicialH : ABarrasElevH_SinTrans, IbarraBase
    {
        private double mitadDiam;

        public bool IsOk { get; set; }
        public BarraVPataInicialH(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {

            //ServicioRedondearBarrasELEv _SRBE = new ServicioRedondearBarrasELEv(_interBArraDto, largoPata);
            //_SRBE.RedondearCentroA_1cm(2.0);
            //_SRBE.RedondearConUnaPata_SUperiorOInferior_5cm();

            RedondearCentroA_1cm();

            RedondearPataInferiro_5cm();

            Curve ladoPataBajo = Line.CreateBound(_interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata, _interBArraDto.ptoini);

            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            _interBArraDto.DireccionRecorridoBarra = Util.CrossProduct(XYZ.BasisZ, ((Line)ladoCentral).Direction);

            double angle = Util.AnguloEntre2PtosGrado90(_interBArraDto.DireccionRecorridoBarra, _interBArraDto.DireccionPataEnFierrado, true);

            _listcurve.Add(ladoPataBajo);
            _listcurve.Add(ladoCentral);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoPataBajo.Length + mitadDiam), 0) }+{ Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam), 0) })";

            double largoTotal = Math.Round(Util.FootToCm(ladoPataBajo.Length + mitadDiam), 0) + Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam), 0);

            if (largoTotal > 1200) Util.ErrorMsg("Barras mayor de 12 mt");

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
            if (RedonderLargoBarras.RedondearFoot5_masArriba((_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal)+ mitadDiam) + largoPata))
                delta5 = RedonderLargoBarras.DeltaDesplazaminetoRedondeoFoot;

            largoPata = largoPata + delta5;

            largoPata = largoPata - mitadDiam;
        }

        private void RedondearCentroA_1cm()
        {
            //b
            double delta = 0;
            if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + Util.MmToFoot(_interBArraDto.diametroMM) / 2))
                delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);

            _interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * (delta);
        }
    }
}
