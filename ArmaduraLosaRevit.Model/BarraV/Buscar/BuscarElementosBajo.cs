using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using System.Diagnostics;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda;
using ArmaduraLosaRevit.Model.UTILES;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarElementosBajo
    {
        private UIApplication _uiapp;
        private Document _doc;
        private double _largoDeBUsquedaFoot;
        private View _view;
        public List<ObjetosEncontradosDTO> listaObjEncontrados;
        private View3D _elem3d;

        private readonly XYZ ptoBusqueda_RefeciaMUro;
        // private readonly double distanciaAlCentroMuro;
        private TipoElementoBArraV _nombreTipo;

        public XYZ _PtoSObreCaraSuperiorLOsa;

        private XYZ _PtoSObreCaraInferiorLOsa;
        private double _espesorMuro;
        private XYZ _OrigenMuro;
        private XYZ _direccionMuro;
        private PlanarFace _planarface;

        public XYZ PuntoSobreFAceHost { get; private set; }
        public Element ElementEncontrado { get; set; }
        public BuscarElementosBajo(UIApplication uIApplication, double _largoDeBUsquedaFoot, View3D view3D, XYZ ptoBusqueda_refeciaMUro)
        {
            this._uiapp = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._view = _doc.ActiveView;
            listaObjEncontrados = new List<ObjetosEncontradosDTO>();
            this._elem3d = view3D;

            this.ptoBusqueda_RefeciaMUro = ptoBusqueda_refeciaMUro;
            // this.distanciaAlCentroMuro = DistanciaAlCentroMuro;
        }

        public bool BuscarObjetosInferiorBArra(XYZ PtoBusqueda, XYZ VectorBusqueda)
        {
            try
            {
                listaObjEncontrados = new List<ObjetosEncontradosDTO>();
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
                //CrearModeLineAyuda.modelarlinea_sinTrans(_doc, PtoBusqueda, PtoBusqueda + VectorBusqueda * 3);

                ReferenceIntersector ri = new ReferenceIntersector(f5, FindReferenceTarget.Face, _elem3d);
                var listaObjetos = ri.Find(PtoBusqueda, VectorBusqueda)
                                     .Where(cc => cc.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(cc, _doc))
                                    .OrderBy(c => c.Proximity).ToList();

                foreach (var ref2 in listaObjetos)
                {
                    if (ref2 != null)
                    {
                        Reference WallRef = ref2.GetReference();
                        ElementEncontrado = _doc.GetElement(WallRef);


                        if (ref2.Proximity < _largoDeBUsquedaFoot)
                        {

                            PuntoSobreFAceHost = WallRef.GlobalPoint;
                            ElementEncontrado = _doc.GetElement(WallRef);
                            if (ElementEncontrado == null) continue;

                            M1_ResetParametrosIniciales();
                            _nombreTipo = TipoElementoBArraV.none;

                            if (ElementEncontrado is Wall)
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;
                                if (!M3_ObtenerPtoInferiorMuro(ElementEncontrado, PtoBusqueda, -ref2.Proximity)) continue;
                                _nombreTipo = TipoElementoBArraV.muro;
                            }
                            else if (ElementEncontrado.Category.Name == "Structural Framing")
                            {
                                if (!M3_ObtenerPtoInferiorViga(PuntoSobreFAceHost, ElementEncontrado)) continue;
                                _nombreTipo = TipoElementoBArraV.viga;
                            }
                            else if (ElementEncontrado.Category.Name == "Structural Columns")
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;
                                if (!M3_ObtenerPtoInferiorColumn(PuntoSobreFAceHost, ElementEncontrado)) continue;
                                _nombreTipo = TipoElementoBArraV.columna;
                            }
                            else if (ElementEncontrado.Category.Name == "Structural Foundations")
                            {
                                if (ElementEncontrado.Name.ToLower().Contains(ConstNH.CONST_FILTRAR_HORMIGON_POBRE))
                                {
                                    continue;
                                }
                                if (!M4_ObtenerPtoInferior(PuntoSobreFAceHost, ElementEncontrado)) continue;
                                _nombreTipo = TipoElementoBArraV.fundacion;
                            }
                            else if (ElementEncontrado.Category.Name == "Floors")
                            {
                                if (!ElementEncontrado.IsEstructural()) continue;
                                if (!M4_ObtenerPtoInferior(PuntoSobreFAceHost, ElementEncontrado)) continue;
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
                                ptoSObreCaraInferiorLosa = _PtoSObreCaraInferiorLOsa,
                                EspesorMuro = _espesorMuro,
                                OrigenMuro = _OrigenMuro,
                                DireccionMuro = _direccionMuro,
                                PlanarfaceIntersectada = _planarface,
                                NormalFace = _planarface.FaceNormal
                            };
                            AuxlistaObjEncontrados.Add(objetosEncontrados);
                        }
                    }
                }

                // buscando discontinuridada
                foreach (var item in AuxlistaObjEncontrados.OrderBy(c => c.distancia))
                {
                    if (Util.IsIgualSentido(item.NormalFace, VectorBusqueda) || item.nombreTipo == TipoElementoBArraV.fundacion) // 0.5 por los vectore no son completamente pararelos ,                     
                        listaObjEncontrados.Add(item);
                    else
                    {
                        var list = AuxlistaObjEncontrados.Where(c => Util.IsSimilarValor(c.distancia, item.distancia, 0.01) &&
                                                                     c.elemtid != item.elemtid &&
                                                                    Util.IsIgualSentido(c.NormalFace, VectorBusqueda)).ToList();

                        if (list.Count > 0)
                        { listaObjEncontrados.Add(item); }
                        else
                            continue;
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


        #region metodos ayuda
        private void M1_ResetParametrosIniciales()
        {
            _nombreTipo = TipoElementoBArraV.none;
            _PtoSObreCaraInferiorLOsa = XYZ.Zero;
            _planarface = null;
            _espesorMuro = 0;
            _OrigenMuro = XYZ.Zero;
            _direccionMuro = XYZ.Zero;

        }

        private bool M2_ObtenerDatosMuroParaleloVIew(Element ElementEncontrado, XYZ PtoBusqueda, double proximidad)
        {
            try
            {
                if (ElementEncontrado == null) return false;

                Wall wallENcontrado = ((Wall)ElementEncontrado);

                _PtoSObreCaraInferiorLOsa = ElementEncontrado.ObtenerPtosInterseccionFaceInferior(PtoBusqueda);

                _direccionMuro = ObtenerDireccionMuro(ElementEncontrado);
                //en caso muro sea perpendicular a view
                if (!Util.IsParallel(_direccionMuro, _view.RightDirection))
                {
                    //buscar muro hacia el centro del muro
                    BuscarMuros BuscarMuros = new BuscarMuros(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);
                    bool istru = true;
                    int cont = 0;
                    while (istru && cont < 10)
                    {

                        var (MuroMAsAlCentro, espesor, ptoSobreMuro) = BuscarMuros
                                                            .OBtenerRefrenciaMuro(_elem3d, ptoBusqueda_RefeciaMUro.AsignarZ(PtoBusqueda.Z + proximidad) + cont * _view.RightDirection * Util.CmToFoot(10), -_view.ViewDirection);
                        if (MuroMAsAlCentro == null)
                        {

                            cont = cont + 1;
                            //  Util.ErrorMsg("Error Buscar muro costado hacia abajo");
                            Debug.WriteLine("Error Buscar muro costado hacia abajo");
                            //return false;
                        }
                        istru = false;
                        ElementEncontrado = MuroMAsAlCentro;
                    }
                }

                //---muro buscado hacia el centro de muro
                _planarface = wallENcontrado.ObtenerPlanerFaceMasCercano(PtoBusqueda);

                _espesorMuro = wallENcontrado.ObtenerEspesorMuroFoot(_doc);
                _OrigenMuro = wallENcontrado.ObtenerOrigin();
                _direccionMuro = ObtenerDireccionMuro(ElementEncontrado);

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private XYZ ObtenerDireccionMuro(Element ElementEncontrado)
        {
            XYZ _aux_direccionMuro = (ElementEncontrado as Wall).ObtenerDireccion();
            double factorDireccion = Util.GetProductoEscalar(_view.RightDirection.GetXY0(), _aux_direccionMuro.GetXY0());
            factorDireccion = (factorDireccion == 0 ? 1 : factorDireccion / Math.Abs(factorDireccion));

            _aux_direccionMuro = new XYZ(_aux_direccionMuro.X * factorDireccion, _aux_direccionMuro.Y * factorDireccion, _aux_direccionMuro.Z);
            return _aux_direccionMuro;
        }

        private bool M3_ObtenerPtoInferiorMuro(Element ElementEncontrado, XYZ PtoBusqueda, double proximidad)
        {
            try
            {
                if (ElementEncontrado == null) return false;

                Wall wallENcontrado = ((Wall)ElementEncontrado);
                _direccionMuro = ObtenerDireccionMuro(ElementEncontrado);

                //en caso muro sea perpendicular a view
                if (!Util.IsParallel(_direccionMuro, _view.RightDirection))
                {
                    var _planarfaceAUX = ElementEncontrado.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(PtoBusqueda, new XYZ(0, 0, -1));
                    //buscar muro hacia el centro del muro
                    BuscarMuros BuscarMuros = new BuscarMuros(_uiapp, ConstNH.CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW);

                    bool istru = true;
                    int cont = 0;
                    XYZ deltaParaNoEstarEnborde = -_planarfaceAUX.FaceNormal * Util.CmToFoot(5);// vector par amover el pto de busqueda para no estar justo en la cara inferior o superior
                    while (istru && cont < 10)
                    {                   
                        var (MuroMAsAlCentro, espesor, ptoSobreMuro) = BuscarMuros.OBtenerRefrenciaMuro(_elem3d,
                                                        ptoBusqueda_RefeciaMUro.AsignarZ(PtoBusqueda.Z + proximidad) 
                                                        + cont * _view.RightDirection * Util.CmToFoot(10)+ deltaParaNoEstarEnborde, -_view.ViewDirection);
                        if (MuroMAsAlCentro == null)
                        {
                            cont = cont + 1;
                            //  Util.ErrorMsg("Error Buscar muro costado hacia abajo");
                            Debug.WriteLine("Error Buscar muro costado hacia abajo");
                            // return false;
                        }
                        else
                        {
                            istru = false;
                            _espesorMuro = espesor;
                            ElementEncontrado = MuroMAsAlCentro;
                        }
                    }
                }
                else
                    _espesorMuro = wallENcontrado.ObtenerEspesorMuroFoot(_doc);
                //---muro buscado hacia el centro de muro
                _planarface = wallENcontrado.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(PtoBusqueda, new XYZ(0, 0, -1));
                _PtoSObreCaraInferiorLOsa = _planarface.ProjectNH(PtoBusqueda);

                _OrigenMuro = wallENcontrado.ObtenerOrigin();
                _direccionMuro = ObtenerDireccionMuro(ElementEncontrado);

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        private bool M3_ObtenerPtoInferiorViga(XYZ ptoBusqueda, Element elementEncontrado)
        {
            try
            {
                if (elementEncontrado == null) return false;

                PlanarFace faceInferior = ElementEncontrado.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(ptoBusqueda, new XYZ(0, 0, -1));
                FamilyInstance _familyInstance = ((FamilyInstance)ElementEncontrado);
                // PlanarFace faceInferior = (elementEncontrado).ObtenerCaraInferior();
                if (faceInferior == null) return false;
                // IntersectionResult interseccionSObreCaraInferiorLOsa = faceInferior.Project(ptoBusqueda);
                // if (interseccionSObreCaraInferiorLOsa == null) return false;
                _PtoSObreCaraInferiorLOsa = ptoBusqueda;// interseccionSObreCaraInferiorLOsa.XYZPoint;

                var _vigaSelect = (FamilyInstance)ElementEncontrado;
                if (_vigaSelect.StructuralType == StructuralType.Beam)
                {
                    //************
                    _planarface = faceInferior; ///_vigaSelect.ObtenerPlanerFaceMasCercano(ptoBusqueda);
                    _espesorMuro = _vigaSelect.ObtenerEspesorConPtos_foot(ptoBusqueda, _view.ViewDirection);
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

                Util.ErrorMsg($"Error en 'ObtenerPtoInferiorViga' ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private bool M3_ObtenerPtoInferiorColumn(XYZ ptoBusqueda, Element elementEncontrado)
        {
            try
            {
                if (elementEncontrado == null) return false;

                PlanarFace faceInferior = ElementEncontrado.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(ptoBusqueda, new XYZ(0, 0, -1));
                FamilyInstance _familyInstance = ((FamilyInstance)ElementEncontrado);
                // PlanarFace faceInferior = (elementEncontrado).ObtenerCaraInferior();
                if (faceInferior == null) return false;
                // IntersectionResult interseccionSObreCaraInferiorLOsa = faceInferior.Project(ptoBusqueda);
                //if (interseccionSObreCaraInferiorLOsa == null) return false;
                _PtoSObreCaraInferiorLOsa = ptoBusqueda;// interseccionSObreCaraInferiorLOsa.XYZPoint;

                var _vigaSelect = (FamilyInstance)ElementEncontrado;
                if (_vigaSelect.StructuralType == StructuralType.Column)
                {
                    //************
                    _planarface = faceInferior; ///_vigaSelect.ObtenerPlanerFaceMasCercano(ptoBusqueda);
                    _espesorMuro = _vigaSelect.ObtenerEspesorConPtos_foot(ptoBusqueda, _view.ViewDirection);
                    _OrigenMuro = _vigaSelect.ObtenerOrigin();
                    _direccionMuro = _view.RightDirection;  //M3_1_ObtenerDireccionViga(ElementEncontrado);
                }
                else
                {
                    Util.ErrorMsg($"FamilyInstance tipo {_vigaSelect.StructuralType} no esta implementada. ");
                    return false;
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error en 'ObtenerPtoInferiorViga' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool M4_ObtenerPtoInferior(XYZ PtoBusqueda, Element ElementEncontrado)
        {
            try
            {
                if (ElementEncontrado == null) return false;
                _planarface = ElementEncontrado.ObtenerCaraMasCercanaAPto_soloCArasHorizontales(PtoBusqueda, new XYZ(0, 0, -1));

                if (_planarface == null) return false;

                //_PtoSObreCaraSuperiorLOsa = ElementEncontrado.ObtenerPtoInterseccionCara_segunDireccion(PtoBusqueda, new XYZ(0, 0, -1));
                //_PtoSObreCaraInferiorLOsa = ElementEncontrado.ObtenerPtoInterseccionCara_segunDireccion(PtoBusqueda, new XYZ(0, 0, -1));// interseccionSObreCaraInferiorLOsa.XYZPoint;

                _PtoSObreCaraSuperiorLOsa = ElementEncontrado.ObtenerPtosInterseccionFaceSuperior(PtoBusqueda);
                _PtoSObreCaraInferiorLOsa = ElementEncontrado.ObtenerPtosInterseccionFaceInferior(PtoBusqueda);// interseccionSObreCaraInferiorLOsa.XYZPoint;

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
        #endregion
    }
}
