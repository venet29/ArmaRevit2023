using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Buscar
{
    public class BuscarLosaShaft
    {
        private UIApplication uIApplication;
        private Document _doc;
        private double _largoDeBUsquedaFoot;
        private readonly View3D _elem3D;

        public XYZ PuntoSobreFAceHost { get; private set; }
        public Element FundLosaElementHost { get; private set; }
        public XYZ _PtoSObreCaraInferiorFund { get; private set; }

        public List<BuscarShaftLosaDTO> ListaShaftLosaEncontrado { get; set; }
        public BuscarLosaShaft(UIApplication uIApplication, double _largoDeBUsquedaFoot, View3D elem3d)
        {
            this.uIApplication = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._elem3D = elem3d;
            this.ListaShaftLosaEncontrado = new List<BuscarShaftLosaDTO>();
        }

        public bool OBtenerRefrenciaLosaSahft(XYZ PtoBusqueda, XYZ VectorBusqueda)
        {
            ElementCategoryFilter filterLosa = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            ElementCategoryFilter filterFund = new ElementCategoryFilter(BuiltInCategory.OST_ShaftOpening);

            LogicalOrFilter f1 = new LogicalOrFilter(filterLosa, filterFund);

            ReferenceIntersector ri = new ReferenceIntersector(f1, FindReferenceTarget.All, _elem3D);
            // ReferenceWithContext ref2 = ri.FindNearest(PtoBusqueda, VectorBusqueda);
            var listaObjetos = ri.Find(PtoBusqueda, VectorBusqueda).Where(x => x.Proximity < _largoDeBUsquedaFoot).OrderBy(c => c.Proximity);

            foreach (var ref2 in listaObjetos)
            {
                if (ref2 != null)
                {
                    if (ref2.Proximity < _largoDeBUsquedaFoot)
                    {
                        Reference FundRef = ref2.GetReference();
                        PuntoSobreFAceHost = FundRef.GlobalPoint;
                        FundLosaElementHost = _doc.GetElement(FundRef);

                        if (FundLosaElementHost.Category.Name == "Shaft Openings")
                        {
                            if (!FundLosaElementHost.IsEstructural()) return false;
                            ListaShaftLosaEncontrado.Add(new BuscarShaftLosaDTO()
                            {
                                distancia = ref2.Proximity,
                                _PtoSObreFaceLosaShaft = PuntoSobreFAceHost,
                                _TipoElementoBArraV = TipoElementoBArraV.fundacion
                            });

                        }
                        else if (FundLosaElementHost.Category.Name == "Floors")
                        {
                            if (!FundLosaElementHost.IsEstructural()) return false;
                            ListaShaftLosaEncontrado.Add(new BuscarShaftLosaDTO()
                            {
                                distancia = ref2.Proximity,
                                _PtoSObreFaceLosaShaft = PuntoSobreFAceHost,
                                _TipoElementoBArraV = TipoElementoBArraV.losa
                            });

                        }
                    }
                }
            }

            return (ListaShaftLosaEncontrado.Count == 0 ? false : true);
        }

    }
}
