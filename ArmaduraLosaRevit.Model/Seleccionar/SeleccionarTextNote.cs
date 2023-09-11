using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.LosaArmadura.Geometria;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Intervalos;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{

    public class SeleccionarTextNote
    {
        protected Document _doc;
        protected UIDocument _uidoc;
        private View _view;
        protected UIApplication _uiapp;


        public bool _todoBien { get; set; }
        public List<TextNote> ListaTextNote { get; set; }
        public List<TextNote> ListaTextoPorViewYNombre { get; private set; }

        public SeleccionarTextNote(UIApplication uiapp)
        {
            this._doc = uiapp.ActiveUIDocument.Document;
            this._uidoc = uiapp.ActiveUIDocument;
            this._view = this._doc.ActiveView;
            this._uiapp = uiapp;
            _todoBien = true;
            ListaTextNote = new List<TextNote>();
        }


        public bool ObtenerTodosTextNote(View _view)
        {
            try
            {
                _todoBien = true;
                //buscar primer nivel
                FilteredElementCollector Colectornivel = new FilteredElementCollector(_doc, _view.Id);
                ListaTextNote = Colectornivel
                         .OfClass(typeof(TextNote))
                         .OfCategory(BuiltInCategory.OST_TextNotes)
                         .Cast<TextNote>().ToList();

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                _todoBien = false;
            }

            return _todoBien;
        }

        public bool SeleccionarTextNoteConNombre(string name, View _view) 
        {
            try
            {
                if (!ObtenerTodosTextNote(_view)) return false;

                ListaTextoPorViewYNombre =ListaTextNote.Where(c => c.Name == name).ToList();

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                _todoBien = false;
            }

            return _todoBien;
        }
        public bool SeleccionarTextNoteConNombre_mouse(string name)
        {
            try
            {
                _todoBien = true;


                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new FiltroTextNote();

                //selecciona un objeto floor
                var refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element, f, "Seleccionar TextNote");

                if (refs_pickobjects == null) return false;
                if (refs_pickobjects.Count == 0) return false;

                foreach (Reference ref_elem in refs_pickobjects)
                {
                    TextNote Element_pickobject_element = _uidoc.Document.GetElement(ref_elem.ElementId) as TextNote;
                    ListaTextNote.Add(Element_pickobject_element);
                }


            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                _todoBien = false;
            }

            return _todoBien;
        }

    }
}
