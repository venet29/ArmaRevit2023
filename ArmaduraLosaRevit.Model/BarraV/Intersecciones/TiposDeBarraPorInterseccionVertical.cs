using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Intersecciones
{
    public class TiposDeBarraPorInterseccionVertical
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private readonly View3D _view3D;
        private readonly BuscarMurosDTO _muroHost50CmSobrePtoinicialDTO;
        private readonly IntervaloBarrasDTO _intervaloBarrasDTO;
        private readonly DatosMuroSeleccionadoDTO _muroSeleccionadoDTO;
        private XYZ _retrocesoPorRecubrimineto;

        public XYZ _ptoini { get; set; }
        public XYZ _pto1NivelAntesfinal { get; set; }

        private readonly XYZ _pto1NivelSobreInicial;
        public XYZ _ptofinal { get; set; }

        public XYZ _ptobuscarWall_inferior { get; set; }
        public XYZ _ptobuscarWall_superior { get; set; }
        //  private int diametroMM;

        public TipoPataBarra ResulttTpobarraVSuperior;
        public TipoPataBarra ResultTipobarraVInferior;

        private BuscarElementosArriba _buscarElementosArriba;
        private BuscarMurosoViga _muroHost50cmBajoPtoFinal;
        private BuscarElementosBajo _buscarElementosBajo;
        private double _desviacionHaciaCentro;
        public TipoPataBarra ResultTipoBarraV { get; internal set; }

        public TiposDeBarraPorInterseccionVertical(UIApplication _uiapp, View3D _view3D,
                 BuscarMurosDTO _muroHost50cmSobrePtoinicialDTO, IntervaloBarrasDTO intervaloBarrasDTO, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view3D = _view3D;
            this._muroHost50CmSobrePtoinicialDTO = _muroHost50cmSobrePtoinicialDTO;
            this._intervaloBarrasDTO = intervaloBarrasDTO;
            this._muroSeleccionadoDTO = muroSeleccionadoDTO;

            //variable para buscar hacia el arribal desde el centro del muro host
            this._desviacionHaciaCentro = (_muroSeleccionadoDTO._EspesorMuroFoot - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) * 2) / 2.0f;
            //this.diametroMM = diametroMM;

            this._ptobuscarWall_inferior = _muroHost50cmSobrePtoinicialDTO._ptobuscarWall_inferior;
            this._ptobuscarWall_superior = _muroHost50cmSobrePtoinicialDTO._ptobuscarWall_superior;

            this._ptoini = intervaloBarrasDTO.ptoini;
            this._ptofinal = intervaloBarrasDTO.ptofinal;
            this._pto1NivelAntesfinal = _ptofinal + new XYZ(0, 0, -5);
            this._pto1NivelSobreInicial = _ptoini + new XYZ(0, 0, 5);
            this._retrocesoPorRecubrimineto = XYZ.Zero;
        }

        public void BuscarInterseccion()
        {
            ResulttTpobarraVSuperior = TipoPataBarra.BarraVSinPatas;
            ResultTipobarraVInferior = TipoPataBarra.BarraVSinPatas;
            ResultTipoBarraV = TipoPataBarra.BarraVSinPatas;

            RetrocederReCubrimientoCasoMAllaVertical();

            M1_BuscarIntersecionHaciaArriba();
            M2_BuscarIntersecionHaciaBajo();
            M3_ObternerResultTipoBarraV();
        }

        // retrocedes el recubrimineto y espesor de muro para  no entonctrar muro superior si
        private bool RetrocederReCubrimientoCasoMAllaVertical()
        {
            try
            {
                if (_intervaloBarrasDTO.TipoBarraVertical_ == TipoBarraVertical.MallaV)
                    _retrocesoPorRecubrimineto = -_muroSeleccionadoDTO.DireccionPataEnFierrado * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_MALLA_CM);
                else if (_intervaloBarrasDTO.TipoBarraVertical_ == TipoBarraVertical.Cabeza)
                    _retrocesoPorRecubrimineto = -_muroSeleccionadoDTO.NormalEntradoView * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_MALLA_CM);


                // hace un retroceso en el sentido de la vista, para buscar cambios de espesor pequeño entre muro. Esto pq aunque se encuentre muro de diferente espsor
                //puede que no se detecte la discontinuidad si no se resta el recubrimineto 


            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        #region 1) Buscar Hacia Arriba
        private void M1_BuscarIntersecionHaciaArriba()
        {
            ObtenerMuroCOntenedor_BuscandoCentroPier();

            //si por algun error no hay muro en el nivel actual
            if (_muroHost50cmBajoPtoFinal == null) ObtenerMuroCOntenedor_BuscandoDesdeBarraHAstaCentro();
            if (_muroHost50cmBajoPtoFinal == null) return;

            // se agrega para por configuracion
            if (_intervaloBarrasDTO.tipobarraV == TipoPataBarra.BarraVPataFinal || _intervaloBarrasDTO.tipobarraV == TipoPataBarra.BarraVPataAmbos)
            {
                _ptofinal = _ptofinal + ObtenerREcubrimiento_MitadEspesor(TipoElementoBArraV.none);
                ResulttTpobarraVSuperior = TipoPataBarra.BarraVPataFinal;
                return;
            }
            else if (_intervaloBarrasDTO.tipobarraV == TipoPataBarra.BarraVSinPatas)
            {
                // ResulttTpobarraVSuperior = TipoPataBarra.BarraVSinPatas;
                M2_0_Asignado_ptoFinalMasLArgoDesarrollo();
                return;
            }
            else if (_intervaloBarrasDTO.tipobarraV == TipoPataBarra.BarraVPataInicial)
            {
                M2_0_Asignado_ptoFinalMasLArgoDesarrollo();
                return;
            }


            double DistanciaBajaDesdeNivelSup = Math.Min(_ptofinal.DistanceTo(_pto1NivelAntesfinal) / 2, 5);
            double LargoBusuqeda = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM)
                                 + Math.Min(_ptofinal.DistanceTo(_pto1NivelAntesfinal) / 2, 5)
                                 + (_intervaloBarrasDTO.IsBuscarCororonacion ? Util.CmToFoot(150) : 0);

            _buscarElementosArriba = new BuscarElementosArriba(_uiapp,
                                                                LargoBusuqeda,
                                                                _view3D,
                                                                _muroSeleccionadoDTO,
                                                                ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT,
                                                                _muroHost50CmSobrePtoinicialDTO);
            _buscarElementosArriba.BuscarObjetosEntrayectoriaDeBArra(this._ptofinal - new XYZ(0, 0, DistanciaBajaDesdeNivelSup) + _retrocesoPorRecubrimineto,
                                                                    new XYZ(0, 0, 1),
                                                                    _intervaloBarrasDTO.DireccionRecorridoBarra * _desviacionHaciaCentro);

            if (NoProlongarLosaHaciaArriba()) return;

            if (_buscarElementosArriba.ListaObjEncontrados.Count == 0 || _intervaloBarrasDTO.tipobarraV == TipoPataBarra.NoBuscar)
            {
                M2_0_Asignado_ptoFinalMasLArgoDesarrollo();
                return;
            }
            else
            {
                var listaMaslejanos = BuscarMasLejanos_ListaElementoVERTICAL(_buscarElementosArriba.ListaObjEncontrados);

                if (listaMaslejanos.Count == 1)
                {
                    //if (listaMaslejanos[0].ptoInterseccion.DistanceTo(listaMaslejanos[0].PtoSObreCaraSuperior) > Util.CmToFoot(150))
                    if (Math.Abs(listaMaslejanos[0].ptoInterseccion.Z - listaMaslejanos[0].PtoSObreCara.Z) > Util.CmToFoot(150))
                        M2_0_Asignado_ptoFinalMasLArgoDesarrollo();
                    else
                    {
                        ObtnenerPtoFInalConpata(listaMaslejanos);
                        //double valorzPoyeccionPtoEnLosa1 = listaMaslejanos.Last().PtoSObreCaraSuperior.Z;
                        //_ptofinal = _ptofinal.AsignarZ(valorzPoyeccionPtoEnLosa1) + ObtenerREcubrimiento_MitadEspesor(listaMaslejanos.Last().nombreTipo);
                        //ResulttTpobarraVSuperior = TipoPataBarra.BarraVPataFinal;
                    }
                }
                else if (listaMaslejanos.Count > 1)
                {
                    var PlanosOpuesto_vectoBusqueda = listaMaslejanos.Where(c => Util.IsParallel_DistintoSentido(c.NormalFace, new XYZ(0, 0, 1), 0.9)).FirstOrDefault();

                    //si encuentra  lleva para  (pq selecciono cara mas externa)
                    if (PlanosOpuesto_vectoBusqueda != null)
                    {
                        if (PlanosOpuesto_vectoBusqueda.nombreTipo == TipoElementoBArraV.muro &&
                            PlanosOpuesto_vectoBusqueda.EspesorMuro < _muroHost50CmSobrePtoinicialDTO._espesorMuro)// _muroSeleccionadoDTO._EspesorMuroFoot)// buscar si muro superior tiene espesor menor
                        {

                            ObtnenerPtoFInalSINpata(listaMaslejanos);
                            return;
                        }


                        M2_0_Asignado_ptoFinalMasLArgoDesarrollo();
                    }
                    else //sino genera  prolongacion pq estoy dentro de un objeto
                        ObtnenerPtoFInalConpata(listaMaslejanos);
                }
            }
        }

        private void ObtnenerPtoFInalConpata(List<ObjetosEncontradosDTO> listaMaslejanos)
        {
            double valorzPoyeccionPtoEnLosa1 = listaMaslejanos.Last().PtoSObreCara.Z;
            _ptofinal = _ptofinal.AsignarZ(valorzPoyeccionPtoEnLosa1) + ObtenerREcubrimiento_MitadEspesor(listaMaslejanos.Last().nombreTipo);
            ResulttTpobarraVSuperior = TipoPataBarra.BarraVPataFinal;
        }
        private void ObtnenerPtoFInalSINpata(List<ObjetosEncontradosDTO> listaMaslejanos)
        {
            double valorzPoyeccionPtoEnLosa1 = listaMaslejanos.Last().PtoSObreCara.Z;
            _ptofinal = _ptofinal.AsignarZ(valorzPoyeccionPtoEnLosa1) + ObtenerREcubrimiento_MitadEspesor(listaMaslejanos.Last().nombreTipo);
            ResulttTpobarraVSuperior = TipoPataBarra.BarraVSinPatas;
        }

        private void ObtenerMuroCOntenedor_BuscandoCentroPier()
        {
            if (_ptobuscarWall_inferior == null) return;

            double[] rango = new double[] { 0.8, 0.9, 0.7, 0.5 };

            foreach (var valor in rango)
            {
                double z2tercio = Math.Min(_ptobuscarWall_inferior.Z, _ptobuscarWall_superior.Z) + Math.Abs(_ptobuscarWall_inferior.Z - _ptobuscarWall_superior.Z) * valor;
                //busca muro 50cm bajo el pto final del barra       
                XYZ ptoBUsqueda = ((_ptobuscarWall_inferior + _ptobuscarWall_superior) / 2).AsignarZ(z2tercio);//+ new XYZ(0, 0, -Util.CmToFoot(50));

#if DEBUG
                // CrearModeLineAyuda.modelarlinea_sinTrans(_doc, ptoBUsqueda, ptoBUsqueda + _muroSeleccionadoDTO.NormalEntradoView * 2);
#endif
                _muroHost50cmBajoPtoFinal = M1_0_BuscarMuroPerpendicularVIew(ptoBUsqueda);
                if (_muroHost50cmBajoPtoFinal != null)
                    return;
            }
        }

        private void ObtenerMuroCOntenedor_BuscandoDesdeBarraHAstaCentro()
        {
            if (_ptobuscarWall_inferior == null) return;
            XYZ vectorDesplamieto = _intervaloBarrasDTO.DireccionPataEnFierrado;
            double[] rango = new double[] { 0.8, 0.9, 0.7, 0.5 };
            double desplaza10cm = Util.CmToFoot(20);

            foreach (var valor in rango)
            {
                for (int i = 1; i < 10; i++)
                {
                    double z2tercio = Math.Min(_ptobuscarWall_inferior.Z, _ptobuscarWall_superior.Z) + Math.Abs(_ptobuscarWall_inferior.Z - _ptobuscarWall_superior.Z) * valor;
                    //busca muro 50cm bajo el pto final del barra       
                    XYZ ptoBUsqueda = _ptofinal.AsignarZ(z2tercio) + vectorDesplamieto * desplaza10cm * i;//+ new XYZ(0, 0, -Util.CmToFoot(50));
#if DEBUG
                    // CrearModeLineAyuda.modelarlinea_sinTrans(_doc, ptoBUsqueda, ptoBUsqueda + _muroSeleccionadoDTO.NormalEntradoView * 2);
#endif

                    _muroHost50cmBajoPtoFinal = M1_0_BuscarMuroPerpendicularVIew(ptoBUsqueda);
                    if (_muroHost50cmBajoPtoFinal != null)
                        return;
                }
            }
        }

        private bool NoProlongarLosaHaciaArriba()
        {

            if (!_intervaloBarrasDTO.IsNoProloganLosaArriba) return false;

            _ptofinal = _ptofinal + ObtenerREcubrimiento_MitadEspesor(TipoElementoBArraV.none);
            if (ConstNH.TipoTerminacionCambioMuro == TipoTerminacionCambioMuro.SinPAta)
            {
                ResulttTpobarraVSuperior = TipoPataBarra.BarraVSinPatas;
            }
            else if (_buscarElementosArriba.ListaObjEncontrados.Count(c => (c.nombreTipo == TipoElementoBArraV.muro || c.nombreTipo == TipoElementoBArraV.muroPerpeView)) > 0)
                ResulttTpobarraVSuperior = TipoPataBarra.BarraVPataFinal;
            else
                ResulttTpobarraVSuperior = TipoPataBarra.BarraVSinPatas;

            return true;
        }

        private BuscarMurosoViga M1_0_BuscarMuroPerpendicularVIew(XYZ ptoBUsqueda)
        {
            BuscarMurosoViga BuscarMuros = new BuscarMurosoViga(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
            XYZ ptobusquedaMuro = ptoBUsqueda +
                                -_muroSeleccionadoDTO.NormalEntradoView * Util.CmToFoot(10);// + //mueve pto 2 cm separado de la cara del muero inicial. 2cm hacia la vista 
                                                                                            // _muroSeleccionadoDTO.DireccionEnFierrado * ConstantesGenerales.CONST_DISTANCIA_BUSQUEDA_MUROFOOT; //muevo el pto en direccion del muro

            Element wallSeleccionado = BuscarMuros.OBtenerRefrenciaMuro(_view3D, ptobusquedaMuro, _muroSeleccionadoDTO.NormalEntradoView);
            //NOTA : CONST_DISTANCIA_BUSQUEDA_MUROFOOT se debe incrementar si el mento encontrdo es un pilar o un muro en la direccon perpendicular de la vista
            if (wallSeleccionado == null) return null;
            return BuscarMuros;
        }

        private XYZ ObtenerREcubrimiento_MitadEspesor(TipoElementoBArraV _TipoElementoBArraV)
        {
            double recubrimiento = (_TipoElementoBArraV == TipoElementoBArraV.fundacion ?
                                            ConstNH.RECUBRIMIENTO_FUNDACIONES_foot :
                                            ConstNH.RECUBRIMIENTO_PATA_BARRAV_Foot);
            return new XYZ(0, 0, -recubrimiento - Util.MmToFoot(_intervaloBarrasDTO.diametroMM) / 2.0f);
        }

        #endregion

        #region 2) buscar hacia abajo

        private void M2_BuscarIntersecionHaciaBajo()
        {
            // se busca pata
            double DistanciaRetrocesoPtoSup = Util.CmToFoot(1);
            XYZ direccionBArras = new XYZ(0, 0, -1);
            double AuxBuscarFundaciones = 0;
            if (_intervaloBarrasDTO.IsbarraIncial)
            {
                AuxBuscarFundaciones = Util.CmToFoot(100);
            }

            if (_intervaloBarrasDTO.tipobarraV == TipoPataBarra.BarraVPataInicial ||
                _intervaloBarrasDTO.tipobarraV == TipoPataBarra.BarraVPataAmbos)
            {
                ResultTipobarraVInferior = TipoPataBarra.BarraVPataInicial;
                _ptoini = _ptoini;
                return;
            }
            else if (_intervaloBarrasDTO.tipobarraV == TipoPataBarra.BarraVSinPatas)
            {
                //ResultTipobarraVInferior = TipoPataBarra.BarraVSinPatas;
                M2_0_Asignado_ptoInialMenosLArgoDesarrollo();
                return;
            }
            else if (_intervaloBarrasDTO.tipobarraV == TipoPataBarra.BarraVPataFinal)
            {
                M2_0_Asignado_ptoInialMenosLArgoDesarrollo();
                return;
            }

            _buscarElementosBajo = new BuscarElementosBajo(_uiapp,
                                                       UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) + AuxBuscarFundaciones,
                                                       _view3D,
                                                       _ptobuscarWall_inferior);

            //solucion parche para para solod ejar los elemtos a una distancia maxima de  'UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM)'  y las fundaciones que encuentre
            _buscarElementosBajo.BuscarObjetosInferiorBArra(this._ptoini - direccionBArras * DistanciaRetrocesoPtoSup + _retrocesoPorRecubrimineto, direccionBArras);
            _buscarElementosBajo.listaObjEncontrados = _buscarElementosBajo.listaObjEncontrados
                                                        .Where(c => (c.distancia < UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM) && c.nombreTipo != TipoElementoBArraV.fundacion) ||
                                                                     c.nombreTipo == TipoElementoBArraV.fundacion)
                                                        .ToList();

            if (M2_7_BuscarSiTerminaEnFundacion()) return;

            if (M2_6_BuscarSIesMuroNoAtachadoENLosa_casoABjo()) return;

            if (M2_4_BuscarSiTerminaMuroConLargoProlongacion()) return;

            if (M2_5_IsBArraInicial()) return;

            if (_buscarElementosBajo.listaObjEncontrados.Count == 0 || _intervaloBarrasDTO.tipobarraV == TipoPataBarra.NoBuscar)
            {
                M2_0_Asignado_ptoInialMenosLArgoDesarrollo();
                return;
            }
            else
            {
                if (_buscarElementosBajo.listaObjEncontrados.Count == 0) return;
                var listaMaslejanos = BuscarMasLejanos_ListaElementoVERTICAL(_buscarElementosBajo.listaObjEncontrados);

                if (listaMaslejanos.Count == 1)
                {
                    //if (listaMaslejanos[0].ptoInterseccion.DistanceTo(listaMaslejanos[0].ptoSObreCaraInferiorLosa) > Util.CmToFoot(150))
                    if (Math.Abs(listaMaslejanos[0].ptoInterseccion.Z - listaMaslejanos[0].ptoSObreCaraInferiorLosa.Z) > Util.CmToFoot(150))
                        M2_0_Asignado_ptoFinalMasLArgoDesarrollo();
                    else
                    {
                        double valorzPoyeccionPtoEnLosa = listaMaslejanos.Last().ptoSObreCaraInferiorLosa.Z;
                        _ptoini = _ptoini.AsignarZ(valorzPoyeccionPtoEnLosa) - ObtenerREcubrimiento_MitadEspesor(listaMaslejanos.Last().nombreTipo);
                        ResultTipobarraVInferior = TipoPataBarra.BarraVPataInicial;
                    }
                }
                else if (listaMaslejanos.Count > 1)
                {

                    var PlanosOpuesto_vectoBusqueda = listaMaslejanos.Where(c => Util.IsParallel_DistintoSentido(c.NormalFace, new XYZ(0, 0, -1), 0.9)).FirstOrDefault();

                    //si encuentra  lleva para  (pq selecciono cara mas externa)
                    if (PlanosOpuesto_vectoBusqueda != null)
                    {
                        if (PlanosOpuesto_vectoBusqueda.nombreTipo == TipoElementoBArraV.muro)// buscar si muro superior tiene espesor menor
                        {
                            if (PlanosOpuesto_vectoBusqueda.EspesorMuro > _muroHost50CmSobrePtoinicialDTO._espesorMuro)// _muroSeleccionadoDTO._EspesorMuroFoot)
                                M2_0_Asignado_ptoFinalMasLArgoDesarrollo();
                        }
                    }
                }

            }

            ResultTipobarraVInferior = TipoPataBarra.BarraVSinPatas;
            _ptoini = _ptoini;

        }

        private bool M2_7_BuscarSiTerminaEnFundacion()
        {
            if (_buscarElementosBajo.listaObjEncontrados.Count == 0) return false;
            var ElementoSeleccionado = _buscarElementosBajo.listaObjEncontrados.Where(c => c.nombreTipo == TipoElementoBArraV.fundacion).OrderByDescending(c => c.distancia).FirstOrDefault();

            if (ElementoSeleccionado == null) return false;

            double valorzPoyeccionPtoEnLosa = ElementoSeleccionado.ptoSObreCaraInferiorLosa.Z;
            _ptoini = _ptoini.AsignarZ(valorzPoyeccionPtoEnLosa) - ObtenerREcubrimiento_MitadEspesor(ElementoSeleccionado.nombreTipo);
            ResultTipobarraVInferior = TipoPataBarra.BarraVPataInicial;

            return true;
        }

        private bool M2_4_BuscarSiTerminaMuroConLargoProlongacion()
        {

            if (_intervaloBarrasDTO.IsProloganLosaBajo)
            {
                M2_0_Asignado_ptoInialMenosLArgoDesarrollo();

                return true;
            }
            else
            {
                if (_buscarElementosBajo.listaObjEncontrados.Count == 0) return false;

                var listaElementosMaslejanos = BuscarMasLejanos_ListaElementoVERTICAL(_buscarElementosBajo.listaObjEncontrados);

                var contienMuro = listaElementosMaslejanos.Where(c => c.nombreTipo == TipoElementoBArraV.muro).FirstOrDefault();

                if (contienMuro != null)
                {
                    if (contienMuro.EspesorMuro > _muroHost50CmSobrePtoinicialDTO._espesorMuro)//_muroSeleccionadoDTO._EspesorMuroFoot)
                    {
                        M2_0_Asignado_ptoInialMenosLArgoDesarrollo();
                        return true;
                    }
                }
            }
            return false;
        }
        private bool M2_5_IsBArraInicial()
        {
            if (_intervaloBarrasDTO.IsbarraIncial)
            {
                if (_buscarElementosBajo.listaObjEncontrados.Count == 0) return false;

                var listaElementosMaslejanos = BuscarMasLejanos_ListaElementoVERTICAL(_buscarElementosBajo.listaObjEncontrados);

                if (listaElementosMaslejanos.Count == 1)
                {
                    double valorzPoyeccionPtoEnLosa = listaElementosMaslejanos.Last().ptoSObreCaraInferiorLosa.Z;
                    _ptoini = _ptoini.AsignarZ(valorzPoyeccionPtoEnLosa) - ObtenerREcubrimiento_MitadEspesor(listaElementosMaslejanos.Last().nombreTipo);
                    ResultTipobarraVInferior = TipoPataBarra.BarraVPataInicial;
                }
                else if (listaElementosMaslejanos.Count > 1)
                {

                    if (listaElementosMaslejanos.Exists(c => c.nombreTipo == TipoElementoBArraV.fundacion))
                    {

                        var ElementoSeleccionado = listaElementosMaslejanos.Find(c => c.nombreTipo == TipoElementoBArraV.fundacion);

                        double valorzPoyeccionPtoEnLosa = ElementoSeleccionado.ptoSObreCaraInferiorLosa.Z;
                        _ptoini = _ptoini.AsignarZ(valorzPoyeccionPtoEnLosa) - ObtenerREcubrimiento_MitadEspesor(ElementoSeleccionado.nombreTipo);
                        ResultTipobarraVInferior = TipoPataBarra.BarraVPataInicial;
                    }
                    else if (listaElementosMaslejanos.Exists(c => c.nombreTipo == TipoElementoBArraV.muro))
                    {
                        return true;
                    }
                    else if (listaElementosMaslejanos.Exists(c => c.NormalFace.Z < 0))
                    {

                        var ElementoSeleccionado = listaElementosMaslejanos.Last();

                        double valorzPoyeccionPtoEnLosa = ElementoSeleccionado.ptoSObreCaraInferiorLosa.Z;
                        _ptoini = _ptoini.AsignarZ(valorzPoyeccionPtoEnLosa) - ObtenerREcubrimiento_MitadEspesor(ElementoSeleccionado.nombreTipo);
                        ResultTipobarraVInferior = TipoPataBarra.BarraVPataInicial;
                    }
                    else
                        M2_0_Asignado_ptoInialMenosLArgoDesarrollo();
                }

                return true;
            }
            return false;
        }

        // METODOS PAR NO CREAR PARA EN EL CASO DE MUROS NO ATACHDOS A LOSA
        private bool M2_6_BuscarSIesMuroNoAtachadoENLosa_casoABjo()
        {
            if (_buscarElementosBajo.listaObjEncontrados.Count == 0) return false;

            var listaElementosMaslejanos = BuscarMasLejanos_ListaElementoVERTICAL(_buscarElementosBajo.listaObjEncontrados);

            if (listaElementosMaslejanos.Count == 1 && listaElementosMaslejanos[0].nombreTipo == TipoElementoBArraV.losa)
            {
                var elemetoLosa = listaElementosMaslejanos[0];
                double espesor = elemetoLosa.elemt.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM).AsDouble();
                var losaMaxDistancia = _buscarElementosBajo.listaObjEncontrados
                                                           .Where(c => Util.IsSimilarValor(Math.Abs(c.distancia - elemetoLosa.distancia), espesor, 0.001) &&
                                                                       c.NormalFace.Z > 0 &&
                                                                       c.nombreTipo != TipoElementoBArraV.losa)
                                                           .FirstOrDefault();
                if (losaMaxDistancia != null)
                {
                    if (losaMaxDistancia.EspesorMuro > _muroHost50CmSobrePtoinicialDTO._espesorMuro)//busca si muro tienes mayor espesor conrespecto al 
                    {
                        M2_0_Asignado_ptoInialMenosLArgoDesarrollo();

                    }
                    return true;
                }
            }
            return false;
        }

        private void M2_0_Asignado_ptoFinalMasLArgoDesarrollo()
        {
            //_ptoini = _ptoini + new XYZ(0, 0, ConstantesGenerales.RECUBRIMIENTO_PATA_BARRAV_);
            _ptofinal = _ptofinal + new XYZ(0, 0, +UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM));
            ResulttTpobarraVSuperior = TipoPataBarra.BarraVSinPatas;
        }

        private void M2_0_Asignado_ptoInialMenosLArgoDesarrollo()
        {
            //_ptoini = _ptoini + new XYZ(0, 0, ConstantesGenerales.RECUBRIMIENTO_PATA_BARRAV_);
            _ptoini = _ptoini + new XYZ(0, 0, -UtilBarras.largo_L9_DesarrolloFoot_diamMM(_intervaloBarrasDTO.diametroMM));
            ResultTipobarraVInferior = TipoPataBarra.BarraVSinPatas;
        }

        // buscar elementos mas distantes
        private List<ObjetosEncontradosDTO> BuscarMasLejanos_ListaElementoVERTICAL(List<ObjetosEncontradosDTO> _buscarElementos)
        {
            if (_buscarElementos.Count == 0) return new List<ObjetosEncontradosDTO>();
            var losaMaxDistancia = _buscarElementos.Max(c => c.distancia);

            var ListaMAslejanos = _buscarElementos.Where(c => Util.IsSimilarValor(c.distancia, losaMaxDistancia, ConstNH.TOLERANCIACERO_3mm)).ToList();

            return ListaMAslejanos;
        }

        #endregion

        private void M3_ObternerResultTipoBarraV()
        {
            if (ResulttTpobarraVSuperior == TipoPataBarra.BarraVPataFinal && ResultTipobarraVInferior == TipoPataBarra.BarraVPataInicial)
            {
                ResultTipoBarraV = TipoPataBarra.BarraVPataAmbos;
            }
            else if (ResulttTpobarraVSuperior == TipoPataBarra.BarraVPataFinal)
            {
                ResultTipoBarraV = TipoPataBarra.BarraVPataFinal;
            }
            else if (ResultTipobarraVInferior == TipoPataBarra.BarraVPataInicial)
            {
                ResultTipoBarraV = TipoPataBarra.BarraVPataInicial;
            }
            else
            { ResultTipoBarraV = TipoPataBarra.BarraVSinPatas; }
        }

    }
}
