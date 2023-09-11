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
    public class GeomeTagEstriboRef : GeomeTagBaseRef, IGeometriaTag
    {


        public GeomeTagEstriboRef(Document doc, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag) :
            base(doc, ptoIni, ptoFin, posiciontag)
        { }

        public GeomeTagEstriboRef() { }

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
                Util.ErrorMsg($"Error ejecutar TagEstriboRef  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagEstriboRef> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseRef _geomeTagBase)
        {
            _geomeTagBase.TagP0_F.IsOk = false;
            _geomeTagBase.TagP0_C.IsOk = false;
            _geomeTagBase.TagP0_L.IsOk = false;

        }
    }
}
