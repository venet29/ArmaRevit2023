using ApiRevit.FILTROS;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Calculos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Niveles.Vigas;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public class SelecionarPtoSup
    {
        public UIApplication _uiapp { get; set; }

        private Document _doc;
        private View _view;
        private UIDocument _uidoc;
        private ConfiguracionIniciaWPFlBarraVerticalDTO _confiEnfierradoDTO;
        public Element _ElemenetoSelectSup { get; set; }
        public XYZ _PtoInicioIntervaloBarra { get; set; }
        public XYZ _PtoFinalIntervaloBarra { get; set; }
        public List<Level> _listaLevel { get; set; }

        //lista con el intervalor de niveles que contiene level selecionado entre '_PtoInicioIntervaloBarra' y '_PtoFinalIntervaloBarra'
        //public List<Level> ListaLevelIntervalo { get; set; }
        public List<double> ListaLevelIntervalo { get; set; }
        public bool IsConError { get; private set; }
        public XYZ _PtoFinalIntervaloBarra_ProyectadoCaraMuroHost { get; set; }
        public XYZ NormalCaraElemento { get; private set; }
        public XYZ _ptoSeleccionMouseCentroCaraMuro { get; set; }
        public XYZ _PtoInicioIntervaloBarra_mallaVertiva { get; internal set; }
        public XYZ _PtoFinalIntervaloBarra_mallaVertiva { get; internal set; }
        public RecalcularPtosYEspaciamieto_Horquilla _RecalcularPtosYEspaciamieto_HORQUILLA { get; internal set; }

        public SelecionarPtoSup(UIApplication _uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO, List<Level> listaLevel)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.ActiveView;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
            if (listaLevel == null)
                this._listaLevel = new List<Level>();
            else
                this._listaLevel = listaLevel;
            this.IsConError = false;
        }

        public SelecionarPtoSup()
        {
        }

        public bool SeleccionarPtoSuperior(XYZ _PtoInicioIntervaloBarra, XYZ _ptoSeleccionMouseCentroCaraMuro)
        {
             NormalCaraElemento = _view.ViewDirection6();
            this._ptoSeleccionMouseCentroCaraMuro = _ptoSeleccionMouseCentroCaraMuro;
            this._PtoInicioIntervaloBarra = _PtoInicioIntervaloBarra;
            if (SeleccionarPtoFin(_ptoSeleccionMouseCentroCaraMuro, new FiltroVIga_Muro_Column_Floor_Rebar())) return false;

            // M1_4_BuscarPtoSuperiorSobreMuro();

            _PtoFinalIntervaloBarra_ProyectadoCaraMuroHost = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(NormalCaraElemento, _ptoSeleccionMouseCentroCaraMuro, _PtoFinalIntervaloBarra);

            ObtenerIntervalLevel();

            return IsConError;
        }
        public void ObtenerIntervalLevel()
        {
            try
            {
                ListaLevelIntervalo = _listaLevel.Where(ll => ll.ProjectElevation > _PtoInicioIntervaloBarra.Z - Util.CmToFoot(50) &&
                                                                ll.ProjectElevation < _PtoFinalIntervaloBarra.Z + Util.CmToFoot(50)).Select(rr => rr.ProjectElevation).OrderBy(nn => nn).ToList();


                //caso a) 1 nivel, superior: con mouse inferior nivel
                if (ListaLevelIntervalo.Count == 1 && (_confiEnfierradoDTO.TipoSeleccionMousePtoSuperior == TipoSeleccionMouse.mouse && _confiEnfierradoDTO.TipoSeleccionMousePtoInferior == TipoSeleccionMouse.nivel))
                {
                    if (ListaLevelIntervalo[0] < _PtoFinalIntervaloBarra.Z)
                    {
                        ListaLevelIntervalo.Add(_PtoFinalIntervaloBarra.Z);
                        return;
                    }
                }
                //caso b) 1 nivel, superior: con nivel , inferior: mouse
                if (ListaLevelIntervalo.Count == 1 && (_confiEnfierradoDTO.TipoSeleccionMousePtoSuperior == TipoSeleccionMouse.nivel && _confiEnfierradoDTO.TipoSeleccionMousePtoInferior == TipoSeleccionMouse.mouse))
                {
                    if (ListaLevelIntervalo[0] > _PtoInicioIntervaloBarra.Z)
                    {
                        double valorIncial = ListaLevelIntervalo[0];
                        ListaLevelIntervalo.Clear();
                        ListaLevelIntervalo.Add(_PtoFinalIntervaloBarra.Z);
                        ListaLevelIntervalo.Add(valorIncial);
                        return;
                    }
                }

                // caso vig invertida no enteindo
                if (ListaLevelIntervalo.Count == 1)
                {
                    ListaLevelIntervalo = NivelVigaInvertida.CasoVigaInvertido(ListaLevelIntervalo, _PtoInicioIntervaloBarra.Z);
                }
                else if (ListaLevelIntervalo.Count == 0)//caso  seleccion vertical dentro de un mismo nivel
                {
                    ListaLevelIntervalo.Add(_PtoInicioIntervaloBarra.Z);
                    ListaLevelIntervalo.Add(_PtoFinalIntervaloBarra.Z);
                }

                if (ListaLevelIntervalo.Count == 0 &&
                    (_confiEnfierradoDTO.TipoSeleccionMousePtoSuperior == TipoSeleccionMouse.mouse &&
                    _confiEnfierradoDTO.TipoSeleccionMousePtoInferior == TipoSeleccionMouse.mouse))
                {
                    ListaLevelIntervalo.Add(_PtoInicioIntervaloBarra.Z);
                    ListaLevelIntervalo.Add(_PtoFinalIntervaloBarra.Z);
                }
            }
            catch (Exception)
            {
                IsConError = true;
                ListaLevelIntervalo.Clear();
            }

        }


        public bool SeleccionarPtoFin(XYZ _ptoSeleccionMouseCentroCaraMuro, ISelectionFilter filtroMuro)
        {
            IsConError = false;
            try
            {
                if (_confiEnfierradoDTO.TipoSelecion == TipoSeleccion.ConElemento)
                {
                    //ISelectionFilter filtroMuro = new FiltroVIga_Muro_Rebar_Columna();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                    Reference ref_pickobject_element = default;
                    _ElemenetoSelectSup = default;

                    bool ISCOntinuar = true;
                    while (ISCOntinuar)
                    {
                         ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.PointOnElement, "Seleccionar Punto Superior Barra:");
                         _ElemenetoSelectSup = _doc.GetElement(ref_pickobject_element);

                        if (filtroMuro.AllowElement(  _ElemenetoSelectSup))
                        {
                            ISCOntinuar = false;
                        }
                        else
                            Util.ErrorMsg($"Selecciono un elemento :{_ElemenetoSelectSup.Category.Name}\n\nSeleccionar elemetos BARRA, VIGA , MURO o COLUMNA");
                    }
           
                    _PtoFinalIntervaloBarra = ref_pickobject_element.GlobalPoint;
             
             
                    if (_ElemenetoSelectSup is Rebar)
                    {
                        Rebar _barra1 = (Rebar)_doc.GetElement(ref_pickobject_element);
                        if (!AyudaObtenerNormarPlanoVisisible.Obtener(_barra1, _view)) return false;
                    }
                    else
                    {
                        if (!AyudaObtenerNormarPlanoVisisible.Obtener(_ElemenetoSelectSup, _view)) return false;
                    }

                    NormalCaraElemento = AyudaObtenerNormarPlanoVisisible.FaceNormal;
                }
                else if (_confiEnfierradoDTO.TipoSelecion == TipoSeleccion.ConMouse)
                {
                    ObjectSnapTypes snapTypes = ObjectSnapTypes.Intersections | ObjectSnapTypes.Endpoints | ObjectSnapTypes.Midpoints;                     
                    _PtoFinalIntervaloBarra = _uidoc.Selection.PickPoint(snapTypes, "Seleccionar Punto Superior Barra");
                }

                if (_PtoInicioIntervaloBarra.Z > _PtoFinalIntervaloBarra.Z)
                {
                    Util.ErrorMsg("Punto inicial no puedes tener coordenada Z mayor que pto final");

                    return IsConError = true;
                }

                _PtoFinalIntervaloBarra = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(NormalCaraElemento, _ptoSeleccionMouseCentroCaraMuro, _PtoFinalIntervaloBarra);
            }
            catch (Exception)
            {

                return IsConError = true;
            }

            return IsConError;
        }


    }
}
