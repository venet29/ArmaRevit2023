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
    public class GeomeTagS4_incli : GeomeTagBaseRebar, IGeometriaTag
    {
        public GeomeTagS4_incli(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagS4_incli(Document doc, RebarInferiorDTO rebarInferiorDTO1) :
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
                
                case "s4_Inclinada_Derecha":
                    TagP0_A = AgregarTagRebarLista("A", -20, -20, _p2, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", 12, -5, _p2, $"MRA Rebar_B_" + escala);

                    TagP0_L = AgregarTagRebarLista("L", -55, 15, _ptoMouse, $"MRA Rebar_L_" + escala);
                    TagP0_C = AgregarTagRebarLista("CLosa",- 55, -15, _ptoMouse, $"MRA Rebar_CLosa_" + escala);
                    TagP0_F = AgregarTagRebarLista("F", -55, 7, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);
                    break;

                case "s4_Inclinada_Superior":
                    TagP0_A = AgregarTagRebarLista("A", -20, -20, _p2, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", 12, -5, _p2, $"MRA Rebar_B_" + escala);
                    TagP0_L = AgregarTagRebarLista("L", -55, 15, _ptoMouse, $"MRA Rebar_L_" + escala);
                    TagP0_C = AgregarTagRebarLista("CLosa", -55, -15, _ptoMouse, $"MRA Rebar_CLosa_" + escala);
                    TagP0_F = AgregarTagRebarLista("F", -55, 7, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);


                    break;
                case "s4_Inclinada_Izquierda":

                    TagP0_A = AgregarTagRebarLista("A", 15, -20, _p1, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", -12, -10, _p1, $"MRA Rebar_B_" + escala);
                    TagP0_L = AgregarTagRebarLista("L", 50, 15, _ptoMouse, $"MRA Rebar_L_" + escala);
                    TagP0_C = AgregarTagRebarLista("CLosa", 50, -15, _ptoMouse, $"MRA Rebar_CLosa_" + escala);
                    TagP0_F = AgregarTagRebarLista("F", 50, 7, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);
                    break;
                case "s4_Inclinada_Inferior":


                    TagP0_A = AgregarTagRebarLista("A", 15, -20, _p1, $"MRA Rebar_A_" + escala);
                    TagP0_B = AgregarTagRebarLista("B", -12, -10, _p1, $"MRA Rebar_B_" + escala);
                    TagP0_L = AgregarTagRebarLista("L", 50, 15, _ptoMouse, $"MRA Rebar_L_" + escala);
                    TagP0_C = AgregarTagRebarLista("CLosa", 50, -15, _ptoMouse, $"MRA Rebar_CLosa_" + escala);
                    TagP0_F = AgregarTagRebarLista("F", 50, 7, _ptoMouse, "MRA Rebar_FLosaEsc_" + escala);

                    break;
                default:
                    break;
            }

           


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
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagS4_incli> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {
          //  _geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            _geomeTagBase.TagP0_D.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;
           // _geomeTagBase.TagP0_C.IsOk = false;

        }
    }
}
