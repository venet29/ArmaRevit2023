using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG
{
    public class GeomeTagLateralesMuro : GeomeTagEstriboBase, IGeometriaTag
    {
        private readonly TipoEstriboConfig _tipoEstriboConfig;
        private EstriboMuroDTO _EstriboMuroDTO;
        private XYZ p0_Lateral;

        //public GeomeTagLateralesMuro(Document doc,XYZ posiciontag, string nombreDefamiliaBase, TipoEstriboConfig tipoEstriboConfig) :
        //    base(doc,  posiciontag, nombreDefamiliaBase)
        //{
        //    this._tipoEstriboConfig = tipoEstriboConfig;
        //}
        public GeomeTagLateralesMuro(Document doc, EstriboMuroDTO item, TipoEstriboConfig tipoEstriboConfig) : base(doc, item.Posi1TAg, item.NombreFamilia + "L")
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
                Util.ErrorMsg($"Error ejecutar TagLateralesMuro  ex:${ex.Message}");
                return false;
            }
            return true;

        }




        public void M2_RECAlcularPtosDeTAg(bool IsGraficarEnForm = false)
        {
            p0_Lateral = null;

            if (escala_realview == 50)
                M2_Configuracion_escala50();
            else if (escala_realview == 75)
                M2_Configuracion_escala75();
            else if (escala_realview == 100)
                M2_Configuracion_escala100();



            AgregaroEditaPosicionTAgLitsta("LATERAL", p0_Lateral);
            //TagP0_Lateral = M1_1_ObtenerTAgBarra(p0_Lateral, "LATERAL", nombreDefamiliaBase, escala);
            //listaTag.Add(TagP0_Lateral);
        }


        public void M2_Configuracion_escala50()
        {

            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12);
                    else
                        p0_Lateral = CentroBarra;
                    break;
                case TipoEstriboConfig.ET:
                    return;
                case TipoEstriboConfig.EL_TF:
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12);
                    break;
                case TipoEstriboConfig.L:
                    p0_Lateral = CentroBarra;
                    break;
                case TipoEstriboConfig.LT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra;
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12);
                    break;

                case TipoEstriboConfig.T:
                    return;

            }
        }

        public void M2_Configuracion_escala75()
        {

            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12 + 7);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(2);
                    break;
                case TipoEstriboConfig.ET:
                    return;
                case TipoEstriboConfig.EL_TF:
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12 + 7);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12 + 7);
                    break;
                case TipoEstriboConfig.L:

                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(2);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(2);
                    break;
                   
                case TipoEstriboConfig.LT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(2);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12 + 7);
                    break;

                case TipoEstriboConfig.T:
                    return;

            }
        }

        public void M2_Configuracion_escala100()
        {

            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12 + 14);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(4);
                    break;
                case TipoEstriboConfig.ET:
                    return;
                case TipoEstriboConfig.EL_TF:
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(12 + 14);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(16 + 10);
                    break;
                case TipoEstriboConfig.L:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(4);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(4);
                    break;
                case TipoEstriboConfig.LT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(4);
                    else
                        p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(16 + 10);
                    break;

                case TipoEstriboConfig.T:
                    return;

            }
        }

        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagLateralesMuro> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagEstriboBase _geomeTagBase)
        {
            _geomeTagBase.TagP0_Estribo.IsOk = false;
            _geomeTagBase.TagP0_Traba.IsOk = false;
            _geomeTagBase.TagP0_Espesor.IsOk = false;
        }


    }
}
