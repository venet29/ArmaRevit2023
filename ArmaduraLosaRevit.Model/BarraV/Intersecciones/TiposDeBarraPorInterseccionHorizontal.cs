using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ArmaduraLosaRevit.Model.BarraV.Buscar.BuscarElementosArriba;
using ArmaduraLosaRevit.Model.BarraV.AppState;

namespace ArmaduraLosaRevit.Model.BarraV.Intersecciones
{
    public class TiposDeBarraPorInterseccionHorizontal
    {
        private readonly UIApplication _uiapp;
        private Autodesk.Revit.DB.View _view;
        private readonly View3D _view3D;

        private readonly IntervaloBarrasDTO _intervaloBarrasDTO;
        private readonly ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO;

        public XYZ _ptoini { get; set; }
        public XYZ _pto1NevelAntesfinal { get; set; }
        public XYZ _ptofinal { get; set; }

        public XYZ _direccionBarra;
        private double recubrimiento_foot;

        //  private int diametroMM;

        public TipoPataBarra ResulttTpobarraDerecha;
        public TipoPataBarra ResultTipobarraIzquierda;
        public TipoPataBarra ResultTipoBarraV { get; set; }

        private BuscarElementosHorizontal _buscarElementosDerecha;

        private BuscarElementosHorizontal _buscarElementosIzquierda;

        public EmpotramientoPatasDTO _EmpotramientoPatasDTO { get; }

        public TiposDeBarraPorInterseccionHorizontal(UIApplication _uiapp, View3D _view3D,
            IntervaloBarrasDTO intervaloBarrasDTO, EmpotramientoPatasDTO _empotramientoPatasDTO,
            ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO)
        {
            this._uiapp = _uiapp;
            this._view = _uiapp.ActiveUIDocument.ActiveView;
            this._view3D = _view3D;
            this._intervaloBarrasDTO = intervaloBarrasDTO;
            _EmpotramientoPatasDTO = _empotramientoPatasDTO;
            this._confiEnfierradoDTO = _confiEnfierradoDTO;

            //this.diametroMM = diametroMM;
            this._ptoini = intervaloBarrasDTO.ptoini;
            this._ptofinal = intervaloBarrasDTO.ptofinal;
            this._pto1NevelAntesfinal = _ptofinal + new XYZ(0, 0, -5);
            this._direccionBarra = (_ptofinal - _ptoini).Normalize();

            this.recubrimiento_foot = (intervaloBarrasDTO.IsFundacion ? ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot : ConstNH.RECUBRIMIENTO_PATA_BARRAV_Foot);
        }

        public void BuscarInterseccion()
        {
            ResulttTpobarraDerecha = TipoPataBarra.BarraVSinPatas;
            ResultTipobarraIzquierda = TipoPataBarra.BarraVSinPatas;
            ResultTipoBarraV = TipoPataBarra.BarraVSinPatas;

            M1_BuscarIntersecionHaciaDerecha();
            M2_BuscarIntersecionHaciaIzquierda();
            M3_ObternerResultTipoBarraV();
        }


