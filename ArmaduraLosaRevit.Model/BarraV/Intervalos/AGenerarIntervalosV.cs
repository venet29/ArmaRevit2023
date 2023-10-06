using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
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
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public abstract class AGenerarIntervalosV
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected UIDocument _uidoc;
        protected View _view;
        protected ConfiguracionIniciaWPFlBarraVerticalDTO _confiWPFEnfierradoDTO;


        protected List<double> ListaLevelIntervalo;
        protected DatosMuroSeleccionadoDTO _muroSeleccionadoInicialDTO;
        protected DatosMuroSeleccionadoDTO muroSeleccionadoDTO;
        public SelecionarPtoSup _selecionarPtoSup { get; set; }
        public List<IntervaloBarrasDTO> ListaIntervaloBarrasDTO { get; set; }
        public List<IbarraBase> ListaIbarraVertical { get; set; }

        public IIniciarConXTraslapo _iniciarConXTraslapo { get; set; }

        public AGenerarIntervalosV(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confWPFiEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            this._confiWPFEnfierradoDTO = confWPFiEnfierradoDTO;
            this._selecionarPtoSup = selecionarPtoSup;
            this.ListaLevelIntervalo = selecionarPtoSup.ListaLevelIntervalo;
            this._muroSeleccionadoInicialDTO = muroSeleccionadoDTO;
            ListaIntervaloBarrasDTO = new List<IntervaloBarrasDTO>();
            ListaIbarraVertical = new List<IbarraBase>();

        }

        protected AGenerarIntervalosV(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            this._confiWPFEnfierradoDTO = confiEnfierradoDTO;

            this.ListaLevelIntervalo = new List<double>();
            this._muroSeleccionadoInicialDTO = muroSeleccionadoDTO;
            ListaIntervaloBarrasDTO = new List<IntervaloBarrasDTO>();
            ListaIbarraVertical = new List<IbarraBase>();
        }

        public abstract void M1_ObtenerIntervaloBarrasDTO();

        public virtual List<IbarraBase> M2_GenerarListaBarraVertical()
        {
            ListaIbarraVertical = new List<IbarraBase>();
            try
            {
                foreach (IntervaloBarrasDTO itemIntervaloBarrasDTO in ListaIntervaloBarrasDTO)
                {
                    itemIntervaloBarrasDTO.BarraTipo = _confiWPFEnfierradoDTO.BarraTipo;
                    itemIntervaloBarrasDTO._intervaloBarras_HorqDTO = _confiWPFEnfierradoDTO.IntervaloBarras_HorqDTO_;
                    IbarraBase newIbarraVertical = FactoryBarraVertical.FActoryIGeometriaTagVertical(_uiapp, itemIntervaloBarrasDTO, _confiWPFEnfierradoDTO);
                    ListaIbarraVertical.Add(newIbarraVertical);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" AGenerarIntervalosV   --  >ex:{ex.Message}");
                ListaIbarraVertical.Clear();
                return ListaIbarraVertical;
            }
            return ListaIbarraVertical;
        }

        public BuscarMuros BuscarMuroPerpendicularVIew(XYZ ptoBUsqueda)
        {
            int contador = 1;
            bool continuar = true;
           // Element wallSeleccionado = null;

            List<BuscarMurosEstado> ListaResuult = new List<BuscarMurosEstado>();

            BuscarMuros BuscarMuros = null;
            //ptoBUsqueda = ptoBUsqueda + new XYZ(0, 0, Util.CmToFoot(50));
            while (continuar && contador < 10)
            {
                //mueve el to 50 cm hacia arriba respecto al pto inicial de la barra
            
                BuscarMuros = new BuscarMuros(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                XYZ ptobusquedaMuro = ptoBUsqueda +
                                    -_muroSeleccionadoInicialDTO.NormalEntradoView * Util.CmToFoot(20) + new XYZ(0, 0, Util.CmToFoot(20)) * (contador) +  //mueve pto 2 cm separado de la cara del muero inicial. 2cm hacia la vista y sub 20 cm
                                    _muroSeleccionadoInicialDTO.DireccionEnFierrado * ConstNH.CONST_DESPLZAMIENTO_BUSQUEDA_MUROFOOT * contador; //muevo el pto en direccion del muro

                bool IsDIbujarLinea = false;
#if DEBUG
       //IsDIbujarLinea = true;
#endif
                var (wallSeleccionado, espesor, ptoSobreMuro) = BuscarMuros.OBtenerRefrenciaMuro(_confiWPFEnfierradoDTO.view3D_paraBuscar, ptobusquedaMuro, _muroSeleccionadoInicialDTO.NormalEntradoView, null,IsDIbujarLinea);

                ListaResuult.Add(BuscarMuros.BuscarMurosEstado_);
                contador += 1;
                if (wallSeleccionado != null) continuar = false;
             
            }


            if (ListaResuult.Contains(BuscarMurosEstado.MurosEncontradoConerror) && !ListaResuult.Contains(BuscarMurosEstado.MuroEncontrado))
            {
                BuscarMuros.BuscarMurosEstado_ = BuscarMurosEstado.MurosEncontradoConerror;
                Util.ErrorMsg($"Se se encontro muros para enfirrar  id:{BuscarMuros.WalloVigaElementHost?.Id}, pero presentan error.\na)Puede que el muro no este paralelo a la vista.\nb)Puede que el muro no este marcado estructural");
            }
            else if (BuscarMuros.BuscarMurosEstado_ == BuscarMurosEstado.MuroNoEncontrado)
            {
                BuscarMuros.BuscarMurosEstado_ = BuscarMurosEstado.MuroNoEncontrado;
                Util.ErrorMsg("No se encontro muro para enfierrar en la coodenadas ingresadas.");
            }

            return BuscarMuros;
        }

        //usado en diseño auto
        public BuscarMuros BuscarMuroPerpendicularVIew_conPtoCentroMuro(XYZ ptoCentroMuro)
        {
#if DEBUG
            // CrearModeLineAyuda.modelarlinea_sinTrans(_doc, ptoCentroMuro, ptoCentroMuro + _muroSeleccionadoInicialDTO.NormalEntradoView * 2);
#endif
            //mueve el to 50 cm hacia arriba respecto al pto inicial de la barra
            ptoCentroMuro = ptoCentroMuro + new XYZ(0, 0, Util.CmToFoot(50));
            BuscarMuros BuscarMuros = new BuscarMuros(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
            XYZ ptobusquedaMuro = ptoCentroMuro +
                                -_muroSeleccionadoInicialDTO.NormalEntradoView * Util.CmToFoot(2);// + mueve pto 2 cm separado de la cara del muero inicial. 2cm hacia la vista 
                                                                                                  //_muroSeleccionadoInicialDTO.DireccionEnFierrado * ConstantesGenerales.CONST_DISTANCIA_BUSQUEDA_MUROFOOT; //muevo el pto en direccion del muro
            var (wallSeleccionado, espesor, ptoSobreMuro) = BuscarMuros.OBtenerRefrenciaMuro(_confiWPFEnfierradoDTO.view3D_paraBuscar, ptobusquedaMuro, _muroSeleccionadoInicialDTO.NormalEntradoView);
            if (wallSeleccionado == null) return null;
            return BuscarMuros;
        }

        public ResultBuscarMurosDTO ObtenerBuscarMurosDTO(CoordenadasBarra item, XYZ _PtoInicioIntervaloBarra, XYZ _PtoFinalIntervaloBarra)
        {
            IntervaloBarrasDTO interBArraDto = new IntervaloBarrasDTO(_muroSeleccionadoInicialDTO, _confiWPFEnfierradoDTO);

            BuscarMurosDTO _buscarMurosDTO = null;
            try
            {
                interBArraDto.IsUltimoTramoCOnMouse = item.IsUltimoTramoCOnMouse;
                //busca muro 50cm sobre el pto inicial del muroi
                //  XYZ PtoInicalAUX = _muroSeleccionadoInicialDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(item.ptoIni_foot.Z);
                XYZ PtoInicalAUX = _selecionarPtoSup._ptoSeleccionMouseCentroCaraMuro.AsignarZ(item.ptoBusqueda_muro.Z);

                if (_muroSeleccionadoInicialDTO.elementoContenedor is Wall)
                {
                    BuscarMuros muroHost = BuscarMuroPerpendicularVIew(PtoInicalAUX);
                    if (muroHost == null) return new ResultBuscarMurosDTO(false);
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
                        if (vigaHost == null) return new ResultBuscarMurosDTO(false);

                        vigaHost.AsignarColumna(_muroSeleccionadoInicialDTO);
                        interBArraDto.M1_AsignarElementoHost(vigaHost, _uiapp.ActiveUIDocument.Document);
                        _buscarMurosDTO = vigaHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                     _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                    }
                    else
                    {
                        BuscarViga vigaHost = new BuscarViga(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                        if (vigaHost == null) return new ResultBuscarMurosDTO(false);
                        vigaHost.AsignarViga(_familyInstanceSelect, _muroSeleccionadoInicialDTO);
                        interBArraDto.M1_AsignarElementoHost(vigaHost, _uiapp.ActiveUIDocument.Document);
                        _buscarMurosDTO = vigaHost.GetBuscarMurosDTO(_PtoInicioIntervaloBarra.AsignarZ(item.ptoIni_foot.Z),
                                                                     _PtoFinalIntervaloBarra.AsignarZ(item.ptoFin_foot.Z));
                    }
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return new ResultBuscarMurosDTO(false);
            }
            return new ResultBuscarMurosDTO(interBArraDto, _buscarMurosDTO); ;
        }


    }


    public class ResultBuscarMurosDTO
    {
        public bool Isok { get; set; } = true;
        public IntervaloBarrasDTO _interBArraDto { get; set; }
        public BuscarMurosDTO _buscarMurosDTO { get; set; }

        public ResultBuscarMurosDTO(bool isok)
        {
            this.Isok = isok;
        }

        public ResultBuscarMurosDTO(IntervaloBarrasDTO interBArraDto, BuscarMurosDTO buscarMurosDTO, bool isok = true)
        {
            this._interBArraDto = interBArraDto;
            this._buscarMurosDTO = buscarMurosDTO;
            this.Isok = isok;
        }



    }
}
