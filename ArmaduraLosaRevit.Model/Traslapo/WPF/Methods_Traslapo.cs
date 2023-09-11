using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.Renombrar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Events;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;

namespace ArmaduraLosaRevit.Model.Traslapo.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    /// 


    internal class Methods_Traslapo
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



        /// <summary>
        /// Print the Title of the Revit Document on the main text box of the WPF window of this application.
        /// </summary>
        /// <param name="ui">An instance of our UI class, which in this template is the main WPF
        /// window of the application.</param>
        /// <param name="doc">The Revit Document to print the Title of.</param>


        public static void M1_DocumentEditPAth(Ui_traslapo ui, UIApplication _uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            string tipoPosiicon = ui.BotonOprimido;


            //** para denombrar familia pathsymbol 13-04-2022
            ManejadorReNombrarFamPathSymbol _ManejadorReNombrar = new ManejadorReNombrarFamPathSymbol(_uiapp);
            if (_ManejadorReNombrar.IsFamiliasAntiguas())
                _ManejadorReNombrar.Renombarra();
            //**


            CalcularLargoTraslapoPAthDTO _CalcularLargoPAthDTO=     ui.ObtenerParametrosENtrada();
            if (tipoPosiicon == "BTraslapar")
            {
                ui.Hide();

                //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
                UtilitarioFallasDialogosCarga.Cargar(_uiapp);
                try
                {


                    SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiapp);
                    if (seleccionarPathReinfomentConPto.SeleccionarPathReinforment())
                    {
                        //cargardatos
                        PathReinformeTraslapoManejador pathReinformeTraslapo = new PathReinformeTraslapoManejador(_uiapp, seleccionarPathReinfomentConPto, _CalcularLargoPAthDTO);
                        pathReinformeTraslapo.M0_EjecutarTraslapo();
                    }

                }
                catch (System.Exception ex)
                {

                    string msje = ex.Message;

                }
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                ui.Show();
            }


            UpdateGeneral.M2_CargarBArras(_uiapp);

            //CargarCambiarPathReinfomenConPto_Wpf
        }


    }
}