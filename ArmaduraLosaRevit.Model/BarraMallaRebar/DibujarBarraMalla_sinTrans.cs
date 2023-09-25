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
    public class DibujarBarraMalla_sinTrans
    {
        private readonly UIApplication _uiapp;
        private readonly DatosMallasAutoDTO _datosMallasDTO;
        private readonly DireccionRecorrido _DireccionRecorrido;

        public List<CreadorBarrasV> _listaCreadorBarrasV { get; set; }

        public ConfiguracionIniciaWPFlBarraVerticalDTO _confiWPFEnfierradoDTO { get; }

        private CreadorBarrasV_malla creadorBarrasV;
        private XYZ DirePataEnfirerr_aux;

        public DibujarBarraMalla_sinTrans(UIApplication _uiapp,
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


        public void CrearMAllaConRebar_Sintras(SelecionarPtoSup selecionarPtoSup,
                                      DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            //*****************************************************************************************************************************
            //barra vertila
            DirePataEnfirerr_aux = muroSeleccionadoDTO.DireccionPataEnFierrado;
           // _datosMallasDTO.CualMallaDibujar = CualMAllaDibujar.Horizontal;
            if (_datosMallasDTO.CualMallaDibujar == CualMAllaDibujar.Ambos || _datosMallasDTO.CualMallaDibujar == CualMAllaDibujar.Vertical)
                DibujarMallaVertical_Sintras(selecionarPtoSup, muroSeleccionadoDTO);

            if (_listaCreadorBarrasV.Count > 2 && _listaCreadorBarrasV[0].ZSUperior_soloMAllaVertical_auto > -100000)
            {
                if (Math.Abs(selecionarPtoSup.ListaLevelIntervalo[2] - _listaCreadorBarrasV[0].ZSUperior_soloMAllaVertical_auto) < Util.CmToFoot(30))
                {
                    selecionarPtoSup.ListaLevelIntervalo.RemoveAt(2);
                }
                selecionarPtoSup.ListaLevelIntervalo[1] = (_listaCreadorBarrasV[0].ZSUperior_soloMAllaVertical_auto);
            }
            //*****************************************************************************************************************************
            //barra horizontal

            if (_datosMallasDTO.CualMallaDibujar == CualMAllaDibujar.Ambos || _datosMallasDTO.CualMallaDibujar == CualMAllaDibujar.Horizontal)
                DibujarMallaHorizontal_Sintras(selecionarPtoSup, muroSeleccionadoDTO);

        }

        private void DibujarMallaHorizontal_Sintras(SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
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

                if (i == 0) //I ES LA CANTIDAD DE LINES 
                    _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.BarraVPataFinal; // la linea inicial malla
                else if (i == _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length - 1)
                    _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.BarraVPataInicial; //la linea final de mall
                else
                    _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.NoBuscar;// TipoPataBarra.BarraVSinPatas;// la lineas intermedias

                _confiWPFEnfierradoDTO.LineaBarraAnalizada = i + 1;

                creadorBarrasV = new CreadorBarrasV_malla(_uiapp, selecionarPtoSup, _confiWPFEnfierradoDTO, muroSeleccionadoDTO);
                creadorBarrasV.M1_AsignarCAntidadEspaciemientoNuevaLineaBarra_autoH(i);
                creadorBarrasV.Ejecutar_SinTrasn();

                _listaCreadorBarrasV.Add(creadorBarrasV);
            }
        }

        private void DibujarMallaVertical_Sintras(SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            double _parteDeci = Util.ParteDecimal(_DireccionRecorrido.LargoRecorridoCm / _datosMallasDTO.espaciemientoV);
            muroSeleccionadoDTO.DesplazamientoVerticalFoot = Util.CmToFoot(_datosMallasDTO.espaciemientoV * (1 + _parteDeci)) / 2;

            //4
            //XYZ _PtoInicioSobrePLanodelMuro_aux = muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;
            _confiWPFEnfierradoDTO.IntervalosEspaciamiento[0] = (int)_datosMallasDTO.espaciemientoV;
            _confiWPFEnfierradoDTO.EspaciamietoRecorridoBarraFoot = Util.CmToFoot(_datosMallasDTO.espaciemientoV).ToString();
            _confiWPFEnfierradoDTO.TipoBarraRebar_ = TipoBarraVertical.MallaV;
            _confiWPFEnfierradoDTO.BarraTipo = TipoRebar.ELEV_MA_V;
            _confiWPFEnfierradoDTO.inicial_diametroMM = _datosMallasDTO.diametroV;
            _confiWPFEnfierradoDTO.IsDibujarTag = true;
            // _confiWPFEnfierradoDTO.inicial_tipoBarraV = TipoPatasMAllasVertical(_datosMallasDTO.Tipo_PataV);

            for (int i = 0; i < _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length; i++)
            {
            //    if (i == _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length - 1) muroSeleccionadoDTO.DireccionPataEnFierrado *= -1;

                _confiWPFEnfierradoDTO.IntervalosCantidadBArras[i] = (int)(_DireccionRecorrido.LargoRecorridoCm / _datosMallasDTO.espaciemientoV);
                _confiWPFEnfierradoDTO.LineaBarraAnalizada = i + 1;
                _confiWPFEnfierradoDTO.CuantiaMalla = _datosMallasDTO.ObtenerTExto();


                creadorBarrasV = new CreadorBarrasV_malla(_uiapp, selecionarPtoSup, _confiWPFEnfierradoDTO, muroSeleccionadoDTO);
                creadorBarrasV.M1_AsignarCAntidadEspaciemientoNuevaLineaBarra_autoV(i, _datosMallasDTO);
                creadorBarrasV.Ejecutar_SinTrasn();
                _confiWPFEnfierradoDTO.IsDibujarTag = false;
                _listaCreadorBarrasV.Add(creadorBarrasV);
            }

        }

        protected bool RecalcularEspaciamientoLineasBarrasHorizontal(double _espesorMuroFoot, int factorDesplazamiento)
        {

            try
            {
                double espacimientoEntreLinea = //obs1)
                    (Util.FootToCm(_espesorMuroFoot)
                    - ConstNH.RECUBRIMIENTO_BARRA_MALLA_CM * 2
                    - (_confiWPFEnfierradoDTO.inicial_diametroMM / 10.0f) * factorDesplazamiento) /
                    (_confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length - 1);

                for (int i = 0; i < _confiWPFEnfierradoDTO.IntervalosEspaciamiento.Length; i++)
                    _confiWPFEnfierradoDTO.IntervalosEspaciamiento[i] = espacimientoEntreLinea;

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
