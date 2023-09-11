
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

using ArmaduraLosaRevit.Model.BarraV.Intersecciones;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class IntervaloBarrasHorizontalDTO
    {
        private DatosMuroSeleccionadoDTO _muroSeleccionadoDTO;
        private ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO;

        public Element ElementoHost { get; set; }
        public double EspesorElementoHost { get; set; }
        public XYZ normalInversoView { get; set; }
        public XYZ DireccionEnFierrado { get; set; }
        public XYZ ptoini { get; set; }
        public XYZ ptofinal { get; set; }

        public XYZ ptoPosicionTAg { get; set; }

        public TipoPataBarra tipobarraV { get; set; }
        public int diametroMM { get; set; }
        public double espaciamientoFoot { get; set; }
        public double recorridoBrrar { get; set; }
        public List<Curve> listaCurve { get; set; }

        public View3D view3D_paraBuscar { get; set; }

        public View3D view3D_paraVisualizar { get; set; }
        public View viewActual { get; set; }

        public IntervaloBarrasHorizontalDTO()
        {

        }

        public IntervaloBarrasHorizontalDTO(DatosMuroSeleccionadoDTO muroSeleccionadoDTO, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO)
        {

            this._muroSeleccionadoDTO = muroSeleccionadoDTO;
            this._confiEnfierradoDTO = confiEnfierradoDTO;


            this.ElementoHost = muroSeleccionadoDTO.elementoContenedor;
            this.normalInversoView = muroSeleccionadoDTO.NormalEntradoView;
            this.DireccionEnFierrado = muroSeleccionadoDTO.DireccionEnFierrado;

            this.EspesorElementoHost = muroSeleccionadoDTO.EspesorElemetoHost;
            this.viewActual = confiEnfierradoDTO.viewActual;
            this.view3D_paraBuscar = confiEnfierradoDTO.view3D_paraBuscar;
            this.view3D_paraVisualizar = confiEnfierradoDTO.view3D_paraVisualizar;


            this.diametroMM = confiEnfierradoDTO.incial_diametroMM;

            M1_1_AsignarEspaciamientoYrecorrido();

        }


        public void M1_AsignarElementoHost(BuscarViga buscarMuros, Document _doc)
        {
            if (buscarMuros == null) return;
            this.ElementoHost = buscarMuros._vigaElementHost;
            this.EspesorElementoHost = (this.ElementoHost as Wall).ObtenerEspesorMuroFoot(_doc);

            M1_1_AsignarEspaciamientoYrecorrido();

            M1_2_BuscarPtoInicioBase(buscarMuros.CentroCaraNormalSaliendoVista);
        }

        private void M1_1_AsignarEspaciamientoYrecorrido()
        {
            if (_confiEnfierradoDTO.NuevaLineaCantidadbarra == 1)
            {
                throw new NotImplementedException("Solo Una linea de barra.Caso no implementado");
            }
            else
            {
                this.espaciamientoFoot = (this.EspesorElementoHost - Util.CmToFoot((ConstNH.RECUBRIMIENTO_MURO_CM + (diametroMM) / 10)) * 2.0) / (_confiEnfierradoDTO.NuevaLineaCantidadbarra - 1);
                this.recorridoBrrar = (this.EspesorElementoHost - Util.CmToFoot((ConstNH.RECUBRIMIENTO_MURO_CM + (diametroMM) / 10)) * 2.0);
            }
        }

        public bool M1_2_BuscarPtoInicioBase(XYZ ptoSObreCaraMuro)
        {
            try
            {
                Plane plano = Plane.CreateByNormalAndOrigin(-this.normalInversoView, ptoSObreCaraMuro);
                _muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = plano.ProjectOnto(_muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost).Redondear8();

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void M2_AsiganrCoordenadas(double ziniFoot, double zfinFoot, bool IsmoverTraslapo)
        {
            double espaciamientoBordeFoot = _confiEnfierradoDTO.EspaciamientoREspectoBordeFoot + (IsmoverTraslapo ? ConstNH.CONST_DESVIACION_TRASLAPOFOOT : 0);
            this.tipobarraV = TipoPataBarra.BarraVSinPatas;
            this.ptoini = _muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(ziniFoot) +
                                    normalInversoView * Util.CmToFoot(2) + DireccionEnFierrado * espaciamientoBordeFoot;


            this.ptoPosicionTAg = _muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(zfinFoot - ConstNH.CONST_DISTANCIA_BAJATAG_Foot) +
                                    normalInversoView * Util.CmToFoot(2) + DireccionEnFierrado * espaciamientoBordeFoot;

            this.ptofinal = _muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.AsignarZ(zfinFoot) +
                                    normalInversoView * Util.CmToFoot(2) + DireccionEnFierrado * espaciamientoBordeFoot;
        }


        public void BuscarPatasAmbosLados(UIApplication _uipp,   BuscarViga _muroHost50cmSobrePtoinicial)
        {

            //TiposDeBarraPorInterseccion TiposDeBarraPorInterseccion = 
            //    new TiposDeBarraPorInterseccion(_uipp, _confiEnfierradoDTO.view3D_paraBuscar, _muroHost50cmSobrePtoinicial,this, _muroSeleccionadoDTO,8);

            //TiposDeBarraPorInterseccion.BuscarInterseccion();
            //ptoini = TiposDeBarraPorInterseccion._ptoini;
            //ptofinal = TiposDeBarraPorInterseccion._ptofinal;
            //tipobarraV = TiposDeBarraPorInterseccion.ResultTipoBarraV;

        }
    }
}
