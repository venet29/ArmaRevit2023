using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Creador;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar
{
    public class DibujarBarraMalla
    {
        private readonly UIApplication _uiapp;
        private readonly DatosMallasAutoDTO _datosMallasDTO;
        private readonly DireccionRecorrido _DireccionRecorrido;

        public List<CreadorBarrasV> _listaCreadorBarrasV { get; set; }

        public ConfiguracionIniciaWPFlBarraVerticalDTO _confiWPFEnfierradoDTO { get; }

        private CreadorBarrasV_malla creadorBarrasV;
        private XYZ DirePataEnfirerr_aux;

        public DibujarBarraMalla(UIApplication _uiapp,
            ConfiguracionIniciaWPFlBarraVerticalDTO _confiWPFEnfierradoDTO,
            DatosMallasAutoDTO _datosMallasDTO,
             DireccionRecorrido _DireccionRecorrido)
        {
            this._uiapp = _uiapp;
            this._confiWPFEnfierradoDTO = _confiWPFEnfierradoDTO;
            this._datosMallasDTO = _datosMallasDTO;
            this._DireccionRecorrido = _DireccionRecorrido;
            _listaCreadorBarrasV = new List<CreadorBarrasV>();
        }


        public void CrearMAllaConRebar(SelecionarPtoSup selecionarPtoSup,
                                      DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            //*****************************************************************************************************************************
            //barra vertila
            DirePataEnfirerr_aux = muroSeleccionadoDTO.DireccionPataEnFierrado;
            _confiWPFEnfierradoDTO.DatosMallasDTO = _datosMallasDTO;

            if (_datosMallasDTO.CualMallaDibujar == CualMAllaDibujar.Ambos || _datosMallasDTO.CualMallaDibujar == CualMAllaDibujar.Vertical)
                DibujarMallaVertical(selecionarPtoSup, muroSeleccionadoDTO);

            if (_listaCreadorBarrasV.Count > 0 && _listaCreadorBarrasV[0].ZSUperior_soloMAllaVertical_auto > -100000)
            {
                if (Math.Abs(selecionarPtoSup.ListaLevelIntervalo[2] - -_listaCreadorBarrasV[0].ZSUperior_soloMAllaVertical_auto) < Util.CmToFoot(30))
                {
                    selecionarPtoSup.ListaLevelIntervalo.RemoveAt(2);
                }
                selecionarPtoSup.ListaLevelIntervalo[1] = (_listaCreadorBarrasV[0].ZSUperior_soloMAllaVertical_auto);
            }
            //*****************************************************************************************************************************
            //barra horizontal

            if (_datosMallasDTO.CualMallaDibujar == CualMAllaDibujar.Ambos || _datosMallasDTO.CualMallaDibujar == CualMAllaDibujar.Horizontal)
                DibujarMallaHorizontal(selecionarPtoSup, muroSeleccionadoDTO);


        }

        private void DibujarMallaHorizontal(SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            //;
            _confiWPFEnfierradoDTO.TipoBarraRebar_ = _confiWPFEnfierradoDTO.TipoBarraRebarHorizontal_;// _  TipoBarraVertical.MallaH;
            muroSeleccionadoDTO.DireccionPataEnFierrado = DirePataEnfirerr_aux;
            _confiWPFEnfierradoDTO.M1_ObtenerIntervalosDireccionMuro(_datosMallasDTO.paraCantidadLineasH, "20");
            _confiWPFEnfierradoDTO.inicial_diametroMM = _datosMallasDTO.diametroH;
            _confiWPFEnfierradoDTO.IntervalosEspaciamiento[0] = (int)_datosMallasDTO.espaciemientoV;//20
            _confiWPFEnfierradoDTO.EspaciamietoRecorridoBarraFoot = Util.CmToFoot(_datosMallasDTO.espaciemientoH).ToString();//20
            _confiWPFEnfierradoDTO.LineaBarraAnalizada = 0;
            _confiWPFEnfierradoDTO.BarraTipo = TipoRebar.ELEV_MA_H;
            RecalcularEspaciamientoLineasBarrasHorizontal(muroSeleccionadoDTO._EspesorMuroFoot, 1);
            // _confiWPFEnfierradoDTO.inicial_tipoBarraV = TipoPatasMAllasVertical(_datosMallasDTO.Tipo_PataH);

            for (int i = 0; i < _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length; i++)
            {
                if (i == 1) muroSeleccionadoDTO.DireccionPataEnFierrado *= -1;


                if (_datosMallasDTO.Tipo_PataH == TipoPataMAlla.Sin)
                {
                    _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.BarraVSinPatas;
                }
                else
                {

                    if (i == 0) //I ES LA CANTIDAD DE LINES 
                        _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.BarraVPataFinal; // la linea inicial malla
                    else if (i == _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length - 1)
                        _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.BarraVPataInicial; //la linea final de mall
                    else
                        _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.BarraVSinPatas;// la lineas intermedias
                }
                _confiWPFEnfierradoDTO.LineaBarraAnalizada = i + 1;

                creadorBarrasV = new CreadorBarrasV_malla(_uiapp, selecionarPtoSup, _confiWPFEnfierradoDTO, muroSeleccionadoDTO);
                creadorBarrasV.Ejecutar(i);

                _listaCreadorBarrasV.Add(creadorBarrasV);
            }
        }

        private void DibujarMallaVertical(SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            double _parteDeci = Util.ParteDecimal(_DireccionRecorrido.LargoRecorridoCm / _datosMallasDTO.espaciemientoV);
            muroSeleccionadoDTO.DesplazamientoVerticalFoot = Util.CmToFoot(_datosMallasDTO.espaciemientoV * (1 + _parteDeci)) / 2;

            //4

            if (_datosMallasDTO.tipoMallaV != TipoMAllaMuro.SM)
                _confiWPFEnfierradoDTO.IntervalosEspaciamiento[0] = (int)_datosMallasDTO.espaciemientoV;
            _confiWPFEnfierradoDTO.EspaciamietoRecorridoBarraFoot = Util.CmToFoot(_datosMallasDTO.espaciemientoV).ToString();
            _confiWPFEnfierradoDTO.TipoBarraRebar_ = TipoBarraVertical.MallaV;
            _confiWPFEnfierradoDTO.BarraTipo = TipoRebar.ELEV_MA_V;
            _confiWPFEnfierradoDTO.inicial_diametroMM = _datosMallasDTO.diametroV;
            _confiWPFEnfierradoDTO.IsDibujarTag = true;
            // _confiWPFEnfierradoDTO.inicial_tipoBarraV = TipoPatasMAllasVertical(_datosMallasDTO.Tipo_PataV);

            for (int i = 0; i < _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length; i++)
            {
                // if (i == _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length - 1) muroSeleccionadoDTO.DireccionPataEnFierrado *= -1;

                _confiWPFEnfierradoDTO.IntervalosCantidadBArras[i] = (int)(_DireccionRecorrido.LargoRecorridoCm / _datosMallasDTO.espaciemientoV);
                _confiWPFEnfierradoDTO.LineaBarraAnalizada = i + 1;
                _confiWPFEnfierradoDTO.CuantiaMalla = _datosMallasDTO.ObtenerTExto();


                if (_confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length - 1 == i)
                {
                    muroSeleccionadoDTO.DireccionPataEnFierrado = muroSeleccionadoDTO.DireccionPataEnFierrado * -1;

                    //var largo = selecionarPtoSup._PtoFinalIntervaloBarra.GetXY0().DistanceTo(selecionarPtoSup._PtoInicioIntervaloBarra.GetXY0());
                    //var direccio = (selecionarPtoSup._PtoFinalIntervaloBarra.GetXY0() - selecionarPtoSup._PtoInicioIntervaloBarra.GetXY0()).Normalize();
                    //selecionarPtoSup._PtoFinalIntervaloBarra = selecionarPtoSup._PtoFinalIntervaloBarra+largo* direccio;
                    //selecionarPtoSup._PtoInicioIntervaloBarra = selecionarPtoSup._PtoInicioIntervaloBarra+largo* direccio;
                }


                creadorBarrasV = new CreadorBarrasV_malla(_uiapp, selecionarPtoSup, _confiWPFEnfierradoDTO, muroSeleccionadoDTO);
                creadorBarrasV.Ejecutar(i);
                _confiWPFEnfierradoDTO.IsDibujarTag = false;
                _listaCreadorBarrasV.Add(creadorBarrasV);
            }

        }

        private TipoPataBarra TipoPatasMAllasVertical(TipoPataMAlla tipoMallaV)
        {
            switch (tipoMallaV)
            {
                case TipoPataMAlla.Auto:
                    return TipoPataBarra.buscar;
                case TipoPataMAlla.Izquierda:
                    return TipoPataBarra.BarraVPataInicial;
                case TipoPataMAlla.Derecha:
                    return TipoPataBarra.BarraVPataFinal;
                case TipoPataMAlla.Ambos:
                    return TipoPataBarra.BarraVPataAmbos;
                case TipoPataMAlla.Sin:
                    return TipoPataBarra.BarraVSinPatas;
                default:
                    return TipoPataBarra.BarraVSinPatas;
            }
        }

        protected bool RecalcularEspaciamientoLineasBarrasVertical(SeleccionarElementosV _seleccionarElementos, int factorDesplazamiento)
        {

            try
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
                            sumadeltaEspesor += _datosMallasDTO.diametroH / 10.0f;
                            _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea + sumadeltaEspesor;// + _datosMallasDTO.diametroH*2/10;
                        }
                        else
                        {
                            _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea - sumadeltaEspesor * 2;
                            sumadeltaEspesor -= _datosMallasDTO.diametroH / 10.0f;
                        }
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
        protected bool RecalcularEspaciamientoLineasBarrasHorizontal(double _espesorMuroFoot, int factorDesplazamiento)
        {
            try
            {
                if (_confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length == 1)
                {
                    _confiWPFEnfierradoDTO.IntervalosEspaciamiento[0] = Util.FootToCm(_datosMallasDTO.espesorFoot) / 2 - (_confiWPFEnfierradoDTO.inicial_diametroMM / 10.0f);
                }
                else
                {
                    double espacimientoEntreLinea = //obs1)
                        (Util.FootToCm(_espesorMuroFoot)
                        - ConstNH.RECUBRIMIENTO_BARRA_MALLA_CM * 2
                        - (_confiWPFEnfierradoDTO.inicial_diametroMM / 10.0f) * factorDesplazamiento) /
                        (_confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length - 1);

                    for (int i = 0; i < _confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length; i++)
                        _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

    }
}
