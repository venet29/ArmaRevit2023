using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    // CON PATA 'C' HACIA ARRIBA PARA ESCALERAS
    public class GeomeTagF3BEsc : GeomeTagBase, IGeometriaTag
    {
        public GeomeTagF3BEsc(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagF3BEsc() { }

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
                Util.ErrorMsg($"Error ejecutar TagF3BEsc  ex:${ex.Message}");
                return false;
            }
            return true;

        }
        private void M2_1_ReCAlcularPtosDeTAg()
        {
            //XYZ p0_D = XYZ.Zero; ;
            string tipo = $"{_solicitudBarraDTO.TipoBarra}_{_solicitudBarraDTO.UbicacionEnlosa}";
            switch (tipo)
            {
                // Derecha, Izquierda, Superior, Inferior, NONE
                case "f3b_esc45_Derecha":
                case "f3b_esc45_Superior":

                    TagP0_A = AgregarTagPathreinLitsta("A", -83, -25, _p1);
                    TagP0_B = AgregarTagPathreinLitsta("B", -77, -45, _p1);
                    TagP0_C = AgregarTagPathreinLitsta("C", -35, -30, _p1);
                    TagP0_D = AgregarTagPathreinLitsta("D", 45, -9, _ptoMouse);

                    break;
                case "f3b_esc45_Izquierda":
                case "f3b_esc45_Inferior":

                    TagP0_D = AgregarTagPathreinLitsta("D", 35, -30, _p2);
                    TagP0_E = AgregarTagPathreinLitsta("E", 77, -45, _p2);
                    TagP0_G = AgregarTagPathreinLitsta("G", 83, -25, _p2);

                    break;
                case "f3b_esc135_Derecha":
                case "f3b_esc135_Superior":
                    TagP0_A = AgregarTagPathreinLitsta("A", -40, 60, _p1);
                    TagP0_B = AgregarTagPathreinLitsta("B", -68, 55, _p1);
                    TagP0_C = AgregarTagPathreinLitsta("C", -55, 15, _p1);
                    TagP0_D = AgregarTagPathreinLitsta("D", 45, -9, _ptoMouse);
                    break;
                case "f3b_esc135_Izquierda":
                case "f3b_esc135_Inferior":


                    TagP0_D = AgregarTagPathreinLitsta("D", 55, 15, _p2);
                    TagP0_E = AgregarTagPathreinLitsta("E", 70, 45, _p2);
                    TagP0_G = AgregarTagPathreinLitsta("G", 40, 60, _p2);

                    break;
                default:
                    break;
            }

            TagP0_F = AgregarTagRebarLista("F", -55, 7, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);
            //listaTag.RemoveAll(c => c.nombre == "F");
            //XYZ p0_F = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_ptoMouse, _anguloBarraRad, Util.CmToFoot(-55), Util.CmToFoot(7));
            //TagP0_F = M1_1_ObtenerTAgRebarBarra(p0_F, "F", "MRA Rebar_FLosaEsc_" + escala, escala);
            //listaTag.Add(TagP0_F);
        }



        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF3BEsc> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
            string tipo = $"{_solicitudBarraDTO.TipoBarra}_{_solicitudBarraDTO.UbicacionEnlosa}";
            switch (tipo)
            {


                case "f3b_esc45_Izquierda":
                case "f3b_esc45_Inferior":
                case "f3b_esc135_Izquierda":
                case "f3b_esc135_Inferior":
                    _geomeTagBase.TagP0_A.IsOk = false;
                    _geomeTagBase.TagP0_B.IsOk = false;
                    // _geomeTagBase.TagP0_E.IsOk = false;

                    _geomeTagBase.TagP0_G.CAmbiar(_geomeTagBase.TagP0_A);
                    _geomeTagBase.TagP0_E.CAmbiar(_geomeTagBase.TagP0_B);
                    var auxD = _geomeTagBase.TagP0_D.Copiar();
                    _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_C);
                    _geomeTagBase.TagP0_C.CAmbiar(auxD);
                    break;
                case "f3b_esc45_Derecha":
                case "f3b_esc45_Superior":
                case "f3b_esc135_Derecha":
                case "f3b_esc135_Superior":
                    //  _geomeTagBase.TagP0_A.IsOk = false;
                    _geomeTagBase.TagP0_E.IsOk = false;


                    break;

                default:
                    break;
            }

        }
    }
}
