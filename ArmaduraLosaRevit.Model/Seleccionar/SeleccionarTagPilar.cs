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
    public class SeleccionarTagPilar
    {

        /// <summary>
        /// Entrega lista con las vigas en un determinado nivel
        /// </summary>
        /// <param name="document"></param>
        /// <param name="level"> nivel del view en el que se trabaja</param>
        /// <returns></returns>
        public static List<IndependentTag> GetTagPilar(Document document,  View _view)
        {

            ElementCategoryFilter annoter = new ElementCategoryFilter(BuiltInCategory.OST_RebarTags);
            ElementClassFilter filterClass = new ElementClassFilter(typeof(FamilyInstance));
            LogicalAndFilter hostFilter = new LogicalAndFilter(annoter, annoter);

            FilteredElementCollector collector3 = new FilteredElementCollector(document, _view.Id);
            collector3.OfClass(typeof(IndependentTag)).WherePasses(hostFilter).WhereElementIsNotElementType(); // Filters;
            List<IndependentTag> listLevelLINQ = collector3.OfType<IndependentTag>().Where(c=>c.Name== "MRA Rebar_PILAR_50" || c.Name == "MRA Rebar_PILAR_75" || c.Name == "MRA Rebar_PILAR_100").ToList();

            //List<FamilyInstance> lista = listLevelLINQ.ToList();

            return listLevelLINQ;
        }

    }
}
