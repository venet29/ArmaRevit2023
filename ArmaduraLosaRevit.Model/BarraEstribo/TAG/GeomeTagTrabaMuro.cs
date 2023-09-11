using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG
{
    public class GeomeTagTrabaMuro : GeomeTagEstriboBase, IGeometriaTag
    {
        private EstriboMuroDTO _EstriboMuroDTO;
        private TipoEstriboConfig _tipoEstriboConfig;
        XYZ p0_Traba;
        public GeomeTagTrabaMuro(Document doc, EstriboMuroDTO item, TipoEstriboConfig tipoEstriboConfig) :
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
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagTrabaMuro  ex:${ex.Message}");
                return false;
            }
            return true;
            
        }
        public void M2_RECAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {
            p0_Traba = null;
            XYZ desfase_haciaAbajo = new XYZ(0, 0, Util.CmToFoot(5));
            // p0_Traba = CentroBarra;

            if (escala_realview == 50)
                M2_Configuracion_escala50();
            else if (escala_realview == 75)
                M2_Configuracion_escala75();
            else if (escala_realview == 100)
                M2_Configuracion_escala100();


            p0_Traba = p0_Traba - desfase_haciaAbajo;

            if (p0_Traba != null)
                AgregaroEditaPosicionTAgLitsta("TRABA", p0_Traba);
            //TagP0_Lateral = M1_1_ObtenerTAgBarra(p0_Lateral, "LATERAL", nombreDefamiliaBase, escala);
            //listaTag.Add(TagP0_Lateral);
        }

        private void M2_Configuracion_escala50()
        {
            p0_Traba = CentroBarra;
            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    return;
                case TipoEstriboConfig.ET:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12);
                    break;
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(24);
                    break;
                case TipoEstriboConfig.L:
                    return;
                case TipoEstriboConfig.LT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12);

                    break;
                case TipoEstriboConfig.T:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra;
                    break;

            }
        }

        private void M2_Configuracion_escala75()
        {
            p0_Traba = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(2);
            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    return;
                case TipoEstriboConfig.ET:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12 + 7);
                    break;
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(24 + 12);
                    break;
                case TipoEstriboConfig.L:
                    return;
                case TipoEstriboConfig.LT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12 + 7);

                    break;
                case TipoEstriboConfig.T:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(2);
                    break;

            }
        }

        private void M2_Configuracion_escala100()
        {
            p0_Traba = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(4);
            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    return;
                case TipoEstriboConfig.ET:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12 + 14);
                    break;
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(24 + 24);
                    break;
                case TipoEstriboConfig.L:
                    return;
                case TipoEstriboConfig.LT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12 + 14);

                    break;
                case TipoEstriboConfig.T:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Traba = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(4);
                    break;

            }
        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagTrabaMuro> rutina)
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
