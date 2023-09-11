using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    // CON  PATA 'LISA' PARA ESCALERAS
    public class GeomeTagF3_incli : GeomeTagBase, IGeometriaTag
    {
        private double elevacion;
        private  RebarInferiorDTO _rebarInferiorDTO1;
        private double _diferenciaZ;
        private VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaInicialExternaInclinadaDTO;
        private VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaFinalExternaInclinadaDTO;

        public GeomeTagF3_incli(Document doc, RebarInferiorDTO rebarInferiorDTO1) :
            base(doc, rebarInferiorDTO1)
        {
            this.elevacion = doc.ActiveView.GenLevel.Elevation;
            this._rebarInferiorDTO1 = rebarInferiorDTO1;
      
        }


        public GeomeTagF3_incli() { }

        public void Ejecutar(GeomeTagArgs args)
        {
            double AnguloRadian = args.angulorad;
            _diferenciaZ = args.diferenciaZInicialFinal;
            _vectorDireccionLosaInicialExternaInclinadaDTO= args._vectorDireccionLosaInicialExternaInclinadaDTO;
            _vectorDireccionLosaFinalExternaInclinadaDTO = args._vectorDireccionLosaFinalExternaInclinadaDTO;
          
            M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
            M2_CAlcularPtosDeTAg();
            M2_1_ReCAlcularPtosDeTAg(AnguloRadian);
            M3_DefinirRebarShape();
        }
        private void M2_1_ReCAlcularPtosDeTAg(double anguloBarraRad)
        {
            ConstantesGenerales.sbLog.AppendLine($"GeomeTagF3_inclixxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

            _p1 = _p1.AsignarZ(elevacion);
            _p2 = _p2.AsignarZ(elevacion);
            ConstantesGenerales.sbLog.AppendLine($" datos originales");
            ConstantesGenerales.sbLog.AppendLine($" p1={_p1}       p2 ={_p2}    angulo={anguloBarraRad}");

            M2_1_1_ObtenerCoordenadasTrasladadasEnXYP1_P2(anguloBarraRad);

            M2_1_2_RecalcularOrientacion();

            XYZ _ptoMouse = ObtenerNuevoptoMouseANivelView(elevacion);
            TagP0_A = AgregarTAgLitsta("A", -18, 8, _p1);
            TagP0_C = AgregarTAgLitsta("C", 18, 8, _p2);
            switch (_ubicacionEnlosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:
                    //  _p1 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, anguloBarraRad + Math.PI / 2, Math.Abs(_diferenciaZ/2));
                    //   _p2 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, anguloBarraRad - Math.PI / 2, Math.Abs(_diferenciaZ / 2));
                  //  XYZ _ptoMouse = ObtenerNuevoptoMouseANivelView(elevacion);

                    //TagP0_A = AgregarTAgLitsta("A", -18, 8, _p1);

                    TagP0_B = AgregarTAgLitsta("B", 30, -20, _ptoMouse);

                 //   TagP0_C = AgregarTAgLitsta("C", 18, 8, _p2);

                    TagP0_L = AgregarTAgLitsta("L", 40, 5, _ptoMouse);

                    TagP0_F = AgregarTAgLitsta("F", -30, 10, _ptoMouse);

                    break;
                case UbicacionLosa.Inferior:
                case UbicacionLosa.Izquierda:

                    //  _p1 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, anguloBarraRad - Math.PI / 2, _diferenciaZ / 2);
                    //  _p2 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, anguloBarraRad + Math.PI / 2, _diferenciaZ / 2);
                 //   _ptoMouse = ObtenerNuevoptoMouseANivelView(elevacion);
                 //   TagP0_A = AgregarTAgLitsta("A", -18, 8, _p1);

                    TagP0_B = AgregarTAgLitsta("B", 30, -10, _ptoMouse);

                 //   TagP0_C = AgregarTAgLitsta("C", 18, 8, _p2);

                    TagP0_L = AgregarTAgLitsta("L", 40, 20, _ptoMouse);

                    TagP0_F = AgregarTAgLitsta("F", -30, 5, _ptoMouse);

                    ConstantesGenerales.sbLog.AppendLine($" datos movidos");
                    ConstantesGenerales.sbLog.AppendLine($" p1={_p1}       p2 ={_p2}");
                    break;
                case UbicacionLosa.NONE:
                    break;
                default:
                    break;
            }
        }

        private void M2_1_1_ObtenerCoordenadasTrasladadasEnXYP1_P2(double anguloBarraRad)
        {
            if (Math.Abs(_diferenciaZ) > 0.01)
            {
                _p1 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, anguloBarraRad + (_diferenciaZ > 0 ? -1 : 1) * Math.PI / 2, Math.Abs(_diferenciaZ / 2));
                _p2 = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, anguloBarraRad + (_diferenciaZ > 0 ? 1 : -1) * Math.PI / 2, Math.Abs(_diferenciaZ / 2));
            }
        }
        private void M2_1_2_RecalcularOrientacion()
        {
            if (_diferenciaZ > 0.01)
            {
                if (_ubicacionEnlosa == UbicacionLosa.Derecha) _ubicacionEnlosa =UbicacionLosa.Izquierda;
                if (_ubicacionEnlosa == UbicacionLosa.Superior) _ubicacionEnlosa = UbicacionLosa.Inferior;
            }
            else
            {
                if (_ubicacionEnlosa == UbicacionLosa.Izquierda) _ubicacionEnlosa = UbicacionLosa.Derecha;
                if (_ubicacionEnlosa == UbicacionLosa.Inferior) _ubicacionEnlosa = UbicacionLosa.Superior;
            }
        }
        //private XYZ ObtenerNuevoptoCirculo()
        //{
        //  XYZ  ptoMouseAnivelVista = Util.IntersectionXYZ(_p1.GetXY0(), _p2.GetXY0(), _rebarInferiorDTO1.PtoDirectriz1.GetXY0(), _rebarInferiorDTO1.PtoDirectriz2.GetXY0()).AsignarZ(elevacion);

        //    return ptoMouseAnivelVista;
        //}

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF3_incli> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
            if (!_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada) _geomeTagBase.TagP0_A.IsOk = false;
            if (!_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada) _geomeTagBase.TagP0_C.IsOk = false;
            _geomeTagBase.TagP0_D.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;

        }
    }


}
