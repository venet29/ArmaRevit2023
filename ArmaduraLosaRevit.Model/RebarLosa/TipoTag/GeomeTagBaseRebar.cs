using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.TipoTag
{
   public class GeomeTagBaseRebar: GeomeTagBase
    {


        public GeomeTagBaseRebar(Document doc, RebarInferiorDTO rebarInferiorDTO1):base(doc,rebarInferiorDTO1)
        {

        }

        public GeomeTagBaseRebar(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO solicitudBarraDTO)
            :base(doc,  ptoMOuse, listaPtosPerimetroBarras,  solicitudBarraDTO)
        {
        }

        public void M2_CAlcularPtosDeTAgRebar(bool IsGraficarEnForm = false)
        {

            if (_view.Scale == 50)
                M2_CAlcularPtosDeTAgRebar_escala50();
            else if (_view.Scale == 75)
                M2_CAlcularPtosDeTAgRebar_escala75();
            else if (_view.Scale == 100)
                M2_CAlcularPtosDeTAgRebar_escala100();

        }

        private void M2_CAlcularPtosDeTAgRebar_escala50()
        {

            TagP0_A = AgregarTagRebarLista("A", 55, 24, _p1, $"MRA Rebar_A_" + escala);


            TagP0_B = AgregarTagRebarLista("B", -12, 7, _p1, $"MRA Rebar_B_" + escala);

       
            TagP0_C = AgregarTagRebarLista("CLosa", -45, -9, _ptoMouse, $"MRA Rebar_CLosa_" + escala);

            TagP0_F = AgregarTagRebarLista("F", -55, 7, _ptoMouse, $"MRA Rebar_F_" + escala);


            TagP0_L = AgregarTagRebarLista("L", 50, 0, _ptoMouse, $"MRA Rebar_L_" + escala);
           

         

            TagP0_D = AgregarTagRebarLista("D", 13, 7, _p2, $"MRA Rebar_D_" + escala);


            TagP0_E = AgregarTagRebarLista("E", -45, 24, _p2, $"MRA Rebar_E_" + escala);
        }

        private void M2_CAlcularPtosDeTAgRebar_escala75()
        {
          
            TagP0_A = AgregarTagRebarLista("A", 55, 38, _p1, $"MRA Rebar_A_" + escala);

            TagP0_B = AgregarTagRebarLista("B", -12, 7, _p1, $"MRA Rebar_B_" + escala);

            TagP0_C = AgregarTagRebarLista("CLosa", -45, -15, _p1, $"MRA Rebar_CLosa_" + escala);

            TagP0_F = AgregarTagRebarLista("F", -80, 12, _ptoMouse, $"MRA Rebar_F_" + escala);

            TagP0_L = AgregarTagRebarLista("L", 50, 0, _ptoMouse, $"MRA Rebar_L_" + escala);

            TagP0_D = AgregarTagRebarLista("D", 13, 7, _p2, $"MRA Rebar_D_" + escala);

            TagP0_E = AgregarTagRebarLista("E", -45, 38, _p2, $"MRA Rebar_E_" + escala);

        }

        private void M2_CAlcularPtosDeTAgRebar_escala100()
        {
           
            TagP0_A = AgregarTagRebarLista("A", 55, 50, _p1, $"MRA Rebar_A_" + escala);

            TagP0_B = AgregarTagRebarLista("B", -15, 12, _p1, $"MRA Rebar_B_" + escala);

            TagP0_C = AgregarTagRebarLista("CLosa", -45, -18, _p1, $"MRA Rebar_CLosa_" + escala);

            TagP0_F = AgregarTagRebarLista("F", -100, 15, _ptoMouse, $"MRA Rebar_F_" + escala);

            TagP0_L = AgregarTagRebarLista("L", 70, 15, _ptoMouse, $"MRA Rebar_L_" + escala);

            TagP0_D = AgregarTagRebarLista("D", 15, 12, _p2, $"MRA Rebar_D_" + escala);

            TagP0_E = AgregarTagRebarLista("E", -45, 50, _p2, $"MRA Rebar_E_" + escala);



        }



    }
}
