using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos
{
    public class GenerarIntervalos1Nivel_Cabeza_Horq : AGenerarIntervalosV, IGenerarIntervalosV
    {

        private IIniciarConXTraslapo _iniciarSinTraslapo;

        public GenerarIntervalos1Nivel_Cabeza_Horq(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
            : base(_uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO)
        {

        }

        public override void M1_ObtenerIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();

            try
            {
                _iniciarSinTraslapo = new IniciarSinTraslapoV_Horq(_selecionarPtoSup, _confiWPFEnfierradoDTO);
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

             //   IntervaloBarrasDTO interBArraDto = new IntervaloBarrasHorqDTO(_muroSeleccionadoInicialDTO, _confiWPFEnfierradoDTO);
                IntervaloBarrasHorqDTO interBArraDto = new IntervaloBarrasHorqDTO(_muroSeleccionadoInicialDTO, _confiWPFEnfierradoDTO);
                // IntervaloBarrasDTO interBArraDto =  _ResultBuscarMurosDTO._interBArraDto;

                interBArraDto.M2_AsiganrCoordenadasH_reccorridoParaleloViewHorq(item.ptoIni_foot.Z, item.ptoFin_foot.Z, _selecionarPtoSup);
                interBArraDto.tipoHookInicial = null;
                interBArraDto.tipoHookFinal = null;
                interBArraDto.IsbarraIncial = AuxIsbarraIncial;
                interBArraDto._tipoLineaMallaH =TipoPataBarra.BarraVPataInicial;
                interBArraDto.Largopata = Util.CmToFoot( _confiWPFEnfierradoDTO.IntervalosEspaciamiento[0]);



                interBArraDto.RecalcularPtosYEspaciamieto_Horqu =_selecionarPtoSup._RecalcularPtosYEspaciamieto_HORQUILLA;
                moverPorTraslapo = !moverPorTraslapo;
                _PtoInicioIntervaloBarra = _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z);

                lista.Add(interBArraDto);
                //lista.Add(interBArraDto);
            }

            return lista;
        }

    }
}
