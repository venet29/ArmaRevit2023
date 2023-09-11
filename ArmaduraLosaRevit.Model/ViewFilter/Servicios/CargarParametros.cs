using ArmaduraLosaRevit.Model.Cubicacion.Seleccionar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Visualizacion;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter.Servicios
{

    class CargarParametros
    {
        private UIApplication _uiapp;
        private SeleccionarRebar_PathReinforment _SeleccionarRebar_PathReinforment;
        private Document _doc;
        private View _view3D;

        public CargarParametros(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view3D = _doc.ActiveView;
        }



        public bool Ejecutar_copiarType_BarLengh()
        {
            string idBarra = "";
            try
            {
                if (!Obtener3D()) return false;

                VisualizacionRebar _VisualizacionRebar = new VisualizacionRebar(_uiapp,(View3D) _view3D);
                if (!_VisualizacionRebar.CambiarVisualizacion_Path_rebar_sectBox(EstadoVista.visualizar)) return false;

                _SeleccionarRebar_PathReinforment = new SeleccionarRebar_PathReinforment(_uiapp, _view3D);
                if (!_SeleccionarRebar_PathReinforment.M0_CArgar_PAthReinforment()) return false;
                if (!_SeleccionarRebar_PathReinforment.M0_Cargar_rebar()) return false;

                if (!_SeleccionarRebar_PathReinforment.M1_Ejecutar_rebar())
                {
                    Util.ErrorMsg("Error al obtener Lista Rebar");
                    return false;
                }
                if (!_SeleccionarRebar_PathReinforment.M1_Ejecutar_PAthReinforment())
                {
                    Util.ErrorMsg("Error al obtener Lista PAthReinforment");
                    return false;
                }

                using (Transaction tr = new Transaction(_doc, "CopiarParametroRevision-NH"))
                {
                    tr.Start();

                    for (int i = 0; i < _SeleccionarRebar_PathReinforment._lista_A_ElementoREbarDTO.Count; i++)
                    {
                        idBarra = "";
                        var rebar_ = _SeleccionarRebar_PathReinforment._lista_A_ElementoREbarDTO[i];
                        if (rebar_ == null) continue;
                        if (rebar_._rebar == null) continue;
                        if (!rebar_._rebar.IsValidObject) continue;
                        idBarra = rebar_._rebar.Id.IntegerValue.ToString()+"Rebar";
                        var diamtroBarra = ParameterUtil.FindParaByName(rebar_._rebar, "Type").AsValueString();
                        var largoBArras = ParameterUtil.FindParaByName(rebar_._rebar, "Bar Length").AsDouble();

                        // ParameterUtil.SetParaStringNH(rebar_._rebar, "DiametroRevision", diamtroBarra);
                        ParameterUtil.SetParaDoubleNH(rebar_._rebar, "LargoRevision", Util.FootToCm(largoBArras));

                        if (_view3D != null)
                        {
                            //permite que la barra se vea en el 3d como solidD
                            rebar_._rebar.SetSolidInView((View3D)_view3D, true);
                            //permite que la barra se vea en el 3d como sin interferecnias 
                            rebar_._rebar.SetUnobscuredInView(_view3D, true);
                        }
                    }


                    for (int i = 0; i < _SeleccionarRebar_PathReinforment._lista_A_ElementoPathReinformentDTO.Count; i++)
                    {
                        var Path = _SeleccionarRebar_PathReinforment._lista_A_ElementoPathReinformentDTO[i];
                        if (Path == null) continue;
                        
                        for (int j = 0; j < Path._lista_A_DeRebarInSystem.Count; j++)
                        {


                            var rebar_ = (RebarInSystem)Path._lista_A_DeRebarInSystem[j];
                            if (rebar_ == null) continue;
                            //if (!rebar_.IsValidObject) continue;
                            idBarra = rebar_.Id.IntegerValue.ToString() + "RebarSystem";
                            //  var diamtroBarra = ParameterUtil.FindParaByName(rebar_, "Type").AsValueString();
                            var largoBArras = ParameterUtil.FindParaByName(rebar_, "Bar Length").AsDouble();

                            if (largoBArras < Util.CmToFoot(10)) continue;
                            // ParameterUtil.SetParaStringNH(rebar_._rebar, "DiametroRevision", diamtroBarra);
                            ParameterUtil.SetParaDoubleNH(rebar_, "LargoRevision", Util.FootToCm(largoBArras));

                            if (_view3D != null)
                            {
                                //permite que la barra se vea en el 3d como solidD
                                rebar_.SetSolidInView((View3D)_view3D, true);
                                //permite que la barra se vea en el 3d como sin interferecnias 
                                rebar_.SetUnobscuredInView(_view3D, true);
                            }
                        }
                    }

                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en validad datos iniciales  id:{idBarra}\n ex{ex.Message}");
                return false;
            }
            return true;
        }

        private bool Obtener3D()
        {
            if (_view3D == null)
            {
                Util.ErrorMsg("Vista actual igual null");
                return false;
            }
            if (!(_view3D is View3D))
            {
                Util.ErrorMsg("Vista actual no es View3D");
                return false;
            }
            return true;
        }
    }



}
