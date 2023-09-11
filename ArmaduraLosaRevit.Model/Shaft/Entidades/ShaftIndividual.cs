using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Shaft.Entidades
{
    public class ShaftIndividual : IShaftIndividual
    {
        private CurveLoop _curveLoop;
        private Level LevelShaft;
        private UIApplication _uiapp;
        private UtilitarioFallasAdvertencias _utilfallas;
        private Element line_styles_LineasVAcios;

        public List<XYZ> vertices { get; set; }
        public List<Line> Lineas { get; set; }

        public bool IsOk { get; set; }
        public UV[] verticesUV { get; set; }
        public Document _doc { get; }

        public ShaftIndividual(Document _doc, CurveLoop curveLoop, Level LevelShaft)
        {
            this._doc = _doc;
            this._curveLoop = curveLoop;
            this.LevelShaft = LevelShaft;
            this.vertices = new List<XYZ>();
            this.Lineas = new List<Line>();
            this.verticesUV = new UV[curveLoop.NumberOfCurves()];
            this.IsOk = false;
           
         //   _utilfallas = new UtilitarioFallasAdvertencias();
            M0_GenerarListaPtos();
          
        }

        public ShaftIndividual()
        {
            this.IsOk = false;
        }

        private void M0_GenerarListaPtos()
        {
            int cont = 0;
            foreach (Curve _curve in _curveLoop)
            {
                Lineas.Add(Line.CreateBound(_curve.GetEndPoint(0), _curve.GetEndPoint(1)));
                XYZ ptoinicial = _curve.GetEndPoint(0);


                ptoinicial = new XYZ(ptoinicial.X, ptoinicial.Y, LevelShaft.ProjectElevation);

                if (!vertices.Contains(ptoinicial)) vertices.Add(ptoinicial);

                UV nuevoUV = new UV(ptoinicial.X, ptoinicial.Y);
                if (!verticesUV.Contains(nuevoUV)) verticesUV[cont] = nuevoUV;
                cont += 1;
            }

        }

        public bool IsPtoDentroShaf(XYZ ptomouse)
        {
            UV nuevoUV = new UV(ptomouse.X, ptomouse.Y);
            PointInPoly _PointInPoly = new PointInPoly();
            IsOk = _PointInPoly.PolygonContains(verticesUV, nuevoUV);
            return IsOk;
        }


        public bool M2_IsMAs2Ptos()
        {
            if (vertices == null) return false;
            return (vertices.Count > 2 ? true : false);
        }
        public void M3_CrearSeparacionRoom(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            if (vertices.Count > 0)
            {
                //_doc.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                UtilitarioFallasDialogosCarga.Cargar(_uiapp);
                UtilitarioFallasDialogosCarga.Cargar_SuprimirCuadroDialogo(_uiapp);               
              
                foreach (var _linea in Lineas)
                {
                    try
                    {
                        using (Transaction trans = new Transaction(_doc))
                        {

                            trans.Start("creaSeparateShaft-NH");

                            List<Line> listaIndivudual = new List<Line>();
                            listaIndivudual.Add(_linea);

                            CurveArray cArray = new CurveArray();
                            foreach (Curve item in listaIndivudual)
                            {

                                cArray.Append(item);
                            }

                            SketchPlane skP = SketchPlane.Create(_doc, LevelShaft.Id);
                            var linea = _doc.Create.NewRoomBoundaryLines(skP, cArray, _doc.ActiveView);

                            // cambiar paramatro PHASE_CREATED =Existing
                            foreach (var item in linea)
                            {
                                Element line_e = item as Element;
                                // get the phase id "New construction"
                                if (!TipoPhases_.ObtenerFasesExistenete(_doc)) return;
                                ElementId idPhase = TipoPhases_.idPhase;

                                line_e.get_Parameter(BuiltInParameter.PHASE_CREATED).Set(idPhase);
                            }

                            trans.Commit();
                        }
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {

                        // message = ex.Message;

                    }
                }
                UtilitarioFallasDialogosCarga.DesCargar_SuprimirCuadroDialogo(_uiapp);
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);


            }



        }

        public void M4_CrearCruz()
        {
            if (vertices.Count != 4) return;
          
            M4_1_ObtenerLineStyle_CRUZ_SHAFT();

            //_doc.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            UtilitarioFallasDialogosCarga.Cargar_SuprimirCuadroDialogo(_uiapp);
            if (M4_2_CreadorCruz(vertices[0], vertices[1], vertices[2], vertices[3])) return;
            if (M4_2_CreadorCruz(vertices[0], vertices[2], vertices[1], vertices[3])) return;
            if (M4_2_CreadorCruz(vertices[0], vertices[3], vertices[1], vertices[2])) return;
            //_doc.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            UtilitarioFallasDialogosCarga.DesCargar_SuprimirCuadroDialogo(_uiapp); ;
        }

        private void M4_1_ObtenerLineStyle_CRUZ_SHAFT()
        {

             line_styles_LineasVAcios = TiposLineaPattern.ObtenerTipoLinea("CRUZ_SHAFT", _doc);
    
            if (line_styles_LineasVAcios == null)
            {
                CrearLineStyle CrearLineStyle = new CrearLineStyle(_doc, "CRUZ_SHAFT", 1, new Color(218, 218, 218), "Dash");
                CrearLineStyle.CreateLineStyleConTrans();

                line_styles_LineasVAcios = TiposLineaPattern.ObtenerTipoLinea("CRUZ_SHAFT", _doc);

            }
        }

        private bool M4_2_CreadorCruz(XYZ a1, XYZ a2, XYZ b1, XYZ b2)
        {
            Line line1 = Line.CreateBound(a1, a2);
            Line line2 = Line.CreateBound(b1, b2);

            XYZ result = Util.Intersection2(line1, line2);

            if (!result.IsAlmostEqualTo(XYZ.Zero))
            {
                Creator creator = new Creator(_uiapp);
                try
                {
                    using (Transaction trans = new Transaction(_doc))
                    {

                        trans.Start("CreaCruzShaft-NH");
                        ModelCurve m1 = creator.CreateModelCurve(line1);

                       // ModelLine m1 = Creator.CreateModelLine(_doc, a1, a2);
                        if (line_styles_LineasVAcios != null) m1.LineStyle = line_styles_LineasVAcios;

                        ModelCurve m2 = creator.CreateModelCurve(line2);
                       // ModelLine m2 = Creator.CreateModelLine(_doc, b1, b2);
                        if (line_styles_LineasVAcios != null) m2.LineStyle = line_styles_LineasVAcios;

                        CrearLineStyle.ReadElementOverwriteLinePattern_sinTrasn(m1.LineStyle, _doc);
                        CrearLineStyle.ReadElementOverwriteLinePattern_sinTrasn(m2.LineStyle, _doc);

                        trans.Commit();
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                    // message = ex.Message;

                }

                return true;
            }
            return false;
        }
      

    }
}
