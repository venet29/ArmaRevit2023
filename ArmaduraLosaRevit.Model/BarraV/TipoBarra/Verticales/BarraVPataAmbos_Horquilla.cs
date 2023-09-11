using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales
{
    //HORQUILLA QUE RECODDIRO VA ENTRANDO HACIA VIEW (BARRA tipoA  Obs1))
    public class BarraVPataAmbos_Horquilla : ABarrasElevV_SinTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public BarraVPataAmbos_Horquilla(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag):base(uiapp, interBArraDto, _newGeometriaTag)
        {
            IsOk = true;
         
        }

        public override bool M1_DibujarBarra()
        {
            try
            {
                if (!M1_1_CalculosIniciales()) return false;

                M0_a_RecalculoLargoPAta();
                M0_CalcularCurva();
                var result = M1_3_DibujarBarraCurve();

                _interBArraDto._parametrosInternoRebarDTO._texToCantidadoBArras = _interBArraDto._nuevaLineaCantidadbarra.ToString();
                if (result != Result.Succeeded) return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error  ex:{ex.Message}");
                return false;
            }
            return true;

        }

        //recalcula largo de pata 
        private void M0_a_RecalculoLargoPAta()
        {
            largoPata= _interBArraDto.Largopata + UtilBarras.largo_L9_DesarrolloFoot_diamMM(_interBArraDto.diametroMM); 
        }

        public override void M0_CalcularCurva()
        {
            double mitadDiam = Util.MmToFoot(_interBArraDto.diametroMM) / 2.0;
            largoPata = largoPata - mitadDiam;

            ServicioRedondearBarrasELEv _SRBE = new ServicioRedondearBarrasELEv(_interBArraDto, largoPata);
            _SRBE.RedondearCentroA_1cm();
            _SRBE.RedondearPataAmbosLados_5cm();




            //  double largoDesarrollo = UtilBarras.largo_ganchoFoot_diamMM(_interBArraDto.diametroMM);
            _interBArraDto.DireccionRecorridoBarra = -XYZ.BasisZ.CrossProduct(_interBArraDto.DireccionPataEnFierrado);

            Curve ladoPataBajo = Line.CreateBound(_interBArraDto.ptoini + _interBArraDto.DireccionPataEnFierrado* _SRBE.largoPataInicial, _interBArraDto.ptoini);
            Curve ladoCentral = Line.CreateBound(_interBArraDto.ptoini, _interBArraDto.ptofinal);
            Curve ladoPataArriba = Line.CreateBound(_interBArraDto.ptofinal, _interBArraDto.ptofinal + _interBArraDto.DireccionPataEnFierrado * _SRBE.largoPataFinal);
           
            _listcurve.Add(ladoPataBajo);
            _listcurve.Add(ladoCentral);
            _listcurve.Add(ladoPataArriba);
            _interBArraDto._parametrosInternoRebarDTO._texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoPataBajo.Length + mitadDiam), 0) }+{ Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam * 2), 0) }+{ Math.Round(Util.FootToCm(ladoPataArriba.Length + mitadDiam), 0) })";

            double largoTotal = Math.Round(Util.FootToCm(ladoPataBajo.Length + mitadDiam), 0) + Math.Round(Util.FootToCm(ladoCentral.Length + mitadDiam * 2), 0) + Math.Round(Util.FootToCm(ladoPataArriba.Length + mitadDiam));

            if (largoTotal > 1200) UtilBarras.BarrasMAyores();

            _interBArraDto._parametrosInternoRebarDTO._texToLargoTotal = $"{Math.Round(largoTotal, 0)}";
        }

    }
}
