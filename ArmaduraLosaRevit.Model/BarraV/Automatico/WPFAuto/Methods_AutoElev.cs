using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using formNH = System.Windows.Forms;
using System.IO;
using ArmaduraLosaRevit.Model.BarraV.Automatico.WPF;
using ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model;
using ArmaduraLosaRevit.Model.BarraV.Automatico.Vigas;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ConfiguracionInicial.Elemento;
using ArmaduraLosaRevit.Model.GruposNh;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto
{

    public class VariasElevAutoDTO
    {
        public string NombreView { get; set; }
        public FileInfo NombreFileInfo { get; set; }
        public View ViewSection { get; internal set; }

        public VariasElevAutoDTO(View viewElev, FileInfo archJsonSeleccion)
        {
            this.ViewSection = viewElev;
            this.NombreView = viewElev.Name;
            this.NombreFileInfo = archJsonSeleccion;
        }



    }
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_AutoElev
    {
        public static string mpathDirecotrio { get; private set; }

        public static void M1_EjecutarRutinas(Ui_AutoElev ui_Barrav, UIApplication _uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;

            Document _doc = _uiapp.ActiveUIDocument.Document;
            string tipoPosiicon = ui_Barrav.BotonOprimido;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            if (tipoPosiicon == "Ejecutar")
            {
                ui_Barrav.Hide();
                try
                {
                    ManejadorPreAnalisisELemento _ManejadorPreAnalisisELemento = new ManejadorPreAnalisisELemento(_uiapp);
                    _ManejadorPreAnalisisELemento.AnalisarMuros();

                    ManejadorBarrasAutomaticas manejadorBarrasAutomaticas = new ManejadorBarrasAutomaticas(_uiapp, ref ui_Barrav);
                    manejadorBarrasAutomaticas.EjecutarImportacion();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ex :" + ex.Message);
                }
                ui_Barrav.Show();
            }
            else if (tipoPosiicon == "EjecutarH")
            {
                ui_Barrav.Hide();

                if (ui_Barrav.cbx_diseñoBarras.Text == "Desplazar")
                {
                    // ui_Barrav.cbx_diseñoBarras.Text = "Dibujar";
                    VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras = false;

                }
                else
                    VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras = true;
                try
                {
                    // VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras = true;
                    ManejadorVigasAuto _ManejadorVigasAuto = new ManejadorVigasAuto(_uiapp, ref ui_Barrav);
                    _ManejadorVigasAuto.ImportarBarrasViga();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ex :" + ex.Message);
                }
                ui_Barrav.Show();
            }
            else if (tipoPosiicon == "Mostrar_Level")
            {
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _doc.ActiveView);
                ArmaduraLosaRevit.Model.Visibilidad.ManejadorVisibilidad ManejadorVisibilidad = new ArmaduraLosaRevit.Model.Visibilidad.ManejadorVisibilidad(_uiapp, seleccionarRebar);
                ManejadorVisibilidad.M8_DesOcultarBarrasVigaIdem();
            }
            else if (tipoPosiicon == "Ocultar_Level")
            {

                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _doc.ActiveView);
                ArmaduraLosaRevit.Model.Visibilidad.ManejadorVisibilidad ManejadorVisibilidad = new ArmaduraLosaRevit.Model.Visibilidad.ManejadorVisibilidad(_uiapp, seleccionarRebar);
                ManejadorVisibilidad.M7_OcultarBarrasVigaIdem();

            }
            else if (tipoPosiicon == "Ejecutar_variasM")
            {
                ui_Barrav.Hide();
                try
                {
                    List<FileInfo> ListaRutasArchivos = ObtenerListaArchivos();

                    if (ListaRutasArchivos.Count != 0 || true)
                    {
                        List<ElevacionVAriosDto> ListaElevacionVAriosDto = ObtenerListaElevacionVAriosDto.Ejecutar(_doc, ui_Barrav.RemoverView, ui_Barrav.RemoverJson, ListaRutasArchivos);

                        //cargar wpf de json
                        ManejadorWPF_VariasElevAuto _ManejadorWPF_Cub = new ManejadorWPF_VariasElevAuto(_uiapp, ListaElevacionVAriosDto, ui_Barrav, mpathDirecotrio);
                        _ManejadorWPF_Cub.Execute();


                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ex :" + ex.Message);
                }
                ui_Barrav.Show();
            }
            else if (tipoPosiicon == "DesAgrupar")
            {
                ManejadorGrupos _ManejadorGrupos = new ManejadorGrupos(_uiapp);
                bool resul = true;
                _ManejadorGrupos.BuscarGrupos();
                _ManejadorGrupos.DesHaceGrupos();

            }
            else if (tipoPosiicon == "Agrupar")
            {
                ManejadorGrupos _ManejadorGrupos = new ManejadorGrupos(_uiapp);
                _ManejadorGrupos.CrearGrupos(true);

            }
            else if (tipoPosiicon == "Cerrar")
            {
                ui_Barrav.Close();
            }
            UpdateGeneral.M2_CargarBArras(_uiapp);
        }

        private static List<FileInfo> ObtenerListaArchivos()
        {
            List<FileInfo> result = new List<FileInfo>();

            formNH.FolderBrowserDialog folderBrowserDialog1 = new formNH.FolderBrowserDialog();
            folderBrowserDialog1.Description = "Seleccionar carpeta";

            string rutaInicial = @"\\SERVER-CDV\Proyectos_automaticos\2022";
            if (!Directory.Exists(rutaInicial)) rutaInicial = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            folderBrowserDialog1.SelectedPath = rutaInicial;


            if (folderBrowserDialog1.ShowDialog() == formNH.DialogResult.OK)
            {
                mpathDirecotrio = folderBrowserDialog1.SelectedPath;

                DirectoryInfo di = new DirectoryInfo(mpathDirecotrio);
                Console.WriteLine("No search pattern returns:");
                foreach (var fi in di.GetFiles().Where(c => c.Extension == ".json"))
                {
                    Console.WriteLine(fi.Name);
                }

                result = di.GetFiles().Where(c => c.Extension == ".json").ToList();
            }

            return result;
        }

    }




}