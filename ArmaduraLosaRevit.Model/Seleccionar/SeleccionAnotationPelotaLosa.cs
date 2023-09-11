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
    public interface ISeleccionAnotationPelotaLosa
    {
        List<AnotationGeneralPelotaLosa> GetAllAnotationPelotaLosaFromLevel(UIDocument uidoc, ElementId levelId, BuiltInCategory targetCategory);
        List<AnotationGeneralPelotaLosa> GetAnotationPelotaLosaFromViewWithMouse(UIDocument uidoc);
        List<AnotationGeneralPelotaLosa> Get_SoloUno_AnotationPelotaLosaFromViewWithMouse(UIDocument uidoc);
    }
    public class SeleccionAnotationPelotaLosa : ISeleccionAnotationPelotaLosa
    {
        public SeleccionAnotationPelotaLosa()
        {

        }

        /// <summary>
        /// Entrega lista con las vigas en un determinado nivel
        /// </summary>
        /// <param name="document"></param>
        /// <param name="level"> nivel del view en el que se trabaja</param>
        /// <returns></returns>
        public List<AnotationGeneralPelotaLosa> GetAllAnotationPelotaLosaFromLevel(UIDocument uidoc, ElementId levelId, BuiltInCategory targetCategory)
        {
            Document document = uidoc.Document;
            // Level 2 example criteria
            List<AnotationGeneralPelotaLosa> LISTA = new List<AnotationGeneralPelotaLosa>();


            FilteredElementCollector collector = new FilteredElementCollector(document, document.ActiveView.Id);
            ElementCategoryFilter annoter = new ElementCategoryFilter(targetCategory);
            ElementClassFilter filterClass = new ElementClassFilter(typeof(FamilyInstance));
            LogicalAndFilter filter = new LogicalAndFilter(filterClass, annoter);

            ElementLevelFilter filterElementsOnLevel = new ElementLevelFilter(levelId);
            LogicalAndFilter filter2 = new LogicalAndFilter(filter, filterElementsOnLevel);

            ICollection<Element> elemes = collector.WherePasses(filter).ToElements();


            foreach (Element item in elemes)
            {
                // Level refLevel = document.GetElement(item.LevelId) as Level;
                //if (item.Name == ConstantesGenerales.CONST_TAGLOSAESTRUCTURAL + "_" + +ConstantesGenerales.CONST_ESCALA_BASE ||
                //item.Name == ConstantesGenerales.CONST_TAGLOSAESTRUCTURAL + "Var_" + ConstantesGenerales.CONST_ESCALA_BASE)//document.ActiveView.Scale)
                    if (item.Name.Contains(ConstNH.CONST_TAGLOSAESTRUCTURAL))
                    {

                    XYZ pointUbicacion = ((Location)item.Location as LocationPoint).Point;
                    ParameterSet pars = item.Parameters;
                    var espesor = ParameterUtil.FindParaByName(pars, "ESPESOR");

                    var numero = ParameterUtil.FindParaByName(pars, "NUMERO");

                    var angle = ParameterUtil.FindParaByName(pars, "ANGULO");

                    AnotationGeneralPelotaLosa item_ = new AnotationGeneralPelotaLosa()
                    {
                        PelotaLosa = item,
                        Numero = numero.AsString(),
                        Espesor = espesor.AsString(),
                        PointUbicacion = pointUbicacion,
                        Angulo = angle.AsDouble(),
                        IsEspesorVariable = (item.Name.Contains("Var") ? true : false)
                    };

                    LISTA.Add(item_);
                }
            }


            return LISTA;
        }

        public List<AnotationGeneralPelotaLosa> GetAnotationPelotaLosaFromViewWithMouse(UIDocument uidoc)
        {
            Document _doc = uidoc.Document;
            // Level 2 example criteria
            List<AnotationGeneralPelotaLosa> LISTA = new List<AnotationGeneralPelotaLosa>();

            IList<Element> pickedElements = new List<Element>();
            //clase filtro para selecccionar

            ISelectionFilter selFilter = new JtElementsOfClassSelectionFilter<AnnotationSymbol>();
            try
            {
                pickedElements = uidoc.Selection.PickElementsByRectangle(selFilter, "Selecionar Room con rectangulo");
            }
            catch (Exception)
            {

                return LISTA;
            }

            LISTA = ObtenerListaAnotationGeneralPelotaLosa(_doc, pickedElements);
            return LISTA;
        }

        public List<AnotationGeneralPelotaLosa> Get_SoloUno_AnotationPelotaLosaFromViewWithMouse(UIDocument uidoc)
        {
            Document _doc = uidoc.Document;
            // Level 2 example criteria
            List<AnotationGeneralPelotaLosa> LISTA = new List<AnotationGeneralPelotaLosa>();

            IList<Element> pickedElements = new List<Element>();
            //clase filtro para selecccionar
            ISelectionFilter selFilter = new JtElementsOfClassSelectionFilter<AnnotationSymbol>();
            Reference pickedElementref;
            try
            {
                pickedElementref = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "Selecionar Room con rectangulo");
            }
            catch (Exception)
            {

                return LISTA;
            }

            // Element anotation = _doc.GetElement(pickedElementref);// as AnnotationSymbol;
            pickedElements.Add(_doc.GetElement(pickedElementref));

            LISTA = ObtenerListaAnotationGeneralPelotaLosa(_doc, pickedElements);

            return LISTA;
        }

        private List<AnotationGeneralPelotaLosa> ObtenerListaAnotationGeneralPelotaLosa(Document _doc, IList<Element> pickedElements)
        {
            List<AnotationGeneralPelotaLosa> LISTA = new List<AnotationGeneralPelotaLosa>();
            foreach (Element item in pickedElements)
            {
                // Level refLevel = document.GetElement(item.LevelId) as Level;
                //if (item.Name == ConstantesGenerales.CONST_TAGLOSAESTRUCTURAL + "_" + ConstantesGenerales.CONST_ESCALA_BASE ||
                //   item.Name == ConstantesGenerales.CONST_TAGLOSAESTRUCTURAL + "Var_" + ConstantesGenerales.CONST_ESCALA_BASE)// + _doc.ActiveView.Scale)
                if (item.Name.Contains(ConstNH.CONST_TAGLOSAESTRUCTURAL))
                {

                    XYZ pointUbicacion = ((Location)item.Location as LocationPoint).Point;
                    ParameterSet pars = item.Parameters;
                    var espesor = ParameterUtil.FindParaByName(pars, "ESPESOR");
                    var numero = ParameterUtil.FindParaByName(pars, "NUMERO");
                    var angle = ParameterUtil.FindParaByName(pars, "ANGULO");

                    AnotationGeneralPelotaLosa item_ = new AnotationGeneralPelotaLosa()
                    {
                        PelotaLosa = item,
                        Numero = numero.AsString(),
                        Espesor = espesor.AsString(),
                        PointUbicacion = pointUbicacion,
                        Angulo = angle.AsDouble(),
                        IsEspesorVariable = (item.Name.Contains("Var") ? true : false)
                    };

                    LISTA.Add(item_);
                }
            }
            return LISTA;
        }
    }




}
