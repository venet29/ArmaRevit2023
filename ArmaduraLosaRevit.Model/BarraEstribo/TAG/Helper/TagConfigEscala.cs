using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG.Helper
{
   public class TagConfigEscala
    {


   
        public XYZ p0_Estribo { get; set; }
        private EstriboMuroDTO _EstriboMuroDTO;
        private TipoEstriboConfig _tipoEstriboConfig;
        private readonly XYZ CentroBarra;

        public TagConfigEscala(EstriboMuroDTO item, TipoEstriboConfig tipoEstriboConfig, XYZ CentroBarra)
        {
            this._EstriboMuroDTO = item;
            this._tipoEstriboConfig = tipoEstriboConfig;
            this.CentroBarra = CentroBarra;
        }

        public void M2_Configuracion_escala50()
        {
            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra;
                    else
                        p0_Estribo = CentroBarra;
                    break;
                case TipoEstriboConfig.E_TF:
                case TipoEstriboConfig.EL:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra;//; + _EstriboMuroDTO.direccionTag * Util.CmToFoot(2); 
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12);
                    break;
                case TipoEstriboConfig.ET:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra;
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12);
                    break;
                case TipoEstriboConfig.EL_TF:
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra;
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(24);

                    break;
                case TipoEstriboConfig.L:
                    // p0_Lateral = CentroBarra;
                    return;
                case TipoEstriboConfig.LT:
                    //p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(5);
                    return;
                case TipoEstriboConfig.T:
                    return;

            }
        }

        public void M2_Configuracion_escala75()
        {
            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(2);
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(2);
                    break;
                case TipoEstriboConfig.E_TF:
                case TipoEstriboConfig.EL:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(2);
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12 + 7);
                    break;
                case TipoEstriboConfig.ET:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(2);
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12 + 7);
                    break;
                case TipoEstriboConfig.EL_TF:
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(2);
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(24 + 13);
                    break;
                case TipoEstriboConfig.L:
                    // p0_Lateral = CentroBarra;
                    return;
                case TipoEstriboConfig.LT:
                    //p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(5);
                    return;
                case TipoEstriboConfig.T:
                    return;

            }
        }

        public void M2_Configuracion_escala100()
        {
            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(4);
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(4);
                    break;
                case TipoEstriboConfig.E_TF:
                case TipoEstriboConfig.EL:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(4);
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(16 + 10);
                    break;
                case TipoEstriboConfig.ET:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(4);
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(16 + 10);

                    break;
                case TipoEstriboConfig.EL_TF:
                case TipoEstriboConfig.ELT:
                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionTag * Util.CmToFoot(4);
                    else
                        p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(34 + 14);

                    break;
                case TipoEstriboConfig.L:
                    // p0_Lateral = CentroBarra;
                    return;
                case TipoEstriboConfig.LT:
                    //p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(5);
                    return;
                case TipoEstriboConfig.T:
                    return;

            }
        }

    }
}
