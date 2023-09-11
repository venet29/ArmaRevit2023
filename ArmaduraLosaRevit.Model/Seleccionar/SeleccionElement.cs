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
    public class SeleccionElement
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="targetType">  ejemplo
        /// 
        /// typeof(SpatialElement)
        /// typeof(Level)
        /// typeof(View), 
        /// typeof(ReferencePlane) 
        /// </param>
        /// <param name="level"></param>
        /// <param name="elemento"></param>
        /// <returns></returns>
        public static List<Element> GetElementoFromLevel(Document document, Type targetType, Level level)
        {

            // busca el nivel del pisos analizado
            ElementId levelId = Util.FindLevelId(document, level.Name);

            // First, narrow down to the elements of the given type and category 
            var collector = new FilteredElementCollector(document).OfClass(targetType);
            //if (targetCategory.HasValue)
            //{  

            //    collector.OfCategory(targetCategory.Value);
            //}
            // Parse the collection for the given names 
            // Using LINQ query here. 
            var elems = from element in collector where element.LevelId.IntegerValue == levelId.IntegerValue select element;
            // Put the result as a list of element for accessibility. 
            var aaa = elems.ToList();




            List<Element> elemntos = new FilteredElementCollector(document).OfClass(targetType).ToList();


            var result = new List<Element>(elemntos.Where(r => r.LevelId.IntegerValue == levelId.IntegerValue).Select(r => r as Element));


            return result;
        }


    }

}
