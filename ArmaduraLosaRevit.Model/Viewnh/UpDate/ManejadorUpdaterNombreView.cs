using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Viewnh.UpDate
{
    public class ManejadorUpdaterNombreView
    {

        private static UpdaterNombreView updateopen;
        private static bool IsCARGADOUpdaterNombreView = false;

        //private UIApplication _uiapp;
        //private Document _doc;
        //private  bool IsCargarUpdaterNombreView = false;
        //public ManejadorUpdaterNombreView(UIApplication _uiapp)
        //{
        //    this._uiapp = _uiapp;
        //    this._doc = _uiapp.ActiveUIDocument.Document;
        //}

        public static void CargarUpdaterNombreView(UIApplication _uiapp)
        {
            var _doc = _uiapp.ActiveUIDocument.Document;
            ConstNH.corte();//  // borrar lo de arriba(estatico) desconemtar el constructor y borrar variable 'IsCargarUpdateREbar'
            if (updateopen == null)
                updateopen = new UpdaterNombreView(_doc,_uiapp.ActiveAddInId);

            if (IsCARGADOUpdaterNombreView) return;

            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

                    #region ELEMENTOS DISPARADORES
                    ElementCategoryFilter filtromuros = new ElementCategoryFilter(BuiltInCategory.OST_Views);
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtromuros, Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.VIEW_NAME)));
                    IsCARGADOUpdaterNombreView = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdaterNombreView' ex:" + ex.Message);
            }
        }


        public static void DesCargarUpdaterNombreView(UIApplication _uiapp)
        {
            var _doc = _uiapp.ActiveUIDocument.Document;

            if (updateopen == null)
                updateopen = new UpdaterNombreView(_doc, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                    IsCARGADOUpdaterNombreView=false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdaterNombreView' ex:" + ex.Message );
            }
        }
    }
}
