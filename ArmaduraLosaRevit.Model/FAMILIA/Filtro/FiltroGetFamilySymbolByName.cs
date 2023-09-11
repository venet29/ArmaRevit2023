using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FAMILIA.Filtro
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
        public static ElementType GetFamilySymbolByName(Document doc, string name)
        {
            return new FilteredElementCollector(doc)
              .OfClass(typeof(FamilySymbol))
              .First(q => q.Name.Equals(name))
                as FamilySymbol;
        }


        /// <summary>
        /// Use a parameter filter to return the first element
        /// of the given type and with the specified string-valued
        /// built-in parameter matching the given name.
        /// OSEA  q el valor q aparece 'BuiltInParameter' seal igual al  'ValorBuiltInParameter'
        /// </summary>
        Element GetFirstElementOfTypeWithBipString(Document doc, Type type, BuiltInParameter bip, string ValorBuiltInParameter)
        {
            FilteredElementCollector a = GetElementsOfType(doc, type);

            ParameterValueProvider provider = new ParameterValueProvider(new ElementId(bip));

            FilterStringRuleEvaluator evaluator = new FilterStringEquals();

            FilterRule rule = new FilterStringRule(provider, evaluator, ValorBuiltInParameter);

            ElementParameterFilter filter = new ElementParameterFilter(rule);

            return a.WherePasses(filter).FirstElement();
        }
        /// <summary>
        /// Return a collector of all elements of the given type.
        /// </summary>
        FilteredElementCollector GetElementsOfType(Document doc, Type type)
        {
            return new FilteredElementCollector(doc).OfClass(type);
        }


        /// Retrieve a database element 
        /// of the given type and name.
        /// </summary>
        public static Element FindElementByName(Document doc, Type targetType, string targetName)
        {
            return new FilteredElementCollector(doc)
              .OfClass(targetType)
              .FirstOrDefault<Element>(e => e.Name.Equals(targetName));
        }

    }
}
