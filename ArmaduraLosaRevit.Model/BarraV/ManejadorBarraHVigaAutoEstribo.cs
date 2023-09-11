
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.BarraV.ColorRebar;
using System.IO;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Prueba.User;

namespace ArmaduraLosaRevit.Model.BarraV
{

    //clase los para barras horizontales en vigas // no incido laterales  ni barras horrizontales de muro
    public class ManejadorBarraHVigaAutoEstribo : ManejadorBarraH
    {
        private SeleccionarElementosHAuto _seleccionarElementos_auto;

        public ManejadorBarraHVigaAutoEstribo(UIApplication uiapp, ISeleccionarNivel seleccionarNivel, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO)
            : base(uiapp, seleccionarNivel, confiEnfierradoDTO)
        {

            IsdibujarDimension = false;
        }

        /// crea barras automaticamente en viga, al crear refuerzo, tanto inferior como superior
        public void CrearBArraHorizontalVigaAutoEstribo(ConfiguracionInicialBarraHorizontalDTO confiEnfierrado_BArrasInferiorDTO, XYZ _ptoseleccion = default, Element _elemetSelect = null)
        {

            if (!_configuracionInicialBarraHorizontalDTO.IsDibujarBArra && !confiEnfierrado_BArrasInferiorDTO.IsDibujarBArra)
                return;

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR CrearBArraHorizontalTramov2 ");

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
            if (_elemetSelect == null) return;


            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return;

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            _listaRebar.Clear();
            try
            {
                if (!M1_CalculosIniciales(1) || (!Directory.Exists(ConstNH.CONST_COT))) return;
                DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(_view, DireccionRecorrido_.PerpendicularEntradoVista);

                _seleccionarElementos_auto = new SeleccionarElementosHAuto(_uiapp, _configuracionInicialBarraHorizontalDTO, _DireccionRecorrido);

                if (!_seleccionarElementos_auto.M1_ObtenerPtoinicio_ConASignarMuro(_elemetSelect, _ptoseleccion)) return;

                _configuracionInicialBarraHorizontalDTO.Inicial_Cantidadbarra = confiEnfierrado_BArrasInferiorDTO.Inicial_Cantidadbarra;
                _configuracionInicialBarraHorizontalDTO.incial_diametroMM = confiEnfierrado_BArrasInferiorDTO.incial_diametroMM;
                _configuracionInicialBarraHorizontalDTO.Inicial_espacienmietoCm_direccionmuro = confiEnfierrado_BArrasInferiorDTO.Inicial_espacienmietoCm_direccionmuro;


                //a) barra superior
                if (_configuracionInicialBarraHorizontalDTO.IsDibujarBArra)
                {
                    _seleccionarElementos_auto.M1_6a_ResetearListas();
                    _seleccionarElementos_auto.M1_6_ObtenerListaIntervalos(TipoCaraObjeto.Superior);
                    if (!EjecutarCAlculos()) return;
                    if (!GenerarIntervalosBarras()) return;
                }
                //b) barra inferior
                if (confiEnfierrado_BArrasInferiorDTO.IsDibujarBArra)
                {
                    _seleccionarElementos_auto.M1_6a_ResetearListas();
                    _seleccionarElementos_auto.M1_6_ObtenerListaIntervalos(TipoCaraObjeto.Inferior);
                    if (!EjecutarCAlculos()) return;
                    if (!GenerarIntervalosBarras()) return;
                }

                ConfiguracionTAgBarraDTo confBarraTagSindirectriz = Obtener_ConfiguracionTAgBarraDTo.Ejecutar(_vigaSeleccionadoDTO.DireccionEnFierrado, false, XYZ.Zero);

                // dibujar
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("CrearGrupoBarraHorizontal-NH");
                    M6_DibujarBarras(confBarraTagSindirectriz);

                    ColorRebarAsignar _ColorRebar = new ColorRebarAsignar(_uiapp, _listaRebar);
                    _ColorRebar.M7_CAmbiarColor();

                    transGroup.Assimilate();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            return;
        }

        private bool EjecutarCAlculos()
        {
            try
            {
                _vigaSeleccionadoDTO = _seleccionarElementos_auto.M2_OBtenerElementoREferenciaDTO();
                if (_vigaSeleccionadoDTO == null) return false;
                // tipoelemto = _seleccionarElementos_auto._elementoSeleccionado;
                _seleccionarElementos = _seleccionarElementos_auto;
                _listaptoTramo = _seleccionarElementos_auto._listaptoTramo;
                //puede ser cualquiera q no sea 'barra'
                _vigaSeleccionadoDTO.TipoElementoSeleccionado = ElementoSeleccionado.Viga;
                //_elementoSeleccionado = ElementoSeleccionado.Viga;

                for (int i = 0; i < _listaptoTramo.Count - 1; i++)
                {
                    if (_listaptoTramo[i].DistanceTo(_listaptoTramo[i + 1]) < ConstNH.CONST_LARGOMIN_BARRA_FOOT)
                        return false;
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'SeleccionarElementosHAuto'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
