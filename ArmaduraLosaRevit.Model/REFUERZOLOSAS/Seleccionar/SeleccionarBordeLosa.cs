using ApiRevit.FILTROS;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



using System.Collections.ObjectModel;
using ArmaduraLosaRevit.Model.Niveles.Vigas;
using ArmaduraLosaRevit.Model.LosaArmadura.Geometria;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar
{
    public class SeleccionarBordeLosa
    {
        protected UIApplication _uiapp;
        protected UIDocument _uidoc;
        protected Document _doc;
        protected View _seccionView;





        public XYZ PtoSeleccionBordeLosa { get; set; }
        public Floor _FloorSeleccion { get; set; }

      //  private LosaGeometrias _losaGeometrias;

    //    public Curve _curvaBordeLosa { get;  set; }

        public SeleccionarBordeLosa(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;

            this._seccionView = _doc.ActiveView;
        }


        public bool SeleccionarEjecutar() => M1_SeleccionarBordeLosa();

        //funciona  pero no usa fitro
        public bool M1_SeleccionarBordeLosa()
        {
            try
            {
                ISelectionFilter f = new JtElementsOfClassSelectionFilter<Floor>();
                Reference ref_pickobject_element=null;
                try
                {
                    ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Edge, f, "Seleccionar borde de losa:");
                }
                catch (Exception)
                {
                    _FloorSeleccion = null;
                    return true;
                }
    
            

                if (ref_pickobject_element == null) return false;
                //obtiene una referencia floor con la referencia r
                Element Element_pickobject_element =_doc.GetElement(ref_pickobject_element.ElementId);


                if (!(Element_pickobject_element is Floor)) return false;

                _FloorSeleccion = Element_pickobject_element as Floor;
                PtoSeleccionBordeLosa = ref_pickobject_element.GlobalPoint;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
