using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Dimensiones.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
    public class CreadorAligneDimensiones
    {
        private  Document _doc;
        private readonly UIApplication _uiapp;
        private  View _view;
        private  Reference ref1;
        private  Reference ref2;

        private  string _nombreTipo;
        private DimensionesDatosTextoDTO _DimensionesDatosTexto;
        private Dimension _dimension;
        private string graphic_stylesLineName;
        private DimensionType _dimensionType;
        private SketchPlane _skplane;

        public DetailLine lineafalsa { get; private set; }
        public Dimension dimension { get; private set; }

        public CreadorAligneDimensiones(UIApplication uiapp)
        {
            this._doc = uiapp.ActiveUIDocument.Document;
            this._uiapp = uiapp;
            this._view = _uiapp.ActiveUIDocument.ActiveView;
        }

        public CreadorAligneDimensiones(UIApplication uiapp, View _view, Reference ref1, Reference ref2, string NombreTipo, DimensionesDatosTextoDTO _DimensionesDatosTexto)//COTA 50 (J.D.)
        {
            this._doc = uiapp.ActiveUIDocument.Document;
            this._uiapp = uiapp;
            this._view = _view;
            this.ref1 = ref1;
            this.ref2 = ref2;
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

                _dimension = CreateLinearDimension_sinTrans(_doc, ref1, ref2, _view);

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

        private bool CAlculosIniciales(string graphic_stylesLineName)
        {
            try
            {
                if (graphic_stylesLineName == "") return true;
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


        private Dimension CreateLinearDimension_sinTrans(Document doc, Reference ref1,  Reference ref2, View _view)
        {
            Dimension dimension = null;
            try
            {

    

                dimension = doc.Create.NewAlignment(doc.ActiveView, ref1, ref2);
                // _doc.Delete(line.GraphicsStyleId); 

            }
            catch (Exception ex)
            {
                dimension = null;
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
            return dimension;
        }

        public Dimension CreateLinearDimensionConrefer_ConTrans(XYZ pt1, XYZ pt2, Reference ref1, Reference ref2, View _view)
        {
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Crear dimension-NH");
                    if (!CreateLinearDimensionConrefer_sinTrans(pt1,  pt2,  ref1,  ref2,  _view))//  CreateLinearDimension_sinTrans(_doc, p1, p2, _doc.ActiveView);
                        trans.RollBack();

                    //dimension.LeaderEndPosition = dimension.LeaderEndPosition - desfase;
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                _dimension = null;
            }

            return _dimension;

        }

        public bool CreateLinearDimensionConrefer_sinTrans( XYZ pt1, XYZ pt2, Reference ref1, Reference ref2, View _view)
        {
           // Dimension dimension = null;
            try
            {
                // Line dummyLine = Line.CreateBound(XYZ.Zero, XYZ.BasisY);
                Line line = Line.CreateBound(pt2, pt1);

   
                // DetailLine dummy = doc.Create.NewDetailCurve(doc.ActiveView, dummyLine) as DetailLine;

                ReferenceArray ra = new ReferenceArray();



                ra.Append(ref1);
                ra.Append(ref2);
                dimension = _doc.Create.NewDimension(_view, line, ra);
                // _doc.Delete(line.GraphicsStyleId); 

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
                return false;
                
            }
            return true;
        }

    }




}
