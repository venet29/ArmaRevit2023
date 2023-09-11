using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.TIpoBarra
{
    //Horquilla horizontal que recorrido es en Z tipoB), ver obs2) de documento J:\_revit\PROYECTOS_REVIT\REVITArmaduraLosaRevit\ArmaduraLosaRevit_2022\ArmaduraLosaRevit.Model\BarraV\TipoBarra\Verticales\OBS).docx
    public class HorquillaPataFinal : ABarrasElevV_SinTrans, IbarraBase
    {
#pragma warning disable CS0169 // The field 'HorquillaPataFinal.ladoPataBajo35_' is never used
        private double ladoPataBajo35_;
#pragma warning restore CS0169 // The field 'HorquillaPataFinal.ladoPataBajo35_' is never used
        private double ladoPataEspesorBajo_;
        private double ladoCentral_;
        private double ladoPataEspesorArriba_;
        private double ladoPataArriba35_;
        public bool IsOk { get; set; }
        public HorquillaPataFinal(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag):base(uiapp, interBArraDto, _newGeometriaTag)
        {
            ladoPataEspesorBajo_ = 0;
            ladoCentral_ = 0;
            ladoPataEspesorArriba_ = 0;
            ladoPataArriba35_ = 0;
            IsOk = true;
            _BarraTipo = TipoRebar.ELEV_BA_HORQ;
        }


        public override void M0_CalcularCurva()
        {
            //double largoPata = (_interBArraDto.IsLargopata ? _interBArraDto.Largopata : base.largoPata);
            double largoPata = _interBArraDto.LargoEspesorMalla_SinRecub_foot;


            XYZ ptoFinPata_esp = _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata;
            XYZ ptoIniPata_esp = _interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata;

            Curve ladoPosterior = Line.CreateBound(ptoIniPata_esp,ptoFinPata_esp );
            Curve ladoPataEspesorBajo = Line.CreateBound(ptoFinPata_esp , _interBArraDto.ptofinal);
            Curve ladofrontal = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptoini);

            _listcurve.Add(ladoPosterior);
            _listcurve.Add(ladoPataEspesorBajo);
            _listcurve.Add(ladofrontal);

            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ladoPosterior}+{ladoPataEspesorBajo}+{ ladofrontal })";

            GenerarFormaFalsa(largoPata);

            //*****
            ObtenerLArgoTotal();
        }


        private void GenerarFormaFalsa(double largoPata)
        {
            double desplazamiento = Util.CmToFoot(50);

            double largoDeafase = _interBArraDto.ptofinal.DistanceTo(_interBArraDto.ptoini);
            XYZ ptoini_desfasado = _interBArraDto.ptoini + (desplazamiento + largoDeafase) * _view.RightDirection + new XYZ(0, 0, desplazamiento);
            XYZ ptofin_desfasado = _interBArraDto.ptofinal + (desplazamiento + largoDeafase) * _view.RightDirection + new XYZ(0, 0, desplazamiento);

            XYZ ptoIniPata_esp_elev = ptoini_desfasado + new XYZ(0, 0, largoPata);
            XYZ ptoFinPata_esp_Elev = ptofin_desfasado + new XYZ(0, 0, largoPata);

            Curve ladoPosterior_elev = Line.CreateBound(ptoFinPata_esp_Elev, ptoIniPata_esp_elev);
            Curve ladoPataEspesorBajo_elev = Line.CreateBound(ptoIniPata_esp_elev, ptoini_desfasado);
            Curve ladofrontal_elev = Line.CreateBound(ptoini_desfasado, ptofin_desfasado);
            _listcurveElevacion.Add(ladoPosterior_elev);
            _listcurveElevacion.Add(ladoPataEspesorBajo_elev);
            _listcurveElevacion.Add(ladofrontal_elev);
        }

        private void ObtenerLArgoTotal()
        {
            double largoTotal = ladoPataEspesorBajo_ + ladoCentral_ + ladoPataEspesorArriba_ + (_interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro ? ladoPataArriba35_ : 0);


            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }



    }
}
