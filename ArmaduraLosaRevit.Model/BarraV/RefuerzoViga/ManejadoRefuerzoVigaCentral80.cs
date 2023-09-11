
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
using ArmaduraLosaRevit.Model.BarraV.TipoTagH.Ayuda;

namespace ArmaduraLosaRevit.Model.BarraV
{

    //clase los para barras horizontales // no incido laterales  ni barras horrizontales de muro
    public class ManejadoRefuerzoVigaCentral80 : ManejadorBarraH
    {
        private SeleccionarElementosHAuto _seleccionarElementos_auto;

        public ManejadoRefuerzoVigaCentral80(UIApplication uiapp, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO)
            : base(uiapp, null, confiEnfierradoDTO)
        {

            IsdibujarDimension = false;
        }

        /// crea barras automaticamente en viga, al crear refuerzo, tanto inferior como superior
        public void CrearBArraHorizontal()
        {

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


            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return;

            _listaRebar.Clear();
            try
            {
                if (!M1_CalculosIniciales(1) || (!Directory.Exists(ConstNH.CONST_COT))) return;
                DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(_view, DireccionRecorrido_.PerpendicularEntradoVista);

                _configuracionInicialBarraHorizontalDTO.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.RefuerzoCentral;

                _seleccionarElementos_auto = new SeleccionarElementosHAuto(_uiapp, _configuracionInicialBarraHorizontalDTO, _DireccionRecorrido);

                if (!_seleccionarElementos_auto.M1_ObtenerPtoinicio_RefuerzoBorde()) return;


                var UbicacionBarras = _seleccionarElementos_auto.Obtener_UbicacionBarras_Altura();

                XYZ _ptoseleccion = _seleccionarElementos_auto.PtoSeleccionMouseCentroCaraMuro6;
                Element _elemetSelect = _seleccionarElementos_auto.ElemetSelect;

                //a) barra superior
                if (_configuracionInicialBarraHorizontalDTO.IsDibujarBArra)
                {
                    _seleccionarElementos_auto.M1_6_ObtenerListaIntervalos(UbicacionBarras);
                    if (!EjecutarCAlculos()) return;
                    if(!GenerarIntervalosBarras_paraREfurzo()) return;
                }
                else
                    return;

                ConfiguracionTAgBarraDTo confBarraTagSindirectriz = Obtener_ConfiguracionTAgBarraDTo.Ejecutar(_vigaSeleccionadoDTO.DireccionEnFierrado, true, AyudaGeomtria.MoverSOloRefuerzoManual_porEscala(_vigaSeleccionadoDTO.DireccionEnFierrado,_view));// _view.RightDirection * (1 + Util.CmToFoot(13.8)));
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

                XYZ p1 = _seleccionarElementos_auto._listaptoTramo[0];
                XYZ p2 = _seleccionarElementos_auto._listaptoTramo[1];
                double LArgo = p1.DistanceTo(p2);
                XYZ direc_p1_toP2 = (p2 - p1).Normalize();
                _listaptoTramo = _seleccionarElementos_auto._listaptoTramo;
                _listaptoTramo[0] = _listaptoTramo[0] + direc_p1_toP2 * LArgo * 0.1;
                _listaptoTramo[1] = _listaptoTramo[1] - direc_p1_toP2 * LArgo * 0.1;
                //puede ser cualquiera q no sea 'barra'
                _vigaSeleccionadoDTO.TipoElementoSeleccionado = ElementoSeleccionado.Viga;
                //_elementoSeleccionado = ElementoSeleccionado.Viga;

                if (_listaptoTramo[0].DistanceTo(_listaptoTramo[1]) < ConstNH.CONST_LARGOMIN_BARRA_FOOT )
                    return false;

       
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'ManejadoRefuerzoVigaCentral'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
