using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarAreaPath
    {

        private UIDocument _uidoc;
        private IList<Element> _listaElementsRebarSeleccionado;
#pragma warning disable CS0169 // The field 'SeleccionarAreaPath._listaDeREbarNivelActual' is never used
        private List<Element> _listaDeREbarNivelActual;
#pragma warning restore CS0169 // The field 'SeleccionarAreaPath._listaDeREbarNivelActual' is never used
        private FilteredElementCollector _collectorPathReinfNivelActual;
        private UIApplication _uiapp;
        private readonly View _viewObtenerBArras;

        public List<WrapperAreaRein> _lista_A_DeAreaReinVistaActual { get; private set; }

        public SeleccionarAreaPath(UIApplication _uiapp, View _viewObtenerBArras)
        {
            this._uiapp = _uiapp;
            this._viewObtenerBArras = _viewObtenerBArras;
            this._uidoc = _uiapp.ActiveUIDocument;

        }


        public void GetRoomSeleccionadosConRectaguloYFiltros()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new AreaPathSelectionFilter();
            //selecciona un objeto floor
            _listaElementsRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar barra (Rebar) para borrar");

            // if (_listaElementsRebarSeleccionado.Count > 0) ObtenerListaDeREbarNivelActualS();
        }


        public IList<ElementId> ObtenerListaIDSeleccionados()
        {
            return _listaElementsRebarSeleccionado.Select(c => c.Id).ToList();
        }

        public void BorrarAreaPathSeleccionado()
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
                        _uidoc.Document.Delete(item.Id);
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

        public void BuscarListaAreaReinEnVistaActual()
        {
            _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document, _viewObtenerBArras.Id);
            //para las familias en el para las instacias de familia en el plano
            _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_AreaRein).WhereElementIsNotElementType();

            _lista_A_DeAreaReinVistaActual = _collectorPathReinfNivelActual.Select(c =>
                                        new WrapperAreaRein()
                                        {
                                            element = c,
                                            NombreView = NewMethod(c),
                                            ListaRebarInsistem = (c as AreaReinforcement).GetRebarInSystemIds()
                                        }).ToList();
        }
      
        private  string NewMethod(Element c)
        {
            var name = c.LookupParameter("NombreVista").AsString();
            if (name == null) return  "sin";
            if (name == "") return "sin";
            return name;
        }
    }
}
