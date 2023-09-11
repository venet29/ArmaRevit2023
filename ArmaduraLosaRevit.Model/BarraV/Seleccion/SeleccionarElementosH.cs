using ApiRevit.FILTROS;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Shaft.Buscar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{
    internal class ptoseleccionDTO
    {
        public double distancia { get; set; }
        public Line _borde { get; set; }
    }
    public class SeleccionarElementosH
    {
        protected UIApplication _uiapp;
        protected ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO;
        protected View3D _view3D;
        protected DireccionRecorrido _direccionRecorrido;
        protected UIDocument _uidoc;
        protected Document _doc;
        protected View _view;
        protected View _seccionView;

        internal CoordenadasElementoDTO CoordenadasElementoDTO { get; set; }
        internal DatosMuroSeleccionadoDTO VigaSeleccionadoDTO { get; set; }
        //protected XYZ _ptoSeleccionMouseCentroCaraMuro6;
        internal XYZ _PtoInicioBaseBordeViga6 { get; set; }


        public Element Element_pickobject_element { get; set; }
        protected XYZ _origenSeccionView6; //punto mas al derecha, abajo y mas cerca de pantalla a la  vista
        protected XYZ _RightDirection6;//direccion paralalea a la pantalla (izq hacia derecha)
        protected XYZ _ViewDirectionSaliento6;//direccion perpendicular a la pantalla(saliendo de lapantalla)

        public Wall WallSelect { get; set; }

        public XYZ PtoSeleccionMouseCentroCaraMuro6 { get; set; }
        public XYZ PtoSeleccionMouseCaraMuro { get; set; }


        public Element ElemetSelect { get; set; }
        public FamilyInstance VigaSelect { get; set; }
        public Floor FundacionSelecciondo { get; set; }

        public ElementoSeleccionado _elementoSeleccionado { get; set; }

        public XYZ DireccionMuro6 { get; set; }

        public XYZ DireccionNormalFace { get; set; }
        public double EspesorViga { get; set; }

        public double LargoViga { get; set; }

        public XYZ PtoInicioBaseBordeViga_ProyectadoCaraMuroHost { get; set; }
        public XYZ DireccionBordeElemeto { get; set; }
        public Curve CurvaBorde { get; private set; }

        private BuscarElementosArriba _buscarElementosArriba;
        private bool _IsFundacion;

        public SeleccionarElementosH(UIApplication _uiapp, ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO, DireccionRecorrido _DireccionRecorrido)
        {
            this._uiapp = _uiapp;
            this._configuracionInicialBarraHorizontalDTO = _configuracionInicialBarraHorizontalDTO;
            this._view3D = _configuracionInicialBarraHorizontalDTO.view3D_paraBuscar;
            this._direccionRecorrido = _DireccionRecorrido;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
            this._view = _doc.ActiveView;
            this._seccionView = _doc.ActiveView;
            this._IsFundacion = false;
        }

        public SeleccionarElementosH()
        {
        }

        public virtual bool M1_ObtenerPtoinicio_RefuerzoBorde()
        {
            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            if (!M1_2_SeleccionarBordeViga()) return false;
            if (!M1_2a_AnalizarBordeViga()) return false;


            if (!M1_3_SeleccionarVigaElement()) return false;
            if (!M1_3a_RedireccionSegunMurooVIgaSeleccionado()) return false;
            if (!M1_3b_ObtenerDatosVigaElement()) return false;

            if (!M1_4_BuscarPtoInicioBase()) return false;
            return M1_5_BuscarFundacionArriba();
        }


        public bool M1_1_CrearWorkPLane_EnCentroViewSecction()
        {
            _origenSeccionView6 = _seccionView.Origin.Redondear8();
            _RightDirection6 = _seccionView.RightDirection6();
            _ViewDirectionSaliento6 = _seccionView.ViewDirection6();

            if (!CrearWorkPLane.Ejecutar(_doc, _view))
            {
                Util.ErrorMsg($"Error al crear plano de referencia");
                return false;
            }

            //double AnchoView = _seccionView.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).AsDouble();
            //XYZ NuevoOrigen = _origenSeccionView6 + -_ViewDirectionSaliento6 * AnchoView / 2;
            //if (!CrearOAsignarSketchPlane(NuevoOrigen)) return false;

            return true;
        }



        //_PtoInicioBase : solo re ferencia, despues se proyecto en el plano del muro
        public bool M1_2_SeleccionarBordeViga()
        {
            try
            {
                if (_configuracionInicialBarraHorizontalDTO.TipoSelecion == TipoSeleccion.ConElemento ||
                    _configuracionInicialBarraHorizontalDTO.TipoSelecion == TipoSeleccion.ConMouse)
                {
                    ISelectionFilter filtroMuro = new FiltroVIga_Muro_Rebar_Columna();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                    Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.PointOnElement, "Seleccionar Borde Muro:");
                    _PtoInicioBaseBordeViga6 = ref_pickobject_element.GlobalPoint;

                    Element_pickobject_element = _doc.GetElement(ref_pickobject_element.ElementId);
                }
                else if (_configuracionInicialBarraHorizontalDTO.TipoSelecion == TipoSeleccion.ConMouse)
                {
                    ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
                    //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                    _PtoInicioBaseBordeViga6 = _uidoc.Selection.PickPoint(snapTypes, "Seleccionar Borde Viga").Redondear8();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M1_2a_AnalizarBordeViga()
        {
            try
            {
                if (_configuracionInicialBarraHorizontalDTO.TipoSelecion == TipoSeleccion.ConElemento ||
                    _configuracionInicialBarraHorizontalDTO.TipoSelecion == TipoSeleccion.ConMouse)
                {

                    if (Element_pickobject_element is Wall)
                        _elementoSeleccionado = ElementoSeleccionado.Muro;
                    else if (Element_pickobject_element is Opening)
                        _elementoSeleccionado = ElementoSeleccionado.Muro;
                    else if (Element_pickobject_element is FamilyInstance)
                    {
                        FamilyInstance famy = Element_pickobject_element as FamilyInstance;
                        if (famy.StructuralType == StructuralType.Column)
                            _elementoSeleccionado = ElementoSeleccionado.Columna;
                        else
                            _elementoSeleccionado = ElementoSeleccionado.Viga;
                    }
                    else if (Element_pickobject_element is Rebar)
                        _elementoSeleccionado = ElementoSeleccionado.Barra;
                    else if (Element_pickobject_element is Floor)
                        _elementoSeleccionado = ElementoSeleccionado.Losa;
                    else
                    {
                        var tipo = Element_pickobject_element.GetType();
                        Util.ErrorMsg($"Error al seleccionar borde de viga.\nElemento seleccionado  tipo:{Element_pickobject_element.Category.Name}");
                        return false;
                    }

                    if (!ObtenerDireccionDeBarra(Element_pickobject_element)) return false;

                }
                else if (_configuracionInicialBarraHorizontalDTO.TipoSelecion == TipoSeleccion.ConMouse)
                {
                    ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
                    //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                    _PtoInicioBaseBordeViga6 = _uidoc.Selection.PickPoint(snapTypes, "Seleccionar Borde Viga").Redondear8();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }




        private bool ObtenerDireccionDeBarra(Element Element_pickobject_element)
        {
            if (Element_pickobject_element is Wall || Element_pickobject_element is FamilyInstance)
            {
                ObtenerDIreccionDelBorde(Element_pickobject_element);
            }
            else if (Element_pickobject_element is Floor)
            {
                ObtenerDIreccionDelBorde(Element_pickobject_element);
            }
            else if (Element_pickobject_element is Rebar)
            {
                RebarDesglose _RebarDesglose = new RebarDesglose(_uiapp, Element_pickobject_element as Rebar);
                _RebarDesglose.Ejecutar();
                DireccionBordeElemeto = _RebarDesglose.CurvaMasLargo.direccion;
                CurvaBorde = _RebarDesglose.CurvaMasLargo._curve;
            }
            else if (Element_pickobject_element is Opening)
            {
                BuscarBordeMAScercanaView _BuscarCaraMAScercanaView = new BuscarBordeMAScercanaView(_uiapp, (Opening)Element_pickobject_element);
                if (!_BuscarCaraMAScercanaView.M2_ObtenerBordeConcurvas(_PtoInicioBaseBordeViga6)) return false;

                DireccionBordeElemeto = _BuscarCaraMAScercanaView.Direccion;
                CurvaBorde = _BuscarCaraMAScercanaView.CurvaBorde;
            }
            return true;
        }

        private void ObtenerDIreccionDelBorde(Element Element_pickobject_element)
        {
            //version nuevo para generaba error en algunos caso   28-02-2023
            try
            {
                var lsitaCurva = Element_pickobject_element.ObtenerListaVertice()
                                                           .SelectMany(c => c)
                                                           .Where(c => (c is Line) && _view.NH_IsEndireccionRightDirection(((Line)c).Direction)).ToList();

                var listaAny = lsitaCurva.Where(c => c.Length > Util.CmToFoot(5)).Select(c => new ptoseleccionDTO()
                {
                    distancia = (_PtoInicioBaseBordeViga6 != null ? ((Line)c).ProjectExtendida3D(_PtoInicioBaseBordeViga6).DistanceTo(_PtoInicioBaseBordeViga6) : ((Line)c).Length),
                    _borde = (Line)c
                }).OrderBy(c => c.distancia).ToList();

                DireccionBordeElemeto = listaAny[0]._borde.Direction;
                CurvaBorde = listaAny[0]._borde;
            }
            catch (Exception ex)
            {
                DireccionBordeElemeto = _view.RightDirection.Redondear(6);
                Util.ErrorMsg($"Error al seleccionar el borde de elementos 'ObtenerDIreccionDelBorde'. ex:{ex.Message}");
            }
        }

        private bool OBtenerDireccionHorizontalDeCaraSeleccionadaMuro(Element Element_pickobject_element)
        {
            try
            {
                var facemaScERCANO = Element_pickobject_element.ObtenerfaceMAsCercanaConDireccion_foot(_PtoInicioBaseBordeViga6, _view.ViewDirection);

                var DireccionCAraMuro = Util.CrossProduct(facemaScERCANO.FaceNormal, new XYZ(0, 0, 1));
                if (DireccionCAraMuro == null) return false;
                if (DireccionCAraMuro.IsAlmostEqualTo(XYZ.Zero)) return false;

                DireccionBordeElemeto = DireccionCAraMuro;
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public bool M1_3_SeleccionarVigaElement()
        {
            try
            {
                ICollection<BuiltInCategory> categories = new[] {

                                                                  BuiltInCategory.OST_StructuralFraming,          BuiltInCategory.OST_StructuralFoundation,
                                                                };
                ElementFilter catFilter = new ElementMulticategoryFilter(categories);
                // ISelectionFilter filtroViga = new FiltroViga();
                ISelectionFilter filtroViga = SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance)).Or(SelFilter.GetElementFilter(catFilter));

                Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Element, filtroViga, "Seleccionar Cara de Viga:");
                PtoSeleccionMouseCentroCaraMuro6 = ref_pickobject_element.GlobalPoint.Redondear8();

                ElemetSelect = _doc.GetElement(ref_pickobject_element);
                var result = ElemetSelect.IsEstructural();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private bool M1_3a_RedireccionSegunMurooVIgaSeleccionado()
        {
            try
            {
                var facemaScERCANO = ElemetSelect.ObtenerfaceMAsCercanaConDireccion_foot(PtoSeleccionMouseCentroCaraMuro6, _view.ViewDirection);

                if (facemaScERCANO == null)
                {
                    Util.ErrorMsg($"Error recalcular direccion del borde  con elemento seleccionado, se mantiene direccion del borde inicial.\nNOTA: Si borde seleccionado es de losa, puede generar barras con forma incorrecta.");
                    return false;
                }
                else
                {
                    XYZ P1 = CurvaBorde.GetEndPoint(0);
                    XYZ P2 = CurvaBorde.GetEndPoint(1);

                    XYZ P1_inter = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(facemaScERCANO.FaceNormal, PtoSeleccionMouseCentroCaraMuro6, P1);
                    XYZ P2_inter = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(facemaScERCANO.FaceNormal, PtoSeleccionMouseCentroCaraMuro6, P2);

                    Line auxLine = Line.CreateBound(P1_inter, P2_inter);
                    DireccionBordeElemeto = auxLine.Direction;
                    CurvaBorde = auxLine;

                    //CurvaBorde = listaAny[0]._borde;


                    DireccionBordeElemeto = (P2_inter - P1_inter).Normalize();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool M1_3b_ObtenerDatosVigaElement(Element _ElemetSelectIngresado = null)
        {
            try
            {
                if (_ElemetSelectIngresado != null)
                    ElemetSelect = _ElemetSelectIngresado;

                if (ElemetSelect is Wall)
                {
                    WallSelect = (Wall)ElemetSelect;
                    DireccionMuro6 = ((Line)((LocationCurve)(WallSelect.Location)).Curve).Direction.Redondear8();
                    EspesorViga = WallSelect.ObtenerEspesorMuroFoot(_doc);// tipoFamiliaMuro.Width;
                    LargoViga = WallSelect.ObtenerLargo();
                }
                else if (ElemetSelect is FamilyInstance)
                {
                    VigaSelect = (FamilyInstance)ElemetSelect;
                    if (VigaSelect.StructuralType == StructuralType.Beam)
                    {
                        DireccionMuro6 = ((Line)((LocationCurve)(VigaSelect.Location)).Curve).Direction.Redondear8();
                        EspesorViga = VigaSelect.ObtenerEspesorConPtos_foot(PtoSeleccionMouseCentroCaraMuro6, _view.ViewDirection);// tipoFamiliaMuro.Width;
                        LargoViga = VigaSelect.ObtenerLargo(); //ObtenerAnchoConPtos(_ptoSeleccionMouseCentroCaraMuro6, _view.ViewDirection);
                    }
                    else
                    {
                        Util.ErrorMsg($"FamilyInstance tipo {VigaSelect.StructuralType} no esta implementada. ");
                        return false;
                    }
                }
                else
                {
                    Util.ErrorMsg("Elemento Seleccionado debe ser Muro o viga(FamilyInstance)");
                    return false;
                }

                (bool reult, PlanarFace caraVisibleVErtical) = ElemetSelect.ObtenerCaraVerticalVIsible(_view);
                if (!reult)
                {
                    Util.ErrorMsg("Error al obtener cara visible vertical de elemento.");
                    return false;
                }

                DireccionNormalFace = -caraVisibleVErtical.FaceNormal;
                _direccionRecorrido.DireccionNormalFace = DireccionNormalFace.Redondear8();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }


        //Obs1)
        public virtual bool M1_4_BuscarPtoInicioBase()
        {
            try
            {


                //utilizar plano de cara seleccionada( puede q sea viga o muro inclinado) o viewdirecction de vista
                XYZ NormalPlanoENfierrado = (_direccionRecorrido.DireccionNormalFace.IsAlmostEqualTo(XYZ.Zero) ? _seccionView.ViewDirection6() : _direccionRecorrido.DireccionNormalFace);

                Plane plano = Plane.CreateByNormalAndOrigin(NormalPlanoENfierrado, PtoSeleccionMouseCentroCaraMuro6);
                PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = plano.ProjectOntoXY(_PtoInicioBaseBordeViga6).Redondear8();

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        public bool M1_5_BuscarFundacionArriba()
        {
            try
            {
                if (_seccionView == null) return false;
                if (PtoSeleccionMouseCentroCaraMuro6 == null) return false;

                //Plane plano = Plane.CreateByNormalAndOrigin(_seccionView.ViewDirection6(), PtoSeleccionMouseCentroCaraMuro6);
                //if (plano == null) return false;
                //PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = plano.ProjectOnto(_PtoInicioBaseBordeViga6).Redondear8();
                // CrearModeLineAyuda.modelarlineas(_doc, PtoInicioBaseBordeViga_ProyectadoCaraMuroHost, PtoInicioBaseBordeViga_ProyectadoCaraMuroHost + XYZ.BasisZ * 1);
                _buscarElementosArriba = new BuscarElementosArriba(_uiapp, Util.CmToFoot(3), _view3D);
                _buscarElementosArriba.BuscarObjetosEntrayectoriaDeBArra(PtoInicioBaseBordeViga_ProyectadoCaraMuroHost - new XYZ(0, 0, Util.CmToFoot(1)), new XYZ(0, 0, 1), XYZ.Zero);
                M2_1_BuscarSiTerminaFundaciones();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al buscar fundaciones hacia arriba ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private bool M2_1_BuscarSiTerminaFundaciones()
        {
            var fundacionesEncontrado = _buscarElementosArriba.ListaObjEncontrados.Where(m => m.nombreTipo == TipoElementoBArraV.fundacion).FirstOrDefault();
            if (fundacionesEncontrado == null) return false;

            _IsFundacion = true;
            return true;
        }

        public DatosMuroSeleccionadoDTO M2_OBtenerElementoREferenciaDTO()
        {
            //  ptoRefereMuro - _PtoInicioSobrePLanodelMuro == _seccionView.RightDirection  -- enfieraa hacia derecha
            //  ptoRefereMuro - _PtoInicioSobrePLanodelMuro <> _seccionView.RightDirection  -- enfieraa hacia izquierda
            XYZ direccionEnfierrrar_aux6 = (PtoSeleccionMouseCentroCaraMuro6.Z > _PtoInicioBaseBordeViga6.Z ? new XYZ(0, 0, 1) : new XYZ(0, 0, -1));

            DatosMuroSeleccionadoDTO muroSeleccionadoDTO = new DatosMuroSeleccionadoDTO()
            {
                PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = PtoInicioBaseBordeViga_ProyectadoCaraMuroHost,
                ptoSeleccionMouseCentroCaraMuro6 = PtoSeleccionMouseCentroCaraMuro6,
                DireccionEnFierrado = direccionEnfierrrar_aux6.Normalize(),
                DireccionMuro = DireccionMuro6.Normalize(),
                NormalEntradoView = -_seccionView.ViewDirection.Normalize(),
                DireccionRecorridoBarra = Util.CrossProduct(XYZ.BasisZ, DireccionMuro6.Normalize()),//  _direccionRecorrido.ObtenerDireccionRecorridoBarra6(),// - _seccionView.ViewDirection.Normalize(),
                DireccionLineaBarra = _direccionRecorrido.ObtenerDireccionLineaBarra(direccionEnfierrrar_aux6),// - _seccionView.ViewDirection.Normalize(),
                EspesorElemetoHost = EspesorViga,
                DireccionPataEnFierrado = _direccionRecorrido.ObtenerDireccionPataEnFierrado(direccionEnfierrrar_aux6),
                LargoElemetoHost = LargoViga,
                IdelementoContenedor = ElemetSelect.Id,
                elementoContenedor = ElemetSelect,
                soloTag1 = true,
                IsFundacion = _IsFundacion,
                direccionBordeElemeto = DireccionBordeElemeto,
                TipoElementoSeleccionado = _elementoSeleccionado

            };
            return muroSeleccionadoDTO;
        }

    }
}
