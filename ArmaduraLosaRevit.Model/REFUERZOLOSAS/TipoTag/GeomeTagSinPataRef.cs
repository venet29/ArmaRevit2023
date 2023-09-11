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

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.TipoTag
{
    public class GeomeTagSinPataRef : GeomeTagBaseRef, IGeometriaTag
    {
        private string tipoPosicionEstribo;

        public GeomeTagSinPataRef(Document doc, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag) :
            base(doc, ptoIni, ptoFin, posiciontag)
        { }

        public GeomeTagSinPataRef() { }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {

                tipoPosicionEstribo = args.tipoPosicionEstribo;
                double AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
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

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagSinPataRef> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseRef _geomeTagBase)
        {
            if (tipoPosicionEstribo == "Superior")
            {
                listaTag.RemoveAll(c => c.nombre == "F");
                XYZ p0_F = CentroBarra - _direccionBarra * Util.CmToFoot(45);// new XYZ(0, 0, _largoMedioEnFoot * 0.25);
                TagP0_F = M2_1_ObtenerTAgBarra(p0_F, "F", nombreDefamiliaBase + "_FRef_" + _escala, _escala);
                listaTag.Add(TagP0_F);


                listaTag.RemoveAll(c => c.nombre == "L");
                XYZ p0_L = CentroBarra + _direccionBarra * Util.CmToFoot(30); // new XYZ(0, 0,- _largoMedioEnFoot * 0.25);
                TagP0_L = M2_1_ObtenerTAgBarra(p0_L, "L", nombreDefamiliaBase + "_LRef_" + _escala, _escala);
                listaTag.Add(TagP0_L);
            }
            else if (tipoPosicionEstribo == "Inferior")
            {
                listaTag.RemoveAll(c => c.nombre == "F");
                XYZ p0_F = CentroBarra - _direccionBarra * Util.CmToFoot(45);// new XYZ(0, 0, _largoMedioEnFoot * 0.25);
                TagP0_F = M2_1_ObtenerTAgBarra(p0_F, "F", nombreDefamiliaBase + "_FRef_" + _escala, _escala);
                listaTag.Add(TagP0_F);


                listaTag.RemoveAll(c => c.nombre == "L");
                XYZ p0_L = CentroBarra + _direccionBarra * Util.CmToFoot(30); // new XYZ(0, 0,- _largoMedioEnFoot * 0.25);
                TagP0_L = M2_1_ObtenerTAgBarra(p0_L, "L", nombreDefamiliaBase + "_LRef_" + _escala, _escala);
                listaTag.Add(TagP0_L);
            }


            _geomeTagBase.TagP0_C.IsOk = false;
            _geomeTagBase.TagP0_Estri.IsOk = false;

        }
    }
}
