using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class Tipos_ProjectInformation
    {
        public static Element elemetEncontrado;

        public static Element ObtenerPrimerProjectInformation(Document _doc)
        {
            if (elemetEncontrado != null)
                return elemetEncontrado;


            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(_doc);
            filteredElementCollector.OfCategory(BuiltInCategory.OST_ProjectInformation);
            List<Element> ElementProjectInformation = filteredElementCollector.ToList();

            foreach (var item in ElementProjectInformation)
            {
                if(item is ProjectInfo)
                    Debug.WriteLine(item.Name);
            }

            
            return elemetEncontrado=(ElementProjectInformation.Count > 0 ? ElementProjectInformation[0] : null);
        }


    }


}
