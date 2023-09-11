using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArmaduraLosaRevit.Model.ViewportnNH.model
{

    public class ViewportNH
    {
        private UIApplication _uiapp;
        private View viewIter;
        private ViewSheet viewSheet;
        private Document _doc;

        private BoundingBoxUV box;

        public string SheetNumber { get; private set; }
        public string SheetNombre { get; private set; }
        public string TipoEstructura { get; private set; }

        private BoundingBoxXYZ boxXYZ;

        public double Ancho { get; private set; }
        public double Alto { get; private set; }
        public XYZ ptoSupIzq_InSheet_ { get; private set; }

        public ElementId IdPOrt { get; internal set; }
        public XYZ PtoInsercion { get; internal set; }
        public XYZ PuntoSuperiorDerecho { get; internal set; }

        public ViewportNH(UIApplication uiapp, View _viewPOrt, ViewSheet viewSheet)
        {
            this._uiapp = uiapp;
            this.viewIter = _viewPOrt;
            this.viewSheet = viewSheet;
            this._doc = _uiapp.ActiveUIDocument.Document;
            SheetNumber = viewSheet.SheetNumber;
     
            this.IdPOrt = _viewPOrt.Id;
        }


        public bool ObtenerDatos(XYZ ptoSupIzq_Sheet)
        {
            try
            {
                boxXYZ = viewIter.CropBox;
                Ancho = (boxXYZ.Max.X - boxXYZ.Min.X) / viewIter.Scale;
                Alto = (boxXYZ.Max.Y - boxXYZ.Min.Y) / viewIter.Scale;

                ptoSupIzq_InSheet_ = new XYZ(ptoSupIzq_Sheet.X + Ancho / 2, ptoSupIzq_Sheet.Y - Alto / 2, 0);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerDatos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        internal void CalcularPtoInsercion(XYZ puntoSuuperiorDerecho)
        {
            if (puntoSuuperiorDerecho == null)
            {
                PtoInsercion = ptoSupIzq_InSheet_;
                PuntoSuperiorDerecho = PtoInsercion + new XYZ(Ancho, 0, 0);
            }
            else
            {
                PtoInsercion = puntoSuuperiorDerecho;

                PuntoSuperiorDerecho = PtoInsercion + new XYZ(Ancho/2, 0, 0);

            }
        }
    }

}
