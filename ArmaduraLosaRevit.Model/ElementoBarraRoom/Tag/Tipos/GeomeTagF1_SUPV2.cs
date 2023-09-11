using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF1_SUPV2 : GeomeTagBaseSX, IGeometriaTag
    {
        public GeomeTagF1_SUPV2()
        {
        }

        public GeomeTagF1_SUPV2(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        {
        }
        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar GeomeTagF1_SUPV2  ex:${ex.Message}");
                return false;
            }
            return true;

        }


        public override void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(10), Util.CmToFoot(-12));
            TagP0_A = M1_1_ObtenerTAgBarra(p0_A, "A", "M_Path Reinforcement Tag(ID_cuantia_largo)_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);

            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-10), Util.CmToFoot(7));
            TagP0_B = M1_1_ObtenerTAgBarra(p0_B, "B", "M_Path Reinforcement Tag(ID_cuantia_largo)_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);

            if (_ubicacionEnlosa == UbicacionLosa.Inferior || _ubicacionEnlosa == UbicacionLosa.Izquierda)
                ConfiguracionLadoIzqInf();
            else
                ConfiguracionLadoDereSup();

            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(10), Util.CmToFoot(10));
            TagP0_D = M1_1_ObtenerTAgBarra(p0_D, "D", "M_Path Reinforcement Tag(ID_cuantia_largo)_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-8), Util.CmToFoot(-12));
            TagP0_E = M1_1_ObtenerTAgBarra(p0_E, "E", "M_Path Reinforcement Tag(ID_cuantia_largo)_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);
        }

        private void ConfiguracionLadoIzqInf()
        {
            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-14), Util.CmToFoot(2));
            TagP0_C = M1_1_ObtenerTAgBarra(p0_C, "C", "M_Path Reinforcement Tag(ID_cuantia_largo)_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(18));
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(40), Util.CmToFoot(2));
            TagP0_L = M1_1_ObtenerTAgBarra(p0_L, "L", "M_Path Reinforcement Tag(ID_cuantia_largo)_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
        }
        private void ConfiguracionLadoDereSup()
        {
            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(20), Util.CmToFoot(2));
            TagP0_C = M1_1_ObtenerTAgBarra(p0_C, "C", "M_Path Reinforcement Tag(ID_cuantia_largo)_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-50), Util.CmToFoot(18));
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", "M_Path Reinforcement Tag(ID_cuantia_largo)_S1_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-50), Util.CmToFoot(2));
            TagP0_L = M1_1_ObtenerTAgBarra(p0_L, "L", "M_Path Reinforcement Tag(ID_cuantia_largo)_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
        }
        public void M3_DefinirRebarShape() => AsignarPArametros(this);

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


    }


}
