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
using ArmaduraLosaRevit.Model.EditarPath.CAmbiosPAth;

namespace ArmaduraLosaRevit.Model.EditarTipoPath.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    /// 


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
            Task.Run(() =>
            {
                UtilWPF.LogThreadInfo("Sheet Rename Show Results");

                // report the count
                string message = $"There are {sheets.Count} Sheets in the project";
                // ui.Dispatcher.Invoke(() => ui.TbDebug.Text += "\n" + (DateTime.Now).ToLongTimeString() + "\t" + message);
            });

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

        public static void M1_DocumentEditPAth(Ui ui, UIApplication _uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            string tipoPosiicon = ui.BotonOprimido;
            DatosDiseño.IS_PATHREIN_AJUSTADO = VariablesSistemas.IsAjusteBarra_Recorrido;
            //DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO = (bool)ui.IsAjustar.IsChecked;

            //** para denombrar familia pathsymbol 13-04-2022
            ManejadorReNombrarFamPathSymbol _ManejadorReNombrar = new ManejadorReNombrarFamPathSymbol(_uiapp);
            if (_ManejadorReNombrar.IsFamiliasAntiguas())
                _ManejadorReNombrar.Renombarra();
            //**



            EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);
            if (tipoPosiicon == "pathToPath")
            {
                ui.Hide();
                M1_1_MetodoPathToPath(ui, EditarPathReinMouse);
                ui.Show();
            }

            else if (tipoPosiicon == "pathtoPto")
            {
                ui.Hide();
                List<ObjectSnapTypes> listaSnap = ui.ObtenerListaSnap();
                bool predefi = DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO;

                EditarPathReinMouse_ExtederPathA2punto _EditarPathReinMouse_ExtederPathApunto = new EditarPathReinMouse_ExtederPathA2punto(_uiapp, listaSnap);
                M1_2_MetodoPathToPto_pto2(ui, _EditarPathReinMouse_ExtederPathApunto);
                DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO = predefi;
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

                bool valorTraslapar = DatosDiseño.IS_PATHREIN_AJUSTADO;
                bool _iS_PATHREIN_AJUSTADO_LARGO = VariablesSistemas.IsAjusteBarra_Largo;
                DatosDiseño.IS_PATHREIN_AJUSTADO = false;
                DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO =false;

                M1_6_MetodoPathActulizarTipoPreseleccionado(ui, _uiapp);

                DatosDiseño.IS_PATHREIN_AJUSTADO = valorTraslapar;
                DatosDiseño.IS_PATHREIN_AJUSTADO = _iS_PATHREIN_AJUSTADO_LARGO;

                ui.Show();
            }
            else if (tipoPosiicon == "btnSeleccionarPath")
            {
                ui.Hide();
                M1_5_MetodoPathEditarTipoMouse(ui, _uiapp);
                ui.Show();
            }

            else if (tipoPosiicon == "btnAmbosLados2")
            {
                ui.Hide();
                M1_7_MetodoPathDistanciaBotton(ui, EditarPathReinMouse);
                ui.Show();
            }
            else if (tipoPosiicon == "btnExtenderPAth")
            {
                EditarPathReinMouse.M3_EjecutarExtenderPath(ui.direccion, Util.CmToFoot(ui.valorCm));
                return;
            }
            UpdateGeneral.M2_CargarBArras(_uiapp);

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


        private static void M1_2_MetodoPathToPto_pto2(Ui ui, EditarPathReinMouse_ExtederPathA2punto _EditarPathReinMouse_ExtederPathApunto2)
        {

            if (!Util.IsNumeric(ui.distanPtoPto.Text)) return;
            if (!Util.IsNumeric(ui.distaDefinir.Text)) return;
            // double dist = Util.ConvertirStringInDouble(ui.distanPtoPto.Text);

            TipoCasoAlternativo _tipoCasoAlternativo = ui.ObtenerTipoCasoAlternativo();
            DatosDiseño.IS_PATHREIN_AJUSTADO_LARGO = _tipoCasoAlternativo.IsAjustar;
            _EditarPathReinMouse_ExtederPathApunto2.M1_ExtederPathApunto(_tipoCasoAlternativo);

        }
        private static void M1_3_MetodoPathDistanciaBotton(Ui ui, EditarPathReinMouse EditarPathReinMouse, string tipoPosiicon)
        {
            if (!Util.IsNumeric(ui.distan.Text) || ui.distan.Text == "0")
            {
                return;
            }

            DireccionEdicionPathRein direccionEdicionPathRein = ObtenerDIreccion(tipoPosiicon);
            double dist = Util.ConvertirStringInDouble(ui.distan.Text);

            EditarPathReinMouse.M3_EjecutarExtenderPath(direccionEdicionPathRein, Util.CmToFoot(dist));


        }

        private static void M1_7_MetodoPathDistanciaBotton(Ui ui, EditarPathReinMouse EditarPathReinMouse)
        {
            if (!Util.IsNumeric(ui.distan.Text) || ui.distan.Text == "-0")
            {
                return;
            }

            double ladoIzq = 200;
            double ladoDere = 150;

            if (ui.largototal.IsChecked == true)
            {
                if (!Util.IsNumeric(ui.largoTotal.Text) || ui.largoTotal.Text == "0") return;
                double dist = Util.ConvertirStringInDouble(ui.largoTotal.Text);
                if (dist <= 0) return;
                ladoIzq = dist / 2;
                ladoDere = dist / 2;
            }
            else
            {
                if (!Util.IsNumeric(ui.largoParcialIzq.Text) || ui.largoParcialIzq.Text == "0") return;
                if (!Util.IsNumeric(ui.largoparcialDere.Text) || ui.largoparcialDere.Text == "0") return;

                ladoIzq = Util.ConvertirStringInDouble(ui.largoParcialIzq.Text);
                ladoDere = Util.ConvertirStringInDouble(ui.largoparcialDere.Text);
                if (ladoIzq <= 0) return;
                if (ladoDere <= 0) return;
            }


            TipoCasoAlternativo _tipoCasoAlternativo = new TipoCasoAlternativo() { TipoCasoAlternativo_ = TipoCasoAlternativo_enum.Proporcional };

            EditarPathReinMouse.M5_EjecutarAmbosLados(Util.CmToFoot(ladoIzq), Util.CmToFoot(ladoDere), _tipoCasoAlternativo);

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
            if (ui.IsNoCambioNAda()) return ;


            double dist = Util.ConvertirStringInDouble(ui.distan.Text);

            ui.Dispatcher.Invoke(() => ui.idElem.Text = "");
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
       

                if (ui.IsSOloCAmbioDiamtroOEspaciamieto())
                {

                    CambiarSoloDiametroEspaciamieto _CambiarSoloDiametroEspaciamieto = new CambiarSoloDiametroEspaciamieto(uiapp, ui.EstadoCambioTIpoBarras_, ui._seleccionarPathReinfomentConPto);
                    _CambiarSoloDiametroEspaciamieto.Ejecutar();

                    ui.Dispatcher.Invoke(() => ui.idElem.Text = _CambiarSoloDiametroEspaciamieto._pathReinforcement.Id.IntegerValue.ToString());
                    ui.ActualizarDatos();
                }
                else
                {

                    string tipoBarra = ObtenerCasoAoBDeAhorro.Obtenercaso_16_20_21_22_AoB(ui.dtTipo.Text, EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, ui.dtorient.Text));

                    ui._seleccionarPathReinfomentConPto._tipobarra = tipoBarra;

                    if (ui.dtorient.Text == "Derecha" && tipoBarra == "f16a")
                        ui.dtorient.Text = "Izquierda";
                    else if(ui.dtorient.Text == "Superior" && tipoBarra == "f16a")
                        ui.dtorient.Text = "Inferior";

                    CargarActualizarPathReinfoment_Wpf.ExecuteSoloActualizando_sinSeleccion(uiapp,
                                                               EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, ui.dtorient.Text),
                                                               EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, tipoBarra),
                                                               ui._TipoDireccionBarra,
                                                              Util.ConvertirStringInDouble(ui.dtDia.Text),
                                                              Util.ConvertirStringInDouble(ui.dtEsp.Text),
                                                              ui._seleccionarPathReinfomentConPto);
                }
            }
            else
            {
                Util.ErrorMsg("No se ha seleccionado ningun PathReinformeSymbol");
            }
            if (CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto != null)
                ui._seleccionarPathReinfomentConPto = ReAsignarNuevoPathCreado(ui, uiapp);

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

                string tipoBArra = ObtenerCasoAoBDeAhorro.Obtenercaso_INVERTIR_16_20_21_22_AoB(CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._tipobarra.ToString());
                tipoBArra = ObtenerCasoAoBDeAhorro.ConversortoS4(tipoBArra);

                ui.idElem.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto.PathReinforcementSymbol.Id.IntegerValue.ToString();
                ui._TipoDireccionBarra = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._TipoDireccionBarra.ToString();
                ui.dtTipo.Text = tipoBArra;
                ui.dtDia.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._diametro.ToString();
                ui.dtEsp.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._espaciamiento.ToString();
                ui.dtorient.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._direccion.ToString();
                ui.ActualizarDatos();
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