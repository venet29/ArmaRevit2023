using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    class BuscadorParametroCompartido
    {
        public static Dictionary<string, Definition> ListaFamilias { get; set; }
        protected static Definition elemetEncontrado;


        public static Definition BuscarTipoBarras(Document _doc, string name)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            //Debug.WriteLine($" ---->   name:{name}");
            Definition elemento = ShareParameterExists(_doc,name);

#pragma warning disable CS0642 // Possible mistaken empty statement
            if (elemento != null) ;
#pragma warning restore CS0642 // Possible mistaken empty statement
            AgregarDiccionario(name, elemento);

            return elemento;
        }

        private static Definition ShareParameterExists(Document doc, string paramName)
        {
            BindingMap bindingMap = doc.ParameterBindings;
            DefinitionBindingMapIterator iter = bindingMap.ForwardIterator();
            iter.Reset();

            while (iter.MoveNext())
            {
                elemetEncontrado = iter.Key;

                // find the definition of which the name is the appointed one
                if (String.Compare(elemetEncontrado.Name, paramName) != 0)
                {
                    continue;
                }
                InternalDefinition intDef = (InternalDefinition)iter.Key;
               // if (intDef != null) listaIdExistentes.Add(intDef.Id);
                // get the category which is bound
                ElementBinding binding = bindingMap.get_Item(elemetEncontrado) as ElementBinding;

                CategorySet bindCategories = binding.Categories;
                foreach (Category category in bindCategories)
                {
                    if (category.Name == doc.Settings.Categories.get_Item(BuiltInCategory.OST_Rebar).Name)
                    {

                        return elemetEncontrado;
                    }
                }
            }

            return null;
        }

        private static void AgregarDiccionario(string nombre, Definition element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Definition>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Definition>();
                return false;
            }
            else if (ListaFamilias.Count == 0)
            {
                return false;
            }


            if (ListaFamilias.ContainsKey(nombre))
                elemetEncontrado = ListaFamilias[nombre];

            return (elemetEncontrado == null ? false : true);
        }

    }


}