        private bool M1_BuscarIntersecionHaciaDerecha()
        {
            try
            {
                double desplazamientoLinea = (_confiEnfierradoDTO.LineaBarraAnalizada - 1) * ConstNH.RECUBRIMIENTO_PATA_BARRAV_LINEAANALIZADO;
                desplazamientoLinea = 0;

                // se agrega para por configuracion
                if (_EmpotramientoPatasDTO.TipoPataDereSup == TipoPataBarra.BarraVPataFinal || _EmpotramientoPatasDTO.TipoPataDereSup == TipoPataBarra.BarraVPataAmbos)
                {
                    M1_1_pataAlfinal(desplazamientoLinea, TipoElementoBArraV.none);
                    return false;
                }
                else if (_EmpotramientoPatasDTO.TipoPataDereSup== TipoPataBarra.BarraVSinPatas) //actiavar esta parte si no se quire largo prolongacion  || _EmpotramientoPatasDTO.TipoPataIzqInf == TipoPataBarra.BarraVPataInicial)
                {
                    //sin mofificacoin
                    return false;
                }

                //solo para coso de Tipo barras refuerzo viga
                if (_confiEnfierradoDTO.TipoBarraRefuerzoViga == TipoBarraRefuerzoViga.RefuerzoBorde)
                    _direccionBarra = _view.RightDirection;

                // se busca pata
                double DistanciaRetrocesoPtoSup = Util.CmToFoot(1);
                double LargoBusuqeda = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) * M1_1_FactorEmpotramientoDere();// Util.CmToFoot(1.5);
                _buscarElementosDerecha = new BuscarElementosHorizontal(_uiapp, LargoBusuqeda, _view3D);
                _buscarElementosDerecha.BuscarObjetos(this._ptofinal - _direccionBarra * DistanciaRetrocesoPtoSup, _direccionBarra);

                if (_buscarElementosDerecha.listaObjEncontrados.Count == 0 && _EmpotramientoPatasDTO.TipoPataDereSup == TipoPataBarra.BuscarSinExtender)
                {
                    //_ptofinal = _ptofinal;
                    return false;
                }
                else if (_buscarElementosDerecha.listaObjEncontrados.Count == 0 || _EmpotramientoPatasDTO.TipoPataDereSup == TipoPataBarra.NoBuscar)
                {
                    _ptofinal = _ptofinal + _direccionBarra * UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) * M1_1_FactorEmpotramientoDere();
                    return false;
                }
                else
                {
                    if (BuscarSiBarraCreadaEnvigaDentroDeLosa_ladoDerecho()) return true;

                    var listaMaslejanos = BuscarMasLejanos_ListaElementoHORIZONTAL(_buscarElementosDerecha.listaObjEncontrados);
                    if (listaMaslejanos.Count == 1)
                        M1_1_pataAlfinal(listaMaslejanos[0].distancia - DistanciaRetrocesoPtoSup, listaMaslejanos[0].nombreTipo);
                    else if (listaMaslejanos.Count > 1)
                    {
                        //busca si hay algina cara con normal igual al ladireccion de busqueda
                        var PlanosOpuesto_vectoBusqueda = listaMaslejanos.Where(c => Util.IsParallel_DistintoSentido(c.NormalFace, _direccionBarra, 0.9)).FirstOrDefault();

                        //si encuentra  lleva para  (pq selecciono cara mas externa)
                        if (PlanosOpuesto_vectoBusqueda != null)
                            _ptofinal = _ptofinal + _direccionBarra * UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) * M1_1_FactorEmpotramientoDere();
                        else //sino genera  prolongacion pq estoy dentro de un objeto
                            M1_1_pataAlfinal(listaMaslejanos[0].distancia - DistanciaRetrocesoPtoSup, listaMaslejanos[0].nombreTipo);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private bool BuscarSiBarraCreadaEnvigaDentroDeLosa_ladoDerecho()
        {
            if ((_buscarElementosDerecha.listaObjEncontrados.Count == 1) && _buscarElementosDerecha.listaObjEncontrados[0].nombreTipo == TipoElementoBArraV.viga
                && AppManejadorBarraHState.IsOk
                && AppManejadorBarraHState.TipoElementoSeleccionado == ElementoSeleccionado.Losa)
            {
                if (!AppManejadorBarraHState.VerificarPtoDentroElemento()) return false;

                _ptofinal = _ptofinal + _direccionBarra * UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) * M1_1_FactorEmpotramientoDere();
                return true;
            }
            else
                return false;
        }

