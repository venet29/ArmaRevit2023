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


namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarViga
    {
        public XYZ _ptoSeleccionMouseCentroCaraViga { get; private set; }
        public Element _ElemetSelect { get; private set; }
        public List<Element> listAvigas { get; private set; }

        /// <summary>
        /// Entrega lista con las vigas en un determinado nivel
        /// </summary>
        /// <param name="document"></param>
        /// <param name="level"> nivel del view en el que se trabaja</param>
        /// <returns></returns>
        public static IEnumerable<FamilyInstance> GetVigaFromLevel(Document document, View _view)
        {

            //Level level_ = _view.GenLevel;




            //BuiltInParameter bip = BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM;
            //ParameterValueProvider provider = new ParameterValueProvider(new ElementId(bip));
            //FilterNumericRuleEvaluator evaluator = new FilterNumericEquals();
            //FilterRule rule = new FilterElementIdRule(provider, evaluator, _view.Id);
            //ElementParameterFilter filter = new ElementParameterFilter(rule);
            //FilteredElementCollector collector2 = new FilteredElementCollector(document);
            //collector2.OfClass(typeof(FamilyInstance)).WherePasses(filter);
            //ICollection<Element> listLevelParam = collector2.ToElements();



            // Structural type filters firstly
            LogicalOrFilter stFilter = new LogicalOrFilter(new ElementStructuralTypeFilter(StructuralType.Beam), new ElementStructuralTypeFilter(StructuralType.Beam));
            // StructuralMaterialType should be Concrete
            LogicalAndFilter hostFilter = new LogicalAndFilter(stFilter, new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete));
            FilteredElementCollector collector3 = new FilteredElementCollector(document, _view.Id);
            collector3.OfClass(typeof(FamilyInstance)).WherePasses(hostFilter).WhereElementIsNotElementType(); // Filters;
            IEnumerable<FamilyInstance> listLevelLINQ = collector3.OfType<FamilyInstance>();

            //List<FamilyInstance> lista = listLevelLINQ.ToList();

            return listLevelLINQ;
        }
        public static IEnumerable<FamilyInstance> GetVigaFromLevelv2(Document document, View _view)
        {

            //Level level_ = _view.GenLevel;




            //BuiltInParameter bip = BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM;
            //ParameterValueProvider provider = new ParameterValueProvider(new ElementId(bip));
            //FilterNumericRuleEvaluator evaluator = new FilterNumericEquals();
            //FilterRule rule = new FilterElementIdRule(provider, evaluator, _view.Id);
            //ElementParameterFilter filter = new ElementParameterFilter(rule);
            //FilteredElementCollector collector2 = new FilteredElementCollector(document);
            //collector2.OfClass(typeof(FamilyInstance)).WherePasses(filter);
            //ICollection<Element> listLevelParam = collector2.ToElements();


            ISelectionFilter filtroRebarTag = new FiltroRebarTag();
            ElementCategoryFilter filterViga = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            // Structural type filters firstly
            LogicalOrFilter stFilter = new LogicalOrFilter(new ElementStructuralTypeFilter(StructuralType.Beam), filterViga);
            // StructuralMaterialType should be Concrete
            // LogicalAndFilter hostFilter = new LogicalAndFilter(stFilter, new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete));

            FilteredElementCollector collector3 = new FilteredElementCollector(document, _view.Id);
            collector3.OfClass(typeof(FamilyInstance)).WherePasses(stFilter).WhereElementIsNotElementType(); // Filters;
            IEnumerable<FamilyInstance> listLevelLINQ = collector3.OfType<FamilyInstance>();

            //List<FamilyInstance> lista = listLevelLINQ.ToList();

            return listLevelLINQ;
        }


        public bool M1_3_SeleccionarViga(UIDocument _uidoc)
        {
            try
            {
                Document _doc = _uidoc.Document;
                //  ISelectionFilter filtroMuro = new FiltroMuro();
                ISelectionFilter filtroMuro = new FiltroVIga_Muro();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                Reference ref_pickobject_element = _uidoc.Selection.PickObject(ObjectType.Element, filtroMuro, "Seleccionar cara de viga o muro:");
                _ptoSeleccionMouseCentroCaraViga = ref_pickobject_element.GlobalPoint;
                _ElemetSelect = _doc.GetElement(ref_pickobject_element);


                if (_ElemetSelect == null)
                {
                    Util.ErrorMsg($"No se puedo encontrar Muro de referencia.");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }


        public bool M1_3_SeleccionarVariasViga(UIDocument _uidoc)
        {
            try
            {
                listAvigas = new List<Element>();
                IList<Reference> refs_pickobjects = null;
                Document _doc = _uidoc.Document;
                try
                {
                    ISelectionFilter filtroMuro = new FiltroVIga_Muro();//   SelFilter.GetElementFilter(typeof(Wall), typeof(FamilyInstance));
                    refs_pickobjects = _uidoc.Selection.PickObjects(ObjectType.Element, filtroMuro, "Seleccionar Cara de viga:");
                }
                catch (Exception)
                {
                    return false;

                }

                //   _ptoSeleccionMouseCentroCaraViga = ref_pickobject_element.GlobalPoint;
                foreach (var item in refs_pickobjects)
                {
                    listAvigas.Add(_doc.GetElement(item));
                }

                if (listAvigas.Count == 0)
                {

                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }


        public bool SeleccionarTodasVigas(Document document, View _view)
        {
            try
            {

                FilteredElementCollector collector3 = new FilteredElementCollector(document, _view.Id);
                collector3.OfCategory(BuiltInCategory.OST_StructuralFraming).WhereElementIsNotElementType(); // Filters;
                var listaref = collector3.ToList();
                listAvigas = collector3.Where(c => c.Category.Name == "Structural Framing").ToList();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
