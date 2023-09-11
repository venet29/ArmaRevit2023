using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{

    //solo para barras horizontals o inclinadas  /// no consideradas para laterales ni mallas horizonales de muro
    public class GenerarIntervalosSINNivel : IGenerarIntervalosH
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected View _view;

        protected ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO;
        protected SelecionarPtoHorizontal _selecionarPtoHorizontal;
        protected DatosMuroSeleccionadoDTO _vigaSeleccionadoDTO;
        protected IniciarSinTraslapoH _iniciarSinTraslapo;
        public List<IbarraBase> ListaIbarraHorizontal { get; set; }

        public List<IntervaloBarrasDTO> ListaIntervaloBarrasDTO { get; set; }

        public GenerarIntervalosSINNivel(UIApplication _uiapp, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO,
                                         SelecionarPtoHorizontal selecionarPtoHorizontal, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)

        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
            this._selecionarPtoHorizontal = selecionarPtoHorizontal;
            this._vigaSeleccionadoDTO = muroSeleccionadoDTO;
            ListaIntervaloBarrasDTO = new List<IntervaloBarrasDTO>();
        }

        public void M1_ObtenerIntervaloBarrasDTO()
        {
            List<IntervaloBarrasDTO> lista = new List<IntervaloBarrasDTO>();

            try
            {
                _iniciarSinTraslapo = new IniciarSinTraslapoH(_selecionarPtoHorizontal, _confiEnfierradoDTO);
                _iniciarSinTraslapo.CalcularIntervalo();
                M1_1_GEnerarListaIntervaloBarrasDTO();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" Error ex:{ex.Message} ");
                lista.Clear();

            }

        }

        protected void M1_1_GEnerarListaIntervaloBarrasDTO()
        {
            bool moverPorTraslapo = false;
            bool IsPrimeraBarra = true;
            foreach (CoordenadasBarra item in _iniciarSinTraslapo.ListaCoordenadasBarra)
            {
                IntervaloBarrasDTO interBArraDto = new IntervaloBarrasDTO(_vigaSeleccionadoDTO, _confiEnfierradoDTO);
                // BuscarMurosDTO _buscarMurosDTO = null;
                if (_vigaSeleccionadoDTO.elementoContenedor is Wall)
                {
                    BuscarMuros muroHost = new BuscarMuros(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                    if (muroHost == null) continue;
                    muroHost.AsignarMuro(_vigaSeleccionadoDTO.elementoContenedor);

                    interBArraDto.M1_AsignarElementoHost(muroHost, _uiapp.ActiveUIDocument.Document);
                    // _buscarMurosDTO = muroHost.GetBuscarMurosDTO();
                }
                else
                {
                    BuscarViga vigaHost = new BuscarViga(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                    if (vigaHost == null) continue;
                    vigaHost.AsignarViga((FamilyInstance)_vigaSeleccionadoDTO.elementoContenedor, _vigaSeleccionadoDTO);

                    interBArraDto.M1_AsignarElementoHost(vigaHost, _uiapp.ActiveUIDocument.Document);
                    //  _buscarMurosDTO = vigaHost.GetBuscarMurosDTO();
                }

                //asignar punto incial y final
                Line auxlinea = Line.CreateBound(_vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost - _vigaSeleccionadoDTO.direccionBordeElemeto * 1,
                    _vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost + _vigaSeleccionadoDTO.direccionBordeElemeto * 1);

                // CrearModeLineAyuda.modelarlineas(_doc, auxlinea.GetEndPoint(0), auxlinea.GetEndPoint(1));

                XYZ pinit = _selecionarPtoHorizontal._PtoInicioIntervaloBarra6.AsignarZ(_vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.Z);
                pinit = auxlinea.ProjectExtendida3D(_selecionarPtoHorizontal._PtoInicioIntervaloBarra6);

                XYZ pfin = _selecionarPtoHorizontal._PtoFinalIntervaloBarra.AsignarZ(_vigaSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.Z);
                pfin = auxlinea.ProjectExtendida3D(_selecionarPtoHorizontal._PtoFinalIntervaloBarra);


                //if (_confiEnfierradoDTO.TipoBarraRefuerzoViga == TipoBarraRefuerzoViga.RefuerzoCentral)
                //    interBArraDto.M2_AsiganrCoordenadasH_vigaAuto(pinit, pfin, moverPorTraslapo, IsPrimeraBarra);
                //else

                interBArraDto.M2_AsiganrCoordenadasH(pinit, pfin, moverPorTraslapo, IsPrimeraBarra);

                //TipoEmpotramiento _conEmpotramiento = TipoEmpotramiento.sin;
                //string _tipoBarraV = "BarraVSinPatas";
                //interBArraDto.tipobarraV = TipoBarraV.BarraVPataAmbos;// ObtenertipoBarraV(_tipoBarraV);
                interBArraDto.BuscarPatasAmbosLadosHorizontal(_uiapp, _confiEnfierradoDTO._empotramientoPatasDTO);

                IsPrimeraBarra = false;
                moverPorTraslapo = AyudaMoverBarras.MOverBarras(moverPorTraslapo);
                ListaIntervaloBarrasDTO.Add(interBArraDto);
            }

        }



        public virtual List<IbarraBase> M2_GenerarListaBarraHorizontal()
        {
            ListaIbarraHorizontal = new List<IbarraBase>();
            try
            {
                foreach (IntervaloBarrasDTO itemIntervaloBarrasDTO in ListaIntervaloBarrasDTO)
                {
                    IGeometriaTag _newGeometriaTag = FactoryGeomTagRebarH.CrearGeometriaTagH(_uiapp,
                                                                                            itemIntervaloBarrasDTO.tipobarraV,
                                                                                            itemIntervaloBarrasDTO.ptoini,
                                                                                            itemIntervaloBarrasDTO.ptofinal,
                                                                                            itemIntervaloBarrasDTO.ptoPosicionTAg,
                                                                                            -itemIntervaloBarrasDTO.DireccionPataEnFierrado * (itemIntervaloBarrasDTO.DireccionPataEnFierrado.Z == 1 ? 0.8 : 0.2));

                    _newGeometriaTag.Ejecutar(GeomeTagArgs.ValorDefaul());

                    IbarraBase newIbarraVertical = FactoryBarraHorizontal.GeneraraIbarraHorizontal(_uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                    newIbarraVertical.IsSoloTag = _vigaSeleccionadoDTO.soloTag1;
                    // newIbarraVertical.M0_CalcularCurva();
                    ListaIbarraHorizontal.Add(newIbarraVertical);
                }
            }
            catch (Exception)
            {
                ListaIbarraHorizontal.Clear();
                return ListaIbarraHorizontal;
            }
            return ListaIbarraHorizontal;
        }
    }
}
