
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
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.ServicioManejadorBarraH;

namespace ArmaduraLosaRevit.Model.BarraV
{

    //clase los para barras horizontales // no incido laterales  ni barras horrizontales de muro
    public class ManejadoRefuerzoVigaBorde : ManejadorBarraH
    {


        private SeleccionarElementos_RefuerzoBorde _seleccionarElementos_RefuerzoBorde;

        public ManejadoRefuerzoVigaBorde(UIApplication uiapp, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO)
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

                _configuracionInicialBarraHorizontalDTO.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.RefuerzoBorde;

                _seleccionarElementos_RefuerzoBorde = new SeleccionarElementos_RefuerzoBorde(_uiapp, _configuracionInicialBarraHorizontalDTO, _DireccionRecorrido);
                if (!_seleccionarElementos_RefuerzoBorde.M1_ObtenerPtoinicio_RefuerzoBorde()) return;

                TipoCaraObjeto _ubicacionBarras = _seleccionarElementos_RefuerzoBorde.UbicacionBarras;

                XYZ _ptoseleccion = _seleccionarElementos_RefuerzoBorde.PtoSeleccionMouseCentroCaraMuro6;
                Element _elemetSelect = _seleccionarElementos_RefuerzoBorde.ElemetSelect;

                //a) barra superior
                if (_configuracionInicialBarraHorizontalDTO.IsDibujarBArra)
                {
                    _seleccionarElementos_RefuerzoBorde.M1_6_ObtenerListaIntervalos();
                    if (!EjecutarCAlculos_RefuerzoVigaBordev2()) return;
                    if (!GenerarIntervalosBarras_paraREfurzo()) return;
                }
                else
                    return;

                ConfiguracionTAgBarraDTo confBarraTagSindirectriz = Obtener_ConfiguracionTAgBarraDTo.Ejecutar(_vigaSeleccionadoDTO.DireccionEnFierrado, true, AyudaGeomtria.MoverSOloRefuerzoManual_porEscala(_vigaSeleccionadoDTO.DireccionEnFierrado, _view));
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
                _vigaSeleccionadoDTO = _seleccionarElementos_RefuerzoBorde.M2_OBtenerElementoREferenciaDTO();
                if (_vigaSeleccionadoDTO == null) return false;
                // tipoelemto = _seleccionarElementos_auto._elementoSeleccionado;
                // _seleccionarElementos = _SeleccionarElementos2_refuerzo2vigas.;

                XYZ p1 = _seleccionarElementos_RefuerzoBorde._listaptoTramo[0];
                XYZ p2 = _seleccionarElementos_RefuerzoBorde._listaptoTramo[1];
                double largo1 = _seleccionarElementos_RefuerzoBorde.Largo1;
                //  double largo2 = _seleccionarElementos_RefuerzoBorde.Largo2;
                _listaptoTramo = _seleccionarElementos_RefuerzoBorde._listaptoTramo;
                //double LArgo = p1.DistanceTo(p2);
                // XYZ direc_p1_toP2 = (p2 - p1).Normalize();
                _listaptoTramo[0] = _listaptoTramo[0];// - direc_p1_toP2 * largo1 * 0.25;
                _listaptoTramo[1] = _listaptoTramo[1];// + direc_p1_toP2 * largo1 * 0.25;
                //puede ser cualquiera q no sea 'barra'
                _vigaSeleccionadoDTO.TipoElementoSeleccionado = ElementoSeleccionado.Viga;
                //_elementoSeleccionado = ElementoSeleccionado.Viga;

