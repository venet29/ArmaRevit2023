using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public abstract class ABuscarTipo
    {
        public static Dictionary<string, Element> ListaFamilias { get; set; }

       protected static Element elemetEncontrado;
    
        protected static bool BuscarDiccionario(string nombre)
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
           

            if(ListaFamilias.ContainsKey(nombre))
                elemetEncontrado = ListaFamilias[nombre];

            if (elemetEncontrado != null && elemetEncontrado.IsValidObject == false)
            {
                ListaFamilias.Remove(nombre);
                elemetEncontrado = null;
            }

            return (elemetEncontrado == null ? false : true);
        }

      
  
        public static Element M1_2_BuscarEnColecctor(string name, Document rvtDoc, BuiltInCategory builtInCategory)
        {
            Element elemento = null;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(rvtDoc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(builtInCategory).WhereElementIsElementType();
            var m_roomTagTypes = filteredElementCollector.ToList();
            foreach (var item in m_roomTagTypes)
            {
                if (item.Name == name)
                {
                    elemento = item;
                    return elemento;
                }
            }
            //opcion busqueda
            // var m_roomTagTypes = filteredElementCollector.ToList();
            // return m_roomTagTypes.Where(fa => fa.Name == name || fa.Name == name + "_" + scale).FirstOrDefault();
            return elemento;
        }


        protected static void AgregarDiccionario(string nombre, Element element)
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
