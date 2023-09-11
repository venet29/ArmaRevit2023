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
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.Extension;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarOpening
    {


        /// <summary>
        /// Entrega lista con las vigas en un determinado nivel
        /// </summary>
        /// <param name="document"></param>
        /// <param name="level"> nivel del view en el que se trabaja</param>
        /// <returns></returns>
        public static List<Element> GetOpeningFromLevel(Document document, Level level_)
        {
            // Structural type filters firstly
            ElementClassFilter f1 = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter f3 = new ElementCategoryFilter(BuiltInCategory.OST_ShaftOpening);
            LogicalOrFilter stFilter = new LogicalOrFilter(f3, f3);
            //  LogicalOrFilter stFilter = new LogicalOrFilter(new ElementStructuralTypeFilter(StructuralType.Beam), new ElementStructuralTypeFilter(StructuralType.Column));
            // StructuralMaterialType should be Concrete
            LogicalAndFilter hostFilter = new LogicalAndFilter(stFilter, f1);

            FilteredElementCollector collector = new FilteredElementCollector(document);
            //List<Element> vigas
            //  = new FilteredElementCollector(document)
            //    .OfClass(typeof(FamilyInstance))
            //    .WherePasses(hostFilter) // Filters

            //    //.Where(e => e.Name.Equals(level.Name))
            //    .ToList();
            //// document.GetElement(((Element)viga).LevelId) 
            //return new List<FamilyInstance>(vigas.Where(viga => viga.LevelId!= null).Select(r => r as FamilyInstance));


            List<Element> openingsResulta = new List<Element>();

            #region Filtering with LINQ
            List<Element> openings = collector.WherePasses(stFilter).ToElements() as List<Element>;


            foreach (var open in openings)
            {

                Level LevelOpening = document.GetElement(open.LevelId) as Level;


                Parameter paraLayout = ParameterUtil.FindParaByName(open.Parameters, "Base Offset");
                Parameter paraLayout0 = ParameterUtil.FindParaByName(open.Parameters, "Unconnected Height");

                double offst = paraLayout.AsDouble();
                double UnconnectecHeight = paraLayout0.AsDouble();

                string paraLayout1 = ParameterUtil.FindParaByBuiltInParameter(open, BuiltInParameter.WALL_BASE_OFFSET, document);
                string paraLayout2 = ParameterUtil.FindParaByBuiltInParameter(open, BuiltInParameter.WALL_USER_HEIGHT_PARAM, document);
                double elev = LevelOpening.ProjectElevation;

                if (elev - offst < level_.ProjectElevation - 0.2 && level_.ProjectElevation - 0.2 < elev + UnconnectecHeight)
                {
                    openingsResulta.Add(open);
                }

                //if elev-

            }

            //List<Element> openingInstances = (from instances in openings where instances is FamilyInstance select instances).ToList<Element>();
            #endregion

            //int n = openingInstances.Count;

            return openingsResulta;
        }


        /// <summary>
        /// Entrega lista con las vigas en un determinado nivel
        /// </summary>
        /// <param name="document"></param>
        /// <param name="level"> nivel del view en el que se trabaja</param>
        /// <returns></returns>
        public static ICollection<ElementId> GetOpeningFromLevelId(Document document, Level level_)
        {
            // Structural type filters firstly
            ElementClassFilter f1 = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter f3 = new ElementCategoryFilter(BuiltInCategory.OST_ShaftOpening);
            LogicalOrFilter stFilter = new LogicalOrFilter(f3, f3);
            //  LogicalOrFilter stFilter = new LogicalOrFilter(new ElementStructuralTypeFilter(StructuralType.Beam), new ElementStructuralTypeFilter(StructuralType.Column));
            // StructuralMaterialType should be Concrete
            LogicalAndFilter hostFilter = new LogicalAndFilter(stFilter, f1);

            FilteredElementCollector collector = new FilteredElementCollector(document);
            //List<Element> vigas
            //  = new FilteredElementCollector(document)
            //    .OfClass(typeof(FamilyInstance))
            //    .WherePasses(hostFilter) // Filters

            //    //.Where(e => e.Name.Equals(level.Name))
            //    .ToList();
            //// document.GetElement(((Element)viga).LevelId) 
            //return new List<FamilyInstance>(vigas.Where(viga => viga.LevelId!= null).Select(r => r as FamilyInstance));


            List<ElementId> openingsResulta = new List<ElementId>();

            #region Filtering with LINQ
            List<Element> openings = collector.WherePasses(stFilter).ToElements() as List<Element>;


            foreach (var open in openings)
            {

                Level LevelOpening = document.GetElement(open.LevelId) as Level;


                Parameter paraLayout = ParameterUtil.FindParaByName(open.Parameters, "Base Offset");
                Parameter paraLayout0 = ParameterUtil.FindParaByName(open.Parameters, "Unconnected Height");

                double offst = paraLayout.AsDouble();
                double UnconnectecHeight = paraLayout0.AsDouble();

                string paraLayout1 = ParameterUtil.FindParaByBuiltInParameter(open, BuiltInParameter.WALL_BASE_OFFSET, document);
                string paraLayout2 = ParameterUtil.FindParaByBuiltInParameter(open, BuiltInParameter.WALL_USER_HEIGHT_PARAM, document);
                double elev = LevelOpening.ProjectElevation;

                if (elev - offst < level_.ProjectElevation - 0.2 && level_.ProjectElevation - 0.2 < elev + UnconnectecHeight)
                {
                    openingsResulta.Add(open.Id);
                }

                //if elev-

            }

            //List<Element> openingInstances = (from instances in openings where instances is FamilyInstance select instances).ToList<Element>();
            #endregion

            //int n = openingInstances.Count;

            return openingsResulta;
        }




        /// <summary>
        /// Entrega lista con las vigas en un determinado nivel
        /// </summary>
        /// <param name="document"></param>
        /// <param name="level"> nivel del view en el que se trabaja</param>
        /// <returns></returns>
        public static ICollection<Element> SeleccionarAll(Document document)
        {
            // Structural type filters firstly
            ElementClassFilter f1 = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter f3 = new ElementCategoryFilter(BuiltInCategory.OST_ShaftOpening);
            LogicalAndFilter hostFilter = new LogicalAndFilter(f3, f1);

            FilteredElementCollector collector = new FilteredElementCollector(document);
            List<ElementId> openingsResulta = new List<ElementId>();

            List<Element> openings = collector.OfClass(typeof(Opening)).ToElements() as List<Element>;

            openingsResulta = openings.Select(c => c.Id).ToList();

            return openings;
        }
        public static ICollection<Element> SeleccionarAll_pasadas_ConNombre(Document _doc, string nombrePAsada)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc);

            bool IsNomstrarNombreFamilia = false;
            if(IsNomstrarNombreFamilia)
                filteredElementCollector.ForEach(c => Debug.Write($"Familia: {c.Name}"));

            var famil = filteredElementCollector.OfCategory(BuiltInCategory.OST_GenericModel)
                                                .WhereElementIsNotElementType()
                                                .Where(c => c.Name == nombrePAsada).ToList();

            var openingsResulta = famil.Select(c => c.Id).ToList();

            return famil;
        }


        public static ICollection<Element> SeleccionarAll_pasadas_Contenga(Document _doc,View _view ,string nombrePAsada)
        {

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc,_view.Id);

            bool IsNomstrarNombreFamilia = false;
            if (IsNomstrarNombreFamilia)
                filteredElementCollector.ForEach(c => Debug.Write($"Familia: {c.Name}"));

            var famil = filteredElementCollector.OfCategory(BuiltInCategory.OST_GenericModel)
                                                .WhereElementIsNotElementType()
                                                .Where(c => c.Name.Contains( nombrePAsada)).ToList();

            var openingsResulta = famil.Select(c => c.Id).ToList();

            return famil;
        }
    }
}
