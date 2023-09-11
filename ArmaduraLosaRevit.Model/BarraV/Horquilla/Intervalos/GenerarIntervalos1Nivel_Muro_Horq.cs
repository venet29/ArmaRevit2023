using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos
{
    internal class GenerarIntervalos1Nivel_Muro_Horq : AGenerarIntervalosV,IGenerarIntervalosV
    {

        private IIniciarConXTraslapo _iniciarSinTraslapo;
        public GenerarIntervalos1Nivel_Muro_Horq(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO) :  
            base(_uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO)
        {

        }

        public override void M1_ObtenerIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();

            try
            {
                _iniciarSinTraslapo = new IniciarSinTraslapoV(_selecionarPtoSup, _confiWPFEnfierradoDTO);
                _iniciarSinTraslapo.CalcularIntervalo();
                ListaIntervaloBarrasDTO = M1_1_GEnerarListaIntervaloBarrasDTO();

            }
            catch (Exception ex)
            {
                Console.WriteLine(" error M1_ObtenerIntervaloBarrasDTO():" + ex.Message);
                lista.Clear();
            }
        }




        private List<IntervaloBarrasDTO> M1_1_GEnerarListaIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();
            bool moverPorTraslapo = false;
            bool AuxIsbarraIncial = true;
            XYZ _PtoFinalIntervaloBarra = _selecionarPtoSup._PtoFinalIntervaloBarra;
            XYZ _PtoInicioIntervaloBarra = _selecionarPtoSup._PtoInicioIntervaloBarra;


            foreach (CoordenadasBarra item in _iniciarSinTraslapo.ListaCoordenadasBarra)
            {

                ResultBuscarMurosDTO _ResultBuscarMurosDTO = ObtenerBuscarMurosDTO(item, _PtoInicioIntervaloBarra, _PtoFinalIntervaloBarra);

                if (!_ResultBuscarMurosDTO.Isok) continue;

                IntervaloBarrasDTO interBArraDto = _ResultBuscarMurosDTO._interBArraDto;
  
                interBArraDto.M2_AsiganrCoordenadasH_reccorridoParaleloView(item.ptoIni_foot.Z, item.ptoFin_foot.Z, _selecionarPtoSup);
                interBArraDto.tipoHookInicial = null;
                interBArraDto.tipoHookFinal = null;
                interBArraDto.IsbarraIncial = AuxIsbarraIncial;
                interBArraDto._tipoLineaMallaH = TipoPataBarra.BarraVSinPatas;
                AuxIsbarraIncial = false;

               // IntervaloBarras_HorqDTO _IntervaloBarras_HorqDTO = new IntervaloBarras_HorqDTO() { };
               // interBArraDto._intervaloBarras_HorqDTO = _confiWPFEnfierradoDTO.IntervaloBarras_HorqDTO_;

                // interBArraDto.BuscarPatasAmbosLadosVertical(_uiapp, _buscarMurosDTO, _confiWPFEnfierradoDTO.TipoBarraRebar_);

                moverPorTraslapo = !moverPorTraslapo;
                _PtoInicioIntervaloBarra = _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z);

                lista.Add(_ResultBuscarMurosDTO._interBArraDto);
            }

            return lista;
        }
    }
}