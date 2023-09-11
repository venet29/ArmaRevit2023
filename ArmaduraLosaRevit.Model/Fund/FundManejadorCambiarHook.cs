using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Extension.modelo;
using ArmaduraLosaRevit.Model.PathReinf.Servicios;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using MySqlX.XDevAPI.Common;

namespace ArmaduraLosaRevit.Model.Fund
{
    public class FundManejadorCambiarHook
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public FundManejadorCambiarHook(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }


        public bool EjecutarActualizar()
        {
            bool result = true;
            try
            {
                //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);                
                UpdateGeneral.M3_DesCargarBarras(_uiapp);
                SeleccionarPathReinforment_InfoCompleta _SeleccionarPathReinforment_InfoCompleta = new SeleccionarPathReinforment_InfoCompleta(_uiapp);
                if (_SeleccionarPathReinforment_InfoCompleta.GenerarListaSeleccionado())
                {

                    using (Transaction tr = new Transaction(_doc, "ActualizarPataFundHook"))
                    {
                        tr.Start();
                        for (int i = 0; i < _SeleccionarPathReinforment_InfoCompleta.ListaElementoPathRein.Count; i++)
                        {
                            var path = _SeleccionarPathReinforment_InfoCompleta.ListaElementoPathRein[i];
                            if (!EjecutarActualizarHook(path._pathReinforcement)) return false;
                        }
                        tr.Commit();
                    }


                    using (Transaction tr = new Transaction(_doc, "ActualizarPataFundLargos"))
                    {
                        tr.Start();
                        for (int i = 0; i < _SeleccionarPathReinforment_InfoCompleta.ListaElementoPathRein.Count; i++)
                        {
                            var path = _SeleccionarPathReinforment_InfoCompleta.ListaElementoPathRein[i];
                            if (!EjecutarActualizarTAg(path._pathReinforcement)) return false;
                        }
                        tr.Commit();
                    }
               
                
                }
                UpdateGeneral.M2_CargarBArras(_uiapp);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al actualizar Hook \n ex:{ex.Message}");
                result= false;
            }
            return result;
        }


        public bool EjecutarActualizarHook(PathReinforcement pathFund)
        {
            bool result = true;
            try
            {

                HookPAthRein _hoookPAthRein = pathFund.ObtenerHooksOP2();
                int _diamtro = pathFund.ObtenerDiametro_mm();
                int largoPAtaSegunDiam_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(_diamtro);

                RebarHookType _hook = TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{largoPAtaSegunDiam_cm}", _doc, false);

                if (_hook == null)
                {
                    Util.ErrorMsg($"Error al buscar 'Hook_PataFundacion_{largoPAtaSegunDiam_cm}'. Cargar familia ");
                    return false;
                }

                if (_hoookPAthRein.rebarHookTypePrincipal_star != null)
                    ParameterUtil.SetParaElementIdNH(pathFund, BuiltInParameter.PATH_REIN_HOOK_TYPE_1, _hook.Id);

                if (_hoookPAthRein.rebarHookTypePrincipal_end != null)
                    ParameterUtil.SetParaElementIdNH(pathFund, BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1, _hook.Id);

         

            
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al actualizar Hook \n ex:{ex.Message}");
                result= false;
            }
            return result;
        }


        public bool EjecutarActualizarTAg(PathReinforcement pathFund)
        {
            bool result = true;
            try
            {
                HookPAthRein _hoookPAthRein = pathFund.ObtenerHooksOP2();
                int _diamtro = pathFund.ObtenerDiametro_mm();
                int largoPAtaSegunDiam_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(_diamtro);

                string largoPArcial = pathFund.ObtenerLargoParcialFUndaciones_String();
                double largoTotal = pathFund.ObtenerLargoParcialFUndaciones_double();
                //agergar parametros
                if (ParameterUtil.FindParaByName(pathFund, "LargoTotal") != null)
                    ParameterUtil.SetParaInt(pathFund, "LargoTotal", largoTotal.ToString());


                if (ParameterUtil.FindParaByName(pathFund, "LargoParciales") != null)
                    ParameterUtil.SetParaInt(pathFund, "LargoParciales", largoPArcial);

                double largo = Util.FootToCm(pathFund.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_1).AsDouble());

                ParameterUtil.SetParaDoubleNH(pathFund, "A_", 0);
                
                if (_hoookPAthRein.rebarHookTypePrincipal_end != null && _hoookPAthRein.rebarHookTypePrincipal_star != null)
                {
                    ParameterUtil.SetParaDoubleNH(pathFund, "B_", Util.CmToFoot(largoPAtaSegunDiam_cm));
                    ParameterUtil.SetParaDoubleNH(pathFund, "C_", Util.CmToFoot(largo));
                    ParameterUtil.SetParaDoubleNH(pathFund, "D_", Util.CmToFoot(largoPAtaSegunDiam_cm));
                }
                else if (_hoookPAthRein.rebarHookTypePrincipal_end != null || _hoookPAthRein.rebarHookTypePrincipal_star != null)
                {
                    ParameterUtil.SetParaDoubleNH(pathFund, "B_", Util.CmToFoot(largoPAtaSegunDiam_cm));
                    ParameterUtil.SetParaDoubleNH(pathFund, "C_", Util.CmToFoot(largo));
                    ParameterUtil.SetParaDoubleNH(pathFund, "D_", 0);
                }
                else if (_hoookPAthRein.rebarHookTypePrincipal_end == null && _hoookPAthRein.rebarHookTypePrincipal_star == null)
                {
                    ParameterUtil.SetParaDoubleNH(pathFund, "B_", Util.CmToFoot(largo));
                    ParameterUtil.SetParaDoubleNH(pathFund, "C_", 0.0);
                    ParameterUtil.SetParaDoubleNH(pathFund, "D_", 0.0);
                }


                //cambiar pathsymbol          
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al actualizar Hook \n ex:{ex.Message}");
                result= false;
            }
            return result;
        }
    }
}
