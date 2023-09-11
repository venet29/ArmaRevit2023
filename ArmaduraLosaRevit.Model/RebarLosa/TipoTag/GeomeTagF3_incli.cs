using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.TipoTag
{
    // CON  PATA 'LISA' PARA ESCALERAS
    public class GeomeTagF3_incli : GeomeTagBaseRebar, IGeometriaTag
    {
        private double elevacion;
#pragma warning disable CS0108 // 'GeomeTagF3_incli._rebarInferiorDTO1' hides inherited member 'GeomeTagBase._rebarInferiorDTO1'. Use the new keyword if hiding was intended.
        private RebarInferiorDTO _rebarInferiorDTO1;
#pragma warning restore CS0108 // 'GeomeTagF3_incli._rebarInferiorDTO1' hides inherited member 'GeomeTagBase._rebarInferiorDTO1'. Use the new keyword if hiding was intended.
        private double _diferenciaZ;
        private VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaInicialExternaInclinadaDTO;
        private VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaFinalExternaInclinadaDTO;
        private int _numeroTramos;
        private DireccionPata _ubicacionPata;

        public GeomeTagF3_incli(Document doc, RebarInferiorDTO rebarInferiorDTO1) :
            base(doc, rebarInferiorDTO1)
        {
      
            this._rebarInferiorDTO1 = rebarInferiorDTO1;

        }



        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                var resultZ = _doc.ActiveView.Obtener_Z_SoloPLantas();
                if (!resultZ.Isok) return false;

                this.elevacion = resultZ.valorz;
                _diferenciaZ = args.diferenciaZInicialFinal;
                _vectorDireccionLosaInicialExternaInclinadaDTO = args._vectorDireccionLosaInicialExternaInclinadaDTO;
                _vectorDireccionLosaFinalExternaInclinadaDTO = args._vectorDireccionLosaFinalExternaInclinadaDTO;
                _numeroTramos = args._numeroTramosbarra;
                _ubicacionPata = args._ubicacionPata;

                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M2_1_ReCAlcularPtosDeTAg(AnguloRadian);
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF3_incli  ex:${ex.Message}");
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

            _p1 = _p1.AsignarZ(elevacion);
            _p2 = _p2.AsignarZ(elevacion);

            M2_1_1_ObtenerCoordenadasTrasladadasEnXYP1_P2(anguloBarraRad);

            M2_1_2_RecalcularOrientacion();

            XYZ _ptoMouse = ObtenerNuevoptoMouseANivelView(elevacion);


            if (_numeroTramos == 1)
            {// TagP0_A = AgregarTagPathreinLitsta("A", 30, -10, _ptoMouse); }
            }
            else if (_numeroTramos == 2)
            {
                if (_ubicacionPata == DireccionPata.IzqInf)
                { TagP0_A = AgregarTagRebarLista("A", -18, 8, _p1, $"MRA Rebar_A_" + escala); }
                else if (_ubicacionPata == DireccionPata.DereSup)
                { TagP0_A = AgregarTagRebarLista("A", 18, 8, _p2, $"MRA Rebar_A_" + escala); }

                TagP0_B = AgregarTagRebarLista("B", 30, -10, _ptoMouse,$"MRA Rebar_B_" + escala);
            }
            else if (_numeroTramos == 3)
            {
                TagP0_A = AgregarTagRebarLista("A", -18, 8, _p1, $"MRA Rebar_A_" + escala);
                TagP0_B = AgregarTagRebarLista("B", 30, -10, _ptoMouse, $"MRA Rebar_B_" + escala);
                TagP0_C = AgregarTagRebarLista("CLosa", 18, 0, _p2, $"MRA Rebar_CLosa_" + escala);
            }

            switch (_ubicacionEnlosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:


                    TagP0_L = AgregarTagRebarLista("L", 70, 0, _ptoMouse, $"MRA Rebar_L_" + escala);

                    TagP0_F = AgregarTagRebarLista("F", -100, 15, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);

                    break;
                case UbicacionLosa.Inferior:
                case UbicacionLosa.Izquierda:
                    TagP0_L = AgregarTagRebarLista("L", 70, 0, _ptoMouse, $"MRA Rebar_L_" + escala);

                    TagP0_F = AgregarTagRebarLista("F", -100, 15, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);

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
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF3_incli> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {

            if (_numeroTramos == 1)
            {
                _geomeTagBase.TagP0_A.IsOk = false;
                _geomeTagBase.TagP0_B.IsOk = false;
                _geomeTagBase.TagP0_C.IsOk = false;
                _geomeTagBase.TagP0_D.IsOk = false;
                _geomeTagBase.TagP0_E.IsOk = false;
            }
            else if (_numeroTramos == 2)
            {
                _geomeTagBase.TagP0_C.IsOk = false;
                _geomeTagBase.TagP0_D.IsOk = false;
                _geomeTagBase.TagP0_E.IsOk = false;
            }
            else if (_numeroTramos == 3)
            {
    
                _geomeTagBase.TagP0_D.IsOk = false;
                _geomeTagBase.TagP0_E.IsOk = false;
            }

        }


    }


}
