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
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.EditarTipoPath.WPF;
using ArmaduraLosaRevit.Model.Fund.Editar.WPF;
using ArmaduraLosaRevit.Model.Fund.Editar;
using ArmaduraLosaRevit.Model.Fund.WPFfund.DTO;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.Fund.WPFfund
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_fund
    {
        private static DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales;
        private static CasosFundDTO _casosFundDTO;

        /// <summary>
        /// Method for collecting sheets as an asynchronous operation on another thread.
        /// </summary>
        /// <param name="doc">The Revit Document to collect sheets from.</param>
        /// <returns>A list of collected sheets, once the Task is resolved.</returns>
        private static async Task<List<ViewSheet>> GetSheets(Document doc)
        {
            return await Task.Run(() =>
            {
                ArmaduraLosaRevit.Model.Fund.Editar.WPF.UtilWPF.LogThreadInfo("Get Sheets Method");
                return new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .Select(p => (ViewSheet)p).ToList();
            });
        }




        public static void M1_EjecutarRutinas(Ui_barraFund ui_barraFund, UIApplication _uiapp)
        {
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);

            try
            {
                if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
                string tipoPosiicon = ui_barraFund.BotonOprimido;

                if (ui_barraFund.BotonOprimido == "btnCrearPath_Reactangular")
                {
                    if (!VerificarDatosAutomaticos(ui_barraFund)) return;

                    ui_barraFund.Hide();

                    _casosFundDTO.CasoTipoBArra = "Path";
                    FactoresLargoLeader.TipoFundacion = "Fundacion";
                    FundManejador _fundManejador = new FundManejador(_uiapp);
                    _fundManejador.executeAutomatico_fundacionCuadrada(_datosNuevaBarraDTOIniciales, _casosFundDTO);

                    ui_barraFund.Show();
                }
                else if (ui_barraFund.BotonOprimido == "btnCreaRebar_Reactangular")
                {
                    if (!VerificarDatosAutomaticos(ui_barraFund)) return;

                    ui_barraFund.Hide();
                    //_casosFundDTO.CasoTipoBArra = "Rebar";
                    //FundManejador _fundManejador = new FundManejador(_uiapp);
                    //_fundManejador.executeAutomaticoReBar(_datosNuevaBarraDTOIniciales, _casosFundDTO);

                    _casosFundDTO.CasoTipoBArra = "Rebar";
                    FundManejador _fundManejador = new FundManejador(_uiapp);
                    _datosNuevaBarraDTOIniciales.TipoBarra = "f11_fund"; //patasAmbos lados

                    _fundManejador.executeAutomatico_fundacionCuadrada(_datosNuevaBarraDTOIniciales, _casosFundDTO);
                    ui_barraFund.Show();
                }
                else if (ui_barraFund.BotonOprimido == "btnAceptar_Manual")
                {
                    if (!VerificarDatosManual(ui_barraFund)) return;
                    ui_barraFund.Hide();

                    FundManejador _fundManejador = new FundManejador(_uiapp);
                    _fundManejador.execute(_datosNuevaBarraDTOIniciales);

                    ui_barraFund.Show();
                }


                else if (ui_barraFund.BotonOprimido == "btnEditar_Manual")
                {

                   // if (!VerificarDatosAutomaticos(ui_barraFund)) return;
                    if (!VerificarDatosManual(ui_barraFund)) return;
                    ui_barraFund.Hide();

                    _casosFundDTO = new CasosFundDTO();
                    if (ui_barraFund.check_sup.IsChecked == true)
                    {
                        _casosFundDTO.SuperiorVertical = false;  // SIMPRE FALsae
                        _casosFundDTO.SuperiorHorizontal = true; // simpre true, pq al obtener las coordenadas de del path , entrega las orientacion correcta
                    }
                    else
                    {
                        _casosFundDTO.InferiorVertical = false;
                        _casosFundDTO.InferiorHorizontal = true;
                    }

                    //_datosNuevaBarraDTOIniciales.TipoPataFun = TipoPataFund.IzqInf;
                    _casosFundDTO.CasoTipoBArra = "Rebar";
                    FactoresLargoLeader.TipoFundacion = "Fundacion";
                    ManejadorEditarFundacionesRebar _ManejadorEditarFundacionesRebar = new ManejadorEditarFundacionesRebar(_uiapp);
                    _ManejadorEditarFundacionesRebar.EjecutarEdicion_PathToRebar(_datosNuevaBarraDTOIniciales, _casosFundDTO);
                    ui_barraFund.Show();
                    //CargarEditFundaciones _CargarEditFundaciones = new CargarEditFundaciones(_uiapp, TabEditarPath.Datos);
                    //_CargarEditFundaciones.Cargar();
                }
                else if (ui_barraFund.BotonOprimido == "btnBorrar_Manual")
                {
                    ui_barraFund.Hide();

                    SeleccionarPathReinfomentRectangulo administrador_ReferenciaRoom = new SeleccionarPathReinfomentRectangulo(_uiapp);
                    if (administrador_ReferenciaRoom.SeleccionadosMultiplesPathReinConRectaguloYFiltros())
                        administrador_ReferenciaRoom.BorrarPathReinfSeleccionado();

                    ui_barraFund.Show();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error en fundaciones:" + ex.Message);
                ui_barraFund.Show();
                ui_barraFund.Close();
            }
            UpdateGeneral.M2_CargarBArras(_uiapp);
        }

        private static bool VerificarDatosManual(Ui_barraFund ui_barraFund)
        {
            //    if (!ValidarDatos(ui_barraFund)) return false;

            TipoPataFund tipoBarras = TipoPataFund.Auto;
            if (ui_barraFund.cbx_tiposeleccion.Text == "Auto")
                tipoBarras = TipoPataFund.Auto;
            else if (ui_barraFund.cbx_tiposeleccion.Text == "Izquierda")
                tipoBarras = TipoPataFund.IzqInf;
            else if (ui_barraFund.cbx_tiposeleccion.Text == "Derecha")
                tipoBarras = TipoPataFund.DereSup;
            else if (ui_barraFund.cbx_tiposeleccion.Text == "Ambos")
                tipoBarras = TipoPataFund.Ambos;
            else if (ui_barraFund.cbx_tiposeleccion.Text == "Sin")
                tipoBarras = TipoPataFund.Sin;

            _datosNuevaBarraDTOIniciales = new DatosNuevaBarraDTO()
            {
                DiametroMM = Util.ConvertirStringInDouble(ui_barraFund.dtDiaLong.Text),
                EspaciamientoFoot = Util.CmToFoot(Util.ConvertirStringInDouble(ui_barraFund.espalong.Text)),
                TipoCaraObjeto_ = (ui_barraFund.check_sup.IsChecked == true ? TipoCaraObjeto.Superior : TipoCaraObjeto.Inferior),
                Iscaso_Intervalo = (ui_barraFund.checkBox_selec.Text == "Intervalo" ? true : false),

                TipoPataFun = tipoBarras,
                Tiposeleccion = ui_barraFund.checkBox_selec.Text,// "mouse" ,"Intervalo",
                TiposElemento = ui_barraFund.checkBox_Elemento.Text,//"Fundacion","Losa"
                TipoDeDIreccionBarra = (ui_barraFund.cbx_tiposeleccion.Text == "Auto" ? "none" : (ui_barraFund.comboboxborde.Text.ToLower()))
            };

            return true;
        }

        private static bool VerificarDatosAutomaticos(Ui_barraFund ui_barraFund)
        {

            _casosFundDTO = new CasosFundDTO();


            if ((bool)ui_barraFund.check_supAuto.IsChecked)
            {
                _casosFundDTO.SuperiorHorizontal = (bool)ui_barraFund.chbox_H_cuadra.IsChecked;
                _casosFundDTO.SuperiorVertical = (bool)ui_barraFund.chbox_V_cuadra.IsChecked;
            }
            else
            {
                _casosFundDTO.InferiorHorizontal = (bool)ui_barraFund.chbox_H_cuadra.IsChecked;
                _casosFundDTO.InferiorVertical = (bool)ui_barraFund.chbox_V_cuadra.IsChecked;
            }

          //  if (!ValidarDatos(ui_barraFund)) return false;




            _datosNuevaBarraDTOIniciales = new DatosNuevaBarraDTO()
            {
                DiametroMM_fundH = Util.ConvertirStringInDouble(ui_barraFund.dtDiaLong_HAUto.Text),
                Espaciamiento_fundH_Foot = Util.CmToFoot(Util.ConvertirStringInDouble(ui_barraFund.espalong_HAUto.Text)),

                DiametroMM_fundV = Util.ConvertirStringInDouble(ui_barraFund.dtDiaLong_VAUto.Text),
                Espaciamiento_fundV_Foot = Util.CmToFoot(Util.ConvertirStringInDouble(ui_barraFund.espalong_VAUto.Text)),
                TipoPataFun= TipoPataFund.Ambos,
                TipoCaraObjeto_ = (ui_barraFund.check_sup.IsChecked==true ? TipoCaraObjeto.Superior: TipoCaraObjeto.Inferior),
                TiposElemento = ui_barraFund.checkBox_Elemento.Text,//"Fundacion","Losa"
            };


            return true;
        }

        //private static bool ValidarDatos(Ui_barraFund ui_barraFund)
        //{
        //    if (!Util.IsNumeric(ui_barraFund.dtDiaLong.Text))
        //    {
        //        Util.ErrorMsg("Diamtro longitudinal no es dato nomerico");
        //        return false;
        //    }

        //    if (!Util.IsNumeric(ui_barraFund.espalong.Text))
        //    {
        //        Util.ErrorMsg("Espaciamiento longitudinal no es dato nomerico");
        //        return false;
        //    }

        //    double largolong = Util.ConvertirStringInDouble(ui_barraFund.espalong.Text);
        //    if (!(largolong >= 7.5 && largolong < 30))
        //    {
        //        Util.ErrorMsg("Espaciamiento longitudinal debe estar entre 7.5 y 30cm");
        //        return false;
        //    }
        //    return true;
        //}
    }
}