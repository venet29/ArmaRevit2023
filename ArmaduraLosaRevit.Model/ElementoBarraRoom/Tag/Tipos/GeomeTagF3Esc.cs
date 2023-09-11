using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.TipoTag;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF3Esc : GeomeTagBaseRebar, IGeometriaTag
    {
        public GeomeTagF3Esc(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagF3Esc(Document doc, RebarInferiorDTO rebarInferiorDTO1) :
                 base(doc, rebarInferiorDTO1)
        {
        }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAgRebar();
                M2_1_ReCAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF3Esc  ex:${ex.Message}");
                return false;
            }
            return true;
        }
        private void M2_1_ReCAlcularPtosDeTAg()
        {
            if (_view.Scale == 50)
                M2_1_ReCAlcularPtosDeTAg_escala50();
            else if (_view.Scale == 75)
                M2_CAlcularPtosDeTAgRebar_escala75();
            else if (_view.Scale == 100)
                M2_CAlcularPtosDeTAgRebar_escala100();
        }

        private void M2_1_ReCAlcularPtosDeTAg_escala50()
        {
            //XYZ p0_D = XYZ.Zero; ;
            string tipo = $"{_solicitudBarraDTO.TipoBarra}_{_solicitudBarraDTO.UbicacionEnlosa}";
            switch (tipo)
            {
                // Derecha, Izquierda, Superior, Inferior, NONE
                case "f3_esc45_Derecha":
                case "f3_esc45_Superior":
                    TagP0_A = AgregarTagRebarLista("A", -10, -30, _p1, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", -40, -15, _p1, $"MRA Rebar_B_" + escala);
                    break;
                case "f3_esc45_Izquierda":
                case "f3_esc45_Inferior":

                    TagP0_D = AgregarTagRebarLista("D", 50, -15, _p2, $"MRA Rebar_D_" + escala);
                    break;
                case "f3_esc135_Derecha":
                case "f3_esc135_Superior":
                    TagP0_B = AgregarTagRebarLista("B", -50, 15, _p1, $"MRA Rebar_B_" + escala);
                    break;
                case "f3_esc135_Izquierda":
                case "f3_esc135_Inferior":
                    TagP0_D = AgregarTagRebarLista("D", 50, 15, _p2, $"MRA Rebar_D_" + escala);
                    break;
                default:
                    break;
            }

            TagP0_F = AgregarTagRebarLista("F", -55, 7, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);
        }
        private void M2_CAlcularPtosDeTAgRebar_escala75()
        {
            //falsta implementar
            M2_1_ReCAlcularPtosDeTAg_escala50();
        }
        private void M2_CAlcularPtosDeTAgRebar_escala100()
        {
            //falsta implementar
            M2_1_ReCAlcularPtosDeTAg_escala50();
        }


        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF3Esc> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
            string tipo = $"{_solicitudBarraDTO.TipoBarra}_{_solicitudBarraDTO.UbicacionEnlosa}";
            switch (tipo)
            {


                case "f3_esc45_Izquierda":
                case "f3_esc45_Inferior":
                case "f3_esc135_Izquierda":
                case "f3_esc135_Inferior":
                    _geomeTagBase.TagP0_A.IsOk = false;
                    _geomeTagBase.TagP0_B.IsOk = false;
                    _geomeTagBase.TagP0_E.IsOk = false;

                    _geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_B);
                    _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_A);
                    break;
                case "f3_esc45_Derecha":
                case "f3_esc45_Superior":
                case "f3_esc135_Derecha":
                case "f3_esc135_Superior":
                    _geomeTagBase.TagP0_A.IsOk = false;
                    _geomeTagBase.TagP0_D.IsOk = false;
                    _geomeTagBase.TagP0_E.IsOk = false;

                    _geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_B);
                    _geomeTagBase.TagP0_B.CAmbiar(_geomeTagBase.TagP0_A);
                    break;

                default:
                    break;
            }


        }
    }
}
