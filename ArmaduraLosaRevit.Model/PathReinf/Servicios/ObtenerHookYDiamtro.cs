using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.PathReinf.Servicios
{
    public class ObtenerHookYDiamtro
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public ObtenerHookYDiamtro(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
           // ObtenerHook("");
           // ObtenerTypoDiamtro(10);

        }

        private ElementId ObtenerHook()
        {
            //ElementId rebarHookTypeId = ElementId.InvalidElementId;
            ElementId rebarHookTypeId = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc).Id;
            return rebarHookTypeId;
        }

        private void ObtenerTypoDiamtro(int v)
        {
            throw new NotImplementedException();
        }
    }
}
