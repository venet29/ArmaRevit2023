using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarFundacionLosa
    {
        private UIApplication uIApplication;
        private Document _doc;
        private double _largoDeBUsquedaFoot;
        public XYZ PuntoSobreFAceHostIntersectada { get; private set; }
        public Element FundLosaElementHost { get;  set; }
        public XYZ _PtoSObreCaraInferiorFund { get; private set; }
        public XYZ _PtoSObreCaraSupFund { get; private set; }

        public List<Buscar_elementoEncontradoDTO> _listaFundLosaEncontrado { get; set; }
        public XYZ _PtoSObreCaraSuperiorFund { get;  set; }

        public BuscarFundacionLosa(UIApplication uIApplication, double _largoDeBUsquedaFoot)
        {
            this.uIApplication = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._listaFundLosaEncontrado = new List<Buscar_elementoEncontradoDTO>();
        }

        public bool OBtenerRefrenciaFundacionSegunVector(View3D elem3d, XYZ PtoBusqueda, XYZ VectorBusqueda, bool IsDibujarRAyo = false)
        {
            if (IsDibujarRAyo) CrearModeLineAyuda.modelarlineas(_doc, PtoBusqueda, PtoBusqueda + VectorBusqueda * _largoDeBUsquedaFoot);



            ElementCategoryFilter filterFund = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);

            //LogicalOrFilter f1 = new LogicalOrFilter(filterFund, filterFund);

            ReferenceIntersector ri = new ReferenceIntersector(filterFund, FindReferenceTarget.All, elem3d);
            // ReferenceWithContext ref2 = ri.FindNearest(PtoBusqueda, VectorBusqueda);
            var listaObjetos = ri.Find(PtoBusqueda, VectorBusqueda)
                                  .Where(x => x.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(x, _doc))
                                 .OrderBy(c => c.Proximity).ToList();

            foreach (var ref2 in listaObjetos)
            {
                if (ref2 != null)
                {
                    if (ref2.Proximity < _largoDeBUsquedaFoot)
                    {
                        Reference FundRef = ref2.GetReference();
                        PuntoSobreFAceHostIntersectada = FundRef.GlobalPoint;
                        FundLosaElementHost = _doc.GetElement(FundRef);

                        ObtenerPtoSuperior(PtoBusqueda, FundLosaElementHost);

                        if (FundLosaElementHost.Category.Name == "Structural Foundations")
                        {
                            if (FundLosaElementHost.Name.ToLower().Contains(ConstNH.CONST_FILTRAR_HORMIGON_POBRE)) continue;

                            if (ObtenerPtoInferior(PtoBusqueda, FundLosaElementHost))
                            {
                                _listaFundLosaEncontrado.Add(new Buscar_elementoEncontradoDTO()
                                {
                                    distancia = ref2.Proximity,
                                    PtoSObreCaraInferiorFundLosa = _PtoSObreCaraInferiorFund,
                                    PtoSObreCaraSuperiorFundLosa = _PtoSObreCaraSuperiorFund,
                                    _TipoElementoBArraV = TipoElementoBArraV.fundacion
                                });
                            }
                        }

                    }
                }
            }
            // if (_listaFundLosaEncontrado.Count == 0) Util.ErrorMsg("No se encontro Fundacion para ser host de rebar");
            return (_listaFundLosaEncontrado.Count == 0 ? false : true);
        }


        public bool OBtenerRefrenciaLosaSegunVector(View3D elem3d, XYZ PtoBusqueda, XYZ VectorBusqueda, bool IsDibujarRAyo = false)
        {
            if (IsDibujarRAyo) CrearModeLineAyuda.modelarlineas(_doc, PtoBusqueda, PtoBusqueda + VectorBusqueda * _largoDeBUsquedaFoot);


            ElementCategoryFilter filterLosa = new ElementCategoryFilter(BuiltInCategory.OST_Floors);

            ReferenceIntersector ri = new ReferenceIntersector(filterLosa, FindReferenceTarget.All, elem3d);
            // ReferenceWithContext ref2 = ri.FindNearest(PtoBusqueda, VectorBusqueda);
            var listaObjetos = ri.Find(PtoBusqueda, VectorBusqueda)
                                  .Where(x => x.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(x, _doc))
                                 .OrderBy(c => c.Proximity).ToList();

            foreach (var ref2 in listaObjetos)
            {
                if (ref2 != null)
                {
                    if (ref2.Proximity < _largoDeBUsquedaFoot)
                    {
                        Reference FundRef = ref2.GetReference();
                        PuntoSobreFAceHostIntersectada = FundRef.GlobalPoint;
                        FundLosaElementHost = _doc.GetElement(FundRef);

                        ObtenerPtoSuperior(PtoBusqueda, FundLosaElementHost);

                        if (FundLosaElementHost.Category.Name == "Floors")
                        {

                            if (!FundLosaElementHost.IsEstructural()) continue;
                            //continue;// no aplica solo se busca fundaciones
                            if (ObtenerPtoInferior(PtoBusqueda, FundLosaElementHost))
                            {
                                _listaFundLosaEncontrado.Add(new Buscar_elementoEncontradoDTO()
                                {
                                    distancia = ref2.Proximity,
                                    PtoSObreCaraInferiorFundLosa = _PtoSObreCaraInferiorFund,
                                    
                                    _TipoElementoBArraV = TipoElementoBArraV.losa
                                });
                            }
                        }
                    }
                }
            }
            // if (_listaFundLosaEncontrado.Count == 0) Util.ErrorMsg("No se encontro Fundacion para ser host de rebar");
            return (_listaFundLosaEncontrado.Count == 0 ? false : true);
        }


        private bool ObtenerPtoInferior(XYZ PtoBusqueda, Element ElementEncontrado)
        {

            PlanarFace faceInferior = ElementEncontrado.ObtenerCaraInferior();
            if (faceInferior == null) return false;
            _PtoSObreCaraInferiorFund = faceInferior.ObtenerPtosInterseccionFace(PtoBusqueda, new XYZ(0, 0, 1),true);
            // IntersectionResult interseccionSObreCaraInferiorLOsa = faceInferior.Project(PtoBusqueda);


            if (_PtoSObreCaraInferiorFund.IsLargoCero())
            {
                _PtoSObreCaraInferiorFund = faceInferior.GetPtosIntersFaceUtilizarPlanoNh(PtoBusqueda);
                return _PtoSObreCaraInferiorFund.IsDistintoLargoCero();
            }
            //if (interseccionSObreCaraInferiorLOsa == null) return false;
            //_PtoSObreCaraInferiorFund = interseccionSObreCaraInferiorLOsa.XYZPoint;
            return true;
        }

        private bool ObtenerPtoSuperior(XYZ PtoBusqueda, Element ElementEncontrado)
        {

            PlanarFace faceSuperior = ElementEncontrado.ObtenerPLanarFAce_superior();
            if (faceSuperior == null) return false;
            _PtoSObreCaraSuperiorFund = faceSuperior.ObtenerPtosInterseccionFace(PtoBusqueda, new XYZ(0, 0, 1), true);

            if (_PtoSObreCaraSuperiorFund.IsLargoCero())
            {
                _PtoSObreCaraSuperiorFund = faceSuperior.GetPtosIntersFaceUtilizarPlanoNh(PtoBusqueda);
                return _PtoSObreCaraSuperiorFund.IsDistintoLargoCero();
            }
            //IntersectionResult interseccionSObreCaraInferiorLOsa = faceSuperior.Project(PtoBusqueda);
            //if (interseccionSObreCaraInferiorLOsa == null) return false;
            //_PtoSObreCaraSuperiorFund = interseccionSObreCaraInferiorLOsa.XYZPoint;
            return true;
        }

        public double ObtenerZmenorDeElemetosEncontrados()
        {
            if (_listaFundLosaEncontrado.Count == 0) return 0;

            Buscar_elementoEncontradoDTO buscarFundLosaDTO = _listaFundLosaEncontrado.MinBy(c => -c.distancia);
            return buscarFundLosaDTO.PtoSObreCaraInferiorFundLosa.Z;
        }

    }
}
