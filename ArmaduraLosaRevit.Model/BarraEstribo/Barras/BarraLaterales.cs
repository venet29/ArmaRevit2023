using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Barras
{
    public class BarraLaterales : ABarrasElevV_ConTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public BarraLaterales(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }

   

        public override void M0_CalcularCurva()
        {
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);
            _listcurve.Add(ladoCentral);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $" ";

            double largoTotal = Math.Round(Util.FootToCm(ladoCentral.Length), 0);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }

      
    }
}
