#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WinForms = System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections;
#endregion // Namespaces



namespace ApiRevit.FILTROS
{
    public class SeleccionFindElement
    {
        Application _app;
        Document _doc;


        public Result Execute(     ExternalCommandData commandData,     ref string message,     ElementSet elements)
        {
            // objects for the top level access
            //
            _app = commandData.Application.Application;
            _doc = commandData.Application.ActiveUIDocument.Document;

     
            View pView = findElement(typeof(View), "Front") as View;

            // find the upper ref level
            // findElement() is a helper function. see below.
            Level upperLevel = findElement(typeof(Level), "Upper Ref Level") as Level;
            Reference ref1 = upperLevel.GetPlaneReference();

            ReferencePlane refRight = findElement(typeof(ReferencePlane), "Right") as ReferencePlane;
            ReferencePlane refLeft = findElement(typeof(ReferencePlane), "Left") as ReferencePlane;
            ReferencePlane refFront = findElement(typeof(ReferencePlane), "Front") as ReferencePlane;
            ReferencePlane refBack = findElement(typeof(ReferencePlane), "Back") as ReferencePlane;


            //segunda opcion

            Level level1 = (Level)FindElement(_doc, typeof(Level), "Level 1", null);

            // finally, return
            return Result.Succeeded;
        }

        #region 1) filtra por tipo y nombre


        // ==================================================================================
        //   helper function: find an element of the given type and the name.
        //   You can use this, for example, to find Reference or Level with the given name.
        // ==================================================================================
        Element findElement(Type targetType, string targetName)
        {
            // get the elements of the given type
            //NN
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            collector.WherePasses(new ElementClassFilter(targetType));

            // parse the collection for the given name
            // using LINQ query here. 
            // 
            var targetElems = from element in collector where element.Name.Equals(targetName) select element;
            List<Element> elems = targetElems.ToList<Element>();

            if (elems.Count > 0)
            {  // we should have only one with the given name. 
                return elems[0];
            }

            // cannot find it.
            return null;
        }
        #endregion


        #region 2)filtra seun typo,nombre elemtoi y categoria


        /// <summary>
        /// Helper function: searches elements with given Class, Name and Category (optional), 
        /// and returns the first in the elements found. 
        /// This gets handy when trying to find, for example, Level. 
        /// e.g., FindElement(_doc, GetType(Level), "Level 1") 
        /// </summary>
        public static Element FindElement(Document rvtDoc, Type targetType, string targetName, Nullable<BuiltInCategory> targetCategory)
        {
            // Find a list of elements using the overloaded method. 
            IList<Element> elems = FindElements(rvtDoc, targetType, targetName, targetCategory);

            // Return the first one from the result. 
            if (elems.Count > 0)
            {
                return elems[0];
            }

            return null;
        }


        /// <summary>
        /// Helper function: find a list of element with given Class, Name and Category (optional). 
        /// </summary>  
        public static IList<Element> FindElements(Document rvtDoc, Type targetType, string targetName, Nullable<BuiltInCategory> targetCategory)
        {
            // First, narrow down to the elements of the given type and category 
            var collector = new FilteredElementCollector(rvtDoc).OfClass(targetType);
            if (targetCategory.HasValue)
            {
                collector.OfCategory(targetCategory.Value);
            }
            // Parse the collection for the given names 
            // Using LINQ query here. 
            var elems = from element in collector where element.Name.Equals(targetName) select element;
            // Put the result as a list of element for accessibility. 
            return elems.ToList();
        }


        public static IList<Element> FindElements(Document rvtDoc, Type targetType, Level Level, Nullable<BuiltInCategory> targetCategory)
        {
            // First, narrow down to the elements of the given type and category 
            var collector = new FilteredElementCollector(rvtDoc).OfClass(targetType);
            if (targetCategory.HasValue)
            {
                collector.OfCategory(targetCategory.Value);
            }
            // Parse the collection for the given names 
            // Using LINQ query here. 
            var elems = from element in collector where element.LevelId== Level.LevelId select element;
            // Put the result as a list of element for accessibility. 
            return elems.ToList();
        }

        #endregion


    }
}
