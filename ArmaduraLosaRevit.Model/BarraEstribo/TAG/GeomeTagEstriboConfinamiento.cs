using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.TAG.Helper;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TAG
{
    public class GeomeTagEstriboConfinamiento : GeomeTagEstriboBase, IGeometriaTag
    {
        private readonly EstriboMuroDTO _EstriboMuroDTO;
        private readonly TipoEstriboConfig _tipoEstriboConfig;
        XYZ p0_Estribo;
        public GeomeTagEstriboConfinamiento(Document doc, EstriboMuroDTO item, TipoEstriboConfig tipoEstriboConfig) :
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
                Util.ErrorMsg($"Error ejecutar TagEstriboConfinamiento ex:${ex.Message}");
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

            if (p0_Estribo != null)
                AgregaroEditaPosicionTAgLitsta("ESTRIBO", p0_Estribo);
        }

        public void M2_RECAlcularPtosDeTAgv2(bool IsGraficarEnForm = false)
        {
            XYZ p0_Estribo = null;
            //XYZ p0_lateral = null;
            //XYZ p0_traba = null;

            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    //no se mueve
                    //p0_Estribo = CentroBarra;
                    break;
                case TipoEstriboConfig.EL:
                    //no aplica
                    //if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                    //    p0_Estribo = CentroBarra;
                    //else
                    //    p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12);
                    break;

                case TipoEstriboConfig.ET:

                    if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                    { }
                    else
                    { p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(12); }

                    break;
                case TipoEstriboConfig.ELT:
                    //no aplica

                    //if (_EstriboMuroDTO.DireccionSeleccionConMouse == DireccionSeleccionMouse.DereToIzq)
                    //    p0_Estribo = CentroBarra;
                    //else
                    //    p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(24);


                    break;
                case TipoEstriboConfig.L:
                    //no aplica
                    // p0_Lateral = CentroBarra;
                    return;
                case TipoEstriboConfig.LT:
                    //no aplica
                    //p0_Estribo = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(5);
                    return;
                case TipoEstriboConfig.T:
                    return;

            }

            if (p0_Estribo != null)
                AgregaroEditaPosicionTAgLitsta("ESTRIBO", p0_Estribo);

            ////if (p0_traba != null)
            ////    AgregaroEditaPosicionTAgLitsta("TRABA", p0_traba);

            ////if (p0_lateral!=null)
            ////    AgregaroEditaPosicionTAgLitsta("LATERAL", p0_lateral);

            //TagP0_Lateral = M1_1_ObtenerTAgBarra(p0_Lateral, "LATERAL", nombreDefamiliaBase, escala);
            //listaTag.Add(TagP0_Lateral);
        }



        public void M2_RECAlcularPtosDeTAgLateral(bool IsGraficarEnForm = false)
        {
            XYZ p0_Lateral = null;

            switch (_tipoEstriboConfig)
            {
                case TipoEstriboConfig.E:
                    return;
                case TipoEstriboConfig.EL:
                    p0_Lateral = CentroBarra;
                    break;
                case TipoEstriboConfig.ET:
                    return;
                case TipoEstriboConfig.ELT:
                    return;
                case TipoEstriboConfig.L:
                    p0_Lateral = CentroBarra;
                    break;
                case TipoEstriboConfig.LT:
                    p0_Lateral = CentroBarra + _EstriboMuroDTO.direccionBarra * Util.CmToFoot(5);
                    break;
                case TipoEstriboConfig.T:
                    return;

            }

            AgregaroEditaPosicionTAgLitsta("LATERAL", p0_Lateral);
            //TagP0_Lateral = M1_1_ObtenerTAgBarra(p0_Lateral, "LATERAL", nombreDefamiliaBase, escala);
            //listaTag.Add(TagP0_Lateral);
        }


        public void M3_DefinirRebarShape() => AsignarPArametros(this);

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagEstriboConfinamiento> rutina)
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
