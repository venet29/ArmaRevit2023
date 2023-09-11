using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Fund.WPFfund.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund
{
    public class AyudaOBtenerHookYLargo
    {
        public static (RebarHookType, RebarHookType, double, double, bool) ObtenerHookFundaciones(Document _doc, int dimBarras_mm)
        {
            RebarHookType hookIZ = default;
            RebarHookType hookDere = default;
            double LArgoPataHOokIzq_cm = 0;
            double LArgoPataHOokDere_cm = 0;
            try
            {
                LArgoPataHOokIzq_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(dimBarras_mm);
                LArgoPataHOokDere_cm = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(dimBarras_mm);

                double ptaIzq = 0;// Math.Max(FactoresLargoLeader.LargoPaTaIzq_cm, LArgoPataHOokIzq_cm);
                double ptaDere = 0;// Math.Max(FactoresLargoLeader.LargoPaTaDere_cm, LArgoPataHOokDere_cm);

                if (FactoresLargoLeader.TipoFundacion == "Fundacion")
                {
                    ptaIzq = Math.Max(FactoresLargoLeader.LargoPaTaIzq_cm, LArgoPataHOokIzq_cm);
                    ptaDere = Math.Max(FactoresLargoLeader.LargoPaTaDere_cm, LArgoPataHOokDere_cm);
                }
                else
                {
                    ptaIzq = FactoresLargoLeader.LargoPaTaIzq_cm;
                    ptaDere = FactoresLargoLeader.LargoPaTaDere_cm;
                }


                hookIZ = TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{ptaIzq}", _doc,false);
                hookDere = TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{ptaDere}", _doc, false);


                try
                {
                    //izquierdo
                    if (FactoresLargoLeader.LargoPaTaIzq_cm != 25)
                    {
                        // hookIZ = TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{FactoresLargoLeader.LargoPaTaIzq_cm}", _doc);
                        if (hookIZ == null)
                        {
                            if (TipoRebarHookType.CrearHook(_doc, FactoresLargoLeader.LargoPaTaIzq_cm))
                            {
                                hookIZ = TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{FactoresLargoLeader.LargoPaTaIzq_cm}", _doc);
                                TipoRebarHookType.CrearHook_asignarLargosARebar(_doc, hookIZ, FactoresLargoLeader.LargoPaTaIzq_cm);
                            }
                            else
                            {
                                Util.ErrorMsg($"No se pudo crear Pata fundacion {FactoresLargoLeader.LargoPaTaDere_cm}");
                                return (hookIZ, hookDere, LArgoPataHOokIzq_cm, LArgoPataHOokDere_cm, false);
                            }
                        }
                        LArgoPataHOokIzq_cm = FactoresLargoLeader.LargoPaTaIzq_cm;
                    }
                    // derecho
                    if (FactoresLargoLeader.LargoPaTaDere_cm != 25)
                    {
                        // hookDere = TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{FactoresLargoLeader.LargoPaTaDere_cm}", _doc);
                        if (hookDere == null)
                        {
                            if (TipoRebarHookType.CrearHook(_doc, FactoresLargoLeader.LargoPaTaDere_cm))
                            {
                                hookDere = TipoRebarHookType.ObtenerHook($"Hook_PataFundacion_{FactoresLargoLeader.LargoPaTaDere_cm}", _doc);
                                TipoRebarHookType.CrearHook_asignarLargosARebar(_doc, hookDere, FactoresLargoLeader.LargoPaTaDere_cm);
                            }
                            else
                            {
                                Util.ErrorMsg($"No se pudo crear Pata fundacion {FactoresLargoLeader.LargoPaTaDere_cm}");
                                return (hookIZ, hookDere, LArgoPataHOokIzq_cm, LArgoPataHOokDere_cm, false);
                            }
                        }
                        LArgoPataHOokDere_cm = FactoresLargoLeader.LargoPaTaDere_cm;
                    }
                }
                catch (Exception)
                {
                    return (hookIZ, hookDere, LArgoPataHOokIzq_cm, LArgoPataHOokDere_cm, false);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ayuda para obtner Hook. \n ex:{ex.Message}");
            }
            return (hookIZ, hookDere, LArgoPataHOokIzq_cm, LArgoPataHOokDere_cm, true);
        }
    }
}