        //dere
        private void M1_1_pataAlfinal(double desplazamientoLinea, TipoElementoBArraV _tipo)
        {

            recubrimiento_foot = (_tipo == TipoElementoBArraV.fundacion ? ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot : recubrimiento_foot);

            _ptofinal = _ptofinal + -_direccionBarra * (recubrimiento_foot + Util.MmToFoot(_confiEnfierradoDTO.incial_diametroMM) / 2) + desplazamientoLinea * _direccionBarra;
            ResulttTpobarraDerecha = TipoPataBarra.BarraVPataFinal;
        }

        private bool M2_BuscarIntersecionHaciaIzquierda()
        {
            try
            {
                double desplazamientoLinea = (_confiEnfierradoDTO.LineaBarraAnalizada - 1) * ConstNH.RECUBRIMIENTO_PATA_BARRAV_LINEAANALIZADO;
                desplazamientoLinea = 0;

                // se agrega para por configuracion
                if (_EmpotramientoPatasDTO.TipoPataIzqInf == TipoPataBarra.BarraVPataInicial || _EmpotramientoPatasDTO.TipoPataIzqInf == TipoPataBarra.BarraVPataAmbos)
                {
                    PataAlInicio(desplazamientoLinea, TipoElementoBArraV.none);
                    return false;
                }
                else if (_EmpotramientoPatasDTO.TipoPataIzqInf== TipoPataBarra.BarraVSinPatas) //actiavar esta parte si no se quire largo prolongacion  ||  _EmpotramientoPatasDTO.TipoPataDereSup == TipoPataBarra.BarraVPataFinal)
                {
                    //sin mofificacoin
                    return false;
                }

                //solo para coso de Tipo barras refuerzo viga
                if (_confiEnfierradoDTO.TipoBarraRefuerzoViga == TipoBarraRefuerzoViga.RefuerzoBorde)
                    _direccionBarra = _view.RightDirection;

                // se busca pata
                double DistanciaRetrocesoPtoSup = Util.CmToFoot(1);
                double LargoBusuqeda = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) * M1_1_FactorEmpotramientoIzq(); //Util.CmToFoot(1.5);
                                                                                                                                                  //   _buscarElementosIzquierda = new BuscarElementosIzquierda(_uiapp, LargoBusuqeda, _view3D);
                                                                                                                                                  //    _buscarElementosIzquierda.BuscarObjetosIzquierdaBArra(this._ptoini + _direccionBarra * DistanciaRetrocesoPtoSup, -_direccionBarra)
                _buscarElementosIzquierda = new BuscarElementosHorizontal(_uiapp, LargoBusuqeda, _view3D);
                _buscarElementosIzquierda.BuscarObjetos(this._ptoini + _direccionBarra * DistanciaRetrocesoPtoSup, -_direccionBarra);


                if (_buscarElementosIzquierda.listaObjEncontrados.Count == 0 && _EmpotramientoPatasDTO.TipoPataIzqInf == TipoPataBarra.BuscarSinExtender)
                {
                    //_ptoini = _ptoini;
                    return false;
                }
                else if (_buscarElementosIzquierda.listaObjEncontrados.Count == 0 || _EmpotramientoPatasDTO.TipoPataIzqInf == TipoPataBarra.NoBuscar)
                {
                    _ptoini = _ptoini - _direccionBarra * UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) * M1_1_FactorEmpotramientoIzq();
                    return false;
                }
                else
                {
                    if (BuscarSiBarraCreadaEnvigaDentroDeLosa_ladoDIzquierdo()) return true;

                    var listaMaslejanos = BuscarMasLejanos_ListaElementoHORIZONTAL(_buscarElementosIzquierda.listaObjEncontrados);
                    if (listaMaslejanos.Count == 1)
                        PataAlInicio(listaMaslejanos[0].distancia - DistanciaRetrocesoPtoSup, listaMaslejanos[0].nombreTipo);
                    else if (listaMaslejanos.Count > 1)
                    {
                        //busca si hay algina cara con normal igual al ladireccion de busqueda
                        var PlanosOpuesto_vectoBusqueda = listaMaslejanos.Where(c => Util.IsParallel_DistintoSentido(c.NormalFace, -_direccionBarra, 0.9)).FirstOrDefault();

                        //si encuentra  lleva para  (pq selecciono cara mas externa)
                        if (PlanosOpuesto_vectoBusqueda != null)
                            _ptoini = _ptoini - _direccionBarra * UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) * M1_1_FactorEmpotramientoIzq();
                        else //sino genera  prolongacion pq estoy dentro de un objeto
                            PataAlInicio(listaMaslejanos[0].distancia - DistanciaRetrocesoPtoSup, listaMaslejanos[0].nombreTipo);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
        //izq
        private void PataAlInicio(double desplazamientoLinea, TipoElementoBArraV _tipo)
        {
            recubrimiento_foot = (_tipo == TipoElementoBArraV.fundacion ? ConstNH.RECUBRIMIENTO_VERTICAL_FUND_foot : recubrimiento_foot);

            _ptoini = _ptoini + _direccionBarra * (recubrimiento_foot + Util.MmToFoot(_confiEnfierradoDTO.incial_diametroMM) / 2) - _direccionBarra * desplazamientoLinea;
            ResultTipobarraIzquierda = TipoPataBarra.BarraVPataInicial;
        }

        private bool BuscarSiBarraCreadaEnvigaDentroDeLosa_ladoDIzquierdo()
        {
            if ((_buscarElementosIzquierda.listaObjEncontrados.Count == 1) && _buscarElementosIzquierda.listaObjEncontrados[0].nombreTipo == TipoElementoBArraV.viga
                && AppManejadorBarraHState.IsOk
                && AppManejadorBarraHState.TipoElementoSeleccionado == ElementoSeleccionado.Losa)
            {
                _ptoini = _ptoini - _direccionBarra * UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) * M1_1_FactorEmpotramientoIzq();
                return true;
            }
            else
                return false;
        }


