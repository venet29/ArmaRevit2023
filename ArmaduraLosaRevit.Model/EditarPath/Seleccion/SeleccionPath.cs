using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;

namespace ArmaduraLosaRevit.Model.EditarPath.Seleccion
{
    public class SeleccionPath
    {
   
        public PathReinforcement pathReinforcement { get; set; }
        private XYZ _ptoSeleccionMouse;
        private Application _app;
        private Document _doc;
        private UIDocument _uidoc;
        private View _view;
        private Level _Level;

        public CoordenadaPath _coordenadaPath { get; set; }
        public Line _lineBordeSeleccionadoInicial { get; set; }
        public DireccionEdicionPathRein _direccionEdicionPathRein { get; set; }
        public DireccionEdicionPathRein _direccionEdicionPathReinFinal { get; set; }

        public Line _lineBordeSeleccionadoFinal { get; private set; }
        public double DistanciaAlargarPAth { get; set; }
        public XYZ DireccionDesdeCentroAlBorde { get; set; }
        public XYZ DireccionBordeAlPtoMouse { get; set; }
        public XYZ direccionBarras { get; set; }
        public double _angulo23_rad { get; private set; }
        public TipoCasoAlternativo TipoCasoAlternativo_ { get; internal set; }

        public SeleccionPath(UIApplication _Application, SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto)
        {
            this._app = _Application.Application;
            this._doc = _Application.ActiveUIDocument.Document;
            this._uidoc = _Application.ActiveUIDocument;
            this._view = this._doc.ActiveView;
            this._Level = _doc.ActiveView.GenLevel;
            this.pathReinforcement = seleccionarPathReinfomentConPto.PathReinforcement;
            this._ptoSeleccionMouse = seleccionarPathReinfomentConPto.PuntoSeleccionMouse;
        }

        public void ObtenerBordeDepath()
        {
            CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(pathReinforcement, _doc);
            pathReinformeCalculos.Calcular4PtosPathReinf();
            _coordenadaPath = pathReinformeCalculos.Obtener4pointPathReinf();
            direccionBarras = (_coordenadaPath.p3 - _coordenadaPath.p2).Normalize();
            _angulo23_rad = Util.AnguloEntre2PtosGrado90(_coordenadaPath.p2, _coordenadaPath.p3,false);
        }

        public DireccionEdicionPathRein ObtenerPtoSeleccionConMouseEnPathrein()
        {
            ObtenerPtoSeleccionConMouseEnPathrein(_ptoSeleccionMouse);
            return _direccionEdicionPathRein;
        }

        public DireccionEdicionPathRein ObtenerPtoSeleccionConMouseEnPathrein(XYZ _ptoSeleccionMouse)
        {
            _direccionEdicionPathRein = DireccionEdicionPathRein.NONE;

            if (ptoDentroTriangulo(_coordenadaPath.p1, _coordenadaPath.p2, _coordenadaPath.centro, _ptoSeleccionMouse))
            {
                _lineBordeSeleccionadoInicial = Line.CreateBound(_coordenadaPath.p1.GetXY0(), _coordenadaPath.p2.GetXY0());
                _direccionEdicionPathRein = DireccionEdicionPathRein.Izquierda;
            };
            if (ptoDentroTriangulo(_coordenadaPath.p2, _coordenadaPath.p3, _coordenadaPath.centro, _ptoSeleccionMouse))
            {
                _lineBordeSeleccionadoInicial = Line.CreateBound(_coordenadaPath.p2.GetXY0(), _coordenadaPath.p3.GetXY0());
                _direccionEdicionPathRein = DireccionEdicionPathRein.Inferior;
            }
            if (ptoDentroTriangulo(_coordenadaPath.p3, _coordenadaPath.p4, _coordenadaPath.centro, _ptoSeleccionMouse))
            {
                _lineBordeSeleccionadoInicial = Line.CreateBound(_coordenadaPath.p3.GetXY0(), _coordenadaPath.p4.GetXY0());
                _direccionEdicionPathRein = DireccionEdicionPathRein.Derecha;
            }

            if (ptoDentroTriangulo(_coordenadaPath.p4, _coordenadaPath.p1, _coordenadaPath.centro, _ptoSeleccionMouse))
            {
                _lineBordeSeleccionadoInicial = Line.CreateBound(_coordenadaPath.p4.GetXY0(), _coordenadaPath.p1.GetXY0());
                _direccionEdicionPathRein = DireccionEdicionPathRein.Superior;
            }
            return _direccionEdicionPathRein;

        }



        public DireccionEdicionPathRein ObtenerPtoSeleccionConMouseEnPathrein2Ptos()
        {
            ObtenerPtoSeleccionConMouseEnPathrein2ptos(_ptoSeleccionMouse);
            return _direccionEdicionPathRein;
        }

