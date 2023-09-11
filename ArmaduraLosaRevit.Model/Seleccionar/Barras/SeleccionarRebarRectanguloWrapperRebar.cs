
using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.UTILES.Buscar;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    public class SeleccionarRebarRectanguloWrapperRebar
    {

        protected UIDocument _uidoc;
        protected Document _doc;

        private IList<Element> _listaElementsRebarSeleccionado { get; set; }
        public List<WrapperRebar> _listaWrapperRebar { get; set; }
        public List<ElementId> _listaDimenedionesId { get; set; }
        public List<WrapperRebar> ListaWrapperRebarFiltro { get; set; }

        protected UIApplication _uiapp;
        protected TipoBarraGeneral tipocaso;
        protected readonly string casoEspecifico;

        public SeleccionarRebarRectanguloWrapperRebar(UIApplication _uiapp, TipoBarraGeneral tipocaso, string casoEspecifico = "todos")
        {
            this._uiapp = _uiapp;
            this.tipocaso = tipocaso;
            this.casoEspecifico = casoEspecifico;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uiapp.ActiveUIDocument.Document;
            _listaWrapperRebar = new List<WrapperRebar>();
            ListaWrapperRebarFiltro = new List<WrapperRebar>();
            _listaDimenedionesId = new List<ElementId>();

        }

        public bool BorrarBarras()
        {

            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            bool result = true;
            try
            {
                if (!M1_GetRoomSeleccionadosConRectaguloYFiltros())
                {
                    UpdateGeneral.M2_CargarBArras(_uiapp);
                    return false;
                }


                if (M2_ObtenerListaWrapperRebar())
                {
                    //UpdateGeneral.M2_CargarBArras(_uiapp);

                    if (tipocaso == TipoBarraGeneral.Elevacion)
                        M3_b_ObtenerListaIDSeleccionadosElevaciones();
                    else if (tipocaso == TipoBarraGeneral.Losa)
                        M3_a_ObtenerListaIDSeleccionadosRefuerzoSUperior();
                    else if (tipocaso == TipoBarraGeneral.Fundaciones)
                        M3_c_ObtenerListaIDSeleccionadosFundaciones();
                    else if (tipocaso == TipoBarraGeneral.Escalera)
                        M3_d_ObtenerListaIDSeleccionadosEscalera();
                }

                _M1_borrarDImensiones();

                M4_BorrarRebarSeleccionadoWrapperRebar();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"BOrrar ex:{ex.Message}");
                result = false;
            }
            UpdateGeneral.M2_CargarBArras(_uiapp);
            return result;
        }

        protected void _M1_borrarDImensiones()
        {
            try
            {
                _listaDimenedionesId = _listaElementsRebarSeleccionado.Where(c => ObtenerLineaPorNombreTipo.IsLineaPorNombre(_doc, c, "LineaDimension"))
                                                                      .Select(c => c.Id).ToList();
                //borrar a la selccion inicial los dimensiones
                _listaElementsRebarSeleccionado = _listaElementsRebarSeleccionado.Where(c => c.Category.Name != "Lines").ToList();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.Message);
            }
        }

        public bool M1_GetRoomSeleccionadosConRectaguloYFiltros()
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new RebarDimenionesSelectionFilter();
                //selecciona un objeto floor
                _listaElementsRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar barra (Rebar) para borrar");
            }
            catch (Exception)
            {
                _listaElementsRebarSeleccionado = new List<Element>();
                return false;
            }
            return true;
        }


        public bool M2_ObtenerListaWrapperRebar()
        {
            try
            {
                _listaWrapperRebar = _listaElementsRebarSeleccionado.Where(c => c is Rebar).Select(c =>
                                         new WrapperRebar()
                                         {
                                             element = c,
                                             NombreView = AyudaObtenerParametros.ObtenerNombreView(c),
                                             ObtenerTipoBarra = _ObtenerTipoBarraRebra(c),
                                         }).ToList();

                if (_listaWrapperRebar.Count == 0) return false;

                ListaWrapperRebarFiltro.AddRange(_listaWrapperRebar.Where(c => c.ObtenerTipoBarra != null).ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error ex:{ex.Message}");
                return false;
            }
            return true;
        }

        protected ObtenerTipoBarra _ObtenerTipoBarraRebra(Element _rebar)
        {
            ObtenerTipoBarra _newObtenerTipoBarra = null;
            if (!_rebar.IsValidObject) return null;

            if (_rebar is Rebar)
            {
                _newObtenerTipoBarra = new ObtenerTipoBarra((Rebar)_rebar);
                if (!_newObtenerTipoBarra.Ejecutar()) return null;
            }
            else
                return null;
            Debug.WriteLine(_newObtenerTipoBarra.TipoBarra_.ToString());
            return _newObtenerTipoBarra;
        }

        #region Filtros
        public bool M3_a_ObtenerListaIDSeleccionadosRefuerzoSUperior()
        {
            try
            {
                ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarra_.ToString().Contains("LOSA_SUP")
                                                                      || c.ObtenerTipoBarra.TipoBarraGeneral == Enumeraciones.TipoBarraGeneral.Losa).ToList();

                if (ListaWrapperRebarFiltro.Count == 0) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool M3_b_ObtenerListaIDSeleccionadosElevaciones()
        {
            try
            {
                if ("soloBarras" == casoEspecifico)
                {
                    ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_H
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_V).ToList();
                }
                else if ("soloCOnf" == casoEspecifico)
                {
                    ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO
                                                                   || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO_T).ToList();
                }
                else if ("soloEstriboMuro" == casoEspecifico)
                {
                    ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_L
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_T).ToList();
                }
                else if ("soloEstriboViga" == casoEspecifico)
                {
                    ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_V
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VL
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VT).ToList();
                }
                else if ("soloMalla" == casoEspecifico)
                {
                    ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_H
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_V).ToList();
                }
                else if ("soloElementoViga" == casoEspecifico)
                {
                    ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_V
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VL
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VT
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_H
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_HORQ
                                                                          || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_CABEZA_HORQ).ToList();
                }


                else if ("todos" == casoEspecifico)
                {
                    ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarraGeneral == TipoBarraGeneral.Elevacion).ToList();
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public bool M3_c_ObtenerListaIDSeleccionadosFundaciones()
        {
            try
            {
                ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarraGeneral == Enumeraciones.TipoBarraGeneral.Fundaciones).ToList();
                if (ListaWrapperRebarFiltro.Count == 0) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool M3_d_ObtenerListaIDSeleccionadosEscalera()
        {
            try
            {
                ListaWrapperRebarFiltro = _listaWrapperRebar.Where(c => c.ObtenerTipoBarra.TipoBarraGeneral == Enumeraciones.TipoBarraGeneral.Escalera).ToList();
                if (ListaWrapperRebarFiltro.Count == 0) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        public void M4_BorrarRebarSeleccionadoWrapperRebar()
        {
            if (_listaDimenedionesId.Count == 0 && ListaWrapperRebarFiltro.Count == 0) return;
            try
            {
                using (Transaction transaction = new Transaction(_uidoc.Document))
                {
                    transaction.Start("Borrar Rebar-NH");
                    foreach (WrapperRebar item in ListaWrapperRebarFiltro)
                    {

                        if (item.element is RebarInSystem)
                        {
                            //  _uidoc.Document.Delete(((RebarInSystem)item).SystemId);
                        }
                        else if (item.element is Rebar || item.element is RebarContainer)
                        {
                            _uidoc.Document.Delete(item.element.Id);
                        }

                    }
                    //borrar dimensiones
                    if (_listaDimenedionesId.Count > 0)
                        _uidoc.Document.Delete(_listaDimenedionesId);

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.Message);
            }
        }

    }
}
