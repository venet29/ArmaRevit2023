using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagS4 : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagS4(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagS4() { }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagS4> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {

            if (_view.Scale == 50)
                M2_CAlcularPtosDeTAg_escala50();
            else if (_view.Scale == 75)
                M2_CAlcularPtosDeTAg_escala75();
            else if (_view.Scale == 100)
                M2_CAlcularPtosDeTAg_escala100();


   


            _geomeTagBase.TagP0_A.IsOk = false;
            _geomeTagBase.TagP0_B.IsOk = false;
            _geomeTagBase.TagP0_D.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;
            _geomeTagBase.TagP0_C.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }

        private void M2_CAlcularPtosDeTAg_escala50()
        {
            listaTag.RemoveAll(c => c.nombre == "C");
            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(-20));
            TagP0_C = M1_1_ObtenerTAgPathBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            listaTag.RemoveAll(c => c.nombre == "F");
            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-55), Util.CmToFoot(-10));
            TagP0_F = M1_1_ObtenerTAgPathBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            listaTag.RemoveAll(c => c.nombre == "L");
            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(-20));
            TagP0_L = M1_1_ObtenerTAgPathBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
        }

        private void M2_CAlcularPtosDeTAg_escala75()
        {
            listaTag.RemoveAll(c => c.nombre == "C");
            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(-20));
            TagP0_C = M1_1_ObtenerTAgPathBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            listaTag.RemoveAll(c => c.nombre == "F");
            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-55), Util.CmToFoot(-10));
            TagP0_F = M1_1_ObtenerTAgPathBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            listaTag.RemoveAll(c => c.nombre == "L");
            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(-20));
            TagP0_L = M1_1_ObtenerTAgPathBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
        }

        private void M2_CAlcularPtosDeTAg_escala100()
        {
            listaTag.RemoveAll(c => c.nombre == "C");
            XYZ p0_C = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-45), Util.CmToFoot(-20));
            TagP0_C = M1_1_ObtenerTAgPathBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_C);

            listaTag.RemoveAll(c => c.nombre == "F");
            XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-55), Util.CmToFoot(-10));
            TagP0_F = M1_1_ObtenerTAgPathBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_F);

            listaTag.RemoveAll(c => c.nombre == "L");
            XYZ p0_L = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(50), Util.CmToFoot(-20));
            TagP0_L = M1_1_ObtenerTAgPathBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
            listaTag.Add(TagP0_L);
        }
    }
}
