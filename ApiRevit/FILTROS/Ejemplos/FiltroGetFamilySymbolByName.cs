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
    public class FiltroGetFamilySymbolByName
    {

        /// <summary>
        /// Devuelve el primer símbolo familiar que coincide con el nombre de pila. 
        /// Tenga en cuenta que FamilySymbol es una subclase de ElementType, 
        /// por lo que este método es más restrictivo sobre todo más rápido que el anterior.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ElementType GetFamilySymbolByName(  Document doc,  string name)
        {
            return new FilteredElementCollector(doc)
              .OfClass(typeof(FamilySymbol))
              .First(q => q.Name.Equals(name))
                as FamilySymbol;
        }
    }
}
