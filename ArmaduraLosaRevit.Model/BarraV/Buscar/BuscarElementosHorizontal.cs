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
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.AppState;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarElementosHorizontal
    {
        private UIApplication _uiapp;
        private Document _doc;
        private double _largoDeBUsquedaFoot;
        private View _view;
        public List<ObjetosEncontradosDTO> listaObjEncontrados;
        private View3D _elem3d;
        private bool _IsConReferencia;
        private TipoElementoBArraV _nombreTipo;
        private XYZ _PtoSObreCaraSuperiorLOsa;

        private double _espesorMuro;
        private XYZ _OrigenMuro;
        private double _alturaMuro;

        public PlanarFace _planarface { get; set; }

        private double _espesorLosa;
        private XYZ _direccionMuro;
        private int _espesorViga;
        private XYZ _OrigenViga;
        private XYZ _direccionViga;

        public XYZ PuntoSobreFAceHost { get; set; }
        public Element ElementEncontrado { get; set; }
        public Element ElementMasAlcentroEncontrado;
#pragma warning disable CS0169 // The field 'BuscarElementosHorizontal.PtoBusqueda' is never used
        private XYZ PtoBusqueda;
#pragma warning restore CS0169 // The field 'BuscarElementosHorizontal.PtoBusqueda' is never used
        private XYZ VectorBusqueda;

        public BuscarElementosHorizontal(UIApplication uIApplication, double _largoDeBUsquedaFoot, View3D view3D, bool _isConreferencia = false)
        {
            this._uiapp = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._view = _doc.ActiveView;
            this.listaObjEncontrados = new List<ObjetosEncontradosDTO>();
            this._elem3d = view3D;
            this._IsConReferencia = _isConreferencia;
        }

        public bool BuscarObjetos(XYZ PtoBusqueda, XYZ VectorBusqueda_, bool IsDibujarRAyo = false)
        {
            try
            {
                VectorBusqueda = VectorBusqueda_;
                if (IsDibujarRAyo) CrearModeLineAyuda.modelarlineas(_doc, PtoBusqueda, PtoBusqueda + VectorBusqueda * _largoDeBUsquedaFoot);

                listaObjEncontrados = new List<ObjetosEncontradosDTO>();
                ElementCategoryFilter filterMuro = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
                ElementCategoryFilter filterLosa = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
                ElementCategoryFilter filterViga = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
                ElementCategoryFilter filterFund = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);
                ElementCategoryFilter filtercolum = new ElementCategoryFilter(BuiltInCategory.OST_Columns);
                ElementCategoryFilter filterGeneriMOdel = new ElementCategoryFilter(BuiltInCategory.OST_GenericModel);

                LogicalOrFilter f3 = new LogicalOrFilter(filterMuro, filterLosa);
                LogicalOrFilter f3b = new LogicalOrFilter(f3, filterViga);
                LogicalOrFilter f4 = new LogicalOrFilter(f3b, filterFund);
                LogicalOrFilter f5 = new LogicalOrFilter(f4, filtercolum);
                LogicalOrFilter f6 = new LogicalOrFilter(f5, filterGeneriMOdel);

                ReferenceIntersector ri = new ReferenceIntersector(f6, FindReferenceTarget.Face, _elem3d);



                var listaObjetos = ri.Find(PtoBusqueda, VectorBusqueda)
                    .Where(cc => cc.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(cc, _doc))
                    .OrderBy(c => c.Proximity).ToList();

                //seleccionar elemto
                //ReferenceIntersector Elemet = new ReferenceIntersector(filterLosa, FindReferenceTarget.Element, _elem3d);
                //var listaObjetosElement = Elemet.Find(PtoBusqueda, VectorBusqueda)
                //   .Where(cc => cc.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(cc, _doc))
                //   .OrderBy(c => c.Proximity).Select(c=> _doc.GetElement(c.GetReference())).ToList();

                double proximidadAnterior = -1;
                bool verificar = false;
                foreach (var ref2 in listaObjetos)
                {
                    if (ref2 != null)
                    {
                        if (ref2.Proximity < _largoDeBUsquedaFoot)
                        {
                            //si no tiene igual igual proximidad que el anteriro caso, entonces hay discontinuidad  espacio en blanco
                            if (!Util.IsSimilarValor(proximidadAnterior, ref2.Proximity, 0.001) && verificar && AppManejadorBarraHState.IsOk
                                && AppManejadorBarraHState.TipoElementoSeleccionado != ElementoSeleccionado.Losa) break;

                            Reference WallRef = ref2.GetReference();
                            PuntoSobreFAceHost = WallRef.GlobalPoint;
                            ElementEncontrado = _doc.GetElement(WallRef);
                            if (ElementEncontrado == null) continue;

                            M1_ResetParametrosIniciales();

                            _nombreTipo = TipoElementoBArraV.none;

                            if (ElementEncontrado is Wall)
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;
                                M2_ObtenerDatosMuro(ElementEncontrado, PtoBusqueda);
                                _nombreTipo = TipoElementoBArraV.muro;
                            }
                            else if (ElementEncontrado.Category.Name == "Structural Framing")
                            {
                                M3_ObtenerPtoViga(ElementEncontrado, PtoBusqueda);
                                _nombreTipo = TipoElementoBArraV.viga;
                            }
                            else if (ElementEncontrado.Category.Name == "Structural Columns")
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;
                                M3_ObtenerPtoColumna(ElementEncontrado, PtoBusqueda);
                                _nombreTipo = TipoElementoBArraV.columna;
                            }

                            else if (ElementEncontrado.Category.Name == "Structural Foundations")
                            {
                                M2_ObtenerDatosFundacion(ElementEncontrado, PtoBusqueda);
                                _nombreTipo = TipoElementoBArraV.fundacion;
                            }
                            else if (ElementEncontrado.Category.Name == "Floors")
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;
                                M2_ObtenerDatosFloor(ElementEncontrado, PtoBusqueda);
                                _nombreTipo = TipoElementoBArraV.losa;
                            }
                            else if (ElementEncontrado.Category.Name == "Generic Models" && ElementEncontrado.Name == "PASADA_AZUL")
                            {
                                //  if (!ElementEncontrado.IsEstructural()) continue;
                                // M2_ObtenerDatosFloor(ElementEncontrado, PtoBusqueda);
                                // _nombreTipo = TipoElementoBArraV.GenericModel;
                            }

                            if (_nombreTipo == TipoElementoBArraV.none) continue;



                            ObjetosEncontradosDTO objetosEncontrados = new ObjetosEncontradosDTO()
                            {
                                elemt = ElementEncontrado,
                                elemtid = ElementEncontrado.Id.IntegerValue,
                                nombreTipo = _nombreTipo,
                                distancia = ref2.Proximity,
                                PlanarfaceIntersectada = _planarface,
                                NormalFace = _planarface?.FaceNormal,
                                ptoInterseccion = PuntoSobreFAceHost,
                                EspesorMuro = _espesorMuro,
                                EspesorViga = _espesorViga,
                                EspesorLosa = _espesorLosa
                            };
                            listaObjEncontrados.Add(objetosEncontrados);

                            //cambiar proximidad
                            // si el elmento anterior y el actual estan a la misma distancia, no analizar pq deben estar juntas y no existe espacio vacio
                            if (!Util.IsSimilarValor(proximidadAnterior, ref2.Proximity, 0.01) && _planarface!=null)
                                //si vector face de objeto encontrada tiene normal igual direccion  a vector de busqueda , verificar si el sigueinte objeto intersectado esta separado
                                //vector normal
                                verificar = Util.IsParallel_igualSentido(VectorBusqueda, _planarface?.FaceNormal, 0.9);
                            else
                                verificar = false;


                            proximidadAnterior = ref2.Proximity;
                            //

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

        public bool BuscarObjetos_Pasadas(XYZ PtoBusqueda, XYZ VectorBusqueda_, bool IsDibujarRAyo = false)
        {
            try
            {
                VectorBusqueda = VectorBusqueda_;
                if (IsDibujarRAyo) CrearModeLineAyuda.modelarlineas(_doc, PtoBusqueda, PtoBusqueda + VectorBusqueda * _largoDeBUsquedaFoot);

                listaObjEncontrados = new List<ObjetosEncontradosDTO>();

                ElementCategoryFilter filterGeneriMOdel = new ElementCategoryFilter(BuiltInCategory.OST_GenericModel);

                ReferenceIntersector ri = new ReferenceIntersector(filterGeneriMOdel, FindReferenceTarget.Face, _elem3d);
                var listaObjetos = ri.Find(PtoBusqueda, VectorBusqueda)
                    .Where(cc => cc.Proximity < _largoDeBUsquedaFoot)
                    .OrderBy(c => c.Proximity).ToList();

                foreach (var ref2 in listaObjetos)
                {
                    if (ref2 != null)
                    {
                        if (ref2.Proximity < _largoDeBUsquedaFoot)
                        {
                            //si no tiene igual igual proximidad que el anteriro caso, entonces hay discontinuidad  espacio en blanco
   

                            Reference WallRef = ref2.GetReference();
                            PuntoSobreFAceHost = WallRef.GlobalPoint;
                            ElementEncontrado = _doc.GetElement(WallRef);
                            if (ElementEncontrado == null) continue;

                            M1_ResetParametrosIniciales();

                            _nombreTipo = TipoElementoBArraV.none;
                            if (ElementEncontrado.Category.Name == "Generic Models" )
                            {
                                 _nombreTipo = TipoElementoBArraV.GenericModel;
                            }

                            if (_nombreTipo == TipoElementoBArraV.none) continue;



                            ObjetosEncontradosDTO objetosEncontrados = new ObjetosEncontradosDTO()
                            {
                                elemt = ElementEncontrado,
                                elemtid = ElementEncontrado.Id.IntegerValue,
                                nombreTipo = _nombreTipo,
                                distancia = ref2.Proximity,
                                PlanarfaceIntersectada = _planarface,
                                NormalFace = _planarface?.FaceNormal,
                                ptoInterseccion = PuntoSobreFAceHost,
                                EspesorMuro = _espesorMuro,
                                EspesorViga = _espesorViga,
                                EspesorLosa = _espesorLosa
                            };
                            listaObjEncontrados.Add(objetosEncontrados);
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


        private void M1_ResetParametrosIniciales()
        {
            _nombreTipo = TipoElementoBArraV.none;
            _PtoSObreCaraSuperiorLOsa = XYZ.Zero;
            _espesorMuro = 0;
            _OrigenMuro = XYZ.Zero;
            _direccionMuro = XYZ.Zero;
            _espesorViga = 0;
            _OrigenViga = XYZ.Zero;
            _direccionViga = XYZ.Zero;
        }
        private bool M2_ObtenerDatosMuro(Element ElementEncontrado, XYZ PtoBusqueda)
        {
            try
            {
                if (ElementEncontrado == null) return false;
                Wall wallENcontrado = ((Wall)ElementEncontrado);

                if (ElementEncontrado == null) return false;
                _planarface = wallENcontrado.ObtenerPlanerFaceMasCercano_ConVector(PtoBusqueda, VectorBusqueda, _IsConReferencia);
                _direccionMuro = wallENcontrado.ObtenerDireccionEnElSentidoView(_view.RightDirection);// M2_1_ObtenerDireccionMuro(ElementEncontrado);
                _espesorMuro = wallENcontrado.ObtenerEspesorMuroFoot(_doc);
                _OrigenMuro = wallENcontrado.ObtenerOrigin();
                _alturaMuro = wallENcontrado.ObtenerAlturaMuroFoot();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M2_ObtenerDatosMuro'  ElementID:{ElementEncontrado?.Id} ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private bool M2_ObtenerDatosFloor(Element ElementEncontrado, XYZ PtoBusqueda)
        {
            _planarface = (ElementEncontrado as Floor).ObtenerfaceMAsCercanaConDireccion_foot(PtoBusqueda, VectorBusqueda, _IsConReferencia);
            _espesorLosa = (ElementEncontrado as Floor).ObtenerEspesorConPtos_foot(PtoBusqueda, VectorBusqueda, _IsConReferencia);
            //

            return true;
        }
        private bool M2_ObtenerDatosFundacion(Element ElementEncontrado, XYZ PtoBusqueda)
        {
            _planarface = (ElementEncontrado as Floor).ObtenerPlanerFaceMasCercano_ConVector(PtoBusqueda, VectorBusqueda, _IsConReferencia);

            return true;
        }
        private bool M3_ObtenerPtoViga(Element ElementEncontrado, XYZ ptoBusqueda)
        {
            try
            {
                if (ElementEncontrado == null) return false;

                var _vigaSelect = (FamilyInstance)ElementEncontrado;
                if (_vigaSelect.StructuralType == StructuralType.Beam)
                {
                    _planarface = _vigaSelect.ObtenerPlanerFaceMasCercano_ConVector(ptoBusqueda, VectorBusqueda, _IsConReferencia);
                    _espesorMuro = _vigaSelect.ObtenerEspesorConPtos_foot(ptoBusqueda, _view.ViewDirection);// tipoFamiliaMuro.Width;
                    _OrigenMuro = _vigaSelect.ObtenerOrigin();
                    _direccionMuro = _vigaSelect.ObtenerDireccionEnElSentidoView(_view.RightDirection);  //M3_1_ObtenerDireccionViga(ElementEncontrado);
                }
                else
                {
                    Util.ErrorMsg($"FamilyInstance tipo {_vigaSelect.StructuralType} no esta implementada. ");
                    return false;
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error en 'M3_ObtenerPtoViga'  ElementID:{ElementEncontrado?.Id} .\n ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private bool M3_ObtenerPtoColumna(Element ElementEncontrado, XYZ ptoBusqueda)
        {
            try
            {
                if (ElementEncontrado == null) return false;

                var _columnaSelect = (FamilyInstance)ElementEncontrado;
                if (_columnaSelect.StructuralType == StructuralType.Column)
                {
                    _planarface = _columnaSelect.ObtenerPlanerFaceMasCercano_ConVector(ptoBusqueda, VectorBusqueda, _IsConReferencia);
                    _espesorMuro = _columnaSelect.ObtenerEspesorConPtos_foot(ptoBusqueda, _view.ViewDirection);// tipoFamiliaMuro.Width;
                    _OrigenMuro = _columnaSelect.ObtenerOrigin();
                    _direccionMuro = _columnaSelect.ObtenerDireccionEnElSentidoView(_view.RightDirection);  //M3_1_ObtenerDireccionViga(ElementEncontrado);
                }
                else
                {
                    Util.ErrorMsg($"FamilyInstance tipo {_columnaSelect.StructuralType} no esta implementada. ");
                    return false;
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error en 'M3_ObtenerPtoViga' ElementID:{ElementEncontrado?.Id} . \nex:{ex.Message}");
                return false;
            }
            return true;
        }


    }
}
