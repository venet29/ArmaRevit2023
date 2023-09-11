using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FAMILIA.Varios
{
    class ExportarFamilias
    {
        private Document doc;
        private readonly UIApplication uiapp;

        public ExportarFamilias(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            this.doc = uiapp.ActiveUIDocument.Document;
        }
        //https://thebuildingcoder.typepad.com/blog/2014/09/modifying-saving-and-reloading-families.html
        public bool ExportarFAmilias()
        {
            try
            {
                Family _Family = null;
                string path = " ruta familia";
                string name = "Nombre familia";
                string fName = name + ".rfa";
                string fPath = path + fName;

                // Revit throws an error on this line 
                // saying that Family is not editable
                // What could cause this mayhem?
                // To upload .rfa Family file I need to 
                // save it as a file first and that's what 
                // I try to do until mighty ERROR occurs

                Document famDoc = doc.EditFamily(_Family);
                famDoc.SaveAs(fPath);
                famDoc.Close(false);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }

        public bool CAmbiarTipoLetraFamilia()
        {
            try
            {
                Family _Family = null;
                string path = " ruta familia";
                string name = "Nombre familia";
                string fName = name + ".rfa";
                string fPath = path + fName;

                // Revit throws an error on this line 
                // saying that Family is not editable
                // What could cause this mayhem?
                // To upload .rfa Family file I need to 
                // save it as a file first and that's what 
                // I try to do until mighty ERROR occurs

                Document famDoc = doc.EditFamily(_Family);
                FilteredElementCollector graphic_styles = new FilteredElementCollector(famDoc).OfClass(typeof(TextNoteType));

                var elemetEncontradoAux = graphic_styles.ToList();
                using (Transaction t = new Transaction(famDoc))
                {
                    t.Start("modificar TipoTextNote-NH");

                    for (int i = 0; i < elemetEncontradoAux.Count; i++)
                    {
                        TextNoteType tipotext = elemetEncontradoAux[i] as TextNoteType;
                        tipotext.get_Parameter(BuiltInParameter.TEXT_FONT).Set("Arial");
                    }

                    t.Commit();
                }

                famDoc.SaveAs(fPath);
                famDoc.Close(false);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }


        public bool cmbiarLetraArial(Family fam)
        {
            TextElementType tipotext = null;
            int cont = 0;
            try
            {

                Document docfamily = doc.EditFamily(fam);
                // docfamily.SaveAs(@"C:/Temp/NewFamily.rfa");
                //If necessary, you can open a transaction in family :
                using (Transaction trans = new Transaction(docfamily))
                {
                    trans.Start("family");

                    Document famDoc = doc.EditFamily(fam);
                    FilteredElementCollector graphic_styles = new FilteredElementCollector(famDoc).OfClass(typeof(TextElementType));

                    var elemetEncontradoAux = graphic_styles.ToList();



                    for (int i = 0; i < elemetEncontradoAux.Count; i++)
                    {

                        tipotext = elemetEncontradoAux[i] as TextElementType;

                        if (tipotext == null) continue;
                        if (!tipotext.IsValidObject) continue;
                        Debug.WriteLine($"B) {i}  ----- >TextElementType:{tipotext.Name}");
                        cont = i;
                        var PARA = tipotext.get_Parameter(BuiltInParameter.TEXT_FONT);

                        if (PARA == null) continue;
     
                        PARA.Set("Arial");

                        if (tipotext.Name.Contains("_50"))
                        {
                            var res1 = tipotext.get_Parameter(BuiltInParameter.TEXT_SIZE).AsDouble();
                            tipotext.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(Util.MmToFoot(2.0));
                        }
                        else if (tipotext.Name.Contains("_100") || tipotext.Name.Contains("_75"))
                        {
                            var res2 = tipotext.get_Parameter(BuiltInParameter.TEXT_SIZE).AsDouble();
                            tipotext.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(Util.MmToFoot(1.5));
                        }
                    }

                    trans.Commit();
                }
                // docfamily.Save();
                docfamily.LoadFamily(doc, new CustomFamilyLoadOption());
            }
            catch (Exception eX)
            {
                Util.ErrorMsg($"Error al ejecutar cambio de letras  FAMILIA:{tipotext.Name} \n  ex:{eX.Message}");
                return false;
            }

            return true;
        }
        public class CustomFamilyLoadOption : IFamilyLoadOptions
        {
            public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                return true;
            }

            public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
            {
                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
        }
    }
}
