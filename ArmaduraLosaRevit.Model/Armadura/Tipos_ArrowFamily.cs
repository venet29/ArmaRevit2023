using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;
using ApiRevit;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class Tipos_ArrowFamily
    {


        private static Dictionary<string, Family> ListaFamilias { get; set; }

        private static Family elemetEncontrado;

        public static Family ObtenerArrowheadPorNombre(string name, Document doc)
        {

            if (BuscarDiccionario(name)) return elemetEncontrado;

            Family elemento = M1_2_BuscarEnColecctor(name, doc);

            AgregarDiccionario(name, elemento);

            return elemento;
        }

        public static void Limpiar() => ListaFamilias = new Dictionary<string, Family>();

        private static Family M1_2_BuscarEnColecctor(string name, Document rvtDoc)
        {

            Family m_family_ = null;

            //start = new TimeSpan(DateTime.Now.Ticks);
            FilteredElementCollector filteredElementCollector1 = new FilteredElementCollector(rvtDoc);
            m_family_ = filteredElementCollector1
                .OfClass(typeof(Family))
                .Cast<Family>()
                .Where(c => c.Name == name).FirstOrDefault();
            //opcion para obtener lista de colector
            //var asdf = filteredElementCollector1.OfType<Family>().ToList();
            if (m_family_ != null) Debug.WriteLine($"familia : {name}  , id:{m_family_.Id}");
            return m_family_;

        }



        private static void AgregarDiccionario(string nombre, Family element)
        {
            if (element == null) return;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Family>();
            }
            if (!ListaFamilias.ContainsKey(nombre)) ListaFamilias.Add(nombre, element);


        }
        private static bool BuscarDiccionario(string nombre)
        {
            elemetEncontrado = null;
            if (ListaFamilias == null)
            {
                ListaFamilias = new Dictionary<string, Family>();
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



    }
}





namespace Revit.SDK.Samples.HelloRevit.CS

{


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)

        {

            UIApplication uiApp = commandData.Application;

            Document doc = uiApp.ActiveUIDocument.Document;



            // Access all elements in the model which represent Arrowheads

            // This is being done by filtering all elements which

            // are of ElementType and have the ALL_MODEL_FAMILY_NAME

            // builtIn Parameter set to 'Arrowhead'

            ElementId id = new ElementId(BuiltInParameter.ALL_MODEL_FAMILY_NAME);

            ParameterValueProvider provider = new ParameterValueProvider(id);

            FilterStringRuleEvaluator evaluator = new FilterStringEquals();

            FilterRule rule = new FilterStringRule(provider, evaluator, "Arrowhead");
            
            ElementParameterFilter filter = new ElementParameterFilter(rule);



            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WherePasses(filter);



            using (Transaction trans = new Transaction(doc, "Arrowhead"))

            {

                trans.Start();

                // For simplicity, assuming that the

                // Structural Component Tag is selected

                foreach (Element selectedElement in  uiApp.ActiveUIDocument.Selection.GetSelection2(doc))

                {

                    IndependentTag tag = selectedElement as IndependentTag;

                    if (null != tag)

                    {

                        // Access the Symbol of the IndependentTag element

                        FamilySymbol tagSymbol =                       doc.GetElement(tag.GetTypeId()) as FamilySymbol;



                        // Set the LEADER_ARROWHEAD parameter of the

                        // Symbol with one of the arrowheads that was filtered

                        tagSymbol.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(collector.ToElementIds().ElementAt<ElementId>(0));

                    }

                    trans.Commit();

                }

            }

            return Result.Succeeded;

        }

    }

}
