using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using ApiRevit.FILTROS;
using Autodesk.Revit.UI.Selection;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class HelperSeleccinarMuro
    {
        #region 0) propiedes

        UIApplication _uiapp;
        UIDocument _uidoc;
        Application _app;
        Document _doc;


        private Element ElementoSeleccionado { get; set; }



        public XYZ pto1SeleccionadoConMouse { get; set; }


        public Wall MuroSeleccionado { get; set; }
        public List<XYZ> ListaPtoMuroCaraInferior { get; set; }
        public List<XYZ> ListaPtoMuroCaraSuperior { get; set; }

        public List<XYZ> ListaPtosBordeMuroIntersecatdo { get; set; }

        #endregion

        #region 1)contructor

        public HelperSeleccinarMuro(UIApplication uiapp)
        {

            this._uiapp = uiapp;
            this._uidoc = uiapp.ActiveUIDocument;
            this._app = uiapp.Application;
            this._doc = _uidoc.Document;
            ListaPtosBordeMuroIntersecatdo = new List<XYZ>();
            ListaPtoMuroCaraInferior = new List<XYZ>();
            ListaPtoMuroCaraSuperior = new List<XYZ>();
        }
        #endregion

        #region 2) metodos


        //seleccionar elemento

        public bool SoloSeleccionarMuro()
        {
            try
            {


                Selection sel = _uidoc.Selection;

                Reference pickedReference;
                try
                {
                    pickedReference = sel.PickObject(ObjectType.Element, SelFilter.GetElementFilter(typeof(Wall), typeof(Wall)), "Seleccionar muro");
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    return false;
                    //  return MuroSeleccionado;
                }


                if (pickedReference == null) return false;
                ElementoSeleccionado = _doc.GetElement(pickedReference);
                MuroSeleccionado = ElementoSeleccionado as Wall;
                pto1SeleccionadoConMouse = Util.PtoDeLevelDeGlobalPoint(pickedReference.GlobalPoint, _doc);


            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }








        #endregion
    }
}
