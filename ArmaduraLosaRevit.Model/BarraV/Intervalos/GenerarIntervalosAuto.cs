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
    public class GenerarIntervalosAuto : AGenerarIntervalosV, IGenerarIntervalosV
    {
        private IntervalosBarraAutoDto _newIntervaloBarraAutoDto;

        public bool moverPorTraslapo { get; set; }
        public bool AuxIsbarraIncial { get; set; }

        private IIniciarConXTraslapo _iniciarCon1Traslapo;

        public GenerarIntervalosAuto(UIApplication _uiapp,
            ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO,
            IntervalosBarraAutoDto newIntervaloBarraAutoDto,
              DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
            : base(_uiapp, confiEnfierradoDTO, muroSeleccionadoDTO)
        {
            this._newIntervaloBarraAutoDto = newIntervaloBarraAutoDto;
            //moverPorTraslapo = IntervalosBarraAutoDto.moverPorTraslapo;
            moverPorTraslapo = AyudaMoverBarras.MOverBarras(_newIntervaloBarraAutoDto.AuxIsbarraIncial); 
            AuxIsbarraIncial = newIntervaloBarraAutoDto.AuxIsbarraIncial;
        }


        public override void M1_ObtenerIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();
            try
            {
                _iniciarCon1Traslapo = new IniciarConAuto(_newIntervaloBarraAutoDto);
                _iniciarCon1Traslapo.CalcularIntervalo();

                ListaIntervaloBarrasDTO = M1_1_GEnerarListaIntervaloBarrasDTO();
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

            double diamtroIntervaloAnteriorMM = _newIntervaloBarraAutoDto.diametroIntervaloAnteriorMM;

            foreach (CoordenadasBarra item in _iniciarCon1Traslapo.ListaCoordenadasBarra)
            {

                //diamtroIntervaloAnteriorMM = _newIntervaloBarraAutoDto.diametroIntervaloAnteriorMM;
                IntervaloBarrasDTO interBArraDto = new IntervaloBarrasDTO(_muroSeleccionadoInicialDTO, _confiWPFEnfierradoDTO, _newIntervaloBarraAutoDto);
                interBArraDto.OrientacionTagGrupoBarras = item.OrientacionTag;
                //busca muro 50cm sobre el pto inicial del muroi

                //BuscarMuros muroHost = BuscarMuroPerpendicularVIew_conPtoCentroMuro(_newIntervaloBarraAutoDto.PtoCentralSobreMuro.AsignarZ(item.ptoIni_foot.Z));
                //if (muroHost == null) continue;
                //interBArraDto.M1_AsignarElementoHost(muroHost, _uiapp.ActiveUIDocument.Document);

                BuscarMurosDTO _buscarMurosDTO = null;

                if (_muroSeleccionadoInicialDTO.elementoContenedor is Wall)
                {
                    BuscarMuros muroHost = null;
                    XYZ ptoBuscarMuro = ((item.ptoIni_pier_foot + item.ptoFin_pier_foot) / 2).AsignarZ(item.ptoIni_pier_foot.Z+ Math.Min(1.5, item.ptoFin_pier_foot.Z- item.ptoIni_pier_foot.Z));
                    bool mantaner = true;
                    int cont = 0;
                    while (mantaner && cont < 10)
                    {


                        muroHost = BuscarMuroPerpendicularVIew_conPtoCentroMuro(ptoBuscarMuro - _muroSeleccionadoInicialDTO.NormalEntradoView * ConstNH.CONST_DISTANCIA_RETROCESO_BUSQUEDA_FOOT_PERPENDICULAR_VIEW);

                        if (muroHost != null)
                        { mantaner = false; }
                        else
                        {
                            //obs1)cuando se busca un muro y este es perpendicular ala vista, se mover 10 cm el pto de b
                            ptoBuscarMuro = ptoBuscarMuro + _muroSeleccionadoInicialDTO.DireccionEnFierrado * ConstNH.CONST_MOVER_PTO_BUSCAR_MURO_FOOT;
                        }
                        cont += 1;
                    }

                    if (muroHost == null) continue;
                    interBArraDto.M1_AsignarElementoHost(muroHost, _uiapp.ActiveUIDocument.Document);

                    _buscarMurosDTO = muroHost.GetBuscarMurosDTO(item.ptoIni_pier_foot, item.ptoFin_pier_foot);
                }
                else
                {
                    BuscarViga vigaHost = new BuscarViga(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                    if (vigaHost == null) continue;
                    vigaHost.AsignarViga((FamilyInstance)_muroSeleccionadoInicialDTO.elementoContenedor, _muroSeleccionadoInicialDTO);

                    interBArraDto.M1_AsignarElementoHost(vigaHost, _uiapp.ActiveUIDocument.Document);
                    _buscarMurosDTO = vigaHost.GetBuscarMurosDTO(item.ptoIni_pier_foot, item.ptoFin_pier_foot);
                }


                Orientacion orientacionGrupoTAg = _newIntervaloBarraAutoDto.OrientacionTagGrupoBarras;
                //asignar punto incial y final
                interBArraDto.M2_AsiganrCoordenadasVAuto(item, moverPorTraslapo, diamtroIntervaloAnteriorMM);
                interBArraDto.tipobarraV = item.tipoBarraV;
                interBArraDto.IsbarraIncial = AuxIsbarraIncial;
                AuxIsbarraIncial = false;
                diamtroIntervaloAnteriorMM = interBArraDto.diametroMM;
                interBArraDto.BuscarPatasAmbosLadosVertical(_uiapp, _buscarMurosDTO);

                moverPorTraslapo = AyudaMoverBarras.MOverBarras(moverPorTraslapo);

                lista.Add(interBArraDto);
            }

            IntervalosBarraAutoDto.moverPorTraslapo = moverPorTraslapo;
            return lista;
        }


    }
}
