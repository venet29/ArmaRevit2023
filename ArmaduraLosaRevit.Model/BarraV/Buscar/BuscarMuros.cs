using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Buscar.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Buscar.BuscarBarrarCabezaMuro;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.GEOM;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Buscar
{
    public class BuscarMuros //: GeometriaBase
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

        public double _espesorMuro { get; private set; }
        public XYZ _OrigenMuro { get; private set; }
        public XYZ _direccionMuro { get; private set; }
        public List<Buscar_elementoEncontradoMuroDTO> _listaMuroEcontradoEncontrado { get; set; }

        public BuscarMurosEstado BuscarMurosEstado_ { get; set; }
        public BuscarMuros(UIApplication uIApplication, double _largoDeBUsquedaFoot) //: base(uIApplication)
        {
            this.uIApplication = uIApplication;
            this._doc = uIApplication.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._largoDeBUsquedaFoot = _largoDeBUsquedaFoot;
            this._activeViewRightDirection = this._doc.ActiveView.RightDirection;
            _listaMuroEcontradoEncontrado = new List<Buscar_elementoEncontradoMuroDTO>();
            BuscarMurosEstado_ = BuscarMurosEstado.Inicial;
        }

        public void AsignarMuro(Element _wallElementHost)
        {
            WalloVigaElementHost = _wallElementHost;
            _espesorMuro = (WalloVigaElementHost as Wall).ObtenerEspesorMuroFoot(_doc);
            _OrigenMuro = (WalloVigaElementHost as Wall).ObtenerOrigin();
            _direccionMuro = (WalloVigaElementHost as Wall).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
            _listaMuroEcontradoEncontrado = new List<Buscar_elementoEncontradoMuroDTO>();
            BuscarMurosEstado_ = BuscarMurosEstado.Inicial;
            //PuntoSobreFAceHost = _muroSeleccionadoInicialDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;
        }

        public (Element wallSeleccionado, double espesor, XYZ ptoSobreMuro) OBtenerRefrenciaMuro(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, List<ElementId> ListaExcluair = null, bool IsdibujarRayoDebusqueda = false)
        {
            try
            {
                if (ListaExcluair == null) ListaExcluair = new List<ElementId>();


                bool IsMuroParaleloView = DatosDiseño.IsMuroParaleloView;
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
                ElementCategoryFilter filterColumn = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
                LogicalOrFilter filtro_o = new LogicalOrFilter(filter, filterColumn);
                ReferenceIntersector ri = new ReferenceIntersector(filtro_o, FindReferenceTarget.All, elem3d);

                if (IsdibujarRayoDebusqueda)
                {
#if (DEBUG)
                    CrearLIneaAux _crearLIneaAux = new CrearLIneaAux(_doc);
                    _crearLIneaAux.CrearLinea(PtoCentralBordeRoom, PtoCentralBordeRoom + VectorPerpenBordeRoom * _largoDeBUsquedaFoot);
#endif
                }

                BuscarMurosEstado_ = BuscarMurosEstado.MuroNoEncontrado;

                var ref2cc_ = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom).ToList();
                var ref2_ = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom)
                            .Where(x => x.Proximity < _largoDeBUsquedaFoot && (!ListaExcluair.Contains(x.GetReference().ElementId)) && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(x, _doc))
                            .OrderBy(c => c.Proximity).ToList();
                if (ref2_ == null) return (null, 0, XYZ.Zero);
                if (ref2_.Count == 0) return (null, 0, XYZ.Zero);

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
                                if (!WalloVigaElementHost.IsEstructural()) continue;

                                if (IsMuroParaleloView)
                                {
                                    _direccionMuro = (WalloVigaElementHost as Wall).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                                    if (!Util.IsParallel(_direccionMuro, _activeViewRightDirection, 0.1)) continue;
                                }
                                else
                                {
                                    _direccionMuro = (WalloVigaElementHost as Wall).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                                    if (Util.IsParallel(_direccionMuro, _activeViewRightDirection, 0.1)) continue;
                                    _direccionMuro = _activeViewRightDirection;
                                }
                                
                                //---muro buscado hacia el centro de muro
                                _espesorMuro = (WalloVigaElementHost as Wall).ObtenerEspesorMuroFoot(_doc);
                                _OrigenMuro = (WalloVigaElementHost as Wall).ObtenerOrigin();
                               
                                (bool resulta, PlanarFace _planar) = WalloVigaElementHost.ObtenerCaraVerticalVIsible(_view);
                                CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);
                                //CentroCaraNormalInversaVistaMuro();
                                BuscarMurosEstado_ = BuscarMurosEstado.MuroEncontrado;
                                return (WalloVigaElementHost, _espesorMuro, PuntoSobreFAceHost);
                            }
                            else if (WalloVigaElementHost.Category.Name == "Structural Columns")
                            {
                                FamilyInstance _familyInstance = ((FamilyInstance)WalloVigaElementHost);
                                if (!WalloVigaElementHost.IsEstructural()) continue;
                                _direccionMuro = _view.RightDirection;


                                _espesorMuro = _familyInstance.ObtenerEspesorConPtos_foot(PuntoSobreFAceHost, _view.ViewDirection);
                                _OrigenMuro = _familyInstance.ObtenerOrigin();


                                BuscarMurosEstado_ = BuscarMurosEstado.MuroEncontrado;
                                return (WalloVigaElementHost, _espesorMuro, PuntoSobreFAceHost);
                            }
                            else
                                return (null, 0, XYZ.Zero);
                        }
                    }
                }

                if (Listref2.Count > 0)
                    BuscarMurosEstado_ = BuscarMurosEstado.MurosEncontradoConerror;

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al buscar Muro en el pto: ({PtoCentralBordeRoom.REdondearString_foot(1)})  en la direccion pto:{VectorPerpenBordeRoom.REdondearString_foot(1)} \n ex:{ex.Message}");
            }
            return (null, 0, XYZ.Zero);
        }



        public bool OBtenerBuscarMuroPerpendicular(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, List<ElementId> ListaExcluair = null, bool IsdibujarRayoDebusqueda = false)
        {
            if (ListaExcluair == null) ListaExcluair = new List<ElementId>();

            if (OBtenerListaRefrenciaMuro(elem3d, PtoCentralBordeRoom, VectorPerpenBordeRoom, ListaExcluair))
            {

                var lista = _listaMuroEcontradoEncontrado.Where(c => c._TipoElementoBArraV == TipoElementoBArraV.muroPerpeView).ToList();
                if (lista.Count == 0) return false;

                return true;
            }

            return false;
        }

        public bool OBtenerListaRefrenciaMuro(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom, List<ElementId> ListaExcluair = null, bool IsdibujarRayoDebusqueda = false)
        {
            try
            {
                if (ListaExcluair == null) ListaExcluair = new List<ElementId>();

                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
                ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.All, elem3d);

                if (IsdibujarRayoDebusqueda)
                {
#if (DEBUG)
                    CrearLIneaAux _crearLIneaAux = new CrearLIneaAux(_doc);
                    _crearLIneaAux.CrearLinea(PtoCentralBordeRoom, PtoCentralBordeRoom + VectorPerpenBordeRoom * Util.CmToFoot(50));
#endif
                }


                var ref2_ = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom)
                            .Where(x => x.Proximity < _largoDeBUsquedaFoot &&
                                        (!ListaExcluair.Contains(x.GetReference().ElementId)) &&
                                        AyudaFiltrarEstructural.SiEsWalloLosaEstructural(x, _doc))
                            .OrderBy(c => c.Proximity).ToList();
                if (ref2_ == null) return false;
                if (ref2_.Count == 0) return false;

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
                                if (!WalloVigaElementHost.IsEstructural()) continue;
                                // Wall _wall = (Wall)WalloVigaElementHost;
                                _direccionMuro = (WalloVigaElementHost as Wall).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                                //if (!Util.IsParallel_soloMuro(_direccionMuro, _activeViewRightDirection)) continue;
                                //---muro buscado hacia el centro de muro
                                _espesorMuro = (WalloVigaElementHost as Wall).ObtenerEspesorMuroFoot(_doc);
                                _OrigenMuro = (WalloVigaElementHost as Wall).ObtenerOrigin();

                                (bool resulta, PlanarFace _planar) = WalloVigaElementHost.ObtenerCaraVerticalVIsible(_view);
                                CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);

                               // CentroCaraNormalInversaVistaMuro();

                                _listaMuroEcontradoEncontrado.Add(new Buscar_elementoEncontradoMuroDTO()
                                {
                                    distancia = ref2.Proximity,
                                    _TipoElementoBArraV = (Util.IsParallel_soloElement(_direccionMuro, _activeViewRightDirection) ? TipoElementoBArraV.muro : TipoElementoBArraV.muroPerpeView),
                                    _DireccionMuro = _direccionMuro,
                                    _EspesorMuro = _espesorMuro,
                                    _OrigenMuro = _OrigenMuro,
                                    _CentroCaraNormalInversaVista = CentroCaraNormalSaliendoVista
                                });

                            }

                        }
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                Util.ErrorMsg($"Error al buscar Muro en el pto:{PtoCentralBordeRoom.REdondearString_foot(1)}  en la direccion pto:{VectorPerpenBordeRoom.REdondearString_foot(1)}");
            }
            return (_listaMuroEcontradoEncontrado.Count == 0 ? false : true);
        }


        public Element OBtenerRefrenciaMuroOViga_paraAuto(View3D elem3d, XYZ PtoCentralBordeRoom, XYZ VectorPerpenBordeRoom)
        {

            ElementCategoryFilter filterMuro = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementCategoryFilter filterViga = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            LogicalOrFilter f3 = new LogicalOrFilter(filterMuro, filterViga);

            ReferenceIntersector ri = new ReferenceIntersector(f3, FindReferenceTarget.Face, elem3d);
            //  ReferenceWithContext ref2 = ri.FindNearest(PtoCentralBordeRoom, VectorPerpenBordeRoom);

            bool IsDibujar = true;
            if (IsDibujar)
                CrearModeLineAyuda.modelarlineas(_doc, PtoCentralBordeRoom, PtoCentralBordeRoom +VectorPerpenBordeRoom * Util.CmToFoot(50));



            var ListaRef2aux = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom)
                                .OrderBy(c => c.Proximity).ToList();


            var ListaRef2 = ri.Find(PtoCentralBordeRoom, VectorPerpenBordeRoom)
                                   .Where(cc => cc.Proximity < _largoDeBUsquedaFoot && AyudaFiltrarEstructural.SiEsWalloLosaEstructural(cc, _doc))
                                   .OrderBy(c => c.Proximity).ToList();

            ReferenceWithContext ref2 = ListaRef2.FirstOrDefault();

            if (ref2 != null)
            {
                if (ref2.Proximity < _largoDeBUsquedaFoot)
                {
                    Reference WallRef = ref2.GetReference();
                    PuntoSobreFAceHost = WallRef.GlobalPoint;
                    WalloVigaElementHost = _doc.GetElement(WallRef);
                    if (WalloVigaElementHost is Wall)
                    {
               

                        if (!WalloVigaElementHost.IsEstructural()) return null;
                        //---muro buscado hacia el centro de muro
                        _espesorMuro = (WalloVigaElementHost as Wall).ObtenerEspesorMuroFoot(_doc);
                        _OrigenMuro = (WalloVigaElementHost as Wall).ObtenerOrigin();
                        _direccionMuro = (WalloVigaElementHost as Wall).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                       // CentroCaraNormalInversaVistaMuro();
                        (bool resulta, PlanarFace _planar) = WalloVigaElementHost.ObtenerCaraVerticalVIsible(_view);
                        CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);

                        return WalloVigaElementHost;
                    }
                    else if (WalloVigaElementHost is FamilyInstance)
                    {

                        var _vigaSelect = (FamilyInstance)WalloVigaElementHost;
                        if (_vigaSelect.StructuralType == StructuralType.Beam)
                        {
                            //---muro buscado hacia el centro de muro
                            _espesorMuro = ((FamilyInstance)WalloVigaElementHost).ObtenerEspesorVigaFoot();
                            _OrigenMuro = ((FamilyInstance)WalloVigaElementHost).ObtenerOrigin();
                            _direccionMuro = ((FamilyInstance)WalloVigaElementHost).ObtenerDireccionEnElSentidoView(_activeViewRightDirection);
                            // CentroCaraNormalInversaVistaMuro();
                            (bool resulta, PlanarFace _planar) = WalloVigaElementHost.ObtenerCaraVerticalVIsible(_view);
                            CentroCaraNormalSaliendoVista = (resulta ? _planar.ObtenerCenterDeCara() : XYZ.Zero);
                        }
                        else
                        {
                            Util.ErrorMsg($"FamilyInstance tipo {_vigaSelect.StructuralType} no esta implementada. ");
                            return null;
                        }
                        return WalloVigaElementHost;
                    }
                    else
                        return null;
                }
            }
            return null;
        }
        public BuscarMurosDTO GetBuscarMurosDTO(XYZ _ptobuscarWall_inferior, XYZ _ptobuscarWall_superior)
        {
            if (WalloVigaElementHost == null) return new BuscarMurosDTO() { };
            return new BuscarMurosDTO()
            {
                _idElement = WalloVigaElementHost.Id.IntegerValue,
                _direccionMuro = _direccionMuro,
                _espesorMuro = _espesorMuro,
                _ptobuscarWall_inferior = _ptobuscarWall_inferior,
                _ptobuscarWall_superior = _ptobuscarWall_superior
            };
        }


        //public void CentroCaraNormalInversaVistaMuro()
        //{
        //    //bool resulta = false;
        //    //PlanarFace _planar = default;
        //    (bool resulta, PlanarFace _planar)= WalloVigaElementHost.ObtenerCaraVerticalVIsible(_view);

        //     CentroCaraNormalInversaVista= (resulta?_planar.ObtenerCenterDeCara(): XYZ.Zero);


        //    M1_AsignarGeometriaObjecto(WalloVigaElementHost);

        //    XYZ normalViewSaliendo = _doc.ActiveView.ViewDirection;

        //    List<PlanarFace> plan = listaPlanarFace.Where(c => Util.IsParallel_soloElement(c.FaceNormal, normalViewSaliendo)).ToList();
        //    if (plan == null)
        //    {
        //        CentroCaraNormalInversaVista = XYZ.Zero;
        //        return;
        //    }

        //    if (plan.Count == 0) // si no encuentra planar face completamene con la normal paralela
        //    {
        //        // obtiene lista y deja al inicial la mas paralela al view
        //        var listaPLanarfase = listaPlanarFace.Select(c => new PlanarFaceBuscarMuro(c, normalViewSaliendo, c.FaceNormal.CrossProduct(normalViewSaliendo)
        //            .GetLength()))
        //            .OrderBy(c => c.productoCruz).ToList();

        //        if (listaPLanarfase.Count > 0)
        //        {
        //            (CentroCaraNormalInversaVista, resulta) = listaPLanarfase[0].PlanarFace_.ObtenerCentroCara();
        //        }
        //        else
        //            CentroCaraNormalInversaVista = XYZ.Zero;
        //        return;
        //    }


        //    (CentroCaraNormalInversaVista, resulta) = plan[0].ObtenerCentroCara(); return;

        //}

        //private (XYZ,bool) ObtenerCentroCaraNormalInversaVista(PlanarFace plan)
        //{
        //    if(plan==null) return (XYZ.Zero,false);
        //    BoundingBoxUV bb = plan.GetBoundingBox();
        //    UV ptocentro = (bb.Max + bb.Min) / 2;
        //    CentroCaraNormalInversaVista = plan.Evaluate(ptocentro);
        //    return (plan.Evaluate(ptocentro), true);
        //}

        //public BuscarMurosDTO GetBuscarMurosDTO()
        //{
        //    return new BuscarMurosDTO()
        //    {
        //        _idElement = WalloVigaElementHost.Id.IntegerValue,
        //        _direccionMuro = _direccionMuro,
        //        _espesorMuro = _espesorMuro,
        //        _ptobuscarWall_inferior = CentroCaraNormalInversaVista
        //    };
        //}


    }



    //public class PlanarFaceBuscarMuro
    //{

    //    public PlanarFaceBuscarMuro(PlanarFace _PlanarFace, XYZ normalViewSaliendo, double _productoCruz)
    //    {
    //        this.PlanarFace_ = _PlanarFace;
    //        this.normal = normalViewSaliendo;
    //        this.productoCruz = _productoCruz;
    //    }

    //    public double productoCruz { get; set; }
    //    public XYZ normal { get; set; }
    //    public PlanarFace PlanarFace_ { get; set; }
    //}

}
