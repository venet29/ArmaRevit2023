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
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Servicio.WPF_EText;

namespace ArmaduraLosaRevit.Model.Niveles.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_Ui_Nivel
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

   


        public static void M1_EjecutarRutinas(Ui_Nivel ui_UIGrid, UIApplication _uiapp)
        {

            
            string tipoPosiicon = ui_UIGrid.BotonOprimido;
       
            if (tipoPosiicon == "btnAceptar_e")
            {

                ui_UIGrid.Hide();

                ManejadorCrearTextoEntreNIvel _ManejadorCrearTextoEntreNIvel = new ManejadorCrearTextoEntreNIvel(_uiapp);
                _ManejadorCrearTextoEntreNIvel.Ejecutar(new List<View>());
                //_ManejadorCrearTextoEntreNIvel.EjecutarMultiple();
                ui_UIGrid.Show();

            }
          else  if (tipoPosiicon == "btnMultiple_e")
            {

                ui_UIGrid.Hide();

                ManejadorCrearTextoEntreNIvel _ManejadorCrearTextoEntreNIvel = new ManejadorCrearTextoEntreNIvel(_uiapp);
                _ManejadorCrearTextoEntreNIvel.EjecutarMultiple();
                ui_UIGrid.Show();

            }
            
            else if (tipoPosiicon == "btnAceptarBorrar1_e")
            {

                ui_UIGrid.Hide();
                ManejadorCrearTextoEntreNIvel _ManejadorCrearTextoEntreNIvel = new ManejadorCrearTextoEntreNIvel(_uiapp);
                _ManejadorCrearTextoEntreNIvel.BorrarTExtoViewActualw();
                ui_UIGrid.Show();


            }
            else if (tipoPosiicon == "btnBorrarMultiple_e")
            {

                ui_UIGrid.Hide();
                ManejadorCrearTextoEntreNIvel _ManejadorCrearTextoEntreNIvel = new ManejadorCrearTextoEntreNIvel(_uiapp);
                _ManejadorCrearTextoEntreNIvel.BorrarTExtoMultiplesView();
                ui_UIGrid.Show();


            }


        }

  

    }
}