using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    public class SeleccionarRebarRectangulo
    {

        private UIDocument _uidoc;
        private IList<Element> _listaElementsRebarSeleccionado;
        public List<Element> _ListaRebarSeleccionado { get; set; }
        private List<WrapperRebar> _listaWrapperRebar;
        private List<Element> _listaDeREbarNivelActual;
        private FilteredElementCollector _collectorPathReinfNivelActual;
        private UIApplication _uiapp;

        public SeleccionarRebarRectangulo(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            _listaWrapperRebar = new List<WrapperRebar>();

        }


        public bool GetRebarSeleccionadosConRectaguloYFiltros()
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new RebarSelectionFilter();
                //selecciona un objeto floor
                _listaElementsRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar barra (Rebar) para borrar");
            }
            catch (Exception)
            {
                _listaElementsRebarSeleccionado = new List<Element>();
                return false;
            }
            return true;
        }

        public bool GetUnicamenteRebarSeleccionadosConRectaguloYFiltros()
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new RebarUnicamenteSelectionFilter();
                //selecciona un objeto floor
                _ListaRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar barra (Rebar)").ToList();

                if (_ListaRebarSeleccionado.Count == 0) return false;
            }
            catch (Exception)
            {
                _ListaRebarSeleccionado = new List<Element>();
                return false;
            }
            return true;
        }

        public void BorrarRebarSeleccionado3()
        {
            try
            {

                using (Transaction transaction = new Transaction(_uidoc.Document))
                {

                    transaction.Start("Borrar Rebar-NH");

                    List<Element> listaElemmntosBorrar = new List<Element>();
                    listaElemmntosBorrar.AddRange(_listaElementsRebarSeleccionado);

                    foreach (var item in listaElemmntosBorrar)
                    {
                        if (item is RebarInSystem)
                        {
                            //  _uidoc.Document.Delete(((RebarInSystem)item).SystemId);
                        }
                        else if (item is Rebar || item is RebarContainer)
                        { _uidoc.Document.Delete(item.Id); }

                    }
                    transaction.Commit();

                }
            }
            catch (Exception ex)
            {

                TaskDialog.Show("Revit", ex.Message);
            }
            // borra pelota de losa

        }

        private void ObtenerListaDeREbarNivelActualS()
        {
            ElementClassFilter f1 = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter f2 = new ElementCategoryFilter(BuiltInCategory.OST_Rebar);
            LogicalAndFilter f3 = new LogicalAndFilter(f1, f2);
            _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document);
            //para las familias en el browser
            _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsElementType();
            /*
                     var _llistaDePathReinfNivelActual = collector.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsNotElementType();//para las instacias de familia en el plano
                   int elem =_collectorPathReinfNivelActual.GetElementCount();
            */
            _listaDeREbarNivelActual = _collectorPathReinfNivelActual.ToElements() as List<Element>;


        }

    }
}
