using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public class GenerarIntervalos2Nivel : AGenerarIntervalosV, IGenerarIntervalosV
    {
        private IIniciarConXTraslapo _iniciarTraslapo;

        public GenerarIntervalos2Nivel(UIApplication _uiapp,
            ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO,
            SelecionarPtoSup selecionarPtoSup,
            DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
            : base(_uiapp, confiEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO)
        {

        }


        public override void M1_ObtenerIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();
            try
            {
                List<CoordenadasBarra> listaCoordenadasBarra = new List<CoordenadasBarra>();

                int ComoPartirSegunLineaActual = (_confiWPFEnfierradoDTO.LineaBarraAnalizada % 2 == 0 ?
                                                   _confiWPFEnfierradoDTO.incial_ComoIniciarTraslapo_LineaPAr :
                                                   _confiWPFEnfierradoDTO.incial_ComoIniciarTraslapo_LineaImpar);

                if (ComoPartirSegunLineaActual == 1)
                {
                    _iniciarTraslapo = new IniciarCon1Traslapo(_selecionarPtoSup, _confiWPFEnfierradoDTO);
                }
                else if (ComoPartirSegunLineaActual == 2)
                {
                    _iniciarTraslapo = new IniciarSinTraslapoV(_selecionarPtoSup, _confiWPFEnfierradoDTO);
                }
                else if (ComoPartirSegunLineaActual == 3)
                {
                    _iniciarTraslapo = new IniciarSinTraslapoV(_selecionarPtoSup, _confiWPFEnfierradoDTO);
                }

                _iniciarTraslapo.CalcularIntervalo();

                ListaIntervaloBarrasDTO = M1_1_GEnerarListaIntervaloBarrasDTO();


                // IbarraVertical BarraVertical = new BarraVSinPatas(commandData.Application, interBArraDto, null);
            }
            catch (Exception ex)
            {
                ConstNH.sbLog.AppendLine($"ex: {ex.Message} ");
                ListaIntervaloBarrasDTO.Clear();

            }

        }

        private List<IntervaloBarrasDTO> M1_1_GEnerarListaIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();
            bool moverPorTraslapo = false;
            bool AuxIsbarraIncial = true;
            bool IsPrimeraBarra = true;
            XYZ _PtoFinalIntervaloBarra = _selecionarPtoSup._PtoFinalIntervaloBarra;
            XYZ _PtoInicioIntervaloBarra = _selecionarPtoSup._PtoInicioIntervaloBarra;


            for (int i = 0; i < _iniciarTraslapo.ListaCoordenadasBarra.Count; i++)
            {

                CoordenadasBarra item = _iniciarTraslapo.ListaCoordenadasBarra[i];

                IntervaloBarrasDTO interBArraDto = new IntervaloBarrasDTO(_muroSeleccionadoInicialDTO, _confiWPFEnfierradoDTO);
                interBArraDto.IsUltimoTramoCOnMouse = item.IsUltimoTramoCOnMouse;
                //busca muro 50cm sobre el pto inicial del muroi
                XYZ PtoInicalAUX = _muroSeleccionadoInicialDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(item.ptoBusqueda_muro.Z).ObtenerCopia();

                BuscarMurosDTO _buscarMurosDTO = null;
                if (_muroSeleccionadoInicialDTO.elementoContenedor is Wall)
                {
                    BuscarMuros muroHost = BuscarMuroPerpendicularVIew(PtoInicalAUX);
                    if (muroHost == null) continue;
                    if (!interBArraDto.M1_AsignarElementoHost(muroHost, _uiapp.ActiveUIDocument.Document)) continue;// return new List<IntervaloBarrasDTO>();

                    _buscarMurosDTO = muroHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                 _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                }
                else if (_muroSeleccionadoInicialDTO.elementoContenedor is FamilyInstance)
                {
                    FamilyInstance _familyInstanceSelect = (FamilyInstance)_muroSeleccionadoInicialDTO.elementoContenedor;

                    BuscarViga vigaHost = new BuscarViga(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                    if (vigaHost == null) continue;

                    if (!vigaHost.AsignarViga(_familyInstanceSelect, _muroSeleccionadoInicialDTO))  continue;
                    interBArraDto.M1_AsignarElementoHost(vigaHost, _uiapp.ActiveUIDocument.Document);
                    _buscarMurosDTO = vigaHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                 _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                }
                else
                    continue;



                double valor = Math.Abs(Util.GetProductoEscalar(_muroSeleccionadoInicialDTO.DireccionLineaBarra, _view.RightDirection));
                //asignar punto incial y final
                if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza)
                    interBArraDto.M2_AsiganrCoordenadasV(item.ptoIni_foot.Z, item.ptoFin_foot.Z, moverPorTraslapo, IsPrimeraBarra);
                else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV && _confiWPFEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Manual)
                    interBArraDto.M2_AsiganrCoordenadasV_reccorridoParaleloView(item.ptoIni_foot.Z, item.ptoFin_foot.Z, moverPorTraslapo, _uidoc.Document);
                else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV && _confiWPFEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Automatico)
                    interBArraDto.M2_AsiganrCoordenadasV_reccorridoParaleloView_Auto(item.ptoIni_foot.Z, item.ptoFin_foot.Z, _PtoInicioIntervaloBarra, _PtoFinalIntervaloBarra,
                                                                                    moverPorTraslapo, _uidoc.Document);
                else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaH)
                    interBArraDto.M2_AsiganrCoordenadasH_reccorridoParaleloView(item.ptoIni_foot.Z, item.ptoFin_foot.Z, _selecionarPtoSup);


                //interBArraDto.tipobarraV = item.tipoBarraV;
                interBArraDto.IsbarraIncial = AuxIsbarraIncial;
                IsPrimeraBarra = false;
                AuxIsbarraIncial = false;

                if (_confiWPFEnfierradoDTO.TipoBarraRebar_ != TipoBarraVertical.MallaH)
                    interBArraDto.BuscarPatasAmbosLadosVertical(_uiapp, _buscarMurosDTO);

                moverPorTraslapo = AyudaMoverBarras.MOverBarras(moverPorTraslapo);

                _PtoInicioIntervaloBarra = _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z);

                lista.Add(interBArraDto);

                // 17-05-2023 revisar si barra de piso antes de coronaimeito , se extiende hasta coronomiento entonces, se borra intervalo de coronamiento
                if (_iniciarTraslapo.ListaCoordenadasBarra.Count - 2 == i)
                {
                    if(Math.Abs(interBArraDto.ptofinal.Z - _iniciarTraslapo.ListaCoordenadasBarra.Last().ptoFin_foot.Z)<Util.CmToFoot(15))
                    {
                        _iniciarTraslapo.ListaCoordenadasBarra.RemoveAt(_iniciarTraslapo.ListaCoordenadasBarra.Count - 1);
                    }
                }

            }

            return lista;
        }


    }
}
