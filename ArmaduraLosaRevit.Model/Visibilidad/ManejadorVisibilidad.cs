using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Ocultar;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Actualizar;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public class ManejadorVisibilidad
    {
        private readonly Document _doc;
        private UIDocument _uidoc;
        private readonly View _View;
        private UIApplication _uiapp;
        private readonly string _nombreDeViewMantenerBarras;
        private VisibilidadElemenNoEnView _visibilidadElemenNoEnView;
        private VisibilidadElementBase _visibilidadElement;
        private SeleccionarRebarElemento _SeleccionarRebarElemento;
        private readonly SeleccionarAreaPath _seleccionarAreaPath;
        private SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad;
        private SeleccionarRebarVisibilidad _seleccionarRebarVisibilidad;


        public string NombreAntiguoVista { get; private set; }

        public ManejadorVisibilidad(UIApplication _uiapp,
                                    VisibilidadElementBase visibilidadElement,
                                    SeleccionarPathReinfomentVisibilidad seleccionarPathReinfomentVisibilidad,
                                    SeleccionarRebarElemento seleccionarRebarElemento = null,
                                    SeleccionarRebarVisibilidad _seleccionarRebarVisibilidad = null)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._View = this._doc.ActiveView;
            this._uiapp = _uiapp;
            this._visibilidadElement = visibilidadElement;
            this._SelecPathReinVisibilidad = seleccionarPathReinfomentVisibilidad;
            this._SeleccionarRebarElemento = seleccionarRebarElemento;

            this._seleccionarRebarVisibilidad = _seleccionarRebarVisibilidad;
        }
        public ManejadorVisibilidad(UIApplication _uiapp, SeleccionarRebarElemento seleccionarRebarElemento)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._View = this._doc.ActiveView;
            this._uiapp = _uiapp;
            this._visibilidadElement = null;
            this._SelecPathReinVisibilidad = null;
            this._SeleccionarRebarElemento = seleccionarRebarElemento;
            this.NombreAntiguoVista = "";
        }


        public ManejadorVisibilidad(UIApplication _uiapp,
                                    VisibilidadElemenNoEnView visibilidadElement,
                                    SeleccionarRebarElemento seleccionarRebarElemento,
                                    SeleccionarAreaPath _seleccionarAreaPath,
                                    View ViewMantenerBArras,
                                    string nombreDeViewMantenerBarras)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._uiapp = _uiapp;
            this._View = ViewMantenerBArras;
            this._nombreDeViewMantenerBarras = nombreDeViewMantenerBarras;
            this._visibilidadElemenNoEnView = visibilidadElement;

            this._SelecPathReinVisibilidad = null;
            this._SeleccionarRebarElemento = seleccionarRebarElemento;
            this._seleccionarAreaPath = _seleccionarAreaPath;
            this.NombreAntiguoVista = "";
        }

        public void M1_MostrarPorOrientacion()
        {
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("MostrarPorOrientacion-NH");
                    if (!MostrarPathYOcultarSymbolYTag()) return;
                    try
                    {
                        //segunda trasn
                        using (Transaction trans2 = new Transaction(_doc))
                        {
                            trans2.Start("Ocultando elementos-NH");
                            _doc.Regenerate();
                            trans2.Commit();
                            // _uidoc.RefreshActiveView();
                        } // fin trans 
                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"ex: {ex.Message}");

                    }


                    List<List<ElementoPath>> list_Aux = new List<List<ElementoPath>>();
                    list_Aux.Add(_SelecPathReinVisibilidad._lista_A_VisibilidadElementoPathDTO);
                    _visibilidadElement.Ejecutar(list_Aux, _View);
                    //**********************************************************************************

                    if (!MostraRebar()) return;

                    list_Aux.Clear();
                    list_Aux.Add(_seleccionarRebarVisibilidad._lista_A_VisibilidadElementoREbarDTO);

                    //resetear colores rebar
                    _visibilidadElement.ChangeListaElementColorConTrans(_seleccionarRebarVisibilidad._lista_A_TodosRebarNivelActual.Select(ii => ii.Id).ToList(),
                                                         FactoryColores.ObtenerColoresPorNombre(TipoCOlores.blanco), false);
                    _visibilidadElement.ChangePresentationModeRebarCONTrans(_seleccionarRebarVisibilidad._lista_A_TodosRebarNivelActual.Select(ii => (Rebar)ii).ToList(),
                                                    RebarPresentationMode.FirstLast, _View);
                    //*ejecutar colores en rebar
                    _visibilidadElement.Ejecutar(list_Aux, _View);

                    _visibilidadElement.ChangePresentationModeRebarCONTrans(_seleccionarRebarVisibilidad._lista_A_VisibilidadElementoREbarDTO.Select(el => ((ElementoPathRebar)el)._rebar).ToList(),
                                                     RebarPresentationMode.All, _View);


                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }
        }

        public void M2_MostrarPorDiametro()
        {
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("MostrarPorDiametro-NH");
                    if (!MostrarPathYOcultarSymbolYTag()) return;

                    List<List<ElementoPath>> list_Aux = new List<List<ElementoPath>>();
                    list_Aux.Add(_SelecPathReinVisibilidad._lista_A_VisibilidadElementoPathDTO);
                    _visibilidadElement.Ejecutar(list_Aux, _View);


                    if (!MostraRebar()) return;
                    list_Aux.Clear();
                    list_Aux.Add(_seleccionarRebarVisibilidad._lista_A_VisibilidadElementoREbarDTO);
                    _visibilidadElement.Ejecutar(list_Aux, _View);
                    _visibilidadElement.ChangePresentationModeRebarCONTrans(_seleccionarRebarVisibilidad._lista_A_VisibilidadElementoREbarDTO.Select(el => ((ElementoPathRebar)el)._rebar).ToList(),
                                                     RebarPresentationMode.All, _View);

                    //_visibilidadElement.CAmbiarColorPorDiametro(_SelecPathReinVisibilidad._lista_A_VisibilidadElementoPathDTO, _view);
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }
        }

        private bool MostrarPathYOcultarSymbolYTag()
        {
            if (_SelecPathReinVisibilidad == null) return false;

            try
            {
                _SelecPathReinVisibilidad.M1_ejecutar();

                _visibilidadElement.vi1_OcultarElemento(_SelecPathReinVisibilidad._lista_A_DeRebarInSystemNivelActual_preOcultar, _View);
                //ocultar si existen path visibles()
                _visibilidadElement.vi1_OcultarElemento(_SelecPathReinVisibilidad._lista_A_DeRebarInSystemNivelActual_preOcultar, _View);

                _visibilidadElement.ChangeListaElementColorConTrans(_SelecPathReinVisibilidad._lista_A_DeRebarInSystem.Select(el => el.Id).ToList(),
                                                               FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta), false);

                _visibilidadElement.vi1_OcultarElemento(_SelecPathReinVisibilidad._lista_A_DeRebarInSystem, _View);



                _visibilidadElement.Vi2_DesOcultarElemento_conTrans(_SelecPathReinVisibilidad._lista_A_DeRebarInSystem, _View);

                List<Element> aux_ListaElemento_PAthSymbol = _SelecPathReinVisibilidad._lista_B_DePathSymbolNivelActual.Select(c => c.element).ToList();
                _visibilidadElement.vi1_OcultarElemento(aux_ListaElemento_PAthSymbol, _View);

                List<Element> aux_ListaElemento_TagPAth = _SelecPathReinVisibilidad._lista_C_DePathTagNivelActual.Select(c => c.element).ToList();
                _visibilidadElement.vi1_OcultarElemento(aux_ListaElemento_TagPAth, _View);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error  en 'MostrarPathYOcultarSymbolYTag'  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool MostraRebar()
        {
            if (_SeleccionarRebarElemento == null) return false;

            try
            {
                if (!_seleccionarRebarVisibilidad.M1_Ejecutar_ConrebarSystem()) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error  en 'MostraRebar'  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal()
        {
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("MostrarEstadoNormal-NH");
                    if (_SelecPathReinVisibilidad == null) return false;
                    try
                    {
                        //1
                        if (!ActivarRevealHiddenMode())
                        {
                            t.RollBack();
                            return false;
                        }

                        //2 seleccionar los 3  elemntos // pathrein - pathsymbol- tagpath

                        if (!_SelecPathReinVisibilidad.M1_ejecutar())
                        {
                            t.RollBack();
                            return false;
                        }



                        if (!_seleccionarRebarVisibilidad.M1_Ejecutar_ConrebarSystem())
                        {
                            t.RollBack();
                            return false;
                        }

                        //3
                        if (!DesactivarRevealHiddenMode())
                        {
                            t.RollBack();
                            return false;
                        }

                        //4)
                        try
                        {
                            using (Transaction tr = new Transaction(_doc, "actualizarVisualizacion normal"))
                            {
                                tr.Start();

                                //4)
                                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _View);
                                if (seleccionarRebar.BuscarListaRebarEnVistaActual())
                                {

                                    var listaBarraEstrivos = seleccionarRebar._lista_A_DeRebarVistaActual
                                                                           .Where(c => c != null && (c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_EST_REF_LO ||
                                                                                                    c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_EST_BORDE ||
                                                                                                    c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_ES))
                                                                           .Select(c => c.element.Id).ToList();

                                    CambiarColorBarras_Service CambiarColorBarras_Service = new CambiarColorBarras_Service(_uiapp, TipoConfiguracionEstribo.Estribo);
                                    CambiarColorBarras_Service.M1_2_CAmbiarColor_sintrans(listaBarraEstrivos);

                                }

                                //vuelve pathreinfomente a color magenta
                                _visibilidadElement.ChangeListElementsColorSinTrans(_SelecPathReinVisibilidad._lista_A_DeRebarInSystem.Select(el => el.Id).ToList(),
                                                               FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta), false);
                                //vuelve rebar a blanco
                                _visibilidadElement.ChangeListElementsColorSinTrans(_seleccionarRebarVisibilidad._lista_A_VisibilidadElementoREbarDTO.Select(el => ((ElementoPathRebar)el)._rebar.Id).ToList(),
                                                              FactoryColores.ObtenerColoresPorNombre(TipoCOlores.blanco), false);

                                //REVISAR
                                //_visibilidadElement.ChangePresentationModeRebarSinTrans(_seleccionarRebarVisibilidad._lista_A_VisibilidadElementoREbarDTO.Select(el => ((ElementoPathRebar)el)._rebar).ToList(),
                                //                             RebarPresentationMode.FirstLast, _ViewMantenerBArras);
                                //_rebar.SetPresentationMode(viewActual, RebarPresentationMode.FirstLast);


                           

                                //oculta pathReinforment 
                                _visibilidadElement.vi1_OcultarElemento(_SelecPathReinVisibilidad._lista_A_DeRebarInSystem, _View, IsCOnstrans: false);

                                //desoculta  pathRein
                                _visibilidadElement.Vi3_DesOcultarElemento_SinTrans(_SelecPathReinVisibilidad._lista_A_VisibilidadElementoPathReinDTO.Select(c => (Element)c._pathReinforcement).ToList(), _View);
                                //desoculta  pathsymbol
                                List<Element> aux_ListaElemento_PAthSymbol = _SelecPathReinVisibilidad._lista_B_DePathSymbolNivelActual.Where(cc => cc.TipoPath.ToString().Contains("f") || cc.TipoPath.ToString().Contains("s")).Select(c => c.element).ToList();
                                _visibilidadElement.Vi3_DesOcultarElemento_SinTrans(aux_ListaElemento_PAthSymbol, _View);
                                //desoculta tag de pathreinforment
                                List<Element> aux_ListaElemento_TagPAth = _SelecPathReinVisibilidad._lista_C_DePathTagNivelActual.Where(cc => cc.TipoPath.ToString().Contains("f") || cc.TipoPath.ToString().Contains("s")).Select(c => c.element).ToList();
                                _visibilidadElement.Vi3_DesOcultarElemento_SinTrans(aux_ListaElemento_TagPAth, _View);





                                tr.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($" ex:{ex.Message}");
                            t.RollBack();
                            return false;
                        }



                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                        return false;
                    }
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }


            return true;
        }

        public bool M4_CAmbiar_PorTipo()
        {
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CambiarPorTipo-NH");
                    if (_SelecPathReinVisibilidad == null) return false;
                    try
                    {
                        List<List<ElementoPath>> list_Aux = new List<List<ElementoPath>>();

                        _SelecPathReinVisibilidad.M1_ejecutar();
                        List<ElementoPath> _lista_A_VisibilidadElementoPathDTO_Visible = _SelecPathReinVisibilidad.M2_ObtenerElemntosConPAthSymbol_Visible();
                        list_Aux.Add(_lista_A_VisibilidadElementoPathDTO_Visible);
                        //1
                        ActivarRevealHiddenMode();

                        //2 seleccionar los 3  elemntos // pathrein - pathsymbol- tagpath
                        _SelecPathReinVisibilidad.M1_ejecutar();
                        List<ElementoPath> _lista_A_VisibilidadElementoPathDTO_Ocultos = _SelecPathReinVisibilidad.M2_ObtenerElemntosConPAthSymbol_Visible();
                        _lista_A_VisibilidadElementoPathDTO_Ocultos = _lista_A_VisibilidadElementoPathDTO_Ocultos.
                                                            Where(cc => (!_lista_A_VisibilidadElementoPathDTO_Visible.Contains(cc))).ToList();

                        list_Aux.Add(_lista_A_VisibilidadElementoPathDTO_Ocultos);
                        //3
                        DesactivarRevealHiddenMode();

                        _visibilidadElement.Ejecutar(list_Aux, _View);
                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                        return false;
                    }

                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

            return true;

        }

        public bool M5_OcultarBarraNoElevacion()
        {
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("OcultarBarraNoElevacio-NH");
                    if (_SeleccionarRebarElemento == null) return false;
                    try
                    {

                        ObtenerNombreAntiguoVista();

                        char[] charsToTrim1 = { ConstNH.CARACTER_PTO };
                        string _nombreDeViewMantenerBarrasSinPto = _nombreDeViewMantenerBarras.TrimEnd(charsToTrim1);
                        _SeleccionarRebarElemento.BuscarListaRebarEnVistaActual();

                        _visibilidadElemenNoEnView.vi1_OcultarElemento(_SeleccionarRebarElemento._lista_A_DeRebarVistaActual.Where(c => !c.NombreView.Contains(_nombreDeViewMantenerBarrasSinPto) &&
                                                                                                                        c.NombreView != "" &&
                                                                                                                        c.NombreView != null).Select(x => x.element).ToList(), _View);

                        _SeleccionarRebarElemento.BuscarListaPathReinformetEnVistaActual();
                        _visibilidadElemenNoEnView.vi1_OcultarElemento(_SeleccionarRebarElemento._lista_A_DePathReinfVistaActual.Where(c => !c.NombreView.Contains(_nombreDeViewMantenerBarrasSinPto) &&
                                                                                                                  c.NombreView != "" &&
                                                                                                                  c.NombreView != null).Select(x => x.element).ToList(), _View);
                        _seleccionarAreaPath.BuscarListaAreaReinEnVistaActual();
                        //_visibilidadElemenNoEnView.OcultarElemento(_seleccionarAreaPath._lista_A_DeAreaReinVistaActual.Where(c => c.NombreView != _nombreDeViewMantenerBarras &&
                        //                                                                                         c.NombreView != "" &&
                        //                                                                                         c.NombreView != null).Select(x => x.element).ToList(), ViewMantenerBArras);

                        var listReb = _seleccionarAreaPath._lista_A_DeAreaReinVistaActual.Where(c => !c.NombreView.Contains(_nombreDeViewMantenerBarrasSinPto) &&
                                                                                                                 c.NombreView != "" &&
                                                                                                                 c.NombreView != null).SelectMany(x => x.ListaRebarInsistem).ToList();
                        _visibilidadElemenNoEnView.Vi_1_OcultarElementID_conTrans(listReb, _View);



                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                        return false;
                    }
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

            return true;


        }

        public bool M6_ActualizarNombreVista(string nombreActualView, string ViewNombre_ParamtroActual)
        {
            VisibilidadActualizarNombreVista _VisibilidadActualizarNombreVista = new VisibilidadActualizarNombreVista(_uiapp, nombreActualView);
            try
            {

                string ViewNombreActual = _View.ObtenerNombre_ViewNombre();
                //  using (TransactionGroup t = new TransactionGroup(_doc))
                //  {
                //   t.Start("ActualizarNombreVista-NH");
                if (_SeleccionarRebarElemento == null) return false;
                try
                {
                    _SeleccionarRebarElemento.BuscarListaRebarEnVistaActual();

                    _SeleccionarRebarElemento.BuscarListaPathReinformetEnVistaActual();

                    _seleccionarAreaPath.BuscarListaAreaReinEnVistaActual();

                    NombreAntiguoVista = _View.ObtenerNombre_ViewNombre();
                    _View.DesactivarViewTemplate_ConTrans();
                    // ObtenerNombreAntiguoVista();

                    List<Element> ListaFinalOcultar = new List<Element>();
                    List<Element> ListaFinal = new List<Element>();
                    ListaFinal.AddRange(_SeleccionarRebarElemento._lista_A_DeRebarVistaActual
                                                                 .Where(c => c.NombreView == ViewNombre_ParamtroActual)
                                                                 .Select(x => x.element).ToList());
                    ListaFinal.AddRange(_SeleccionarRebarElemento._lista_A_DePathReinfVistaActual
                                                                .Where(c => c.NombreView == ViewNombre_ParamtroActual)
                                                                .Select(x => x.element).ToList());
                    ListaFinalOcultar.AddRange(ListaFinal);
                    _VisibilidadActualizarNombreVista.ActualizarNombreVista(ListaFinal, _View);


                    ListaFinal.Clear();
                    ListaFinal.AddRange(_SeleccionarRebarElemento._lista_A_DePathReinfVistaActual
                                                                .Where(c => c.NombreView == ViewNombre_ParamtroActual)
                                                                .SelectMany(x => x.ListaRebarInsistem).Select(f => _doc.GetElement(f)).ToList());
                    _VisibilidadActualizarNombreVista.ActualizarNombreVista(ListaFinal, _View);

                    bool iSDesactivadoPOrUsodeFiltro = false;
                    if (iSDesactivadoPOrUsodeFiltro)
                    {   //ocultar en otrsa vista
                        OcultarBarrasRebaroPathrein _OcultarBarrasRebaroPathrein = new OcultarBarrasRebaroPathrein(_doc);
                        _OcultarBarrasRebaroPathrein.OcultarListaBarraEnViewDistintaActual_ConTrans(ListaFinalOcultar);

                        //ocultar en view actual              
                        ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(_uiapp);
                        _ManejadorVisibilidadElemenNoEnView.Ejecutar(_View, _View.Name);
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                    return false;
                }
                //t.Assimilate();
                // }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

            return true;


        }

        public bool M6_ActualizarNombreVista_soloReBArInsystemtemdesdePAthrein()
        {
            // VisibilidadActualizarNombreVista _VisibilidadActualizarNombreVista = new VisibilidadActualizarNombreVista(_uiapp, nombreActualView);
            try
            {
                //  using (TransactionGroup t = new TransactionGroup(_doc))
                //  {
                //   t.Start("ActualizarNombreVista-NH");
                if (_SeleccionarRebarElemento == null) return false;
                try
                {
                    _SeleccionarRebarElemento.BuscarListaPathReinformetEnVistaActual();

                    var ListaPathRein = _SeleccionarRebarElemento
                                        ._lista_A_DePathReinfVistaActual
                                        .Select(x => x.element as PathReinforcement).ToList();

                    ActualizarRebarInSystem.AgregarParametroRebarSystem_ConTrans(_doc, ListaPathRein, "", "");

                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                    return false;
                }
                //t.Assimilate();
                // }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

            return true;


        }

        private void ObtenerNombreAntiguoVista()
        {
            try
            {
                NombreAntiguoVista = _View.ObtenerNombre_ViewNombre();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"'ObtenerNombreAntiguoVista' ex:{ex.Message}");

            }
        }

        public bool M7_OcultarBarrasVigaIdem()
        {
            VisibilidadElementoVigaIdem _VisibilidadElementoVigaIdem = new VisibilidadElementoVigaIdem(_uiapp, _View.Name);
            try
            {
                //  using (TransactionGroup t = new TransactionGroup(_doc))
                //  {
                //   t.Start("ActualizarNombreVista-NH");
                if (_SeleccionarRebarElemento == null) return false;
                if (_VisibilidadElementoVigaIdem == null) return false;
                try
                {

                    _SeleccionarRebarElemento.BuscarListaRebarEnVistaActual();

                    List<Element> listaRebarIdemVIga = _VisibilidadElementoVigaIdem.GetListaRebarIdemVIga(_SeleccionarRebarElemento._lista_A_DeRebarVistaActual);

                    if (listaRebarIdemVIga.Count == 0)
                    {
                        Util.InfoMsg("B) No se encontraron Barras de vigas idem para ocultar");
                        return false;
                    }
                    _VisibilidadElementoVigaIdem.vi1_OcultarElemento(listaRebarIdemVIga, _View);

                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                    return false;
                }
                //t.Assimilate();
                // }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

            return true;


        }

        public bool M8_DesOcultarBarrasVigaIdem()
        {
            VisibilidadElementoVigaIdem _VisibilidadElementoVigaIdem = new VisibilidadElementoVigaIdem(_uiapp, _View.Name);
            try
            {
                //  using (TransactionGroup t = new TransactionGroup(_doc))
                //  {
                //   t.Start("ActualizarNombreVista-NH");
                if (_SeleccionarRebarElemento == null) return false;
                if (_VisibilidadElementoVigaIdem == null) return false;
                try
                {
                    ActivarRevealHiddenMode();
                    _SeleccionarRebarElemento.BuscarListaRebarEnVistaActual();
                    DesactivarRevealHiddenMode();
                    List<Element> listaRebarIdemVIga = _VisibilidadElementoVigaIdem.GetListaRebarIdemVIgaOcultas(_SeleccionarRebarElemento._lista_A_DeRebarVistaActual);

                    if (listaRebarIdemVIga.Count == 0)
                    {
                        Util.InfoMsg("B) No se encontraron Barras de vigas idem para ocultar");
                        return false;
                    }

                    _VisibilidadElementoVigaIdem.Vi2_DesOcultarElemento_conTrans(listaRebarIdemVIga, _View);

                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                    return false;
                }
                //t.Assimilate();
                // }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

            return true;


        }

        public bool M9_Restablecer_Color_BarrasElevacion()
        {
            _seleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
            VisibilidadElemenCambiarColorElev _VisibilidadElemenCambiarColorElev = new VisibilidadElemenCambiarColorElev(_uiapp);
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("MostrarEstadoNormal-NH");
                    if (_SeleccionarRebarElemento == null) return false;

                    if (_VisibilidadElemenCambiarColorElev == null) return false;

                    try
                    {
                        //1
                        if (!_seleccionarRebarVisibilidad.BuscarListaRebarEnVistaActualElevacion()) return false;

                        _VisibilidadElemenCambiarColorElev.M9_Restablecer_Color_BarrasElevacion(_seleccionarRebarVisibilidad._lista_A_DeRebarVistaActualElevacion);

                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                        return false;
                    }
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

            return true;
        }


        public bool M10_Ocultar_Color_BarrasElevacion_traba_estrivovigaYLaterales(bool Isocultar)
        {
            _seleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
            VisibilidadElemenCambiarColorElev _VisibilidadElemenCambiarColorElev = new VisibilidadElemenCambiarColorElev(_uiapp);
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("MostrarEstadoNormal-NH");
                    if (_SeleccionarRebarElemento == null) return false;

                    if (_VisibilidadElemenCambiarColorElev == null) return false;

                    try
                    {
                        //1
                        if (!_seleccionarRebarVisibilidad.BuscarListaRebarEnVistaActualElevacion()) return false;

                        _VisibilidadElemenCambiarColorElev.M9_Ocultar_Color_BarrasElevacion_Traba(_seleccionarRebarVisibilidad._lista_A_DeRebarVistaActualElevacion, Isocultar);

                        _VisibilidadElemenCambiarColorElev.M9_Ocultar_Color_BarrasElevacion_EStriboVigaLAterales(_seleccionarRebarVisibilidad._lista_A_DeRebarVistaActualElevacion, Isocultar);

                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                        return false;
                    }
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }

            return true;
        }


        private bool DesactivarRevealHiddenMode()
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "Unhide3"))
                {
                    tr.Start();
                    TemporaryViewMode tempView = TemporaryViewMode.RevealHiddenElements;
                    if (_View.IsInTemporaryViewMode(tempView)) _View.DisableTemporaryViewMode(tempView);
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool ActivarRevealHiddenMode()
        {

            try
            {
                using (Transaction tr = new Transaction(_doc, "Unhide"))
                {
                    tr.Start();
                    _View.EnableRevealHiddenMode();
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
