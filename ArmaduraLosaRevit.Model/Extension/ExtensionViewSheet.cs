using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{

    public static class ExtensionViewSheet
    {

        public static List<View> ObteneListaVIew(this ViewSheet ViewSheet)
        {
            if (ViewSheet == null) return new List<View>(); 
            var _doc = ViewSheet.Document;
           
            List<View> list  =ViewSheet.GetAllPlacedViews().Select(c => _doc.GetElement(c) as View).ToList();

            return list;
        }

    }
}
