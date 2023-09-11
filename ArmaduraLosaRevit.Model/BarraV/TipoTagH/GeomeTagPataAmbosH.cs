using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoTagH
{
    public class GeomeTagPataAmbosH : GeomeTagBaseH, IGeometriaTag
    {


        public GeomeTagPataAmbosH(UIApplication uiapp, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag) :
            base(uiapp, ptoIni, ptoFin, posiciontag)
        { }

        public GeomeTagPataAmbosH() { }

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
                Util.ErrorMsg($"Error ejecutar TagPataAmbosH   ex:${ex.Message}");
                return false;
            }
            return true;

        }


#pragma warning disable CS0108 // 'GeomeTagPataAmbosH.M2_CAlcularPtosDeTAg(bool)' hides inherited member 'GeomeTagBaseH.M2_CAlcularPtosDeTAg(bool)'. Use the new keyword if hiding was intended.
        public void M2_CAlcularPtosDeTAg(bool IsGraficarEnForm = false)
#pragma warning restore CS0108 // 'GeomeTagPataAmbosH.M2_CAlcularPtosDeTAg(bool)' hides inherited member 'GeomeTagBaseH.M2_CAlcularPtosDeTAg(bool)'. Use the new keyword if hiding was intended.
        {

            XYZ p0_F = _p1 + _direccionBarra * _largoMedioEnFoot * 0.25 ;// new XYZ(0, 0, _largoMedioEnFoot * 0.25);
            TagP0_F = M2_1_ObtenerTAgBarra(p0_F, "F", nombreDefamiliaBase + "_FH_" + escala, escala);
            listaTag.Add(TagP0_F);


            XYZ p0_C = CentroBarra;
            TagP0_C = M2_1_ObtenerTAgBarra(p0_C, "C", nombreDefamiliaBase + "_C_" + escala, escala);
            listaTag.Add(TagP0_C);


            XYZ p0_L = _p2 - _direccionBarra * _largoMedioEnFoot * 0.25; // new XYZ(0, 0,- _largoMedioEnFoot * 0.25);
            TagP0_L = M2_1_ObtenerTAgBarra(p0_L, "L", nombreDefamiliaBase + "_L_" + escala, escala);
            listaTag.Add(TagP0_L);


        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagPataAmbosH> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseH _geomeTagBase)
        {

            //_geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            //_geomeTagBase.TagP0_D.IsOk = false;
            //_geomeTagBase.TagP0_E.IsOk = false;
            //_geomeTagBase.TagP0_C.IsOk = false;
            //_geomeTagBase.TagP0_L.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}
