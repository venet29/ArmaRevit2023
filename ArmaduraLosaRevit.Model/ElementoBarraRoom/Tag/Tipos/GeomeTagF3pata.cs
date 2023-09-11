using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    // CON  PATA 'LISA' PARA ESCALERAS
    public class GeomeTagF3pata : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagF3pata(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagF3pata() { }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M2_1_ReCAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF3pata  ex:${ex.Message}");
                return false;
            }
            return true;

        }
        private void M2_1_ReCAlcularPtosDeTAg()
        {
            //XYZ p0_D = XYZ.Zero; ;
            string tipo = _solicitudBarraDTO.TipoBarra;
            switch (tipo)
            {
                case "f3_ba":
                case "f3_ab":
                    listaTag.RemoveAll(c => c.nombre == "D");
                    XYZ p0_D = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(18), Util.CmToFoot(0));
                    TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D, "D", "M_Path Reinforcement Tag(ID_cuantia_largo)_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
                    listaTag.Add(TagP0_D);

                    listaTag.RemoveAll(c => c.nombre == "B");
                    XYZ p0_B = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-18), Util.CmToFoot(0));
                    TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B, "B", "M_Path Reinforcement Tag(ID_cuantia_largo)_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
                    listaTag.Add(TagP0_B);

                    break;
                case "f3_a0":
                case "f3_b0":

                    listaTag.RemoveAll(c => c.nombre == "B");
                    XYZ p0_B_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p1, _anguloBarraRad, Util.CmToFoot(-18), Util.CmToFoot(0));
                    TagP0_B = M1_1_ObtenerTAgPathBarra(p0_B_, "B", "M_Path Reinforcement Tag(ID_cuantia_largo)_B_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
                    listaTag.Add(TagP0_B);
                    break;
                case "f3_0b":
                case "f3_0a":
                    listaTag.RemoveAll(c => c.nombre == "D");
                    XYZ p0_D_ = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_p2, _anguloBarraRad, Util.CmToFoot(18), Util.CmToFoot(0));
                    TagP0_D = M1_1_ObtenerTAgPathBarra(p0_D_, "D", "M_Path Reinforcement Tag(ID_cuantia_largo)_D_" + escala + "_" + _signoAngulo + Math.Abs(_anguloBArraGrado).ToString(), escala);
                    listaTag.Add(TagP0_D);

                    break;
                default:
                    break;
            }
        }
        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF3pata> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
            string tipo = _solicitudBarraDTO.TipoBarra;
            switch (tipo)
            {
                case "f3_ba":
                case "f3_ab":
                    _geomeTagBase.TagP0_A.IsOk = false;
                    _geomeTagBase.TagP0_E.IsOk = false;
                    break;
                case "f3_a0":
                case "f3_b0":
                    _geomeTagBase.TagP0_A.IsOk = false;
                    _geomeTagBase.TagP0_D.IsOk = false;
                    _geomeTagBase.TagP0_E.IsOk = false;
                    break;
                case "f3_0b":
                case "f3_0a":
                    _geomeTagBase.TagP0_A.IsOk = false;
                    _geomeTagBase.TagP0_B.IsOk = false;
                    _geomeTagBase.TagP0_E.IsOk = false;
                    break;
                default:
                    break;
            }

            // _geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            //_geomeTagBase.TagP0_D.IsOk = false;
            // _geomeTagBase.TagP0_E.IsOk = false;
            // _geomeTagBase.TagP0_C.IsOk = false;

        }
    }

}
