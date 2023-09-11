using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG
{
    public class GeomeTagEstriboViga : GeomeTagEstriboBase, IGeometriaTag
    {
        private readonly TipoEstriboConfig tipoEstriboConfig;

        public GeomeTagEstriboViga(Document doc, XYZ posiciontag, string nombreDefamiliaBase, TipoEstriboConfig tipoEstriboConfig) :
            base(doc, posiciontag, nombreDefamiliaBase)
        {
            this.tipoEstriboConfig = tipoEstriboConfig;
        }

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
                Util.ErrorMsg($"Error ejecutar TagEstrivoViga  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagEstriboViga> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagEstriboBase _geomeTagBase)
        {

            _geomeTagBase.TagP0_Lateral.IsOk = false;
            _geomeTagBase.TagP0_Traba.IsOk = false;
            _geomeTagBase.TagP0_Espesor.IsOk = false;

        }


    }
}
