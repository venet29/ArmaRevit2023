using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Bim.DTO;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.Bim.Model
{
    public class TagBAseBim
    {
        Document _doc;
        private View _view;
        private int? _escala;

        private readonly UIApplication _uiapp;

  

        public XYZ posicion { get; set; }
        // public XYZ _TagHeadPosition { get; set; }
        public XYZ PtoCodo_LeaderElbow { get; set; }
        public XYZ Ptocodo_LeaderEnd { get; set; }
  
  
        public Element ElementIndependentTagPath { get; set; }
        public bool IsOk { get; set; } = true;
        public bool IsDIrectriz { get; set; } = true;
        public bool IsLibre { get; set; }

        public TagBAseBim(UIApplication _uiapp, Element _ElementIndependentTagPath)
        {
            this._uiapp = _uiapp;

            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.posicion = posicion;

            this.ElementIndependentTagPath = _ElementIndependentTagPath;
            this.IsLibre = false;

            _escala = _view.Scale;//  ObtenerNombre_EscalaConfiguracion();
        }



        public void DibujarTag(Element element,ConfiguracionTagDTO conftag)
        {
            if (!IsOk) return;
            IndependentTag independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, _view.Id, new Reference(element), conftag.IsDIrectriz,
                                                      conftag.tagOrientation, conftag.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = conftag.TagHeadPosition;

            AgregarDirectriz(conftag, independentTag);

        }



        private void AgregarDirectriz(ConfiguracionTagDTO confTag, IndependentTag independentTag)
        {
            if (!confTag.IsDIrectriz) return;

            if (!IsDIrectriz) return;

            if (independentTag == null) return;
            independentTag.Set_LeaderEnd(confTag.LeaderElbow);

            FamilySymbol tagSymbol = _doc.GetElement(independentTag.GetTypeId()) as FamilySymbol;

            string nombreFlecha = ConstBim.nombreFlecha;//  "Filled Dot 2mm_" + _escala
            var elem = Tipos_Arrow.ObtenerArrowheadPorNombre(_doc, nombreFlecha);
            if (elem != null)
                tagSymbol.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(elem.Id);

        }


    }

}
