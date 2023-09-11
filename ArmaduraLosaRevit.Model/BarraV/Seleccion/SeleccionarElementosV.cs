using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.Niveles.Vigas;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{
    public partial class SeleccionarElementosV
    {
        protected UIApplication _uiapp;
        protected ConfiguracionIniciaWPFlBarraVerticalDTO _confiEnfierradoDTO;
        private List<Level> _listaLevelTotal;

        protected UIDocument _uidoc;
        protected Document _doc;
        protected View _view;
        private bool ConTransaccionAlCrearSketchPlane;

        public XYZ _ptoSeleccionMouseCentroCaraMuro { get; set; }
        public XYZ _ptoSeleccionMouseCaraMuro { get; set; } // variable alternativva de '_ptoSeleccionMouseCentroCaraMuro' pq por error de diseño se utiliza como pto inferir de diseño barras


       //
        protected XYZ _origenSeccionView; //punto mas al derecha, abajo y mas cerca de pantalla a la  vista
        protected XYZ _RightDirection;//direccion paralalea a la pantalla (izq hacia derecha)
        protected XYZ _ViewNormalDirection6;//direccion perpendicular a la pantalla
        private List<double> ListaLevelIntervalo;
        protected View3D _view3D_paraBuscar;

        public XYZ _PtoInicioBaseBordeMuro { get; set; }
        public Wall _WallSelect { get; set; }
        public FamilyInstance _vigaSelect { get; set; }
        public Rebar _RebarSelect { get; set; }
        public Element _ElemetSelect { get; set; }
        public ElementoSeleccionado _elementoSeleccionado { get; set; }

        public XYZ _direccionMuro { get; set; }
        public double _espesorMuroFoot { get; set; }
        public double _largoMuroFoot { get; set; }
        public double LargoRecorridoHorizontalSeleccionCM { get; set; }
        public XYZ _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost { get; set; }
        public XYZ _PtoInicioIntervaloBarra { get; set; }
        public bool IsCoronacion { get; set; }
        public XYZ NormalCaraElemento { get; private set; }

        public SeleccionarElementosV(UIApplication _uiapp, bool ConTransaccionAlCrearSketchPlane = true)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
            this._view = _doc.ActiveView;
            this.ConTransaccionAlCrearSketchPlane = ConTransaccionAlCrearSketchPlane;
            this._direccionMuro = XYZ.Zero;
            this.NormalCaraElemento = XYZ.Zero;
        }
        public SeleccionarElementosV(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO, List<Level> listaLevelTotal)
        {
            this._uiapp = _uiapp;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
            this._listaLevelTotal = listaLevelTotal;

            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
            this._view3D_paraBuscar = confiEnfierradoDTO.view3D_paraBuscar;
            this._view = _doc.ActiveView;
            this.ConTransaccionAlCrearSketchPlane = true;
            this._direccionMuro = XYZ.Zero;
            this.NormalCaraElemento = XYZ.Zero;
        }



     
        public DatosMuroSeleccionadoDTO M5_OBtenerElementoREferenciaDTO(DireccionRecorrido _direccionRecorrido)
        {
            //  ptoRefereMuro - _PtoInicioSobrePLanodelMuro == _seccionView.RightDirection  -- enfieraa hacia derecha
            //  ptoRefereMuro - _PtoInicioSobrePLanodelMuro <> _seccionView.RightDirection  -- enfieraa hacia izquierda

           // XYZ _viewRightDirection = _view.RightDirection.Redondear8();

            XYZ _viewRightDirection = ObtenerDireccionMuroOrientacionView();

            XYZ direccionEnfierrrar_aux = ((_ptoSeleccionMouseCentroCaraMuro.GetXY0() - _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost.GetXY0()).Normalize().IsAlmostEqualTo(_viewRightDirection.GetXY0(), ConstNH.CONST_FACT_TOLERANCIA_3Deci) ?
                                             _viewRightDirection :
                                             new XYZ(-_viewRightDirection.X, -_viewRightDirection.Y, _viewRightDirection.Z));

            Orientacion orientacion = ((_ptoSeleccionMouseCentroCaraMuro.GetXY0() - _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost.GetXY0()).Normalize().IsAlmostEqualTo(_viewRightDirection.GetXY0(), ConstNH.CONST_FACT_TOLERANCIA_3Deci) ?
                                             Orientacion.izquierda : Orientacion.derecha);



            DatosMuroSeleccionadoDTO muroSeleccionadoDTO = new DatosMuroSeleccionadoDTO()
            {
                PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost,
                ptoSeleccionMouseCentroCaraMuro6 = _ptoSeleccionMouseCentroCaraMuro,
                Pier = "",
                Story = "",
                orientacion = orientacion,
                DireccionMuro = _direccionMuro,
                DireccionEnFierrado = direccionEnfierrrar_aux,
                NormalEntradoView = ObtenerDireccionNormalEntrando() , //- _view.ViewDirection.Redondear8().Normalize(),
                DireccionRecorridoBarra = _direccionRecorrido.ObtenerDireccionRecorridoBarra6(ObtenerDireccionNormalEntrando()),
                DireccionPataEnFierrado = _direccionRecorrido.ObtenerDireccionPataEnFierrado(direccionEnfierrrar_aux),
                DireccionLineaBarra = _direccionRecorrido.ObtenerDireccionLineaBarra(direccionEnfierrrar_aux),// - _seccionView.ViewDirection.Normalize(),
                EspesorElemetoHost = (_direccionRecorrido.IsLargoRecorridoCm ? Util.CmToFoot(_direccionRecorrido.LargoRecorridoCm) : _espesorMuroFoot),
                IsLargoRecorrido = _direccionRecorrido.IsLargoRecorridoCm,
                LargoElemetoHost = _largoMuroFoot,
                IdelementoContenedor = _ElemetSelect.Id,
                elementoContenedor = _ElemetSelect,
                LargoEspesorMalla_SinRecub_foot = _espesorMuroFoot - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_MALLA_CM) * 2,
                _EspesorMuroFoot = _espesorMuroFoot,
                IsCoronacion = IsCoronacion,
                TipoElementoSeleccionado = _elementoSeleccionado


            };

            return muroSeleccionadoDTO;
        }


        //caso poco comun para muros grupos en direccion perpendiular view
        public DatosMuroSeleccionadoDTO M5_OBtenerElementoREferenciaDTO_MuroPerpendicualeView(DireccionRecorrido _direccionRecorrido)
        {
            //  ptoRefereMuro - _PtoInicioSobrePLanodelMuro == _seccionView.RightDirection  -- enfieraa hacia derecha
            //  ptoRefereMuro - _PtoInicioSobrePLanodelMuro <> _seccionView.RightDirection  -- enfieraa hacia izquierda

            // XYZ _viewRightDirection = _view.RightDirection.Redondear8();

            XYZ _viewRightDirection = ObtenerDireccionMuroOrientacionView();

            XYZ direccionEnfierrrar_aux = ((_ptoSeleccionMouseCentroCaraMuro.GetXY0() - _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost.GetXY0()).Normalize().IsAlmostEqualTo(_viewRightDirection.GetXY0(), ConstNH.CONST_FACT_TOLERANCIA_3Deci) ?
                                             _viewRightDirection :
                                             new XYZ(-_viewRightDirection.X, -_viewRightDirection.Y, _viewRightDirection.Z));

            Orientacion orientacion = ((_ptoSeleccionMouseCentroCaraMuro.GetXY0() - _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost.GetXY0()).Normalize().IsAlmostEqualTo(_viewRightDirection.GetXY0(), ConstNH.CONST_FACT_TOLERANCIA_3Deci) ?
                                             Orientacion.izquierda : Orientacion.derecha);



            DatosMuroSeleccionadoDTO muroSeleccionadoDTO = new DatosMuroSeleccionadoDTO()
            {
                PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = _PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost,
                ptoSeleccionMouseCentroCaraMuro6 = _ptoSeleccionMouseCentroCaraMuro,
                Pier = "",
                Story = "",
                orientacion = orientacion,
                DireccionMuro = _direccionMuro,
                DireccionEnFierrado = direccionEnfierrrar_aux,
                NormalEntradoView = ObtenerDireccionNormalEntrando(), //- _view.ViewDirection.Redondear8().Normalize(),
                DireccionRecorridoBarra = _direccionRecorrido.ObtenerDireccionRecorridoBarra6(_viewRightDirection),
                DireccionPataEnFierrado = _direccionRecorrido.ObtenerDireccionPataEnFierrado(direccionEnfierrrar_aux),
                DireccionLineaBarra = _direccionRecorrido.ObtenerDireccionLineaBarra(direccionEnfierrrar_aux),// - _seccionView.ViewDirection.Normalize(),
                EspesorElemetoHost = (_direccionRecorrido.IsLargoRecorridoCm ? Util.CmToFoot(_direccionRecorrido.LargoRecorridoCm) : _espesorMuroFoot),
                IsLargoRecorrido = _direccionRecorrido.IsLargoRecorridoCm,
                LargoElemetoHost = _largoMuroFoot,
                IdelementoContenedor = _ElemetSelect.Id,
                elementoContenedor = _ElemetSelect,
                LargoEspesorMalla_SinRecub_foot = _espesorMuroFoot - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_MALLA_CM) * 2,
                _EspesorMuroFoot = _espesorMuroFoot,
                IsCoronacion = IsCoronacion,
                TipoElementoSeleccionado = _elementoSeleccionado


            };

            return muroSeleccionadoDTO;
        }



        private XYZ ObtenerDireccionMuroOrientacionView()
        {
            if(_direccionMuro.IsAlmostEqualTo(XYZ.Zero)) return -_view.RightDirection.Redondear8().Normalize();

            if (Util.GetProductoEscalar(_direccionMuro, _view.RightDirection) > 0)
                return _direccionMuro;
            else
                return new XYZ(-_direccionMuro.X, -_direccionMuro.Y, _direccionMuro.Z);
        }
        private XYZ ObtenerDireccionNormalEntrando()
        {
            if (NormalCaraElemento.IsAlmostEqualTo(XYZ.Zero)) return - _view.ViewDirection.Redondear8().Normalize();

            if (Util.GetProductoEscalar(NormalCaraElemento, -_view.ViewDirection6()) > 0)
                return NormalCaraElemento;
            else
                return new XYZ(-NormalCaraElemento.X, -NormalCaraElemento.Y, _direccionMuro.Z);
        }
        public List<double> ObtenerIntervalLevel(List<Level> _listaLevel, double zInferior, double zSuperior)
        {
            ListaLevelIntervalo = new List<double>();
            try
            {
                ListaLevelIntervalo = _listaLevel.Where(ll => ll.ProjectElevation > zInferior - Util.CmToFoot(50) &&
                                                                ll.ProjectElevation < zSuperior + Util.CmToFoot(50)).Select(rr => rr.ProjectElevation).OrderBy(nn => nn).ToList();


                if (ListaLevelIntervalo.Count == 1)
                {
                    ListaLevelIntervalo = NivelVigaInvertida.CasoVigaInvertido(ListaLevelIntervalo, zInferior);

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ListaLevelIntervalo.Clear();
            }
            return ListaLevelIntervalo;
        }

    }
}
