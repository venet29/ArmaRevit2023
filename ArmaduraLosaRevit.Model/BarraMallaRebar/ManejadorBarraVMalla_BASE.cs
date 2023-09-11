using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Creador;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar
{
    public abstract class ManejadorBarraVMalla_BASE
    {
        protected UIApplication _uiapp;
        protected UIDocument _uidoc;
        protected Document _doc;
        protected View _view;

        protected ISeleccionarNivel _seleccionarNivel;
        protected ConfiguracionIniciaWPFlBarraVerticalDTO _confiWPFEnfierradoDTO;
        protected DireccionRecorrido _DireccionRecorrido;
        protected DatosMallasAutoDTO _datosMallasDTO;
        protected List<IbarraBase> _listaDebarra;
        protected List<Level> _listaLevelTotal;
        protected List<CreadorBarrasV> _listaCreadorBarrasV;


        public ManejadorBarraVMalla_BASE(UIApplication uiapp,
                ISeleccionarNivel seleccionarNivel)
        {
            this._uiapp = uiapp;
            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._seleccionarNivel = seleccionarNivel;

            this._view = this._uidoc.ActiveView;
            _listaDebarra = new List<IbarraBase>();
            _listaLevelTotal = new List<Level>();
            _listaCreadorBarrasV = new List<CreadorBarrasV>();
        }

        public ManejadorBarraVMalla_BASE(UIApplication uiapp,
                ISeleccionarNivel seleccionarNivel,
                ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO,
                DireccionRecorrido _DireccionRecorrido,
                DatosMallasAutoDTO datosMallasDTO)
        {
            this._uiapp = uiapp;
            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._seleccionarNivel = seleccionarNivel;
            this._confiWPFEnfierradoDTO = confiWPFEnfierradoDTO;
            this._DireccionRecorrido = _DireccionRecorrido;
            this._datosMallasDTO = datosMallasDTO;
            this._view = this._uidoc.ActiveView;
            _listaDebarra = new List<IbarraBase>();
            _listaLevelTotal = new List<Level>();
            _listaCreadorBarrasV = new List<CreadorBarrasV>();
        }

        public abstract void CrearBArra();



        protected bool M1_CalculosIniciales()
        {
            try
            {
                _confiWPFEnfierradoDTO.M1_ObtenerIntervalosDireccionMuro();
                _listaLevelTotal = this._seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(_view);


                if (_listaLevelTotal.Count < 2)
                {
                    _confiWPFEnfierradoDTO.TipoSeleccionMousePtoSuperior = TipoSeleccionMouse.mouse;
                    _confiWPFEnfierradoDTO.TipoSeleccionMousePtoInferior = TipoSeleccionMouse.mouse;
                }
                if (_confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length == 0) return false;
                if (_confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length == 0) return false;
                if (_confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length != _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length) return false;

                View3D _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);
                if (_view3D_paraVisualizar == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial : 3D VISUALIZAR");
                    return false;
                }
                View3D _view3D_ParaBuscar = TiposFamilia3D.Get3DBuscar(_doc);
                if (_view3D_ParaBuscar == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial : 3D BUSCAR");
                    return false;
                }
                _confiWPFEnfierradoDTO.view3D_paraBuscar = _view3D_ParaBuscar;
                _confiWPFEnfierradoDTO.view3D_paraVisualizar = _view3D_paraVisualizar;
                _confiWPFEnfierradoDTO.viewActual = _uidoc.Document.ActiveView;


                // ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(_uiapp);
                // _ManejadorVisibilidadElemenNoEnView.Ejecutar(_confiWPFEnfierradoDTO.view3D_paraBuscar, _view.Name);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error En los calculos iniciales: {ex.Message}");
                return false;
            }
            return true;
        }



        protected bool RecalcularEspaciamientoLineasBarrasVertical(SeleccionarElementosV _seleccionarElementos, int factorDesplazamiento)
        {

            try
            {
                if (_confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length == 1)
                {

                    _confiWPFEnfierradoDTO.IntervalosEspaciamiento[0] = Util.FootToCm(_seleccionarElementos._espesorMuroFoot)/2 - (_confiWPFEnfierradoDTO.inicial_diametroMM / 10.0f);
                    return true;
                }
                else
                {
                    double espacimientoEntreLinea = //obs1)
                    (Util.FootToCm(_seleccionarElementos._espesorMuroFoot)
                    - ConstNH.RECUBRIMIENTO_BARRA_MALLA_CM * 2
                    - (_confiWPFEnfierradoDTO.inicial_diametroMM / 10.0f) * factorDesplazamiento) /
                    (_confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length - 1);

                    double sumadeltaEspesor = 0;
                    bool soloUnavez = true;
                    for (int i = 0; i < _confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length; i++)
                    {
                        if (i == 0)
                            _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea;
                        else if (i == (_confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length - 1))
                            _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea - sumadeltaEspesor;
                        else
                        {
                            if (soloUnavez)
                            {
                                soloUnavez = false;
                                sumadeltaEspesor += _datosMallasDTO.diametroH_mm / 10f;
                                _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea + sumadeltaEspesor;// + _datosMallasDTO.diametroH*2/10;
                            }
                            else
                            {

                                _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea - sumadeltaEspesor * 2;
                                sumadeltaEspesor -= _datosMallasDTO.diametroH_mm / 10f;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        protected bool RecalcularEspaciamientoLineasBarrasHorizontal(double _espesorMuroFoot, int factorDesplazamiento)
        {

            try
            {
                int divisorLineas = (_confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length == 1
                                    ? 1
                                    : (_confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length - 1));

                double espacimientoEntreLinea = //obs1)
                (Util.FootToCm(_espesorMuroFoot)
                - ConstNH.RECUBRIMIENTO_BARRA_MALLA_CM * 2
                - (_confiWPFEnfierradoDTO.inicial_diametroMM / 10.0f) * factorDesplazamiento) / divisorLineas;

                for (int i = 0; i < _confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length; i++)
                    _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        protected void M7_CAmbiarColor(TipoCOlores _color = TipoCOlores.ParaMalla, bool _Halftone = true)
        {
            try
            {
                if (_listaCreadorBarrasV.Count == 0) return;

                List<ElementId> _listaRebarId = _listaCreadorBarrasV.SelectMany(c => c._listaRebar).Where(ba => ba.IsValidObject).Select(r => r.Id).ToList();
                VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
                visibilidadElement.ChangeListaElementColorConTrans(_listaRebarId, FactoryColores.ObtenerColoresPorNombre(_color), _Halftone);
                //visibilidadElement
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al cambiar color.  ex:{ex.Message}  ");
            }
        }
    }
}
