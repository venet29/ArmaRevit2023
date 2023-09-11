using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.WPF;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Borrar;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraV.Agrupar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.BarraV.WPFb
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_CrearBarra
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




        public static void M1_EjecutarRutinas(Ui_BarraV ui_Barrav, UIApplication _uiapp)
        {
            string tipoPosiicon = ui_Barrav.BotonOprimido;

            //Stopwatch timeMeasure = Stopwatch.StartNew();
            //ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            //// _ManejadorUsuarios.PostInscripcion("NHdelporte");
            //Task.Run(async () => await _ManejadorUsuarios.PostInscripcion("NHdelporte")).Wait();        
            //Debug.WriteLine($" tiempo {timeMeasure.ElapsedMilliseconds } ms");
            
           // //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;

                if (tipoPosiicon == "btnCrearBarra")
                {
                       
                    CrearBarrasVerticales(ui_Barrav, _uiapp);

                }
                else if (tipoPosiicon == "btnCrearBarraH")
                {
                    CrearBarrasHorizontal(ui_Barrav, _uiapp);
                }
                else if (tipoPosiicon == "barraSinV" || tipoPosiicon == "barraInferiorV" || tipoPosiicon == "barraSuperiorV" || tipoPosiicon == "barraAmbosV")
                {
                    CambiarBarrasVertical(ui_Barrav, _uiapp, TipoCasobarra.BarraVertical);

                }
                else if (tipoPosiicon == "barraSinH" || tipoPosiicon == "barraInferiorH" || tipoPosiicon == "barraSuperiorH" || tipoPosiicon == "barraAmbosH")
                {
                    CambiarBarrasHorizontal(ui_Barrav, _uiapp, TipoCasobarra.BarrasHorizontal);

                }
                else if (tipoPosiicon == "btnBorrarRebar" || tipoPosiicon == "btnBorrarRebar2")
                {
                    ui_Barrav.Hide();
                    BorrarRebarRectangulo.EjecutarElevaciones(_uiapp, "soloBarras");

                    ui_Barrav.Show();
                }
                
                else if (tipoPosiicon == "btnAgruparHRebar" )
                {
                    ui_Barrav.Hide();


                 

                    ManejadorAgrupar ManejadorAgrupar = new ManejadorAgrupar(_uiapp);
                    ManejadorAgrupar.M2_EjecutarHorizontal();


                    ui_Barrav.Show();
                }
                else if (tipoPosiicon == "TipovigaRefuerzoInferior")
                {
                    ui_Barrav.Hide();

                    ConfiguracionInicialBarraHorizontalDTO confiEnfierrado_BArrasInferiorDTO = ui_Barrav.Obtener_ManejadoRefuerzoVigaCentral80_YRefuerzoEntreVigas();

                    ManejadoRefuerzoVigaCentral80 ManejadorBarraH2 = new ManejadoRefuerzoVigaCentral80(_uiapp, confiEnfierrado_BArrasInferiorDTO);
                    ManejadorBarraH2.CrearBArraHorizontal();

                    ui_Barrav.Show();
                }
                else if (tipoPosiicon == "TipovigaRefuerzoEntreVigas")
                {
                    ui_Barrav.Hide();

                    ConfiguracionInicialBarraHorizontalDTO confiEnfierrado_BArrasInferiorDTO = ui_Barrav.Obtener_ManejadoRefuerzoVigaCentral80_YRefuerzoEntreVigas();
                    ManejadoRefuerzoEntreVigas ManejadorBarraH2 = new ManejadoRefuerzoEntreVigas(_uiapp, confiEnfierrado_BArrasInferiorDTO);
                    ManejadorBarraH2.CrearBArraHorizontal();

                    ui_Barrav.Show();
                }
                else if (tipoPosiicon == "TipovigaRefuerzoBordeViga")
                {
                    ui_Barrav.Hide();

                    ConfiguracionInicialBarraHorizontalDTO confiEnfierrado_BArrasInferiorDTO = ui_Barrav.Obtener_ManejadoRefuerzoVigaCentral80_YRefuerzoEntreVigas();
                    confiEnfierrado_BArrasInferiorDTO.inicial_tipoBarraH = TipoPataBarra.buscar;
                    ManejadoRefuerzoVigaBorde _manejadorBarraH2 = new ManejadoRefuerzoVigaBorde(_uiapp, confiEnfierrado_BArrasInferiorDTO);
                    _manejadorBarraH2.CrearBArraHorizontal();

                    ui_Barrav.Show();
                }

                else if (tipoPosiicon == "btnCerrar_e")
                {
                    ui_Barrav.Close();
                }

            }
            catch (Exception)
            {
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            UpdateGeneral.M2_CargarBArras(_uiapp);
        }

        private static void CambiarBarrasVertical(Ui_BarraV ui_Barrav, UIApplication uiapp, TipoCasobarra TipoCasobarra)
        {
            ui_Barrav.Hide();
            try
            {
                EditarBarraDTO newEditarBarraDTO = ui_Barrav.EditarBarraDTO(TipoCasobarra);
                ManejadorBarraV_CambiarBarra ManejadorBarraV_CambiarBarra = new ManejadorBarraV_CambiarBarra(uiapp, newEditarBarraDTO);
                ManejadorBarraV_CambiarBarra.CambiarFormaBarra();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ex :" + ex.Message);
            }
            ui_Barrav.Show();
        }
        private static void CambiarBarrasHorizontal(Ui_BarraV ui_Barrav, UIApplication uiapp, TipoCasobarra TipoCasobarra)
        {
            ui_Barrav.Hide();
            try
            {
                EditarBarraDTO newEditarBarraDTO = ui_Barrav.EditarBarraDTO(TipoCasobarra);
                ManejadorBarraH_CambiarBarra ManejadorBarraH_CambiarBarra = new ManejadorBarraH_CambiarBarra(uiapp, newEditarBarraDTO);
                ManejadorBarraH_CambiarBarra.CambiarFormaBarra();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ex :" + ex.Message);
            }
            ui_Barrav.Show();
        }
        private static void CrearBarrasVerticales(Ui_BarraV ui_Barrav, UIApplication _uiapp)
        {
            ui_Barrav.Hide();
            try
            {
             
                // ISelectionFilter filtroMuro = SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO = ui_Barrav.ObtenerConfiguracionInicialBarraVerticalVDTO();

                //configuracion barra verticales cabeza muro
                DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(_uiapp.ActiveUIDocument.ActiveView, DireccionRecorrido_.PerpendicularEntradoVista);

                //configuracion barra verticales malla
                //DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(uiapp.ActiveUIDocument.ActiveView, DireccionRecorrido_.ParaleloDerechaVista, 100);


                ManejadorBarraV ManejadorBarraV = new ManejadorBarraV(_uiapp, _seleccionarNivel, confiEnfierradoDTO, _DireccionRecorrido);
                //ManejadorBarraV ManejadorBarraV = new ManejadorBarraV(uiapp, _seleccionarNivel, confiEnfierradoDTO);
                ManejadorBarraV.CrearBArraVErtical();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ex :" + ex.Message);
            }
            ui_Barrav.Show();
        }

        private static void CrearBarrasHorizontal(Ui_BarraV ui_Barrav, UIApplication uiapp)
        {
            ui_Barrav.Hide();
            try
            {
                ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(uiapp);
                ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO = ui_Barrav.ObtenerConfiguracionInicialBarraHorizontalVDTO();

                ManejadorBarraH ManejadorBarraH = new ManejadorBarraH(uiapp, _seleccionarNivel, confiEnfierradoDTO);
                ManejadorBarraH.CrearBArraHorizontalTramo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ex :" + ex.Message);
            }
            ui_Barrav.Show();
        }
    }
}