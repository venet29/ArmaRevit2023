using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmaduraLosaRevit.Model.Cubicacion.Seleccionar;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
    public class AyudaObtenerPesoBArras
    {

        public static bool Ejecutar(UIApplication _uiapp, View view3D_Visualizar)
        {
            try
            {
                var _doc = _uiapp.ActiveUIDocument.Document;


                var SeleccionarRebar_PathReinforment = new SeleccionarRebar_PathReinforment(_uiapp, view3D_Visualizar);

                if (!SeleccionarRebar_PathReinforment.M0_CArgar_PAthReinforment()) return false;
                if (!SeleccionarRebar_PathReinforment.M0_Cargar_rebar()) return false;

                if (!SeleccionarRebar_PathReinforment.M1_Ejecutar_rebar())
                {
                    Util.ErrorMsg("Error al obtener Lista Rebar");
                    return false;
                }
                if (!SeleccionarRebar_PathReinforment.M1_Ejecutar_PAthReinforment())
                {
                    Util.ErrorMsg("Error al obtener Lista PAthReinforment");
                    return false;
                }

                if (!SeleccionarRebar_PathReinforment.M0_Cargar_rebar()) return false;

                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();

                    for (int i = 0; i < SeleccionarRebar_PathReinforment._lista_A_TodasRebarNivelActual_MENOSRebarSystem.Count; i++)
                    {
                        var rebar_ = SeleccionarRebar_PathReinforment._lista_A_ElementoREbarDTO[i];

                        double pesoBArra = rebar_._rebar.ObtenerPeso();

                        if (Util.IsSimilarValor(pesoBArra, 0, 0.01)) continue;

                        ParameterUtil.SetParaDoubleNH(rebar_._rebar, "PesoBarra", pesoBArra);
                    }


                    for (int i = 0; i < SeleccionarRebar_PathReinforment._lista_A_ElementoPathReinformentDTO.Count; i++)
                    {
                        var rPAth_ = SeleccionarRebar_PathReinforment._lista_A_ElementoPathReinformentDTO[i];

                        for (int j = 0; j < rPAth_._lista_A_DeRebarInSystem.Count; j++)
                        {

                            var rebInsys = (RebarInSystem)rPAth_._lista_A_DeRebarInSystem[j];
                            double pesoBArra = ((RebarInSystem)rPAth_._lista_A_DeRebarInSystem[j]).ObtenerPeso();

                            if (Util.IsSimilarValor(pesoBArra, 0, 0.01)) continue;

                            ParameterUtil.SetParaDoubleNH(rebInsys, "PesoBarra", pesoBArra);
                        }

                    }

                    tr.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
