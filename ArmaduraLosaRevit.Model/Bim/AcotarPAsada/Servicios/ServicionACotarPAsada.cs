using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Casos;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GEOM;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Servicios
{
    internal class ServicionACotarPAsada
    {
        private UIApplication _uiapp;
        private readonly EnumPasadas _tipoDireccion_DimensionCentral;
        private readonly DimensionType _dimensionType;
        private Document _doc;
        private View _view;

        public ServicionACotarPAsada(UIApplication uiapp, EnumPasadas tipoDireccion_DimensionCentral, DimensionType _dimensionType)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._tipoDireccion_DimensionCentral = tipoDireccion_DimensionCentral;
            this._dimensionType = _dimensionType;

        }


        public void AcotarRectangular(EnvoltoriPasada envoltoriPasada)
        {
            try
            {
                Dimension dimension1 = default;
                if (envoltoriPasada == null) return;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("dibujar Traslapo-NH");

                    var dim1Planar = envoltoriPasada.parBordeParalelo1;

                    bool result = false;
                    switch (_tipoDireccion_DimensionCentral)
                    {
                        case EnumPasadas.Izquieda:
                            result = dim1Planar.M1_ObtenerHaciaIzq();
                            break;
                        case EnumPasadas.Derecha:
                            result = dim1Planar.M2_ObtenerHaciaDere();
                            break;
                        case EnumPasadas.Arriba:
                            result = dim1Planar.M4_ObtenerHaciaSup();
                            break;
                        case EnumPasadas.Bajo:
                            result = dim1Planar.M3_ObtenerHaciaInferior();
                            break;

                        default:
                            break;
                    }

                    if (result)
                    {
                        Line line = default;
                        ReferenceArray ra = new ReferenceArray();

                        XYZ P1 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, dim1Planar.Pt1);
                        XYZ P2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, dim1Planar.Pt2);


                        line = Line.CreateBound(P1, P2);
                        var ref1 = dim1Planar.face_referencia_IzqInf.Reference;
                        var ref2 = dim1Planar.face_referencia_DereSup.Reference;

                        ra.Append(ref1);
                        ra.Append(ref2);
                        dimension1 = _doc.Create.NewDimension(_doc.ActiveView, line, ra, _dimensionType);


                        ElementTransformUtils.MoveElement(_doc, dimension1.Id, dim1Planar.face_analizada.FaceNormal * 1.5);
                        //dimension1.TextPosition = dim1Planar.Pt_texto;
                        //dimension1.LeaderEndPosition = dim1Planar.Pt_leader;
                        //dimension1.Origin = dim1Planar.Pt_origin;
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        internal void AcotarRectangularYGrilla(EnvoltoriPasada envoltoriPasada, EnumPasadasConGrilla enumPasadasConGrilla_)
        {
            try
            {

                if (envoltoriPasada == null) return;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("dibujar Traslapo-NH");

                    var dim1Planar = envoltoriPasada.parBordeParalelo1;
                    IAcotarCasos _acotar = null;
       
                    switch (enumPasadasConGrilla_)
                    {
                        case EnumPasadasConGrilla.Izquieda_inf:
                            _acotar = new AcotarIzqInf(_uiapp, envoltoriPasada, _dimensionType);
                            break;
                        case EnumPasadasConGrilla.Izquieda_sup:
                            _acotar = new AcotarIzqSup(_uiapp, envoltoriPasada, _dimensionType);
                            break;
                        case EnumPasadasConGrilla.Derecha_inf:
                            _acotar = new AcotarDereInf(_uiapp, envoltoriPasada, _dimensionType);
                            break;
                        case EnumPasadasConGrilla.Derecha_sup:
                            _acotar = new AcotarDereSup(_uiapp, envoltoriPasada, _dimensionType);

                            break;
                        case EnumPasadasConGrilla.Arriba_dere:
                            _acotar = new AcotarArribaDere(_uiapp, envoltoriPasada, _dimensionType);

                            break;
                        case EnumPasadasConGrilla.Arriba_izq:

                            _acotar = new AcotarArribaIzq(_uiapp, envoltoriPasada, _dimensionType);
                            break;
                        case EnumPasadasConGrilla.Bajo_dere:
                            _acotar = new AcotarBajoDere(_uiapp, envoltoriPasada, _dimensionType);
                            break;
                        case EnumPasadasConGrilla.Bajo_izq:
                            _acotar = new AcotarBajoIzq(_uiapp, envoltoriPasada, _dimensionType);
                            break;

                        default:
                            break;
                    }

                    //_envoltoriosPlanos = ListaPLanosPAsadas_aux.FirstOrDefault();

                    //if (!result) return;

                    _acotar.Ejecutar();

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        internal void AcotarCircular(EnvoltoriPasada envoltoriPasada)
        {
            try
            {
                Dimension dimension1 = default;
                if (envoltoriPasada == null) return;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("dibujar Traslapo-NH");

                    var dim1Planar = envoltoriPasada.parBordeParalelo1;

                    bool result = dim1Planar.M5_ObtenerRadial();


                    if (result)
                    {

                        ReferenceArray ra = new ReferenceArray();
                        Arc arc = default;
                        XYZ P1 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, dim1Planar.face_analizada.ObtenerCenterDeCara());
                        // XYZ P2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, dim1Planar.Pt2);

                        Reference ref1 = default;
                        Reference ref2 = default;
                        foreach (EdgeArray edgeArr in dim1Planar.face_analizada.EdgeLoops)
                        {
                            List<Edge> edgesList = new List<Edge>();
                            foreach (Edge edge in edgeArr)
                            {
                                if (ref1 == null)
                                {
                                    arc = (Arc)edge.AsCurve();
                                    ref1 = edge.Reference;
                                    continue;
                                }
                                if (ref2 == null)
                                {
                                    ref2 = edge.Reference;
                                    continue;
                                }
                            }
                        }


                        //var ref1 = dim1Planar.face_referencia_1.Reference;
                        //var ref2 = dim1Planar.face_referencia_2.Reference;
                        //var ref1 = dim1Planar.ListaPLAnar_paraReferencias.Where(c => c.FaceNormal.Z > 0).FirstOrDefault();
                        //if (ref1 == null) return;
                        ra.Append(ref1);
                        ra.Append(ref2);

                        _doc.FamilyCreate.NewAngularDimension(_doc.ActiveView, arc, ref1, ref2, _dimensionType);



                        //ElementTransformUtils.MoveElement(_doc, dimension1.Id, dim1Planar.face_analizada.FaceNormal * 1.5);
                        //dimension1.TextPosition = dim1Planar.Pt_texto;
                        //dimension1.LeaderEndPosition = dim1Planar.Pt_leader;
                        //dimension1.Origin = dim1Planar.Pt_origin;
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

    }
}
