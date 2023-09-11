using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar.TIpoBarra
{
    public class MallaVPataInicial : ABarrasElevV_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }

        public MallaVPataInicial(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {

            double mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2;
            //if (_interBArraDto.IsLargopata)
            //    largoPata = _interBArraDto.Largopata;
            //else
            //    largoPata = base.largoPata;

            largoPata = largoPata - mitadDiam;

          //  _interBArraDto.DireccionRecorridoBarra = _interBArraDto.DireccionRecorridoBarra;// - XYZ.BasisZ.CrossProduct(_interBArraDto.DireccionPataEnFierrado);
            //a
            Curve ladoPataBajo = Line.CreateBound(_interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado * largoPata, _interBArraDto.ptoini);

            //b
            double delta = 0;
            if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + Util.MmToFoot(_interBArraDto.diametroMM)/2))
                delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);

            _interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * (delta);

            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);


            _listcurve.Add(ladoPataBajo);
            _listcurve.Add(ladoCentral);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoPataBajo.Length + mitadDiam), 0) }+{ Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam), 0) })";

            double largoTotal = Math.Round(Util.FootToCm(ladoPataBajo.Length + mitadDiam), 0) + Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam), 0);

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }
    }
}
