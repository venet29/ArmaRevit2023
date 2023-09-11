using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Conector
{
   public  class ManejadorCoupler
    {
        private readonly UIApplication uiapp;
        private Document _doc;

        public ManejadorCoupler(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        public bool CrearCOnSeleccion()
        {

            try
            {

                var rebar1 = _doc.GetElement(new ElementId(3324176)) as Rebar;
                var rebar2 = _doc.GetElement(new ElementId(3324179)) as Rebar;
                List<Rebar> lista = new List<Rebar>();
                lista.Add(rebar1);
                lista.Add(rebar2);

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Draw Polygons");
                    WrapperRebarCoupler _WrapperRebarCoupler = new WrapperRebarCoupler(_doc, lista);
                    _WrapperRebarCoupler.CreateCoupler();
                    t.Commit();
                }

            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }
    }
}