        public DireccionEdicionPathRein ObtenerPtoSeleccionConMouseEnPathrein2ptos(XYZ _ptoSeleccionMouse)
        {
            _direccionEdicionPathRein = DireccionEdicionPathRein.NONE;

            if (ptoDentroTriangulo(_coordenadaPath.p1.GetXY0(), _coordenadaPath.p2.GetXY0(), _coordenadaPath.centro.GetXY0(), _ptoSeleccionMouse.GetXY0()))
            {
                _lineBordeSeleccionadoInicial = Line.CreateBound(_coordenadaPath.p1.GetXY0(), _coordenadaPath.p2.GetXY0());
                _direccionEdicionPathRein = DireccionEdicionPathRein.Izquierda;
            }
            else if (ptoDentroTriangulo(_coordenadaPath.p2.GetXY0(), _coordenadaPath.p3.GetXY0(), _coordenadaPath.centro.GetXY0(), _ptoSeleccionMouse.GetXY0()))
            {
                _lineBordeSeleccionadoInicial = Line.CreateBound(_coordenadaPath.p2.GetXY0(), _coordenadaPath.p3.GetXY0());
                _direccionEdicionPathRein = DireccionEdicionPathRein.Inferior;

                //
                _lineBordeSeleccionadoFinal = Line.CreateBound(_coordenadaPath.p4.GetXY0(), _coordenadaPath.p1.GetXY0());
                _direccionEdicionPathReinFinal = DireccionEdicionPathRein.Superior;
            }
            else if (ptoDentroTriangulo(_coordenadaPath.p3.GetXY0(), _coordenadaPath.p4.GetXY0(), _coordenadaPath.centro.GetXY0(), _ptoSeleccionMouse.GetXY0()))
            {
                _lineBordeSeleccionadoInicial = Line.CreateBound(_coordenadaPath.p3.GetXY0(), _coordenadaPath.p4.GetXY0());
                _direccionEdicionPathRein = DireccionEdicionPathRein.Derecha;
                
            }
            else if (ptoDentroTriangulo(_coordenadaPath.p4.GetXY0(), _coordenadaPath.p1.GetXY0(), _coordenadaPath.centro.GetXY0(), _ptoSeleccionMouse.GetXY0()))
            {
                _lineBordeSeleccionadoInicial = Line.CreateBound(_coordenadaPath.p4.GetXY0(), _coordenadaPath.p1.GetXY0());
                _direccionEdicionPathRein = DireccionEdicionPathRein.Superior;

                _lineBordeSeleccionadoFinal = Line.CreateBound(_coordenadaPath.p2.GetXY0(), _coordenadaPath.p3.GetXY0());
                _direccionEdicionPathReinFinal = DireccionEdicionPathRein.Inferior;
            }
            else
            {
                _direccionEdicionPathRein = DireccionEdicionPathRein.NONE;
                Util.ErrorMsg("Error al seleccionar tercio de path");
            }
            return _direccionEdicionPathRein;
        }

        public double ObtenerDistanciaPerpendicularDesdePtoABorde(XYZ ptoReferencia)
        {
            //IntersectionResult intersectionResult = _lineBordeSeleccionadoInicial.ExtenderLineaXY0(200).Project(ptoReferencia.GetXY0());
            return DistanciaAlargarPAth = Util.ObtenerDistanciaPerpendicularDesdePtoALine_XY0(_lineBordeSeleccionadoInicial, ptoReferencia);
        }


        public XYZ ObtenerDireccionDesdeCentroAlBorde(XYZ ptoCentro)
        {
            IntersectionResult intersectionResult = _lineBordeSeleccionadoInicial.ExtenderLineaXY0(200).Project(ptoCentro.GetXY0());
            return DireccionDesdeCentroAlBorde = intersectionResult.XYZPoint.GetXY0() - ptoCentro.GetXY0();
        }

        public XYZ ObtenerDireccionDesdeCentroAlBordePtoFinal(XYZ ptoCentro)
        {
            IntersectionResult intersectionResult = _lineBordeSeleccionadoFinal.ExtenderLineaXY0(200).Project(ptoCentro.GetXY0());
            return DireccionDesdeCentroAlBorde = intersectionResult.XYZPoint.GetXY0() - ptoCentro.GetXY0();
        }


        public XYZ ObtenerDireccionBordeAlPtoMouse(XYZ ptoReferencia)
        {
            IntersectionResult intersectionResult = _lineBordeSeleccionadoInicial.ExtenderLineaXY0(200).Project(ptoReferencia.GetXY0());
            return DireccionBordeAlPtoMouse = ptoReferencia.GetXY0() - intersectionResult.XYZPoint;
        }

        public XYZ ObtenerDireccionBordeAlPtoMousePtoFinal(XYZ ptoReferencia)
        {
            IntersectionResult intersectionResult = _lineBordeSeleccionadoFinal.ExtenderLineaXY0(200).Project(ptoReferencia.GetXY0());
            return DireccionBordeAlPtoMouse = ptoReferencia.GetXY0() - intersectionResult.XYZPoint;
        }



        public XYZ PtoMEdioDeBordePAthSeleccionado()
        {
            XYZ resul = _lineBordeSeleccionadoInicial.Evaluate(0.5, false);
            return resul;
        }
        // obs3_ptoDentroTriangulo
        private bool ptoDentroTriangulo(XYZ a, XYZ b, XYZ c, XYZ pto)
        {

            //Sea d el segmento ab.
            XYZ d = b - a;
            //Sea e el segmento ac.
            XYZ e = c - a;
            //Variable de ponderación a~b
            double w1 = (e.X * (a.Y - pto.Y) + e.Y * (pto.X - a.X)) / (d.X * e.Y - d.Y * e.X);
            //Variable de ponderación a~c
            double w2 = (pto.Y - a.Y - w1 * d.Y) / e.Y;
            //El punto p se encuentra dentro del triángulo
            //si se cumplen las 3 condiciones:
            if ((w1 >= 0.0) && (w2 >= 0.0) && ((w1 + w2) <= 1.0))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
