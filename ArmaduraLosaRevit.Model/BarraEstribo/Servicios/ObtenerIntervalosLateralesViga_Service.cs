using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Servicios
{
    public class ObtenerIntervalosLateralesViga_Service
    {
        private UIApplication _uiapp;
        private  View3D _view3D_ParaBuscar;
        private  DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO;
        private  XYZ _ptobarra1;
        private  XYZ _ptobarra2;
        private  View _view;
        private double _alturaEstribo;
        private double _alturaEstriboMedia;
        private XYZ _direccionPata;
        private List<double> _espaciamietoLat;
        private double _espaciamientoBase;
        private double _diametroEstribo_foot;
        private double _diametroLat_foot;
        private bool _IsExtenderLatInicio;
        private bool _IsExtenderLatFin;
        private XYZ _direccionBarra;
        private string _textoLat;
        private ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO;
        private SeleccionarElementosH _seleccionarElementos;

        public ObtenerIntervalosLateralesViga_Service(UIApplication _uiapp, View3D view3D_paraBuscar, DatosConfinamientoAutoDTO _configuracionInicialEstriboDTO, XYZ _ptobarra1, XYZ _ptobarra2, View _view)
        {
            this._uiapp = _uiapp;
            this._view3D_ParaBuscar = view3D_paraBuscar;
            this._configuracionInicialEstriboDTO = _configuracionInicialEstriboDTO;
            this._ptobarra1 = _ptobarra1;
            this._ptobarra2 = _ptobarra2;
            this._view = _view;
            _alturaEstribo = Math.Abs(_ptobarra1.Z - _ptobarra2.Z) + ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT * 2;
            _alturaEstriboMedia = _alturaEstribo / 2;
            _direccionPata= XYZ.Zero;
            _espaciamietoLat = new List<double>();
            _espaciamientoBase = Util.CmToFoot(22.5);
            _diametroEstribo_foot = Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM);
            _diametroLat_foot = Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM) / 2;
            _IsExtenderLatInicio = _configuracionInicialEstriboDTO.IsExtenderLatInicio;
            _IsExtenderLatFin = _configuracionInicialEstriboDTO.IsExtenderLatFin;
            this._direccionBarra = (_ptobarra2 - _ptobarra1).AsignarZ(0).Normalize();
        }

        public List<BarraLateralesDTO> M3_ObtenerLateralesEstriboVigaDTO()
        {
            List<BarraLateralesDTO> list = new List<BarraLateralesDTO>();

            List<double> listaIntervalo = (_configuracionInicialEstriboDTO.cantidadLaterales == 0
                                                ? ObtenerListaEspacimientoLateralesAuto()
                                                : ObtenerListaEspacimientoLateralesNumero());

            double ProlonInicial = (_IsExtenderLatInicio ? UtilBarras.largo_L9_DesarrolloFoot_diamMM(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM) : 0);
            double ProlonFinal = (_IsExtenderLatFin ? UtilBarras.largo_L9_DesarrolloFoot_diamMM(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM) : 0);



            for (int i = 0; i < listaIntervalo.Count; i++)
            {
                BarraLateralesDTO barraLateralesDTO = new BarraLateralesDTO()
                {

                    StartPoint_ = _ptobarra1 - _view.ViewDirection * (_diametroEstribo_foot + _diametroLat_foot) + new XYZ(0, 0, listaIntervalo[i]) - _direccionBarra * ProlonInicial,
                    EndPoint_ = _ptobarra2.AsignarZ(_ptobarra1.Z) - _view.ViewDirection * (_diametroEstribo_foot + _diametroLat_foot) + new XYZ(0, 0, listaIntervalo[i]) + _direccionBarra * ProlonFinal,
                    TipoLateral = TipoPataBarra.BarraVSinPatas,
                    DiamtroLat = _configuracionInicialEstriboDTO.DiamtroLateralEstriboMM,
                    TextoLat = _textoLat
                };

                list.Add(barraLateralesDTO);
            }

            return list;
        }

        public List<BarraLateralesDTO> M3_ObtenerLateralesEstriboVigaDTO_buscarPAta()
        {
            List<BarraLateralesDTO> list = new List<BarraLateralesDTO>();

            List<double> listaIntervalo = (_configuracionInicialEstriboDTO.cantidadLaterales == 0
                                                ? ObtenerListaEspacimientoLateralesAuto()
                                                : ObtenerListaEspacimientoLateralesNumero());

            double ProlonInicial = (_IsExtenderLatInicio ? UtilBarras.largo_L9_DesarrolloFoot_diamMM(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM) : 0);
            double ProlonFinal = (_IsExtenderLatFin ? UtilBarras.largo_L9_DesarrolloFoot_diamMM(_configuracionInicialEstriboDTO.DiamtroLateralEstriboMM) : 0);



            ConfiguracionBarraLateralDTO _ConfiguracionBarraLateralDTO = M2_2_1_ObtenerDatosParaTraba();



            //a
            confiEnfierradoDTO = new ConfiguracionInicialBarraHorizontalDTO()
            {
                incial_diametroMM =(int) Util.FootToMm(_diametroLat_foot),
                Inicial_Cantidadbarra = "2",
                incial_ComoIniciarTraslapo_LineaPAr = 1,
                incial_ComoIniciarTraslapo_LineaImpar = 2,//barra incio, barra mas al borde del muro
                incial_ComoTraslapo = 2,

                inicial_tipoBarraH = TipoPataBarra.BarraVPataAUTO,
                incial_IsDirectriz = false,
                incial_ISIntercalar = false,
                Inicial_espacienmietoCm_direccionmuro = "8",
                BarraTipo = TipoRebar.ELEV_BA_H,
                DireccionTraslapoH_ = DireccionTraslapoH.central,// DireccionTraslapoH.izquierda
                TipoSelecion = TipoSeleccion.ConElemento
            };


            for (int i = 0; i < listaIntervalo.Count; i++)
            {

                XYZ PtoInicial = _ptobarra1 - _view.ViewDirection * (_diametroEstribo_foot + _diametroLat_foot) + new XYZ(0, 0, listaIntervalo[i]) - _direccionBarra * ProlonInicial;
                XYZ PtoFinal = _ptobarra2.AsignarZ(_ptobarra1.Z) - _view.ViewDirection * (_diametroEstribo_foot + _diametroLat_foot) + new XYZ(0, 0, listaIntervalo[i]) + _direccionBarra * ProlonFinal;
      

                SelecionarPtoHorizontal selecionarPtoSup = M4_SeleccionarSegundoPtoHorizontalTramo(PtoInicial, PtoFinal);


                DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(_view, DireccionRecorrido_.PerpendicularEntradoVista);

                _seleccionarElementos = new SeleccionarElementosH(_uiapp, confiEnfierradoDTO, _DireccionRecorrido);
                _seleccionarElementos.PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = PtoInicial;
                var _vigaSeleccionadoDTO = _seleccionarElementos.M2_OBtenerElementoREferenciaDTO();

                var lateral = new GenerarIntervalosSINNivel(_uiapp, confiEnfierradoDTO, selecionarPtoSup, _vigaSeleccionadoDTO);
                lateral.M1_ObtenerIntervaloBarrasDTO();
                var result = lateral.ListaIntervaloBarrasDTO[0];


               // XYZ ptoFinal = default;
                XYZ EndPataFinalAux = default;
                XYZ EndPataInicialAux = default;

                if (result.tipobarraV == TipoPataBarra.BarraVPataAmbos)
                {
                    EndPataInicialAux = result.ptoini + _direccionPata * (_IsExtenderLatInicio ? UtilBarras.largo_L9_DesarrolloFoot_diamFoot(_diametroLat_foot) : 0);
                    PtoInicial = result.ptoini;
                    PtoFinal = result.ptofinal;
                    EndPataFinalAux = result.ptofinal + _direccionPata * (_IsExtenderLatFin ? UtilBarras.largo_L9_DesarrolloFoot_diamFoot(_diametroLat_foot) : 0);
                }
                else if (result.tipobarraV == TipoPataBarra.BarraVPataFinal)
                {
                    PtoInicial = PtoInicial - _direccionBarra * (_IsExtenderLatInicio ? UtilBarras.largo_L9_DesarrolloFoot_diamFoot(_diametroEstribo_foot) : 0);

                    PtoFinal = result.ptofinal;
                    EndPataFinalAux = PtoFinal + _direccionPata * (_IsExtenderLatFin ? UtilBarras.largo_L9_DesarrolloFoot_diamFoot(_diametroLat_foot) : 0);
                }
                else if (result.tipobarraV == TipoPataBarra.BarraVPataInicial)
                {
                    EndPataInicialAux = result.ptoini + _direccionPata * (_IsExtenderLatInicio ? UtilBarras.largo_L9_DesarrolloFoot_diamFoot(_diametroLat_foot) : 0);
                    PtoInicial = result.ptoini;

                    PtoFinal = PtoFinal + _direccionBarra * (_IsExtenderLatFin ? UtilBarras.largo_L9_DesarrolloFoot_diamFoot(_diametroEstribo_foot) : 0);

                }
                else
                {
                    PtoInicial = PtoInicial - _direccionBarra * (_IsExtenderLatInicio ? UtilBarras.largo_L9_DesarrolloFoot_diamFoot(_diametroEstribo_foot) : 0);

                    PtoFinal = PtoFinal + _direccionBarra * (_IsExtenderLatFin ? UtilBarras.largo_L9_DesarrolloFoot_diamFoot(_diametroEstribo_foot) : 0); 
                }



                BarraLateralesDTO barraLateralesDTO = new BarraLateralesDTO()
                {
                    PataStart = EndPataInicialAux,
                    StartPoint_ = PtoInicial,// _ptobarra1 - _view.ViewDirection * (_diametroEstribo_foot + _diametroLat_foot) + new XYZ(0, 0, listaIntervalo[i]) - _direccionBarra * ProlonInicial,
                    EndPoint_ = PtoFinal,//_ptobarra2.AsignarZ(_ptobarra1.Z) - _view.ViewDirection * (_diametroEstribo_foot + _diametroLat_foot) + new XYZ(0, 0, listaIntervalo[i]) + _direccionBarra * ProlonFinal,
                    PataEnd = EndPataFinalAux,
                    TipoLateral = result.tipobarraV,
                    DiamtroLat = _configuracionInicialEstriboDTO.DiamtroLateralEstriboMM,
                    TextoLat = _textoLat
                };

                list.Add(barraLateralesDTO);
            }

            return list;
        }

        public SelecionarPtoHorizontal M4_SeleccionarSegundoPtoHorizontalTramo(XYZ _PtoInicioIntervaloBarra, XYZ _PtoFinalIntervaloBarra)
        {

            SelecionarPtoHorizontal selecionarPtoHorizontal = new SelecionarPtoHorizontal(_uiapp, confiEnfierradoDTO, _PtoInicioIntervaloBarra, _PtoFinalIntervaloBarra);
            selecionarPtoHorizontal.MoverPtosSobrePLanoCaraViga(_seleccionarElementos);
            if (selecionarPtoHorizontal.IsConError)
            {
                Util.ErrorMsg($"Error Al seleccionar el punto superior n° {confiEnfierradoDTO.LineaBarraAnalizada}");
                return null;
            }
            return selecionarPtoHorizontal;
        }

        protected ConfiguracionBarraLateralDTO M2_2_1_ObtenerDatosParaTraba()
        {



            return new ConfiguracionBarraLateralDTO()
            {
                DiamtroTrabaEstriboMM = _configuracionInicialEstriboDTO.DiamtroEstriboMM,
                PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost = _ptobarra1,
                PtoSeleccionMouseCentroCaraMuro = (_ptobarra1 + _ptobarra2) / 2,
                Ptobarra1 = _ptobarra1,// + _direccionPerpenEntradoHaciaViewSecction * Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM / 2),
                Ptobarra2 = _ptobarra2,// + _direccionPerpenEntradoHaciaViewSecction * Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM / 2),
                EspesroMuroOVigaFoot = _configuracionInicialEstriboDTO.espesor,
                textoTraba = (_configuracionInicialEstriboDTO.IsTraba == false ? "" : _configuracionInicialEstriboDTO.ObtenerTextBarra_Borrar()),
                DireccionMuro = _direccionBarra,
                DireccionEntradoHaciaView = _view.ViewDirection,
                DireccionEnfierrrado = _direccionBarra,
                //listaEspaciamientoTrabasTransversal = _configuracionInicialEstriboDTO.listaEspaciamientoTrabas,
                ElementoSeleccionado = _configuracionInicialEstriboDTO.ElementoSeleccionado,
                LargoElementoSeleccionadoFoot = _ptobarra2.AsignarZ(0).DistanceTo(_ptobarra1.AsignarZ(0)),
                ViewActual = _view,
                View3D_paraBuscar = _view3D_ParaBuscar,
                View3D_paraVisualizar = null
            };

        }

        private List<double> ObtenerListaEspacimientoLateralesAuto()
        {
            int diaLAt = _configuracionInicialEstriboDTO.DiamtroLateralEstriboMM;

            if (_alturaEstribo < Util.CmToFoot(38))
            {


            }
            else if (_alturaEstribo < Util.CmToFoot(53))
            {
                _textoLat = "LAT.1+1";
                _espaciamietoLat.Add(_alturaEstriboMedia);

            }
            else if (_alturaEstribo < Util.CmToFoot(78))
            {
                _textoLat = "LAT.2+2";
                //_espaciamietoLat.Add(_alturaEstriboMedia - _alturaEstribo *0.5/ 3);
                //_espaciamietoLat.Add(_alturaEstriboMedia + _alturaEstribo *0.5/ 3);

                _espaciamietoLat.Add(_alturaEstriboMedia - _alturaEstribo * 0.5 / 3);
                _espaciamietoLat.Add(_alturaEstriboMedia + _alturaEstribo * 0.5 / 3);
            }
            else if (_alturaEstribo < Util.CmToFoot(103))
            {
                _textoLat = "LAT.3+3";
                //_espaciamietoLat.Add(_alturaEstribo / 4);
                //_espaciamietoLat.Add(_alturaEstribo * 2 / 4);
                //_espaciamietoLat.Add(_alturaEstribo * 3 / 4);

                _espaciamietoLat.Add(_alturaEstriboMedia - _alturaEstriboMedia / 2);
                _espaciamietoLat.Add(_alturaEstriboMedia);
                _espaciamietoLat.Add(_alturaEstriboMedia + _alturaEstriboMedia / 2);
            }
            else if (_alturaEstribo < Util.CmToFoot(128))
            {
                _textoLat = "LAT.4+4";
                _espaciamietoLat.Add(_alturaEstribo / 5);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 5);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 5);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 5);
            }
            else if (_alturaEstribo < Util.CmToFoot(153))
            {
                _textoLat = "LAT.5+5";
                _espaciamietoLat.Add(_alturaEstribo / 6);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 6);
            }
            else if (_alturaEstribo < Util.CmToFoot(178))
            {
                _textoLat = "LAT.6+6";
                _espaciamietoLat.Add(_alturaEstribo / 7);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 7);
            }
            else
            {
                _textoLat = "LAT.7+7";
                _espaciamietoLat.Add(_alturaEstribo / 8);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 7 / 8);
            }
            return _espaciamietoLat;

        }




        private List<double> ObtenerListaEspacimientoLateralesNumero()
        {

            int diaLAt = _configuracionInicialEstriboDTO.DiamtroLateralEstriboMM;
            if (_configuracionInicialEstriboDTO.cantidadLaterales == 1)
            {
                _textoLat = "LAT.1+1Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstriboMedia);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 2)
            {
                _textoLat = "LAT.2+2Ø" + diaLAt;
                //_espaciamietoLat.Add(_alturaEstriboMedia - _alturaEstribo *0.5/ 3);
                //_espaciamietoLat.Add(_alturaEstriboMedia + _alturaEstribo *0.5/ 3);

                _espaciamietoLat.Add(_alturaEstribo / 3);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 3);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 3)
            {
                _textoLat = "LAT.3+3Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 4);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 4);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 4);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 4)
            {
                _textoLat = "LAT.4+4Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 5);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 5);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 5);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 5);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 5)
            {
                _textoLat = "LAT.5+5Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 6);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 6);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 6);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 6)
            {
                _textoLat = "LAT.5+5Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 7);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 7);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 7);
            }
            else if (_configuracionInicialEstriboDTO.cantidadLaterales == 7)
            {
                _textoLat = "LAT.7+7Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 8);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 7 / 8);
            }
            else
            {
                _textoLat = "LAT.7+7Ø" + diaLAt;
                _espaciamietoLat.Add(_alturaEstribo / 8);
                _espaciamietoLat.Add(_alturaEstribo * 2 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 3 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 4 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 5 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 6 / 8);
                _espaciamietoLat.Add(_alturaEstribo * 7 / 8);
            }
            return _espaciamietoLat;

        }
    }
}
