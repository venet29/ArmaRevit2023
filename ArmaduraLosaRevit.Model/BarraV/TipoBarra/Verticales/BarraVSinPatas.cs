using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales
{
    public class BarraVSinPatas : ABarrasElevV_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public BarraVSinPatas(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }
        public override void M0_CalcularCurva()
        {
            ServicioRedondearBarrasELEv _SRBE = new ServicioRedondearBarrasELEv(_interBArraDto, largoPata);
            //_SRBE.RedondearCentroA_1cm(2.0);
            _SRBE.RedondearCentroA5();
            //b
            //double delta = 0;
            //if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal)))
            //    delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);

            //_interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * (delta);

            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            _listcurve.Add(ladoCentral);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $" ";

            double largoTotal =  Math.Round(Util.FootToCm(ladoCentral.Length), 0) ;
            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();
        }
    }
}
