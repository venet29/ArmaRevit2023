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
    public class rutinaEjemplo
    {
        /// <summary>
        /// Devuelve el primer tipo de nota de texto que coincide con el nombre dado.
        /// Tenga en cuenta que TextNoteType es una subclase de ElementType,
        /// por lo que este método es más restrictivo sobre todo más rápido que Util.GetElementTypeByName.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        TextNoteType GetTextNoteTypeByName(Document doc, string name)
        {
            return new FilteredElementCollector(doc)
              .OfClass(typeof(TextNoteType))
              .First(q => q.Name.Equals(name)) as TextNoteType;
        }
    }
}
