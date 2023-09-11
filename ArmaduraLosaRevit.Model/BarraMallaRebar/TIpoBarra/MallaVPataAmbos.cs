using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Extension;
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
    public class MallaVPataAmbos : ABarrasElevV_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public MallaVPataAmbos(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag):base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }


        public override void M0_CalcularCurva()
        {
            double mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2;
            largoPata = largoPata - mitadDiam;

           // _interBArraDto.DireccionRecorridoBarra = -XYZ.BasisZ.CrossProduct(_interBArraDto.DireccionPataEnFierrado);
            //a
            Curve ladoPataBajo = Line.CreateBound(_interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado* largoPata, _interBArraDto.ptoini);

            //b
            double delta = 0;
            if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + Util.MmToFoot( _interBArraDto.diametroMM)))
                delta = Util.CmToFoot( RedonderLargoBarras.largoModificar_deltaCM);

            _interBArraDto.ptofinal=_interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * delta ;
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal );

            //c
            Curve ladoPataArriba = Line.CreateBound(_interBArraDto.ptofinal, _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * largoPata);
           
            _listcurve.Add(ladoPataBajo);
            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataArriba);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoPataBajo.Length+ mitadDiam), 0) }+{ Math.Round(Util.FootToCm(ladoCentral.Length+ mitadDiam*2), 0) }+{ Math.Round(Util.FootToCm(ladoPataArriba.Length+ mitadDiam), 0) })";

            double largoTotal = Math.Round(Util.FootToCm(ladoPataBajo.Length+ mitadDiam), 0) + Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam * 2), 0) + Math.Round(Util.FootToCm(ladoPataArriba.Length+ mitadDiam));

            if (largoTotal > 1200 ) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }

    }
}
