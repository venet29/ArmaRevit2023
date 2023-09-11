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
    // CON PATA 'C' HACIA ARRIBA PARA ESCALERAS
    public class GeomeTagF1Esc : GeomeTagBaseRebar, IGeometriaTag
    {
        public GeomeTagF1Esc(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagF1Esc(Document doc, RebarInferiorDTO rebarInferiorDTO1) :
                 base(doc, rebarInferiorDTO1) { }

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
                Util.ErrorMsg($"Error ejecutar TagF1Esc  ex:${ex.Message}");
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
                case "f1_esc45_conpata_Derecha":
                case "f1_esc45_conpata_Superior":

                    TagP0_A = AgregarTagRebarLista("A", -10, -20, _p2, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", 12, -5, _p2, $"MRA Rebar_B_" + escala);
 

                    TagP0_D = AgregarTagRebarLista("D", -60, -20, _p1, $"MRA Rebar_D_" + escala);
                    TagP0_E = AgregarTagRebarLista("E", -100, -60, _p1, $"MRA Rebar_E_" + escala);
                    TagP0_G = AgregarTagRebarLista("G", -70, -65, _p1, $"MRA Rebar_G_" + escala);

                    break;
                case "f1_esc45_conpata_Izquierda":
                case "f1_esc45_conpata_Inferior":


                    TagP0_A = AgregarTagRebarLista("A", 15, -20, _p1, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", -12, -10, _p1, $"MRA Rebar_B_" + escala);

                    TagP0_D = AgregarTagRebarLista("D", 55, -20, _p2, $"MRA Rebar_D_" + escala);
                    TagP0_E = AgregarTagRebarLista("E", 100, -60, _p2, $"MRA Rebar_E_" + escala);
                    TagP0_G = AgregarTagRebarLista("G", 70, -65, _p2, $"MRA Rebar_G_" + escala);

                    break;
                case "f1_esc135_sinpata_Derecha":
                case "f1_esc135_sinpata_Superior":
                    TagP0_A = AgregarTagRebarLista("A", -20, -20, _p2, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", 12, -5, _p2, $"MRA Rebar_B_" + escala);
    

                    TagP0_D = AgregarTagRebarLista("D", -40, 10, _p1, $"MRA Rebar_D_" + escala);

                    break;
                case "f1_esc135_sinpata_Izquierda":
                case "f1_esc135_sinpata_Inferior":


                    TagP0_A = AgregarTagRebarLista("A", 15, -20, _p1, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", -12, -10, _p1, $"MRA Rebar_B_" + escala); 

                    TagP0_D = AgregarTagRebarLista("D", 60, 20, _p2, $"MRA Rebar_D_" + escala);


                    break;
                default:
                    break;
            }

            TagP0_F = AgregarTagRebarLista("F", -65, 7, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);


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
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF1Esc> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
            string tipo = $"{_solicitudBarraDTO.TipoBarra}_{_solicitudBarraDTO.UbicacionEnlosa}";
            switch (tipo)
            {
                case "f1_esc135_sinpata_Izquierda":
                case "f1_esc135_sinpata_Inferior":
                case "f1_esc135_sinpata_Superior":
                case "f1_esc135_sinpata_Derecha":
                    //_geomeTagBase.TagP0_A.IsOk = false;
                    _geomeTagBase.TagP0_E.IsOk = false;
                    //if (_geomeTagBase.TagP0_G _geomeTagBase.TagP0_G.IsOk = false;
                    break;
                case "f1_esc45_conpata_Izquierda":
                case "f1_esc45_conpata_Inferior":
                case "f1_esc45_conpata_Superior":
                case "f1_esc45_conpata_Derecha":
                    //  _geomeTagBase.TagP0_A.IsOk = false;
                    // _geomeTagBase.TagP0_E.IsOk = false;
                    break;
                default:
                    break;
            }

        }
    }
}
