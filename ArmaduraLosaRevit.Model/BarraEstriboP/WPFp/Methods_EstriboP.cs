using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.WPF;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.BarraV.Borrar;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraEstribo.Editar;
using ArmaduraLosaRevit.Model.BarraEstribo.WPFc;

namespace ArmaduraLosaRevit.Model.BarraEstriboP.WPFp
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_EstriboP
    {





        public static void M1_EjecutarRutinas(Ui_EstriboP ui_EstriboP, UIApplication _uiapp)
        {
            string tipoPosiicon = ui_EstriboP.BotonOprimido;
            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            if (tipoPosiicon == "btnAceptar_E" && ui_EstriboP.Rdbton_dibujar.IsChecked == true)
            {
                ui_EstriboP.Hide();
                try
                {
                    DatosConfinamientoAutoDTO configuracionInicialEstriboDTO = ObtenerCOnfigracion_wpf(ui_EstriboP);

                    ManejadorEstriboCOnfinamiento manejadorEstriboCOnfinamiento = new ManejadorEstriboCOnfinamiento(_uiapp, configuracionInicialEstriboDTO);
                    manejadorEstriboCOnfinamiento.M1_Ejecutar();
                }
                catch (Exception)
                {

                }
                ui_EstriboP.Show();
            }


            else if (tipoPosiicon == "btnAceptar_E" && ui_EstriboP.Rdbton_Editar.IsChecked == true)
            {
                ui_EstriboP.Hide();

                DatosConfinamientoAutoDTO configuracionInicialEstriboDTO = ObtenerCOnfigracion_wpf(ui_EstriboP);

                ManejadorEditarDiamtroEspacimiento _ManejadorEditarDiamtroEspacimiento = new ManejadorEditarDiamtroEspacimiento(_uiapp, configuracionInicialEstriboDTO);
                _ManejadorEditarDiamtroEspacimiento.CalculosIniciales();
                _ManejadorEditarDiamtroEspacimiento.Ejecutar("soloEstriboMuro");


                ui_EstriboP.Show();
            }


            else if (tipoPosiicon == "btnBorrarRebarMuro")
            {
                ui_EstriboP.Hide();
                BorrarRebarRectangulo.EjecutarElevaciones(_uiapp, "soloEstriboMuro");

                ui_EstriboP.Show();
            }
            else if (tipoPosiicon == "btnCerrar_E")
            {
                ui_EstriboP.Close();

            }


            UpdateGeneral.M2_CargarBArras(_uiapp);
        }

        private static DatosConfinamientoAutoDTO ObtenerCOnfigracion_wpf(Ui_EstriboP ui_EstriboP)
        {
            return new DatosConfinamientoAutoDTO()
            {
                DiamtroEstriboMM = Util.ConvertirStringInInteger(ui_EstriboP.diam_estribo.Text),
                espaciamientoEstriboCM = Util.ConvertirStringInDouble(ui_EstriboP.espa_estribo.Text),
                cantidadEstribo = ui_EstriboP.tipo_estribo.Text,

                DiamtroLateralEstriboMM = Util.ConvertirStringInInteger(ui_EstriboP.diam_lat.Text),
                cantidadLaterales = Util.ConvertirStringInInteger(ui_EstriboP.cantidad_lat.Text),

                DiamtroTrabaEstriboMM = Util.ConvertirStringInInteger(ui_EstriboP.diam_traba.Text),
                espaciamientoTrabaCM = Util.ConvertirStringInDouble(ui_EstriboP.espa_traba.Text),
                cantidadTraba = Util.ConvertirStringInInteger(ui_EstriboP.cantidads_traba.Text),

                tipoConfiguracionEstribo = TipoConfiguracionEstribo.EstriboMuro,
                tipoEstriboGenera = TipoEstriboGenera.EMuro,
                tipoTraba_posicion = EnumeracionBuscador.ObtenerEnumGenerico(TipoTraba_posicion.A, ui_EstriboP.tipo_trabaOrient.Text),

                PtoInferior = (ui_EstriboP.rbt_ini_P_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),
                PtoSuperior = (ui_EstriboP.rbt_sup_P_M.IsChecked == true ? TipoSeleccionMouse.mouse : TipoSeleccionMouse.nivel),

                IsEstribo = ui_EstriboP.chbox_estribo.IsChecked,
                IsLateral = ui_EstriboP.chbox_lat.IsChecked,
                IsTraba = ui_EstriboP.chbox_traba.IsChecked
            };
        }
    }
}