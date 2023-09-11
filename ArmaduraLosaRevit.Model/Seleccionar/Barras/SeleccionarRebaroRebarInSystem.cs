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
    public class SeleccionarRebaroRebarInSystemElemento
    {

        private UIDocument _uidoc;
        private Reference _ElementsRebarSeleccionado;

        private UIApplication _uiapp;
        private Document _doc;
        private View _viewObtenerBArras;
        public Element _ElementoSeleccion { get; set; }
        public Rebar _RebarSeleccion { get; set; }
        public XYZ _ptoRebarSeleccion { get; private set; }


#pragma warning disable CS0169 // The field 'SeleccionarRebaroRebarInSystemElemento._collectorRebarViewActual' is never used
        private FilteredElementCollector _collectorRebarViewActual;
#pragma warning restore CS0169 // The field 'SeleccionarRebaroRebarInSystemElemento._collectorRebarViewActual' is never used


        public SeleccionarRebaroRebarInSystemElemento(UIApplication _uiapp, View _viewObtenerBArras)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._viewObtenerBArras = _viewObtenerBArras;
            this._uidoc = _uiapp.ActiveUIDocument;

        }


        public bool GetSelecionarRebaroRebarInsistem()
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

                if (!(Element_pickobject_element is Rebar || Element_pickobject_element is RebarInSystem)) return false;

                _ElementoSeleccion = Element_pickobject_element;

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

   

     


    



        public void BorrarRebarSeleccionado()
        {
            if (_RebarSeleccion == null) return;
            if (_RebarSeleccion.Pinned)
            {
                Util.ErrorMsg($"Barra tiene Pin asigando, no es posible editar. ");
                return;
            }
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
