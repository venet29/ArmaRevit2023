using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.TipoTag
{
    // CON  PATA 'LISA' PARA ESCALERAS
    public class GeomeTagF11_rebar : GeomeTagBaseRebar, IGeometriaTag
    {
        private double elevacion;
    //    private RebarInferiorDTO _rebarInferiorDTO1;
        private double _diferenciaZ;
      //  private VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaInicialExternaInclinadaDTO;
       // private VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaFinalExternaInclinadaDTO;
        private BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto_IzqInf;
        private BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto_DereSup;
        private double _largoRecorridoDeltaZ;
        private double _deltaZ;

        public GeomeTagF11_rebar(Document doc, RebarInferiorDTO rebarInferiorDTO1) :
            base(doc, rebarInferiorDTO1)
        { 
            this._rebarInferiorDTO1 = rebarInferiorDTO1;
        }


        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                _diferenciaZ = args.diferenciaZInicialFinal;

                var resultZ = _doc.ActiveView.Obtener_Z_SoloPLantas();
                if (!resultZ.Isok) return false;
                this.elevacion = resultZ.valorz;

                _buscarFaceSuperiorConPto_IzqInf = args._buscarFaceSuperiorConPto_IzqInf;
                _buscarFaceSuperiorConPto_DereSup = args._buscarFaceSuperiorConPto_DereSup;
                _largoRecorridoDeltaZ = args.largoREcorridoDeltaZ;
                _deltaZ = args.deltaZ;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAgRebar();
                M2_1_ReCAlcularPtosDeTAg(AnguloRadian);
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF4_incli  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        private void M2_1_ReCAlcularPtosDeTAg(double anguloBarraRad)
        {
            if (_view.Scale == 50)
                M2_1_ReCAlcularPtosDeTAg_escala50(anguloBarraRad);
            else if (_view.Scale == 75)
                M2_CAlcularPtosDeTAgRebar_escala75(anguloBarraRad);
            else if (_view.Scale == 100)
                M2_CAlcularPtosDeTAgRebar_escala100(anguloBarraRad);
        }


        private void M2_1_ReCAlcularPtosDeTAg_escala50(double anguloBarraRad)
        {
            ConstNH.sbLog.AppendLine($"GeomeTagF11_inclixxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

            _p1 = _p1.AsignarZ(elevacion);
            _p2 = _p2.AsignarZ(elevacion);
            XYZ _ptoMouse = ObtenerNuevoptoMouseANivelView(elevacion);

            double recoridP1_pmouse = _p1.DistanceTo(_ptoMouse);
            double recoridP2_pmouse = _p2.DistanceTo(_ptoMouse);

            M2_1_1_ObtenerCoordenadasTrasladadasEnXYP1_P2(anguloBarraRad);

            M2_1_2_RecalcularOrientacion();




            double dist1 = _p1.DistanceTo(_ptoMouse);

           // TagP0_A = AgregarTagRebarLista("A", 30, (int)Util.FootToCm(_buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorConRecub_foot) + 10, _p1, $"MRA Rebar_A_" + escala);
            TagP0_B = AgregarTagRebarLista("B", -10, 8, _p1, $"MRA Rebar_B_" + escala);

            TagP0_C = AgregarTagRebarLista("CLosa", 18, -15 , _ptoMouse, $"MRA Rebar_CLosa_" + escala);

            TagP0_D = AgregarTagRebarLista("D", 10, 8, _p2, $"MRA Rebar_D_" + escala);
            //TagP0_E = AgregarTAgLitsta("E", -40, (int)Util.FootToCm(_buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorConRecub) + 2 + (int)(_p2.DistanceTo(_ptoMouse) * _deltaZ / _largoRecorridoDeltaZ), _p2);
            // double aux1 = (int)Util.FootToCm(_buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorConRecub) + 2;
            // double aux2 = (int)Util.FootToCm((_p2.DistanceTo(_ptoMouse) * _deltaZ / _largoRecorridoDeltaZ));
          //  TagP0_E = AgregarTagRebarLista("E", -40, (int)Util.FootToCm(_buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorConRecub_foot) + 10, _p2, $"MRA Rebar_E_" + escala);// (int)Util.FootToCm(recoridP2_pmouse * _deltaZ / _largoRecorridoDeltaZ), _p2);; ;// ;// ;
            switch (_ubicacionEnlosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:

                    // TagP0_A = AgregarTAgLitsta("A", -18, 8, _p1);
                    // TagP0_B = AgregarTAgLitsta("C", 30, -20, _ptoMouse);
                    //  TagP0_C = AgregarTAgLitsta("C", 18, 8, _p2);

                    TagP0_L = AgregarTagRebarLista("L", 60, 20, _ptoMouse, $"MRA Rebar_L_" + escala);
                    //TagP0_F = AgregarTagPathreinLitsta("F", -40, 10 - (int)(20 * _deltaZ / _largoRecorridoDeltaZ), _ptoMouse);
                    TagP0_F = AgregarTagRebarLista("F", -60, 10, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);

                    break;
                case UbicacionLosa.Inferior:
                case UbicacionLosa.Izquierda:
                    // TagP0_A = AgregarTAgLitsta("A", -18, 8, _p1); //
                    // TagP0_B = AgregarTAgLitsta("B", 30, -10, _ptoMouse);
                    //  TagP0_C = AgregarTAgLitsta("C", 18, 8, _p2);//

                    TagP0_L = AgregarTagRebarLista("L", 60, 0, _ptoMouse, $"MRA Rebar_L_" + escala);
                    //TagP0_F = AgregarTagPathreinLitsta("F", -35, 10, _ptoMouse);
                    TagP0_F = AgregarTagRebarLista("F", -90, 10, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);
                    break;
                case UbicacionLosa.NONE:
                    break;
                default:
                    break;
            }
        }

        private void M2_CAlcularPtosDeTAgRebar_escala75(double anguloBarraRad)
        {
            //falsta implementar
            M2_1_ReCAlcularPtosDeTAg_escala50(anguloBarraRad);
        }
        private void M2_CAlcularPtosDeTAgRebar_escala100(double anguloBarraRad)
        {
            //falsta implementar
            M2_1_ReCAlcularPtosDeTAg_escala50(anguloBarraRad);
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
                if (_ubicacionEnlosa == UbicacionLosa.Derecha) _ubicacionEnlosa = UbicacionLosa.Izquierda;
                if (_ubicacionEnlosa == UbicacionLosa.Superior) _ubicacionEnlosa = UbicacionLosa.Inferior;
            }
            else
            {
                if (_ubicacionEnlosa == UbicacionLosa.Izquierda) _ubicacionEnlosa = UbicacionLosa.Derecha;
                if (_ubicacionEnlosa == UbicacionLosa.Inferior) _ubicacionEnlosa = UbicacionLosa.Superior;
            }
        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF11_rebar> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
            _geomeTagBase.TagP0_A.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;

        }
    }


}

