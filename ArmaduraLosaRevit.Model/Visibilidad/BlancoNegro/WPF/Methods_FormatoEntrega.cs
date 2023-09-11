using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Linq;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ViewportnNH;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_FormatoEntrega
    {
        public static void M1_Ejecutar(UI_FormatoEntrega _ui, UIApplication _uiapp)
        {


            UtilStopWatch _utilStopWatch = new UtilStopWatch();
         

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            List<View> lista_view = new List<View>() { _uiapp.ActiveUIDocument.ActiveView };
            string tipoPosiicon = _ui.BotonOprimido;
            //  EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);
            if (tipoPosiicon == "Aceptar_estruc_Normal_estructura" || tipoPosiicon == "Aceptar_estruc_Normal_planta" || tipoPosiicon == "Aceptar_estruc_Normal_elev")
            {
                _ui.Hide();

                switch (tipoPosiicon)
                {
                    case "Aceptar_estruc_Normal_estructura":
                        lista_view = _ui.ListaEstructura.Where(c => c.IsSelected).Select(c => c.View_).OrderBy(c=>c.Name).ToList();
                        break;
                    case "Aceptar_estruc_Normal_planta":
                        lista_view = _ui.ListaLosa.Where(c => c.IsSelected).Select(c => c.View_).OrderBy(c => c.Name).ToList();
                        break;
                    case "Aceptar_estruc_Normal_elev":
                        lista_view = _ui.ListaElev.Where(c => c.IsSelected).Select(c => c.View_).OrderBy(c => c.Name).ToList();
                        break;
                    default:
                        break;
                }


                if (lista_view.Count == 0)
                {

                    var result = Util.InfoMsg_YesNoCancel("No se ha seleccionado ningun vista. Desea cambiar a formato 'BlacoNegro' la vista actual??");
                    if (result == System.Windows.Forms.DialogResult.Yes)
                        lista_view = new List<View>() { _uiapp.ActiveUIDocument.ActiveView };
                    else
                        lista_view.Clear();
                }

                if (lista_view.Count > 0)
                {
                    _utilStopWatch.IniciarMedicion();
               
                    ManejadorVisibilidadBlancoNegro _ManejadorVisibilidadBlancoNegro = new ManejadorVisibilidadBlancoNegro(_uiapp);
                    _ManejadorVisibilidadBlancoNegro.M1_EjecutarListaVista_ColorNormal(lista_view);
                    _utilStopWatch.Terminar();
                    Util.InfoMsg($"Ejecutar Color Normal finalizado    tiempo:{_utilStopWatch._listIntervalos.Last()}");
                }
                _ui.Show();
            }
           else if (tipoPosiicon == "Aceptar_estruc_ENTREGA_estructura" || tipoPosiicon == "Aceptar_estruc_ENTREGA_planta" || tipoPosiicon == "Aceptar_estruc_ENTREGA_elev")
            {
                _ui.Hide();

                switch (tipoPosiicon)
                {
                    case "Aceptar_estruc_ENTREGA_estructura":
                        lista_view = _ui.ListaEstructura.Where(c => c.IsSelected).Select(c => c.View_).ToList();
                        break;
                    case "Aceptar_estruc_ENTREGA_planta":
                        lista_view = _ui.ListaLosa.Where(c => c.IsSelected).Select(c => c.View_).ToList();
                        break;
                    case "Aceptar_estruc_ENTREGA_elev":
                        lista_view = _ui.ListaElev.Where(c => c.IsSelected).Select(c => c.View_).ToList();
                        break;
                    default:
                        break;
                }


                if (lista_view.Count == 0)
                {

                    var result = Util.InfoMsg_YesNoCancel("No se ha seleccionado ningun vista. Desea cambiar a formato 'BlacoNegro' la vista actual??");
                    if (result == System.Windows.Forms.DialogResult.Yes)
                        lista_view = new List<View>() { _uiapp.ActiveUIDocument.ActiveView };
                    else
                        lista_view.Clear();
                }

                if (lista_view.Count > 0)
                {
                    _utilStopWatch.IniciarMedicion();



                    ManejadorVisibilidadPorTipo ManejadorVisibilidadPorTip_o = new ManejadorVisibilidadPorTipo(_uiapp);
                    ManejadorVisibilidadPorTip_o.M1_EjecutarListaVista_POrtipo(lista_view);


                    _utilStopWatch.Terminar();




                    Util.InfoMsg($"Ejecutar Color Normal finalizado    tiempo:{_utilStopWatch._listIntervalos.Last()}");
                }
                _ui.Show();
            }
            
            else if (tipoPosiicon == "Aceptar_estruc_blancoNegro_estructura" || tipoPosiicon == "Aceptar_estruc_blancoNegro_planta" || tipoPosiicon == "Aceptar_estruc_blancoNegro_elev")
            {
                _ui.Hide();

                switch (tipoPosiicon)
                {
                    case "Aceptar_estruc_blancoNegro_estructura":
                        lista_view = _ui.ListaEstructura.Where(c => c.IsSelected).Select(c => c.View_).ToList();
                        break;
                    case "Aceptar_estruc_blancoNegro_planta":
                        lista_view = _ui.ListaLosa.Where(c => c.IsSelected).Select(c => c.View_).ToList();
                        break;
                    case "Aceptar_estruc_blancoNegro_elev":
                        lista_view = _ui.ListaElev.Where(c => c.IsSelected).Select(c => c.View_).ToList();
                        break;
                    default:
                        break;
                }

                if (lista_view.Count == 0)
                {

                    var result = Util.InfoMsg_YesNoCancel("No se ha seleccionado ningun vista. Desea cambiar a formato 'BlacoNegro' la vista actual??");
                    if (result == System.Windows.Forms.DialogResult.Yes)
                        lista_view = new List<View>() { _uiapp.ActiveUIDocument.ActiveView };
                    else
                        lista_view.Clear();
                }

                if (lista_view.Count > 0)
                {
                    _utilStopWatch.IniciarMedicion();
                    ManejadorVisibilidadBlancoNegro _ManejadorVisibilidadBlancoNegro = new ManejadorVisibilidadBlancoNegro(_uiapp);
                    _ManejadorVisibilidadBlancoNegro.M2_EjecutarListaVista_BlancoNegro(lista_view);
                    _utilStopWatch.Terminar();
                    Util.InfoMsg($"Ejecutar blanco negro finalizado    tiempo:{_utilStopWatch._listIntervalos.Last()}");
                }

                _ui.Show();
            }
           
            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }

    }
}