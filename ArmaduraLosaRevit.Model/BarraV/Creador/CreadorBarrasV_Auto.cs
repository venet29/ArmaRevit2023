using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.Traslapos;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Creador
{
    public class CreadorBarrasV_Auto
    {

        private List<IbarraBase> _listaDebarra;
        private UIApplication _uiapp;
        private Document _doc;
        private ConfiguracionIniciaWPFlBarraVerticalDTO _confiEnfierradoDTO;
        private readonly IntervalosBarraAutoDto _newIntervaloBarraAutoDto;
        private readonly DatosMuroSeleccionadoDTO _muroSeleccionadoDTO;
        private readonly bool IsDibujarTagBArras;

        public ManejadorTraslapo _manejadorTraslapo { get; set; }
        public List<ElementId> _listaRebarId { get; set; }
        public List<BarraIng> _listaBarraIng { get; set; }

        private ConfiguracionTAgBarraDTo confBarraTag;

        // IsDibujarTagBArras=true; dibuja barra por barras
        // IsDibujarTagBArras = dibuja las barras agrupadas
        public CreadorBarrasV_Auto(UIApplication uiapp,
                                   ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO,
                                   IntervalosBarraAutoDto newIntervaloBarraAutoDto,
                                   DatosMuroSeleccionadoDTO muroSeleccionadoDTO, bool IsDibijarTagBArras = true)
        {
            this._uiapp = uiapp;

            this._doc = uiapp.ActiveUIDocument.Document;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
            this._newIntervaloBarraAutoDto = newIntervaloBarraAutoDto;
            this._muroSeleccionadoDTO = muroSeleccionadoDTO;
            this.IsDibujarTagBArras = IsDibijarTagBArras;
            _listaRebarId = new List<ElementId>();
            _listaBarraIng = new List<BarraIng>();

            confBarraTag = new ConfiguracionTAgBarraDTo()
            {
                desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 1.5),
                IsDIrectriz = true,
                LeaderElbow = new XYZ(0, 0, 1.5),
                tagOrientation = TagOrientation.Vertical,
                BarraTipo = TipoRebar.ELEV_BA_V
            };
            this._manejadorTraslapo = new ManejadorTraslapo(uiapp);
        }

        public bool Ejecutar(double diamtroanterior)
        {
            try
            {
                _newIntervaloBarraAutoDto.diametroIntervaloAnteriorMM = diamtroanterior;
                M1_AsignarCAntidadEspaciemientoNuevaLineaBarra();
                M2_GenerarIntervalos();
                M3_DibujarBarras();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Dibujar barras automatico vertical--> ex:{ex.Message}");
                return false;
            }
            return true;
        }


        private void M1_AsignarCAntidadEspaciemientoNuevaLineaBarra()
        {
            int i = 0;
            _confiEnfierradoDTO.NuevaLineaCantidadbarra = _confiEnfierradoDTO.IntervalosCantidadBArras[i];
            if (i == 0)
                _confiEnfierradoDTO.EspaciamientoREspectoBordeFoot = ConstNH.CONST_RECUBRIMIENTO_PRIMERABARRAFOOT;
            else
                _confiEnfierradoDTO.EspaciamientoREspectoBordeFoot += Util.CmToFoot(_confiEnfierradoDTO.IntervalosEspaciamiento[i]);

            _confiEnfierradoDTO.NumeroBarraLinea = _confiEnfierradoDTO.IntervalosCantidadBArras[i];
        }

        private void M2_GenerarIntervalos()
        {
            IGenerarIntervalosV igenerarIntervalosAUTO = FactoryGenerarIntervalos.CrearGeneradorDeIntervalosVAUTO(_uiapp, _confiEnfierradoDTO, _newIntervaloBarraAutoDto, _muroSeleccionadoDTO);
            igenerarIntervalosAUTO.M1_ObtenerIntervaloBarrasDTO();
            igenerarIntervalosAUTO.M2_GenerarListaBarraVertical();
            _listaDebarra = igenerarIntervalosAUTO.ListaIbarraVertical;
        }

        private void M3_DibujarBarras()
        {
            //dibujar tag
            try
            {


                int i = 0;
                foreach (IbarraBase item in _listaDebarra)
                {
                    _manejadorTraslapo.AgregarbarrasInicialTraslpoa();
                    item.M1_DibujarBarra();


                    if (IsDibujarTagBArras) item.M2_DibujarTags(confBarraTag);
                    ElementId idreabar = null;
                    Rebar nuevaBArra = default;
                    try
                    {
                         idreabar = item.M3_ObtenerIdRebar();
                        if (idreabar != null)
                        {
                            nuevaBArra = item.GetResult()._rebar;
                            if (i == 0 && AyudaManejadorTraslapo.IsContinudadDebarra(nuevaBArra))
                            {
                                _manejadorTraslapo.AsignarBarrasIntervaloAnterior();
                            }

                            _listaRebarId.Add(idreabar);
                            var ptoTag = item.GetResult().ptoPosicionTAg;// GetIsPo_P2paraTag();
                            BarraIng ingBarras_aux = new BarraIng(item, _newIntervaloBarraAutoDto, ptoTag);
                            //ingBarras_aux.ObtenerDatos(_doc.ActiveView.Origin);
                            //ingBarras_aux.ObtenerDatos_CantidadDiametr0();
                            _listaBarraIng.Add(ingBarras_aux);
                            _manejadorTraslapo.AgregarbarrasFinalTraslpoa(i, nuevaBArra);
                        }
                        else
                            _manejadorTraslapo.Reset();

                        if (_listaDebarra.Count == 1)
                            AyudaManejadorTraslapo.IsInicialIntervalo = true;
                    }
                    catch (Exception  ex)
                    {
                        Util.DebugDescripcion(ex);
                        
                    }
                    i = 1 + i;
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }


        public void M4_DibujarBarrasCOnfiguracion()
        {
            //dibujar configuraciones
            try
            {
                foreach (IbarraBase item in _listaDebarra)
                {
                    item.M1_1_DibujarBarraCOnfiguracion();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }
    }
}
