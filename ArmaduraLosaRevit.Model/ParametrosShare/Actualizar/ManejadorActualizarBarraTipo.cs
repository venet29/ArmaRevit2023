
using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar.Traba;
using ArmaduraLosaRevit.Model.ParametrosShare.Actualizar.Model;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ParametosShare.Actualizar
{
    public class ManejadorActualizarBarraTipo
    {
        private UIApplication _uiapp;
        private Document _doc;
        private List<Element> listaTotal;

        public ManejadorActualizarBarraTipo(UIApplication _uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
        }
        #region Actualizar BARRA TIPO

        public void Ejecutar_enVistaActual()
        {
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            string nombreView = _view.Name;
            try
            {
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                seleccionarRebar.BuscarListaRebarEnVistaActual();
                var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual.Select(c => c.element).ToList();

                ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, _view);
                _ActualizarBarraTipo.Ejecutar(listaRebar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");

        }

        //solo saul
        public void Ejecutar_enVistaActual_tarba7_5()
        {
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            string nombreView = _view.Name;
            try
            {
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                seleccionarRebar.BuscarListaRebarEnVistaActual();
                var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual
                    .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO_T)
                    .Select(c => c.element).ToList();

                ActualizarNUmeroTraba7_5 _ActualizarBarraTipo = new ActualizarNUmeroTraba7_5(_uiapp, _view);
                _ActualizarBarraTipo.Ejecutar(listaRebar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");

        }


        //  actualiza parametro de trababa 'cantidadEstriboTraba'  :+5TR.Ø8a10  -->  +5TR.
        public void Ejecutar_enVistaActual_FOrmatoTraba(View _view)
        {
            _uiapp.ActiveUIDocument.ActiveView = _view;
            string nombreView = _view.Name;
            try
            {
     


                //**trabas
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                seleccionarRebar.BuscarListaRebarEnVistaActual();
                var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual
                    .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO_T || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_T)
                    .Select(c => c.element).ToList();

                Actualizar_formatoPArametroTrabas _ActualizarBarraTipo = new Actualizar_formatoPArametroTrabas(_uiapp, _view);
                _ActualizarBarraTipo.Ejecutar_Trabas(listaRebar);

                //***laterales
                var listaRebarLAterales = seleccionarRebar._lista_A_DeRebarVistaActual
                    .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_L || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VL)
                    .Select(c => c.element).ToList();

                //Actualizar_formatoPArametroTrabas _ActualizarBarraTipo_Laterales = new Actualizar_formatoPArametroTrabas(_uiapp, _view);
                _ActualizarBarraTipo.EjecutarLAterales(listaRebarLAterales);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        public void CargarFamiliar()
        {
            // RECARGAR LAS FAMILIAS
            List<string> NombreFamilia = new List<string>() { "M_Structural MRA Rebar_PILARL", "M_Structural MRA Rebar_PILART",
                                                                    "M_Structural MRA Rebar_VIGAL","M_Structural MRA Rebar_VIGAT" };
            ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
            _ManejadorCargarFAmilias.cargarFamilias_COnLista(NombreFamilia, true);
        }

        public void Ejecutar_CambirSOLOTipoBrrar(string antiguoNombreParametroCompartido, string nuevoNombreParametroCompartido,
                                                List<CambirSOLOTipoBarraDTO> ListaParamtrosCambiar, SeleccionarRebarElemento seleccionarRebar )
        {

            // Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(_uiapp);
            Manejador_UpdateRebar.DesCargarUpdateREbar(_uiapp);

            View _view = _uiapp.ActiveUIDocument.ActiveView;
            string nombreView = _view.Name;
            try
            {

                for (int i = 0; i < ListaParamtrosCambiar.Count; i++)
                {
                    var item = ListaParamtrosCambiar[i];
                    listaTotal = new List<Element>();

                    //a) rebar
                    var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual
                                                     .Where(r => item.ContinuarSiesIgual.Contains(r.ObtenerTipoBarra.TipoBarra_.ToString()))
                                                     .Select(c => c.element).ToList();
                    listaTotal.AddRange(listaRebar);

                    //var listat = seleccionarRebar._lista_A_DePathReinfVistaActual.Where(c => c.element.Id.IntegerValue == 2850825).ToList();

                    //b) path
                    var listaPAthReinf = seleccionarRebar._lista_A_DePathReinfVistaActual
                                                          .Where(r => item.ContinuarSiesIgual.Contains(r.ObtenerTipoBarra?.TipoBarra_.ToString()))
                                                          .Select(c => c.element).ToList();
                    listaTotal.AddRange(listaPAthReinf);

                    if (listaTotal.Count == 0) return;
                    //******
                    ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, _view);
                    _ActualizarBarraTipo
                        .EjecutarSinTrasn_actulizarBarraTipo_intercambiar(listaTotal, antiguoNombreParametroCompartido, nuevoNombreParametroCompartido, item);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            Manejador_UpdateRebar.CargarUpdateREbar(_uiapp);
        }




        public void Ejecutar_CambirNombreVista(string antiguoNombreParametroCompartido, string nuevoNombreParametroCompartido)
        {

            // Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(_uiapp);
            Manejador_UpdateRebar.DesCargarUpdateREbar(_uiapp);

            View _view = _uiapp.ActiveUIDocument.ActiveView;
            string nombreView = _view.Name;
            try
            {
                listaTotal = new List<Element>();
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                seleccionarRebar.BuscarListaRebarEnVistaActual();
                var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual.Select(c => c.element).ToList();
                listaTotal.AddRange(listaRebar);


                seleccionarRebar.BuscarListaPathReinformetEnVistaActual();
                var listaPAthReinf = seleccionarRebar._lista_A_DePathReinfVistaActual.Select(c => c.element).ToList();
                listaTotal.AddRange(listaPAthReinf);

                ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, _view);
                _ActualizarBarraTipo.EjecutarSinTrasn_actulizarBarraTipo_intercambiar(listaTotal, antiguoNombreParametroCompartido, nuevoNombreParametroCompartido);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");
            Manejador_UpdateRebar.CargarUpdateREbar(_uiapp);
        }

        public void Ejecutar_CAmbirBarraTipoNormal()
        {
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            if (_view == null)
            {
                Util.ErrorMsg("Vista actual igual Null");
                return;
            }
            View3D view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            if (view3D_Visualizar == null)
            {
                Util.ErrorMsg($"Vista3D '{ConstNH.CONST_NOMBRE_3D_VISUALIZAR}' igual Null");
                return;
            }


            if (_view.Name != view3D_Visualizar.Name)
            {
                Util.ErrorMsg($"Comando se debe ejecutar en vista {view3D_Visualizar.Name}");
                return;
            }

            try
            {
                listaTotal = new List<Element>();
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, view3D_Visualizar);
                seleccionarRebar.BuscarListaRebarEnVistaActual();
                var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual.Select(c => c.element).ToList();
                listaTotal.AddRange(listaRebar);


                seleccionarRebar.BuscarListaPathReinformetEnVistaActual();
                var listaPAthReinf = seleccionarRebar._lista_A_DePathReinfVistaActual.Select(c => c.element).ToList();
                listaTotal.AddRange(listaPAthReinf);

                ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, view3D_Visualizar);
                _ActualizarBarraTipo.EjecutarSinTrasn_actulizarBarraTipo_intercambiar(listaTotal, "NombreVista2", "NombreVista");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");

        }



        public void EjecutarTodoVIewSection_actulizarBarraTipo()
        {
            SeleccionarView _SeleccionarView = new SeleccionarView();
            List<ViewSection> list = _SeleccionarView.ObtenerTodosViewSection(_uiapp.ActiveUIDocument.Document);

            try
            {
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarBarraTipo-NH");

                    foreach (ViewSection _view in list)
                    {
                        if (VistaNoEscorrecta(_view.Name)) continue;
                        SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                        seleccionarRebar.BuscarListaRebarEnVistaActual();
                        var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual.Select(c => c.element).ToList();
                        if (listaRebar.Count == 0) continue;
                        ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, _view);
                        _ActualizarBarraTipo.EjecutarSinTrasn_actulizarBarraTipoPorCOlor(listaRebar); ;
                    }
                    trans2.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. {list.Count} ViewSection analizadas ");

        }

        #endregion


        public void EjecutarViewActual_GenerarID_CCorrelativo()
        {
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            string nombreView = _view.Name;
            try
            {
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                seleccionarRebar.BuscarListaRebarEnVistaActual();
                var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual.Select(c => c.element).ToList();

                seleccionarRebar.BuscarListaPathReinformetEnVistaActual();
                //    listaRebar.AddRange(seleccionarRebar._lista_A_DePathReinfVistaActual.Select(x => x.element).ToList());
                listaRebar.AddRange(seleccionarRebar._lista_A_DePathReinfVistaActual.SelectMany(x => x.ListaRebarInsistem).Select(f => _doc.GetElement(f)).ToList());

                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("GenerarID_Correlativo-NH");
                    ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, _view);
                    _ActualizarBarraTipo.EjecuEjecutarSinTrasn_GenerarId_Correlativo(listaRebar);
                    trans2.Commit();
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");
        }


        #region Actualizat nombre de vista







        public void Ejecuta_Seleccionar_actulizarNOmbreVista(View _view)
        {

            // SeleccionarView _SeleccionarView = new SeleccionarView();
            //  List<ViewSection> list = _SeleccionarView.ObtenerTodosViewSection(_uiapp.ActiveUIDocument.Document);
            try
            {
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarBarraTipoSeleccion-NH");

                    SeleccionarRebarOPathRein _seleccionarRebarOPathRein = new SeleccionarRebarOPathRein(_uiapp);
                    _seleccionarRebarOPathRein.GetListaRebarPathRein();
                    if (_seleccionarRebarOPathRein.ListaBarras.Count == 0) return;
                    ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, _view);
                    _ActualizarBarraTipo.EjecutarSinTrasn_actulizarNOmbreVista(_seleccionarRebarOPathRein.ListaBarras); ;

                    trans2.Commit();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            //   Util.InfoMsg($"Proceso Terminado. {list.Count} ViewSection analizadas ");

        }

        public void Ejecutar1VIewSection_actulizarNOmbreVista(View _view)
        {

            SeleccionarView _SeleccionarView = new SeleccionarView();
            List<ViewSection> list = _SeleccionarView.ObtenerTodosViewSection(_uiapp.ActiveUIDocument.Document);
            try
            {
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarBarraTipo-NH");

                    if (VistaNoEscorrecta(_view.Name)) return;
                    SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                    seleccionarRebar.BuscarListaRebarEnVistaActual();
                    var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual.Select(c => c.element).ToList();
                    if (listaRebar.Count == 0) return;
                    ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, _view);
                    _ActualizarBarraTipo.EjecutarSinTrasn_actulizarNOmbreVista(listaRebar); ;

                    trans2.Commit();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            Util.InfoMsg($"Proceso Terminado. {list.Count} ViewSection analizadas ");

        }


        public void Ejecutar1VIewSection_actulizarNOmbreVistaTipoBArra_PAthreinf()
        {

            View3D view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            if (view3D_Visualizar == null)
            {
                Util.ErrorMsg($"Vista3D '{ConstNH.CONST_NOMBRE_3D_VISUALIZAR}' igual Null");
                return;
            }

            SeleccionarView _SeleccionarView = new SeleccionarView();
            List<ViewSection> list = _SeleccionarView.ObtenerTodosViewSection(_uiapp.ActiveUIDocument.Document);
            try
            {
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarNOmbreVistaTipoBArra_PAthreinf-NH");

                    listaTotal = new List<Element>();

                    if (VistaNoEscorrecta(view3D_Visualizar.Name)) return;
                    SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, view3D_Visualizar);

                    seleccionarRebar.BuscarListaPathReinformetEnVistaActual();
                    var listaPAthReinf = seleccionarRebar._lista_A_DePathReinfVistaActual.Select(c => c.element).ToList();
                    listaTotal.AddRange(listaPAthReinf);

                    ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, view3D_Visualizar);
                    _ActualizarBarraTipo.ActualizarNOmbreVistaTipoBArra_PAthreinf(listaTotal);

                    trans2.Commit();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. {list.Count} ViewSection analizadas ");

        }




        public void EjecutarTodoVIewSection_actulizarNOmbreVista()
        {

            SeleccionarView _SeleccionarView = new SeleccionarView();
            List<ViewSection> list = _SeleccionarView.ObtenerTodosViewSection(_uiapp.ActiveUIDocument.Document);
            try
            {
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarBarraTipo-NH");
                    foreach (ViewSection _view in list)
                    {
                        if (VistaNoEscorrecta(_view.Name)) continue;
                        SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                        seleccionarRebar.BuscarListaRebarEnVistaActual();
                        var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual.Select(c => c.element).ToList();
                        if (listaRebar.Count == 0) continue;
                        ActualizarBarraTipo _ActualizarBarraTipo = new ActualizarBarraTipo(_uiapp, _view);
                        _ActualizarBarraTipo.EjecutarSinTrasn_actulizarNOmbreVista(listaRebar); ;
                    }
                    trans2.Commit();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            Util.InfoMsg($"Proceso Terminado. {list.Count} ViewSection analizadas ");

        }

        private bool VistaNoEscorrecta(string nombre)
        {
            switch (nombre)
            {
                case "SUR":
                    return true;

                case "NORTE":

                    return true;
                default:
                    return false;
            }

        }
        #endregion
    }
}
