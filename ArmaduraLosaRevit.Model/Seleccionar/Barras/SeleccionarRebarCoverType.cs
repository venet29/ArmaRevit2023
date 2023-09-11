using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
   public class SeleccionarRebarCoverType
    {
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private FilteredElementCollector _collectorRebarCoverType;

        public List<RebarCoverType> ListaRebarCOverType { get;  set; }

        public SeleccionarRebarCoverType(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;

        }

        

        public bool ObtenerListaRebarCoverType()
        {
            _collectorRebarCoverType = new FilteredElementCollector(_uidoc.Document);
            //para las familias typo           nn
            _collectorRebarCoverType.OfClass(typeof(RebarCoverType));
            _collectorRebarCoverType.OfCategory(BuiltInCategory.OST_CoverType);//.WhereElementIsNotElementType();
            var ListaRebarCOverType2 = _collectorRebarCoverType.ToList();
            ListaRebarCOverType = _collectorRebarCoverType.Cast<RebarCoverType>().ToList();

            return true;
        }

        public bool CambiarRecubrimiento(int recub_mm)
        {

            try
            {
                using (Transaction trans = new Transaction(_uidoc.Document))
                {

                    trans.Start("CambiarRecubrimiento-NH");

                    foreach (RebarCoverType item in ListaRebarCOverType)
                    {
                        if (ParameterUtil.FindParaByName(item, "Length") != null)
                            ParameterUtil.SetParaInt(item, "Length", Util.MmToFoot(recub_mm));  //"nombre de vista"
                      
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Error ex:" + ex.Message);

            }
            return true;

        }
    }
}
