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
    public class SeleccionarWalls
    {
        public static List<Element> listAWall { get; private set; }

        public static bool SeleccionarTodasElementos(Document document, View _view)
        {
            try
            {

                FilteredElementCollector collector3 = new FilteredElementCollector(document, _view.Id);
                collector3.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType(); // Filters;
                var listaref = collector3.ToList();
                listAWall = collector3.Where(c => c.Category.Name == "Walls").ToList();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
