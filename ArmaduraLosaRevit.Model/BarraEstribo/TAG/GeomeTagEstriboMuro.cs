using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.TAG.Helper;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG
{
    public class GeomeTagEstriboMuro : GeomeTagEstriboBase, IGeometriaTag
    {
        private EstriboMuroDTO _EstriboMuroDTO;
        private readonly TipoEstriboConfig _tipoEstriboConfig;
        XYZ p0_Estribo;

        public GeomeTagEstriboMuro(Document doc, EstriboMuroDTO item, TipoEstriboConfig tipoEstriboConfig) :
        base(doc, item.Posi1TAg, item.NombreFamilia)
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
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar 'GeomeTagEstriboMuro ejecutar'  ex:${ex.Message}");
                return false;
            }
            return true;

        }
        public void M2_RECAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {
            p0_Estribo = null;

            TagConfigEscala _TagConfigEscala = new TagConfigEscala(_EstriboMuroDTO, _tipoEstriboConfig, CentroBarra);

            if (escala_realview == 50)
                _TagConfigEscala.M2_Configuracion_escala50();
            else if (escala_realview == 75)
                _TagConfigEscala.M2_Configuracion_escala75();
            else if (escala_realview == 100)
                _TagConfigEscala.M2_Configuracion_escala100();

            p0_Estribo = _TagConfigEscala.p0_Estribo;

            AgregaroEditaPosicionTAgLitsta("ESTRIBO", p0_Estribo);
            //TagP0_Lateral = M1_1_ObtenerTAgBarra(p0_Lateral, "LATERAL", nombreDefamiliaBase, escala);
            //listaTag.Add(TagP0_Lateral);
        }



        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagEstriboMuro> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagEstriboBase _geomeTagBase)
        {

            _geomeTagBase.TagP0_Lateral.IsOk = false;
            _geomeTagBase.TagP0_Traba.IsOk = false;

        }


    }
}
