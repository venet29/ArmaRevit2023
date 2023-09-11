using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoTagH
{
    public class GeomeTagPataAmbosV_Horquilla : GeomeTagBaseV, IGeometriaTag
    {


        public GeomeTagPataAmbosV_Horquilla(Document doc, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag) :
            base(doc, ptoIni, ptoFin, posiciontag)
        { }

        public GeomeTagPataAmbosV_Horquilla() { }

#pragma warning disable CS0108 // 'GeomeTagPataAmbosV_Horquilla.Ejecutar(GeomeTagArgs)' hides inherited member 'GeomeTagBaseV.Ejecutar(GeomeTagArgs)'. Use the new keyword if hiding was intended.
        public bool Ejecutar(GeomeTagArgs args)
#pragma warning restore CS0108 // 'GeomeTagPataAmbosV_Horquilla.Ejecutar(GeomeTagArgs)' hides inherited member 'GeomeTagBaseV.Ejecutar(GeomeTagArgs)'. Use the new keyword if hiding was intended.
        {
            try
            {

                double AnguloRadian = args.angulorad;
                M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian);
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }


        public override void M3_DefinirRebarShape()
        {
            double desfaseCodo = 0;
            if (_view.Scale == 75)
                desfaseCodo = 1;
            if (_view.Scale == 100)
                desfaseCodo = 2;

            listaTag.RemoveAll(c => c.nombre == "F");
            XYZ p0_F = CentroBarra+ XYZ.BasisZ* desfaseCodo;
            TagP0_F = M1_1_ObtenerTAgBarra(p0_F, "F", nombreDefamiliaBase + "_F_" + escala, escala);
            listaTag.Add(TagP0_F);

            AsignarPArametros(this);
        }
        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagPataAmbosV_Horquilla> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseV _geomeTagBase)
        {

            //_geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            //_geomeTagBase.TagP0_D.IsOk = false;
            //_geomeTagBase.TagP0_E.IsOk = false;
            _geomeTagBase.TagP0_F_SIN.IsOk = false;

            _geomeTagBase.TagP0_C.IsOk = false;
            _geomeTagBase.TagP0_L.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}
