using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
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

namespace ArmaduraLosaRevit.Model.BarraMallaRebar.Intervalos
{
    public class GenerarIntervalos1Nivel_MuroV : AGenerarIntervalosV, IGenerarIntervalosV
    {

        private IIniciarConXTraslapo _iniciarSinTraslapo;

        public GenerarIntervalos1Nivel_MuroV(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
            : base(_uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO)
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

            XYZ _PtoFinalIntervaloBarra_mallaV = _selecionarPtoSup._PtoFinalIntervaloBarra_mallaVertiva;
            XYZ _PtoInicioIntervaloBarra_mallaV = _selecionarPtoSup._PtoInicioIntervaloBarra_mallaVertiva;

            foreach (CoordenadasBarra item in _iniciarSinTraslapo.ListaCoordenadasBarra)
            {

                ResultBuscarMurosDTO _ResultBuscarMurosDTO = ObtenerBuscarMurosDTO(item, _PtoInicioIntervaloBarra, _PtoFinalIntervaloBarra);

                if (!_ResultBuscarMurosDTO.Isok) continue;

                IntervaloBarrasDTO interBArraDto = _ResultBuscarMurosDTO._interBArraDto;
                BuscarMurosDTO _buscarMurosDTO = _ResultBuscarMurosDTO._buscarMurosDTO;

                if (_selecionarPtoSup._listaLevel?.Count > 0)
                {//para desfasar por traslapo, mueve 0.5Diametro, hacia izq o dere segun par o impar del la posicion del nivel de la barra en la lista de niveles
                    Level levelBArras = _selecionarPtoSup._listaLevel?.Where(c => c.ProjectElevation > item.ptoIni_foot.Z - 2).OrderBy(c => c.ProjectElevation).FirstOrDefault();
                    if (levelBArras != null)
                    {
                        moverPorTraslapo = Util.IsImPar(_selecionarPtoSup._listaLevel.IndexOf(levelBArras));
                    }
                }
                if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV && _confiWPFEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Manual)
                    interBArraDto.M2_AsiganrCoordenadasV_reccorridoParaleloView(item.ptoIni_foot.Z, item.ptoFin_foot.Z,  moverPorTraslapo, _uidoc.Document);
                else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV && _confiWPFEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Automatico)
                    interBArraDto.M2_AsiganrCoordenadasV_reccorridoParaleloView_Auto(item.ptoIni_foot.Z, item.ptoFin_foot.Z, _PtoInicioIntervaloBarra_mallaV, _PtoFinalIntervaloBarra_mallaV, 
                                                                                    moverPorTraslapo, _uidoc.Document);

                interBArraDto.IsbarraIncial = AuxIsbarraIncial;
                AuxIsbarraIncial = false;
                //string _tipoBarraV = "BarraVSinPatas";
                //interBArraDto.tipobarraV = TipoBarraV.BarraVPataAmbos;// ObtenertipoBarraV(_tipoBarraV);
                
                interBArraDto.BuscarPatasAmbosLadosVertical(_uiapp, _buscarMurosDTO, _confiWPFEnfierradoDTO.TipoBarraRebar_);
              
                _PtoInicioIntervaloBarra = _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z);

                lista.Add(interBArraDto);
            }

            return lista;
        }


    }
}
