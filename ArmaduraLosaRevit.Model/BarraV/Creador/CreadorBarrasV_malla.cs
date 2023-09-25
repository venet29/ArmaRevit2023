using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Creador
{
    public class CreadorBarrasV_malla:CreadorBarrasV
    {
        private ConfiguracionTAgBarraDTo confBarraTagMalla;

        public CreadorBarrasV_malla(UIApplication uiapp,
            SelecionarPtoSup selecionarPtoSup,
            ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO,
            DatosMuroSeleccionadoDTO muroSeleccionadoDTO):base( uiapp,             selecionarPtoSup,             confiEnfierradoDTO,             muroSeleccionadoDTO)
        
        {

        }

        public override bool Ejecutar(int ii)
        {
            try
            {
                M1_AsignarCAntidadEspaciemientoNuevaLineaBarra(ii);
                IGenerarIntervalosV igenerarIntervalos = FactoryGenerarIntervalos.CrearGeneradorDeIntervalosV_mallas(_uiapp, _confiWPFEnfierradoDTO, _selecionarPtoSup, _muroSeleccionadoDTO);
                M2_GenerarIntervalos(igenerarIntervalos);

                M3_1_ObtenerConfiguracionTAgBarraMallaDTo();
                M3_DibujarBarras(confBarraTagMalla);

                M4_DibujarBarrasCOnfiguracion();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex, "Error al crear rebar verticaL CreadorBarrasV");
                return false;
            }

            return true;
        }
        public  bool Ejecutar_SinTrasn()
        {
            try
            {
                //M1_AsignarCAntidadEspaciemientoNuevaLineaBarra_auto(ii);
                IGenerarIntervalosV igenerarIntervalos = FactoryGenerarIntervalos.CrearGeneradorDeIntervalosV_mallas(_uiapp, _confiWPFEnfierradoDTO, _selecionarPtoSup, _muroSeleccionadoDTO);
                M2_GenerarIntervalos(igenerarIntervalos);

                M3_1_ObtenerConfiguracionTAgBarraMallaDTo();
                M3_DibujarBarras_sinTrasn(confBarraTagMalla);

                M4_DibujarBarrasCOnfiguracion_sintras();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex, "Error al crear rebar verticaL CreadorBarrasV");
                return false;
            }

            return true;
        }





        public void M1_AsignarCAntidadEspaciemientoNuevaLineaBarra_autoH(int i)
        {
            _confiWPFEnfierradoDTO.NuevaLineaCantidadbarra = _confiWPFEnfierradoDTO.IntervalosCantidadBArras[i];
            if (i == 0 && _muroSeleccionadoDTO.TipoElementoSeleccionado != ElementoSeleccionado.Barra)
            {
                if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza || _confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza_BarraVHorquilla)
                    _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot = ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT;
                else
                    _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot = ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT_MALLA;
            }
            else if (_confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length == 3) //para malla triple
            {
                double espaBarraFoot = (_muroSeleccionadoDTO._EspesorMuroFoot - ConstNH.RECUBRIMIENTO_MALLA_foot * 2) / (_confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length-1);
                _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot += espaBarraFoot;
            }
            else
            {
                double espaBarraFoot = (_muroSeleccionadoDTO._EspesorMuroFoot - ConstNH.RECUBRIMIENTO_MALLA_foot * 2) / (_confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length - i);
                _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot += espaBarraFoot;
            }

            _confiWPFEnfierradoDTO.NumeroBarraLinea = _confiWPFEnfierradoDTO.IntervalosCantidadBArras[i];
        }

        public void M1_AsignarCAntidadEspaciemientoNuevaLineaBarra_autoV(int i, DatosMallasAutoDTO _datosMallasDTO)
        {
            _confiWPFEnfierradoDTO.NuevaLineaCantidadbarra = _confiWPFEnfierradoDTO.IntervalosCantidadBArras[i];
            if (i == 0 && _muroSeleccionadoDTO.TipoElementoSeleccionado != ElementoSeleccionado.Barra)
            {
                if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza || _confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza_BarraVHorquilla)
                    _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot = ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT;
                else
                    _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot = ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT_MALLA;
            }
            else
            {
                double espaBarraFoot = (_muroSeleccionadoDTO._EspesorMuroFoot - ConstNH.RECUBRIMIENTO_MALLA_foot * 2-Util.MmToFoot(_datosMallasDTO.diametroH+ _datosMallasDTO.diametroV*2)) / (_confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length - i);
                _confiWPFEnfierradoDTO.EspaciamientoREspectoBordeFoot += espaBarraFoot;
            }

            _confiWPFEnfierradoDTO.NumeroBarraLinea = _confiWPFEnfierradoDTO.IntervalosCantidadBArras[i];
        }

        public void M3_1_ObtenerConfiguracionTAgBarraMallaDTo()
        {
            double desfase = 1.5;
            double desfaseCodo = ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT;

             confBarraTagMalla = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, desfase),
                IsDIrectriz = (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza  ? true : false),
                LeaderElbow = new XYZ(0, 0, desfaseCodo),
                tagOrientation = (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza ? TagOrientation.Vertical : TagOrientation.Horizontal),
                BarraTipo = TipoRebar.ELEV_BA_V
             };
        }


    }
}
