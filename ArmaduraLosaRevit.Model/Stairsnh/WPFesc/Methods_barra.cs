using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.EditarTipoPath;
using ArmaduraLosaRevit.Model.WPF;
using System.Runtime.InteropServices.WindowsRuntime;
using ArmaduraLosaRevit.Model.RebarLosa;
using ArmaduraLosaRevit.Model.Stairsnh.DTO;

namespace ArmaduraLosaRevit.Model.Stairsnh.WPFesc
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_barra
    {
        /// <summary>
        /// Method for collecting sheets as an asynchronous operation on another thread.
        /// </summary>
        /// <param name="doc">The Revit Document to collect sheets from.</param>
        /// <returns>A list of collected sheets, once the Task is resolved.</returns>
        private static async Task<List<ViewSheet>> GetSheets(Document doc)
        {
            return await Task.Run(() =>
            {
                UtilWPF.LogThreadInfo("Get Sheets Method");
                return new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .Select(p => (ViewSheet)p).ToList();
            });
        }

   


        public static void M1_EjecutarRutinas(Ui_barraEscaleras ui_barraEscaleras, UIApplication uiapp)

        {

            if (!VerificarDatos(ui_barraEscaleras)) return;

            try
            {
        
            ui_barraEscaleras.Hide();
                DatosFormularios _datosFormularios = new DatosFormularios()
                {
                    diaLongMM = Util.ConvertirStringInInteger(ui_barraEscaleras.dtDiaLong.Text),
                    diaTransMM = Util.ConvertirStringInInteger(ui_barraEscaleras.dtDiaTrans.Text),
                    espaciLongCm = Util.ConvertirStringInDouble(ui_barraEscaleras.espalong.Text),
                    espaciTrasnCM = Util.ConvertirStringInDouble(ui_barraEscaleras.espaTrans.Text),
                    LargoPataEnLosaCm = largoTRaslapo(Util.ConvertirStringInInteger(ui_barraEscaleras.dtDiaLong.Text))
                };

                //TipoBarra varsad = EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, _probar_fx_sx.tipobarra);
                BarraEscaleraManejador barraManejador = new BarraEscaleraManejador(uiapp);
                barraManejador.BarraInferioresEscalera(_datosFormularios);

                ui_barraEscaleras.Show();
            }
            catch (Exception)
            {

                ui_barraEscaleras.Show();
                ui_barraEscaleras.Close();
            }
            //CargarCambiarPathReinfomenConPto_Wpf
        }

        private static bool VerificarDatos(Ui_barraEscaleras ui_barraEscaleras)
        {

            if (!Util.IsNumeric(ui_barraEscaleras.dtDiaLong.Text))
            {
                Util.ErrorMsg("Diamtro longitudinal no es dato nomerico");
                return false;
            }
            if (!Util.IsNumeric(ui_barraEscaleras.dtDiaTrans.Text))
            {
                Util.ErrorMsg("Diamtro transversal no es dato nomerico");
                return false;
            }

            if (!Util.IsNumeric(ui_barraEscaleras.espalong.Text))
            {
                Util.ErrorMsg("Espaciamiento longitudinal no es dato nomerico");
                return false;
            }

            if (!Util.IsNumeric(ui_barraEscaleras.espaTrans.Text))
            {
                Util.ErrorMsg("Espaciamiento transversal no es dato nomerico");
                return false;
            }

            double largolong = Util.ConvertirStringInDouble(ui_barraEscaleras.espalong.Text);
            if (!(largolong >= 7.5 && largolong < 30))
            {
                Util.ErrorMsg("Espaciamiento longitudinal debe estar entre 7.5 y 30cm");
                return false;
            }

            double largotrans = Util.ConvertirStringInDouble(ui_barraEscaleras.espaTrans.Text);
            if (!(largotrans>=7.5 && largotrans<30))
            {
                Util.ErrorMsg("Espaciamiento transversal debe estar entre 7.5 y 30cm");
                return false;
            }
            

            return true;

        }

        private static double largoTRaslapo(int diam)
        {
           
            double _largoTraslapo = UtilBarras.largo_traslapoFoot_diamMM(diam);
            double largoDefult = Util.CmToFoot(10);
            return Util.FootToCm((largoDefult / 2 > _largoTraslapo / 2 ? largoDefult / 2 : _largoTraslapo / 2));
        }
    }
}