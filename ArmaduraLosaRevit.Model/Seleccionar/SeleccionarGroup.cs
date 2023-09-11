#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WinForms = System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Elemento_Losa;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Enumeraciones;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarGroup
    {
        private readonly UIApplication _uiapp;
        private UIDocument _uidoc;
        private Document _doc;

        public List<Element> ListaGroup { get; set; }
     
        public SeleccionarGroup(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;
            this.ListaGroup = new List<Element>();

        }

        public bool PreseleccionarGroup()
        {
            List<ElementId> refs_pickobjects = new List<ElementId>();
            try
            {
                //   refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element,f, "SELECCION PICKOBJECTS: SELECCIONA UNO o VARIOS");
                refs_pickobjects = _uidoc.Selection.GetElementIds().ToList();
                foreach (ElementId item in refs_pickobjects)
                {
                    Element elem = _doc.GetElement(item);
                    if (elem is Group)
                    {
                        ListaGroup.Add(elem);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
    }
}
