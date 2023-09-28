using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    public class SeleccionarPathReinfomentRectangulo
    {
        protected UIApplication _uiapp;
        protected UIDocument _uidoc;
        protected Document _doc;

        protected List<Element> _listaDePathReinfNivelActual;

        protected FilteredElementCollector _collectorPathReinfNivelActual;

        protected PathReinSpanSymbol pathReinSpanSymbol_uno;
        protected IList<Element> pathReinSpanSymbol_Lista;
        public SeleccionarPathReinfomentRectangulo(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
        }


        public bool SeleccionadosMultiplesPathReinConRectaguloYFiltros()
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new PathReinSymbolSelectionFilter();
                //selecciona un objeto floor

                pathReinSpanSymbol_Lista = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar (pathreinforment) barras ");
                //pathReinSpanSymbol_Lista = _uidoc.Selection.PickObjects(ObjectType.Element, f, $"Seleccionar barras   ");
                //pathReinSpanSymbol_Lista = _uidoc.Selection.PickObjects(f, "Seleccionar (pathreinforment) barras ");

                if (pathReinSpanSymbol_Lista.Count > 0)
                {
                    buscarListaPathReinFamiliasEnBrowser();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
        }


        public bool Seleccionados1Path()
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new PathReinSymbolSelection_RebarInsystemFilter();
                //selecciona un objeto floor
                //1
                Reference ref_baar1 = _uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar (pathreinforment) barra ");

                Element elememt = _doc.GetElement(ref_baar1);

                if (elememt is PathReinSpanSymbol)
                    pathReinSpanSymbol_uno = (PathReinSpanSymbol)elememt;
                else if (elememt is RebarInSystem)
                {
                    var rebarInsystem = (RebarInSystem)elememt;
                    var path = _doc.GetElement(rebarInsystem.SystemId) as PathReinforcement;

                    var _ResultPara_nombreVista = ParameterUtil.FindParaByName(path, "NombreVista");
                    if (_ResultPara_nombreVista == null) return false;
                    string _nombreVista=_ResultPara_nombreVista.AsString();
                    var aux_ListaEstructura = SeleccionarView.ObtenerView_losa_elev_fund_estructura(_uiapp.ActiveUIDocument);
                    var _viewConpath=aux_ListaEstructura.Where(c=> c.Name==_nombreVista).FirstOrDefault();
                    if(_viewConpath==null) return false;

                    SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _viewConpath);
                    _SelecPathReinVisibilidad.M1_ejecutar();
                    var elementoEncoantrado=_SelecPathReinVisibilidad._lista_A_VisibilidadElementoPathReinDTO.Where(c=> c._pathReinforcement.Id== path.Id).FirstOrDefault();
                    if(elementoEncoantrado==null) return false;

                    pathReinSpanSymbol_uno = elementoEncoantrado.pathSymbol as PathReinSpanSymbol;
                }

                if (pathReinSpanSymbol_uno != null)
                {
                    pathReinSpanSymbol_Lista = new List<Element>();
                    pathReinSpanSymbol_Lista.Add(pathReinSpanSymbol_uno);
                    buscarListaPathReinFamiliasEnBrowser();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
        }


        public bool BorrarPathReinfSeleccionado()
        {

            try
            {

                if (_listaDePathReinfNivelActual == null) return false;

                List<Element> listaElemmntosBorrar = new List<Element>();

                foreach (var ePathReinSymbol in pathReinSpanSymbol_Lista)
                {

                    if (!(ePathReinSymbol is PathReinSpanSymbol)) return false;

                    var _pathReinSymbol = ePathReinSymbol as PathReinSpanSymbol;
                    Element ePathReinforcement = _pathReinSymbol.Obtener_GetTaggedLocalElement(_uiapp);
                    //obtiene una referencia floor con la referencia r
                    if (!(ePathReinforcement is PathReinforcement)) return false;

                    listaElemmntosBorrar.Add(ePathReinforcement);
                }

                if (listaElemmntosBorrar == null) return false;
                if (listaElemmntosBorrar.Count == 0) return false;

                //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
                UpdateGeneral.M3_DesCargarBarras(_uiapp);
                try
                {
                    using (Transaction transaction = new Transaction(_uidoc.Document))
                    {
                        transaction.Start("Borrar Path-NH");
                        foreach (var item in listaElemmntosBorrar)
                        {
                            if (item.IsValidObject)
                                _doc.Delete(item.Id);
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {

                    TaskDialog.Show("Error al borrar barras", ex.Message);
                }
                UpdateGeneral.M2_CargarBArras(_uiapp);

            }
            catch (Exception)
            {
            }

            return true;
        }


        public IList<ElementId> ObtenerListaIDSeleccionados()
        {
            return _listaDePathReinfNivelActual.Select(c => c.Id).ToList();
        }

        // se borra al path que esta en el browser pq cada path genrea una familia en el browser, y se elimnina ese elemto
        private void buscarListaPathReinFamiliasEnBrowser()
        {
            ElementClassFilter f1 = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter f2 = new ElementCategoryFilter(BuiltInCategory.OST_PathRein);
            LogicalAndFilter f3 = new LogicalAndFilter(f1, f2);
            _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document);
            //para las familias en el browser
            _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsElementType();
            /*
                     var _llistaDePathReinfNivelActual = collector.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsNotElementType();//para las instacias de familia en el plano
                   int elem =_collectorPathReinfNivelActual.GetElementCount();
            */
            _listaDePathReinfNivelActual = _collectorPathReinfNivelActual.ToElements() as List<Element>;


        }

    }
}
