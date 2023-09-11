using ArmaduraLosaRevit.Model.BarraV.ColorRebar;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ParametosShare.Actualizar
{
    public class ActualizarnNumeroBarra
    {
        private Document _doc;
        private Element _barraAnalizada;
        private UIApplication _uiapp;
        private View _view;
#pragma warning disable CS0169 // The field 'ActualizarnNumeroBarra.nombreActualView' is never used
        private string nombreActualView;
#pragma warning restore CS0169 // The field 'ActualizarnNumeroBarra.nombreActualView' is never used


        public static int cont { get; set; } = 0;
        public ActualizarnNumeroBarra(UIApplication _uiapp, View _view)
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
                    trans2.Start("ActualizarNUmeroBarra-NH");
                    //view.IsolateElementsTemporary(ElementosIDs);
                    Ejecutar_Sintrasn(ElementosIDs);
                    trans2.Commit();
                    // uidoc.RefreshActiveView();nn
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
                Util.ErrorMsg("Error en viewSection:" + _view.Name);

            }
        }
        public void Ejecutar_Sintrasn(List<Element> ElementosIDs)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {

                    for (int i = 0; i < ElementosIDs.Count; i++)
                    {
                        _barraAnalizada = ElementosIDs[i];
                        if (_barraAnalizada == null) continue;
                        var largosPAth = (_barraAnalizada as PathReinforcement).ObtenerNumeroBarras(_doc);
                        if (largosPAth == null) continue;



                        if (_view != null && ParameterUtil.FindParaByName(_barraAnalizada, "NumeroPrimario") != null && largosPAth[0] != 0)
                            ParameterUtil.SetParaInt(_barraAnalizada, "NumeroPrimario", largosPAth[0].ToString());  //"nombre de vista"
                        if (_view != null && ParameterUtil.FindParaByName(_barraAnalizada, "NumeroSecundario") != null && largosPAth[1] != 0)
                            ParameterUtil.SetParaInt(_barraAnalizada, "NumeroSecundario", largosPAth[1].ToString());  //"nombre de vista"
                    }

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
                Util.ErrorMsg("Error en viewSection:" + _view.Name);

            }
        }

        public void EjecutarFUndaciones(List<Element> ElementosIDs)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarNUmeroBarra-NH");
                    //view.IsolateElementsTemporary(ElementosIDs);
                    for (int i = 0; i < ElementosIDs.Count; i++)
                    {
                        _barraAnalizada = ElementosIDs[i];
                        if (_barraAnalizada == null) continue;
                        var largosPAth = (_barraAnalizada as PathReinforcement).ObtenerNumeroBarras(_doc);
                        if (largosPAth == null) continue;



                        if (_view != null && ParameterUtil.FindParaByName(_barraAnalizada, "NumeroPrimario") != null && largosPAth[0] != 0)
                            ParameterUtil.SetParaInt(_barraAnalizada, "NumeroPrimario", largosPAth[0].ToString());  //"nombre de vista"

                    }

                    trans2.Commit();
                    // uidoc.RefreshActiveView();nn
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
                Util.ErrorMsg("Error en viewSection:" + _view.Name);

            }
        }



        private string ObtenerNUmeroBarraActual()
        {
            string result = "";
            OverrideGraphicSettings grhapOverRRider = _view.GetElementOverrides(_barraAnalizada.Id);
            Color colorRebra = grhapOverRRider.ProjectionLineColor;
            if (colorRebra == null) return "";
            if (!colorRebra.IsValid) return "";
            Rebar _rebar = null;
            if (colorRebra.Red == 255 && colorRebra.Green == 0 && colorRebra.Blue == 255)
            {
                _rebar = (Rebar)_barraAnalizada;
                CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(_rebar, XYZ.Zero);
                _CreadorListaWraperRebarLargo.Ejecutar();
                var _wraperRebar = _CreadorListaWraperRebarLargo.ListaCurvaBarras.Where(c => c.IsBarraPrincipal).FirstOrDefault();
                if (Util.IsSimilarValor(Math.Abs(_wraperRebar.direccion.Z), 1))
                    result = "ELEV_BA_V";
                else
                    result = "ELEV_BA_H";

            }
            else if (colorRebra.Red == 102 && colorRebra.Green == 102 && colorRebra.Blue == 102)
                result = "ELEV_MA_V";
            else if (colorRebra.Red == 38 && colorRebra.Green == 76 && colorRebra.Blue == 76)
            {
                result = "ELEV_ES";
                _rebar = (Rebar)_barraAnalizada;
                if (_rebar.TotalLength == 0) return "";
                RebarShape _RebarShape = _doc.GetElement(_rebar.GetShapeId()) as RebarShape;
                if (_RebarShape == null) return "";
                if (_RebarShape.Name == "M_T1")
                {
                    XYZ _normal = _rebar.GetShapeDrivenAccessor().Normal;
                    if (Util.IsEqual(_normal.Z, 1))
                        result = "ELEV_ES";
                    else
                        result = "ELEV_ES_V";
                }
                else
                {

                    CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(_rebar, XYZ.Zero);
                    _CreadorListaWraperRebarLargo.Ejecutar();
                    var _wraperRebar = _CreadorListaWraperRebarLargo.ListaCurvaBarras.Where(c => c.IsBarraPrincipal).FirstOrDefault();
                    if (_wraperRebar == null) return result;
                    if (Util.IsSimilarValor(Math.Abs(_wraperRebar.direccion.Z), 1))
                        result = "ELEV_ES";
                    else
                        result = "ELEV_ES_V";

                }


            }
            else if (colorRebra.Red == 254 && colorRebra.Green == 254 && colorRebra.Blue == 254)
                result = "ELEV_CO";



            return result;
        }


    }
}
