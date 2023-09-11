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

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.WPFEdB
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_EditarBarraLargo
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

   


        public static void M1_EjecutarRutinas(Ui_EditarBarraLargo _Ui_EditarBarraLargo, UIApplication uiapp)
        {
            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView)) return ;

            string tipoPosiicon = _Ui_EditarBarraLargo.BotonOprimido;
       
            if  (tipoPosiicon == "btnLargoMouse" || tipoPosiicon == "btnLargoFijo")
            {

                _Ui_EditarBarraLargo.Hide();
                try
                {


                    EditarBarraDTO newEditarBarraDTO = new EditarBarraDTO()
                    {
                        IsCambiarDiametroYEspacia = false
                    };

                    EditarBarraLargoDTO _EditarBarraLargoDTO = _Ui_EditarBarraLargo.Obtener();

                    ManejadorBarraV_LargoBarra ManejadorBarraV_CambiarBarra = new ManejadorBarraV_LargoBarra(uiapp, newEditarBarraDTO, _EditarBarraLargoDTO);
                    ManejadorBarraV_CambiarBarra.CambiarLargoBarra();
            
                }
                catch (Exception)
                {


                }
                _Ui_EditarBarraLargo.Show();


            }

    
        }

  

    }
}