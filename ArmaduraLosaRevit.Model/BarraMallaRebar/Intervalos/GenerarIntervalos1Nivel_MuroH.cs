using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar.Intervalos
{
    public class GenerarIntervalos1Nivel_MuroH : AGenerarIntervalosV, IGenerarIntervalosV
    {

        private IIniciarConXTraslapo _iniciarSinTraslapo;

        public GenerarIntervalos1Nivel_MuroH(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
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

                if (_confiWPFEnfierradoDTO.tipobarraH == TipoPataBarra.NoBuscar && _confiWPFEnfierradoDTO.IntervalosCantidadBArras.Length==3)
                    _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.NoBuscar;


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
            XYZ _PtoFinalIntervaloBarra = _selecionarPtoSup._PtoFinalIntervaloBarra_ProyectadoCaraMuroHost;
            XYZ _PtoInicioIntervaloBarra = _selecionarPtoSup._PtoInicioIntervaloBarra;
            XYZ _direccionInicialToFinal = (_PtoFinalIntervaloBarra - _PtoInicioIntervaloBarra).Normalize();
            foreach (CoordenadasBarra item in _iniciarSinTraslapo.ListaCoordenadasBarra)
            {
                ResultBuscarMurosDTO _ResultBuscarMurosDTO = ObtenerBuscarMurosDTO(item, _PtoInicioIntervaloBarra, _PtoFinalIntervaloBarra);

                if (!_ResultBuscarMurosDTO.Isok) continue;

                IntervaloBarrasDTO interBArraDto = _ResultBuscarMurosDTO._interBArraDto;
                BuscarMurosDTO _buscarMurosDTO = _ResultBuscarMurosDTO._buscarMurosDTO;

                //a)busca  fundaciones para prolongar hasta 5 cm sobre nivel inferior de fundacion
                BuscarFundacionLosa _buscarElementosBajo = new BuscarFundacionLosa(_uiapp, Util.CmToFoot(150));
                if (_buscarElementosBajo.OBtenerRefrenciaFundacionSegunVector(interBArraDto._view3D_paraBuscar, _PtoInicioIntervaloBarra + new XYZ(0, 0, 1), new XYZ(0, 0, -1)))
                {
                    var fundacionesMAslejana = _buscarElementosBajo._listaFundLosaEncontrado.OrderByDescending(c => c.distancia).FirstOrDefault();
                    if (fundacionesMAslejana != null)
                        item.ptoIni_foot = item.ptoIni_foot.AsignarZ(fundacionesMAslejana.PtoSObreCaraInferiorFundLosa.Z + ConstNH.RECUBRIMIENTO_FUNDACIONES_foot + Util.CmToFoot(interBArraDto.diametroMM));
                }
                
                //b) PATA INICIAL--> buscar muro perpendicular al inicio para para generar pata de 35 cm
                XYZ ptoInicial = _PtoInicioIntervaloBarra + new XYZ(0, 0, 1) + _view.ViewDirection * Util.CmToFoot(2);
                // CrearModeLineAyuda.modelarlineas(_doc, ptoInicial, ptoInicial + (_muroSeleccionadoInicialDTO._EspesorMuroFoot + Util.CmToFoot(20) * 2) * -_view.ViewDirection);

                double ZmedioINtervalo = (_PtoInicioIntervaloBarra.Z + _PtoFinalIntervaloBarra.Z) / 2;
                bool IsDebeTenerPataInicio = VerificarSiDebeTenerPata(-_view.RightDirection, ptoInicial.AsignarZ(ZmedioINtervalo), interBArraDto._view3D_paraBuscar);
                interBArraDto._tipoPataMallaInicial = (IsDebeTenerPataInicio ? interBArraDto._tipoPataMallaInicial : TipoPaTaMalla.SinBordeMuro);

               BuscarMuros _buscarPerpendicular = new BuscarMuros(_uiapp, _muroSeleccionadoInicialDTO._EspesorMuroFoot + Util.CmToFoot(20) * 2);
                List<ElementId> listaExclusion = new List<ElementId>();
                listaExclusion.Add(_muroSeleccionadoInicialDTO.IdelementoContenedor);
                if (_buscarPerpendicular.OBtenerBuscarMuroPerpendicular(interBArraDto._view3D_paraBuscar,
                                                                         ptoInicial,
                                                                         -_view.ViewDirection,
                                                                         listaExclusion))
                {
                    interBArraDto._tipoPataMallaInicial = TipoPaTaMalla.intersecccionMuro;
                }
                else if (_buscarPerpendicular.OBtenerBuscarMuroPerpendicular(interBArraDto._view3D_paraBuscar,
                                                                         ptoInicial - _view.ViewDirection * Util.CmToFoot(4),
                                                                         _direccionInicialToFinal,
                                                                         listaExclusion))
                { 
                    interBArraDto._tipoPataMallaInicial = TipoPaTaMalla.intersecccionMuro;
                }

                //C) PATA FINAL buscar muro perpendicular al Final para para generar pata de 35 cm
                ptoInicial = _PtoFinalIntervaloBarra - new XYZ(0, 0, 1) + _view.ViewDirection * Util.CmToFoot(2);
               
                
                bool IsDebeTenerPataFin = VerificarSiDebeTenerPata(_view.RightDirection, ptoInicial.AsignarZ(ZmedioINtervalo), interBArraDto._view3D_paraBuscar);
                interBArraDto._tipoPataMallaFinal = (IsDebeTenerPataFin ? interBArraDto._tipoPataMallaFinal : TipoPaTaMalla.SinBordeMuro);


                BuscarMuros _buscarPerpendicularFinal = new BuscarMuros(_uiapp, _muroSeleccionadoInicialDTO._EspesorMuroFoot + Util.CmToFoot(20) * 2);
                listaExclusion.Clear();
                listaExclusion.Add(_muroSeleccionadoInicialDTO.IdelementoContenedor);
                if (_buscarPerpendicularFinal.OBtenerBuscarMuroPerpendicular(interBArraDto._view3D_paraBuscar,
                                                                         ptoInicial,
                                                                         -_view.ViewDirection,
                                                                         listaExclusion))
                {
                    interBArraDto._tipoPataMallaFinal = TipoPaTaMalla.intersecccionMuro;
                }
                else if (_buscarPerpendicularFinal.OBtenerBuscarMuroPerpendicular(interBArraDto._view3D_paraBuscar,
                                                                   ptoInicial- _view.ViewDirection * Util.CmToFoot(4),
                                                                   -_direccionInicialToFinal,
                                                                   listaExclusion))
                {
                    interBArraDto._tipoPataMallaFinal = TipoPaTaMalla.intersecccionMuro;
                }

                //D DEFINICION
                interBArraDto._tipoLineaMallaH = ObtenerTipoPata(interBArraDto);

                interBArraDto.M2_AsiganrCoordenadasH_reccorridoParaleloView(item.ptoIni_foot.Z, item.ptoFin_foot.Z, _selecionarPtoSup);

                interBArraDto.IsbarraIncial = AuxIsbarraIncial;

                AuxIsbarraIncial = false;
                interBArraDto.BuscarPatasAmbosLadosVertical(_uiapp, _buscarMurosDTO, _confiWPFEnfierradoDTO.TipoBarraRebar_);

                moverPorTraslapo = !moverPorTraslapo;
                _PtoInicioIntervaloBarra = _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z);

                lista.Add(_ResultBuscarMurosDTO._interBArraDto);
            }

            return lista;
        }

        private bool VerificarSiDebeTenerPata(XYZ direccion, XYZ pto, View3D _View3D)
        {
            try
            {
                pto = pto + - _view.ViewDirection * Util.CmToFoot(10) + -direccion * Util.CmToFoot(2); // retrocedor 2 cm por si selecionnan borde de muro
                BuscarElementosHorizontal _buscarElementosBajo = new BuscarElementosHorizontal(_uiapp, ConstNH.RECUBRIMIENTO_MALLA_foot+ Util.CmToFoot(20), _View3D);
                if (_buscarElementosBajo.BuscarObjetos(pto, direccion))
                {
                    return (_buscarElementosBajo.listaObjEncontrados.Count > 0 ? true : false);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ex:{ex.Message}");
            }
            return false;
        }

        private TipoPataBarra ObtenerTipoPata(IntervaloBarrasDTO interBArraDto)
        {

            if (interBArraDto._tipoLineaMallaH == TipoPataBarra.NoBuscar) return TipoPataBarra.BarraVSinPatas;
            if (interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro || interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro)
            {
                if (interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro && interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro)
                    return TipoPataBarra.BarraVPataAmbos;
                else if (interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.intersecccionMuro)
                    return TipoPataBarra.BarraVPataInicial;
                else if (interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.intersecccionMuro)
                    return TipoPataBarra.BarraVPataFinal;
            }
           else if (interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.SinBordeMuro || interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.SinBordeMuro)
            {
                if (interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.SinBordeMuro && interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.SinBordeMuro)
                    return TipoPataBarra.BarraVSinPatas;
                else if (interBArraDto._tipoPataMallaInicial == TipoPaTaMalla.SinBordeMuro)
                    return TipoPataBarra.BarraVPataFinal;
                else if (interBArraDto._tipoPataMallaFinal == TipoPaTaMalla.SinBordeMuro)
                    return TipoPataBarra.BarraVPataInicial;
            }


            return interBArraDto._tipoLineaMallaH;
        }
    }
}
