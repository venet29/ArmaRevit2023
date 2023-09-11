using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
    public class ObtenerNombreTipoBarra
    {
        public static string Ejecutar(Document _doc,  View _view, Element _barraAnalizada)
        {
            string result = "";
            OverrideGraphicSettings grhapOverRRider = _view.GetElementOverrides(_barraAnalizada.Id);
            Color colorRebra = grhapOverRRider.ProjectionLineColor;
            if (colorRebra == null) return "";
            Rebar _rebar = null;

            if (!colorRebra.IsValid)
            {
                _rebar = (Rebar)_barraAnalizada;
                CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(_rebar, XYZ.Zero);
                _CreadorListaWraperRebarLargo.Ejecutar();
                var _wraperRebar = _CreadorListaWraperRebarLargo.ListaCurvaBarras.Where(c => c.IsBarraPrincipal).FirstOrDefault();
                if (Util.IsSimilarValor(Math.Abs(_wraperRebar.direccion.Z), 1))
                    result = "ELEV_BA_V";
                else
                    result = "ELEV_BA_H";

                return result;
            }

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
