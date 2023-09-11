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
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;

namespace ArmaduraLosaRevit.Model.LosaEstructural.WPFp
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_pelotaLosa
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

   


        public static void M1_EjecutarRutinas(Ui_pelotaLosa ui_pelotaLosa, UIApplication uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView)) return;
            string tipoPosiicon = ui_pelotaLosa.BotonOprimido;
       
            if (tipoPosiicon == "btnAceptar_e" && ui_pelotaLosa.tipoPElota.Text== "Estructura")
            {
                ui_pelotaLosa.Hide();
                CreardorPelotaLosaEstructural CrearPelotaLosaEstructural = new CreardorPelotaLosaEstructural(uiapp);
                CrearPelotaLosaEstructural.Ejecutar_e(ui_pelotaLosa.tbx_nombre_e.Text, ui_pelotaLosa.tbx_angulo_e.Text);
                ui_pelotaLosa.Show();
            }

            else if (tipoPosiicon == "btnAceptar_var" && ui_pelotaLosa.tipoPElota.Text == "Estructura")
            {
                ui_pelotaLosa.Hide();
                CreardorPelotaLosaEstructural CrearPelotaLosaEstructural = new CreardorPelotaLosaEstructural(uiapp);
                CrearPelotaLosaEstructural.EjecutarVAr(ui_pelotaLosa.tbx_nombre_var.Text, ui_pelotaLosa.tbx_angulo_var.Text, ui_pelotaLosa.tbx_espesor_var.Text);
                ui_pelotaLosa.Show();
            }

            if (tipoPosiicon == "btnAceptar_e" && ui_pelotaLosa.tipoPElota.Text == "Armadura")
            {
                ui_pelotaLosa.Hide();
                CreardorPelotaLosaEstructural CrearPelotaLosaEstructural = new CreardorPelotaLosaEstructural(uiapp);
                CrearPelotaLosaEstructural.Ejecutar_e_Armadura(ui_pelotaLosa.tbx_nombre_e.Text, ui_pelotaLosa.tbx_angulo_e.Text, ui_pelotaLosa.tbx_DireV_e.Text, ui_pelotaLosa.tbx_DireH_e.Text);

                ui_pelotaLosa.Show();
            }

            else if (tipoPosiicon == "btnAceptar_var" && ui_pelotaLosa.tipoPElota.Text == "Armadura")
            {
                ui_pelotaLosa.Hide();
                Util.InfoMsg("En desarrollo.");
                ui_pelotaLosa.Show();
            }
            else if (tipoPosiicon == "btnSup" || tipoPosiicon == "btnIzq" || tipoPosiicon == "btnDere" || tipoPosiicon == "btnInf")
            {
             
            }
            else if (tipoPosiicon == "btnCerrar_e")
            {
                ui_pelotaLosa.Close();
            }
            else if (tipoPosiicon == "btnCambiarTipo")
            {
               
            }
            //CargarCambiarPathReinfomenConPto_Wpf
        }

  

    }
}