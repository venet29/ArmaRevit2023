using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    class TipoTagVIga
    {
 
        public static Dictionary<string, Element> ListaFamilias { get; set; }

        public static Element elemetEncontrado;

        public static Element M1_GetVigaTag(string name, string familyName, Document rvtDoc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            Element elemento = M1_2_BuscarEnColecctor(name, familyName, BuiltInCategory.OST_StructuralFramingTags, rvtDoc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
                return false;
            }
            else if (ListaFamilias.Count == 0)
            {
                return false;
            }


            if (ListaFamilias.ContainsKey(nombre))
                elemetEncontrado = ListaFamilias[nombre];

            if (elemetEncontrado != null && elemetEncontrado.IsValidObject == false)
            {
                ListaFamilias.Remove(nombre);
                elemetEncontrado = null;
            }

            return (elemetEncontrado == null ? false : true);
        }

        private static FamilySymbol M1_2_BuscarEnColecctor(string name, string familyName, BuiltInCategory builtInCategory, Document rvtDoc)
        {
            FamilySymbol elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(builtInCategory);
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                FamilySymbol item_fasy = (FamilySymbol)item;
                Debug.Print($"nombre:{ item_fasy.Name} && familyname:{ item_fasy.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString()}");
                if (item_fasy.Name == name && item_fasy.FamilyName== familyName)
                {
                    elemento = item_fasy;
                    return elemento;
                }
            }

            return elemento;
        }



        private static void AgregarDiccionario(string nombre, Element element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Element>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }

    }
}
