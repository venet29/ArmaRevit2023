using ApiRevit.FILTROS;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.Seleccion.Vigas;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{

    public class SeleccionarElementosHAuto_vigas : SeleccionarElementosH
    {

        private View3D _view3D_BUSCAR;

#pragma warning disable CS0414 // The field 'SeleccionarElementosHAuto_vigas._ubicacionBarras' is assigned but its value is never used
        private TipoCaraObjeto _ubicacionBarras;
#pragma warning restore CS0414 // The field 'SeleccionarElementosHAuto_vigas._ubicacionBarras' is assigned but its value is never used


        public List<XYZ> _listaptoTramo { get; set; }
        public List<XYZ> _listaptoTramoSup { get; set; }
        public List<XYZ> _listaptoTramoInf { get; set; }
        public SeleccionPtosEstriboViga_sinSeleccionBarras _seleccionPtosEstriboViga { get; set; }

        public SeleccionarElementosHAuto_vigas(UIApplication _uiapp, ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO, DireccionRecorrido _DireccionRecorrido)
            : base(_uiapp, _configuracionInicialBarraHorizontalDTO, _DireccionRecorrido)
        {
            _ubicacionBarras = TipoCaraObjeto.Inferior;
            M1_6a_ResetearListas();
        }


        public override bool M1_ObtenerPtoinicio_RefuerzoBorde()
        {
            _view3D_BUSCAR = TiposFamilia3D.Get3DBuscar(_doc);
            if (_view3D_BUSCAR == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return false;
            }

            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            if (!M1_2_SeleccionarElemeto()) return false;
            if (!M1_2a_AnalizarBordeViga()) return false;
            CoordenadasElementoDTO = _seleccionPtosEstriboViga.M1_5_ObtenerPtosDeElemento();
            // if (!M1_3_SeleccionarVigaElement()) return false;
            if (!M1_3b_ObtenerDatosVigaElement()) return false;
            //   M1_6_ObtenerListaIntervalos();
            //  _vigaSeleccionadoDTO = M2_OBtenerElementoREferenciaDTO();

            return true;
        }

        public bool M1_ObtenerPtoinicio_ConASignarMuro(Element _elemetSelect, XYZ _ptoseleccion)
        {
            _view3D_BUSCAR = TiposFamilia3D.Get3DBuscar(_doc);
            if (_view3D_BUSCAR == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return false;
            }

            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            if (!M1_2_SeleccionarElemeto(_elemetSelect, _ptoseleccion)) return false;
            if (!M1_2a_AnalizarBordeViga()) return false;
            CoordenadasElementoDTO = _seleccionPtosEstriboViga.M1_5_ObtenerPtosDeElemento();
            // if (!M1_3_SeleccionarVigaElement()) return false;
            if (!M1_3b_ObtenerDatosVigaElement()) return false;
            //   M1_6_ObtenerListaIntervalos();
            //  _vigaSeleccionadoDTO = M2_OBtenerElementoREferenciaDTO();

            return true;
        }

        internal TipoCaraObjeto Obtener_UbicacionBarras_Altura()
        {
            if (PtoSeleccionMouseCaraMuro.Z > CoordenadasElementoDTO._ptoCentroElemento_conrecub.Z)
                return TipoCaraObjeto.Superior;
            else
                return TipoCaraObjeto.Inferior;
        }

        internal XYZ OBtenerPtoTextoCapa()
        {
            return (_listaptoTramo[0] + _listaptoTramo[1]) / 2 + (new XYZ(0, 0, 1) * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);
        }

        internal TipoCaraObjeto Obtener_UbicacionBarras_IZqOderecha()
        {
            if (PtoSeleccionMouseCaraMuro.DistanceTo(CoordenadasElementoDTO._ptobarra1Inf_conrecub) <
                PtoSeleccionMouseCaraMuro.DistanceTo(CoordenadasElementoDTO._ptobarra2Inf_conrecub))
                return TipoCaraObjeto.Izquierdo;
            else
                return TipoCaraObjeto.Derecho;
        }

        private bool M1_2_SeleccionarElemeto()
        {
            try
            {
                DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO = new DatosConfinamientoAutoDTO()
                {
                    DiamtroEstriboMM = 8
                };


                _seleccionPtosEstriboViga = new SeleccionPtosEstriboViga_sinSeleccionBarras(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO);
                //_seleccionPtosEstriboViga = new SeleccionPtosEstriboVigaGeom(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO);
                if (!_seleccionPtosEstriboViga.M1_Ejecutar_SeleccionViga()) return false;
                PtoSeleccionMouseCentroCaraMuro6 = _seleccionPtosEstriboViga._ptoSeleccionMouseCentroCaraMuro;
                PtoSeleccionMouseCaraMuro = _seleccionPtosEstriboViga._ptoSeleccionMouseCaraMuro;
                ElemetSelect = _seleccionPtosEstriboViga._ElemetSelect;

                _PtoInicioBaseBordeViga6 = (_seleccionPtosEstriboViga._ptobarra1 + _seleccionPtosEstriboViga._ptobarra2) / 2;
                Element_pickobject_element = _seleccionPtosEstriboViga._ElemetSelect;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool M1_3_ASignarMuroElement(Element elemento)
        {
            try
            {
                //  ISelectionFilter filtroMuro = new FiltroMuro();
                var planarFrontal = elemento.ObtenerCaraSegun_IgualDireccion_MasCercamo(_view.ViewDirection);
                PtoSeleccionMouseCentroCaraMuro6 = planarFrontal.ObtenerCenterDeCara();
                PtoSeleccionMouseCaraMuro = PtoSeleccionMouseCentroCaraMuro6;

                ElemetSelect = elemento;// _doc.GetElement(ref_pickobject_element);
                Element_pickobject_element = elemento;

                if (ElemetSelect == null)
                {
                    Util.ErrorMsg($"No se puedo encontrar Muro de referencia.");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }
        private bool M1_2_SeleccionarElemeto(Element _elemetSelect, XYZ _ptoseleccion)
        {
            try
            {
                DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO = new DatosConfinamientoAutoDTO()
                {
                    DiamtroEstriboMM = 8
                };


                _seleccionPtosEstriboViga = new SeleccionPtosEstriboViga_sinSeleccionBarras(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO);
                //_seleccionPtosEstriboViga = new SeleccionPtosEstriboVigaGeom(_uiapp, _view3D_BUSCAR, _configuracionInicialEstriboDTO);
                if (!_seleccionPtosEstriboViga.M1_Ejecutar_SeleccionViga(_elemetSelect, _ptoseleccion)) return false;
                PtoSeleccionMouseCentroCaraMuro6 = _seleccionPtosEstriboViga._ptoSeleccionMouseCentroCaraMuro;
                ElemetSelect = _seleccionPtosEstriboViga._ElemetSelect;
                Element_pickobject_element = ElemetSelect;
                _PtoInicioBaseBordeViga6 = _seleccionPtosEstriboViga._ptoSeleccionMouseCentroCaraMuro;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public void M1_6a_ResetearListas()
        {
            _listaptoTramoSup = new List<XYZ>();
            _listaptoTramo = new List<XYZ>();
            _listaptoTramoInf = new List<XYZ>();
        }



        public bool M1_6_ObtenerListaIntervalos_Conviga(BarraFlexion _barraFlexion)
        {

            XYZ recubrimieto = _view.ViewDirection * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT; //con esto se obtiene los pto sobre la cara de seleccion
            XYZ pt1 = _barraFlexion.BarraFlexionTramosDTO_.P1_Revit_foot;
            XYZ pt2 = _barraFlexion.BarraFlexionTramosDTO_.P2_Revit_foot;
            try
            {
                DireccionBordeElemeto = (pt2 - pt1).Normalize();
                if (TipoCaraObjeto.Superior == _barraFlexion.Ubicacion)
                {
                    PtoInicioBaseBordeViga_ProyectadoCaraMuroHost =
                        (CoordenadasElementoDTO._ptobarra2Sup_conrecub + CoordenadasElementoDTO._ptobarra1Sup_conrecub) / 2 + (new XYZ(0, 0, 1) * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);
                }
                else
                {
                    PtoInicioBaseBordeViga_ProyectadoCaraMuroHost =
                        (CoordenadasElementoDTO._ptobarra2Inf_conrecub + CoordenadasElementoDTO._ptobarra1Inf_conrecub) / 2 - (new XYZ(0, 0, 1) * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);
                }

                _listaptoTramo.Add(pt1);
                _listaptoTramo.Add(pt2);
                _listaptoTramoSup.AddRange(_listaptoTramo);

                PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = PtoInicioBaseBordeViga_ProyectadoCaraMuroHost + recubrimieto;
                _PtoInicioBaseBordeViga6 = PtoInicioBaseBordeViga_ProyectadoCaraMuroHost.ObtenerCopia();
                PtoSeleccionMouseCentroCaraMuro6 = CoordenadasElementoDTO._ptoCentroElemento_conrecub.ObtenerCopia() + recubrimieto;
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
