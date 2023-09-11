using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
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

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public class GenerarIntervalosVariosNivel : AGenerarIntervalosV, IGenerarIntervalosV
    {
        private IIniciarConXTraslapo _iniciarCon1Traslapo;

        public GenerarIntervalosVariosNivel(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
            : base(_uiapp, confiEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO)
        { }


        public override void M1_ObtenerIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();
            try
            {
                // List<CoordenadasBarra> listaCoordenadasBarra = new List<CoordenadasBarra>();

                int ComoPartirSegunLineaActual = (_confiWPFEnfierradoDTO.LineaBarraAnalizada % 2 == 0 ?
                                                   _confiWPFEnfierradoDTO.incial_ComoIniciarTraslapo_LineaPAr :
                                                   _confiWPFEnfierradoDTO.incial_ComoIniciarTraslapo_LineaImpar);

                if (ComoPartirSegunLineaActual == 1)
                {
                    _iniciarCon1Traslapo = new IniciarCon1Traslapo(_selecionarPtoSup, _confiWPFEnfierradoDTO);
                }
                else if (ComoPartirSegunLineaActual == 2)
                {
                    _iniciarCon1Traslapo = new IniciarCon2Traslapo(_selecionarPtoSup, _confiWPFEnfierradoDTO);
                }
                else if (ComoPartirSegunLineaActual == 3)
                {
                }

                _iniciarCon1Traslapo.CalcularIntervalo();

                ListaIntervaloBarrasDTO = M1_1_GEnerarListaIntervaloBarrasDTO();


                // IbarraVertical BarraVertical = new BarraVSinPatas(commandData.Application, interBArraDto, null);
            }
            catch (Exception ex)
            {
                ConstNH.sbLog.AppendLine($"Error en 'M1_ObtenerIntervaloBarrasDTO'. ex: {ex.Message} ");
                ListaIntervaloBarrasDTO.Clear();

            }

        }

        private List<IntervaloBarrasDTO> M1_1_GEnerarListaIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();
            try
            {              
                bool moverPorTraslapo = false;
                bool AuxIsbarraIncial = true;
                bool IsPrimeraBarra = true;
                XYZ _PtoFinalIntervaloBarra = _selecionarPtoSup._PtoFinalIntervaloBarra;
                XYZ _PtoInicioIntervaloBarra = _selecionarPtoSup._PtoInicioIntervaloBarra;

                XYZ PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = _muroSeleccionadoInicialDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;

                foreach (CoordenadasBarra item in _iniciarCon1Traslapo.ListaCoordenadasBarra)
                {


                    IntervaloBarrasDTO interBArraDto = new IntervaloBarrasDTO(_muroSeleccionadoInicialDTO, _confiWPFEnfierradoDTO);
                    interBArraDto.IsUltimoTramoCOnMouse = item.IsUltimoTramoCOnMouse;
                    //busca muro 50cm sobre el pto inicial del muroi
                    XYZ PtoInicalAUX = PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(item.ptoBusqueda_muro.Z);
                    // BuscarMuros muroHost = BuscarMuroPerpendicularVIew(PtoInicalAUX);
                    //if (muroHost == null) continue;

                    BuscarMurosDTO _buscarMurosDTO = null;

                    if (_muroSeleccionadoInicialDTO.elementoContenedor is Wall)
                    {
                        BuscarMuros muroHost = BuscarMuroPerpendicularVIew(PtoInicalAUX);
                        if (muroHost == null) continue;
                        interBArraDto.M1_AsignarElementoHost(muroHost, _uiapp.ActiveUIDocument.Document);

                        _buscarMurosDTO = muroHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                               _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                    }
                    else
                    {
                        BuscarViga vigaHost = new BuscarViga(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                        if (vigaHost == null) continue;
                        vigaHost.AsignarViga((FamilyInstance)_muroSeleccionadoInicialDTO.elementoContenedor, _muroSeleccionadoInicialDTO);

                        interBArraDto.M1_AsignarElementoHost(vigaHost, _uiapp.ActiveUIDocument.Document);
                        _buscarMurosDTO = vigaHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                     _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                    }

                    double valor = Math.Abs(Util.GetProductoEscalar(_muroSeleccionadoInicialDTO.DireccionLineaBarra, _view.RightDirection));
                    //asignar punto incial y final
                    if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza)
                        interBArraDto.M2_AsiganrCoordenadasV(item.ptoIni_foot.Z, item.ptoFin_foot.Z, moverPorTraslapo, IsPrimeraBarra);
                    else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV && _confiWPFEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Manual)
                        interBArraDto.M2_AsiganrCoordenadasV_reccorridoParaleloView(item.ptoIni_foot.Z, item.ptoFin_foot.Z, moverPorTraslapo, _uidoc.Document);
                    else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV && _confiWPFEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Automatico)
                        interBArraDto.M2_AsiganrCoordenadasV_reccorridoParaleloView_Auto(item.ptoIni_foot.Z, item.ptoFin_foot.Z, item.ptoIni_MallaVertical, item.ptoFin_MallaVertical,
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
                }
            }
            catch (Exception ex)
            {
                ConstNH.sbLog.AppendLine($"Error en 'M1_1_GEnerarListaIntervaloBarrasDTO'. ex: {ex.Message} ");
                lista.Clear();
            }
            return lista;
        }


    }
}
