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

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public abstract class AGenerarIntervalosH 
    {
        protected UIApplication _uiapp;
        private UIDocument _uidoc;
        protected ConfiguracionIniciaWPFlBarraVerticalDTO _confiWPFEnfierradoDTO;
        protected SelecionarPtoSup _selecionarPtoSup;

        protected List<double> ListaLevelIntervalo;
        protected DatosMuroSeleccionadoDTO _muroSeleccionadoInicialDTO;

        public List<IntervaloBarrasDTO> ListaIntervaloBarrasDTO { get;  set; }
        public List<IbarraBase> ListaIbarraHorizontal { get;  set; }

        public IIniciarConXTraslapo _iniciarConXTraslapo { get; set; }

        public AGenerarIntervalosH(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO )
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._confiWPFEnfierradoDTO = confiEnfierradoDTO;
            this._selecionarPtoSup = selecionarPtoSup;
            this.ListaLevelIntervalo = selecionarPtoSup.ListaLevelIntervalo;
            this._muroSeleccionadoInicialDTO = muroSeleccionadoDTO;
            ListaIntervaloBarrasDTO = new List<IntervaloBarrasDTO>();
            ListaIbarraHorizontal = new List<IbarraBase>();

        }


        public abstract void M1_ObtenerIntervaloBarrasDTO();

        public virtual List<IbarraBase> M2_GenerarListaBarraHorizontal()
        {
            ListaIbarraHorizontal = new List<IbarraBase>();
            try
            {
                foreach (IntervaloBarrasDTO itemIntervaloBarrasDTO in ListaIntervaloBarrasDTO)
                {
                    IbarraBase newIbarraVertical = FactoryBarraVertical.FActoryIGeometriaTagVertical(_uiapp,itemIntervaloBarrasDTO, _confiWPFEnfierradoDTO);
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

        public BuscarMuros BuscarMuroPerpendicularVIew(XYZ ptoBUsqueda)
        {
            //mueve el to 50 cm hacia arriba respecto al pto inicial de la barra
            ptoBUsqueda = ptoBUsqueda + new XYZ(0, 0, Util.CmToFoot(50));
            BuscarMuros BuscarMuros = new BuscarMuros(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
            XYZ ptobusquedaMuro = ptoBUsqueda + 
                                -_muroSeleccionadoInicialDTO.NormalEntradoView * Util.CmToFoot(2) + //mueve pto 2 cm separado de la cara del muero inicial. 2cm hacia la vista 
                                _muroSeleccionadoInicialDTO.DireccionEnFierrado * ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT; //muevo el pto en direccion del muro
            var (wallSeleccionado, espesor, ptoSobreMuro) = BuscarMuros.OBtenerRefrenciaMuro(_confiWPFEnfierradoDTO.view3D_paraBuscar, ptobusquedaMuro, _muroSeleccionadoInicialDTO.NormalEntradoView);
            if (wallSeleccionado == null) return null;
            return BuscarMuros;
        }

    }
}
