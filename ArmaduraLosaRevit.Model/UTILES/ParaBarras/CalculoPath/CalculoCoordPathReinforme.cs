using ArmaduraLosaRevit.Model.Traslapo.Help;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Traslapo.extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using System;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Enumeraciones;


namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath
{
    /// <summary>
    /// otiene los cuatro putos de path
    /// </summary>
    public class CalculoCoordPathReinforme
    {
        public UbicacionLosa DireccionBarra_ { get; private set; }
        public bool IsPtoOK { get; private set; } = false;
        public double AngleP2_p3 { get; private set; }

        public CoordenadaPath _4pointPathReinf { get; set; }
        public PathReinforcement _pathReinforcement { get; set; }

        private Document _doc;
        private XYZ _normalPlanoPAth;
        private XYZ _DireccionBarra;

        //  private XYZ _puntoSeleccionMouse;

        //CalculoTiposTraslapos _calculoTiposTraslapos;

        public CalculoCoordPathReinforme()
        {

        }

        public CalculoCoordPathReinforme(SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto, Document doc)
        {
            this._doc = doc;
            //     this._puntoSeleccionMouse = seleccionarPathReinfomentConPto.puntoSeleccionMouse;
            this._pathReinforcement = seleccionarPathReinfomentConPto.PathReinforcement;
            this._4pointPathReinf = new CoordenadaPath();

        }
        public CalculoCoordPathReinforme(PathReinforcement pathReinforcement, Document doc)
        {
            this._doc = doc;
            this._pathReinforcement = pathReinforcement;
            
            this._4pointPathReinf = new CoordenadaPath();
            //   this._calculoTiposTraslapos = _calculoTiposTraslapos;
        }

        public CoordenadaPath Obtener4pointPathReinf() => _4pointPathReinf;

        public CoordenadaPath Calcular4PtosPathReinf()
        {
            try
            {
                if (!ObtenerPrimeraLineaPerpendiDireccionBarra()) return new CoordenadaPath();
                if (!ObtenerSegundaLinaPerpendiDireccionBarra()) return new CoordenadaPath();
                DireccionBarra_ = EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, _pathReinforcement.ObtenerDireccionBarra());
                IsPtoOK = true;
                _4pointPathReinf.CalcularCentroPath();
                _4pointPathReinf.ObtenerLargos();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'Calcular4PtosPathReinf' ex:{ex.Message}");
                IsPtoOK = false;
            }
            return _4pointPathReinf;
        }


        public bool ObtenerAngulo_P2_P3()
        {
            try
            {
                //p1   p4
                //p2   p3
                if (_pathReinforcement == null) return false;

                //obtener la linea que define al path
                var LineaDefineREbarInsystem = _pathReinforcement.GetRebarInSystemIds().ToList();
                if (LineaDefineREbarInsystem == null) return false;
                if (LineaDefineREbarInsystem.Count == 0) return false;

                Element elemREbarSystem = _doc.GetElement2(LineaDefineREbarInsystem[0]);
                RebarInSystem rebarInSystem_ = (RebarInSystem)elemREbarSystem;
                if (rebarInSystem_ == null) return false;
                Line linepath = rebarInSystem_.GetDistributionPath();

                XYZ direccion = linepath.Direction.GetXY0();
                AngleP2_p3=  Util.GetAnguloVectoresEnGrados_enPlanoXY(direccion)+(DireccionBarra_==UbicacionLosa.Izquierda|| DireccionBarra_ == UbicacionLosa.Derecha ? 90:0);


            }
            catch (System.Exception ex)
            {
                Util.ErrorMsg($" Error en ex:{ex.Message}");
                return false;
            }
            return true;
        }


        private bool ObtenerPrimeraLineaPerpendiDireccionBarra()
        {
            try
            {
                //NOta   la direccion :la path la obtengo del 'RebarInSystem'
                //       el origen :   del ModelLine  ->  _pathReinforcement.GetCurveElementIds()
                //       el _normalPlanoPAth :   del ModelLine  ->  _pathReinforcement.GetCurveElementIds()
                // todo lo anterior para obtener '_DireccionBarra'
                if (_pathReinforcement == null) return false;

                //obtener la linea que define al path
                var LineaDefineREbarInsystem = _pathReinforcement.GetRebarInSystemIds().ToList();
                if (LineaDefineREbarInsystem == null) return false;
                if (LineaDefineREbarInsystem.Count == 0) return false;

                Element elemREbarSystem = _doc.GetElement2(LineaDefineREbarInsystem[0]);
                RebarInSystem rebarInSystem_ = (RebarInSystem)elemREbarSystem;
                if (rebarInSystem_ == null) return false;
                Line linepath = rebarInSystem_.GetDistributionPath();

                //linea
                var LineaDefinePath = _pathReinforcement.GetCurveElementIds().ToList();
                if (LineaDefinePath == null) return false;
                if (LineaDefinePath.Count == 0) return false;
                Element elem = _doc.GetElement2(LineaDefinePath[0]);

                if (!(elem is ModelLine)) return false;
                ModelLine MOdeLine = (ModelLine)elem;

                if (MOdeLine == null) return false;
                _normalPlanoPAth = MOdeLine.SketchPlane.GetPlane().Normal;

                Line LineCUrve = MOdeLine.GeometryCurve as Line;

                var ListaPto = LineCUrve.Tessellate().ToList();

                _4pointPathReinf.p4 = ListaPto[0];// LineCUrve.Origin;// _pathReinforcement.ObtenerOrigenodificado(LineCUrve)    ;

                // parametros importante
                _DireccionBarra = Util.CrossProduct(linepath.Direction, _normalPlanoPAth);

                //XYZ PathReinf_direccion = LineCUrve.Direction;
                // double PathReinf_largo = LineCUrve.Length;
                _4pointPathReinf.p3 = ListaPto[1];// _4pointPathReinf.p4 + PathReinf_direccion * PathReinf_largo;

            }
            catch (System.Exception ex)
            {
                Util.ErrorMsg($" Error en ex:{ex.Message}");
                return false;
            }
            return true;
        }


        private bool ObtenerSegundaLinaPerpendiDireccionBarra()
        {
            try
            {

                contornoPath _contornoPath = ObtenerLineaParalelaBarra_DesdeConjuntoDeElemtosDelPathReinforment();

                if (_contornoPath.Isok == false) return false;

                _4pointPathReinf.p2 = _4pointPathReinf.p3 + _DireccionBarra * _contornoPath._line.Length;
                _4pointPathReinf.p1 = _4pointPathReinf.p4 + _DireccionBarra * _contornoPath._line.Length;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtenerSegundaLinaPerpendiDireccionBarra' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private List<CoordenadaPath> ObtenerSegundaLinea_DesdeConjuntoDeElemtosDelPathReinforment()
        {
            //buscar la otra linea
            Options gOptions = new Options();
            gOptions.ComputeReferences = true;
            gOptions.DetailLevel = ViewDetailLevel.Undefined;
            gOptions.IncludeNonVisibleObjects = false;

            GeometryElement ConjuntoDeElemtosDelPathReinforment = _pathReinforcement.get_Geometry(gOptions);
            if (ConjuntoDeElemtosDelPathReinforment == null) return new List<CoordenadaPath>();

            List<CoordenadaPath> listapTOS_aux = ConjuntoDeElemtosDelPathReinforment.
                                     Select(obj => new CoordenadaPath
                                     {
                                         p2 = (obj as Line).GetEndPoint(1),
                                         p1 = (obj as Line).GetEndPoint(0)
                                     }).ToList();

            List<CoordenadaPath> listapTOS = ConjuntoDeElemtosDelPathReinforment.
                                            Where(obj => obj.LineaOpuesta(_4pointPathReinf)).
                                            Select(obj => new CoordenadaPath
                                            {
                                                p2 = (obj as Line).GetEndPoint(1),
                                                p1 = (obj as Line).GetEndPoint(0)
                                            }).ToList();
            return listapTOS;
        }

        private contornoPath ObtenerLineaParalelaBarra_DesdeConjuntoDeElemtosDelPathReinforment()
        {
            try
            {
                //buscar la otra linea
                Options gOptions = new Options();
                gOptions.ComputeReferences = true;
                gOptions.DetailLevel = ViewDetailLevel.Undefined;
                gOptions.IncludeNonVisibleObjects = false;

                GeometryElement ConjuntoDeElemtosDelPathReinforment = _pathReinforcement.get_Geometry(gOptions);

                if (ConjuntoDeElemtosDelPathReinforment == null) return new contornoPath() { Isok = false };

                List<contornoPath> listapTOS_aux = ConjuntoDeElemtosDelPathReinforment.
                                         Select(obj => new contornoPath
                                         {
                                             _direccion = (obj as Line).Direction,
                                             _line = (obj as Line),
                                             p_final = (obj as Line).GetEndPoint(1),
                                             p_origen = (obj as Line).GetEndPoint(0),
                                             Isok = true
                                         }).ToList();


                contornoPath _bordeParaleloBarra = listapTOS_aux.Where(c => Util.IsParallel(c._direccion, _DireccionBarra)).FirstOrDefault();

                if (_bordeParaleloBarra == null)

                    return new contornoPath() { Isok = false };
                else
                    return _bordeParaleloBarra;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtenerLineaParalelaBarra_DesdeConjuntoDeElemtosDelPathReinforment' ex:{ex.Message}");
                return new contornoPath() { Isok = false };
            }
        }
    }


    public class contornoPath
    {
        public bool Isok { get; set; }
        public XYZ p_origen { get; set; }
        public XYZ p_final { get; set; }
        public XYZ _direccion { get; set; }
        public Line _line { get; set; }
    }
}
