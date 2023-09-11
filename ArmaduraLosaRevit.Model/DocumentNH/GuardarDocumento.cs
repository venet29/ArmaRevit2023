using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.DocumentNH
{
    public class GuardarDocumento
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public string VersionRevit { get; private set; }

        public GuardarDocumento(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
    
            VersionRevit = _uiapp.Application.VersionNumber; 
        }
        public bool CrearCopiaAS()
        {

            try
            {
                string _nombre = Path.GetFileNameWithoutExtension(_doc.PathName);

                string ruta = $@"\\Server-cdv\iyd\SaveRevit\{VersionRevit}\";
                if (!Directory.Exists(ruta)) {
                    Util.ErrorMsg($"No se encuentra ruta:  {ruta}");
                    return false;
                }
                string filepath = ruta+ $"{ _nombre}_{(DateTime.Now).ToString("MM_dd_yyyy")}.rvt";
        
                DocumentPreviewSettings settings = _doc.GetDocumentPreviewSettings();
                // Find a candidate 3D view
                FilteredElementCollector collector = new FilteredElementCollector(_doc);
                collector.OfClass(typeof(View3D));

                Func<View3D, bool> isValidForPreview = v => settings.IsViewIdValidForPreview(v.Id);

                View3D viewForPreview = collector.OfType<View3D>().First<View3D>(isValidForPreview);
                // Set the preview settings
                using (Transaction setTransaction = new Transaction(_doc, "Set preview view id"))
                {
                    setTransaction.Start();
                    settings.PreviewViewId = viewForPreview.Id;
                    setTransaction.Commit();
                }
                _doc.Save();
                //SaveAsOptions options = new SaveAsOptions();       
                //_doc.SaveAs(filepath, options);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }
    }
}
