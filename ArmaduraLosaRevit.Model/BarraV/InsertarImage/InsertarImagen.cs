using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.InsertarImage
{
    public  class InsertarImagen
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public InsertarImagen(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.Document.ActiveView;
        }
    }
}
