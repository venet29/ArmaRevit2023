using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArmaduraLosaRevit.Model.UTILES
{
    public class CreadorCirculo
    {

        public static List<DetailArc> ListaCirculos { get; set; } = new List<DetailArc>();
        public static DetailArc UltimoCirculoCreado { get; set; }

        ///// <summary>
        ///// crea circulo , para referecnia en 3D
        ///// </summary>
        ///// <param name="d"></param>
        ///// <param name="centro"></param>
        ///// <param name="doc"></param>
        public static ModelArc CrearCirculo_ModelLine(Document _doc, double d, XYZ centro, string graphic_stylesName = "Lines")
        {
            ModelArc arc1 = null;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create circulo-NH");

                    Arc geomArc1 = Arc.Create(centro, d / 2, 0.0, 2.0 * Math.PI, XYZ.BasisX, XYZ.BasisY);

                    // Create a geometry plane in Revit application
                    XYZ origin = centro;
                    XYZ normal = new XYZ(0, 0, 1);
                    Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin); // 2017

                    // Create a sketch plane in current document
                    SketchPlane sketch = SketchPlane.Create(_doc, geomPlane);
                    arc1 = _doc.Create.NewModelCurve(geomArc1, sketch) as ModelArc;

                    Element line_styles_Magenta = TiposLineaPattern.ObtenerTipoLinea(graphic_stylesName, _doc);
                    if (line_styles_Magenta != null)
                        arc1.LineStyle = line_styles_Magenta;

                    t.Commit();
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
            }
            return arc1;
        }

        //en 3D
        public static void CrearCirculo_ModelLine(double d, XYZ centro, UIDocument uidoc, string graphic_stylesName = "ROJO")
        {
            try
            {
                using (Transaction t = new Transaction(uidoc.Document))
                {
                    t.Start("Create circulo2-NH");

                    Arc geomArc1 = Arc.Create(centro, d / 2, 0.0, 2.0 * Math.PI, XYZ.BasisX, XYZ.BasisY);

                    // Create a geometry plane in Revit application
                    XYZ origin = centro;
                    XYZ normal = new XYZ(0, 0, 1);
                    Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin); // 2017

                    // Create a sketch plane in current document
                    SketchPlane sketch = SketchPlane.Create(uidoc.Document, geomPlane);
                    ModelArc arc1 = uidoc.Document.Create.NewModelCurve(geomArc1, sketch) as ModelArc;

                    Element red_line_styles = TiposLineaPattern.ObtenerTipoLinea(graphic_stylesName, uidoc.Document);

                    if (red_line_styles != null) arc1.LineStyle = red_line_styles;
                    // uidoc.Document.Regenerate();
                    t.Commit();
                    //  uidoc.RefreshActiveView();
                }

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }

        //vistas
        public static DetailArc CrearCirculo_DetailLine_ConTrans(double d, XYZ centro, UIDocument uidoc, XYZ direccionX, XYZ DireccionY, string graphic_stylesName = "ROJO", Autodesk.Revit.DB.View view = null)
        {
            DetailArc lineafalsa = null;
            Document _doc = uidoc.Document;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create circulo2-NH");

                    lineafalsa = CrearCirculo_DetailLine_SinTrans(d, centro, uidoc, direccionX, DireccionY, graphic_stylesName, view);
                    //lineafalsa.GeometryCurve();
                    //uidoc.Document.Regenerate();
                    t.Commit();
                    //uidoc.RefreshActiveView();
                }

                funcionAUx_movercirculo(centro, lineafalsa, _doc);

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error en 'CrearCirculo b'  EX:{ex.Message}");
            }

            
            return lineafalsa;
        }

        //25-09-2023
        //funcion creada por el ciruclo se movia cerca del  (0,0,0) cuando l centro estaba en  ()
        private static void funcionAUx_movercirculo(XYZ centro, DetailArc lineafalsa, Document _doc)
        {
            var arcGeom = lineafalsa.GeometryCurve as Arc;
            if (arcGeom != null)
            {
                XYZ center = arcGeom.Center;
                if (center.DistanceTo(centro) > 2)
                {

                    try
                    {
                        using (Transaction t = new Transaction(_doc))
                        {
                            t.Start("CMOver barra-NH");
                            ElementTransformUtils.MoveElement(_doc, lineafalsa.Id, centro.GetXY0() - center.GetXY0());
                            t.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        Util.ErrorMsg("No se puede mover barra");
                    }
                }
            }
        }

        public static DetailArc CrearCirculo_DetailLine_SinTrans(double d, XYZ centro, UIDocument uidoc, XYZ direccionX, XYZ DireccionY, string graphic_stylesName, View view = null)
        {
            DetailArc lineafalsa = default;
            if (view == null)
                view = uidoc.ActiveView;
            try
            {
                Arc geomArc1 = Arc.Create(centro, d / 2, 0.0, 2.0 * Math.PI, direccionX, DireccionY);
                lineafalsa = uidoc.Document.Create.NewDetailCurve(view, geomArc1) as DetailArc;

                Element red_line_styles = TiposLineaPattern.ObtenerTipoLinea(graphic_stylesName, uidoc.Document);

                if (red_line_styles != null && lineafalsa != null) lineafalsa.LineStyle = red_line_styles;
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error en 'CrearCirculo a' EX:{ex.Message}");
            }

            return lineafalsa;
        }

        public static void Crear_ModelArc_ModelLine(double d, XYZ centro, Document doc)
        {
            try
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Create circulo3-NH");


                    Autodesk.Revit.ApplicationServices.Application application = doc.Application;

                    // Create a geometry line in Revit application
                    XYZ startPoint = new XYZ(0, 0, 0);
                    XYZ endPoint = new XYZ(10, 10, 0);
                    Line geomLine = Line.CreateBound(startPoint, endPoint);

                    // Create a geometry arc in Revit application
                    XYZ end0 = new XYZ(1, 0, 0);
                    XYZ end1 = new XYZ(10, 10, 10);
                    XYZ pointOnCurve = new XYZ(10, 0, 0);
                    Arc geomArc = Arc.Create(end0, end1, pointOnCurve);

                    // Create a geometry plane in Revit application
                    XYZ origin = new XYZ(0, 0, 0);
                    XYZ normal = new XYZ(1, 1, 0);
                    Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin); // 2017



                    // Create a sketch plane in current document
                    SketchPlane sketch = SketchPlane.Create(doc, geomPlane);

                    // Create a ModelLine element using the created geometry line and sketch plane
                    ModelLine line = doc.Create.NewModelCurve(geomLine, sketch) as ModelLine;

                    // Create a ModelArc element using the created geometry arc and sketch plane
                    ModelArc arc = doc.Create.NewModelCurve(geomArc, sketch) as ModelArc;

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }


        public static void BorrasCirculosCreado_COntrans(Document _doc)
        {
            try
            {
                if (UltimoCirculoCreado == null) return;
                if (!UltimoCirculoCreado.IsValidObject) return;
                List<DetailArc> listcirc1 = new List<DetailArc>();
                listcirc1.Add(UltimoCirculoCreado);
                BorrasListaCirculos(_doc, listcirc1);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
            UltimoCirculoCreado = null;
        }


        public static void BorrasCirculosCreado_Sintrans(Document _doc)
        {
            try
            {
                if (UltimoCirculoCreado == null) return;
                if (!UltimoCirculoCreado.IsValidObject) return;
                List<DetailArc> listcirc1 = new List<DetailArc>();
                listcirc1.Add(UltimoCirculoCreado);
                _doc.Delete(UltimoCirculoCreado.Id);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
            UltimoCirculoCreado = null;
        }

        public static void BorrasListaCirculos(Document _doc, List<DetailArc> listcirc)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create circulo2-NH");
                    _doc.Delete(UltimoCirculoCreado.Id);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }
    }
}
