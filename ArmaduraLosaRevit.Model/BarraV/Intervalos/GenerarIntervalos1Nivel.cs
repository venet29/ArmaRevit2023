using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
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

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public class GenerarIntervalos1Nivel : AGenerarIntervalosV, IGenerarIntervalosV
    {

        private IIniciarConXTraslapo _iniciarSinTraslapo;

        public GenerarIntervalos1Nivel(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO,
            SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
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
            bool IsPrimeraBarra = true;
            XYZ _PtoFinalIntervaloBarra = _selecionarPtoSup._PtoFinalIntervaloBarra;
            XYZ _PtoInicioIntervaloBarra = _selecionarPtoSup._PtoInicioIntervaloBarra;

            XYZ _PtoFinalIntervaloBarra_mallaV = _selecionarPtoSup._PtoFinalIntervaloBarra_mallaVertiva;
            XYZ _PtoInicioIntervaloBarra_mallaV = _selecionarPtoSup._PtoInicioIntervaloBarra_mallaVertiva;

            foreach (CoordenadasBarra item in _iniciarSinTraslapo.ListaCoordenadasBarra)
            {

                IntervaloBarrasDTO interBArraDto = new IntervaloBarrasDTO(_muroSeleccionadoInicialDTO, _confiWPFEnfierradoDTO);
                interBArraDto.IsUltimoTramoCOnMouse = item.IsUltimoTramoCOnMouse;
                //busca muro 50cm sobre el pto inicial del muroi
                //  XYZ PtoInicalAUX = _muroSeleccionadoInicialDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(item.ptoIni_foot.Z);
               // XYZ PtoInicalAUX = _selecionarPtoSup._ptoSeleccionMouseCentroCaraMuro.AsignarZ(item.ptoBusqueda_muro.Z);
                XYZ PtoInicalAUX = _selecionarPtoSup._ptoSeleccionMouseCentroCaraMuro.AsignarZ(item.ptoIni_foot.Z);
                BuscarMurosDTO _buscarMurosDTO = null;

                if (_muroSeleccionadoInicialDTO.elementoContenedor is Wall)
                {
                    BuscarMuros muroHost = BuscarMuroPerpendicularVIew(PtoInicalAUX);
                    if (muroHost == null) continue;
                    interBArraDto.M1_AsignarElementoHost(muroHost, _uiapp.ActiveUIDocument.Document);

                    _buscarMurosDTO = muroHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                 _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                }
                else if (_muroSeleccionadoInicialDTO.elementoContenedor is FamilyInstance)
                {
                    FamilyInstance _familyInstanceSelect = (FamilyInstance)_muroSeleccionadoInicialDTO.elementoContenedor;

                    if (_familyInstanceSelect.Category.Name == "Structural Columns") //viga
                    {

                        BuscarColumna vigaHost = new BuscarColumna(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                        if (vigaHost == null) continue;

                        vigaHost.AsignarColumna( _muroSeleccionadoInicialDTO);
                        interBArraDto.M1_AsignarElementoHost(vigaHost, _uiapp.ActiveUIDocument.Document);
                        _buscarMurosDTO = vigaHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                     _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                    }
                    else
                    {
                        BuscarViga vigaHost = new BuscarViga(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                        if (vigaHost == null) continue;
                        vigaHost.AsignarViga(_familyInstanceSelect, _muroSeleccionadoInicialDTO);
                        interBArraDto.M1_AsignarElementoHost(vigaHost, _uiapp.ActiveUIDocument.Document);
                        _buscarMurosDTO = vigaHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                     _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                    }
                 
                }
                else
                    continue;

                //asignar punto incial y final
                //interBArraDto.M2_AsiganrCoordenadasV(item.ptoIni_foot.Z, item.ptoFin_foot.Z, moverPorTraslapo);

                double valor = Math.Abs(Util.GetProductoEscalar(_muroSeleccionadoInicialDTO.DireccionLineaBarra, _view.RightDirection));
                //asignar punto incial y final
                if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza)
                {
                    interBArraDto.M2_AsiganrCoordenadasV(item.ptoIni_foot.Z, item.ptoFin_foot.Z, moverPorTraslapo,IsPrimeraBarra);
                    interBArraDto.tipobarraV = item.tipoBarraV;
                }
                else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza_BarraVHorquilla)
                {
                    interBArraDto.M2_AsiganrCoordenadasV(item.ptoIni_foot.Z, item.ptoFin_foot.Z, moverPorTraslapo, IsPrimeraBarra);

                    interBArraDto.ptoPosicionTAg = interBArraDto.ptoPosicionTAg + new XYZ(0, 0, Util.CmToFoot(180));
                    interBArraDto.tipobarraV = TipoPataBarra.BarraVPataAmbos_Horquilla;                 
                    interBArraDto.Largopata = _confiWPFEnfierradoDTO.IntervaloBarras_HorqDTO_.LargoPataBarra;                    
                }
                else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV && _confiWPFEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Manual)
                    interBArraDto.M2_AsiganrCoordenadasV_reccorridoParaleloView(item.ptoIni_foot.Z, item.ptoFin_foot.Z,  moverPorTraslapo, _uidoc.Document);
                else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV && _confiWPFEnfierradoDTO.CasoAnalisasBarrasElevacion_ == CasoAnalisasBarrasElevacion.Automatico)
                    interBArraDto.M2_AsiganrCoordenadasV_reccorridoParaleloView_Auto(item.ptoIni_foot.Z, item.ptoFin_foot.Z, _PtoInicioIntervaloBarra_mallaV, _PtoFinalIntervaloBarra_mallaV, 
                                                                                    moverPorTraslapo, _uidoc.Document);
                else if (_confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaH)
                    interBArraDto.M2_AsiganrCoordenadasH_reccorridoParaleloView(item.ptoIni_foot.Z, item.ptoFin_foot.Z, _selecionarPtoSup);



                interBArraDto.IsbarraIncial = AuxIsbarraIncial;
                 IsPrimeraBarra = false;
                AuxIsbarraIncial = false;
                //string _tipoBarraV = "BarraVSinPatas";
                //interBArraDto.tipobarraV = TipoBarraV.BarraVPataAmbos;// ObtenertipoBarraV(_tipoBarraV);
                
                interBArraDto.BuscarPatasAmbosLadosVertical(_uiapp, _buscarMurosDTO, _confiWPFEnfierradoDTO.TipoBarraRebar_);

                moverPorTraslapo = AyudaMoverBarras.MOverBarras(moverPorTraslapo);
                _PtoInicioIntervaloBarra = _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z);

                lista.Add(interBArraDto);
            }

            return lista;
        }

        //private TipoPataBarra ObtenertipoBarraV(string tipoBarraV)
        //{
        //    if (tipoBarraV == "BarraVSinPatas")
        //        return TipoPataBarra.BarraVSinPatas;
        //    else if (tipoBarraV == "BarraVPataInicial")
        //        return TipoPataBarra.BarraVPataInicial;
        //    else if (tipoBarraV == "BarraVPataFinal")
        //        return TipoPataBarra.BarraVPataFinal;
        //    else if (tipoBarraV == "BarraVPataAmbos")
        //        return TipoPataBarra.BarraVPataAmbos;
        //    return TipoPataBarra.BarraVSinPatas;
        //}
    }
}
