using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar.TIpoBarra
{
    public class MallaHPataAmbos : ABarrasElevV_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public MallaHPataAmbos(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag):base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {

            _interBArraDto.ptoini = _interBArraDto.ptoini + new XYZ(0, 0, _interBArraDto.espaciamientoRecorridoBarrasFoot / 2);
            _interBArraDto.ptofinal= _interBArraDto.ptofinal  +new XYZ(0, 0, _interBArraDto.espaciamientoRecorridoBarrasFoot / 2);

            //  double largoDesarrollo = UtilBarras.largo_ganchoFoot_diamMM(_interBArraDto.diametroMM);
//            double largoPata = (_interBArraDto.IsLargopata ? _interBArraDto.Largopata : _interBArraDto.LargoEspesorMalla_SinRecub_foot);
            double largoPata = _interBArraDto.LargoEspesorMalla_SinRecub_foot;
           // largoPata = Util.CmToFoot(_interBArraDto.EspesorElementoHost);

            XYZ ptoIniPata_esp = _interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata;
            XYZ ptoFinPata_esp = _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata;

          

            Curve ladoPataBajo35 = Line.CreateBound(ptoIniPata_esp + _Direccion_ptoIniToPtoFin * ConstNH.CONST_LARGO_PATAMALLA_FOOT, ptoIniPata_esp);
            Curve ladoPataEspesorBajo = Line.CreateBound(ptoIniPata_esp, _interBArraDto.ptoini);
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);
            Curve ladoPataArriba = Line.CreateBound(_interBArraDto.ptofinal, ptoFinPata_esp);
            Curve ladoPataArriba35 = Line.CreateBound(ptoFinPata_esp, ptoFinPata_esp + -_Direccion_ptoIniToPtoFin * ConstNH.CONST_LARGO_PATAMALLA_FOOT);

            if (_interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro) _listcurve.Add(ladoPataBajo35);
            _listcurve.Add(ladoPataEspesorBajo);
            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataArriba);
            if (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro) _listcurve.Add(ladoPataArriba35);

            //*****
            if (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro)
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoPataBajo35.Length), 0) }+{ Math.Round(Util.FootToCm(ladoPataEspesorBajo.Length), 0) }" +
                                                                  $"+{ Math.Round(Util.FootToCm(ladoCentral.Length), 0) }" +
                                                                  $"{ Math.Round(Util.FootToCm(ladoPataArriba.Length), 0) }+{ Math.Round(Util.FootToCm(ladoPataArriba35.Length), 0) })";
            else
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoPataEspesorBajo.Length), 0) }+{ Math.Round(Util.FootToCm(ladoCentral.Length), 0) }+{ Math.Round(Util.FootToCm(ladoPataArriba.Length), 0) })";

            //*****
            double largoTotal = (_interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro ? Math.Round(Util.FootToCm(ladoPataBajo35.Length), 0) : 0)+
                                Math.Round(Util.FootToCm(ladoPataEspesorBajo.Length), 0) + Math.Round(Util.FootToCm(ladoCentral.Length), 0) + Math.Round(Util.FootToCm(ladoPataArriba.Length))+
                                (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro ? Math.Round(Util.FootToCm(ladoPataArriba35.Length), 0) : 0);

            if (largoTotal > 1200 ) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }

    }
}
