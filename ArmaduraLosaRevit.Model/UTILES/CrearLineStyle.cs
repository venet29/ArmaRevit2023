
using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class CrearLineStyle
    {
        private readonly Document _doc;
        private readonly int lineEight;
        private readonly Color color;
        private readonly string _linePattern;
        private string _nombreNuevaLinea;

        public CrearLineStyle(Document _doc, string nombreNuevaLinea, int lineEight, Color color, string linePattern)
        {
            this._doc = _doc;
            this._nombreNuevaLinea = nombreNuevaLinea;
            this.lineEight = lineEight;
            this.color = color;
            this._linePattern = linePattern;
        }
        public void CreateLineStyleConTrans()
        {
         

            //    FilteredElementCollector graphic_styles = new FilteredElementCollector(_doc).OfClass(typeof(GraphicsStyle));
           // List<Element> red_line_styles = graphic_styles.Where(e => e.Name.ToString() == _nombreNuevaLinea).ToList();//graphic_stylesName  -->e.Name.ToString() == "ROJO"
            //if (red_line_styles.Count > 0) return;
            Element red_line_styles = TiposLineaPattern.ObtenerTipoLinea(_nombreNuevaLinea, _doc);
            if (red_line_styles != null) return;

            // Find existing linestyle.  Can also opt to
            // create one with LinePatternElement.Create()

            FilteredElementCollector fec = new FilteredElementCollector(_doc).OfClass(typeof(LinePatternElement));
            var ListlinePatternElem = fec.Where(c => c != null).ToList();
            LinePatternElement linePatternElem = fec.Where(c => c.Name == _linePattern).FirstOrDefault() as LinePatternElement;
            // LinePatternElement linePatternElem = fec.Cast<LinePatternElement>().First<LinePatternElement>(linePattern => linePattern.Name == "Solid");
            if (linePatternElem == null)
            {
                Util.ErrorMsg($"No se pudo crear linea '{_nombreNuevaLinea}', porque No se encontro 'LinePattern' de nombre '{_linePattern}'.");
                return;
            }
            // The new linestyle will be a subcategory 
            // of the Lines category        

            Categories categories = _doc.Settings.Categories;

            Category lineCat = categories.get_Item(BuiltInCategory.OST_Lines);
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create LineStyle-NH");

                    // Add the new linestyle 

                    Category newLineStyleCat = categories.NewSubcategory(lineCat, _nombreNuevaLinea);

                    _doc.Regenerate();

                    // Set the linestyle properties 
                    // (weight, color, pattern).

                    newLineStyleCat.SetLineWeight(lineEight, GraphicsStyleType.Projection);

                    newLineStyleCat.LineColor = color;

                    newLineStyleCat.SetLinePatternId(linePatternElem.Id, GraphicsStyleType.Projection);

                    //OverrideGraphicSettings overrides = _doc.ActiveView.GetCategoryOverrides(newLineStyleCat.Id);
                    //ElementId elId = new ElementId(-3000010);
                    //overrides.SetCutLinePatternId(elId);
                    // _doc.ActiveView.SetElementOverrides(newLineStyleCat.Id, overrides);
                    ProjectLocation projectLocation = _doc.ActiveProjectLocation;
                    XYZ origin = new XYZ(0, 0, 0);
                    ProjectPosition position = projectLocation.GetProjectPosition(origin);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }



        public static void ReadElementOverwriteLinePattern_ConTrasn(Element elem2, Document doc)
        {
            ElementId id = new ElementId(-3000010);

            Transaction trans = new Transaction(doc);
            trans.Start("change cut line pattern-NH");
            OverrideGraphicSettings override2 = doc.ActiveView.GetElementOverrides(elem2.Id);
            override2.SetCutLinePatternId(id);
            doc.ActiveView.SetElementOverrides(elem2.Id, override2);
            trans.Commit();
        }
        public static void ReadElementOverwriteLinePattern_sinTrasn(Element elem2, Document doc)
        {
            ElementId id = new ElementId(-3000010);
            OverrideGraphicSettings override2 = doc.ActiveView.GetElementOverrides(elem2.Id);
            override2.SetCutLinePatternId(id);
            doc.ActiveView.SetElementOverrides(elem2.Id, override2);

        }
    }
}
