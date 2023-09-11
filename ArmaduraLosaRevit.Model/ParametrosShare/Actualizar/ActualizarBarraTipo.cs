using ArmaduraLosaRevit.Model.BarraV.ColorRebar;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ParametrosShare.Actualizar.Model;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Actualizar;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ParametosShare.Actualizar
{
    public class ActualizarBarraTipo
    {
        private Document _doc;
        private Element _barraAnalizada;
        private UIApplication _uiapp;
        private View _view;
#pragma warning disable CS0169 // The field 'ActualizarBarraTipo.nombreActualView' is never used
        private string nombreActualView;
#pragma warning restore CS0169 // The field 'ActualizarBarraTipo.nombreActualView' is never used


        public static int cont { get; set; } = 0;
        public ActualizarBarraTipo(UIApplication _uiapp, View _view)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
            this._view = _view;

        }



        public void Ejecutar(List<Element> ElementosIDs)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarBarraTipo-NH");
                    //view.IsolateElementsTemporary(ElementosIDs);
                    for (int i = 0; i < ElementosIDs.Count; i++)
                    {
                        _barraAnalizada = ElementosIDs[i];
                        if (_barraAnalizada == null) continue;
                        string _BarraTipo = ObtenerNombreTipoBarra.Ejecutar(_doc, _view, _barraAnalizada);
                        if (_BarraTipo == "") continue;

                        if (_view != null && ParameterUtil.FindParaByName(_barraAnalizada, "BarraTipo") != null)
                            ParameterUtil.SetParaInt(_barraAnalizada, "BarraTipo", _BarraTipo);  //"nombre de vista"
                    }

                    trans2.Commit();
                    // uidoc.RefreshActiveView();
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
                Util.ErrorMsg("Error en viewSection:" + _view.Name);

            }
        }

        public void EjecutarSinTrasn_actulizarBarraTipoPorCOlor(List<Element> ElementosIDs)
        {

            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn

                //view.IsolateElementsTemporary(ElementosIDs);
                for (int i = 0; i < ElementosIDs.Count; i++)
                {
                    _barraAnalizada = ElementosIDs[i];
                    if (_barraAnalizada == null) continue;

                    TipoRebarPorColor _TipoRebarPorColor = new TipoRebarPorColor((Rebar)_barraAnalizada, _view);

                    if (!_TipoRebarPorColor.ObtenerNombreTipoBarraPorCOlor()) continue;

                    string nombreBarraTipo = _TipoRebarPorColor.nombreBarraTipoPorColor;

                    if (nombreBarraTipo == "") continue;

                    if (_view != null && ParameterUtil.FindParaByName(_barraAnalizada, "BarraTipo") != null)
                        ParameterUtil.SetParaInt(_barraAnalizada, "BarraTipo", nombreBarraTipo);
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }
        }


        public void EjecutarSinTrasn_actulizarBarraTipo_intercambiar(List<Element> ElementosIDs, string antiguoNombreParametroCompartido, string nuevoNombreParametroCompartido,
                                                                    CambirSOLOTipoBarraDTO _datoDto=null)
        {
            if(_datoDto==null)
                _datoDto= new CambirSOLOTipoBarraDTO();

            if (ElementosIDs == null)
            {
                Util.ErrorMsg("listaIds igual null");
                return;
            }
            if (ElementosIDs.Count == 0)
            {
                Util.ErrorMsg("No se encontraron barras.");
                return;
            }
            if (_view == null)
            {
                Util.ErrorMsg("View analizada es null");
                return;
            }

            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarBarraTipo-NH");

                    //view.IsolateElementsTemporary(ElementosIDs);
                    for (int i = 0; i < ElementosIDs.Count; i++)
                    {
                        Debug.WriteLine($"--> {i} de {ElementosIDs.Count} ");
                        _barraAnalizada = ElementosIDs[i];
                        if (_barraAnalizada == null) continue;

                        Parameter para = ParameterUtil.FindParaByName(_barraAnalizada, antiguoNombreParametroCompartido);

                        if (para != null)
                        {
                            string valor = para.AsString();
                            
                            if (_datoDto.valor_inicial != "")
                                valor = _datoDto.valor_inicial;

                            ParameterUtil.SetParaInt(_barraAnalizada, nuevoNombreParametroCompartido, valor);  //"nombre de vista"
                        }
                    }
                    trans2.Commit();
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }
        }


        public void EjecuEjecutarSinTrasn_GenerarId_Correlativo(List<Element> ListaElementos)
        {

            if (ListaElementos == null) return;
            if (ListaElementos.Count == 0) return;
            try
            {
                //segunda trasn

                //view.IsolateElementsTemporary(ElementosIDs);
                for (int i = 0; i < ListaElementos.Count; i++)
                {
                    _barraAnalizada = ListaElementos[i];
                    if (_barraAnalizada == null) continue;

                    if (_barraAnalizada is Rebar)
                    {
                        Rebar _rebar = (Rebar)_barraAnalizada;
                        if (_rebar.TotalLength == 0) continue;
                    }

                    int nombreActualView = _barraAnalizada.Id.IntegerValue;

                    if (nombreActualView == 0) continue;

                    if (_view != null && ParameterUtil.FindParaByName(_barraAnalizada, "IdBarra") != null)
                        ParameterUtil.SetParaInt(_barraAnalizada, "IdBarra", nombreActualView);  //"nombre de vista"


                    if (_view != null && ParameterUtil.FindParaByName(_barraAnalizada, "Correlativo") != null)
                    {
                        int con_ = ++cont;
                        ParameterUtil.SetParaInt(_barraAnalizada, "Correlativo", con_.ToString());  //"nombre de vista"
                    }
                    else
                    {
#pragma warning disable CS0219 // The variable 'con1_' is assigned but its value is never used
                        int con1_ = 0;
#pragma warning restore CS0219 // The variable 'con1_' is assigned but its value is never used
                    }

                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }
        }

        public void EjecutarSinTrasn_actulizarNOmbreVista(List<Element> ElementosIDs)
        {

            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn

                //view.IsolateElementsTemporary(ElementosIDs);
                for (int i = 0; i < ElementosIDs.Count; i++)
                {
                    _barraAnalizada = ElementosIDs[i];
                    if (_barraAnalizada == null) continue;

                    string nombreActualView = _view.ObtenerNombreIsDependencia();

                    if (nombreActualView == "") continue;

                    if (_view != null && ParameterUtil.FindParaByName(_barraAnalizada, "NombreVista") != null)
                        ParameterUtil.SetParaInt(_barraAnalizada, "NombreVista", nombreActualView);  //"nombre de vista"
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }
        }

        public void ActualizarNOmbreVistaTipoBArra_PAthreinf(List<Element> ElementosIDs)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn

                //view.IsolateElementsTemporary(ElementosIDs);
                for (int i = 0; i < ElementosIDs.Count; i++)
                {
                    Debug.WriteLine($" iteracion {i} de {ElementosIDs.Count}");
                    _barraAnalizada = ElementosIDs[i];
                    if (_barraAnalizada == null) continue;

                    var _paraNombreVista = ParameterUtil.FindParaByName(_barraAnalizada, "NombreVista");
                    if (_paraNombreVista == null) continue;
                    string _NombreVista = _paraNombreVista.AsString();



                    var _paraBarraTipo = ParameterUtil.FindParaByName(_barraAnalizada, "BarraTipo");
                    if (_paraBarraTipo == null) continue;
                    string _BarraTipo = _paraBarraTipo.AsString();


                    ActualizarRebarInSystem.AgregarParametroRebarSystem_sinTrans((PathReinforcement)_barraAnalizada, _NombreVista, _BarraTipo);
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }
        }
    }
}
