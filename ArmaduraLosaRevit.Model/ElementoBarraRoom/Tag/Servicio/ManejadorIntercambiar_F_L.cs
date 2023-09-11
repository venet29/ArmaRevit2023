using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Servicio
{
    public class ManejadorIntercambiar_F_L
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public ManejadorIntercambiar_F_L(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public bool Ejecutar()
        {

            try
            {

                SeleccionarPathSymbol _SeleccionarPathSymbol = new SeleccionarPathSymbol(_uiapp);
                if (!_SeleccionarPathSymbol.Seleccionados1Path()) return false;



                PathReinSpanSymbol _independentTag = _SeleccionarPathSymbol.PathReinforcementSymbol;
                if (_independentTag == null) return false;

                PathReinforcement PathRein = _doc.GetElement(_independentTag.Obtener_GetTaggedLocalElementID()) as PathReinforcement;
                if (PathRein == null) return false;

                List<IndependentTag> listaIndependentTag = TiposPathReinTagsEnView.M1_GetFamilySymbol_ConPathReinforment(PathRein.Id, _doc, _doc.ActiveView.Id);
                if (listaIndependentTag.Count == 0) return false;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("MoverFL");

                    var TagP0_F = listaIndependentTag.Where(c => c.Name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_F_")).FirstOrDefault();
                    var TagP0_L = listaIndependentTag.Where(c => c.Name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_L_")).FirstOrDefault();
                    var TagP0_F2 = listaIndependentTag.Where(c => c.Name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_F2_")).FirstOrDefault();
                    var TagP0_L2 = listaIndependentTag.Where(c => c.Name.Contains("M_Path Reinforcement Tag(ID_cuantia_largo)_L2_")).FirstOrDefault();


                    if (TagP0_F != null && TagP0_L != null)
                    {
                        XYZ ptF = TagP0_L.TagHeadPosition-TagP0_F. TagHeadPosition;
                        XYZ ptL = TagP0_F.TagHeadPosition- TagP0_L.TagHeadPosition;

                        ElementTransformUtils.MoveElement(_doc, TagP0_F.Id, ptF);

                        ElementTransformUtils.MoveElement(_doc, TagP0_L.Id, ptL);
                    }

                    if (TagP0_F2 != null && TagP0_L2 != null)
                    {
                        XYZ ptF = TagP0_L2.TagHeadPosition- TagP0_F2.TagHeadPosition;
                        XYZ ptL = TagP0_F2.TagHeadPosition-TagP0_L2.TagHeadPosition;

                        ElementTransformUtils.MoveElement(_doc, TagP0_F2.Id, ptF);
                        ElementTransformUtils.MoveElement(_doc, TagP0_L2.Id, ptL);
                    }

                    t.Commit();
                }
                //ManejarEditTagUpdate_MoverInterCAmbiar_F_L _ManejarEditTagUpdate_move = new ManejarEditTagUpdate_MoverInterCAmbiar_F_L(_uiapp, PathRein, _independentTag.TagHeadPosition, listaIndependentTag);
                //_ManejarEditTagUpdate_move.Ejecutar();

            }
#pragma warning disable CS0168 // The variable 'Ex' is declared but never used
            catch (Exception Ex)
#pragma warning restore CS0168 // The variable 'Ex' is declared but never used
            {

                throw;
            }
            return true;
        }
    }
}
