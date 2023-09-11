using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.BarraV.Borrar;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraEstriboV.Editar;
using ArmaduraLosaRevit.Model.BarraV;
using ArmaduraLosaRevit.Model.BarraEstribo.Editar;
using ArmaduraLosaRevit.Model.BarraEstriboP.WPFp;

namespace ArmaduraLosaRevit.Model.BarraEstriboV.WPFv
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_EstriboV
    {
        public static void M1_EjecutarRutinas(Ui_EstriboV ui_EstriboV, UIApplication _uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);

            string tipoPosiicon = ui_EstriboV.BotonOprimido;
            ui_EstriboV.Hide();
            if (tipoPosiicon == "btnAceptar_Viga" && ui_EstriboV.Rdbton_dibujar.IsChecked == true)
            {
                try
                {
                    DatosConfinamientoAutoDTO configuracionInicialEstriboDTO = ui_EstriboV.ObtenerPArametrosDIseño();

                    if (configuracionInicialEstriboDTO == null) return;
                    ManejadorEstriboCOnfinamiento manejadorEstriboCOnfinamiento = new ManejadorEstriboCOnfinamiento(_uiapp, configuracionInicialEstriboDTO);
                    manejadorEstriboCOnfinamiento.M1_Ejecutar();

                    // para crear barras Horizontal auto
                    CrearBarrarHorizontales(ui_EstriboV, _uiapp, manejadorEstriboCOnfinamiento);
                }
                catch (Exception)
                {

                }
            }
            else if (tipoPosiicon == "btnAceptar_Viga" &&  ui_EstriboV.Rdbton_Editar.IsChecked==true)
            {
                try
                {

                    DatosConfinamientoAutoDTO configuracionInicialEstriboDTO = ui_EstriboV.ObtenerPArametrosDIseño();

                    ManejadorEditarDiamtroEspacimiento _ManejadorEditarDiamtroEspacimiento = new ManejadorEditarDiamtroEspacimiento(_uiapp, configuracionInicialEstriboDTO);
                    _ManejadorEditarDiamtroEspacimiento.CalculosIniciales();
                    _ManejadorEditarDiamtroEspacimiento.Ejecutar("soloEstriboViga");
                }
                catch (Exception)
                {

                }
            }
            else if (tipoPosiicon == "btnActualizarEstrivo_Viga")
            {
                ManejarEditar _ManejarEditar = new ManejarEditar(_uiapp);
                _ManejarEditar.ReAjecutarEstribo();
            }
            else if (tipoPosiicon == "btnBorrarRebarViga")
            {
                BorrarRebarRectangulo.EjecutarElevaciones(_uiapp, "soloEstriboViga");
            }
            else if (tipoPosiicon == "btnAceptar_var")
            {
                //   CreardorPelotaLosaEstructural CrearPelotaLosaEstructural = new CreardorPelotaLosaEstructural(uiapp);
                //  CrearPelotaLosaEstructural.EjecutarVAr(ui_pelotaLosa.tbx_nombre_var.Text, ui_pelotaLosa.tbx_angulo_var.Text, ui_pelotaLosa.tbx_espesor_var.Text);
            }
            ui_EstriboV.Show();
            UpdateGeneral.M2_CargarBArras(_uiapp);
        }

        private static void CrearBarrarHorizontales(Ui_EstriboV ui_EstriboV, UIApplication _uiapp, ManejadorEstriboCOnfinamiento manejadorEstriboCOnfinamiento)
        {
            //CREAR BARRA HORIZONTALES DE BORDE
            if ((bool)ui_EstriboV.chbox_barraHInf.IsChecked || (bool)ui_EstriboV.chbox_barraHSup.IsChecked)
            {
                ConfiguracionInicialBarraHorizontalDTO confiEnfierrado_BarrasSUperiorDTO = new ConfiguracionInicialBarraHorizontalDTO()
                {
                    IsDibujarBArra = (bool)ui_EstriboV.chbox_barraHSup.IsChecked,
                    Inicial_Cantidadbarra = ui_EstriboV.cantidadSup.Text,
                    incial_diametroMM = Util.ConvertirStringInInteger(ui_EstriboV.diam_Sup.Text),
                    Inicial_espacienmietoCm_direccionmuro = ui_EstriboV.esp_Sup.Text,
                };

                ConfiguracionInicialBarraHorizontalDTO confiEnfierrado_BArrasInferiorDTO = new ConfiguracionInicialBarraHorizontalDTO()
                {
                    IsDibujarBArra = (bool)ui_EstriboV.chbox_barraHInf.IsChecked,
                    Inicial_Cantidadbarra = ui_EstriboV.cantidadInf.Text,
                    incial_diametroMM = Util.ConvertirStringInInteger(ui_EstriboV.diam_Inf.Text),
                    Inicial_espacienmietoCm_direccionmuro = ui_EstriboV.esp_Inf.Text
                };

                ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);

                ManejadorBarraHVigaAutoEstribo ManejadorBarraH2 = new ManejadorBarraHVigaAutoEstribo(_uiapp, _seleccionarNivel, confiEnfierrado_BarrasSUperiorDTO);

                XYZ ptocentrocara = manejadorEstriboCOnfinamiento.generarDatosIniciales_Service._SeleccionPtosEstriboViga_sinSeleccionBarras._ptoSeleccionMouseCaraMuro;
                Element VigaSeleccion = manejadorEstriboCOnfinamiento.generarDatosIniciales_Service._SeleccionPtosEstriboViga_sinSeleccionBarras._ElemetSelect;


                ManejadorBarraH2.CrearBArraHorizontalVigaAutoEstribo(confiEnfierrado_BArrasInferiorDTO, ptocentrocara, VigaSeleccion);
            }
        }
    }
}