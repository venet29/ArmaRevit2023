using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Servicios;
using ArmaduraLosaRevit.Model.FILTROS;
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


    public class SeleccionarPasadasConMouse
    {
        private Document _doc;
        private UIDocument _uidoc;
        private readonly UIApplication _uiapp;
        private bool isTest;
        // private Floor _selecFloorMouse;
        public XYZ PtoMOuse { get; set; }
        public bool _todoBien { get;  set; }
        public List<Element> ListaPAsadas { get; private set; }

        public SeleccionarPasadasConMouse(UIApplication _UIapp, bool isTest = false)
        {
            this._uiapp = _UIapp;
            this._doc = _UIapp.ActiveUIDocument.Document;
            this._uidoc = _UIapp.ActiveUIDocument;            
            this.isTest = isTest;
            _todoBien = true;
        }


        public bool M2_SelecconaShaftOpeningRectagulo()
        {
            try
            {
                ISelectionFilter f = new FiltroPasadaRectagular_Contener();

                ListaPAsadas = new List<Element>();                
                ListaPAsadas = _uidoc.Selection.PickElementsByRectangle( f, "Seleccionar Pasada Rectagula").ToList();

                return true;
                
                var refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element, f, "Seleccionar Pasada Rectagula");
                if (refs_pickobjects == null) return false;
                if (refs_pickobjects.Count == 0) return false;

                foreach (Reference ref_elem in refs_pickobjects)
                {
                    Element Element_pickobject_element = _uidoc.Document.GetElement(ref_elem.ElementId);
                    ListaPAsadas.Add(Element_pickobject_element);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool M1_SeleccionarPAsadas()
        {
            try
            {
                ISelectionFilter f = new FiltroPasadaRectagular_Contener();

                ListaPAsadas = new List<Element>();
 
                var refs_pickobjects = _uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar pasada");

                if (refs_pickobjects == null) return false;

                Element Element_pickobject_element = _uidoc.Document.GetElement(refs_pickobjects.ElementId);
             
                ListaPAsadas.Add(Element_pickobject_element);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }


    }
}
