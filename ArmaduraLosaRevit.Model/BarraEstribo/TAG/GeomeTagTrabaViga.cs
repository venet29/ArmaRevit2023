using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG
{
    public class GeomeTagTrabaViga : GeomeTagEstriboBase, IGeometriaTag
    {
        private readonly EstriboMuroDTO _EstriboMuroDTO;
        private readonly TipoEstriboConfig _tipoEstriboConfig;

        public GeomeTagTrabaViga(Document doc, EstriboMuroDTO item, TipoEstriboConfig tipoEstriboConfig) :
            base(doc, item.Posi1TAg, item.NombreFamilia + "T")
        {
            this._EstriboMuroDTO = item;
            this._tipoEstriboConfig = tipoEstriboConfig;
        }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                double AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M2_RECAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        public void M2_RECAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {

            XYZ p0_Trabas = null;

            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    return;
                case TipoEstriboConfig.ET:
                    p0_Trabas = CentroBarra - new XYZ(0, 0, Util.CmToFoot(5));
                    break;
                case TipoEstriboConfig.ELT:
                    return;
                case TipoEstriboConfig.L:
                    return;
                case TipoEstriboConfig.LT:
                    p0_Trabas = CentroBarra - new XYZ(0, 0, Util.CmToFoot(5));
                    break;
                case TipoEstriboConfig.T:
                    p0_Trabas = CentroBarra + new XYZ(0, 0, Util.CmToFoot(10));
                    break;

            }

            AgregaroEditaPosicionTAgLitsta("TRABA", p0_Trabas);

        }
        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagTrabaViga> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagEstriboBase _geomeTagBase)
        {
            _geomeTagBase.TagP0_Estribo.IsOk = false;
            _geomeTagBase.TagP0_Lateral.IsOk = false;
            _geomeTagBase.TagP0_Espesor.IsOk = false;
        }


    }
}
