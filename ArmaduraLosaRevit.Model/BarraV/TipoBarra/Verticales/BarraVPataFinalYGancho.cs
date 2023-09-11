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
    public class BarraVPataFinalYGancho : ABarrasElevV_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public BarraVPataFinalYGancho(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag):base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {
            double largoPata = 0;
          //  double largoDesarrollo = UtilBarras.largo_ganchoFoot_diamMM(_interBArraDto.diametroMM);

            //if (_interBArraDto.IsLargopata)
            //    largoPata = _interBArraDto.Largopata;
            //else
            //    largoPata = base.largoPata;

            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);
            Curve ladoPataArriba = Line.CreateBound(_interBArraDto.ptofinal, _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata);


            XYZ direccionBarra = (_interBArraDto.ptoini - _interBArraDto.ptofinal).Normalize();
            Curve ladoPataArribaGAncho = Line.CreateBound(_interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata,
                                                          _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata + direccionBarra*Util.CmToFoot(35));

            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataArriba);
            _listcurve.Add(ladoPataArribaGAncho);

            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = 
                $"({ Math.Round(Util.FootToCm(ladoCentral.Length), 0) }+{ Math.Round(Util.FootToCm(ladoPataArriba.Length), 0) }+ {Math.Round(Util.FootToCm(ladoPataArribaGAncho.Length), 0)})";

            double largoTotal =  Math.Round(Util.FootToCm(ladoCentral.Length), 0) + Math.Round(Util.FootToCm(ladoPataArriba.Length))+ Math.Round(Util.FootToCm(ladoPataArribaGAncho.Length));

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }

    }
}
