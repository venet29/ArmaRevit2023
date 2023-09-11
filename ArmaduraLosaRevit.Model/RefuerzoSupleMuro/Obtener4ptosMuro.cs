using System;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Fund.Servicios;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.RefuerzoSupleMuro
{
    internal class Obtener4ptosMuro
    {
        private UIApplication _uiapp;
        private View _view;
        private SeleccionarElementosV _seleccionarElementos;
        private PuntosSeleccionMouse _puntosSeleccionMouse;
        private List<PlanarFace> _planarFaceSup;
        private double Zsup;
        private double ZIni;
        private double AlturaMenorMuro;


        public XYZ _p1 { get; set; }
        public XYZ _p2 { get; set; }
        public XYZ _p3 { get; set; }
        public XYZ _p4 { get; set; }
        public double LargoBarra_foot { get; private set; }

        public Obtener4ptosMuro(UIApplication uiapp, SeleccionarElementosV seleccionarElementos, PuntosSeleccionMouse puntosSeleccionMouse)
        {
            _uiapp = uiapp;
            this._view = _uiapp.ActiveUIDocument.ActiveView;
            _seleccionarElementos = seleccionarElementos;
            _puntosSeleccionMouse = puntosSeleccionMouse;
        }

        public bool EjecutarObtener4ptos()
        {
            try
            {
                XYZ vectorFaceNormal = _view.ViewDirection;
                if (!AyudaObtenerNormarPlanoVisisible.Obtener(_seleccionarElementos._ElemetSelect, _view)) return false;
                vectorFaceNormal = AyudaObtenerNormarPlanoVisisible.FaceNormal;

                _puntosSeleccionMouse.p1_origenDiretriz = ProyectadoEnPlano
                    .ObtenerPtoProyectadoEnPlano_conRedondeo8(vectorFaceNormal, _seleccionarElementos._ptoSeleccionMouseCentroCaraMuro, _puntosSeleccionMouse.p1_origenDiretriz)
                    + _view.ViewDirection * ConstNH.RECUBRIMIENTO_MALLA_foot;

                _puntosSeleccionMouse.p2_SentidoDiretriz = ProyectadoEnPlano
                    .ObtenerPtoProyectadoEnPlano_conRedondeo8(vectorFaceNormal, _seleccionarElementos._ptoSeleccionMouseCentroCaraMuro, _puntosSeleccionMouse.p2_SentidoDiretriz)
                    + _view.ViewDirection * ConstNH.RECUBRIMIENTO_MALLA_foot;

                if (!comprabaciones()) return false;

                if (!ObtenerAlturaMuro()) return false;

                if (!Obtener4Ptos()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error para obtener 4 puntos. \n ex:{ex.Message}");
                return false;
            }
            return true;
        }



        private bool ObtenerAlturaMuro()
        {
            try
            {
                PlanarFace _PlanarFaceSaliendo = _planarFaceSup.Where(c => Util.IsSimilarValor(Util.GetProductoEscalar(c.FaceNormal, -_view.ViewDirection), 1, 0.0001)).FirstOrDefault();

                List<Curve> ListaCurveVertical = _PlanarFaceSaliendo.ObtenerListaCurvas().Where(c => Util.IsVertical(c.GetEndPoint(1) - c.GetEndPoint(0)) && c.Length > Util.CmToFoot(200)).ToList();

                Curve minimoBorde = ListaCurveVertical.MinBy(c => c.Length);

                XYZ pto1 = minimoBorde.GetEndPoint(0);
                XYZ pto2 = minimoBorde.GetEndPoint(1);

                if (pto2.Z > pto1.Z)
                {
                    Zsup = pto2.Z;
                    ZIni = pto1.Z;
                }
                else
                {
                    Zsup = pto1.Z;
                    ZIni = pto2.Z;
                }

                AlturaMenorMuro = Zsup - ZIni;
                Zsup = Zsup - AlturaMenorMuro * 0.1;
                ZIni = ZIni + AlturaMenorMuro * 0.1;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }

            return true;
        }

        private bool Obtener4Ptos()
        {
            try
            {
                //XYZ direccionDiretriz = (_puntosSeleccionMouse.p2_SentidoDiretriz.GetXY0() - _puntosSeleccionMouse.p1_origenDiretriz.GetXY0()).Normalize();

                //if (Util.IsSimilarValor(Util.GetProductoEscalar(direccionDiretriz, _view.RightDirection), 1, 0.0001))
                //{
                //    /*
                //     p4   p3
                //     p1   p2
                //     */

                //    _p1 = _puntosSeleccionMouse.p1_origenDiretriz.AsignarZ(ZIni);
                //    _p2 = _puntosSeleccionMouse.p2_SentidoDiretriz.AsignarZ(ZIni);
                //    _p3 = _puntosSeleccionMouse.p2_SentidoDiretriz.AsignarZ(Zsup);
                //    _p4 = _puntosSeleccionMouse.p1_origenDiretriz.AsignarZ(Zsup);
                //}
                //else
                //{

                //    _p1 = _puntosSeleccionMouse.p2_SentidoDiretriz.AsignarZ(ZIni);
                //    _p2 = _puntosSeleccionMouse.p1_origenDiretriz.AsignarZ(ZIni);
                //    _p3 = _puntosSeleccionMouse.p1_origenDiretriz.AsignarZ(Zsup);
                //    _p4 = _puntosSeleccionMouse.p2_SentidoDiretriz.AsignarZ(Zsup);
                //}


                //     p4   p3
                //     p1   p2
                _p1 = _puntosSeleccionMouse.p1_origenDiretriz.AsignarZ(ZIni);
                _p2 = _puntosSeleccionMouse.p2_SentidoDiretriz.AsignarZ(ZIni);
                _p3 = _puntosSeleccionMouse.p2_SentidoDiretriz.AsignarZ(Zsup);
                _p4 = _puntosSeleccionMouse.p1_origenDiretriz.AsignarZ(Zsup);

                LargoBarra_foot = _p2.DistanceTo(_p3);
            }
            catch (Exception)
            {

                return false;
            }
            return true; ;
        }



        private bool comprabaciones()
        {
            try
            {
                Wall muroSeleccionado = _seleccionarElementos._ElemetSelect as Wall;

                if (muroSeleccionado == null) return false;
                List<List<PlanarFace>> ListaPlanarFaceSup = muroSeleccionado.ListaFace();
                if (ListaPlanarFaceSup.Count == 0)
                {
                    Util.ErrorMsg("Error en el numero dse mensajes a)");
                    return false;
                }

                _planarFaceSup = ListaPlanarFaceSup[0];
                if (_planarFaceSup.Count == 0)
                {
                    Util.ErrorMsg("Error en el numero dse mensajes b)");
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }
    }
}