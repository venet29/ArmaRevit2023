using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.EditarTipoPath;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;

namespace ArmaduraLosaRevit.Model.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods
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
        /// Rename all the sheets in the project. This opens a transaction, and it MUST be executed
        /// in a "Valid Revit API Context", otherwise the add-in will crash. Because of this, we must
        /// wrap it in a ExternalEventHandler, as we do in the App.cs file in this template.
        /// </summary>
        /// <param name="ui">An instance of our UI class, which in this template is the main WPF
        /// window of the application.</param>
        /// <param name="doc">The Revit Document to rename sheets in.</param>
        public static void SheetRename(Ui ui, Document doc)
        {
            UtilWPF.LogThreadInfo("Sheet Rename Method");

            // get sheets - note that this may be replaced with the Async Task method above,
            // however that will only work if we want to only PULL data from the sheets,
            // and executing a transaction like below from an async collection, will crash the app
            List<ViewSheet> sheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .Select(p => (ViewSheet)p).ToList();

            // report results - push the task off to another thread
     

            // rename all the sheets, but first open a transaction
            using (Transaction t = new Transaction(doc, "Rename Sheets"))
            {
                UtilWPF.LogThreadInfo("Sheet Rename Transaction");

                // start a transaction within the valid Revit API context
                t.Start("Rename Sheets-NH");

                // loop over the collection of sheets using LINQ syntax
                foreach (string renameMessage in from sheet in sheets
                                                 let renamed = sheet.LookupParameter("Sheet Name")?.Set("TEST")
                                                 select $"Renamed: {sheet.Title}, Status: {renamed}")
                {
                    ui.Dispatcher.Invoke(() =>
                        ui.TbDebug.Text += "\n" + (DateTime.Now).ToLongTimeString() + "\t" + renameMessage);
                }

                t.Commit();
                t.Dispose();
            }

            // invoke the UI dispatcher to print the results to report completion
            ui.Dispatcher.Invoke(() => ui.TbDebug.Text += "\n" + (DateTime.Now).ToLongTimeString() + "\t" + "SHEETS HAVE BEEN RENAMED");
        }

        /// <summary>
        /// Print the Title of the Revit Document on the main text box of the WPF window of this application.
        /// </summary>
        /// <param name="ui">An instance of our UI class, which in this template is the main WPF
        /// window of the application.</param>
        /// <param name="doc">The Revit Document to print the Title of.</param>
        public static void DocumentInfo(Ui ui, UIApplication uiapp)
        {
            ui.Dispatcher.Invoke(() => ui.TbDebug.Text += "\n" + (DateTime.Now).ToLongTimeString() + "\t" + uiapp.ActiveUIDocument.Document.Title);
        }

        public static void M1_DocumentEditPAth(Ui ui, UIApplication uiapp)
        {
            if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView)) return ;

            DatosDiseño.IS_PATHREIN_AJUSTADO = VariablesSistemas.IsAjusteBarra_Recorrido;
            DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO = VariablesSistemas.IsAjusteBarra_Largo;

            string tipoPosiicon = ui.BotonOprimido;
            EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(uiapp);
            if (tipoPosiicon == "pathToPath")
            {
                ui.Hide();
                M1_1_MetodoPathToPath(ui, EditarPathReinMouse);
                ui.Show();
            }

            else if (tipoPosiicon == "pathtoPto")
            {
                ui.Hide();
                EditarPathReinMouse_ExtederPathA2punto _EditarPathReinMouse_ExtederPathApunto = new EditarPathReinMouse_ExtederPathA2punto(uiapp);
                M1_2_MetodoPathToPto_pto2(ui, _EditarPathReinMouse_ExtederPathApunto);
                ui.Show();
            }
            else if (tipoPosiicon == "btnSup" || tipoPosiicon == "btnIzq" || tipoPosiicon == "btnDere" || tipoPosiicon == "btnInf")
            {
                ui.Hide();
                M1_3_MetodoPathDistanciaBotton(ui, EditarPathReinMouse, tipoPosiicon);
                ui.Show();
            }
            else if (tipoPosiicon == "btnSoloDistancia")
            {
                ui.Hide();
                M1_4_MetodoPathDistanciaMouse(ui, EditarPathReinMouse);
                ui.Show();
            }
            else if (tipoPosiicon == "btnActualizar")
            {
                ui.Hide();
                M1_6_MetodoPathActulizarTipoPreseleccionado(ui, uiapp);
                ui.Show();
            }
            else if (tipoPosiicon == "btnCambiarTipo")
            {
                ui.Hide();
                M1_5_MetodoPathEditarTipoMouse(ui, uiapp);
                ui.Show();
            }
            //CargarCambiarPathReinfomenConPto_Wpf
        }



        private static void M1_1_MetodoPathToPath(Ui ui, EditarPathReinMouse EditarPathReinMouse)
        {

            if (!Util.IsNumeric(ui.distanPathPto.Text))
            {
                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + "A.0) Distancia mal ingresada "));
                return;
            }
            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " A.1) Iniciando Path a Path" + "\n"));
            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " A.2) Seleccionar path"));
            double dist = Util.ConvertirStringInDouble(ui.distanPathPto.Text);
            EditarPathReinMouse.M2_ExtenderPathaPath((bool)ui.Borrar2Path.IsChecked, Util.CmToFoot(dist));
            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " A.3) Finalizando "));
        }
        /*
        private static void M1_2_MetodoPathToPto(Ui ui, EditarPathReinMouse_ExtederPathApunto _EditarPathReinMouse_ExtederPathApunto)
        {

            if (!Util.IsNumeric(ui.distanPtoPto.Text))
            {
                return;
            }
            double dist = Util.ConvertirStringInDouble(ui.distanPtoPto.Text);
            _EditarPathReinMouse_ExtederPathApunto.M1_ExtederPathApunto(Util.CmToFoot(dist));

        }*/

        private static void M1_2_MetodoPathToPto_pto2(Ui ui, EditarPathReinMouse_ExtederPathA2punto _EditarPathReinMouse_ExtederPathApunto2)
        {

            if (!Util.IsNumeric(ui.distanPtoPto.Text)) return;

            double dist = Util.ConvertirStringInDouble(ui.distanPtoPto.Text);
            TipoCasoAlternativo _tipoCasoAlternativo = new TipoCasoAlternativo() {
                distancia_foot = Util.CmToFoot(dist),
                TipoCasoAlternativo_ = TipoCasoAlternativo_enum.Proporcional
            };

            _EditarPathReinMouse_ExtederPathApunto2.M1_ExtederPathApunto(_tipoCasoAlternativo);
     
        }
        private static void M1_3_MetodoPathDistanciaBotton(Ui ui, EditarPathReinMouse EditarPathReinMouse, string tipoPosiicon)
        {

            if (!Util.IsNumeric(ui.distan.Text) || ui.distan.Text == "0")
            {
                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " C.0) Distancia mal ingresada "));
                return;
            }

            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " C.1) Iniciando Editar path " + "\n"));
            DireccionEdicionPathRein direccionEdicionPathRein = ObtenerDIreccion(tipoPosiicon);
            double dist = Util.ConvertirStringInDouble(ui.distan.Text);
            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " C.2) Seleccionar path"));
            EditarPathReinMouse.M3_EjecutarExtenderPath(direccionEdicionPathRein, Util.CmToFoot(dist));

            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " C.3) Finalizando "));
        }
        private static void M1_4_MetodoPathDistanciaMouse(Ui ui, EditarPathReinMouse EditarPathReinMouse)
        {
            if (!Util.IsNumeric(ui.distan2.Text) || ui.distan2.Text == "0")
            {
                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " D.0) Distancia mal ingresada "));
                return;
            }

            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " D.1) Iniciando Path a Punto"));
            int contadorPAthSelecionado = 1;
            bool mientras = true;
            while (mientras)
            {

                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " D.2) Seleccionar el path N°" + contadorPAthSelecionado));
                double dist = Util.ConvertirStringInDouble(ui.distan2.Text);
                mientras = EditarPathReinMouse.M4_ExtederPathDistancia(Util.CmToFoot(dist)) == Result.Succeeded;
                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + "\n" + (DateTime.Now).ToLongTimeString() + " D.3) Finalizando "));
                contadorPAthSelecionado += 1;
            }
        }
        private static void M1_5_MetodoPathEditarTipoMouse(Ui ui, UIApplication uiapp)
        {
            if (!M1_5_1_ValidacionCAmbioTipo(ui)) return;

            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.1) Iniciando Editar tIPO path " + "\n"));

            double dist = Util.ConvertirStringInDouble(ui.distan.Text);
            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.2) Seleccionar path"));

            ui.idElem.Text = "";

            CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto = null;
            CargarActualizarPathReinfoment_Wpf.ExecuteSeleccionador_YActualizar(uiapp,
                                                       EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, ui.dtorient.Text),
                                                       EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, ui.dtTipo.Text),
                                                      Util.ConvertirStringInDouble(ui.dtDia.Text.Replace("d", "")),
                                                      Util.ConvertirStringInDouble(ui.dtEsp.Text),
                                                      ui.idElem.Text);
            if (CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto != null)
                ui._seleccionarPathReinfomentConPto = ReAsignarNuevoPathCreado(ui, uiapp);

            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.3) Finalizando "));
        }

        private static void M1_6_MetodoPathActulizarTipoPreseleccionado(Ui ui, UIApplication uiapp)
        {
            if (!M1_5_1_ValidacionCAmbioTipo(ui)) return;

            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.1) Iniciando Editar tIPO path " + "\n"));

            double dist = Util.ConvertirStringInDouble(ui.distan.Text);
            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.2) Seleccionar path"));
            ui.idElem.Text = "";
            CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto = null;

            if (ui._seleccionarPathReinfomentConPto.PathReinforcement.IsValidObject == false)
            {
                Util.ErrorMsg("PathReinforcement no valido. Posiblemente se haya borrado previamente");
            }
            else if (ui._seleccionarPathReinfomentConPto.PathReinforcementSymbol.IsValidObject == false)
            {
                Util.ErrorMsg("PathReinforcementSymbol no valido. Posiblemente se haya borrado previamente");
            }
            else if (ui._seleccionarPathReinfomentConPto != null)
            {
                CargarActualizarPathReinfoment_Wpf.ExecuteSoloActualizando_sinSeleccion(uiapp,
                                                           EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, ui.dtorient.Text),
                                                           EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, ui.dtTipo.Text),
                                                           ui._TipoDireccionBarra,
                                                          Util.ConvertirStringInDouble(ui.dtDia.Text),
                                                          Util.ConvertirStringInDouble(ui.dtEsp.Text),
                                                          ui._seleccionarPathReinfomentConPto);
            }
            else
            {
                Util.ErrorMsg("No se ha seleccionado ningun PathReinformeSymbol");
            }
            if (CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto != null)
                ui._seleccionarPathReinfomentConPto = ReAsignarNuevoPathCreado(ui, uiapp);

            ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.3) Finalizando "));
        }

        private static SeleccionarPathReinfomentConPto ReAsignarNuevoPathCreado(Ui ui, UIApplication uiapp)
        {
            SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPtoNew = new SeleccionarPathReinfomentConPto(uiapp.ActiveUIDocument, uiapp.Application);
            PathReinSpanSymbol aux_PathReinforcementSymbol = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto.PathReinforcementSymbol;
            if (!seleccionarPathReinfomentConPtoNew.AsignarPathReinformentSymbol(aux_PathReinforcementSymbol))
            {
                seleccionarPathReinfomentConPtoNew = null;
            }
            else
            {
                ui.idElem.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto.PathReinforcementSymbol.Id.IntegerValue.ToString();
                ui.dtTipo.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._tipobarra.ToString();
                ui.dtDia.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._diametro.ToString();
                ui.dtEsp.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._espaciamiento.ToString();
                ui.dtorient.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._direccion.ToString();
            }
            return CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto;
        }

        private static bool M1_5_1_ValidacionCAmbioTipo(Ui ui)
        {
            if (ui.dtTipo.Text == "")
            {
                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.0) Tipo mal ingresada "));
                return false;
            }
            if (ui.dtDia.Text == "")
            {
                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.0) Diametro mal ingresada "));
                return false;
            }
            if (ui.dtorient.Text == "")
            {
                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.0) Orientacion mal ingresada "));
                return false;
            }
            if (!Util.IsNumeric(ui.dtEsp.Text) || ui.dtEsp.Text == "0")
            {
                ui.Dispatcher.Invoke(() => ui.TbDebug.Text = ui.TbDebug.Text.Insert(0, "\n" + (DateTime.Now).ToLongTimeString() + " E.0) Espaciamiento mal ingresada "));
                return false;
            }
            return true;
        }


        private static DireccionEdicionPathRein ObtenerDIreccion(string tipoPosiicon)
        {
            DireccionEdicionPathRein direccionEdicionPathRein = DireccionEdicionPathRein.NONE;
            if (tipoPosiicon == "btnSup")
            {
                direccionEdicionPathRein = DireccionEdicionPathRein.Superior;
            }
            else if (tipoPosiicon == "btnIzq")
            { direccionEdicionPathRein = DireccionEdicionPathRein.Izquierda; }
            else if (tipoPosiicon == "btnDere")
            { direccionEdicionPathRein = DireccionEdicionPathRein.Derecha; }
            else if (tipoPosiicon == "btnInf")
            { direccionEdicionPathRein = DireccionEdicionPathRein.Inferior; }

            return direccionEdicionPathRein;
        }

        /// <summary>
        /// Count the walls in the Revit Document, and print the count
        /// on the main text box of the WPF window of this application.
        /// </summary>
        /// <param name="ui">An instance of our UI class, which in this template is the main WPF
        /// window of the application.</param>
        /// <param name="doc">The Revit Document to count the walls of.</param>
        public static void WallInfo(Ui ui, Document doc)
        {
            Task.Run(() =>
            {
                UtilWPF.LogThreadInfo("Wall Count Method");

                // get all walls in the document
                ICollection<Wall> walls = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType()
                    .Select(p => (Wall)p).ToList();

                // format the message to show the number of walls in the project
                string message = $"There are {walls.Count} Walls in the project";

                // invoke the UI dispatcher to print the results to the UI
                ui.Dispatcher.Invoke(() =>
                    ui.TbDebug.Text += "\n" + (DateTime.Now).ToLongTimeString() + "\t" + message);
            });
        }
    }
}