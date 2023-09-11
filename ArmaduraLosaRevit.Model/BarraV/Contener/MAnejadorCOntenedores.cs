using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Contener
{
    public class MAnejadorCOntenedores
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public MAnejadorCOntenedores(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        public bool Ejecutar()
        {
            try
            {

                Rebar r1 = _doc.GetElement(new ElementId(3137206)) as Rebar;
                Rebar r2 = _doc.GetElement(new ElementId(3137209)) as Rebar;

                List<Rebar> ListRebar1 = new List<Rebar>();
                ListRebar1.Add(r1); ListRebar1.Add(r2);

                Element muro = _doc.GetElement(new ElementId(1369454));
                ContendorRebar ContendorRebar = new ContendorRebar(_uiapp);

                if (ContendorRebar.CrearContenedor(muro, ListRebar1))
                {

                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