                //if (_listaptoTramo[0].DistanceTo(_listaptoTramo[1]) < ConstNH.CONST_LARGOMIN_BARRA_FOOT)
                //    return false;


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'ManejadoRefuerzoVigaBorde'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool EjecutarCAlculos_RefuerzoVigaBorde()
        {
            try
            {
                _vigaSeleccionadoDTO = _seleccionarElementos_RefuerzoBorde.M2_OBtenerElementoREferenciaDTO();
                if (_vigaSeleccionadoDTO == null) return false;
                // tipoelemto = _seleccionarElementos_auto._elementoSeleccionado;
                // _seleccionarElementos = _SeleccionarElementos2_refuerzo2vigas.;

                XYZ p1 = _seleccionarElementos_RefuerzoBorde._listaptoTramo[0];
                XYZ p2 = _seleccionarElementos_RefuerzoBorde._listaptoTramo[1];
                double largo1 = _seleccionarElementos_RefuerzoBorde.Largo1;
                //  double largo2 = _seleccionarElementos_RefuerzoBorde.Largo2;
                double delta = 0; // si 1/4 viga menor q largo de desarrollo no se considera

                _listaptoTramo = _seleccionarElementos_RefuerzoBorde._listaptoTramo;
                double LArgoBarraEMpotramiento = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_configuracionInicialBarraHorizontalDTO.incial_diametroMM);
                double largoViga = _seleccionarElementos_RefuerzoBorde._seleccionarElementos1_auto.LargoViga;
                XYZ direc_p1_toP2 = _vigaSeleccionadoDTO.DireccionMuro;

                delta = largoViga * 0.25 - LArgoBarraEMpotramiento;
                delta = (delta < 0 ? 0 : delta);

                XYZ deltaDesplazmiento = XYZ.Zero;// BasisZ * (Util.CmToFoot(8) - Util.MmToFoot(_configuracionInicialBarraHorizontalDTO.incial_diametroMM / 2));
                if (_seleccionarElementos_RefuerzoBorde.UbicacionBarras == TipoCaraObjeto.Superior)
                {
                    _listaptoTramo[0] = _listaptoTramo[0] - deltaDesplazmiento;
                    _listaptoTramo[1] = _listaptoTramo[1] - deltaDesplazmiento;
                }
                else
                {
                    _listaptoTramo[0] = _listaptoTramo[0] + deltaDesplazmiento;
                    _listaptoTramo[1] = _listaptoTramo[1] + deltaDesplazmiento;
                }




                if (_seleccionarElementos_RefuerzoBorde.UbicacionBarrasLado == TipoCaraObjeto.Derecho)
                {

                    _listaptoTramo[0] = _listaptoTramo[0] - direc_p1_toP2 * delta;
                    _listaptoTramo[1] = _listaptoTramo[1];// + direc_p1_toP2 * largo1 * 0.25;
                }
                else
                {

                    _listaptoTramo[0] = _listaptoTramo[0];// - direc_p1_toP2 * largo1 * 0.25;
                    _listaptoTramo[1] = _listaptoTramo[1] + direc_p1_toP2 * delta;
                }
                //puede ser cualquiera q no sea 'barra'
                _vigaSeleccionadoDTO.TipoElementoSeleccionado = ElementoSeleccionado.Viga;
                //_elementoSeleccionado = ElementoSeleccionado.Viga;

                //if (_listaptoTramo[0].DistanceTo(_listaptoTramo[1]) < ConstNH.CONST_LARGOMIN_BARRA_FOOT)
                //    return false;


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'ManejadoRefuerzoVigaBorde'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool EjecutarCAlculos_RefuerzoVigaBordev2()
        {
            try
            {
                _vigaSeleccionadoDTO = _seleccionarElementos_RefuerzoBorde.M2_OBtenerElementoREferenciaDTO();
                if (_vigaSeleccionadoDTO == null) return false;
                // tipoelemto = _seleccionarElementos_auto._elementoSeleccionado;
                // _seleccionarElementos = _SeleccionarElementos2_refuerzo2vigas.;

                // XYZ p1 = _seleccionarElementos_RefuerzoBorde._listaptoTramo[0];
                //XYZ p2 = _seleccionarElementos_RefuerzoBorde._listaptoTramo[1];
               // double largo1 = _seleccionarElementos_RefuerzoBorde.Largo1;
                //  double largo2 = _seleccionarElementos_RefuerzoBorde.Largo2;


                _listaptoTramo = _seleccionarElementos_RefuerzoBorde._listaptoTramo;
                double LArgoBarraEMpotramiento = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_configuracionInicialBarraHorizontalDTO.incial_diametroMM);
                double largoViga = _seleccionarElementos_RefuerzoBorde._seleccionarElementos1_auto.LargoViga;
                XYZ direc_p1_toP2 = _vigaSeleccionadoDTO.DireccionMuro;

                double delta = 0; // si 1/4 viga menor q largo de desarrollo no se considera
                delta = largoViga * 0.25 - LArgoBarraEMpotramiento;
                delta = (delta < 0 ? 0 : delta);

                XYZ deltaDesplazmiento = XYZ.Zero;// BasisZ * (Util.CmToFoot(8) - Util.MmToFoot(_configuracionInicialBarraHorizontalDTO.incial_diametroMM / 2));

                if (_seleccionarElementos_RefuerzoBorde.UbicacionBarrasLado == TipoCaraObjeto.Derecho)
                {

                    _listaptoTramo[0] = _listaptoTramo[0] - direc_p1_toP2 * delta;
                    _listaptoTramo[1] = _listaptoTramo[1];// + direc_p1_toP2 * largo1 * 0.25;
                }
                else
                {

                    _listaptoTramo[0] = _listaptoTramo[0];// - direc_p1_toP2 * largo1 * 0.25;
                    _listaptoTramo[1] = _listaptoTramo[1] + direc_p1_toP2 * delta;
                }
                //puede ser cualquiera q no sea 'barra'
                _vigaSeleccionadoDTO.TipoElementoSeleccionado = ElementoSeleccionado.Viga;
                //_elementoSeleccionado = ElementoSeleccionado.Viga;

                //if (_listaptoTramo[0].DistanceTo(_listaptoTramo[1]) < ConstNH.CONST_LARGOMIN_BARRA_FOOT)
                //    return false;


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'ManejadoRefuerzoVigaBorde'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        // extenson oara caso  GenerarIntervalosBarras
        protected bool GenerarIntervalosBarras_RefuerzoVigaBorde()
        {
            try
            {
                //3 seleccionar segundo pto
                for (int j = 0; j < _listaptoTramo.Count - 1; j++)
                {
                    SelecionarPtoHorizontal selecionarPtoSup = M4_SeleccionarSegundoPtoHorizontalTramo(_listaptoTramo[j], _listaptoTramo[j + 1]);
                    if (selecionarPtoSup == null) return false;
                    //if (selecionarPtoSup.VerificarPtos()) return;

                    XYZ _PtoInicioSobrePLanodelViga_aux = _vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;

                    AsignarTipoTraslapo _AsignarTipoTraslapo = new AsignarTipoTraslapo(_uiapp, _configuracionInicialBarraHorizontalDTO, _ubicacionTraslapo, _listaptoTramo);


                    if (_seleccionarElementos_RefuerzoBorde.UbicacionBarrasLado == TipoCaraObjeto.Derecho)
                    {
                        if (!_AsignarTipoTraslapo.M3_AsignarTipoTraslapo_haciaDere(j)) continue;
                    }
                    else
                    {
                        if (!_AsignarTipoTraslapo.M3_AsignarTipoTraslapo_haciaIzq(j)) continue;

                    }

                    _configuracionInicialBarraHorizontalDTO._empotramientoPatasDTO = _AsignarTipoTraslapo._empotramientoPatasDTO;


                    _vigaSeleccionadoDTO.soloTag1 = true;
                    for (int i = 0; i < _configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras.Length; i++)
                    {
                        _configuracionInicialBarraHorizontalDTO.LineaBarraAnalizada = i + 1;


                        M3_AsignarCAntidadEspaciemientoNuevaLineaBarra(i, (VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras ? 0 : j), _vigaSeleccionadoDTO.TipoElementoSeleccionado);

                        IGenerarIntervalosH igenerarIntervalos =
                            FactoryGenerarIntervalos.CrearGeneradorDeIntervalosH(_uiapp, _configuracionInicialBarraHorizontalDTO, selecionarPtoSup, _vigaSeleccionadoDTO);
                        igenerarIntervalos.M1_ObtenerIntervaloBarrasDTO();
                        igenerarIntervalos.M2_GenerarListaBarraHorizontal();
                        var aux_listaDebarra = igenerarIntervalos.ListaIbarraHorizontal;

                        if (igenerarIntervalos.ListaIntervaloBarrasDTO.Count == 0) continue;

                        _listaDebarra.AddRange(aux_listaDebarra);

                        _listaDimensiones.Add(new PtoDimesionesTraslapoDTO()
                        {
                            P1 = igenerarIntervalos.ListaIntervaloBarrasDTO[0].ptoini,
                            P2 = igenerarIntervalos.ListaIntervaloBarrasDTO[0].ptofinal
                        });
                        _vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = _PtoInicioSobrePLanodelViga_aux;
                        _vigaSeleccionadoDTO.soloTag1 = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al calcularIntervalos en 'GenerarIntervalosBarras'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


    }
}
