
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
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.BarraV.Automatico;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.ServicioManejadorBarraH;
using ArmaduraLosaRevit.Model.BarraV.Dimensiones;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.model;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;

namespace ArmaduraLosaRevit.Model.BarraV
{

    //clase los para barras horizontales en vigas // no incido laterales  ni barras horrizontales de muro
    public class ManejadorBarraHVigaAuto : ManejadorBarraH
    {
        private readonly ArmaduraTrasformada _armaduraTrasformada;
        private SeleccionarElementosHAuto_vigas _seleccionarElementosHAuto_vigas;


        public List<RebarTraslapo_paraDubujarTraslapoDimensionDTO> listaCOntendoraFinalParaTraslapo { get; set; }
        public ManejadorBarraHVigaAuto(UIApplication uiapp, ISeleccionarNivel seleccionarNivel, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO, ArmaduraTrasformada armaduraTrasformada)
            : base(uiapp, seleccionarNivel, confiEnfierradoDTO)
        {

            IsdibujarDimension = false;
            this._armaduraTrasformada = armaduraTrasformada;
            IsdibujarDimension = false;
            listaCOntendoraFinalParaTraslapo = new List<RebarTraslapo_paraDubujarTraslapoDimensionDTO>();
            IsdibujarTraslapo = true;
            ListaDebarra_AutoTotal = new List<IbarraBase>();
        }

        public List<IbarraBase> _listaDebarra_Auto { get; private set; }
        public List<IbarraBase> ListaDebarra_AutoTotal { get; private set; }




        //2) conviga
        /// crea barras automaticamente en viga, al crear refuerzo, tanto inferior como superior
        public void CrearBArraHorizontalVigaAuto_conviga(VigaAutomatico viga, XYZ _ptoseleccion = default, Element _elemetSelect = null)
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

                _seleccionarElementosHAuto_vigas = new SeleccionarElementosHAuto_vigas(_uiapp, _configuracionInicialBarraHorizontalDTO, _DireccionRecorrido);

                if (!_seleccionarElementosHAuto_vigas.M1_ObtenerPtoinicio_ConASignarMuro(_elemetSelect, _ptoseleccion)) return;

                viga.VigaGeometriaDTO.PtosInvertirTrasformadas(_armaduraTrasformada);

                for (int i = 0; i < viga.ListaBArras.Count; i++)
                {
                    var barra = viga.ListaBArras[i];

                    barra.PtosInvertirTrasformadas(_armaduraTrasformada);

                    _configuracionInicialBarraHorizontalDTO.Inicial_Cantidadbarra = barra.BarraFlexionTramosDTO_.n_Barras.ToString();// confiEnfierrado_BArrasInferiorDTO.Inicial_Cantidadbarra;
                    _configuracionInicialBarraHorizontalDTO.incial_diametroMM = barra.BarraFlexionTramosDTO_.diametro_Barras__mm;
                    _configuracionInicialBarraHorizontalDTO.Inicial_espacienmietoCm_direccionmuro = "8";
                    _configuracionInicialBarraHorizontalDTO.incial_IsDirectriz = (barra.BarraFlexionTramosDTO_.IsConLaeader ? true : false);
                    _configuracionInicialBarraHorizontalDTO.TipoBarraRefuerzoViga = barra.BarraFlexionTramosDTO_.TipoBarraRefuerzoViga;
                    _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH = barra.inicial_tipoBarraH;
                    _configuracionInicialBarraHorizontalDTO.TipoPataIzqInf = barra.BarraFlexionTramosDTO_.TipoPataIzqInf;
                    _configuracionInicialBarraHorizontalDTO.TipoPataDereSup = barra.BarraFlexionTramosDTO_.TipoPataDereSup;
                    _configuracionInicialBarraHorizontalDTO._NUmeroLinea_paraTagRefuerzo = barra.Linea;


                    _seleccionarElementosHAuto_vigas.M1_6a_ResetearListas();
                    _seleccionarElementosHAuto_vigas.M1_6_ObtenerListaIntervalos_Conviga(barra);
                    if (!EjecutarCAlculos()) continue;
                    if (!GenerarIntervalosBarras_vigaAuto()) continue;

                    ListaDebarra_AutoTotal.Add(_listaDebarra_Auto[0]);

                    var nuevoRebarTraslapoDTO_inicio = new RebarTraslapo_paraDubujarTraslapoDimensionDTO(_listaDebarra_Auto, barra);
                    listaCOntendoraFinalParaTraslapo.Add(nuevoRebarTraslapoDTO_inicio);
                }

                XYZ DireccionEnFierrado_ = XYZ.Zero;
                if (viga.ListaBArras.Count == 0)
                    DireccionEnFierrado_ = XYZ.BasisZ;
                else if (_vigaSeleccionadoDTO != null)
                    DireccionEnFierrado_ = _vigaSeleccionadoDTO.DireccionEnFierrado;
                else
                {
                    Util.ErrorMsg($"Error al obtener 'DireccionEnFierrado' porque viga seleccionda es null. Viga id:{viga.IdentificadorViga_revit}");
                    return;
                }

                ConfiguracionTAgBarraDTo confBarraTagSindirectriz = Obtener_ConfiguracionTAgBarraDTo.Ejecutar(DireccionEnFierrado_, false, XYZ.Zero);

