using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales
{
    public class BarraVPataFinal : ABarrasElevV_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public BarraVPataFinal(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {
            double mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2;

            ServicioRedondearBarrasELEv _SRBE = new ServicioRedondearBarrasELEv(_interBArraDto, largoPata);
            _SRBE.RedondearCentroA_1cm(2.0);
            _SRBE.RedondearConUnaPata_SUperiorOInferior_5cm();
            //double mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2;
            //if (_interBArraDto.IsLargopata)
            //    largoPata = _interBArraDto.Largopata - mitadDiam;
            //else
            //    largoPata = base.largoPata;

            //largoPata = largoPata - mitadDiam;

            //_interBArraDto.DireccionRecorridoBarra = -XYZ.BasisZ.CrossProduct(_interBArraDto.DireccionPataEnFierrado);
            //b
            //double delta = 0;
            //if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + Util.MmToFoot(_interBArraDto.diametroMM) / 2))
            //    delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);

            //_interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * (delta);
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            //c     
            Curve ladoPataArriba = Line.CreateBound(_interBArraDto.ptofinal, _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * _SRBE.largoPata);




            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataArriba);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam)) }+{ Math.Round(Util.FootToCm(ladoPataArriba.Length + mitadDiam)) })";

            double largoTotal = Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam)) + Math.Round(Util.FootToCm(ladoPataArriba.Length + mitadDiam));

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal)}";
        }

    }
}
