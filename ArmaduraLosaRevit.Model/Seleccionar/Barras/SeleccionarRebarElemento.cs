using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.RebarCopia.Entidades;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    public class SeleccionarRebarElemento
    {

        private UIDocument _uidoc;
        private Reference _ElementsRebarSeleccionado;

        private UIApplication _uiapp;
        private Document _doc;
        private  View _viewObtenerBArras;

        public Rebar _RebarSeleccion { get; set; }
        public XYZ _ptoRebarSeleccion { get; set; }
        public List<WrapperRebar> _lista_A_DeRebarVistaActual { get; set; }
        public List<WrapperRebar> _lista_A_DePathReinfVistaActual { get; set; }
        public List<ElementId> ListaBarraEstriboLosa { get; private set; }
        public List<ElementId> ListaBarraRebarComoPAthLosa { get; private set; }
        public List<RebarCopiarDTo> ListaRebarSimple { get; private set; }

        private FilteredElementCollector _collectorRebarViewActual;


        public SeleccionarRebarElemento(UIApplication _uiapp, View _viewObtenerBArras)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._viewObtenerBArras = _viewObtenerBArras;
            this._uidoc = _uiapp.ActiveUIDocument;

        }


        public bool GetSelecionarRebar()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            ISelectionFilter f = new RebarUnicamenteSelectionFilter();
            //selecciona un objeto floor

            try
            {
                _ElementsRebarSeleccionado = _uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar barra (Rebar)");

                if (_ElementsRebarSeleccionado == null) return false;
                //obtiene una referencia floor con la referencia r
                Element Element_pickobject_element = _uidoc.Document.GetElement(_ElementsRebarSeleccionado.ElementId);

                if (!(Element_pickobject_element is Rebar)) return false;

                _RebarSeleccion = Element_pickobject_element as Rebar;

                if (_RebarSeleccion.Pinned)
                {
                    Util.ErrorMsg($"Barra tiene Pin asigando, no es posible editar. ");
                    return false;
                }

                _ptoRebarSeleccion = _ElementsRebarSeleccionado.GlobalPoint;
                //  PuntoSeleccionMouse = Util.PtoDeLevelDeGlobalPoint(referen.GlobalPoint, _uidoc.Document);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M1_Ejecutar()
        {
            try
            {

                BuscarListaRebarEnVistaActual();

                 ListaBarraEstriboLosa = _lista_A_DeRebarVistaActual
                                                       .Where(c => c != null && (c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_EST_REF_LO ||
                                                                                c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_EST_BORDE ||
                                                                                c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_ES))
                                                       .Select(c => c.element.Id).ToList();

                ListaBarraRebarComoPAthLosa = _lista_A_DeRebarVistaActual
                                            .Where(c => c != null && (c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_EST_REF_LO ||
                                                                     c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_EST_BORDE ||
                                                                     c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_ES))
                                            .Select(c => c.element.Id).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool BuscarListaRebarEnVistaActual()
        {

            //nota cambiar esta opcion con clasee :'SeleccionarRebarVisibilidad' 
            try
            {
                _collectorRebarViewActual = new FilteredElementCollector(_uidoc.Document, _viewObtenerBArras.Id);
                //para las familias en el para las instacias de familia en el plano
                _collectorRebarViewActual.OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsNotElementType();

                _lista_A_DeRebarVistaActual = _collectorRebarViewActual.Where(c=> c is Rebar).Select(c =>
                                            new WrapperRebar()
                                            {
                                                element = c,
                                                NombreView = AyudaObtenerParametros.ObtenerNombreView(c),
                                                ObtenerTipoBarra = AyudaObtenerParametros.ObtenerTipoBarraRebra(c),
                                                IdBarraCopiar = AyudaObtenerParametros.ObtenerIdBarraCopiar(c)
                                            }).ToList();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool BuscarListaRebarSimple()
        {

            //nota cambiar esta opcion con clasee :'SeleccionarRebarVisibilidad' 
            try
            {
                _collectorRebarViewActual = new FilteredElementCollector(_uidoc.Document, _viewObtenerBArras.Id);
                //para las familias en el para las instacias de familia en el plano
                _collectorRebarViewActual.OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsNotElementType();

                ListaRebarSimple = _collectorRebarViewActual.Where(c => c is Rebar).Select(c =>
                                             new RebarCopiarDTo()
                                             {
                                                 Rebar= (Rebar)c,
                                                 Elemento = _doc.GetElement( ((Rebar)c).GetHostId()),                     
                                             }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public void BuscarListaPathReinformetEnVistaActual()
        {
            _collectorRebarViewActual = new FilteredElementCollector(_uidoc.Document, _viewObtenerBArras.Id);
            //para las familias en el para las instacias de familia en el plano
            _collectorRebarViewActual.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsNotElementType();

            //a) obtener path
            _lista_A_DePathReinfVistaActual = _collectorRebarViewActual.Select(c =>
                                        new WrapperRebar()
                                        {
                                            element = c,
                                            NombreView = AyudaObtenerParametros.ObtenerNombreView(c),
                                            ObtenerTipoBarra = AyudaObtenerParametros.ObtenerTipoBarraRebra(c),
                                            ListaRebarInsistem = (c.IsValidObject ? ((PathReinforcement)c).GetRebarInSystemIds().ToList() : new List<ElementId>())
                                        }).ToList();

            //a.1)cargar datos rebarInSystem
            _lista_A_DePathReinfVistaActual.ForEach(c =>
            {
                c.ListaRebarInsystemV2= new List<WrapperRebarInSystem> ();
                foreach (ElementId item in c.ListaRebarInsistem)
                {
                    var RebInSYstem=_doc.GetElement(item);
                    if (RebInSYstem is RebarInSystem)
                    {
                        var result = AyudaObtenerParametros.ObtenerTipoBarraRebra(RebInSYstem);
                        c.ListaRebarInsystemV2.Add(new WrapperRebarInSystem()
                        {
                            element = RebInSYstem,
                            NombreView = AyudaObtenerParametros.ObtenerNombreView(RebInSYstem),
                            ObtenerTipoBarra = result,
                            BarraTipo = result.TipoBarra_.ToString()
                        }); ;
                    }
                }
            });
        }

        public void BorrarRebarSeleccionado()
        {
            if (_RebarSeleccion == null) return;
            try
            {
                using (Transaction transaction = new Transaction(_uidoc.Document))
                {
                    transaction.Start("Borrar Rebar-NH");

                    _uidoc.Document.Delete(_RebarSeleccion.Id);
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.Message);
            }
            // borra pelota de losa
        }
    }
}
