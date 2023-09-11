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



namespace BuildingCoder.RUTINAS_NACHO
{
    public class FiltroGetElementTypeByName
    {
        /// <summary>
        /// Devuelve el primer tipo de elemento que coincida con el nombre dado.
        ///  Este filtro podría acelerarse utilizando un filtro de parámetros(rápido)
        ///  en lugar del posprocesamiento LINQ(más lento que lento).
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ElementType GetElementTypeByName(Document doc, string name)
        {
            return new FilteredElementCollector(doc)
              .OfClass(typeof(ElementType))
              .First(q => q.Name.Equals(name)) as ElementType;
        }
    }
}
