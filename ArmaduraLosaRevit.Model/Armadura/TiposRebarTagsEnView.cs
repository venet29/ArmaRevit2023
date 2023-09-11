
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposRebarTagsEnView
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<IndependentTag> ListaRebarTagInView;
        private bool IsOK;

        public TiposRebarTagsEnView(UIApplication _uiapp, View _view)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _view;
            ListaRebarTagInView = new List<IndependentTag>();
        }

        //obtiene todos los tag de la view
        public bool M0_BuscarRebarTagInView()
        {
            IsOK = true;
            try
            {
                FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc, _view.Id);
                //  filteredElementCollector.OfClass(typeof(FamilySymbol));
                filteredElementCollector.OfCategory(BuiltInCategory.OST_RebarTags).WhereElementIsNotElementType();
                ListaRebarTagInView = filteredElementCollector.Cast<IndependentTag>().ToList();                
            }
            catch (Exception)
            {
                IsOK = false;
            }
            return IsOK;
        }

    

        public List<IndependentTag> M1_BuscarEnColecctorPorRebar(ElementId elementid_rebar)
        {
            if (IsOK == false) return ListaRebarTagInView;

            if (ListaRebarTagInView.Count == 0)
                M0_BuscarRebarTagInView();

            var m_roomTagTypes = ListaRebarTagInView.Where(c => c.GetTaggedLocalElementIds().FirstOrDefault() == elementid_rebar).ToList();

            return m_roomTagTypes;
        }

    }
}
