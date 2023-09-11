using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Linq;


namespace ArmaduraLosaRevit.Model.BarraV.ColorRebar
{
    public class TipoRebarPorColor
    {
        private Rebar _barraAnalizada;
        private Document _doc;
        private View _view;
        public string nombreBarraTipoPorColor { get;  set; }
        public TipoRebarPorColor(Rebar _rebar, View _view)
        {
            this._barraAnalizada = _rebar;
            this._doc = _rebar.Document;
            this._view = _view;
        }

    

        public bool ObtenerNombreTipoBarraPorCOlor()
        {
            try
            {


                nombreBarraTipoPorColor = "";
                OverrideGraphicSettings grhapOverRRider = _view.GetElementOverrides(_barraAnalizada.Id);
                Autodesk.Revit.DB.Color colorRebra = grhapOverRRider.ProjectionLineColor;
                if (colorRebra == null) return false;
                if (!colorRebra.IsValid) return false;
                Rebar _rebar = null;
                if (colorRebra.Red == 255 && colorRebra.Green == 0 && colorRebra.Blue == 255)
                {
                    _rebar = (Rebar)_barraAnalizada;
                    CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(_rebar, XYZ.Zero);
                    _CreadorListaWraperRebarLargo.Ejecutar();
                    var _wraperRebar = _CreadorListaWraperRebarLargo.ListaCurvaBarras.Where(c => c.IsBarraPrincipal).FirstOrDefault();
                    if (Util.IsSimilarValor(Math.Abs(_wraperRebar.direccion.Z), 1))
                        nombreBarraTipoPorColor = "ELEV_BA_V";
                    else
                        nombreBarraTipoPorColor = "ELEV_BA_H";

                }
                else if (colorRebra.Red == 102 && colorRebra.Green == 102 && colorRebra.Blue == 102)
                    nombreBarraTipoPorColor = "ELEV_MA_V";
                else if (colorRebra.Red == 38 && colorRebra.Green == 76 && colorRebra.Blue == 76)
                {
                    nombreBarraTipoPorColor = "ELEV_ES";
                    _rebar = (Rebar)_barraAnalizada;
                    if (_rebar.TotalLength == 0) return false;
                    RebarShape _RebarShape = _doc.GetElement(_rebar.GetShapeId()) as RebarShape;
                    if (_RebarShape == null) return false;
                    if (_RebarShape.Name == "M_T1")
                    {
                        XYZ _normal = _rebar.GetShapeDrivenAccessor().Normal;
                        if (Util.IsEqual(_normal.Z, 1))
                            nombreBarraTipoPorColor = "ELEV_ES";
                        else
                            nombreBarraTipoPorColor = "ELEV_ES_V";
                    }
                    else
                    {

                        CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(_rebar, XYZ.Zero);
                        _CreadorListaWraperRebarLargo.Ejecutar();
                        var _wraperRebar = _CreadorListaWraperRebarLargo.ListaCurvaBarras.Where(c => c.IsBarraPrincipal).FirstOrDefault();
                        if (_wraperRebar == null) return false;
                        if (Util.IsSimilarValor(Math.Abs(_wraperRebar.direccion.Z), 1))
                            nombreBarraTipoPorColor = "ELEV_ES";
                        else
                            nombreBarraTipoPorColor = "ELEV_ES_V";

                    }


                }
                else if (colorRebra.Red == 254 && colorRebra.Green == 254 && colorRebra.Blue == 254)
                    nombreBarraTipoPorColor = "ELEV_CO";

            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

    }
}
