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
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraEstriboV.WPFv;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.BarraV.Borrar;

namespace ArmaduraLosaRevit.Model.GRIDS.NombreEje.WPFGrid
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_Ui_UIGrid
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

   


        public static void M1_EjecutarRutinas(Ui_UIGrid ui_UIGrid, UIApplication uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView)) return ;
            string tipoPosiicon = ui_UIGrid.BotonOprimido;
       
            if (tipoPosiicon == "btnAceptar_e")
            {

                ui_UIGrid.Hide();
                MAnejadorCrearTextoEje _MAnejadorCrearTextoEje = new MAnejadorCrearTextoEje(uiapp);
                _MAnejadorCrearTextoEje.GenerarTexto(ui_UIGrid.tbx_nombre_e.Text);

                ui_UIGrid.Show();


            }
             else 
            {
                ui_UIGrid.Close();
                //   CreardorPelotaLosaEstructural CrearPelotaLosaEstructural = new CreardorPelotaLosaEstructural(uiapp);
                //  CrearPelotaLosaEstructural.EjecutarVAr(ui_pelotaLosa.tbx_nombre_var.Text, ui_pelotaLosa.tbx_angulo_var.Text, ui_pelotaLosa.tbx_espesor_var.Text);

            }
    
        }

  

    }
}