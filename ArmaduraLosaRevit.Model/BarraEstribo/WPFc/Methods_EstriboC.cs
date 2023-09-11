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
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.BarraV.Borrar;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraEstribo.Editar;

namespace ArmaduraLosaRevit.Model.BarraEstribo.WPFc
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_EstriboC
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




        public static void M1_EjecutarRutinas(Ui_EstriboC ui_EstriboC, UIApplication _uiapp)
        {

            //btnCerrar_e

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;

            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);



            string tipoPosiicon = ui_EstriboC.BotonOprimido;

            if (tipoPosiicon == "btnCerrar_e")
            {
                ui_EstriboC.Close();
            }
            else if (tipoPosiicon == "btnBorrarRebarEstribo")
            {
                ui_EstriboC.Hide();
                BorrarRebarRectangulo.EjecutarElevaciones(_uiapp, "soloCOnf");

                ui_EstriboC.Show();
            }
            else
            {

                if (ui_EstriboC.Rdbton_dibujar.IsChecked == true)
                {

                    ui_EstriboC.Hide();
                    try
                    {
                        DatosConfinamientoAutoDTO configuracionInicialEstriboDTO =
                                    new DatosConfinamientoAutoDTO()
                                    {
                                        DiamtroEstriboMM = Util.ConvertirStringInInteger(ui_EstriboC.diam_estribo.Text),
                                        espaciamientoEstriboCM = Util.ConvertirStringInDouble(ui_EstriboC.espa_estribo.Text),
                                        cantidadEstribo = ui_EstriboC.tipo_estr.Text,

                                        DiamtroLateralEstriboMM = 8,
                                        cantidadLaterales = 0,

                                        DiamtroTrabaEstriboMM = Util.ConvertirStringInInteger(ui_EstriboC.diam_traba.Text),
                                        espaciamientoTrabaCM = Util.ConvertirStringInDouble(ui_EstriboC.espa_traba.Text),
                                        cantidadTraba = Util.ConvertirStringInInteger(ui_EstriboC.Cantidad_traba.Text),

                                        tipoConfiguracionEstribo = TipoConfiguracionEstribo.Estribo,
                                        tipoEstriboGenera = TipoEstriboGenera.EConfinamiento,
                                        tipoTraba_posicion = EnumeracionBuscador.ObtenerEnumGenerico(TipoTraba_posicion.A, ui_EstriboC.tipo_trabaOrient.Text),

                                        PtoInferior = (ui_EstriboC.rbt_ini_C_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                                        PtoSuperior = (ui_EstriboC.rbt_sup_C_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),

                                        IsEstribo = ui_EstriboC.chbox_estribo.IsChecked,
                                        IsLateral = false,
                                        IsTraba = ui_EstriboC.chbox_traba.IsChecked
                                    };

                        ManejadorEstriboCOnfinamiento manejadorEstriboCOnfinamiento = new ManejadorEstriboCOnfinamiento(_uiapp, configuracionInicialEstriboDTO);
                        manejadorEstriboCOnfinamiento.M1_Ejecutar();
                    }
                    catch (Exception)
                    {


                    }
                    ui_EstriboC.Show();

                }

                else if (ui_EstriboC.Rdbton_Editar.IsChecked == true)
                {
                    DatosConfinamientoAutoDTO configuracionInicialEstriboDTO =
                                    new DatosConfinamientoAutoDTO()
                                    {
                                        DiamtroEstriboMM = Util.ConvertirStringInInteger(ui_EstriboC.diam_estribo.Text),
                                        espaciamientoEstriboCM = Util.ConvertirStringInDouble(ui_EstriboC.espa_estribo.Text),
                                        cantidadEstribo = ui_EstriboC.tipo_estr.Text,

                                        DiamtroLateralEstriboMM = 8,
                                        cantidadLaterales = 0,

                                        DiamtroTrabaEstriboMM = Util.ConvertirStringInInteger(ui_EstriboC.diam_traba.Text),
                                        espaciamientoTrabaCM = Util.ConvertirStringInDouble(ui_EstriboC.espa_traba.Text),
                                        cantidadTraba = Util.ConvertirStringInInteger(ui_EstriboC.Cantidad_traba.Text),

                                        tipoConfiguracionEstribo = TipoConfiguracionEstribo.Estribo,
                                        tipoEstriboGenera = TipoEstriboGenera.EConfinamiento,
                                        tipoTraba_posicion = EnumeracionBuscador.ObtenerEnumGenerico(TipoTraba_posicion.A, ui_EstriboC.tipo_trabaOrient.Text),

                                        PtoInferior = (ui_EstriboC.rbt_ini_C_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                                        PtoSuperior = (ui_EstriboC.rbt_sup_C_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),

                                        IsEstribo = ui_EstriboC.chbox_estribo.IsChecked,
                                        IsLateral = false,
                                        IsTraba = ui_EstriboC.chbox_traba.IsChecked
                                    };
                    ManejadorEditarDiamtroEspacimiento _ManejadorEditarDiamtroEspacimiento = new ManejadorEditarDiamtroEspacimiento(_uiapp, configuracionInicialEstriboDTO);
                    _ManejadorEditarDiamtroEspacimiento.CalculosIniciales();
                    _ManejadorEditarDiamtroEspacimiento.Ejecutar("soloCOnf");


                  //  Util.ErrorMsg("Edicion de trabas aun no implemnetado");
                    //   CreardorPelotaLosaEstructural CrearPelotaLosaEstructural = new CreardorPelotaLosaEstructural(uiapp);
                    //  CrearPelotaLosaEstructural.EjecutarVAr(ui_pelotaLosa.tbx_nombre_var.Text, ui_pelotaLosa.tbx_angulo_var.Text, ui_pelotaLosa.tbx_espesor_var.Text);

                }
            }

            UpdateGeneral.M2_CargarBArras(_uiapp);
        }



    }
}