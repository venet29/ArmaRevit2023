using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos
{

    public class LineasDeSeleccion
    {
        public Element LineaSeleccionada { get; set; }

        public XYZ Pt1 { get; set; }
        public XYZ Pt2 { get; set; }
        public XYZ PtoSeleccion { get;  set; }
    }

    public class CalculoGeometriaRectangulo2LineasViga
    {
        private UIApplication _uiapp;
        private readonly View3D _elem3D_Parabusacr;
        private readonly TipoRefuerzoLOSA _tipoRefuerzoLOSA;
        private XYZ DireccionHaciaLosa;
        private UIDocument _uidoc;
        private Document _doc;
        public Floor _losaSelecionado { get; set; }

        private XYZ ptoIntersecionFloor;

        public List<XYZ> ListaBorde1 { get; set; }
        public List<XYZ> ListaBorde2 { get; set; }
        private XYZ _PtoMouseEspejo1;
        private XYZ _PtoMouseEspejo2;
        private double AnguloDireccionenfierrado;
        private LineasDeSeleccion _ModelLine1;
    
        private LineasDeSeleccion _ModelLine2;
        private XYZ _ptoModelLine2;
        private View _view;

        public CalculoGeometriaRectangulo2LineasViga(UIApplication uiapp, View3D elem3d_parabusacr, TipoRefuerzoLOSA _tipoRefuerzoLOSA)
        {
            this._uiapp = uiapp;
            this._elem3D_Parabusacr = elem3d_parabusacr;
            this._tipoRefuerzoLOSA = _tipoRefuerzoLOSA;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
            this._view = _doc.ActiveView;
            ListaBorde1 = new List<XYZ>();
            ListaBorde2 = new List<XYZ>();

            _ModelLine1 = new LineasDeSeleccion();
            _ModelLine2 = new LineasDeSeleccion();

        }

        public bool Ejecutar()
        {
            if (!SeleciconarLineas1()) return false;
            if (!SeleciconarLineas2()) return false;

            if (!ObtenerLosa()) return false;

            GenerarListaConBordesDelRectanguloV2();
            PtoMouseSobreMuroFalso();

            return true;
        }

        private bool ObtenerLosa()
        {
            XYZ PtoBUscarLosa = (_ModelLine1.PtoSeleccion + _ModelLine2.PtoSeleccion) / 2 + new XYZ(0, 0, 3);

            SeleccionarLosaOFunda _SeleccionarLosaOFunda = new SeleccionarLosaOFunda(_uiapp, _elem3D_Parabusacr, _tipoRefuerzoLOSA);

            if (!_SeleccionarLosaOFunda.SeleccionarElementoFLoor_Fundaction(PtoBUscarLosa)) return false;
            _losaSelecionado = _SeleccionarLosaOFunda.LosaSelecionado;
            ptoIntersecionFloor = _SeleccionarLosaOFunda.PuntoSobreFAceIntersectada;
            //SeleccionarLosaConPto seleccionarLosaConPto = new SeleccionarLosaConPto(_doc);
            ////OTRA ALTERNATIVA SELECIONAR LOSA CON PUNTO
            //Level lv = _view.GenLevel;
            //if (lv == null) return false;
            //_losaSelecionado = seleccionarLosaConPto.EjecturaSeleccionarLosaConPto(PtoBUscarLosa, lv);
            if (_losaSelecionado == null) return false;
            return true;
        }

        private bool SeleciconarLineas1()

        {
            try
            {
                ISelectionFilter f = new LineSelectionFilte();
                Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Element, "Seleccionar RoomLine 1:");


                if (ref_pickobject_element == null) return false;
                //obtiene una referencia floor con la referencia r
                Element Element_pickobject_element = _doc.GetElement(ref_pickobject_element.ElementId);

                _ModelLine1.LineaSeleccionada = Element_pickobject_element;
                _ModelLine1.PtoSeleccion = ref_pickobject_element.GlobalPoint;
                if (Element_pickobject_element is ModelLine)
                {                  
                    _ModelLine1.Pt1 = ((ModelLine)Element_pickobject_element).GeometryCurve.GetEndPoint(0);
                    _ModelLine1.Pt2 = ((ModelLine)Element_pickobject_element).GeometryCurve.GetEndPoint(1);       
                }
                else if (Element_pickobject_element is DetailLine)
                {

                    _ModelLine1.Pt1 = ((DetailLine)Element_pickobject_element).GeometryCurve.GetEndPoint(0);
                    _ModelLine1.Pt2 = ((DetailLine)Element_pickobject_element).GeometryCurve.GetEndPoint(1);
                }
                else
                {
                    Util.ErrorMsg($"Error al seleccionar. Elemento Seleccionado: {Element_pickobject_element.GetType().Name}. Seleccionar ModelLine");
                    return false;
                }

//                _ModelLine1 = Element_pickobject_element as ModelLine;
                //_ptoModelLine1 = ref_pickobject_element.GlobalPoint;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool SeleciconarLineas2()
        {
            try
            {
                ISelectionFilter f = new LineSelectionFilte();
                Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Element, "Seleccionar RoomLine 2:");


                if (ref_pickobject_element == null) return false;
                //obtiene una referencia floor con la referencia r
                Element Element_pickobject_element = _doc.GetElement(ref_pickobject_element.ElementId);


                _ModelLine2.LineaSeleccionada = Element_pickobject_element;
                _ModelLine2.PtoSeleccion = ref_pickobject_element.GlobalPoint;
                if (Element_pickobject_element is ModelLine)
                {
                    _ModelLine2.Pt1 = ((ModelLine)Element_pickobject_element).GeometryCurve.GetEndPoint(0);
                    _ModelLine2.Pt2 = ((ModelLine)Element_pickobject_element).GeometryCurve.GetEndPoint(1);
                }
                else if (Element_pickobject_element is DetailLine)
                {

                    _ModelLine2.Pt1 = ((DetailLine)Element_pickobject_element).GeometryCurve.GetEndPoint(0);
                    _ModelLine2.Pt2 = ((DetailLine)Element_pickobject_element).GeometryCurve.GetEndPoint(1);
                }
                else
                {
                    Util.ErrorMsg("Error al seleccionar. Seleccionar ModelLine");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }


        private void GenerarListaConBordesDelRectanguloV2()
        {
            //ptoIntersecionFloor.Z esto es para posicionar las modelline a la altuara de face superior de floor o fundacion
            XYZ _p1Linea1 = _ModelLine1.Pt1.AsignarZ(ptoIntersecionFloor.Z);
            XYZ _p2Linea1 = _ModelLine1.Pt2.AsignarZ(ptoIntersecionFloor.Z);


            Line _ModelLine2_aux = Line.CreateBound(_ModelLine2.Pt1.AsignarZ(ptoIntersecionFloor.Z),
                                                    _ModelLine2.Pt2.AsignarZ(ptoIntersecionFloor.Z));

            XYZ _p1Linea2 = (_ModelLine2_aux).ProjectExtendidaXY0(_p1Linea1);
            XYZ _p2Linea2 = (_ModelLine2_aux).ProjectExtendidaXY0(_p2Linea1);
            ListaBorde1.Add(_p1Linea1);
            ListaBorde1.Add(_p2Linea1);

            ListaBorde2.Add(_p1Linea2);
            ListaBorde2.Add(_p2Linea2);

        }
        private void PtoMouseSobreMuroFalso()
        {
            _PtoMouseEspejo1 = (ListaBorde1[0] + ListaBorde1[1]) / 2;
            _PtoMouseEspejo2 = (ListaBorde2[0] + ListaBorde2[1]) / 2;
            AnguloDireccionenfierrado = Util.AnguloEntre2PtosGrados_enPlanoXY(_PtoMouseEspejo1, _PtoMouseEspejo2);
        }

        internal SeleccinarMuroRefuerzo ObtenerMuroMirror1()
        {

            SeleccinarMuroRefuerzo seleccinarMuroRefuerzoMirror = new SeleccinarMuroRefuerzo(_uiapp);
            seleccinarMuroRefuerzoMirror.ElementoSeleccionado = _losaSelecionado;
            seleccinarMuroRefuerzoMirror.TipoElemto = TipoElemento.muro;
            seleccinarMuroRefuerzoMirror.pto1SeleccionadoConMouse = _PtoMouseEspejo1;
            seleccinarMuroRefuerzoMirror.ListaPtosBordeMuroIntersectado = ListaBorde1;
            seleccinarMuroRefuerzoMirror.DireccionEnfierrado = DireccionHaciaLosa;
            seleccinarMuroRefuerzoMirror.PtoInterseccionSobreBorde = _PtoMouseEspejo1;
            seleccinarMuroRefuerzoMirror.AnguloDireccionenfierrado = AnguloDireccionenfierrado;
            return seleccinarMuroRefuerzoMirror;
        }

        internal SeleccinarMuroRefuerzo ObtenerMuroMirror2()
        {
            SeleccinarMuroRefuerzo seleccinarMuroRefuerzoMirror = new SeleccinarMuroRefuerzo(_uiapp);
            seleccinarMuroRefuerzoMirror.ElementoSeleccionado = _losaSelecionado;
            seleccinarMuroRefuerzoMirror.TipoElemto = TipoElemento.muro;
            seleccinarMuroRefuerzoMirror.pto1SeleccionadoConMouse = _PtoMouseEspejo2;
            seleccinarMuroRefuerzoMirror.ListaPtosBordeMuroIntersectado = ListaBorde2;
            seleccinarMuroRefuerzoMirror.DireccionEnfierrado = DireccionHaciaLosa;
            seleccinarMuroRefuerzoMirror.PtoInterseccionSobreBorde = _PtoMouseEspejo2;
            seleccinarMuroRefuerzoMirror.AnguloDireccionenfierrado = AnguloDireccionenfierrado;

            return seleccinarMuroRefuerzoMirror;
        }
    }
}

