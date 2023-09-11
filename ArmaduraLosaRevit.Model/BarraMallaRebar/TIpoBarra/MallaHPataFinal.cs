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
    public class MallaHPataFinal : ABarrasElevV_SinTrans, IbarraBase
    {

        private double ladoPataEspesorBajo_;
        private double ladoCentral_;
        private double ladoPataEspesorArriba_;
        private double ladoPataArriba35_;
        public bool IsOk { get; set; }
        public MallaHPataFinal(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            ladoPataEspesorBajo_ = 0;
            ladoCentral_ = 0;
            ladoPataEspesorArriba_ = 0;
            ladoPataArriba35_ = 0;
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {
            _interBArraDto.ptoini = _interBArraDto.ptoini + new XYZ(0, 0, _interBArraDto.espaciamientoRecorridoBarrasFoot / 2);
            _interBArraDto.ptofinal = _interBArraDto.ptofinal  +new XYZ(0, 0, _interBArraDto.espaciamientoRecorridoBarrasFoot / 2);
            if (_interBArraDto.tipobarraV == TipoPataBarra.BarraVPataFinal)
            {
                MA_CalcularCurva_soloPatafinal35();
            }
            else
            {
                startHookOrient = RebarHookOrientation.Right;
                MB_CalcularCurva_PataInicialEspesor_Patafinal35();
            }
        }


        /// caso primera linea de malla sin pata de espesor al inicio, y pata espesor +pata 35cm al final
        public void MA_CalcularCurva_soloPatafinal35()
        {
            //double largoPata = (_interBArraDto.IsLargopata ? _interBArraDto.Largopata : _interBArraDto.LargoEspesorMalla_SinRecub_foot);
            double largoPata = _interBArraDto.LargoEspesorMalla_SinRecub_foot;
           // errores.docx 1) 22-05-2023  error al crear  barrras largoPata = Util.CmToFoot(_interBArraDto.EspesorElementoHost);

            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            XYZ ptoFinPata_esp = _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata;
            Curve ladoPataEspesorArriba = Line.CreateBound(_interBArraDto.ptofinal, ptoFinPata_esp);
            Curve ladoPataArriba35 = Line.CreateBound(ptoFinPata_esp, ptoFinPata_esp + -_Direccion_ptoIniToPtoFin * ConstNH.CONST_LARGO_PATAMALLA_FOOT);

            ladoCentral_ = Math.Round(Util.FootToCm(ladoCentral.Length), 0);
            ladoPataEspesorArriba_ = Math.Round(Util.FootToCm(ladoPataEspesorArriba.Length), 0);

            // si inicialmente tiene pata de espesor
            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataEspesorArriba);
            if (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro)
            {
                _listcurve.Add(ladoPataArriba35);
                ladoPataArriba35_ = Math.Round(Util.FootToCm(ladoPataArriba35.Length), 0);
            }

            //*****

            if (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro)
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ ladoCentral_ }+{ ladoPataEspesorArriba_ }+{ ladoPataArriba35_ })";
            else
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ ladoCentral_ }+{ ladoPataEspesorArriba_ })";


            //*****
            ObtenerLArgoTotal();
        }

        /// caso ultima linea de malla con pata de espesor al inicio, y pata espesor +pata 35cm al final
        public void MB_CalcularCurva_PataInicialEspesor_Patafinal35()
        {
           // double largoPata = (_interBArraDto.IsLargopata ? _interBArraDto.Largopata : _interBArraDto.LargoEspesorMalla_SinRecub_foot);
            double largoPata = _interBArraDto.LargoEspesorMalla_SinRecub_foot;
            //largoPata = Util.CmToFoot(_interBArraDto.EspesorElementoHost);
            XYZ ptoIniPata_esp = _interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata;
            Curve ladoPataEspesorBajo = Line.CreateBound(ptoIniPata_esp, _interBArraDto.ptoini);
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            XYZ ptoFinPata_esp = _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata;
            Curve ladoPataEspesorArriba = Line.CreateBound(_interBArraDto.ptofinal, ptoFinPata_esp);
            Curve ladoPataArriba35 = Line.CreateBound(ptoFinPata_esp, ptoFinPata_esp + -_Direccion_ptoIniToPtoFin * ConstNH.CONST_LARGO_PATAMALLA_FOOT);

            ladoPataEspesorBajo_ = Math.Round(Util.FootToCm(ladoPataEspesorBajo.Length), 0);
            ladoCentral_ = Math.Round(Util.FootToCm(ladoCentral.Length), 0);
            ladoPataEspesorArriba_ = Math.Round(Util.FootToCm(ladoPataEspesorArriba.Length), 0);


            // si inicialmente tiene pata de espesor
            _listcurve.Add(ladoPataEspesorBajo);
            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataEspesorArriba);
            if (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro)
            {
                _listcurve.Add(ladoPataArriba35);
                ladoPataArriba35_ = Math.Round(Util.FootToCm(ladoPataArriba35.Length), 0);
            }
            //*****

            if (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro)
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ladoPataEspesorBajo_}+{ ladoCentral_ }+{ ladoPataEspesorArriba_ }+{ ladoPataArriba35_ })";
            else
                _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ladoPataEspesorBajo_}+{ ladoCentral_ }+{ ladoPataEspesorArriba_ })";


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
