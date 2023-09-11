using ArmaduraLosaRevit.Model.BarraV.Creador;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.Agrupar;
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
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using System.IO;
using ArmaduraLosaRevit.Model.BarraV.Traslapos;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.ayuda;

namespace ArmaduraLosaRevit.Model.BarraV
{
    public class ManejadorBarraV_auto
    {
        private UIApplication _uiapp;
        private UIDocument _uidoc;

        private ISeleccionarNivel _seleccionarNivel;
        private ConfiguracionIniciaWPFlBarraVerticalDTO _confiEnfierradoDTO;
        private readonly DireccionRecorrido _DireccionRecorrido;
        private View _view;

        public Document _doc { get; private set; }
        public List<ManejadorTraslapo> ListaTraslapo { get; private set; }
        public int CantidadBArrasDibujadas { get; private set; }
       

        private List<Level> _listaLevelTotal;
        private List<CreadorBarrasV_Auto> _listaCreadorBarrasVAUTO;

        public ManejadorBarraV_auto(UIApplication uiapp, ISeleccionarNivel seleccionarNivel, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO, DireccionRecorrido _DireccionRecorrido)
        {
            this._uiapp = uiapp;
            this._uidoc = uiapp.ActiveUIDocument;
            this._seleccionarNivel = seleccionarNivel;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
            this._DireccionRecorrido = _DireccionRecorrido;
            this._view = this._uidoc.ActiveView;
            this._doc = _uidoc.Document;
            //  _listaDebarra = new List<IbarraElevacion>();
            _listaLevelTotal = new List<Level>();
            _listaCreadorBarrasVAUTO = new List<CreadorBarrasV_Auto>();
            ListaTraslapo = new List<ManejadorTraslapo>();

        }