                // dibujar
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("CrearGrupoBarraHorizontal-NH");
                    M6_DibujarBarras_SoloBarrasVigaAuto(confBarraTagSindirectriz);

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
                _vigaSeleccionadoDTO = _seleccionarElementosHAuto_vigas.M2_OBtenerElementoREferenciaDTO();
                if (_vigaSeleccionadoDTO == null) return false;
                // tipoelemto = _seleccionarElementos_auto._elementoSeleccionado;
                _seleccionarElementos = _seleccionarElementosHAuto_vigas;
                _listaptoTramo = _seleccionarElementosHAuto_vigas._listaptoTramo;
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



        private bool GenerarIntervalosBarras_vigaAuto()
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
                    //if (!_AsignarTipoTraslapo.M3_AsignarTipoTraslapo(j)) continue;

                    var _empotramientoPatasDTO = new EmpotramientoPatasDTO()
                    {
                        _conEmpotramientoIzqInf = TipoEmpotramiento.total,
                        _conEmpotramientoDereSup = TipoEmpotramiento.total,
                        TipoPataIzqInf = _configuracionInicialBarraHorizontalDTO.TipoPataIzqInf, //(i==0? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar)
                        TipoPataDereSup = _configuracionInicialBarraHorizontalDTO.TipoPataDereSup,//((_listaptoTramo.Count - 2)==i ? _configuracionInicialBarraHorizontalDTO.inicial_tipoBarraH : TipoBarraV.NoBuscar),
                    };

                    _configuracionInicialBarraHorizontalDTO._empotramientoPatasDTO = _empotramientoPatasDTO;// _AsignarTipoTraslapo._empotramientoPatasDTO;


                    _vigaSeleccionadoDTO.soloTag1 = true;
                    for (int i = 0; i < _configuracionInicialBarraHorizontalDTO.IntervalosCantidadBArras.Length; i++)
                    {
                        //_configuracionInicialBarraHorizontalDTO.LineaBarraAnalizada = i + 1;

                        M3_AsignarCAntidadEspaciemientoNuevaLineaBarra(i, (VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras ? 0 : j), _vigaSeleccionadoDTO.TipoElementoSeleccionado);

                        IGenerarIntervalosH igenerarIntervalos = igenerarIntervalos = new GenerarIntervalosSINNivel_vigaAuto(_uiapp, _configuracionInicialBarraHorizontalDTO, selecionarPtoSup, _vigaSeleccionadoDTO);
                        GeomeTagArgs _GeomeTagArgs = GeomeTagArgs.ValorDefaul();

                        //FactoryGenerarIntervalos.CrearGeneradorDeIntervalosH(_uiapp, _configuracionInicialBarraHorizontalDTO, selecionarPtoSup, _vigaSeleccionadoDTO);
                        igenerarIntervalos.M1_ObtenerIntervaloBarrasDTO();
                        igenerarIntervalos.M2_GenerarListaBarraHorizontal();
                        _listaDebarra_Auto = igenerarIntervalos.ListaIbarraHorizontal;

                        if (igenerarIntervalos.ListaIntervaloBarrasDTO.Count == 0) continue;

                        _listaDebarra.AddRange(_listaDebarra_Auto);

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



        private void M6_DibujarBarras_SoloBarrasVigaAuto(ConfiguracionTAgBarraDTo confBarraTagSindirectriz)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CrearBArraH");
                    int i = 0;
                    foreach (var envoltorio in listaCOntendoraFinalParaTraslapo)
                    {
                        IbarraBase item = envoltorio._listaDebarra_Auto[0];

                        IbarraBaseResultDTO _resutItem = item.GetResult();

                        if (_resutItem.IsConDirectriz)
                        {
                            confBarraTagSindirectriz.IsDIrectriz = true;
                            confBarraTagSindirectriz.LeaderElbow = ObtenerDesfasePOrescala(envoltorio.BarraFlexion_.Ubicacion);
                        }
                        else
                            confBarraTagSindirectriz.IsDIrectriz = false;



                        if (!item.M1_DibujarBarra()) continue;
                        if (item.IsSoloTag)
                        {
                            item.M2_DibujarTags(confBarraTagSindirectriz);
                        }

                        _resutItem = item.GetResult();

                        ElementId idreabar = _resutItem._rebar.Id;// item.M3_ObtenerIdRebar();

                        i = 1 + i;
                        if (idreabar == null) continue;

                        _listaRebar.Add(_resutItem._rebar);
                         envoltorio.BarraFlexion_.BarraCreadoElem = _resutItem._rebar;
                        if (envoltorio.TraslaInicio != null)
                            envoltorio.TraslaInicio.BarraAnterior = _resutItem._rebar;

                        if (envoltorio.TraslaFinal != null)
                            envoltorio.TraslaFinal.BarrasPosterior = _resutItem._rebar;
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }

   
        XYZ ObtenerDesfasePOrescala(TipoCaraObjeto ubicacion)
        {
            XYZ desplaSUperior = XYZ.Zero;
            if (ubicacion == TipoCaraObjeto.Superior)
                desplaSUperior = new XYZ(0, 0, -1);


            if (_view.Scale == 50)
                return _view.RightDirection * 1.463 + desplaSUperior;
            else if (_view.Scale == 75)
                return _view.RightDirection * 2.19 + desplaSUperior;
            else if (_view.Scale == 100)
                return _view.RightDirection * 2.928 + desplaSUperior;
            else
                return _view.RightDirection * 1.4527 + desplaSUperior;
        }
    }
}
