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
using System.Runtime.InteropServices.WindowsRuntime;
using ArmaduraLosaRevit.Model.RebarLosa;
using ArmaduraLosaRevit.Model.Stairsnh.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.EditarBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.WPFref
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_barraRefuerzo
    {
        public static bool IsBuscarTipo { get; private set; }

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




        public static void M1_EjecutarRutinas(Ui_barraRefuerzoLosa ui_barraRefuerzoLosa, UIApplication _uiapp)
        {
            string NombreBoton = ui_barraRefuerzoLosa.BotonOprimido;
            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;


            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            if (!VerificarDatos(ui_barraRefuerzoLosa)) return;

            try
            {
                ui_barraRefuerzoLosa.Hide();
                bool IsUsarDosPtos = (bool)ui_barraRefuerzoLosa.chbox_datos2ptos.IsChecked;
                if (NombreBoton == "btnAceptar_refCabMuro")
                {
                    DatosRefuerzoCabezaMuroDTO datosRefuerzoCabezaMuroDTO = new DatosRefuerzoCabezaMuroDTO()
                    {
                        CantidadBarras = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtCantidadBarras.Text),
                        diamtroBarraRefuerzo_MM = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtDiamRefuerzo.Text),
                        diamtroBarraS1_MM = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtDiaSUple.Text),
                        espacimientoS1_Cm = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.espaSuple.Text),
                        tipobarra = "s2",
                        _empotramientoPatasDTO = new EmpotramientoPatasLosaDTO()
                        {

                            TipoPataDere = (IsUsarDosPtos ? TipoBarraRefuerzo.NoBuscar : TipoBarraRefuerzo.BarraRefSinPatas),
                            TipoPataIzq = (IsUsarDosPtos ? TipoBarraRefuerzo.NoBuscar : TipoBarraRefuerzo.BarraRefSinPatas)
                        },
                        IsBArras = (bool)ui_barraRefuerzoLosa.chbox_datosbarras.IsChecked,
                        IsSuple = (bool)ui_barraRefuerzoLosa.chbox_datosSuples.IsChecked,
                        IsUsar2Pto = (bool)ui_barraRefuerzoLosa.chbox_datos2ptos.IsChecked,
                        _tipoRefuerzoLOSA=  (ui_barraRefuerzoLosa.combobox_tipolosa.Text.ToLower() == "losa" ? TipoRefuerzoLOSA.losa : TipoRefuerzoLOSA.fundacion)
                    };

                    ManejadorRefuerzoCabezaMuro ManejadorRefuerzoCabezaMuro = new ManejadorRefuerzoCabezaMuro(_uiapp, datosRefuerzoCabezaMuroDTO);
                    if (IsUsarDosPtos)
                        ManejadorRefuerzoCabezaMuro.EjecutarLibreConDosMOuse();
                    else
                        ManejadorRefuerzoCabezaMuro.Ejecutar();
                }
                else if (NombreBoton == "btnAceptar_refTipoViga")
                {

                    bool Aux_isbarra = (bool)ui_barraRefuerzoLosa.chbox_datosbarrasRefLosa.IsChecked;

                    DatosRefuerzoTipoVigaDTO datosRefuerzoTipoViga = new DatosRefuerzoTipoVigaDTO()
                    {
                        CantidadBarras = (Aux_isbarra ?Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtCantidadBarrasViga.Text):2),
                        diamtroBarraRefuerzo_MM = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtDiamRefuerzoVIga.Text),
                        diamtroEstribo_MM = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtDiaEstriboViga.Text),
                        espacimientoEstribo_Cm = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.espaEstriboViga.Text),
                        tipoPosicionRef = ui_barraRefuerzoLosa.dtTipoPosiRef.Text,
                        _empotramientoPatasDTO = new EmpotramientoPatasLosaDTO()
                        {
                            TipoPataDere = TipoBarraRefuerzo.buscar,
                            TipoPataIzq = TipoBarraRefuerzo.buscar
                        },
                        IsBArras = (bool)ui_barraRefuerzoLosa.chbox_datosbarrasRefLosa.IsChecked,
                        IsEstribo = (bool)ui_barraRefuerzoLosa.chbox_datosEstribo.IsChecked,
                        TipoBarra = TipoBarraRefuerzo.BarraRefSinPatas,
                        IsBuscarPatas = true,
                        _tipoRefuerzoLOSA = (ui_barraRefuerzoLosa.combobox_tipolosa.Text.ToLower()=="losa"? TipoRefuerzoLOSA.losa: TipoRefuerzoLOSA.fundacion)



                    };

                    bool isUsarLInea = (bool)ui_barraRefuerzoLosa.chbox_datosLinea.IsChecked;

                    if (isUsarLInea)
                    {
                        ManejadorRefuerzoTipoVigaModelLIne manejadorRefuerzoTipoViga = new ManejadorRefuerzoTipoVigaModelLIne(_uiapp, datosRefuerzoTipoViga);
                        manejadorRefuerzoTipoViga.Ejecutar();
                    }
                    else
                    {
                        ManejadorRefuerzoTipoViga manejadorRefuerzoTipoViga = new ManejadorRefuerzoTipoViga(_uiapp, datosRefuerzoTipoViga);

                        manejadorRefuerzoTipoViga.Ejecutar();
                    }

                }
                else if (NombreBoton == "barraSin" || NombreBoton == "barraInferiorSup" || NombreBoton == "barraSuperiorSUp" || NombreBoton == "barraAmbosSup" ||
                         NombreBoton == "barraInferiorInf" || NombreBoton == "barraSuperiorInf" || NombreBoton == "barraAmbosInf")
                {

                    try
                    {
                        EditarBarraDTO newEditarBarraDTO = ui_barraRefuerzoLosa.EditarBarraDTO(TipoCasobarra.BarraRefuerzoLosa);
                        ManejadorBarraRefuerzo_CambiarBarra ManejadorBarraH_CambiarBarra = new ManejadorBarraRefuerzo_CambiarBarra(_uiapp, newEditarBarraDTO);
                        ManejadorBarraH_CambiarBarra.CambiarFormaBarra();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Ex :" + ex.Message);
                    }

                }
                else if (NombreBoton == "btnAceptar_refTipoBorde")
                {
                    DatosRefuerzoTipoBorde datosRefuerzoTipoBorde = new DatosRefuerzoTipoBorde()
                    {
                        CantidadBarras = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtCantidadBarrasBorde.Text),
                        diamtroBarraRefuerzo_MM = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtDiamRefuerzoBorde.Text),
                        diamtroEstribo_MM = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtDiaEstriboBorde.Text),
                        espacimientoEstribo_Cm = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.espaEstriboBorde.Text),
                        _empotramientoPatasDTO = new EmpotramientoPatasLosaDTO()
                        {
                            TipoPataDere = TipoBarraRefuerzo.buscar,
                            TipoPataIzq = TipoBarraRefuerzo.buscar
                        },
                        IsEstribo = (bool)ui_barraRefuerzoLosa.chbox_datosEstriboBorde.IsChecked,

                        _tipoRefuerzoLOSA = (ui_barraRefuerzoLosa.combobox_tipolosa.Text == "Losa" ? TipoRefuerzoLOSA.losa : TipoRefuerzoLOSA.fundacion),
                        TipoSeleccionPtos = (ui_barraRefuerzoLosa.dtTipoSeleccionBorde.Text == "Borde" ? TipoSeleccionPtosBordeLosa.Borde : TipoSeleccionPtosBordeLosa.Selec2Puntos),
                        IsIntervalos = (ui_barraRefuerzoLosa.checkBox_intervalo.Text == "Normal" ? false : true)

                    };

                    ManejadorRefuerzoTipoBorde manejadorRefuerzoTipoBorde = new ManejadorRefuerzoTipoBorde(_uiapp, datosRefuerzoTipoBorde);
                    manejadorRefuerzoTipoBorde.Ejecutar();
                }
                else if (NombreBoton == "btnAceptar_Cuantia")
                {
                    bool isSoloCAmbiarDatosInternos = (bool)ui_barraRefuerzoLosa.chbox_datosCuantia.IsChecked;

                    int CantidadBarras = Util.ConvertirStringInInteger(ui_barraRefuerzoLosa.dtCantidadCuantia.Text);

                    ManejadorModificarTag manejadorRefuerzoTipoBorde = new ManejadorModificarTag(_uiapp, CantidadBarras);
                    if (isSoloCAmbiarDatosInternos)
                    { manejadorRefuerzoTipoBorde.EjecutarCambioSintag(); }
                    else
                    { manejadorRefuerzoTipoBorde.EjecutarCambioContag(); }



                }

                ui_barraRefuerzoLosa.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                ui_barraRefuerzoLosa.Show();
                ui_barraRefuerzoLosa.Close();
            }
            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }



        private static bool VerificarDatos(Ui_barraRefuerzoLosa ui_barraEscaleras)
        {

            if (!Util.IsNumeric(ui_barraEscaleras.dtDiamRefuerzo.Text))
            {
                Util.ErrorMsg("Error en Diamtro de barra de refuerzo");
                return false;
            }
            if (!Util.IsNumeric(ui_barraEscaleras.dtDiaSUple.Text))
            {
                Util.ErrorMsg("Error en Diamtro de siple");
                return false;
            }

            if (!Util.IsNumeric(ui_barraEscaleras.espaSuple.Text))
            {
                Util.ErrorMsg("Error en espaciamiento de suple");
                return false;
            }

            if (!Util.IsNumeric(ui_barraEscaleras.dtCantidadBarras.Text))
            {
                Util.ErrorMsg("Error en espaciamiento de cantidad de barras de refuerzo");
                return false;
            }

            return true;

        }

    }
}