using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar.TIpoBarra
{
    public class MallaHSinPatas : BarraVSinPatas, IbarraBase
    {

        private double ladoCentral_;


        public MallaHSinPatas(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            ladoCentral_ = 0;
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {
            _interBArraDto.ptoini = _interBArraDto.ptoini + new XYZ(0, 0, _interBArraDto.espaciamientoRecorridoBarrasFoot / 2);
            _interBArraDto.ptofinal = _interBArraDto.ptofinal + new XYZ(0, 0, _interBArraDto.espaciamientoRecorridoBarrasFoot / 2);

           // double largoPata = (_interBArraDto.IsLargopata ? _interBArraDto.Largopata : base.largoPata);

            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            ladoCentral_ = Math.Round(Util.FootToCm(ladoCentral.Length), 0);

            _listcurve.Add(ladoCentral);
            //*****
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = "";
            ObtenerLArgoTotal();
        }


        private void ObtenerLArgoTotal()
        {
            double largoTotal = ladoCentral_;

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }


    }
}
