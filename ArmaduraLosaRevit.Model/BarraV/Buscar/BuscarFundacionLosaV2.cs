using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarFundacionLosaV2
    {
        private UIApplication uIApplication;
        private Document _doc;
        private double _largoDeBUsquedaFoot;
        private PlanarFace faceSelecc;


        public XYZ PuntoSobreFAceHostIntersectada { get; private set; }
        public Element FundLosaElementHost { get; private set; }

        public XYZ _PtoSObreCaraLosaFund { get; private set; }

        public List<Buscar_elementoEncontradoDTO> _listaFundLosaEncontrado { get; set; }


        public BuscarFundacionLosaV2(UIApplication uIApplication, double _largoDeBUsquedaFoot)
        {
            this.uIApplication = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._listaFundLosaEncontrado = new List<Buscar_elementoEncontradoDTO>();
        }


        public bool OBtenerRefrenciaLosa(View3D elem3d, XYZ PtoBusqueda, XYZ VectorBusqueda, TipoCaraObjeto _TipoCaraObjeto)
        {
            ElementCategoryFilter filterLosa = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            ElementCategoryFilter filterFund = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);

            LogicalOrFilter f1 = new LogicalOrFilter(filterLosa, filterFund);

            ReferenceIntersector ri = new ReferenceIntersector(f1, FindReferenceTarget.All, elem3d);
            // ReferenceWithContext ref2 = ri.FindNearest(PtoBusqueda, VectorBusqueda);
            var listaObjetos = ri.Find(PtoBusqueda, VectorBusqueda)
                                .Where(x => x.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(x, _doc))
                                .OrderBy(c => c.Proximity);

            foreach (var ref2 in listaObjetos)
            {
                if (ref2 != null)
                {
                    if (ref2.Proximity < _largoDeBUsquedaFoot)
                    {
                        Reference FundRef = ref2.GetReference();
                        PuntoSobreFAceHostIntersectada = FundRef.GlobalPoint;
                        FundLosaElementHost = _doc.GetElement(FundRef);

                     
                         if (FundLosaElementHost.Category.Name == "Floors")
                        {
                            if (FundLosaElementHost.Name.ToLower().Contains(ConstNH.CONST_FILTRAR_HORMIGON_POBRE)) continue;
                            if (!FundLosaElementHost.IsEstructural()) continue;
                            if (ObtenerPto_(PtoBusqueda, FundLosaElementHost, _TipoCaraObjeto))
                            {
                                XYZ ptoInferior = default;
                                XYZ ptoSuperior = default;


                                if (_TipoCaraObjeto == TipoCaraObjeto.Superior)
                                {
                                    ptoSuperior = _PtoSObreCaraLosaFund;
                                }
                                else if (_TipoCaraObjeto == TipoCaraObjeto.Inferior)
                                    ptoInferior = _PtoSObreCaraLosaFund;


                                _listaFundLosaEncontrado.Add(new Buscar_elementoEncontradoDTO()
                                {
                                    _PlanarFace = faceSelecc,
                                    distancia = ref2.Proximity,
                                    PtoSObreCaraInferiorFundLosa = ptoInferior,
                                    PtoSObreCaraSuperiorFundLosa=ptoSuperior,
                                    _TipoElementoBArraV = TipoElementoBArraV.losa
                                });
                            }
                        }
                    }
                }
            }

            return (_listaFundLosaEncontrado.Count == 0 ? false : true);
        }

        private bool ObtenerPto_(XYZ PtoBusqueda, Element ElementEncontrado, TipoCaraObjeto _TipoCaraObjeto)
        {

            faceSelecc=null;

            if(_TipoCaraObjeto==TipoCaraObjeto.Inferior)
                faceSelecc = ElementEncontrado.ObtenerCaraInferior();
            else if (_TipoCaraObjeto == TipoCaraObjeto.Superior)
                faceSelecc = ElementEncontrado.ObtenerPLanarFAce_superior();

            if (faceSelecc == null) return false;
            _PtoSObreCaraLosaFund = faceSelecc.ObtenerPtosInterseccionFace(PtoBusqueda, faceSelecc.FaceNormal, true);

           // IntersectionResult interseccionSObreCaraLOsa = faceSelecc.Project(PtoBusqueda);
            if (_PtoSObreCaraLosaFund.IsLargoCero()) return false;
            //_PtoSObreCaraLosaFund = interseccionSObreCaraLOsa.XYZPoint;
            return true;
        }


 
    }
}
