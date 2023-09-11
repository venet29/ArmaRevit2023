using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades
{
    public class WraperRebarLargo
    {
        public CrearTrasformadaSobreVectorDesg _Trasform { get; set; }
        public Curve _curve { get; set; }
        public TipoCUrva TipoCurva { get; set; }
        public XYZ ptoInicial { get; set; }
        public XYZ ptoMedio { get; set; }
        public XYZ ptoFinal { get; set; }
        public XYZ ptoCentroArc { get; set; }

        public FijacionRebar FijacionInicial { get; set; }
        public FijacionRebar FijacionFinal { get; set; }
        public bool IsCurvaSeleccionada { get; set; }
        public parametrosRebar ParametrosRebar { get; }
        public bool IsBarraPrincipal { get; set; }
        public bool alargarInicio { get; set; }
        public XYZ direccion { get; set; }
        public bool alargarFin { get; set; }
        public bool IsOK { get; set; }

        //**para corte
        public XYZ PtoInicialTransformada { get; internal set; }
        public XYZ PtoFinalTransformada { get; internal set; }
        public XYZ PtoMedioTransformada { get; internal set; }
        public double distanciaPtoSeleccion { get; private set; }
        public double distanciaPtoSeleccion_api { get; private set; }

        public WraperRebarLargo(Curve item, parametrosRebar parametrosRebar, bool isBarraPrincipal)
        {
            this._curve = item;
            TipoCurva = (item is Arc ? TipoCUrva.arco : TipoCUrva.linea);
            ParametrosRebar = parametrosRebar;
            IsBarraPrincipal = isBarraPrincipal;
            alargarInicio = false;
            alargarFin = false;
            FijacionInicial = FijacionRebar.fijo;
            FijacionFinal = FijacionRebar.fijo;


        }
        public void DatosIniciales()
        {
            ptoInicial = _curve.GetEndPoint(0);
            ptoFinal = _curve.GetEndPoint(1);
            ptoMedio = _curve.Evaluate(0.5, true);
            direccion = (ptoFinal - ptoInicial).Normalize();

            if (TipoCurva == TipoCUrva.arco)
            {
                ptoCentroArc = ((Arc)_curve).Center;
                //Debug.WriteLine($" Curva   ---->  pInic: {ptoInicial.REdondearString(3)}\n pmedio : {ptoMedio.REdondearString(3)}   pmedio(true)  {_curve.Evaluate(0.5 ,true)} \npfinal : {ptoFinal.REdondearString(3)}  dire:{direccion} ");
            }
            //  else
            //   Debug.WriteLine($" Linea   ---->  PInic: {ptoInicial.REdondearString(3)}\n pmedio : {ptoMedio.REdondearString(3)}   pmedio(true)  {_curve.Evaluate(0.5, true)} \npfinal : {ptoFinal.REdondearString(3)}  dire:{direccion} ");
        }
        public void Generar(XYZ puntoSobreBArra)
        {
            try
            {
               XYZ ptoSobre= ((Line)_curve).ProjectSINExtendida3D(puntoSobreBArra);
                distanciaPtoSeleccion = ptoSobre.DistanceTo(puntoSobreBArra);
                distanciaPtoSeleccion_api = _curve.Distance(puntoSobreBArra);
                if (distanciaPtoSeleccion_api > 0.1) return;

                ReGenerar(puntoSobreBArra);
            }
            catch (Exception)
            {

                IsOK = false;
            }
            IsOK = true;
        }
        public void ReGenerar(XYZ puntoSobreBArra)
        {
            try
            {
                IsCurvaSeleccionada = true;
                double distaptoIni = puntoSobreBArra.DistanceTo(ptoInicial);
                double distaptoFinal = puntoSobreBArra.DistanceTo(ptoFinal);
                if (distaptoIni > distaptoFinal)
                    alargarFin = true;
                else
                    alargarInicio = true;
            }
            catch (Exception)
            {

                IsOK = false;
            }
            IsOK = true;
        }

        public WraperRebarLargo GenerarTrasformada(CrearTrasformadaSobreVectorDesg trasform)
        {
            try
            {
                _Trasform = trasform;
                Curve _AUx_trasncurve = null;
                XYZ AUXTranf_ptoInicial = _Trasform.EjecutarTransform(ptoInicial);
                XYZ AUXTranf_ptoMedio = _Trasform.EjecutarTransform(ptoMedio);
                XYZ AUXTranf_ptoFinal = _Trasform.EjecutarTransform(ptoFinal);

                XYZ AUXTranf_ptoCentroArc = ptoCentroArc;
                if (ptoCentroArc!=null)
                    AUXTranf_ptoCentroArc = _Trasform.EjecutarTransform(ptoCentroArc);

                if (TipoCurva == TipoCUrva.arco)
                {
                    _AUx_trasncurve = Arc.Create(AUXTranf_ptoInicial, AUXTranf_ptoFinal, AUXTranf_ptoCentroArc);

                }
                else if (TipoCurva == TipoCUrva.linea)
                {
                    _AUx_trasncurve = Line.CreateBound(AUXTranf_ptoInicial, AUXTranf_ptoFinal);
                }

                return new WraperRebarLargo(_curve, ParametrosRebar, IsBarraPrincipal)
                {
                    _Trasform = _Trasform,
                    _curve = _AUx_trasncurve,
                    TipoCurva = TipoCurva,
                    ptoInicial = AUXTranf_ptoInicial,
                    ptoMedio = AUXTranf_ptoMedio,
                    ptoFinal = AUXTranf_ptoFinal,
                    ptoCentroArc = AUXTranf_ptoCentroArc,

                    FijacionInicial = FijacionInicial,
                    FijacionFinal = FijacionFinal,
                    IsCurvaSeleccionada = IsCurvaSeleccionada,

                    IsBarraPrincipal = IsBarraPrincipal,
                    alargarInicio = alargarInicio,
                    direccion = (AUXTranf_ptoFinal - AUXTranf_ptoInicial).Normalize(),
                    alargarFin = alargarFin,
                    IsOK = IsOK,
                    PtoInicialTransformada = PtoInicialTransformada,
                    PtoFinalTransformada = PtoFinalTransformada,
                    PtoMedioTransformada = PtoMedioTransformada,
                };
            }
            catch (Exception)
            {

                IsOK = false;
            }
            IsOK = true;

            return null;
        }


        public WraperRebarLargo GenerarTrasformada(Func<XYZ, XYZ> functionToPass)
        {
            try
            {
              
                Curve _AUx_trasncurve = null;
                XYZ AUXTranf_ptoInicial = functionToPass(ptoInicial);
                XYZ AUXTranf_ptoMedio = functionToPass(ptoMedio);
                XYZ AUXTranf_ptoFinal = functionToPass(ptoFinal);

                XYZ AUXTranf_ptoCentroArc = ptoCentroArc;
                if (ptoCentroArc != null)
                    AUXTranf_ptoCentroArc = _Trasform.EjecutarTransformInvertida(ptoCentroArc);

                if (TipoCurva == TipoCUrva.arco)
                {
                    _AUx_trasncurve = Arc.Create(AUXTranf_ptoInicial, AUXTranf_ptoFinal, AUXTranf_ptoCentroArc);

                }
                else if (TipoCurva == TipoCUrva.linea)
                {
                    _AUx_trasncurve = Line.CreateBound(AUXTranf_ptoInicial, AUXTranf_ptoFinal);
                }

                return new WraperRebarLargo(_curve, ParametrosRebar, IsBarraPrincipal)
                {
            
                    _curve = _AUx_trasncurve,
                    TipoCurva = TipoCurva,
                    ptoInicial = AUXTranf_ptoInicial,
                    ptoMedio = AUXTranf_ptoMedio,
                    ptoFinal = AUXTranf_ptoFinal,
                    ptoCentroArc = AUXTranf_ptoCentroArc,

                    FijacionInicial = FijacionInicial,
                    FijacionFinal = FijacionFinal,
                    IsCurvaSeleccionada = IsCurvaSeleccionada,

                    IsBarraPrincipal = IsBarraPrincipal,
                    alargarInicio = alargarInicio,
                    direccion = (AUXTranf_ptoFinal - AUXTranf_ptoInicial).Normalize(),
                    alargarFin = alargarFin,
                    IsOK = IsOK,
                    PtoInicialTransformada = PtoInicialTransformada,
                    PtoFinalTransformada = PtoFinalTransformada,
                    PtoMedioTransformada = PtoMedioTransformada,
                };
            }
            catch (Exception)
            {

                IsOK = false;
            }
            IsOK = true;

            return null;
        }

    }
}