using System;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraAreaPath.Borrar;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraMallaRebar;
using ArmaduraLosaRevit.Model.BarraV.Borrar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraEstribo.Editar;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.WPFm
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_Malla
    {


        public static void M1_EjecutarRutinas(Ui_Malla ui_Malla, UIApplication _uiapp)
        {
            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            string tipoPosiicon = ui_Malla.BotonOprimido;

           // //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            if (tipoPosiicon == "btnAceptar_e" && ui_Malla.Rdbton_dibujar.IsChecked==true)
            {

                if (ui_Malla.Rdbton_dibujar.IsChecked == true)
                {
                    ui_Malla.Hide();
                    try
                    {
                        bool isConAreaPath = false;

                        DatosMallasAutoDTO datosMallasDTO = ui_Malla.ObtenerDatosMallasDTO();

                        if (isConAreaPath)
                        {
                            ManejadorMallaMuro manejadorMallaMuro = new ManejadorMallaMuro(_uiapp, datosMallasDTO);
                            manejadorMallaMuro.CrearMallaMuro();
                        }
                        else
                        {                            
                            ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                            ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO = ui_Malla.ObtenerConfiguracionInicialMallaMuroVDTO();  

                            //configuracion barra verticales cabeza muro
                            // DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(uiapp.ActiveUIDocument.ActiveView, DireccionRecorrido_.PerpendicularEntradoVista);

                            //configuracion barra verticales y horizontal  malla  y Horquilla 
                            DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(_uiapp.ActiveUIDocument.ActiveView, DireccionRecorrido_.ParaleloDerechaVista, 100);


                            ManejadorBarraVMalla ManejadorBarraV =
                                new ManejadorBarraVMalla(_uiapp, _seleccionarNivel, confiWPFEnfierradoDTO, _DireccionRecorrido, datosMallasDTO);
                            //ManejadorBarraV ManejadorBarraV = new ManejadorBarraV(uiapp, _seleccionarNivel, confiEnfierradoDTO);
                            ManejadorBarraV.CrearBArra();
                        }
                    }
                    catch (Exception)
                    {

                    }
                    ui_Malla.Show();
                }
                else if (ui_Malla.Rdbton_dibujar.IsChecked == true)
                {

                }


            }

            else if (tipoPosiicon == "btnAceptar_e" && ui_Malla.Rdbton_Editar.IsChecked == true)
            {
                ui_Malla.Hide();


                DatosMallasAutoDTO datosMalla = ui_Malla.ObtenerDatosMallasDTO();

                ManejadorEditarDiamtroEspacimiento _ManejadorEditarDiamtroEspacimiento = new ManejadorEditarDiamtroEspacimiento(_uiapp, datosMalla);
                _ManejadorEditarDiamtroEspacimiento.CalculosIniciales_Malla();
                _ManejadorEditarDiamtroEspacimiento.Ejecutar("soloMalla");


                ui_Malla.Show();
            }
            else if (tipoPosiicon == "btnCerrar_e")
            {
                ui_Malla.Close();
            }
            else if (tipoPosiicon == "btnBorrar_e")
            {
                ui_Malla.Hide();
                BorrarRebarRectangulo.EjecutarElevaciones(_uiapp, "soloMalla");
                ui_Malla.Show();
            }


            UpdateGeneral.M2_CargarBArras(_uiapp);


        }




    }



}