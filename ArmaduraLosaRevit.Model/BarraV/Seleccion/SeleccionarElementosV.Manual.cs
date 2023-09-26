using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.Niveles.Vigas;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{
    public partial class SeleccionarElementosV
    {

        public Element _ElemenetoSelect1 { get; set; }
        public Element _ElemenetoSelect2 { get; set; }
        public bool  IsSalirSeleccionPilarMuro { get; set; }
        public Element hostParaleloView { get; set; }
        #region Programanual

        public bool M1_ObtenerPtoinicio()
        {

            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            if (!M1_2_SeleccionarBordeMuro()) return false;
            if (!M1_3_SeleccionarMuroElement()) return false;

            return M1_4_BuscarPtoInicioBase(_ElemetSelect);

        }
        public bool M1_SeleccionarElementoHost()
        {
            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            // if (!M1_2_SeleccionarBordeMuro()) return false;
            if (!M1_3_SeleccionarMuroElement()) return false;

            return true; ;
        }


        #region Metodo M1

        public bool M1_1_CrearWorkPLane_EnCentroViewSecction()
        {
            _origenSeccionView = _view.Origin;
            _RightDirection = _view.RightDirection.Redondear8();
            _ViewNormalDirection6 = _view.ViewDirection.Redondear8();

            if (!CrearWorkPLane.Ejecutar(_doc, _view))
            {
                Util.ErrorMsg($"Error al crear plano de referencia");
                return false;
            }     
            //double AnchoView = _view.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).AsDouble();
            //XYZ NuevoOrigen = _origenSeccionView + -_ViewNormalDirection6 * AnchoView / 2;
            //if (!M1_1_1_CrearOAsignarSketchPlane(NuevoOrigen)) return false;
            return true;
        }
     
        #endregion
        //_PtoInicioBase : solo re ferencia, despues se proyecto en el plano del muro
        public bool M1_2_SeleccionarBordeMuro()
        {
            try
            {
                if (_confiEnfierradoDTO.TipoSelecion == TipoSeleccion.ConElemento)
                {
                    ISelectionFilter filtroMuro = new FiltroVIga_Muro_Rebar_Columna();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                    Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.PointOnElement, "Seleccionar Borde Muro:");
                    _PtoInicioBaseBordeMuro = ref_pickobject_element.GlobalPoint;

                    Element Element_pickobject_element = _doc.GetElement(ref_pickobject_element.ElementId);

                    if (Element_pickobject_element is Wall)
                    {
                        _elementoSeleccionado = ElementoSeleccionado.Muro;
                    }
                    else if (Element_pickobject_element is FamilyInstance)
                    {
                        FamilyInstance famy = Element_pickobject_element as FamilyInstance;
                        if (famy.StructuralType == StructuralType.Column)
                            _elementoSeleccionado = ElementoSeleccionado.Columna;
                        else
                            _elementoSeleccionado = ElementoSeleccionado.Viga;
                    }
                    else if (Element_pickobject_element is Rebar)
                    {
                        _elementoSeleccionado = ElementoSeleccionado.Barra;
                    }
                }
                else if (_confiEnfierradoDTO.TipoSelecion == TipoSeleccion.ConMouse)
                {
                    ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
                    //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                    _PtoInicioBaseBordeMuro = _uidoc.Selection.PickPoint(snapTypes, "Seleccionar Borde Muro");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;

        }

        public bool M1_3_SeleccionarMuroElement()
        {
            try
            {
                //  ISelectionFilter filtroMuro = new FiltroMuro();
                ISelectionFilter filtroMuro = new FiltroVIga_Muro_Column();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Element, filtroMuro, "Seleccionar Cara de Muro:");
                _ptoSeleccionMouseCentroCaraMuro = ref_pickobject_element.GlobalPoint;
                _ptoSeleccionMouseCaraMuro = _ptoSeleccionMouseCentroCaraMuro.ObtenerCopia();

                _ElemetSelect = _doc.GetElement(ref_pickobject_element);

                if (_ElemetSelect == null)
                {
                    Util.ErrorMsg($"No se puedo encontrar Muro de referencia.");
                    return false;
                }
                m1_3_1_AuxObtenerMuros(_ElemetSelect);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }

        protected void m1_3_1_AuxObtenerMuros(Element _ElemetSelect)
        {
            NormalCaraElemento = _view.ViewDirection6();
            if (AyudaObtenerNormarPlanoVisisible.Obtener(_ElemetSelect, _view))
                NormalCaraElemento = AyudaObtenerNormarPlanoVisisible.FaceNormal;

            if (_ElemetSelect is Wall)
            {
                _WallSelect = (Wall)_ElemetSelect;
                _direccionMuro = ((Line)((LocationCurve)(_WallSelect.Location)).Curve).Direction;

                if (!DatosDiseño.IsMuroParaleloView)
                    _direccionMuro = _view.RightDirection;


                _espesorMuroFoot = _WallSelect.ObtenerEspesorMuroFoot(_doc);// tipoFamiliaMuro.Width;
                _largoMuroFoot = _WallSelect.ObtenerLargo();
            }
            else
            {
                _vigaSelect = (FamilyInstance)_ElemetSelect;


                if (_vigaSelect.Category.Name == "Structural Columns") //viga
                {
                    _direccionMuro = _view.RightDirection;
                    //if (AyudaObtenerNormarPlanoVisisible.Obtener(_ElemetSelect, _view))
                    //    _direccionMuro = AyudaObtenerNormarPlanoVisisible.FaceNormal;

                    _espesorMuroFoot = _vigaSelect.ObtenerEspesorConPtos_foot(_ptoSeleccionMouseCentroCaraMuro, _view.ViewDirection);// tipoFamiliaMuro.Width;
                    _largoMuroFoot = _vigaSelect.ObtenerAnchoConPtos(_ptoSeleccionMouseCentroCaraMuro, _view.ViewDirection);
                }
                else //viga
                {
                    _direccionMuro = ((Line)((LocationCurve)(_vigaSelect.Location)).Curve).Direction;
                    _espesorMuroFoot = _vigaSelect.ObtenerEspesorVigaFoot();// tipoFamiliaMuro.Width;
                    _largoMuroFoot = _vigaSelect.ObtenerLargo();
                }

            }
        }



        //Obs1)
        public virtual bool M1_4_BuscarPtoInicioBase(Element _elemet)
        {
            try
            {
                XYZ normalFAce = _view.ViewDirection6();
                if (AyudaObtenerNormarPlanoVisisible.Obtener(_elemet, _view))
                    normalFAce = AyudaObtenerNormarPlanoVisisible.FaceNormal;

                _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(normalFAce, _ptoSeleccionMouseCentroCaraMuro, _PtoInicioBaseBordeMuro);
                if (_PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost.IsAlmostEqualTo(XYZ.Zero)) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }




        public bool M2_SeleccionarPtoInicio()
        {
            try
            {
                NormalCaraElemento = _view.ViewDirection6();
                if (_confiEnfierradoDTO.TipoSelecion == TipoSeleccion.ConElemento)
                {
                    ISelectionFilter filtroMuro = new FiltroVIga_Muro_Rebar_Columna();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                    Reference ref_pickobject_element = default;
                    try
                    {
                        ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.PointOnElement, "Seleccionar Punto Inferior intervalo:");
                        _PtoInicioIntervaloBarra = ref_pickobject_element.GlobalPoint;
                    }
                    catch (Exception)
                    {
                        IsSalirSeleccionPilarMuro=false;
                        return false;
                    }
                 

                    _ElemenetoSelect1 = _doc.GetElement(ref_pickobject_element);
                    //Element host = default;
                    if (_ElemenetoSelect1 is Rebar)
                    {
                       Rebar _barra1 = (Rebar)_ElemenetoSelect1;
                        if (!AyudaObtenerNormarPlanoVisisible.Obtener(_barra1, _view)) return false;
                        hostParaleloView = _doc.GetElement(_barra1.GetHostId());
                    }
                    else
                    {
                        if (!AyudaObtenerNormarPlanoVisisible.Obtener(_ElemenetoSelect1, _view)) return false;
                        hostParaleloView = _ElemenetoSelect1;
                 
                    }
              
                    _ptoSeleccionMouseCentroCaraMuro = AyudaObtenerNormarPlanoVisisible.caraVisibleVErtical.ObtenerCenterDeCara();

                    if (_ptoSeleccionMouseCentroCaraMuro?.IsAlmostEqualTo(XYZ.Zero)==true || _ptoSeleccionMouseCentroCaraMuro==null)
                    {
                        (bool reult, PlanarFace plface)  = hostParaleloView.ObtenerCaraVerticalVIsible(_view);

                        if (reult)
                            _ptoSeleccionMouseCentroCaraMuro = plface.ObtenerCenterDeCara();
                        else
                            Util.ErrorMsg($"Errro al obtener 'M2_SeleccionarPtoInicio' elemento con id:{hostParaleloView.Id} ");
                    }

                    NormalCaraElemento = AyudaObtenerNormarPlanoVisisible.FaceNormal;
                }
                else if (_confiEnfierradoDTO.TipoSelecion == TipoSeleccion.ConMouse)
                {
                    ObjectSnapTypes snapTypes = ObjectSnapTypes.Intersections | ObjectSnapTypes.Endpoints | ObjectSnapTypes.Midpoints; //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                    _PtoInicioIntervaloBarra = _uidoc.Selection.PickPoint(snapTypes, "Seleccionar Punto Inferior de intervalo ");
                }

                _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(NormalCaraElemento, _ptoSeleccionMouseCentroCaraMuro, _PtoInicioIntervaloBarra);
                //_PtoInicioIntervaloBarra = _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost;
                if (_PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost.IsAlmostEqualTo(XYZ.Zero)) return false;

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public SelecionarPtoSup M4_SeleccionarPtoSuperiorLineaBarras_paraHorquilla()
        {
            _confiEnfierradoDTO.TipoSelecion = TipoSeleccion.ConMouse;
            return M4_SeleccionarPtoSuperiorLineaBarras();
        }

        public SelecionarPtoSup M4_SeleccionarPtoSuperiorLineaBarras()
        {

            SelecionarPtoSup selecionarPtoSup = new SelecionarPtoSup(_uiapp, _confiEnfierradoDTO, _listaLevelTotal);
            selecionarPtoSup.SeleccionarPtoSuperior(_PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost, _ptoSeleccionMouseCentroCaraMuro);
            if (selecionarPtoSup.IsConError)
            {
                //   Util.ErrorMsg($"Error Al seleccionar el punto superior n° {_confiEnfierradoDTO.LineaBarraAnalizada}");
                return null;
            }
            ObtenerLargoHorizontalSeleccion(selecionarPtoSup);
            return selecionarPtoSup;
        }

        public bool ObtenerLargoHorizontalSeleccion(SelecionarPtoSup selecionarPtoSup)
        {
            try
            {
                Plane plano = Plane.CreateByNormalAndOrigin(_view.ViewDirection6(), _ptoSeleccionMouseCentroCaraMuro);
                XYZ _PtoInicioIntervaloBarra_ProyectadoCaraMuroHost = plano.ProjectOnto(selecionarPtoSup._PtoInicioIntervaloBarra);

                _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost = _PtoInicioIntervaloBarra_ProyectadoCaraMuroHost;
                XYZ _PtoFinalIntervaloBarraa_ProyectadoCaraMuroHost = plano.ProjectOnto(selecionarPtoSup._PtoFinalIntervaloBarra);
                int auxlargo = (int)Util.FootToCm(_PtoFinalIntervaloBarraa_ProyectadoCaraMuroHost.GetXY0().DistanceTo(_PtoInicioIntervaloBarra_ProyectadoCaraMuroHost.GetXY0()));
                //auxlargo = auxlargo/ Util.FootToCm(_confiEnfierradoDTO.EspaciamietoFoot)
                LargoRecorridoHorizontalSeleccionCM = auxlargo;
            }
            catch (Exception)
            {
                LargoRecorridoHorizontalSeleccionCM = Util.FootToCm(_largoMuroFoot);
                return false;
            }
            return true;
        }
        #endregion


    }
}
