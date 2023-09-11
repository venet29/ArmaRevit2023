using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.LosaArmadura.Geometria;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos
{
    public class CalculoGeometriaRectangulo2ptos
    {
        private UIApplication _uiapp;
        private View _view;
        private SeleccionarLosaConMouse _seleccionarLosaConMouse1;
        private readonly int diamtromm;
        private readonly View3D _elem3D_Parabusacr;
        private readonly TipoRefuerzoLOSA _tipoRefuerzoLOSA;
        private SeleccionarLosaConMouse _seleccionarLosaConMouse2;

        public List<XYZ> ListaBorde1 { get; set; }
        public List<XYZ> ListaBorde2 { get; set; }
#pragma warning disable CS0649 // Field 'CalculoGeometriaRectangulo2ptos.DireccionHaciaLosa' is never assigned to, and will always have its default value null
        private XYZ DireccionHaciaLosa;
#pragma warning restore CS0649 // Field 'CalculoGeometriaRectangulo2ptos.DireccionHaciaLosa' is never assigned to, and will always have its default value null
        public Floor _losaSelecionado { get; set; }
        public XYZ _direccionBarra;

        public XYZ _direccionPerpendicular { get; private set; }

        private XYZ _p1;
        private XYZ _p2;
        private XYZ _PtoMouseEspejo1;
        private XYZ _PtoMouseEspejo2;
        private PlanarFace _CaraSuperiorLosa;
        private PlanarFace _CaraSuperiorLosa2;
        private XYZ _VectorNormalbarra;

        public CalculoGeometriaRectangulo2ptos(UIApplication uiapp, SeleccionarLosaConMouse seleccionarLosaConMouse1, int diamtromm, View3D elem3d_parabusacr, TipoRefuerzoLOSA _tipoRefuerzoLOSA)
        {
            this._uiapp = uiapp;
            this._view = _uiapp.ActiveUIDocument.ActiveView;

            this._seleccionarLosaConMouse1 = seleccionarLosaConMouse1;
            this.diamtromm = diamtromm;
            this._elem3D_Parabusacr = elem3d_parabusacr;
            this._tipoRefuerzoLOSA = _tipoRefuerzoLOSA;
            this._seleccionarLosaConMouse2 = new SeleccionarLosaConMouse(uiapp);

            ListaBorde1 = new List<XYZ>();
            ListaBorde2 = new List<XYZ>();
        }

        public bool Ejecutar()
        {
            try
            {
                if (_seleccionarLosaConMouse1 != null)
                {
                    this._seleccionarLosaConMouse2.LosaSelecionado = _seleccionarLosaConMouse1.LosaSelecionado;
                    this._CaraSuperiorLosa = _seleccionarLosaConMouse1.LosaSelecionado.ObtenerCaraSuperior(_seleccionarLosaConMouse1._ptoSeleccionEnLosa, new XYZ(0, 0, 1));
                    if (_CaraSuperiorLosa != null)
                    {
                        this._VectorNormalbarra = _CaraSuperiorLosa.FaceNormal;// seleccionarLosaConMouse1.LosaSelecionado.ObtenerNormal();//    _CaraSuperiorLosa.FaceNormal;
                    }
                    else
                    {
                        var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(_seleccionarLosaConMouse1.LosaSelecionado);
                        if (losa_Encontrada_RuledFace != null)
                            this._VectorNormalbarra = losa_Encontrada_RuledFace.normal;
                        else
                        {
                            Util.ErrorMsg("Error al obtener caras superior de elemento seleccionado");
                            return false;
                        }
                    }
                }
                else
                {
                    Util.ErrorMsg("Error al obtener caras superior de elemento seleccionado");
                    return false;
                }

                if (!ObtenerDirecciones()) return false;
                GenerarListaConRectangulo();
                PtoMouseSobreMuroFalso();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }




        private bool ObtenerDirecciones()
        {
            try
            {
                List<XYZ> lista2ptos = Util.Pick2Point(_uiapp.ActiveUIDocument);

                if (lista2ptos.Count != 2) return false;
                if (lista2ptos[0].DistanceTo(lista2ptos[1]) < Util.CmToFoot(10))
                {
                    Util.ErrorMsg("Distancia entre puntos de seleccion debe ser mayor a 10 cm");
                    return false;
                }

                if (!ObtenerLosaCOn2doPto(lista2ptos)) return false;

                _direccionBarra = (_seleccionarLosaConMouse2._ptoSeleccionEnLosa2 - _seleccionarLosaConMouse1._ptoSeleccionEnLosa).Normalize();

                _direccionPerpendicular = Util.CrossProduct(_direccionBarra, new XYZ(0, 0, 1));
                _VectorNormalbarra = Util.CrossProduct(_direccionBarra, _direccionPerpendicular);

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }

        private bool ObtenerLosaCOn2doPto(List<XYZ> lista2ptos)
        {
            try
            {
                XYZ pto2 = lista2ptos[1];
                if (_tipoRefuerzoLOSA == TipoRefuerzoLOSA.fundacion)
                {
                    SeleccionarLosaOFunda _SeleccionarLosaOFunda = new SeleccionarLosaOFunda(_uiapp, _elem3D_Parabusacr, _tipoRefuerzoLOSA);

                    if (!_SeleccionarLosaOFunda.SeleccionarElementoFLoor_Fundaction(pto2)) return false;
                    _losaSelecionado = _SeleccionarLosaOFunda.LosaSelecionado;
                    if (_losaSelecionado != null)
                    {
                        _CaraSuperiorLosa2 = _losaSelecionado.ObtenerCaraSuperior(pto2, new XYZ(0, 0, 1));
                        _CaraSuperiorLosa = _CaraSuperiorLosa2;
                    }
                }
                else
                {
                    SeleccionarLosaConPto _SeleccionarLosaConPto = new SeleccionarLosaConPto(_uiapp.ActiveUIDocument.Document);
                    Level lv = _view.GenLevel;
                    if (lv == null) return false;
                    _losaSelecionado = _SeleccionarLosaConPto.EjecturaSeleccionarLosaConPto(pto2, lv);
                    if (_losaSelecionado != null)
                    {
                        _CaraSuperiorLosa2 = _losaSelecionado.ObtenerCaraSuperior(pto2, new XYZ(0, 0, 1));
                        _CaraSuperiorLosa = _CaraSuperiorLosa2;
                    }
                }


                //validar
                if (_CaraSuperiorLosa != null)
                {
                    _seleccionarLosaConMouse1._ptoSeleccionEnLosa = _CaraSuperiorLosa.ObtenerPtosInterseccionFaceHorizontal(lista2ptos[0]) + new XYZ(0, 0, -Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm + diamtromm / 10));

                    _seleccionarLosaConMouse2._ptoSeleccionEnLosa2 = _CaraSuperiorLosa.ObtenerPtosInterseccionFaceHorizontal(lista2ptos[1]) + new XYZ(0, 0, -Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm + diamtromm / 10));

                }
                else
                {
                    //pto1
                    var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(_seleccionarLosaConMouse1.LosaSelecionado);
                    if (losa_Encontrada_RuledFace != null)
                        _seleccionarLosaConMouse1._ptoSeleccionEnLosa = losa_Encontrada_RuledFace.ReludfaceNH.ProjectNH(lista2ptos[0]) + new XYZ(0, 0, -Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm + diamtromm / 10));
                    else
                    {
                        Util.ErrorMsg("Error al obtener caras superior de elemento seleccionado");
                        return false;
                    }

                    //pto2
                    var losa_Encontrada_RuledFace2 = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(_seleccionarLosaConMouse1.LosaSelecionado);
                    if (losa_Encontrada_RuledFace2 != null)
                        _seleccionarLosaConMouse2._ptoSeleccionEnLosa2 = losa_Encontrada_RuledFace.ReludfaceNH.ProjectNH(lista2ptos[1]) + new XYZ(0, 0, -Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm + diamtromm / 10));
                    else
                    {
                        Util.ErrorMsg("Error al obtener caras superior de elemento seleccionado");
                        return false;
                    }
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void GenerarListaConRectangulo()
        {
            _p1 = _seleccionarLosaConMouse1._ptoSeleccionEnLosa;
            _p2 = _seleccionarLosaConMouse2._ptoSeleccionEnLosa2;

            double MedioAncho = Util.CmToFoot(10);
            ListaBorde1.Add(_p1 + _direccionPerpendicular * MedioAncho);
            ListaBorde1.Add(_p1 - _direccionPerpendicular * MedioAncho);

            ListaBorde2.Add(_p2 + _direccionPerpendicular * MedioAncho);
            ListaBorde2.Add(_p2 - _direccionPerpendicular * MedioAncho);
        }

        private void PtoMouseSobreMuroFalso()
        {
            _PtoMouseEspejo1 = (ListaBorde1[0] + ListaBorde1[1]) / 2 + (_p1 - _p2).Normalize() * 1;
            _PtoMouseEspejo2 = (ListaBorde2[0] + ListaBorde2[1]) / 2 + (_p2 - _p1).Normalize() * 1;
        }

        internal SeleccinarMuroRefuerzo ObtenerMuroMirror1()
        {

            SeleccinarMuroRefuerzo seleccinarMuroRefuerzoMirror = new SeleccinarMuroRefuerzo(_uiapp);
            seleccinarMuroRefuerzoMirror.ElementoSeleccionado = _seleccionarLosaConMouse2.LosaSelecionado;
            seleccinarMuroRefuerzoMirror.TipoElemto = TipoElemento.losa;
            seleccinarMuroRefuerzoMirror.pto1SeleccionadoConMouse = _PtoMouseEspejo1;
            seleccinarMuroRefuerzoMirror.ListaPtosBordeMuroIntersectado = ListaBorde1;
            seleccinarMuroRefuerzoMirror.DireccionEnfierrado = DireccionHaciaLosa;
            seleccinarMuroRefuerzoMirror.NormalCaraSuperiorLosa = _VectorNormalbarra;

            return seleccinarMuroRefuerzoMirror;
        }

        internal SeleccinarMuroRefuerzo ObtenerMuroMirror2()
        {
            SeleccinarMuroRefuerzo seleccinarMuroRefuerzoMirror = new SeleccinarMuroRefuerzo(_uiapp);
            seleccinarMuroRefuerzoMirror.ElementoSeleccionado = _seleccionarLosaConMouse2.LosaSelecionado;
            seleccinarMuroRefuerzoMirror.TipoElemto = TipoElemento.losa;
            seleccinarMuroRefuerzoMirror.pto1SeleccionadoConMouse = _PtoMouseEspejo2;
            seleccinarMuroRefuerzoMirror.ListaPtosBordeMuroIntersectado = ListaBorde2;
            seleccinarMuroRefuerzoMirror.DireccionEnfierrado = DireccionHaciaLosa;
            seleccinarMuroRefuerzoMirror.NormalCaraSuperiorLosa = _VectorNormalbarra;

            return seleccinarMuroRefuerzoMirror;
        }
    }
}

