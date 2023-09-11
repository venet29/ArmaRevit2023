using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Dimensiones.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Dimensiones
{

    //para agregar texto
    //https://boostyourbim.wordpress.com/2019/03/06/add-additional-way-to-get-to-dimension-text-dialog-to-edit-dimensions-text/
    public class CreadorDimensiones
    {
        private readonly Document _doc;
        private readonly XYZ p1;
        private readonly XYZ p2;
        private readonly string _nombreTipo;
        private readonly DimensionesDatosTextoDTO _DimensionesDatosTexto;
        private Dimension _dimension;
        private string graphic_stylesLineName;
        private DimensionType _dimensionType;
        private SketchPlane _skplane;

        public DetailLine lineafalsa { get; private set; }

        public CreadorDimensiones(Document doc, XYZ p1, XYZ p2, string NombreTipo)//COTA 50 (J.D.)
        {
            this._doc = doc;
            this.p1 = p1;
            this.p2 = p2;
            this._nombreTipo = NombreTipo;
            this._skplane = _doc.ActiveView.SketchPlane;
            this._DimensionesDatosTexto = new DimensionesDatosTextoDTO();
            // this.view
        }

        public CreadorDimensiones(Document doc, XYZ p1, XYZ p2, string NombreTipo, DimensionesDatosTextoDTO _DimensionesDatosTexto)//COTA 50 (J.D.)
        {
            this._doc = doc;
            this.p1 = p1;
            this.p2 = p2;
            this._nombreTipo = NombreTipo;
            this._DimensionesDatosTexto = _DimensionesDatosTexto;
            this._skplane = _doc.ActiveView.SketchPlane;
            // this.view
       
        }


        public Dimension Crear_conTrans(string graphic_stylesLineName = "")
        {
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Crear dimension-NH");
                    if (!Crear_sintrans(graphic_stylesLineName))//  CreateLinearDimension_sinTrans(_doc, p1, p2, _doc.ActiveView);
                        trans.RollBack();
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                _dimension = null;
            }

            return _dimension;

        }


        public bool Crear_sintrans(string graphic_stylesLineName = "")
        {
            try
            {
                if (!CAlculosIniciales(graphic_stylesLineName)) return false;

                _dimension = CreateLinearDimension_sinTrans(_doc, p1, p2, _doc.ActiveView);

                if (_DimensionesDatosTexto.IsSobreEscribir)
                {
                    _dimension.Above = _DimensionesDatosTexto.Above;
                    _dimension.Below = _DimensionesDatosTexto.Below;
                    _dimension.ValueOverride = _DimensionesDatosTexto.ValueOverride;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;

        }
        public bool CrearConRef_sintrans(Reference ref1, Reference ref2, string graphic_stylesLineName = "")
        {
            try
            {
                if (!CAlculosIniciales(graphic_stylesLineName)) return false;

                _dimension = CreateLinearDimensionConrefer_sinTrans(_doc, p1, p2, ref1, ref2, _doc.ActiveView);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
            }
            return true;

        }

        private bool CAlculosIniciales(string graphic_stylesLineName)
        {
            try
            {
                this.graphic_stylesLineName = graphic_stylesLineName;
                this._dimensionType = SeleccionarDimensiones.ObtenerDimensionTypePorNombre(_doc, _nombreTipo);

                if (this._dimensionType == null)
                {
                    Util.ErrorMsg("Familia de Dimension No encontrada");
                    return false;
                }
                if (_skplane == null)
                {
                    Util.ErrorMsg("SketchPlane No encontrada");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message} ");
                return false;
            }
            return true;
        }


        private Dimension CreateLinearDimension_sinTrans(Document doc, XYZ pt1, XYZ pt2, View _view)
        {
            Dimension dimension = null;
            try
            {

                Line line = Line.CreateBound(pt1, pt2);
                // Line dummyLine = Line.CreateBound(XYZ.Zero, XYZ.BasisY);

                lineafalsa = _doc.Create.NewDetailCurve(_view, line) as DetailLine;

                if (graphic_stylesLineName != "")
                {
                    Element line_styles_LineaDim = TiposLineaPattern.ObtenerTipoLinea(graphic_stylesLineName, _doc);
                    if (line_styles_LineaDim != null)
                        lineafalsa.LineStyle = line_styles_LineaDim;

                }

                // DetailLine dummy = doc.Create.NewDetailCurve(doc.ActiveView, dummyLine) as DetailLine;

                ReferenceArray ra = new ReferenceArray();
                ra.Append(lineafalsa.GeometryCurve.GetEndPointReference(0));
                ra.Append(lineafalsa.GeometryCurve.GetEndPointReference(1));
                dimension = doc.Create.NewDimension(doc.ActiveView, line, ra, this._dimensionType);
                // _doc.Delete(line.GraphicsStyleId); 

            }
            catch (Exception ex)
            {
                dimension = null;
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
            return dimension;
        }

        public Dimension CrearConref_conTrans(string textoAbovoe, string replaceWithText, string textobelow, Reference ref1, Reference ref2, string graphic_stylesLineName = "")
        {
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Crear dimension-NH");
                    if (CrearConRef_sintrans(ref1, ref2, graphic_stylesLineName))//  CreateLinearDimension_sinTrans(_doc, p1, p2, _doc.ActiveView);
                    {
                        if (textoAbovoe != null) _dimension.Above = textoAbovoe;
                        if (replaceWithText != "") _dimension.ValueOverride = replaceWithText;

                        if (textobelow != "") _dimension.Below = textobelow;

                    }
                    else
                        trans.RollBack();
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                _dimension = null;
            }

            return _dimension;

        }

        private Dimension CreateLinearDimensionConrefer_sinTrans(Document doc, XYZ pt1, XYZ pt2, Reference ref1, Reference ref2, View _view)
        {
            Dimension dimension = null;
            try
            {
                pt1 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(_view.ViewDirection, _view.Origin, pt1) + -_view.ViewDirection * 0.01;
                pt2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(_view.ViewDirection, _view.Origin, pt2) + -_view.ViewDirection * 0.01;
                // Line dummyLine = Line.CreateBound(XYZ.Zero, XYZ.BasisY);
                Line line = Line.CreateBound(pt2, pt1);

                if (true)
                {
                    lineafalsa = _doc.Create.NewDetailCurve(_view, line) as DetailLine;

                    if (graphic_stylesLineName != "")
                    {
                        Element line_styles_LineaDim = TiposLineaPattern.ObtenerTipoLinea(graphic_stylesLineName, _doc);
                        if (line_styles_LineaDim != null)
                            lineafalsa.LineStyle = line_styles_LineaDim;

                    }
                }
                // DetailLine dummy = doc.Create.NewDetailCurve(doc.ActiveView, dummyLine) as DetailLine;

                ReferenceArray ra = new ReferenceArray();



                ra.Append(lineafalsa.GeometryCurve.GetEndPointReference(0));
                ra.Append(lineafalsa.GeometryCurve.GetEndPointReference(1));
                dimension = doc.Create.NewDimension(doc.ActiveView, line, ra, _dimensionType);
                // _doc.Delete(line.GraphicsStyleId); 

            }
            catch (Exception ex)
            {
                dimension = null;
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
            return dimension;
        }



    }


}
