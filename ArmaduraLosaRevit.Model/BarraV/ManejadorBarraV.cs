using ArmaduraLosaRevit.Model.BarraV.Creador;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.Traslapos;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Prueba.User;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV
{
    public class ManejadorBarraV
    {
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Document _doc;
        private View _view;

        private ISeleccionarNivel _seleccionarNivel;
        private ConfiguracionIniciaWPFlBarraVerticalDTO _confiWPFEnfierradoDTO;
        private readonly DireccionRecorrido _DireccionRecorrido;
        private List<IbarraBase> _listaDebarra;
        private List<Level> _listaLevelTotal;
        private List<CreadorBarrasV> _listaCreadorBarrasV;
        private List<Rebar> _listaRebar;

        
        public ManejadorBarraV(UIApplication uiapp, ISeleccionarNivel seleccionarNivel, ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO, DireccionRecorrido _DireccionRecorrido)
        {
            this._uiapp = uiapp;
            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._seleccionarNivel = seleccionarNivel;
            this._confiWPFEnfierradoDTO = confiWPFEnfierradoDTO;
            this._DireccionRecorrido = _DireccionRecorrido;
            this._view = this._uidoc.ActiveView;
            _listaDebarra = new List<IbarraBase>();
            _listaLevelTotal = new List<Level>();
            _listaCreadorBarrasV = new List<CreadorBarrasV>();   
        }
 
        public void CrearBArraVErtical()
        {

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR CrearBArraVErtical ");

            if (!resultadoConexion)
            {
                Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                return;
            }
            else if (!_ManejadorUsuarios.ObteneResultado())
            {
                Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                return;
            }

            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return ;

            //UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
          
            CreadorBarrasV creadorBarrasV = null;
            try
            {
                //
                if (!M1_CalculosIniciales() || (!Directory.Exists(ConstNH.CONST_COT))) return;
                //1
                SeleccionarElementosV _seleccionarElementos = new SeleccionarElementosV(_uiapp, _confiWPFEnfierradoDTO, _listaLevelTotal);
                if (!_seleccionarElementos.M1_ObtenerPtoinicio())
                {
                    if (UtilBarras.IsConNotificaciones)
                    {
                       // Util.ErrorMsg("Error Al Selecciona muro de referencia");
                    }
                    else
                        Debug.WriteLine("Error Al Selecciona muro de referencia");
                    return;
                }

                DatosMuroSeleccionadoDTO muroSeleccionadoDTO = default;

                if(DatosDiseño.IsMuroParaleloView) // caso normal
                    muroSeleccionadoDTO = _seleccionarElementos.M5_OBtenerElementoREferenciaDTO(_DireccionRecorrido);
                else // caso especial muro entrando en view
                    muroSeleccionadoDTO = _seleccionarElementos.M5_OBtenerElementoREferenciaDTO_MuroPerpendicualeView(_DireccionRecorrido);

                if (muroSeleccionadoDTO == null) return;

                //2nn
                if (!_seleccionarElementos.M2_SeleccionarPtoInicio()) return;

                XYZ _PtoInicioSobrePLanodelMuro_aux = muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;
                bool intercalar = true;
                for (int i = 0; i < _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length; i++)
                {
                    using (TransactionGroup transGroup = new TransactionGroup(_doc))
                    {
                        transGroup.Start("CrearGrupoBarraVertical-NH");

                        if (intercalar)
                        {
                            intercalar = _confiWPFEnfierradoDTO.inicial_ISIntercalar;
                            _confiWPFEnfierradoDTO.LineaBarraAnalizada = i + 1; 
                        }

                        SelecionarPtoSup selecionarPtoSup = _seleccionarElementos.M4_SeleccionarPtoSuperiorLineaBarras();
                        if (selecionarPtoSup == null) continue;
                        if (selecionarPtoSup.ListaLevelIntervalo.Count == 0)
                        {
                            Util.ErrorMsg("Error en la seleccion de nivel. Creacion de barrar finalizada");
                            return;
                        }

                        creadorBarrasV = new CreadorBarrasV(_uiapp, selecionarPtoSup, _confiWPFEnfierradoDTO, muroSeleccionadoDTO);
                        creadorBarrasV.Ejecutar(i);
                       
                        _listaCreadorBarrasV.Add(creadorBarrasV);
                        M7_CAmbiarColor();

        
                        transGroup.Assimilate();
                    }

                    _listaCreadorBarrasV.Clear();
                    //NOTA SOLUCION PARCHE se utiliza en M1_2_BuscarPtoInicioBase()  en IntervaloBarrasDTO.cs
                    muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = _PtoInicioSobrePLanodelMuro_aux;
                }
            }
            catch (Exception ex)
            {
                //TaskDialog.Show("Error", "Error al crear barra Vertical:" + ex.Message);
            }

           // UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
#if DEBUG
            //LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif
            return;
        }

        private bool M1_CalculosIniciales()
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
                View3D _view3D = TiposFamilia3D.Get3DBuscar(_doc);
                if (_view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial : 3D BUSCAR");
                    return false;
                }
                _confiWPFEnfierradoDTO.view3D_paraBuscar = _view3D;
                _confiWPFEnfierradoDTO.view3D_paraVisualizar = _view3D_paraVisualizar;
                _confiWPFEnfierradoDTO.viewActual = _uidoc.Document.ActiveView;

                AyudaManejadorTraslapo.Reset();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error En los calculos iniciales: {ex.Message}");
                return false;
            }
            return true;
        }

        private void M7_CAmbiarColor()
        {

            if (_listaCreadorBarrasV.Count == 0) return;
            _listaRebar = _listaCreadorBarrasV.SelectMany(c => c._listaRebar).ToList();
            ColorearBarrasMenor12mt_magenta();

            VerificarMayores12mt();
            //visibilidadElement
        }

        private void ColorearBarrasMenor12mt_magenta()
        {

            List<ElementId> _listaRebarId = _listaRebar
                                   .Where(c => c.IsValidObject && c.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble() < Util.CmToFoot(1200))
                                   .Select(r => r.Id).ToList();
            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeListaElementColorConTrans(_listaRebarId, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta));
        }

        private void VerificarMayores12mt()
        {
            List<ElementId> _listaRebarId = _listaRebar
                      .Where(c => c.IsValidObject && c.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble() > Util.CmToFoot(1200))
                      .Select(r => r.Id).ToList();

            VisibilidadElementRebarLosa visibilidadElement = new VisibilidadElementRebarLosa(_uiapp);
            visibilidadElement.ChangeListaElementColorConTrans(_listaRebarId, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.amarillo), false);

            //visibilidadElement
        }
    }
}
