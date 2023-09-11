using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.GEOM
{
   public class GemetrieLine
    {
        private readonly UIApplication _uiapp;
        private View _view;
        private Element element;

        public List<Reference> ListaResult { get; set; }

        public GemetrieLine(UIApplication uiapp, Element element_)
        {
            this._uiapp = uiapp;

            if(uiapp!=null)
                _view = _uiapp.ActiveUIDocument.ActiveView;
            else
                _view = element_.Document.ActiveView;

            element = element_;
            ListaResult = new List<Reference>();
        }

        public bool ObtenerLine()
        {

            try
            {
                Options options = new Options();
                options.View = _view; // the view in which you want to place the dimension
                options.ComputeReferences = true; // This will produce the references
                options.IncludeNonVisibleObjects = true;
                GeometryElement geom = element.get_Geometry(options);  //sirve tb para refenrencias de en grid para generar dimensiones


                foreach (GeometryObject geomObj in geom)
                {
                   
                    if (geomObj is Line)
                    {                  
                        Line refLine = geomObj as Line;
                        if (refLine != null && refLine.Reference != null)
                        {
                            ListaResult.Add(refLine.Reference);
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener GeometrieElement  ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
