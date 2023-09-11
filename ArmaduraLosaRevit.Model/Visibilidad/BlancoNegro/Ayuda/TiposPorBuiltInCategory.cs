
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda
{
    public class TiposPorBuiltInCategory
    {
        private Document _doc;
        private View _view;
        
        private bool IsOK;
        public List<Element> ListaElemento { get; set; }
        public Dictionary<string,Element> ListaDictionaryElemento { get; private set; }

        public TiposPorBuiltInCategory(Document rvtDoc,View _view)
        {
            this._doc = rvtDoc;
            this._view = _view;
            ListaElemento = new List<Element>();
        }

        public bool cargarListaDeTagRebar(BuiltInCategory _BuiltInCategory)
        {
            IsOK = true;
            try
            {
                FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc, _view.Id);

                //filteredElementCollector.OfCategory(BuiltInCategory.OST_RebarTags).WhereElementIsNotElementType();
                //ListaRebarTagInView = filteredElementCollector.Cast<IndependentTag>().ToList();

                ListaElemento = filteredElementCollector.OfCategory(_BuiltInCategory).WhereElementIsNotElementType().ToList();
            }
            catch (Exception)
            {
                IsOK = false;
            }
            return IsOK;
        }
     
    }
}
