using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF9 : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagF9(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
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
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF9  ex:${ex.Message}");
                return false;
            }
            return true;

        }
        public void M3_DefinirRebarShape()
        {
            listaTag.RemoveAll(c => c.nombre == "A");
            XYZ p0_A = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(55), Util.CmToFoot(-28));
            TagP0_A = M1_1_ObtenerTAgPathBarra(p0_A, "A", "M_Path Reinforcement Tag(ID_cuantia_largo)_A_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_A);

            listaTag.RemoveAll(c => c.nombre == "B");
            XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-12), Util.CmToFoot(-10));
            TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", "M_Path Reinforcement Tag(ID_cuantia_largo)_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_B);



            listaTag.RemoveAll(c => c.nombre == "D");
            XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(13), Util.CmToFoot(-10));
            TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", "M_Path Reinforcement Tag(ID_cuantia_largo)_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_D);

            listaTag.RemoveAll(c => c.nombre == "E");
            XYZ p0_E = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(-28));
            TagP0_E = M1_1_ObtenerTAgPathBarra(p0_E, "E", "M_Path Reinforcement Tag(ID_cuantia_largo)_E_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_E);

        }

        public bool M4_IsFAmiliaValida() => true;

        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF9> rutina)
        {
            rutina(this);
        }
    }
}
