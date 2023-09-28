using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo;
using ArmaduraLosaRevit.Model.BarraEstriboV.WPFv;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.WPF.Component;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.BarraV.Copiar;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.BarraEstriboV.Editar
{
    class EditarUbicacionEstribo
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private View3D _view3D_BUSCAR;

        private Plane _planoCOrteViga;

        private Element viga;

        public double AltoEstribo_sinRecub { get; private set; }

        private XYZ ptoCentrolVIga;

        public double espesorRealEstriboFoot_sinRecub { get; private set; }

        private XYZ centroEstribo;
        List<EditarUbicacionEstriboDTO> ListaDespl;
        private XYZ PtoCentralSup;
        private XYZ PtoCentralInf;
        private SeleccionarRebarElemento seleccionarRebar;

        private TiposRebarTagsEnView _tiposRebarTagsEnView;

        public EditarUbicacionEstribo(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.ListaDespl = new List<EditarUbicacionEstriboDTO>();
        }
        public bool ReAjecutarEstribo()
        {
            try
            {
                _view3D_BUSCAR = TiposFamilia3D.Get3DBuscar(_doc);

                if (_view3D_BUSCAR == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial");
                    return false;
                }
                //seleccionar viga
                SeleccionarViga _SeleccionarViga = new SeleccionarViga();

                if (!_SeleccionarViga.M1_3_SeleccionarVariasViga(_uiapp.ActiveUIDocument)) return false;

                //lista barra 1
                seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                seleccionarRebar.BuscarListaRebarEnVistaActual();

                //esta lista se utiliza para cargar los tag
                _tiposRebarTagsEnView = new TiposRebarTagsEnView(_uiapp, _doc.ActiveView);
                _tiposRebarTagsEnView.M0_BuscarRebarTagInView();
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("ActualizarBarraVigaGeneral-NH");

                    for (int i = 0; i < _SeleccionarViga.listAvigas.Count; i++)
                    {
                        ListaDespl.Clear();
                        viga = _SeleccionarViga.listAvigas[i];

                        if (!M1_BuscarPuntoCentralViga(viga)) continue;

                        if (!M2_MoverEstribo()) continue;

                        (bool result, int diamtro) = M2_MoverLAteralesEstribo();
                        if (result)
                        {

                            DatosConfinamientoAutoDTO configuracionInicialEstriboDTO =
                                        new DatosConfinamientoAutoDTO()
                                        {
                                            IsEstribo = false,
                                            tipoConfiguracionEstribo = TipoConfiguracionEstribo.EstriboViga,
                                            tipoEstriboGenera = TipoEstriboGenera.Eviga,

                                            IsLateral = true,
                                            DiamtroLateralEstriboMM = diamtro,
                                            cantidadLaterales = 0,
                                            IsExtenderLatInicio = false,
                                            IsExtenderLatFin = false,

                                            IsTraba = false,

                                            TipoDiseñoEstriboViga = TipoDisenoEstriboVIga.AsignarViga,
                                            ElementoSeleccionado = viga
                                        };

                            if (configuracionInicialEstriboDTO == null) return false;
                            ManejadorEstriboCOnfinamiento manejadorEstriboCOnfinamiento = new ManejadorEstriboCOnfinamiento(_uiapp, configuracionInicialEstriboDTO);
                            manejadorEstriboCOnfinamiento.M1_Ejecutar();
                        }

                        if (!M2_MOverBarrasHorizontales()) continue;

                        M3_MoviendoBArras();
                    }

                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'ReAjecutarEstribo'. ex: {ex.Message}");
                return false;
            }
            return true;
        }

        private bool M3_MoviendoBArras()
        {
            try
            {
                if (ListaDespl.Count == 0) return true;
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CMOver barra-NH");
                    for (int i = 0; i < ListaDespl.Count; i++)
                    {
                        Element Rebar = ListaDespl[i]._Rebar;
                        ElementTransformUtils.MoveElement(_doc, Rebar.Id, -ListaDespl[i].Despla);
                    }

                    //ElementTransformUtils.MoveElement(_doc, ss.Id, ss.ViewDirection* p2.AsDouble());
                    t.Commit();
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puede mover barra");
                return false;

            }
            return true;
        }

        private bool M1_BuscarPuntoCentralViga(Element viga)
        {
            try
            {
                var caraSUperior = viga.ObtenerPLanarFAce_superior();
                var caraInferior = viga.ObtenerCaraInferior();

                PtoCentralSup = ((Face)caraSUperior).ObtenerCenterDeCara();
                PtoCentralInf = ((Face)caraInferior).ObtenerCenterDeCara();

                AltoEstribo_sinRecub = PtoCentralSup.DistanceTo(PtoCentralInf) - ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT * 2;
                ptoCentrolVIga = (PtoCentralSup + PtoCentralInf) / 2;
                espesorRealEstriboFoot_sinRecub = viga.ObtenerEspesorConPtos_foot(ptoCentrolVIga, _view.ViewDirection) - ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT * 2;

                _planoCOrteViga = Plane.CreateByNormalAndOrigin(_view.RightDirection, ptoCentrolVIga);

                if (_planoCOrteViga == null) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'M1_BuscarPuntoCentralViga'. ex: {ex.Message}");
                return false;
            }
            return true;
        }
        private bool M2_MoverEstribo()
        {
            try
            {
                //mover estribos
                var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual
                    .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_V && ((Rebar)c.element).GetHostId().IntegerValue == viga.Id.IntegerValue).ToList();

                for (int i = 0; i < listaRebar.Count; i++)
                {
                    var _reba = listaRebar[i];

                    //cambio coordenadas
                    using (Transaction t = new Transaction(_doc))
                    {
                        t.Start("CMOver barra-NH");

                        ParameterUtil.SetParaDoubleNH(_reba.element, "B", AltoEstribo_sinRecub);
                        ParameterUtil.SetParaDoubleNH(_reba.element, "C", espesorRealEstriboFoot_sinRecub);
                        ParameterUtil.SetParaDoubleNH(_reba.element, "D", AltoEstribo_sinRecub);
                        ParameterUtil.SetParaDoubleNH(_reba.element, "E", espesorRealEstriboFoot_sinRecub);

                        ParameterUtil.SetParaDoubleNH(_reba.element, "B_", AltoEstribo_sinRecub);
                        ParameterUtil.SetParaDoubleNH(_reba.element, "C_", espesorRealEstriboFoot_sinRecub);
                        ParameterUtil.SetParaDoubleNH(_reba.element, "D_", AltoEstribo_sinRecub);
                        ParameterUtil.SetParaDoubleNH(_reba.element, "E_", espesorRealEstriboFoot_sinRecub);

                        t.Commit();
                    }

                    if (AyudaCurveRebar.GetMitadRebarCurves(_reba.element as Rebar))
                    {
                        List<Curve> listaCurva = AyudaCurveRebar.curvaMedia.OrderByDescending(c => c.Length).ToList();

                        var curv1 = listaCurva[0];
                        var curv2 = listaCurva[1];

                        centroEstribo = (curv1.ComputeDerivatives(0.5, false).Origin + curv2.ComputeDerivatives(0.5, false).Origin) / 2.0f;
                        centroEstribo = _planoCOrteViga.ProjectOnto(centroEstribo);
                        XYZ desfase = centroEstribo - ptoCentrolVIga;

                        ListaDespl.Add(new EditarUbicacionEstriboDTO(_reba.element, desfase));
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'MoverEstribo'. ex: {ex.Message}");
                return false;
            }
            return true;
        }
        private (bool, int) M2_MoverLAteralesEstribo()
        {
            int diametro = 0;
            try
            {
                //mover estribos
                var listaRebar = seleccionarRebar._lista_A_DeRebarVistaActual.Where(r => r.element.IsValidObject)
                    .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VL && ((Rebar)c.element).GetHostId().IntegerValue == viga.Id.IntegerValue).ToList();

                if (listaRebar.Count == 0) return (false, 0);

                if (listaRebar[0].element is Rebar)
                {
                    diametro = ((Rebar)listaRebar[0].element).ObtenerDiametroInt();
                }
                //borrar laterales
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Borrar Laterales-NH");

                    _doc.Delete(listaRebar.Select(c => c.element.Id).ToList());

                    t.Commit();
                }
                // volver  crarlos    
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'MoverLAteralesEstribo'. ex: {ex.Message}");
                return (false, diametro);
            }
            return (true, diametro);
        }

        private bool M2_MOverBarrasHorizontales()
        {
            try
            {
                // SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                //seleccionarRebar.BuscarListaRebarEnVistaActual();
                var listaRebarHorizontales = seleccionarRebar._lista_A_DeRebarVistaActual
                    .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_H && ((Rebar)c.element).GetHostId().IntegerValue == viga.Id.IntegerValue).ToList();

                for (int i = 0; i < listaRebarHorizontales.Count; i++)
                {
                    var _reba = listaRebarHorizontales[i];

                    if (AyudaCurveRebar.GetPrimeraRebarCurves(_reba.element as Rebar))
                    {
                        //corregir para obterner curva HORIZONTAL MAS BAJA, ESTE CODIGO ENTREGARA ERROR EN CASO DE BARRA DE BORDE DE HORQUILLAS
                        List<Curve> listaCurva = AyudaCurveRebar.ListacurvesSoloLineas[0].OrderByDescending(c => c.Length).ToList();

                        var curv1 = listaCurva[0];

                        var ptoCentroTramoo = curv1.ComputeDerivatives(0.5, false).Origin;

                        var DiamFoot = (_reba.element as Rebar).ObtenerDiametroFoot();

                        //var barraEscontrada = _tiposRebarTagsEnView.M1_BuscarEnColecctorPorRebar(_reba.element.Id);
                        var barraEscontrada = _tiposRebarTagsEnView.M1_BuscarEnColecctorPorRebar(_reba.element.Id)
                                                                   .Where(c => c.Name.Contains("_F")).FirstOrDefault();

                        if (barraEscontrada != null)
                        {
                            //se desplaza 8 cm pa head tab esta bajo el texto, la idea en centrar la coordenada al texto
                            if (barraEscontrada.TagHeadPosition.Z+Util.CmToFoot(8)< ptoCentroTramoo.Z)
                            {
                                XYZ desfase = new XYZ(0, 0, (ptoCentroTramoo.Z - PtoCentralInf.Z) - (ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT + DiamFoot));
                                ListaDespl.Add(new EditarUbicacionEstriboDTO(_reba.element, desfase));
                            }
                            else
                            {
                                XYZ desfase = new XYZ(0, 0, (ptoCentroTramoo.Z - PtoCentralSup.Z) + (ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT * 2 + DiamFoot));
                                ListaDespl.Add(new EditarUbicacionEstriboDTO(_reba.element, desfase));
                            }
                        }
                        else
                        {
                            //solo si barra esta mas cerca de cara inferior
                            if (PtoCentralSup.Z - ptoCentroTramoo.Z > ptoCentroTramoo.Z - PtoCentralInf.Z)
                            {
                                XYZ desfase = new XYZ(0, 0, (ptoCentroTramoo.Z - PtoCentralInf.Z) - (ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT + DiamFoot));
                                ListaDespl.Add(new EditarUbicacionEstriboDTO(_reba.element, desfase));
                            }
                            else
                            {
                                XYZ desfase = new XYZ(0, 0, (ptoCentroTramoo.Z - PtoCentralSup.Z) + (ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT * 2 + DiamFoot));
                                ListaDespl.Add(new EditarUbicacionEstriboDTO(_reba.element, desfase));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'MOverBarrasHorizontales'. ex: {ex.Message}");
                return false;
            }
            return true;
        }
    }

    public class EditarUbicacionEstriboDTO
    {

        public EditarUbicacionEstriboDTO(Element ElemtREbar, XYZ desfase)
        {
            this._Rebar = ElemtREbar;
            this.Despla = desfase;
        }

        public Element _Rebar { get; set; }
        public XYZ Despla { get; set; }
    }
}
