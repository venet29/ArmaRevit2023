using System;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraV.AgregarEspMuro
{
    internal class CreadorAgregarEspesor
    {
        private UIApplication _uiapp;
        private Document _doc;
        private ViewSection _view;
        private static Element IndependentTagPathTagMuroCorto_50;
        private static Element IndependentTagPathTagMuroCorto_75;
        private static Element IndependentTagPathTagMuroCorto_100;

        public CreadorAgregarEspesor(UIApplication uiapp, ViewSection view)
        {
            _uiapp = uiapp;
            _doc = uiapp.ActiveUIDocument.Document;
            _view = view;
        }

        public void AgregarEsp()
        {
            try
            {
                if (IndependentTagPathTagMuroCorto_50 == null)
                    IndependentTagPathTagMuroCorto_50 = TiposWallTagsEnView.cargarListaDetagWall2(_doc,  "12mm_50", "TAG MUROS (CORTO)_elev");
                if (IndependentTagPathTagMuroCorto_75 == null)
                    IndependentTagPathTagMuroCorto_75 = TiposWallTagsEnView.cargarListaDetagWall2(_doc,  "12mm_75", "TAG MUROS (CORTO)_elev");
                if (IndependentTagPathTagMuroCorto_100 == null)
                    IndependentTagPathTagMuroCorto_100 = TiposWallTagsEnView.cargarListaDetagWall2(_doc,  "12mm_100", "TAG MUROS (CORTO)_elev");

                if (IndependentTagPathTagMuroCorto_50 == null || IndependentTagPathTagMuroCorto_75 == null || IndependentTagPathTagMuroCorto_100 == null) return;

                var listatagPilar = SeleccionarTagPilar.GetTagPilar(_uiapp.ActiveUIDocument.Document, _view);
                if (listatagPilar.Count == 0) return;

                foreach (IndependentTag item in listatagPilar)
                {
                    XYZ ptoinserccion = item.TagHeadPosition;
                    Element elem= item.Obtener_GetTaggedLocalElement(_uiapp);
                    if (elem==null) continue;
                    Rebar _rebarHost = elem as Rebar;
                    if (_rebarHost == null) continue;

                    var nombreBarraTipo = ParameterUtil.FindParaByName(_rebarHost.Parameters, "BarraTipo").AsString();

                    if (nombreBarraTipo != "ELEV_ES") continue;

                    ElementId _elemtoIdMuro = _rebarHost.GetHostId();
                    if (_elemtoIdMuro == null) continue;

                    Wall _wall = _doc.GetElement(_elemtoIdMuro) as Wall;
                    if (_wall == null) continue;

                    DibujarTagPathReinforment(_wall, _doc, _view, ptoinserccion + new XYZ(0, 0, Util.CmToFoot(70)));
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        public void DibujarTagPathReinforment(Element elementMuro, Document doc, View viewActual, XYZ posicion)
        {
            Element tagslecc = null;

            if (viewActual.Scale == 50)
                tagslecc = IndependentTagPathTagMuroCorto_50;
            else if (viewActual.Scale == 75)
                tagslecc = IndependentTagPathTagMuroCorto_75;
            else if (viewActual.Scale == 100)
                tagslecc = IndependentTagPathTagMuroCorto_100;
            IndependentTag independentTag = IndependentTag.Create(doc, tagslecc.Id, viewActual.Id, new Reference(elementMuro), false,
                                                      TagOrientation.Vertical, XYZ.Zero); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicion;
        }
    }
}