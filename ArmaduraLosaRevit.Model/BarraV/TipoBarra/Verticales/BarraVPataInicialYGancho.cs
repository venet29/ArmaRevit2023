using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales
{
    public class BarraVPataInicialYGancho : ABarrasElevV_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public BarraVPataInicialYGancho(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {
            double largoPata = 0;
         //   double largoDesarrollo = UtilBarras.largo_ganchoFoot_diamMM(_interBArraDto.diametroMM);

            //if (_interBArraDto.IsLargopata)
            //    largoPata = _interBArraDto.Largopata;
            //else
            //    largoPata = base.largoPata;

            largoPata = base.largoPata;

            XYZ direccionBarra = (_interBArraDto.ptofinal - _interBArraDto.ptoini ).Normalize();
            Curve ladoPataBajoGAncho = Line.CreateBound(  _interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata + direccionBarra * Util.CmToFoot(35),
                                                          _interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata);

            Curve ladoPataBajo = Line.CreateBound(_interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata, _interBArraDto.ptoini);
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            _listcurve.Add(ladoPataBajoGAncho);
            _listcurve.Add(ladoPataBajo);
            _listcurve.Add(ladoCentral);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoPataBajoGAncho.Length), 0) }+{ Math.Round(Util.FootToCm(ladoPataBajo.Length), 0) }+{ Math.Round(Util.FootToCm(ladoCentral.Length), 0) })";

            double largoTotal = Math.Round(Util.FootToCm(ladoPataBajoGAncho.Length), 0) + Math.Round(Util.FootToCm(ladoPataBajo.Length), 0) + Math.Round(Util.FootToCm(ladoCentral.Length), 0);

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }
    }
}
