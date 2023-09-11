using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF1_SUP : GeomeTagBaseSX, IGeometriaTag
    {
        private CoordenadasLetra _coordenadasLetra;

        public GeomeTagF1_SUP()
        {
        }

        public GeomeTagF1_SUP(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        {
        }
        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();

                if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) &&
                    Util.RadianeToGrados(AnguloRadian) > 1)
                    M3_DefinirRebarShape_casoEspecial();
                else
                    M3_DefinirRebarShape();



            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF1_SUP  ex:${ex.Message}");
                return false;
            }
            return true;

        }


        public override void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {
            if (_view.Scale == 50)
                M2_CAlcularPtosDeTAg_escala50();
            else if (_view.Scale == 75)
                M2_CAlcularPtosDeTAg_escala75();
            else if (_view.Scale == 100)
                M2_CAlcularPtosDeTAg_escala100();

            //_coordenadasLetra = new CoordenadasLetra(A:new XYZnh(10,-11), B: new XYZnh(-10, 7),
            //                                        C: new XYZnh(10, -11), F: new XYZnh(10, -11), L: new XYZnh(10, -11),
            //                                        D: new XYZnh(10, -11), E:new XYZnh(10, -11));


            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, _coordenadasLetra.A.X, _coordenadasLetra.A.Y);
            TagP0_A = M1_1_ObtenerTAgBarra(p0_A, "A", "M_Path Reinforcement Tag(ID_cuantia_largo)_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);

            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, _coordenadasLetra.B.X, _coordenadasLetra.B.Y);
            TagP0_B = M1_1_ObtenerTAgBarra(p0_B, "B", "M_Path Reinforcement Tag(ID_cuantia_largo)_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);

            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, _coordenadasLetra.C.X, _coordenadasLetra.C.Y);
            TagP0_C = M1_1_ObtenerTAgBarra(p0_C, "C", "M_Path Reinforcement Tag(ID_cuantia_largo)_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, _coordenadasLetra.F.X, _coordenadasLetra.F.Y);
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, _coordenadasLetra.L.X, _coordenadasLetra.L.Y);
            TagP0_L = M1_1_ObtenerTAgBarra(p0_L, "L", "M_Path Reinforcement Tag(ID_cuantia_largo)_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, _coordenadasLetra.D.X, _coordenadasLetra.D.Y);
            TagP0_D = M1_1_ObtenerTAgBarra(p0_D, "D", "M_Path Reinforcement Tag(ID_cuantia_largo)_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, _coordenadasLetra.D.X, _coordenadasLetra.E.Y);
            TagP0_E = M1_1_ObtenerTAgBarra(p0_E, "E", "M_Path Reinforcement Tag(ID_cuantia_largo)_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }

        private void M2_CAlcularPtosDeTAg_escala100()
        {
            if (_ubicacionEnlosa == UbicacionLosa.Inferior || _ubicacionEnlosa == UbicacionLosa.Izquierda)
                _coordenadasLetra = new CoordenadasLetra(A: new XYZnh(-20, -20), B: new XYZnh(-20, 7),
                                         C: new XYZnh(-14, 2), F: new XYZnh(50, 33), L: new XYZnh(75, 2),
                                         D: new XYZnh(10, 10), E: new XYZnh(-8, -18));
            else
                _coordenadasLetra = new CoordenadasLetra(A: new XYZnh(10, -11), B: new XYZnh(-10, 7),
                                        C: new XYZnh(10, 2), F: new XYZnh(-50, 33), L: new XYZnh(-75, 2),
                                        D: new XYZnh(20, 7), E: new XYZnh(-10, -20));
        }

        private void M2_CAlcularPtosDeTAg_escala75()
        {
            if (_ubicacionEnlosa == UbicacionLosa.Inferior || _ubicacionEnlosa == UbicacionLosa.Izquierda)
                _coordenadasLetra = new CoordenadasLetra(A: new XYZnh(10, -12), B: new XYZnh(-15, 7),
                                         C: new XYZnh(-14, 2), F: new XYZnh(50, 25), L: new XYZnh(50, 2),
                                         D: new XYZnh(10, 10), E: new XYZnh(-8, -12));
            else
                _coordenadasLetra = new CoordenadasLetra(A: new XYZnh(10, -11), B: new XYZnh(-15, 7),
                                        C: new XYZnh(20, 2), F: new XYZnh(-50, 25), L: new XYZnh(-50, 2),
                                       D: new XYZnh(10, 7), E: new XYZnh(-10, -12));
        }

        private void M2_CAlcularPtosDeTAg_escala50()
        {
            if ((_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior) &&
                Util.RadianeToGrados(AnguloRadian) > 1) //obs 1
            {
                if (_ubicacionEnlosa == UbicacionLosa.Inferior )
                    _coordenadasLetra = new CoordenadasLetra(A: new XYZnh(-10, -9), B: new XYZnh(-10, 10),
                                       C: new XYZnh(20, 5), F: new XYZnh(-50, 21), L: new XYZnh(-50, 5),
                                       D: new XYZnh(10, 10), E: new XYZnh(-10, -9));

                else
                    _coordenadasLetra = new CoordenadasLetra(A: new XYZnh(10, 10), B: new XYZnh(-10, 10),
                                          C: new XYZnh(-14, 5), F: new XYZnh(50, 21), L: new XYZnh(40, 5),
                                          D: new XYZnh(10, 10), E: new XYZnh(-8, -9));



            }
            else
            {

                if (_ubicacionEnlosa == UbicacionLosa.Inferior || _ubicacionEnlosa == UbicacionLosa.Izquierda)
                    _coordenadasLetra = new CoordenadasLetra(A: new XYZnh(-10, -12), B: new XYZnh(-10, 7),
                                             C: new XYZnh(-14, 2), F: new XYZnh(50, 18), L: new XYZnh(40, 2),
                                             D: new XYZnh(10, 10), E: new XYZnh(-8, -12));
                else
                    _coordenadasLetra = new CoordenadasLetra(A: new XYZnh(10, 7), B: new XYZnh(-10, 7),
                                            C: new XYZnh(20, 2), F: new XYZnh(-50, 18), L: new XYZnh(-50, 2),
                                            D: new XYZnh(10, 7), E: new XYZnh(-10, -12));
            }



        }


        public void M3_DefinirRebarShape() => AsignarPArametros(this);
        //caso especial cuando angulo de pelota de losa es mayor de 90  -> OBS1
        public void M3_DefinirRebarShape_casoEspecial() => AsignarPArametros_caso(this);

        public bool M4_IsFAmiliaValida() => true;

        public void AsignarPArametros(GeomeTagBaseSX _geomeTagBase)
        {
            if (_geomeTagBase._ubicacionEnlosa == UbicacionLosa.Inferior || _geomeTagBase._ubicacionEnlosa == UbicacionLosa.Izquierda)
            {
                _geomeTagBase.TagP0_D.IsOk = false;
                _geomeTagBase.TagP0_E.IsOk = false;
            }
            else
            {
                _geomeTagBase.TagP0_A.IsOk = false;
                _geomeTagBase.TagP0_B.IsOk = false;

                _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_B);
                _geomeTagBase.TagP0_E.CAmbiar(_geomeTagBase.TagP0_A);
            }

        }

        //caso especial cuando angulo de pelota de losa es mayor de 90 -> OBS1
        public void AsignarPArametros_caso(GeomeTagBaseSX _geomeTagBase)
        {
            if (_geomeTagBase._ubicacionEnlosa == UbicacionLosa.Inferior)
            {
                _geomeTagBase.TagP0_A.IsOk = false;
                _geomeTagBase.TagP0_B.IsOk = false;

                _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_B);
                _geomeTagBase.TagP0_E.CAmbiar(_geomeTagBase.TagP0_A);
            }
            else
            {
                _geomeTagBase.TagP0_D.IsOk = false;
                _geomeTagBase.TagP0_E.IsOk = false;
         
            }

        }
    }


}
