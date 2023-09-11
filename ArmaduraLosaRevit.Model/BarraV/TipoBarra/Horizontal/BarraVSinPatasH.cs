using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Horizontal
{
    public class BarraVSinPatasH : ABarrasElevH_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public BarraVSinPatasH(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
        }

        public override void M0_CalcularCurva()
        {
            //b
            RedondearCentroA_1cm();

            RedondearCentroA5();

            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);

            _interBArraDto.DireccionRecorridoBarra = Util.CrossProduct(XYZ.BasisZ, ((Line)ladoCentral).Direction);

            _listcurve.Add(ladoCentral);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"";

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(Util.FootToCm(ladoCentral.Length), 0)}";
            if (Util.FootToCm(ladoCentral.Length) > 1200) UtilBarras.BarrasMAyores(); 
        }

        private void RedondearCentroA5()
        {
            double delta5 = 0;
            if (RedonderLargoBarras.RedondearFoot5_masArriba(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal)))
                delta5 = RedonderLargoBarras.DeltaDesplazaminetoRedondeoFoot;

            _interBArraDto.ptoini = _interBArraDto.ptoini - _Direccion_ptoIniToPtoFin * (delta5 / 2);
            _interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * (delta5 / 2);
        }

        private void RedondearCentroA_1cm()
        {
            double delta = 0;
            if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal)))
                delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);

            _interBArraDto.ptofinal = _interBArraDto.ptofinal + _Direccion_ptoIniToPtoFin * (delta);
        }
    }
}
