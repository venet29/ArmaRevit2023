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
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;

namespace ArmaduraLosaRevit.Model.RefuerzoSupleMuro.WPFp
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_RefuerzoSupleMuro
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

   


        public static void M1_EjecutarRutinas(Ui_RefuerzoSupleMuro ui_RefuerzoSupleMuro, UIApplication uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView)) return ;
            string tipoPosiicon = ui_RefuerzoSupleMuro.BotonOprimido;
       
            if (tipoPosiicon == "btnAceptar_e")
            {
                ui_RefuerzoSupleMuro.Hide();
                DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales = ui_RefuerzoSupleMuro.ObtenerDatosNuevaBarraDTO();
     

                RefuerzoSupleMuroManejador _fundManejador = new RefuerzoSupleMuroManejador(uiapp);
                _fundManejador.execute(_datosNuevaBarraDTOIniciales);
                ui_RefuerzoSupleMuro.Show();
            }


            else if (tipoPosiicon == "btnCerrar_e")
            {
                ui_RefuerzoSupleMuro.Close();
            }
            else if (tipoPosiicon == "btnCambiarTipo")
            {
               
            }
            //CargarCambiarPathReinfomenConPto_Wpf
        }

  

    }
}