using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Extension;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Buscar.Model;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarElementosArriba
    {
        private UIApplication _uiapp;
        private Document _doc;
        private double _largoDeBUsquedaFoot;
        private View _view;

        private View3D _elem3d;
        private readonly DatosMuroSeleccionadoDTO _muroSeleccionadoDTO;
        private readonly XYZ _direccionEnfierrar;
        private readonly double _distanciaAlCentroMuro;
        private readonly BuscarMurosDTO _muroHost50CmSobrePtoinicialDTO;
        private TipoElementoBArraV _nombreTipo;
        private XYZ _PtoSObreCarar;

        private double _espesorMuro;
        private XYZ _OrigenMuro;
        private double _alturaMuro;
        private XYZ _direccionMuro;
        private int _espesorViga;
        private XYZ _OrigenViga;
        private XYZ _direccionViga;
        public List<ObjetosEncontradosDTO> ListaObjEncontrados { get; set; }
        public PlanarFace _planarface { get; set; }
        public XYZ PuntoSobreFAceHost { get; set; }
        public Element ElementEncontrado { get; set; }
        public Element ElementMasAlcentroEncontrado;
        private List<ObjetosEncontrado> listaObjetos;

        public BuscarElementosArriba(UIApplication uIApplication, double _largoDeBUsquedaFoot, View3D view3D, DatosMuroSeleccionadoDTO muroSeleccionadoDTO, double DistanciaAlCentroMuro, BuscarMurosDTO _muroHost50cmSobrePtoinicialDTO)
        {
            this._uiapp = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._view = _doc.ActiveView;
            this.ListaObjEncontrados = new List<ObjetosEncontradosDTO>();
            this._elem3d = view3D;
            this._muroSeleccionadoDTO = muroSeleccionadoDTO;
            this._direccionEnfierrar = muroSeleccionadoDTO.DireccionEnFierrado;
            this._distanciaAlCentroMuro = DistanciaAlCentroMuro;
            this._muroHost50CmSobrePtoinicialDTO = _muroHost50cmSobrePtoinicialDTO;
        }

        // segundo constructor fundaciones
        public BuscarElementosArriba(UIApplication uIApplication, double _largoDeBUsquedaFoot, View3D view3D)
        {
            this._uiapp = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._view = _doc.ActiveView;
            this.ListaObjEncontrados = new List<ObjetosEncontradosDTO>();
            this._elem3d = view3D;

        }

        //obs1)
        public bool BuscarObjetosEntrayectoriaDeBArra(XYZ PtoBusqueda, XYZ VectorBusqueda, XYZ vertorDesplazamietno, bool IsdibujarRayoDebusqueda = false)
        {

            try
            {
                var AuxlistaObjEncontrados = new List<ObjetosEncontradosDTO>();
                if (IsdibujarRayoDebusqueda)
                {
#if (DEBUG)
                    CrearLIneaAux _crearLIneaAux = new CrearLIneaAux(_doc);
                    _crearLIneaAux.CrearLinea(PtoBusqueda, PtoBusqueda + VectorBusqueda * _largoDeBUsquedaFoot);
#endif
                }

                if (!GEnerarListaElementosIntersectado(PtoBusqueda, VectorBusqueda, vertorDesplazamietno)) return false;

                //double proximidadAnterior = -1;
                //bool verificar = false;
                foreach (var ref2 in listaObjetos)
                {
                    if (ref2 != null)
                    {
                        if (ref2.Proximity < _largoDeBUsquedaFoot)
                        {



                            //   Reference WallRef = ref2.GetReference();
                            PuntoSobreFAceHost = ref2.PuntoSobreFAceHost;// WallRef.GlobalPoint - vertorDesplazamietno;
                            ElementEncontrado = ref2.ElementEncontrado;// _doc.GetElement(WallRef);
                            if (ElementEncontrado == null) continue;

                            //si no tiene igual igual proximidad que el anteriro caso, entonces hay discontinuidad  espacio en blanco
                            //if (!Util.IsSimilarValor(proximidadAnterior, ref2.Proximity, 0.001) && verificar) break;

                            M1_ResetParametrosIniciales();

                            _nombreTipo = TipoElementoBArraV.none;

                            if (ElementEncontrado is Wall)
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;

                                if (M2_ObtenerDatosMuro(ElementEncontrado, PuntoSobreFAceHost, ref2.Proximity))
                                { //muro direccion del la vista
                                    ElementEncontrado = ElementMasAlcentroEncontrado;
                                    _nombreTipo = TipoElementoBArraV.muro;
                                }
                                else
                                {//muro perpendicular a la vista
                                    if (ElementMasAlcentroEncontrado == null) continue;
                                    ElementEncontrado = ElementMasAlcentroEncontrado;
                                    _nombreTipo = TipoElementoBArraV.muroPerpeView;
                                }
                            }
                            else if (ElementEncontrado.Category.Name == "Structural Framing")
                            {
                                if (!M3_ObtenerPtoSuperiorViga(PuntoSobreFAceHost, ElementEncontrado)) continue;
                                _nombreTipo = TipoElementoBArraV.viga;
                            }
                            else if (ElementEncontrado.Category.Name == "Structural Columns")
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;

                                if (!M3_ObtenerPtoSuperiorColumna(PuntoSobreFAceHost, ElementEncontrado)) continue;
                                _nombreTipo = TipoElementoBArraV.columna;
                            }
                            else if (ElementEncontrado.Category.Name == "Structural Foundations")
                            {
                                if (ElementEncontrado.Name.ToLower().Contains(ConstNH.CONST_FILTRAR_HORMIGON_POBRE)) continue;

                                if (!M4_ObtenerPtoSuperior(PtoBusqueda, ElementEncontrado)) continue;
                                _nombreTipo = TipoElementoBArraV.fundacion;
                            }
                            else if (ElementEncontrado.Category.Name == "Floors")
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;
                                if (!M4_ObtenerPtoSuperior(PuntoSobreFAceHost, ElementEncontrado)) continue;
                                _nombreTipo = TipoElementoBArraV.losa;
                            }

                            if (_nombreTipo == TipoElementoBArraV.none) continue;


                            ObjetosEncontradosDTO objetosEncontrados = new ObjetosEncontradosDTO()
                            {
                                elemt = ElementEncontrado,
                                elemtid = ElementEncontrado.Id.IntegerValue,
                                nombreTipo = _nombreTipo,
                                ptoInterseccion = PuntoSobreFAceHost,
                                distancia = ref2.Proximity,
                                PtoSObreCara = _PtoSObreCarar,
                                EspesorMuro = _espesorMuro,
                                OrigenMuro = _OrigenMuro,
                                DireccionMuro = _direccionMuro,
                                EspesorViga = _espesorViga,
                                OrigenViga = _OrigenViga,
                                DireccionViga = _direccionViga,
                                PlanarfaceIntersectada = _planarface,
                                NormalFace = _planarface.FaceNormal
                            };

                            AuxlistaObjEncontrados.Add(objetosEncontrados);



                        }
                    }
                }//fin for


                // buscando discontinuridada
                foreach (var item in AuxlistaObjEncontrados.OrderBy(c => c.distancia))
                {
                    if (Util.IsIgualSentido(item.NormalFace, VectorBusqueda)) // 0.5 por los vectore no son completamente pararelos ,                     
                        ListaObjEncontrados.Add(item);
                    else
                    {
                        var list = AuxlistaObjEncontrados.Where(c => Util.IsSimilarValor(c.distancia, item.distancia, 0.01) &&
                                                                     c.elemtid != item.elemtid &&
                                                                    Util.IsIgualSentido(c.NormalFace, VectorBusqueda)).ToList();

                        if (list.Count > 0)
                        { ListaObjEncontrados.Add(item); }
                        else if (ListaObjEncontrados.Count > 0)
                        {
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private bool GEnerarListaElementosIntersectado(XYZ PtoBusqueda, XYZ VectorBusqueda,XYZ vertorDesplazamietno)
        {
            try
            {
                ListaObjEncontrados = new List<ObjetosEncontradosDTO>();
                var AuxlistaObjEncontrados = new List<ObjetosEncontradosDTO>();
                ElementCategoryFilter filterMuro = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
                ElementCategoryFilter filterLosa = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
                ElementCategoryFilter filterColum = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
                ElementCategoryFilter filterViga = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
                ElementCategoryFilter filterFund = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);

                LogicalOrFilter f3 = new LogicalOrFilter(filterMuro, filterLosa);
                LogicalOrFilter f3b = new LogicalOrFilter(f3, filterViga);
                LogicalOrFilter f4 = new LogicalOrFilter(f3b, filterFund);
                LogicalOrFilter f5 = new LogicalOrFilter(f4, filterColum);


                ReferenceIntersector ri = new ReferenceIntersector(f5, FindReferenceTarget.Face, _elem3d);
                listaObjetos = ri.Find(PtoBusqueda, VectorBusqueda)
                                       .Where(cc => cc.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(cc, _doc))
                                       .Select(c =>
                                       new ObjetosEncontrado()
                                       {
                                           Proximity = c.Proximity,
                                           PuntoSobreFAceHost = c.GetReference().GlobalPoint - vertorDesplazamietno,
                                           ElementEncontrado = _doc.GetElement(c.GetReference())
                                       })
                                       .OrderBy(c => c.Proximity).ToList();

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public bool BuscarParaCoronacion(XYZ PtoBusqueda, XYZ VectorBusqueda, bool IsdibujarRayoDebusqueda = false)
        {
            try
            {
                var AuxlistaObjEncontrados = new List<ObjetosEncontradosDTO>();
                if (IsdibujarRayoDebusqueda)
                {
#if (DEBUG)
                    CrearLIneaAux _crearLIneaAux = new CrearLIneaAux(_doc);
                    _crearLIneaAux.CrearLinea(PtoBusqueda, PtoBusqueda + VectorBusqueda * _largoDeBUsquedaFoot);
#endif
                }

                 if (!GEnerarListaElementosIntersectado(PtoBusqueda , VectorBusqueda,XYZ.Zero)) return false;

                foreach (var ref2 in listaObjetos)
                {
                    if (ref2 != null)
                    {
                        if (ref2.Proximity < _largoDeBUsquedaFoot)
                        {



                            //   Reference WallRef = ref2.GetReference();
                            PuntoSobreFAceHost = ref2.PuntoSobreFAceHost;// WallRef.GlobalPoint - vertorDesplazamietno;
                            ElementEncontrado = ref2.ElementEncontrado;// _doc.GetElement(WallRef);
                            if (ElementEncontrado == null) continue;

                            //si no tiene igual igual proximidad que el anteriro caso, entonces hay discontinuidad  espacio en blanco
                            //if (!Util.IsSimilarValor(proximidadAnterior, ref2.Proximity, 0.001) && verificar) break;

                            M1_ResetParametrosIniciales();

                            _nombreTipo = TipoElementoBArraV.none;

                            if (ElementEncontrado is Wall)
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;
                                //ElementEncontrado = ElementMasAlcentroEncontrado;
                                _nombreTipo = TipoElementoBArraV.muro;
                            }
                            //else if (ElementEncontrado.Category.Name == "Structural Framing")
                            //{
                            //    if (!M3_ObtenerPtoSuperiorViga(PuntoSobreFAceHost, ElementEncontrado)) continue;
                            //    _nombreTipo = TipoElementoBArraV.viga;
                            //}
                            //else if (ElementEncontrado.Category.Name == "Structural Columns")
                            //{
                            //    if (!ElementEncontrado.IsEstructural()) continue;

                            //    if (!M3_ObtenerPtoSuperiorColumna(PuntoSobreFAceHost, ElementEncontrado)) continue;
                            //    _nombreTipo = TipoElementoBArraV.columna;
                            //}
                          

                            if (_nombreTipo == TipoElementoBArraV.none) continue;


                            ObjetosEncontradosDTO objetosEncontrados = new ObjetosEncontradosDTO()
                            {
                                elemt = ElementEncontrado,
                                elemtid = ElementEncontrado.Id.IntegerValue,
                                nombreTipo = _nombreTipo,
                                ptoInterseccion = PuntoSobreFAceHost,
                                distancia = ref2.Proximity,
                                //PtoSObreCara = _PtoSObreCarar,
                                //EspesorMuro = _espesorMuro,
                                //OrigenMuro = _OrigenMuro,
                                //DireccionMuro = _direccionMuro,
                                //EspesorViga = _espesorViga,
                                //OrigenViga = _OrigenViga,
                                //DireccionViga = _direccionViga,
                                //PlanarfaceIntersectada = _planarface,
                                //NormalFace = _planarface.FaceNormal
                            };

                            ListaObjEncontrados.Add(objetosEncontrados);



                        }
                    }
                }//fin for

                return (ListaObjEncontrados.Count > 1 ? false : true);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
        }


        private void M1_ResetParametrosIniciales()
        {
            _nombreTipo = TipoElementoBArraV.none;
            _PtoSObreCarar = XYZ.Zero;
            _espesorMuro = 0;
            _planarface = null;
            _OrigenMuro = XYZ.Zero;
            _direccionMuro = XYZ.Zero;
            _espesorViga = 0;
            _OrigenViga = XYZ.Zero;
            _direccionViga = XYZ.Zero;
        }
        private bool M2_ObtenerDatosMuro(Element ElementEncontrado, XYZ PtoBusqueda, double proximidad)
        {

            if (M2_1_BuscarSiMuroEncontradoIguaDireccionMuroReferencia(ElementEncontrado, PtoBusqueda)) return true;
            //
            return M2_2_BuscarMuroMasAlCentroRespectoMuroSuperiorEncontrado(ElementEncontrado, PtoBusqueda);
        }
        private bool M2_1_BuscarSiMuroEncontradoIguaDireccionMuroReferencia(Element ElementEncontrado, XYZ PtoBusqueda)
        {
            try
            {
                if (_muroHost50CmSobrePtoinicialDTO == null) return false;

                Wall wallENcontrado = ((Wall)ElementEncontrado);

                _espesorMuro = wallENcontrado.ObtenerEspesorMuroFoot(_doc);
                _OrigenMuro = wallENcontrado.ObtenerOrigin();
                _direccionMuro = wallENcontrado.ObtenerDireccionEnElSentidoView(_view.RightDirection);// M2_1_ObtenerDireccionMuro(ElementEncontrado);

                _planarface = wallENcontrado.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(PtoBusqueda, new XYZ(0, 0, 1));

                if (_planarface == null)
                    _planarface = wallENcontrado.ObtenerCaraSuperior(); 

                _PtoSObreCarar = _planarface.GetPtosIntersFaceUtilizarPlanoNh(PtoBusqueda);
                ElementMasAlcentroEncontrado = ElementEncontrado;

                //veridicar si elemto encontrado en la misma disreccionde muro de referencia
                if (Util.IsSimilarValor(Math.Abs(Util.GetProductoEscalar(_muroHost50CmSobrePtoinicialDTO._direccionMuro, _direccionMuro)), 1))
                { return true; }
                else
                { return false; }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }

        }

        private bool M2_2_BuscarMuroMasAlCentroRespectoMuroSuperiorEncontrado(Element ElementEncontrado, XYZ PtoBusqueda)
        {
            try
            {
                //essto solo aplica cuando se usa segundo constructor
                if (_direccionEnfierrar == null) return false;
                Wall wallENcontrado = ((Wall)ElementEncontrado);

                _alturaMuro = wallENcontrado.ObtenerAlturaMuroFoot();
                //
                double distanciaSUbeoBaja = Util.CmToFoot(_alturaMuro * 0.1);
                if (Math.Abs(_OrigenMuro.Z - PtoBusqueda.Z) > 1)
                {
                    // entonces se esta seleccionado el mismo muro  entrer coordenadas pinf y pto sup 
                    //if (Math.Abs(Util.GetProductoEscalar(_direccionMuro, _seccionView.RightDirection)) != 1) return false; // se encontro muro a nivel entre pini y ppto sup pero no en la direccion vista
                    distanciaSUbeoBaja = distanciaSUbeoBaja * -1;
                }
                else // se esta analizando el muro sobre el ptoSup
                    distanciaSUbeoBaja = distanciaSUbeoBaja * 1;

                //buscar muro hacia el centro del muro
                BuscarMuros BuscarMuros = new BuscarMuros(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                var (MuroMAsAlCentro, espesor, ptoSobreMuro) = BuscarMuros.OBtenerRefrenciaMuro(_elem3d,
                                                                            PtoBusqueda + new XYZ(0, 0, distanciaSUbeoBaja) + _direccionEnfierrar * _distanciaAlCentroMuro,
                                                                            -_view.ViewDirection);

                if (MuroMAsAlCentro == null)
                {
                    ///Util.ErrorMsg("Error Buscar muro costado hacia abajo");
                    return false;
                }


                _espesorMuro = ((Wall)MuroMAsAlCentro).ObtenerEspesorMuroFoot(_doc);
                _OrigenMuro = ((Wall)MuroMAsAlCentro).ObtenerOrigin();
                _direccionMuro = ((Wall)MuroMAsAlCentro).ObtenerDireccionEnElSentidoView(_view.RightDirection);// M2_1_ObtenerDireccionMuro(MuroMAsAlCentro);

                _planarface = MuroMAsAlCentro.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(PtoBusqueda, new XYZ(0, 0, 1));
                //---muro buscado hacia el centro de muro
                _PtoSObreCarar = _planarface.GetPtosIntersFaceUtilizarPlanoNh(PtoBusqueda);
                ElementMasAlcentroEncontrado = MuroMAsAlCentro;
                //_planarface = wallENcontrado.ObtenerCaraSuperior();

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        private bool M3_ObtenerPtoSuperiorViga(XYZ PtoBusqueda, Element ElementEncontrado)
        {
            try
            {
                if (ElementEncontrado == null) return false;
                _planarface = ElementEncontrado.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(PtoBusqueda, new XYZ(0, 0, 1));
                FamilyInstance _familyInstance = ((FamilyInstance)ElementEncontrado);
                //  _planarface = _familyInstance.ObtenerCaraSuperior();
                if (_planarface == null) return false;

                //  IntersectionResult interseccionSObreCaraInferiorLOsa = _planarface.Project(PtoBusqueda);
                //  if (interseccionSObreCaraInferiorLOsa == null) return false;
                _PtoSObreCarar = PtoBusqueda;

                _espesorMuro = _familyInstance.ObtenerEspesorConPtos_foot(PtoBusqueda, _view.ViewDirection);
                _OrigenMuro = _familyInstance.ObtenerOrigin();
                _direccionMuro = _familyInstance.ObtenerDireccionEnElSentidoView(_view.RightDirection);  //M3_1_ObtenerDireccionViga(ElementEncontrado);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
        private bool M3_ObtenerPtoSuperiorColumna(XYZ PtoBusqueda, Element ElementEncontrado)
        {
            try
            {
                if (ElementEncontrado == null) return false;

                _planarface = ElementEncontrado.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(PtoBusqueda, new XYZ(0, 0, 1));
                FamilyInstance _familyInstance = ((FamilyInstance)ElementEncontrado);
                // _planarface = _familyInstance.ObtenerCaraSuperior();
                if (_planarface == null) return false;

                // IntersectionResult interseccionSObreCaraInferiorLOsa = _planarface.Project(PtoBusqueda);
                // if (interseccionSObreCaraInferiorLOsa == null) return false;
                _PtoSObreCarar = PtoBusqueda; //interseccionSObreCaraInferiorLOsa.XYZPoint;

                _espesorMuro = _familyInstance.ObtenerEspesorConPtos_foot(PtoBusqueda, _view.ViewDirection);
                _OrigenMuro = _familyInstance.ObtenerOrigin();
                _direccionMuro = _view.RightDirection;// _familyInstance.ObtenerDireccionEnElSentidoView(_view.RightDirection);  //M3_1_ObtenerDireccionViga(ElementEncontrado);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
        private bool M4_ObtenerPtoSuperior(XYZ PtoBusqueda, Element ElementEncontrado)
        {
            try
            {
                if (ElementEncontrado == null) return false;

                _planarface = ((Floor)ElementEncontrado).ObtenerCaraMasCercanaAPto_soloCArasHorizontales(PtoBusqueda, new XYZ(0, 0, 1));

                if (_planarface == null) return false;

                _PtoSObreCarar= _planarface.ObtenerPtosInterseccionFace(PtoBusqueda, new XYZ(0, 0, 1),true);

               //IntersectionResult interseccionSObreCaraInferiorLOsa = _planarface.Project(PtoBusqueda);
                if (_PtoSObreCarar.IsLargoCero())
                {
                    _PtoSObreCarar = _planarface.GetPtosIntersFaceUtilizarPlanoNh(PtoBusqueda);
                    return _PtoSObreCarar.IsDistintoLargoCero();
                }
              //  _PtoSObreCarar = interseccionSObreCaraInferiorLOsa.XYZPoint;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

    }
}
