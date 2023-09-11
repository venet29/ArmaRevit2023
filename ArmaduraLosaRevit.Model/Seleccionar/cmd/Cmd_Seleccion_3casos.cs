#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WinForms = System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Elemento_Losa;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Enumeraciones;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.Seleccionar.cmd
{
   

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class FilterParameterEx2 : IExternalCommand
    {
        static string _level_name = "Level 2"; // "02 - Floor"

        public Result Execute(          ExternalCommandData commandData,          ref string message,          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // Level 2 example criteria
            ElementId levelId = Util.FindLevelId(doc, _level_name);

            Stopwatch sw = new Stopwatch();
            string names = string.Empty;

            // Use the Level filter to find all 
            // FamilyInstances with desired Level.
            // Note this is a slow filter.

            sw.Reset();
            sw.Start();

            ElementLevelFilter filterElementsOnLevel = new ElementLevelFilter(levelId);

            FilteredElementCollector collector1 = new FilteredElementCollector(doc);

            collector1.OfClass(typeof(FamilyInstance)).WherePasses(filterElementsOnLevel);

            ICollection<Element> listLevel = collector1.ToElements();

            // Now that we have only what we want, 
            // foreach process them into a report.

            names = string.Empty;
            foreach (FamilyInstance instance in listLevel)
            {
                names += "\nLevel Name = " + instance.Level2()+ "   Instance name = " + instance.Name + "   id: " + instance.Id.ToString();
            }

            sw.Stop();

            //Util.ShowElapsedTime(sw, "Using ElementLevelFilter to find " + listLevel.Count().ToString() + " family instances on a given level: "
            //  + names);

            // Use ElementParameterFilter to find all 
            // FamilyInstances with desired Level by 
            // finding the data in the FAMILY_LEVEL_PARAM 
            // parameter.

            sw.Reset();
            sw.Start();

            BuiltInParameter bip = BuiltInParameter.FAMILY_LEVEL_PARAM;

            ParameterValueProvider provider = new ParameterValueProvider(new ElementId(bip));

            FilterNumericRuleEvaluator evaluator = new FilterNumericEquals();

            FilterRule rule = new FilterElementIdRule(provider, evaluator, levelId);

            ElementParameterFilter filter = new ElementParameterFilter(rule);

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);

            collector2.OfClass(typeof(FamilyInstance)).WherePasses(filter);

            ICollection<Element> listLevelParam = collector2.ToElements();

            // Now that we have only what we want, 
            // foreach process them into a report.

            names = string.Empty;
            foreach (FamilyInstance instance in listLevelParam)
            {
                names += "\nLevel Name = " + instance.Level2().Name + "   Instance name = " + instance.Name + "   id: " + instance.Id.ToString();
            }

            sw.Stop();

          //  Util.ShowElapsedTime(sw, "Using Element Parameter Filter: " + listLevelParam.Count().ToString() + names);

            // Use LINQ to find all FamilyInstances 
            // with desired Level.

            sw.Reset();
            sw.Start();
            FilteredElementCollector collector3 = new FilteredElementCollector(doc);

            collector3.OfClass(typeof(FamilyInstance));

            IEnumerable<FamilyInstance> listLevelLINQ = collector3.OfType<FamilyInstance>();

            // Use LINQ to filter down those that 
            // match the desired level name.

            //
            //IEnumerable<FamilyInstance> listFiOnLevelLINQ = from fi in listLevelLINQ
            //                                                where ((level == fi.Level2())) != null) && (level.Id.Equals(levelId)) select fi;


            Level level = null;
            IEnumerable<FamilyInstance> listFiOnLevelLINQ = from fi in listLevelLINQ
                                                            where ((fi.Level2() is Level) && (level = (Level)fi.Level2()) != null) && (level.Id == levelId)
                                                            select fi;
            // Now that we have only what we want, 
            // foreach process them into a report.

            names = string.Empty;
            foreach (FamilyInstance instance in listFiOnLevelLINQ)
            {
                names += "\nLevel Name = " + instance.Level2().Name
                  + "   Instance name = " + instance.Name
                  + "   id: " + instance.Id.ToString();
            }

            sw.Stop();

            //Util.ShowElapsedTime(sw,
            //  "Using LINQ to find "
            //  + listFiOnLevelLINQ.Count<FamilyInstance>().ToString()
            //  + " family instances on a specific level: "
            //  + names);

            return Result.Succeeded;
        }
  
    
    }
}
