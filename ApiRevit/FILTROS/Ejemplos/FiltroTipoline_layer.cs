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
    public class FiltroTipoline_layer
    {

        /// <summary>
        ///  Este colector de elementos filtrados recopila todos los elementos del documento,
        ///  excepto las líneas de separación de salas.
        /// </summary>
        /// <param name="doc"></param>
        public void FiltratTodoMenos(Document doc)
        {
            var lines = new FilteredElementCollector(doc).OfClass(typeof(CurveElement)).Where(q => q.Category.Id != new ElementId(BuiltInCategory.OST_RoomSeparationLines))
              .ToList();

            foreach (var line in lines)
            {
                doc.Delete(line.Id);
            }

        }


        /// <summary>
        /// opcion mas rapido usando filtros
        /// Delete all non-room-separating curve elements
        /// </summary>
        public void DeleteNonRoomSeparators(Document doc)
        {
            ElementCategoryFilter non_room_separator = new ElementCategoryFilter(BuiltInCategory.OST_RoomSeparationLines, true);

            FilteredElementCollector a = new FilteredElementCollector(doc).OfClass(typeof(CurveElement)).WherePasses(non_room_separator);

            doc.Delete(a.ToElementIds());
        }

    }
}
