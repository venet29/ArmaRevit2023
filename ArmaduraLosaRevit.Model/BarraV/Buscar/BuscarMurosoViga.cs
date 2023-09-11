using ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GEOM;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarMurosoViga// : GeometriaBase
    {
        private UIApplication uIApplication;
        private Document _doc;
        private View _view;
        private double _largoDeBUsquedaFoot;
        private XYZ _activeViewRightDirection;

        public XYZ PuntoSobreFAceHost { get; private set; }
        public XYZ CentroCaraNormalSaliendoVista { get; set; }

        // public Element WallElementHost { get;  set; }
        public Element WalloVigaElementHost { get; set; }

        public double _espesorMuroFoot { get; private set; }
        public XYZ _OrigenMuro { get; private set; }
        public XYZ _direccionMuro { get; private set; }


        public BuscarMurosoViga(UIApplication uIApplication, double _largoDeBUsquedaFoot) //: base(uIApplication)
        {
            this.uIApplication = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._activeViewRightDirection = this._doc.ActiveView.RightDirection;
        }



        public Element OBtenerRefrenciaMuro(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, bool IsdibujarRayoDebusqueda = false)
        {
            // ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ReferenceIntersector ri = new ReferenceIntersector(M2_1_2_ObtenerFiltroWallViga(), FindReferenceTarget.All, elem3d);
            //   ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            if (IsdibujarRayoDebusqueda)
            {
#if (DEBUG)
                CrearLIneaAux _crearLIneaAux = new CrearLIneaAux(_doc);
                _crearLIneaAux.CrearLinea(PtoCentralBordeRoom, PtoCentralBordeRoom + VectorPerpenBordeRoom * Util.CmToFoot(50));
#endif
            }
            bool IsMuroParaleloView = DatosDiseño.IsMuroParaleloView; ;

            var ref2_ = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom)
                        .Where(x => x.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(x, _doc))
                        .OrderBy(c => c.Proximity).ToList();
            if (ref2_ == null) return null;
            if (ref2_.Count == 0) return null;

            List<ReferenceWithContext> Listref2 = ref2_.ToList();

            foreach (ReferenceWithContext ref2 in Listref2)
            {
                if (ref2 != null)
                {
                    if (ref2.Proximity < _largoDeBUsquedaFoot)
                    {
                        Reference WallRef = ref2.GetReference();
                        PuntoSobreFAceHost = WallRef.GlobalPoint;
                        WalloVigaElementHost = _doc.GetElement(WallRef);

                        if (WalloVigaElementHost is Wall)
                        {
                            // Wall _wall = (Wall)WalloVigaElementHost;

                            if (IsMuroParaleloView)
                            {
                                _direccionMuro = (WalloVigaElementHost as Wall).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                                if (!Util.IsParallel_soloElement(_direccionMuro, _activeViewRightDirection)) continue;
                            }
                            else
                            {
                                _direccionMuro = (WalloVigaElementHost as Wall).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                                if (Util.IsParallel(_direccionMuro, _activeViewRightDirection, 0.1)) continue;
                                _direccionMuro = _activeViewRightDirection;
                            }

                            //---muro buscado hacia el centro de muro
                            _espesorMuroFoot = (WalloVigaElementHost as Wall).ObtenerEspesorMuroFoot(_doc);
                            _OrigenMuro = (WalloVigaElementHost as Wall).ObtenerOrigin();
                            
                            (bool resulta, PlanarFace _planar) = WalloVigaElementHost.ObtenerCaraVerticalVIsible(_view);
                            CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);
                          //  CentroCaraNormalInversaVistaMuro();
                            return WalloVigaElementHost;
                        }
                        else if (WalloVigaElementHost is FamilyInstance)
                        {

                            //---muro buscado hacia el centro de muro
                            _espesorMuroFoot = ((FamilyInstance)WalloVigaElementHost).ObtenerEspesorVigaFoot();
                            _OrigenMuro = ((FamilyInstance)WalloVigaElementHost).ObtenerOrigin();
                            _direccionMuro = ((FamilyInstance)WalloVigaElementHost).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                            return WalloVigaElementHost;
                        }
                        else
                            return null;
                    }
                }
            }
            //   ReferenceWithContext ref2 = null;


            return null;
        }
        private ElementFilter M2_1_2_ObtenerFiltroWallViga()
        {
            ElementCategoryFilter fwall = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementCategoryFilter fbeam = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            LogicalOrFilter filtrosWallViga = new LogicalOrFilter(fwall, fbeam);
            return filtrosWallViga;
        }
        public Element OBtenerRefrenciaMuroOViga(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom)
        {

            ElementCategoryFilter filterMuro = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementCategoryFilter filterViga = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            LogicalOrFilter f3 = new LogicalOrFilter(filterMuro, filterViga);

            ReferenceIntersector ri = new ReferenceIntersector(f3, FindReferenceTarget.Element, elem3d);
          //  ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            ReferenceWithContext ref2 = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom)
                             .Where(cc => cc.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(cc, _doc))
                             .OrderBy(c => c.Proximity).FirstOrDefault();
            if (ref2 != null)
            {
                if (ref2.Proximity < _largoDeBUsquedaFoot)
                {
                    Reference WallRef = ref2.GetReference();
                    PuntoSobreFAceHost = WallRef.GlobalPoint;
                    WalloVigaElementHost = _doc.GetElement(WallRef);

                    return ObtenerTipoElemento(PuntoSobreFAceHost);
                }
            }
            return null;
        }

        private Element ObtenerTipoElemento(XYZ ptoSeleccionMouseCentroCaraMuro6)
        {
            if (WalloVigaElementHost is Wall)
            {
          
                //---muro buscado hacia el centro de muro
                _espesorMuroFoot = (WalloVigaElementHost as Wall).ObtenerEspesorConPtos_foot(ptoSeleccionMouseCentroCaraMuro6, _view.ViewDirection);
                _OrigenMuro =  (WalloVigaElementHost as Wall).ObtenerOrigin();
                _direccionMuro = (WalloVigaElementHost as Wall).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                (bool resulta, PlanarFace _planar) = WalloVigaElementHost.ObtenerCaraVerticalVIsible(_view);
                CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);

                return WalloVigaElementHost;
            }
            else if (WalloVigaElementHost is FamilyInstance)
            {
                var result = (WalloVigaElementHost as FamilyInstance).Symbol;
                //---muro buscado hacia el centro de muro
                _espesorMuroFoot = ((FamilyInstance)WalloVigaElementHost).ObtenerEspesorVigaFoot();
                _OrigenMuro = ((FamilyInstance)WalloVigaElementHost).ObtenerOrigin();
                _direccionMuro = ((FamilyInstance)WalloVigaElementHost).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                //CentroCaraNormalInversaVistaMuro();
                (bool resulta, PlanarFace _planar) = WalloVigaElementHost.ObtenerCaraVerticalVIsible(_view);
                CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);
                return WalloVigaElementHost;
            }
            else
                return null;
        }

        public bool AsignarMuroOviga(Element _wallOvigaElementHost, DatosMuroSeleccionadoDTO _muroSeleccionadoInicialDTO)
        {
            try
            { 
                if (_wallOvigaElementHost == null) return false;
                WalloVigaElementHost = _wallOvigaElementHost;
                ObtenerTipoElemento(_muroSeleccionadoInicialDTO.ptoSeleccionMouseCentroCaraMuro6);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Asignar muro o viga   \n  ex:{ex.Message};");
                return false;
            }
            return true;
        }
        //public void CentroCaraNormalInversaVistaMuro()
        //{
        //    M1_AsignarGeometriaObjecto(WalloVigaElementHost);
        //    XYZ normalViewSaliendo = _doc.ActiveView.ViewDirection;
        //    //List<PlanarFace> plan = listaPlanarFace.Where(c => Util.IsParallel_soloMuro(c.FaceNormal, normalViewSaliendo)).ToList();

        //   // var PlanarVerticalVIsiblNoVerticales = listaPlanarFace.Where(c => !Util.IsVertical(c.FaceNormal)).ToList();
        //   var PlanarVerticalVIsible = listaPlanarFace
        //                                        .OrderByDescending(c => Util.GetProductoEscalar(c.FaceNormal.GetXY0(), normalViewSaliendo.GetXY0()))
        //                                        .FirstOrDefault();
        //    if (PlanarVerticalVIsible == null)
        //    {
        //        CentroCaraNormalInversaVista = XYZ.Zero;
        //        return;
        //    }
        //    BoundingBoxUV bb = PlanarVerticalVIsible.GetBoundingBox();
        //    UV ptocentro = (bb.Max + bb.Min) / 2;
        //    CentroCaraNormalInversaVista = PlanarVerticalVIsible.Evaluate(ptocentro);
        //}



        //public BuscarMurosDTO GetBuscarMurosDTO()
        //{
        //    return new BuscarMurosDTO()
        //    {
        //        _idElement = WalloVigaElementHost.Id.IntegerValue,
        //        _direccionMuro = _direccionMuro,
        //        _espesorMuro = _espesorMuroFoot,
        //        _ptobuscarWall_inferior = CentroCaraNormalInversaVista
        //    };
        //}

        public BuscarMurosDTO GetBuscarMurosDTO(XYZ _ptobuscarWall_inferior, XYZ _ptobuscarWall_superior)
        {
            return new BuscarMurosDTO()
            {
                _idElement = WalloVigaElementHost.Id.IntegerValue,
                _direccionMuro = _direccionMuro,
                _espesorMuro = _espesorMuroFoot,
                _ptobuscarWall_inferior = _ptobuscarWall_inferior,
                _ptobuscarWall_superior = _ptobuscarWall_superior
            };
        }
    }
}