        private double M1_1_FactorEmpotramientoDere()
        {
            switch (_EmpotramientoPatasDTO._conEmpotramientoDereSup)
            {
                case TipoEmpotramiento.sin:
                    return 0;
                case TipoEmpotramiento.mitad:
                    return 0.5;
                case TipoEmpotramiento.total:
                    return 1;
                default:
                    return 0;
            }
        }

        private double M1_1_FactorEmpotramientoIzq()
        {
            switch (_EmpotramientoPatasDTO._conEmpotramientoIzqInf)
            {
                case TipoEmpotramiento.sin:
                    return 0;
                case TipoEmpotramiento.mitad:
                    return 0.5;
                case TipoEmpotramiento.total:
                    return 1;
                default:
                    return 0;
            }
        }

        private void M3_ObternerResultTipoBarraV()
        {
            if (ResulttTpobarraDerecha == TipoPataBarra.BarraVPataFinal && ResultTipobarraIzquierda == TipoPataBarra.BarraVPataInicial)
            {
                ResultTipoBarraV = TipoPataBarra.BarraVPataAmbos;
            }
            else if (ResulttTpobarraDerecha == TipoPataBarra.BarraVPataFinal)
            {
                ResultTipoBarraV = TipoPataBarra.BarraVPataFinal;
            }
            else if (ResultTipobarraIzquierda == TipoPataBarra.BarraVPataInicial)
            {
                ResultTipoBarraV = TipoPataBarra.BarraVPataInicial;
            }
            else
            { ResultTipoBarraV = TipoPataBarra.BarraVSinPatas; }
        }

        private List<ObjetosEncontradosDTO> BuscarMasLejanos_ListaElementoHORIZONTAL(List<ObjetosEncontradosDTO> _buscarElementos)
        {
            var losaMaxDistancia = _buscarElementos.Max(c => c.distancia);

            var ListaMAslejanos = _buscarElementos.Where(c => Util.IsSimilarValor(c.distancia, losaMaxDistancia, ConstNH.TOLERANCIACERO_3mm)).ToList();

            return ListaMAslejanos;
        }
    }
}
