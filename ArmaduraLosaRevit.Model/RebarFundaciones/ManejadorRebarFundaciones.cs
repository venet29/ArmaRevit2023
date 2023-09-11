using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarFundaciones
{


    public class ManejadorRebarFundaciones
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public ManejadorRebarFundaciones(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
        }



        public bool Ejecutar()
        {
            try
            {


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }

}
