using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.TIpoBarra
{
    //Horquilla horizontal que recorrido es en Z tipoB), ver obs2) de documento J:\_revit\PROYECTOS_REVIT\REVITArmaduraLosaRevit\ArmaduraLosaRevit_2022\ArmaduraLosaRevit.Model\BarraV\TipoBarra\Verticales\OBS).docx
    public class HorquillaPataInicial : ABarrasHorq_SinTrans, IbarraBase
    {

        public bool IsOk { get; set; }
        public HorquillaPataInicial(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            _BarraTipo = TipoRebar.ELEV_BA_HORQ;
            IsOk = true;
        }
        public override void M0_CalcularCurva()
        {
            double mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2;

            //ServicioRedondearBarrasELEv _SRBE = new ServicioRedondearBarrasELEv(_interBArraDto, largoPata);
            //_SRBE.RedondearCentroA_1cm();
            //_SRBE.RedondearPataAmbosLados_5cm();

            double delta = 0;
            if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.GetXY0().DistanceTo(_interBArraDto.ptofinal.GetXY0()) + mitadDiam))
                delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);

            XYZ direccionVIga = (_interBArraDto.ptofinal.GetXY0() - _interBArraDto.ptoini.GetXY0()).Normalize();

            _interBArraDto.ptofinal = _interBArraDto.ptofinal + direccionVIga * delta;

            double largoPata = _interBArraDto.Largopata - mitadDiam;

            double anchoHorquilla = _interBArraDto.Largopata;// espesor viga - Diam
            XYZ ptoFinPata_esp = _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * anchoHorquilla;
            XYZ ptoIniPata_esp = _interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * anchoHorquilla;
            Curve ladoPosterior = Line.CreateBound(ptoFinPata_esp, ptoIniPata_esp);
            Curve ladoPataEspesorBajo = Line.CreateBound(ptoIniPata_esp, _interBArraDto.ptoini);
            Curve ladofrontal = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            _listcurve.Add(ladoPosterior);
            _listcurve.Add(ladoPataEspesorBajo);
            _listcurve.Add(ladofrontal);

            //para dibujar forma de referencia
            GenerarFormaFalsa(anchoHorquilla, mitadDiam);

            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ladoPosterior.Length + mitadDiam}+{ladoPataEspesorBajo.Length + mitadDiam * 2}+{ ladofrontal.Length + mitadDiam })";
            //*****
            double largoTotal = Math.Round(Util.FootToCm(ladoPosterior.Length + mitadDiam))
                              + Math.Round(Util.FootToCm(ladoPataEspesorBajo.Length + mitadDiam * 2))
                              + Math.Round(Util.FootToCm(ladofrontal.Length + mitadDiam));

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal)}";
            //ObtenerLArgoTotal();
        }


        private void GenerarFormaFalsa(double largoPata, double mitadDiam)
        {
            double desplazamiento = Util.CmToFoot(30);

            double desplazamientoZ = DesplazarEnZ_curvafalsa();



            XYZ Direcc = (_interBArraDto.ptofinal - _interBArraDto.ptoini).Normalize();
            double largoDeafase = _interBArraDto.ptofinal.DistanceTo(_interBArraDto.ptoini);
            XYZ ptoini_desfasado = _interBArraDto.ptoini - (desplazamiento + largoDeafase) * Direcc + new XYZ(0, 0, -desplazamientoZ);
            XYZ ptofin_desfasado = _interBArraDto.ptofinal - (desplazamiento + largoDeafase) * Direcc + new XYZ(0, 0, -desplazamientoZ);

            XYZ ptoIniPata_esp_elev = ptoini_desfasado + new XYZ(0, 0, largoPata);
            XYZ ptoFinPata_esp_Elev = ptofin_desfasado + new XYZ(0, 0, largoPata);

            //1
            Curve ladoPosterior_elev = Line.CreateBound(ptoFinPata_esp_Elev, ptoIniPata_esp_elev);
            DatosTExtoDTO _DatosTExtoDTOposterior = new DatosTExtoDTO()
            {
                pto = (ptoFinPata_esp_Elev + ptoIniPata_esp_elev) / 2 + new XYZ(0, 0, 0.5),
                texto = Math.Round(Util.FootToCm(ptoFinPata_esp_Elev.DistanceTo(ptoIniPata_esp_elev)) + _interBArraDto.diametroMM / 20.0f, 0).ToString()
            };

            //2
            Curve ladoPataEspesorBajo_elev = Line.CreateBound(ptoIniPata_esp_elev, ptoini_desfasado);
            DatosTExtoDTO _DatosladoPataEspesorBajo_ = new DatosTExtoDTO()
            {
                pto = (ptoIniPata_esp_elev + ptoini_desfasado) / 2 - Direcc * 0.5,
                texto = Math.Round(Util.FootToCm(ptoIniPata_esp_elev.DistanceTo(ptoini_desfasado)) + _interBArraDto.diametroMM / 10.0f, 0).ToString()
            };

            //3
            Curve ladofrontal_elev = Line.CreateBound(ptoini_desfasado, ptofin_desfasado);
            DatosTExtoDTO _DatosLadofrontal_ = new DatosTExtoDTO()
            {
                pto = (ptoini_desfasado + ptofin_desfasado) / 2 - new XYZ(0, 0, 0.5),
                texto = Math.Round(Util.FootToCm(ptoini_desfasado.DistanceTo(ptofin_desfasado)) + _interBArraDto.diametroMM / 20.0f, 0).ToString()
            };

            _listcurveElevacion.Add(ladoPosterior_elev);
            _listcurveElevacion.Add(ladoPataEspesorBajo_elev);
            _listcurveElevacion.Add(ladofrontal_elev);


            //largo total
            double LArgo = Math.Round(Util.FootToCm(ptoini_desfasado.DistanceTo(ptofin_desfasado)) + _interBArraDto.diametroMM / 20.0f, 0) +
                           Math.Round(Util.FootToCm(ptoIniPata_esp_elev.DistanceTo(ptoini_desfasado)) + _interBArraDto.diametroMM / 10.0f, 0) +
                           Math.Round(Util.FootToCm(ptoFinPata_esp_Elev.DistanceTo(ptoIniPata_esp_elev)) + _interBArraDto.diametroMM / 20.0f, 0);

            DatosTExtoDTO _DatosLArgoToal_ = new DatosTExtoDTO()
            {
                pto = (ptoini_desfasado + ptofin_desfasado) / 2 - new XYZ(0, 0, 1.1),
                texto = $"L={LArgo}"
            };


            _listTextoElevacion.Add(_DatosTExtoDTOposterior);
            _listTextoElevacion.Add(_DatosladoPataEspesorBajo_);
            _listTextoElevacion.Add(_DatosLadofrontal_);

            //_listTextoElevacion.Add(_DatosLArgoToal_);
        }

        private double DesplazarEnZ_curvafalsa()
        {
            double desplazamientoZ = Util.CmToFoot(100);
            // bajar 
            double desplamientoCorr = _interBArraDto.RecalcularPtosYEspaciamieto_Horqu.PtoInicial_Corregido.Z - _interBArraDto.RecalcularPtosYEspaciamieto_Horqu.PtoInicial_Original.Z;
            if (_view.Scale == 75)
                desplamientoCorr = desplamientoCorr + Util.CmToFoot(15);
            else if (_view.Scale == 100)
                desplamientoCorr = desplamientoCorr + Util.CmToFoot(30);

            desplazamientoZ = desplazamientoZ + desplamientoCorr;
            return desplazamientoZ;
        }

        public override bool M2_DibujarTags(ConfiguracionTAgBarraDTo confBarraTag)
        {
            if (_rebar == null) return false;
            try
            {
                if (_newGeometriaTag.M4_IsFAmiliaValida())
                {
                    foreach (TagBarra item in _newGeometriaTag.listaTag)
                    {
                        if (item == null) continue;
                        if (!item.IsOk) continue;

                        if (item.nombreFamilia == "MRA Rebar_SIN_50")
                            item.DibujarTagRebar_ConLibre(_rebar, _uiapp, _view, confBarraTag);
                        else
                            item.DibujarTagRebar_HorquillaHorizontal(_rebar, _uiapp, _view, confBarraTag);
                    }
                }
                M8_CrearPatSymbolFalso_SinTRans();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear tag:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
