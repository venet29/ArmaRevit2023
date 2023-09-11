using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar.TIpoBarra
{
    public class MallaHPataInicial : ABarrasElevV_SinTrans, IbarraBase
    {
        private double ladoPataBajo35_;
        private double ladoPataEspesorBajo_;
        private double ladoCentral_;
        private double ladoPataEspesorArriba_;
#pragma warning disable CS0649 // Field 'MallaHPataInicial.ladoPataArriba35_' is never assigned to, and will always have its default value 0
        private double ladoPataArriba35_;
#pragma warning restore CS0649 // Field 'MallaHPataInicial.ladoPataArriba35_' is never assigned to, and will always have its default value 0
        public bool IsOk { get; set; }
        public MallaHPataInicial(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            ladoPataBajo35_ = 0;
            ladoPataEspesorBajo_ = 0;
            ladoCentral_ = 0;
            ladoPataEspesorArriba_ = 0;
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {
            _interBArraDto.ptoini = _interBArraDto.ptoini + new XYZ(0, 0, _interBArraDto.espaciamientoRecorridoBarrasFoot / 2);
            _interBArraDto.ptofinal = _interBArraDto.ptofinal  +new XYZ(0, 0, _interBArraDto.espaciamientoRecorridoBarrasFoot / 2);

            if (_interBArraDto.tipobarraV == TipoPataBarra.BarraVPataInicial)
                MA_CalcularCurva_soloPataInicial35();
            else
            {
                endHookOrient = RebarHookOrientation.Left;
                MB_CalcularCurva_PataFinalEspesor_PataInicial35();
            }
        }
        private void MA_CalcularCurva_soloPataInicial35()
        {
            //double largoPata = (_interBArraDto.IsLargopata ? _interBArraDto.Largopata : _interBArraDto.LargoEspesorMalla_SinRecub_foot);
            double largoPata = _interBArraDto.LargoEspesorMalla_SinRecub_foot;
            // 22-05-2023 al  generar pata en viga el largo del espsoor es igual al largo de la viga  largoPata = Util.CmToFoot(_interBArraDto.EspesorElementoHost);

            XYZ ptoIniPata_esp = _interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata;
            Curve ladoPataBajo35 = Line.CreateBound(ptoIniPata_esp + _Direccion_ptoIniToPtoFin * ConstNH.CONST_LARGO_PATAMALLA_FOOT, ptoIniPata_esp);
            Curve ladoPataEspesorBajo = Line.CreateBound(ptoIniPata_esp, _interBArraDto.ptoini);
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

      
            if (_interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro)
            {
                _listcurve.Add(ladoPataBajo35);
                ladoPataBajo35_ = Math.Round(Util.FootToCm(ladoPataBajo35.Length), 0);
            }
            ladoPataEspesorBajo_ = Math.Round(Util.FootToCm(ladoPataEspesorBajo.Length), 0);
            ladoCentral_ = Math.Round(Util.FootToCm(ladoCentral.Length), 0);

            _listcurve.Add(ladoPataEspesorBajo);
            _listcurve.Add(ladoCentral);

            //*****
            if (_interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro)
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ladoPataBajo35}+{ladoPataEspesorBajo_}+{ ladoCentral_ })";
            else
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ladoPataEspesorBajo_}+{ ladoCentral_ })";

            //*****
            ObtenerLArgoTotal();
        }

        private void MB_CalcularCurva_PataFinalEspesor_PataInicial35()
        {
            //double largoPata = (_interBArraDto.IsLargopata ? _interBArraDto.Largopata : _interBArraDto.LargoEspesorMalla_SinRecub_foot);
            //largoPata = Util.CmToFoot(_interBArraDto.EspesorElementoHost);
            double largoPata = _interBArraDto.LargoEspesorMalla_SinRecub_foot;

            XYZ ptoIniPata_esp = _interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata;
            Curve ladoPataBajo35 = Line.CreateBound(ptoIniPata_esp + _Direccion_ptoIniToPtoFin * ConstNH.CONST_LARGO_PATAMALLA_FOOT, ptoIniPata_esp);
            Curve ladoPataEspesorBajo = Line.CreateBound(ptoIniPata_esp, _interBArraDto.ptoini);
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            XYZ ptoFinPata_esp = _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata;
            Curve ladoPataEspesorArriba = Line.CreateBound(_interBArraDto.ptofinal, ptoFinPata_esp);

            if (_interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro)
            {
                _listcurve.Add(ladoPataBajo35);
                ladoPataBajo35_ = Math.Round(Util.FootToCm(ladoPataBajo35.Length), 0);
            }
            ladoPataEspesorBajo_ = Math.Round(Util.FootToCm(ladoPataEspesorBajo.Length), 0);
            ladoCentral_ = Math.Round(Util.FootToCm(ladoCentral.Length), 0);
            ladoPataEspesorArriba_ = Math.Round(Util.FootToCm(ladoPataEspesorArriba.Length), 0);

            _listcurve.Add(ladoPataEspesorBajo);
            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataEspesorArriba);
            //*****
            if (_interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro)
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ladoPataBajo35}+{ladoPataEspesorBajo_}+{ ladoCentral_ }+{ladoPataEspesorArriba_})";
            else
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ladoPataEspesorBajo_}+{ ladoCentral_ }+{ladoPataEspesorArriba_})";

            //*****
            ObtenerLArgoTotal();
        }

      
        private void ObtenerLArgoTotal()
        {
            double largoTotal = ladoPataEspesorBajo_ + ladoCentral_ + ladoPataEspesorArriba_ + (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro ? ladoPataArriba35_ : 0);

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }

    }
}