        // IsDibujarTagBArras=true; dibuja barra por barras
        // IsDibujarTagBArras = dibuja las barras agrupadas
        public void CrearBArraVErticalv2(bool IsDibujarTagBArras = true)
        {
            if (_confiEnfierradoDTO.ListaIntervaloBarraAutoDto.Count == 0) return;

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            List<CreadorBarrasV_Auto> ListaCreadorBarrasV_Auto = new List<CreadorBarrasV_Auto>();

            Stopwatch stopwatch = new Stopwatch();
            //1)  calculo
            try
            {
                if (!M1_CalculosInicialesView() || (!Directory.Exists(ConstNH.CONST_COT))) return;
                if (!M1_1_ObtenerListaNiveles()) return;
                var ListConfinaminetoGrupo = _confiEnfierradoDTO.ListaIntervaloBarraAutoDto.GroupBy(c => new { c.x_referencia_ordenar })
                                                   .OrderBy(g => g.Key.x_referencia_ordenar);
                foreach (var GrupoLineas in ListConfinaminetoGrupo)
                {
                    var listaLinea = GrupoLineas.OrderBy(c => c.z_referencia_ordenar).ToList();
                    for (int i = 0; i < listaLinea.Count; i++)
                    {

                        IntervalosBarraAutoDto newIntervaloBarraAutoDto = listaLinea[i];
                        if (newIntervaloBarraAutoDto.ListaCoordenadasBarra[0].AuxIsbarraIncial)
                        {
                            // IntervalosBarraAutoDto.moverPorTraslapo = false;
                            newIntervaloBarraAutoDto.AuxIsbarraIncial = true;
                        }
                        //
                        if (!M1_CalculosIniciales()) return;
                        //1                        
                        SeleccionarElementosV _seleccionarElementos = new SeleccionarElementosV(_uiapp, _confiEnfierradoDTO, _listaLevelTotal);

                        if (!_seleccionarElementos.M1_ObtenerPtoinicioAuto(newIntervaloBarraAutoDto.PtoBordeMuro, newIntervaloBarraAutoDto.PtoCentralSobreMuro))
                        {
                            if (UtilBarras.IsConNotificaciones)
                            {
                                //Util.ErrorMsg("Error Al Selecciona muro de referencia");
                            }
                            else
                                Debug.WriteLine("Error Al Selecciona muro de referencia");

                            continue;
                        }

                        DatosMuroSeleccionadoDTO muroSeleccionadoDTO = _seleccionarElementos.M5_OBtenerElementoREferenciaDTO(_DireccionRecorrido);
                        if (muroSeleccionadoDTO == null) return;

                        CreadorBarrasV_Auto CreadorBarrasV_Auto =
                            new CreadorBarrasV_Auto(_uiapp, _confiEnfierradoDTO, newIntervaloBarraAutoDto, muroSeleccionadoDTO, IsDibujarTagBArras);

                        ListaCreadorBarrasV_Auto.Add(CreadorBarrasV_Auto);
                    }//dibujar barra
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }

            //2) dibujar
            ListaTraslapo = new List<ManejadorTraslapo>();
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    double diamtroAnteriorMM = 0;
                    t.Start("dibujar barras-NH");
                    foreach (CreadorBarrasV_Auto item in ListaCreadorBarrasV_Auto)
                    {
                        if (item.Ejecutar(diamtroAnteriorMM))
                        {
                            _listaCreadorBarrasVAUTO.Add(item);

                            if (item._listaBarraIng.Count > 0)
                                diamtroAnteriorMM = item._listaBarraIng[0].diametroInt;

                            ListaTraslapo.Add(item._manejadorTraslapo);
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
            //   _utilStopWatch.StopYContinuar("b) Termino dibujar");


            //2) configuracion
            /*
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("dibujar Traslapo-NH");
                    foreach (var _manejadorTraslapo in ListaTraslapo)
                    {
                        _manejadorTraslapo.M4_DibujarTraslapoSinTrans();
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
            */

            //2) configuracion
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("dibujar barrasconfiguracion-NH");
                    foreach (CreadorBarrasV_Auto item in ListaCreadorBarrasV_Auto)
                    {
                        item.M4_DibujarBarrasCOnfiguracion();
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }

            // _utilStopWatch.StopYContinuar("c) tarmino paramatros internos");
            if (IsDibujarTagBArras == false)
            {

                for (int j = 0; j < ListaCreadorBarrasV_Auto.Count; j++)
                {
                    var listaBarra = ListaCreadorBarrasV_Auto[j]._listaBarraIng;
                    for (int m = 0; m < listaBarra.Count; m++)
                    {
                        listaBarra[m].ObtenerDatos(_view.Origin);
                        listaBarra[m].ObtenerDatos_CantidadDiametr0();

                    }
                }
                var listaRebar = ListaCreadorBarrasV_Auto.SelectMany(c => c._listaBarraIng).ToList();
                CantidadBArrasDibujadas = listaRebar.Count();
                ManejadorAgrupar ManejadorAgrupar = new ManejadorAgrupar(_uiapp, _listaLevelTotal);
                ManejadorAgrupar.M3_EjecutarAutomatico(listaRebar);
            }

            //   _utilStopWatch.TerminarMiliseg("e) terminar agrupar");
            //3) color
            try
            {
                M7_CAmbiarColor();
                // _uidoc.RefreshActiveView();
            }
            catch (Exception ex)
            {

                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
#if DEBUG
            //LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif
            return;
        }
        //     

        private bool M1_CalculosInicialesView()
        {
            try
            {
                View3D _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);
                if (_view3D_paraVisualizar == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial: 3D Visualizar");
                    return false;
                }
                View3D _view3D = TiposFamilia3D.Get3DBuscar(_doc);
                if (_view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial 3D Biuscar");
                    return false;
                }
                _confiEnfierradoDTO.view3D_paraBuscar = _view3D;
                _confiEnfierradoDTO.view3D_paraVisualizar = _view3D_paraVisualizar;
                _confiEnfierradoDTO.viewActual = _uidoc.Document.ActiveView; ;


                AyudaManejadorTraslapo.Reset();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error En los calculos iniciales: {ex.Message}");
                return false;
            }
            return true;
        }

        private bool M1_1_ObtenerListaNiveles()
        {
            try
            {
                _listaLevelTotal = this._seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(_view);

                if (_listaLevelTotal.Count < 2)
                {
                    Util.ErrorMsg("No se encontraron niveles visibles en la vista. Favor de agrgar o modificar 'Crop view' para que esten visibles ");
                    return false;///sin niveles no hay traslapo, esta coorecta esta linea
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erro al generar lista de niveles ex:{ex.Message}");
                return false;
            }

            return true;
        }

        private bool M1_CalculosIniciales()
        {
            try
            {
                if (!(_uidoc.Document.ActiveView is ViewSection))
                {
                    const string _instructions = "Comando debe ejecutarse en una vista VIEW SECTION";
                    Util.ErrorMsg(_instructions);
                    return false;
                }

                _confiEnfierradoDTO.M1_ObtenerIntervalosDireccionMuro();

                if (_confiEnfierradoDTO.IntervalosCantidadBArras.Length == 0) return false;
                if (_confiEnfierradoDTO.IntervalosCantidadBArras.Length == 0) return false;
                if (_confiEnfierradoDTO.IntervalosCantidadBArras.Length != _confiEnfierradoDTO.IntervalosCantidadBArras.Length) return false;

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

            if (_listaCreadorBarrasVAUTO.Count == 0) return;

            List<ElementId> _listaRebarId = _listaCreadorBarrasVAUTO.SelectMany(c => c._listaRebarId).ToList();

            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeListaElementColorConTrans(_listaRebarId, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta));
            //visibilidadElement
        }
    }
}
